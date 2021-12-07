// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ComboEscapeController : IRollbackStateOwner
{
	private PlayerPhysicsController physics;

	private IMoveInput input;

	private ComboEscapeConfig config;

	private IPlayerState playerState;

	private ICombatController combat;

	private ComboEscapeState state;

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public ConfigData configData
	{
		get;
		set;
	}

	public void Init(PlayerPhysicsController physics, IPlayerState playerState, IMoveInput input, ICombatController combat, ComboEscapeConfig config)
	{
		this.physics = physics;
		this.input = input;
		this.config = config;
		this.combat = combat;
		this.playerState = playerState;
		this.state = new ComboEscapeState();
	}

	public void OnHitLagBegin(Fixed comboEscapeMultiplier, Fixed comboEscapeAngleMultiplier, bool isBounce, bool isFlourish)
	{
		this.state.lastInput = this.input.GetAxisValue();
		this.state.lastInput.ClampToUnitCircle();
		this.state.escapeMultiplier = comboEscapeMultiplier;
		this.state.escapeAngleMultiplier = comboEscapeAngleMultiplier;
		this.state.isBounce = isBounce;
		this.state.isFlourish = isFlourish;
	}

	public void TickFrame()
	{
		if (this.state.inputsRead >= this.config.maxEscapeInputs)
		{
			return;
		}
		Vector2F axisValue = this.input.GetAxisValue();
		axisValue.ClampToUnitCircle();
		Fixed one = Vector3F.Angle(this.state.lastInput, axisValue);
		if ((this.state.lastInput.sqrMagnitude == 0 || one >= this.config.minAngleDifference) && axisValue.sqrMagnitude > 0)
		{
			this.escapeTranslate(this.config.escapeDistance * axisValue, this.config.allowLandingDuringHitlag);
			this.state.lastInput = axisValue;
			this.state.inputsRead++;
		}
	}

	public void OnHitLagEnd()
	{
		Vector2F axisValue = this.input.GetAxisValue();
		axisValue.ClampToUnitCircle();
		this.escapeTranslate(this.config.autoEscapeDistance * axisValue, this.config.allowLandingDuringHitlag);
		bool flag = axisValue.sqrMagnitude > 0;
		if (flag)
		{
			Vector3F knockbackVelocity = this.physics.KnockbackVelocity;
			if (knockbackVelocity.sqrMagnitude > 0)
			{
				Vector2F v = new Vector2F(-knockbackVelocity.y, knockbackVelocity.x);
				v.Normalize();
				axisValue.Normalize();
				Fixed one = Vector3F.Dot(axisValue, v);
				Fixed one2 = this.config.maxRotationAngle;
				if (this.config.scaling)
				{
					Fixed magnitude = knockbackVelocity.magnitude;
					if (magnitude <= this.config.scalingFloor)
					{
						one2 = this.config.scalingMax;
					}
					else if (magnitude >= this.config.scalingCeiling)
					{
						one2 = this.config.scalingMin;
					}
					else
					{
						Fixed other = this.config.scalingCeiling - this.config.scalingFloor;
						Fixed other2 = this.config.scalingMax - this.config.scalingMin;
						Fixed one3 = (magnitude - this.config.scalingFloor) / other;
						one2 = this.config.scalingMin + (1 - one3) * other2;
					}
				}
				if (this.state.isBounce)
				{
					one2 = this.configData.spikeConfig.comboEscapeMaxRotationAngle;
				}
				Fixed other3 = one2 * this.state.escapeAngleMultiplier;
				Fixed rotateZDegrees = one * other3;
				Vector3F v2 = MathUtil.RotateVector(knockbackVelocity, rotateZDegrees);
				this.physics.StopMovement(true, true, VelocityType.Knockback);
				this.physics.AddVelocity(v2, 1, VelocityType.Knockback);
			}
		}
		this.state.Clear();
	}

	private void escapeTranslate(Vector3F delta, bool checkFeet)
	{
		if (this.configData.flourishConfig.stopSDI && this.state.isFlourish)
		{
			return;
		}
		delta *= this.state.escapeMultiplier;
		if (this.physics.IsGrounded && Vector3F.Dot(delta, Vector3F.down) > 0)
		{
			return;
		}
		if (this.physics.IsGrounded && this.physics.Velocity.y <= 0)
		{
			delta.y = 0;
		}
		if (this.playerState.IsShieldingState)
		{
			delta.y = 0;
			delta *= this.config.shieldEscapeMultiplier;
		}
		this.physics.ForceTranslate(delta, checkFeet, this.combat.IsMeteorStunned);
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ComboEscapeState>(ref this.state);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ComboEscapeState>(this.state));
		return true;
	}
}
