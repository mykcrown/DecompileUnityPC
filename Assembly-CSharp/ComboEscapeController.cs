using System;
using FixedPoint;

// Token: 0x020003C4 RID: 964
public class ComboEscapeController : IRollbackStateOwner
{
	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06001511 RID: 5393 RVA: 0x00074A2D File Offset: 0x00072E2D
	// (set) Token: 0x06001512 RID: 5394 RVA: 0x00074A35 File Offset: 0x00072E35
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06001513 RID: 5395 RVA: 0x00074A3E File Offset: 0x00072E3E
	// (set) Token: 0x06001514 RID: 5396 RVA: 0x00074A46 File Offset: 0x00072E46
	[Inject]
	public ConfigData configData { get; set; }

	// Token: 0x06001515 RID: 5397 RVA: 0x00074A4F File Offset: 0x00072E4F
	public void Init(PlayerPhysicsController physics, IPlayerState playerState, IMoveInput input, ICombatController combat, ComboEscapeConfig config)
	{
		this.physics = physics;
		this.input = input;
		this.config = config;
		this.combat = combat;
		this.playerState = playerState;
		this.state = new ComboEscapeState();
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x00074A84 File Offset: 0x00072E84
	public void OnHitLagBegin(Fixed comboEscapeMultiplier, Fixed comboEscapeAngleMultiplier, bool isBounce, bool isFlourish)
	{
		this.state.lastInput = this.input.GetAxisValue();
		this.state.lastInput.ClampToUnitCircle();
		this.state.escapeMultiplier = comboEscapeMultiplier;
		this.state.escapeAngleMultiplier = comboEscapeAngleMultiplier;
		this.state.isBounce = isBounce;
		this.state.isFlourish = isFlourish;
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x00074AE8 File Offset: 0x00072EE8
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

	// Token: 0x06001518 RID: 5400 RVA: 0x00074BD4 File Offset: 0x00072FD4
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

	// Token: 0x06001519 RID: 5401 RVA: 0x00074DF8 File Offset: 0x000731F8
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

	// Token: 0x0600151A RID: 5402 RVA: 0x00074EE8 File Offset: 0x000732E8
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ComboEscapeState>(ref this.state);
		return true;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x00074EF8 File Offset: 0x000732F8
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ComboEscapeState>(this.state));
		return true;
	}

	// Token: 0x04000DD6 RID: 3542
	private PlayerPhysicsController physics;

	// Token: 0x04000DD7 RID: 3543
	private IMoveInput input;

	// Token: 0x04000DD8 RID: 3544
	private ComboEscapeConfig config;

	// Token: 0x04000DD9 RID: 3545
	private IPlayerState playerState;

	// Token: 0x04000DDA RID: 3546
	private ICombatController combat;

	// Token: 0x04000DDB RID: 3547
	private ComboEscapeState state;
}
