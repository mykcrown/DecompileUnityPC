// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Linq;
using UnityEngine;

public class PlayerCombatController : ICombatController, IRollbackStateOwner, ITickable
{
	private IPlayerDelegate playerDelegate;

	private ConfigData config;

	private IEvents events;

	private IModeOwner modeOwner;

	private PhysicsSimulator physicsSimulator;

	private GameObject displayObject;

	private ComboEscapeController comboEscape;

	private ActionState[] groundStunActions = new ActionState[]
	{
		ActionState.HitStunGroundS,
		ActionState.HitStunGroundM,
		ActionState.HitStunGroundL
	};

	private ActionState[] airStunActions = new ActionState[]
	{
		ActionState.HitStunAirS,
		ActionState.HitStunAirM,
		ActionState.HitStunAirL
	};

	private ActionState[] meteorStunActions = new ActionState[]
	{
		ActionState.HitStunMeteorS,
		ActionState.HitStunMeteorM,
		ActionState.HitStunMeteorL
	};

	public static int MAX_DAMAGE = 999;

	bool ICombatController.IsAirHitStunned
	{
		get
		{
			return this.playerDelegate.State.IsStunned && this.airStunActions.Contains(this.playerDelegate.State.ActionState);
		}
	}

	bool ICombatController.IsGroundHitStunned
	{
		get
		{
			return this.playerDelegate.State.IsStunned && this.groundStunActions.Contains(this.playerDelegate.State.ActionState);
		}
	}

	bool ICombatController.IsMeteorStunned
	{
		get
		{
			return this.playerDelegate.State.IsStunned && this.meteorStunActions.Contains(this.playerDelegate.State.ActionState);
		}
	}

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	private CameraConfig cameraOptions
	{
		get
		{
			return this.gameController.currentGame.Camera.cameraOptions;
		}
	}

	public int StunFrames
	{
		get
		{
			return this.playerDelegate.Model.stunFrames;
		}
		set
		{
			this.playerDelegate.Model.stunFrames = value;
		}
	}

	public void Setup(IPlayerDelegate player, ConfigData config, IEvents events, IModeOwner modeOwner, PhysicsSimulator simulator, GameObject displayObject)
	{
		this.events = events;
		this.config = config;
		this.playerDelegate = player;
		this.modeOwner = modeOwner;
		this.displayObject = displayObject;
		this.comboEscape = new ComboEscapeController();
		this.injector.Inject(this.comboEscape);
		this.comboEscape.Init(player.Physics, player.State, player.PlayerInput, player.Combat, config.comboEscapeConfig);
		if (this.groundStunActions.Length != this.airStunActions.Length)
		{
			throw new Exception("Air/Ground Mismatch");
		}
	}

	bool IRollbackStateOwner.ExportState(ref RollbackStateContainer container)
	{
		return this.comboEscape.ExportState(ref container);
	}

	bool IRollbackStateOwner.LoadState(RollbackStateContainer container)
	{
		return this.comboEscape.LoadState(container);
	}

	public void TickFrame()
	{
		if (this.playerDelegate.Model.hitLagFrames > 0)
		{
			if (this.playerDelegate.State.IsStunned)
			{
				this.comboEscape.TickFrame();
			}
			this.playerDelegate.Model.hitLagFrames--;
			this.playerDelegate.Model.ignoreHitLagFrames--;
			if (this.playerDelegate.Model.hitLagFrames <= 0)
			{
				this.endHitLag();
			}
		}
		if (this.playerDelegate.Model.chainGrabPreventionFrames > 0)
		{
			this.playerDelegate.Model.chainGrabPreventionFrames--;
		}
		if (this.playerDelegate.Physics.PlayerState.hitVibrate.amplitude > 0f && this.config.knockbackConfig.enableHitVibration)
		{
			this.doHitVibrate();
		}
	}

	private void doHitVibrate()
	{
		PlayerShakeModel hitVibrate = this.playerDelegate.Physics.PlayerState.hitVibrate;
		int frameCount = hitVibrate.frameCount;
		if (frameCount > hitVibrate.framesUntilReduction)
		{
			hitVibrate.amplitude -= Mathf.Max(hitVibrate.startingAmplitude * 0.03f, hitVibrate.amplitude * this.config.knockbackConfig.hitlagVibrationTaperSpeed);
		}
		if (hitVibrate.amplitude <= 0f)
		{
			this.playerDelegate.Model.displayOffset = Vector3F.zero;
			this.displayObject.transform.localPosition = Vector3.zero;
		}
		else
		{
			float wavelength = hitVibrate.wavelength;
			float num = Mathf.Sin((float)frameCount / wavelength * 3.14159274f * 0.5f + 2.3561945f);
			float num2 = Mathf.Cos((float)frameCount / wavelength * 3.14159274f * 0.5f + 2.3561945f);
			float num3 = hitVibrate.amplitude * 0.01f;
			float num4 = (UnityEngine.Random.value - 0.5f) * this.config.knockbackConfig.hitlagVibrationRandomizer * 2f;
			float num5 = (UnityEngine.Random.value - 0.5f) * this.config.knockbackConfig.hitlagVibrationRandomizer * 2f;
			Vector3 vector = default(Vector3);
			vector.x = (1f + num4 * 1f) * num2 * num3 * hitVibrate.xFactor;
			vector.y = (1f + num5 * 1f) * num * num3 * hitVibrate.yFactor;
			hitVibrate.frameCount++;
			this.playerDelegate.Model.displayOffset = (Vector3F)vector;
			this.displayObject.transform.localPosition = Vector3.zero;
			this.displayObject.transform.position = this.displayObject.transform.position + vector;
		}
	}

	private string getHitAnimation(CharacterActionData hitMove, Hit hit)
	{
		return hitMove.name;
	}

	public void BeginHitLag(int frames, IHitOwner owner, HitData hitData, bool isFlourish)
	{
		bool flag = owner != null && hitData.cameraZoom && owner.PlayerNum == this.playerDelegate.PlayerNum;
		this.playerDelegate.Model.hitLagFrames = frames;
		this.playerDelegate.Model.ignoreHitLagFrames = 0;
		this.playerDelegate.Model.isCameraZoomHitLag |= flag;
		this.playerDelegate.Model.isClankLag = false;
		this.playerDelegate.AnimationPlayer.SetPause(true);
		if (this.playerDelegate.State.IsStunned)
		{
			this.comboEscape.OnHitLagBegin(hitData.comboEscapeMulitplier, hitData.comboEscapeAngleMulti, false, isFlourish);
		}
	}

	public void BeginClankLag(int frames, HitData hitData)
	{
		this.BeginHitLag(frames, null, hitData, false);
		this.playerDelegate.Model.isClankLag = true;
	}

	public void BeginHitVibration(int frames, float amplitude, float xFactor, float yFactor)
	{
		PlayerShakeModel hitVibrate = this.playerDelegate.Physics.PlayerState.hitVibrate;
		hitVibrate.framesUntilReduction = frames;
		hitVibrate.frameCount = 0;
		hitVibrate.amplitude = amplitude;
		hitVibrate.startingAmplitude = hitVibrate.amplitude;
		if (this.playerDelegate.State.IsGrounded)
		{
			hitVibrate.xFactor = 1f;
			hitVibrate.yFactor = 0f;
		}
		else
		{
			hitVibrate.xFactor = xFactor;
			hitVibrate.yFactor = yFactor;
		}
	}

	private void endHitLag()
	{
		this.playerDelegate.AnimationPlayer.SetPause(false);
		bool isFlourishOwner = this.playerDelegate.Model.isFlourishOwner;
		this.playerDelegate.Model.isKillCamHitlag = false;
		this.playerDelegate.Model.isFlourishOwner = false;
		this.playerDelegate.Model.isCameraZoomHitLag = false;
		if (isFlourishOwner && this.config.flourishConfig.gravityAssist)
		{
			this.playerDelegate.Physics.PlayerState.gravityAssistFrames = this.config.flourishConfig.gravityAssistFrames;
			this.playerDelegate.Physics.PlayerState.gravityAssistTotalFrames = this.playerDelegate.Physics.PlayerState.gravityAssistFrames;
		}
		if (this.playerDelegate.State.IsStunned)
		{
			this.comboEscape.OnHitLagEnd();
			if (this.modeOwner.IsTrainingMode)
			{
			}
			if (this.playerDelegate.State.MetaState == MetaState.Jump)
			{
				Fixed magnitude = this.playerDelegate.Physics.KnockbackVelocity.magnitude;
				if (magnitude > this.config.soundSettings.flyoutSoundKnockback)
				{
					this.audioManager.PlayGameSound(SoundKey.inGame_flyout, this.playerDelegate as IAudioOwner);
				}
				else if (magnitude > this.config.soundSettings.flyoutSoundKnockbackSmall)
				{
					this.audioManager.PlayGameSound(SoundKey.inGame_flyout_small, this.playerDelegate as IAudioOwner);
				}
			}
		}
		else if (this.playerDelegate.Model.isClankLag)
		{
			this.playerDelegate.Model.isClankLag = false;
			if (this.config.knockbackConfig.clankUseRecoilAnimation)
			{
				this.playerDelegate.StateActor.StartCharacterAction(ActionState.Recoil, null, null, true, 0, false);
			}
			else
			{
				this.playerDelegate.StateActor.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
			}
		}
	}

	void ICombatController.OnShieldHitImpact(HitData hitData, IHitOwner other)
	{
		Fixed @fixed = this.combatCalculator.CalculateModifiedDamage(hitData, other);
		if (!this.playerDelegate.State.IsShieldBroken)
		{
			this.playerDelegate.Shield.OnHit(hitData, other);
			if (hitData.baseKnockback > 0f || hitData.knockbackScaling > 0f)
			{
				Fixed one = @fixed * this.config.knockbackConfig.shieldKnockbackMultiplier + this.config.knockbackConfig.shieldKnockbackAdd;
				int multi = (!(other.Position.x > this.playerDelegate.Position.x)) ? 1 : (-1);
				Vector2F push = new Vector2F(multi * one * this.config.knockbackConfig.knockbackToSpeedConversion, 0);
				this.playerDelegate.Physics.AddVelocity(push, 1, VelocityType.Movement);
			}
		}
		if (this.playerDelegate.Shield.IsBroken)
		{
			this.playerDelegate.StateActor.BreakShield();
		}
		else
		{
			this.BeginStun(this.combatCalculator.CalculateShieldStunFrames(hitData, other), StunType.ShieldStun, true, false, 0, Vector2F.zero, HitContext.Null, null);
		}
		this.BeginHitLag(this.combatCalculator.CalculateHitLagFrames(hitData, other, @fixed, true), other, hitData, false);
	}

	public void ReceiveDamage(Fixed damage)
	{
		this.playerDelegate.DispatchInteraction(PlayerController.InteractionSignalData.Type.TakeDamage);
		this.playerDelegate.Model.damage = FixedMath.Min(PlayerCombatController.MAX_DAMAGE, FixedMath.Max(0, this.playerDelegate.Model.damage + damage));
	}

	ActionState ICombatController.GetGroundStunAction()
	{
		for (int i = 0; i < this.airStunActions.Length; i++)
		{
			if (this.airStunActions[i] == this.playerDelegate.State.ActionState)
			{
				return this.groundStunActions[i];
			}
		}
		return ActionState.None;
	}

	ActionState ICombatController.GetAirStunAction()
	{
		for (int i = 0; i < this.groundStunActions.Length; i++)
		{
			if (this.groundStunActions[i] == this.playerDelegate.State.ActionState)
			{
				return this.airStunActions[i];
			}
		}
		return ActionState.None;
	}

	void ICombatController.OnHitImpact(HitData hitData, IHitOwner other, ImpactType impactType, ref HitContext hitContext)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.KnockbackTuning))
		{
		}
		Vector3F collisionPosition = hitContext.collisionPosition;
		if (this.playerDelegate.Invincibility.IsInvincible)
		{
			this.playerDelegate.Model.ClearLastHitData();
			return;
		}
		Fixed damage = this.playerDelegate.Model.damage;
		Fixed @fixed = this.combatCalculator.CalculateModifiedDamage(hitData, other);
		this.ReceiveDamage(@fixed);
		if (impactType != ImpactType.DamageOnly && !(this.playerDelegate as IHitOwner).ResistsHit(hitData, other, collisionPosition))
		{
			int multi;
			bool hitWasReversed = this.combatCalculator.CheckReverseHit(hitData, other, this.playerDelegate as IHitOwner, out multi);
			Fixed damage2 = this.combatCalculator.CalculateModifiedDamageUnstaled(hitData, other);
			Vector2F vector2F;
			Fixed fixed2 = this.combatCalculator.CalculateKnockback(hitData, other, this.playerDelegate, this.playerDelegate.Model.damage, damage2, collisionPosition, out vector2F);
			vector2F.x *= multi;
			if (this.config.flourishConfig.printDebug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Actual damage ",
					this.playerDelegate.Model.damage,
					" ",
					damage,
					" ",
					@fixed
				}));
				UnityEngine.Debug.Log("Actual knockback " + vector2F);
			}
			if (hitContext.useKillFlourish)
			{
				Fixed fixed3 = 1 + this.config.flourishConfig.increaseKnockback / 100;
				vector2F *= fixed3;
				fixed2 *= fixed3;
			}
			if (this.playerDelegate.State.IsGrounded && (FixedMath.Abs(vector2F.y) <= this.config.knockbackConfig.knockbackLiftOffThreshold || !hitData.knockbackCausesFlinching))
			{
				Vector2F b = this.playerDelegate.Physics.GroundedNormal * Vector2F.Dot(this.playerDelegate.Physics.GroundedNormal, vector2F);
				vector2F -= b;
			}
			bool flag = true;
			if (vector2F.sqrMagnitude == 0 || !hitData.knockbackCausesFlinching)
			{
				flag = false;
			}
			if (hitData.preventHelplessness)
			{
				if (this.playerDelegate.State.IsHelpless)
				{
					this.playerDelegate.StateActor.BeginFalling(ActionState.FallStraight, false);
					this.playerDelegate.Physics.ResetAirJump();
				}
				else if (this.playerDelegate.ActiveMove.IsActive)
				{
					this.playerDelegate.Model.ignoreNextHelplessness = true;
				}
			}
			else
			{
				this.playerDelegate.Model.ignoreNextHelplessness = false;
			}
			bool flag2 = flag && hitData.forcesGetUp && this.playerDelegate.State.IsDownState;
			if (vector2F.x != 0 && @fixed != 0 && !flag2)
			{
				HorizontalDirection facingAndRotation = other.CalculateVictimFacing(hitWasReversed);
				this.playerDelegate.SetFacingAndRotation(facingAndRotation);
			}
			if (flag)
			{
				if (this.playerDelegate.ActiveMove.IsActive && this.playerDelegate.ActiveMove.Data.IsTauntMove())
				{
					this.events.Broadcast(new LogStatEvent(StatType.TauntHit, 1, PointsValueType.Addition, this.playerDelegate.PlayerNum, this.playerDelegate.Team));
				}
				this.playerDelegate.LedgeGrabController.ReleaseGrabbedLedge(true, true);
				this.playerDelegate.OnFlinched();
				this.playerDelegate.TrailEmitter.ResetData();
				this.playerDelegate.KnockbackEmitter.ResetData();
				if (hitData.applyTrailOnHit)
				{
					this.playerDelegate.TrailEmitter.LoadEmitterData(hitData.trailData);
				}
				this.playerDelegate.Model.landingOverrideData = null;
				this.playerDelegate.Model.helplessStateData = null;
				if (this.playerDelegate.State.IsShieldBroken)
				{
					this.playerDelegate.StateActor.ReleaseShieldBreak();
				}
				int num = 0;
				bool isSpike = false;
				if (flag2)
				{
					fixed2 = 0;
					vector2F = Vector2F.zero;
					if (this.config.knockbackConfig.forcedGetUpHitstunFrames > 0)
					{
						StunType stunType = StunType.ForceGetUpHitStun;
						this.BeginStun(this.config.knockbackConfig.forcedGetUpHitstunFrames, stunType, hitData.cannotTech, isSpike, fixed2, vector2F, hitContext, hitData);
					}
					else
					{
						this.playerDelegate.ForceGetUp();
					}
				}
				else
				{
					StunType stunType = StunType.HitStun;
					num = this.combatCalculator.CalculateHitStunFrames(hitData, fixed2);
					int knockbackIteration = this.combatCalculator.CalculateKnockbackIterator(fixed2);
					int smokeTrailFrames = this.combatCalculator.CalculateSmokeTrailFrames(hitData, fixed2);
					isSpike = this.config.spikeConfig.isSpike((int)hitData.knockbackAngle);
					this.BeginStun(num, stunType, hitData.cannotTech, isSpike, fixed2, vector2F, hitContext, hitData);
					this.playerDelegate.Model.knockbackIteration = knockbackIteration;
					this.playerDelegate.Model.smokeTrailFrames = smokeTrailFrames;
					this.playerDelegate.Model.floatyNameStun = (this.config.uiuxSettings.floatyNameOnTumble != 0 && vector2F.magnitude >= this.config.uiuxSettings.floatyNameOnTumble);
					if (!this.playerDelegate.State.IsGrounded || vector2F.y > 0)
					{
						this.playerDelegate.State.MetaState = MetaState.Jump;
					}
				}
				this.playerDelegate.EndActiveMove(MoveEndType.Cancelled, true, false);
				this.playerDelegate.Shield.OnShieldReleased();
				if (this.config.spikeConfig.isSpike(hitData))
				{
					this.playerDelegate.Model.jumpStunFrames = this.config.spikeConfig.getEscapeFrames(hitData);
				}
				else
				{
					this.playerDelegate.Model.jumpStunFrames = this.StunFrames;
				}
				int num2 = this.startHitLag(other, hitData, @fixed, vector2F, ref hitContext);
				this.cameraMods(hitData, other, num2, num, vector2F, (float)@fixed);
				this.impactEmissions(hitData, num2);
				this.playerDelegate.Model.ledgeGrabsSinceLanding = 0;
				this.playerDelegate.Physics.PlayerState.platformDropFrames = 0;
				if (hitData.hitType == HitType.Throw && hitData.releaseGrabbedOpponent)
				{
					this.playerDelegate.Model.chainGrabPreventionFrames = Mathf.Max(this.config.grabConfig.chainGrabPreventionFrames + num2 + num, this.playerDelegate.Model.chainGrabPreventionFrames);
					if (this.config.grabConfig.useRegrabDelay && !hitData.ignoreRegrabLimit)
					{
						this.playerDelegate.Invincibility.BeginGrabIntangibility(Mathf.Max(new int[]
						{
							this.config.grabConfig.regrabDelayFrames + num2
						}));
					}
				}
				this.playerDelegate.State.SubState = SubStates.Resting;
			}
			else if (this.playerDelegate.State.IsGrabbedState && !this.playerDelegate.State.IsThrown && other.PlayerNum == this.playerDelegate.GrabController.GrabbingOpponent)
			{
				this.playerDelegate.StateActor.StartCharacterAction(ActionState.GrabbedPummelled, null, null, true, 0, false);
				this.startHitLag(other, hitData, @fixed, vector2F, ref hitContext);
			}
			this.playerDelegate.Physics.StopMovement(hitData.resetXVelocity, hitData.resetYVelocity, VelocityType.Total);
			this.playerDelegate.Physics.ClearFastFall();
			if (vector2F != Vector2F.zero)
			{
				this.playerDelegate.Physics.AddVelocity(vector2F, 1, VelocityType.Knockback);
			}
			if (!this.modeOwner.IsTrainingMode || !(fixed2 != 0) || this.StunFrames > 0)
			{
			}
		}
		this.signalBus.GetSignal<PlayerHitConfirmSignal>().Dispatch(hitData, this.playerDelegate, other);
	}

	private int startHitLag(IHitOwner owner, HitData hitData, Fixed damageDealt, Vector2F knockbackVelocity, ref HitContext hitContext)
	{
		bool useKillFlourish = hitContext.useKillFlourish;
		int num;
		if (useKillFlourish)
		{
			num = this.config.flourishConfig.hitLagFrames;
		}
		else
		{
			num = this.combatCalculator.CalculateHitLagFrames(hitData, owner, damageDealt, false);
		}
		this.BeginHitLag(num, owner, hitData, hitContext.useKillFlourish);
		float amplitude = this.combatCalculator.CalculateHitVibration(hitData, damageDealt, false);
		float num2 = (float)MathUtil.VectorToAngle(ref knockbackVelocity);
		float xFactor = Mathf.Cos(num2 * 0.0174532924f);
		float yFactor = Mathf.Sin(num2 * 0.0174532924f);
		if (!useKillFlourish || !this.config.flourishConfig.disableVibrate)
		{
			this.BeginHitVibration(num, amplitude, xFactor, yFactor);
		}
		if (useKillFlourish)
		{
			this.playerDelegate.Model.isKillCamHitlag = true;
		}
		return num;
	}

	private void startSpikeBounceHitLag(CollisionData collision)
	{
		int num = this.combatCalculator.CalculateHitLagForSpikeBounce(this.playerDelegate.Physics.KnockbackVelocity.magnitude);
		if (num > 0)
		{
			this.playerDelegate.Model.hitLagFrames = num;
			this.playerDelegate.AnimationPlayer.SetPause(true);
			if (this.config.spikeConfig.useGroundbounceComboEscape)
			{
				this.comboEscape.OnHitLagBegin(1, 1, true, false);
			}
			float num2 = (float)MathUtil.VectorToAngle(ref collision.normal);
			float xFactor = Mathf.Cos(num2 * 0.0174532924f);
			float yFactor = Mathf.Sin(num2 * 0.0174532924f);
			this.BeginHitVibration(num, this.config.knockbackConfig.hitlagVibration, xFactor, yFactor);
		}
	}

	private float getDamageBasedCameraShake(float damage)
	{
		if (damage <= (float)this.cameraOptions.shakeData.hitShakeDamageMin || damage <= (float)this.cameraOptions.shakeData.hitShakeDamageThreshold)
		{
			return 0f;
		}
		float num = Mathf.Clamp(damage, (float)this.cameraOptions.shakeData.hitShakeDamageMin, (float)this.cameraOptions.shakeData.hitShakeDamageMax);
		float num2 = (float)(this.cameraOptions.shakeData.hitShakeDamageMax - this.cameraOptions.shakeData.hitShakeDamageMin);
		float num3 = num - (float)this.cameraOptions.shakeData.hitShakeDamageMin;
		return num3 / num2;
	}

	private float getKnockbackBasedCameraShake(Vector2F knockbackVelocity)
	{
		float num = (float)knockbackVelocity.magnitude;
		if (this.cameraOptions.shakeData.hitShakeKnockbackMin < 0)
		{
			return num / 55f;
		}
		if (num <= (float)this.cameraOptions.shakeData.hitShakeKnockbackMin || num <= (float)this.cameraOptions.shakeData.hitShakeKnockbackThreshold)
		{
			return 0f;
		}
		num = Mathf.Clamp(num, (float)this.cameraOptions.shakeData.hitShakeKnockbackMin, (float)this.cameraOptions.shakeData.hitShakeKnockbackMax);
		float num2 = (float)(this.cameraOptions.shakeData.hitShakeKnockbackMax - this.cameraOptions.shakeData.hitShakeKnockbackMin);
		float num3 = num - (float)this.cameraOptions.shakeData.hitShakeKnockbackMin;
		return num3 / num2;
	}

	private void impactEmissions(HitData hitData, int hitLagFrames)
	{
		if (hitData.impactEmissionFrames > 0)
		{
			this.playerDelegate.Renderer.OverrideColor(hitData.impactEmissionFrames, hitData.impactEmission);
		}
		else if (this.config.characterColorConfig.useImpactEmission)
		{
			int num = hitLagFrames;
			if (num > this.config.characterColorConfig.impactEmissionMaxFrames)
			{
				num = this.config.characterColorConfig.impactEmissionMaxFrames;
			}
			this.playerDelegate.Renderer.OverrideColor(num, this.config.characterColorConfig.impactEmission);
		}
	}

	private void cameraMods(HitData hitData, IHitOwner other, int hitLagFrames, int stunFrames, Vector2F knockbackVelocity, float damage)
	{
		if (knockbackVelocity.x != 0)
		{
			int direction = (int)(knockbackVelocity.x / FixedMath.Abs(knockbackVelocity.x));
			this.gameController.currentGame.Camera.StartImpact(new CameraImpactModeRequest(Mathf.Abs((float)knockbackVelocity.x), direction, hitLagFrames, stunFrames));
		}
		if (hitData.overrideCameraShake)
		{
			this.gameController.currentGame.Camera.ShakeCamera(new CameraShakeRequest(hitData.cameraShake));
		}
		else
		{
			float num;
			if (this.cameraOptions.shakeData.hitShakeMethod == HitCameraShakeMethod.KNOCKBACK)
			{
				num = this.getKnockbackBasedCameraShake(knockbackVelocity);
			}
			else if (this.cameraOptions.shakeData.hitShakeMethod == HitCameraShakeMethod.DAMAGE)
			{
				num = this.getDamageBasedCameraShake(damage);
			}
			else if (this.cameraOptions.shakeData.hitShakeMethod == HitCameraShakeMethod.DAMAGE_OR_KNOCKBACK)
			{
				float damageBasedCameraShake = this.getDamageBasedCameraShake(damage);
				float knockbackBasedCameraShake = this.getKnockbackBasedCameraShake(knockbackVelocity);
				num = Mathf.Max(damageBasedCameraShake, knockbackBasedCameraShake);
			}
			else
			{
				num = (float)hitLagFrames / 22f;
			}
			if (num != 0f)
			{
				if (hitData != null)
				{
					num *= hitData.cameraShakeMulti;
				}
				CameraShakeData hitShake = this.cameraOptions.shakeData.hitShake;
				bool mirror = false;
				float angle;
				if (hitData.useCameraShakeAngleOverride)
				{
					angle = hitData.overrideCameraShakeAngle;
					mirror = (other.Facing == HorizontalDirection.Left);
				}
				else
				{
					angle = (float)MathUtil.VectorToAngle(ref knockbackVelocity);
				}
				CameraShakeRequest request = new CameraShakeRequest(hitShake);
				request.framesUntilReduction = (float)hitLagFrames;
				request.useMulti(num);
				request.useAngle(angle, mirror);
				this.gameController.currentGame.Camera.ShakeCamera(request);
			}
		}
	}

	public void BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F knockbackVelocity, HitContext hitContext, HitData hitData)
	{
		int hitAngle = (int)MathUtil.VectorToAngle(ref knockbackVelocity);
		int num = frames;
		if (isSpike)
		{
			num = (int)(num * this.config.spikeConfig.groundedSpikeHitStunMulti);
		}
		if (num > 0)
		{
			this.StunFrames = num;
			this.playerDelegate.Model.clearInputBufferOnStunEnd = false;
			this.playerDelegate.Model.stunType = stunType;
			this.playerDelegate.Model.stunTechMode = ((!cannotTech) ? StunTechMode.Techable : StunTechMode.Untechable);
			this.playerDelegate.Physics.WasHit = true;
			this.playerDelegate.Model.dashDanceFrames = 0;
			this.playerDelegate.Model.ledgeGrabCooldownFrames = 0;
			this.playerDelegate.Physics.ResetDelayedMovement();
			if (stunType == StunType.ForceGetUpHitStun)
			{
				this.playerDelegate.StateActor.StartCharacterAction(ActionState.HitStunForcedGetUp, null, null, true, 0, false);
			}
			else if (stunType == StunType.HitStun)
			{
				ActionState actionState = ActionState.None;
				if (hitData != null && hitData.overrideHitActionState)
				{
					actionState = hitData.hitActionState;
				}
				else if (knockbackForce >= this.config.knockbackConfig.tumbleKnockbackThreshold)
				{
					if (knockbackForce >= this.config.knockbackConfig.spinKnockbackThreshold)
					{
						actionState = ActionState.HitTumbleSpin;
					}
					else if (this.isUpwardTumble(hitAngle))
					{
						actionState = ActionState.HitTumbleTop;
					}
					else
					{
						HurtBoxTumbleAnimation hurtBoxTumbleAnimation = HurtBoxTumbleAnimation.DEFAULT;
						HurtBoxState hurtBoxState = hitContext.hurtBoxState as HurtBoxState;
						if (hurtBoxState != null)
						{
							hurtBoxTumbleAnimation = hurtBoxState.hurtBox.tumbleAnim;
						}
						switch (hurtBoxTumbleAnimation)
						{
						case HurtBoxTumbleAnimation.HIGH:
							actionState = ActionState.HitTumbleHigh;
							goto IL_1B7;
						case HurtBoxTumbleAnimation.LOW:
							actionState = ActionState.HitTumbleLow;
							goto IL_1B7;
						}
						actionState = ActionState.HitTumbleNeutral;
					}
					IL_1B7:
					this.playerDelegate.Model.stunIsSpike = isSpike;
					if (this.config.spikeConfig.firstSpikeIsUntechable && isSpike && !this.playerDelegate.State.IsGrounded && this.playerDelegate.Model.stunTechMode == StunTechMode.Techable)
					{
						this.playerDelegate.Model.stunTechMode = StunTechMode.FirstBounceUntechable;
					}
					this.playerDelegate.GameInput.Vibrate(1f, 1f, 0.2f);
				}
				else
				{
					ActionState[] array;
					if (this.playerDelegate.State.IsGrounded && knockbackVelocity.y <= 0)
					{
						if (isSpike)
						{
							array = this.meteorStunActions;
						}
						else
						{
							array = this.groundStunActions;
						}
					}
					else
					{
						array = this.airStunActions;
					}
					for (int i = 0; i < array.Length; i++)
					{
						ActionState actionState2 = array[i];
						if (frames <= this.calculateActionFrames(actionState2) || i == array.Length - 1)
						{
							actionState = actionState2;
							break;
						}
					}
				}
				if (actionState != ActionState.None)
				{
					this.playerDelegate.StateActor.StartCharacterAction(actionState, null, null, true, 0, false);
					if (actionState == ActionState.HitTumbleSpin && !this.playerDelegate.State.IsHitLagPaused)
					{
						this.playerDelegate.Orientation.OrientToTumbleSpin(knockbackVelocity);
					}
				}
			}
		}
	}

	private bool isUpwardTumble(int hitAngle)
	{
		if (hitAngle >= this.config.knockbackConfig.minTumbleUpAngle && hitAngle <= this.config.knockbackConfig.maxTumbleUpAngle)
		{
			return true;
		}
		if (this.playerDelegate.State.IsGrounded)
		{
			int num = -hitAngle;
			if (num >= this.config.knockbackConfig.minTumbleUpAngle && num <= this.config.knockbackConfig.maxTumbleUpAngle)
			{
				return true;
			}
		}
		return false;
	}

	public void OnKnockbackBounce(CollisionData collision)
	{
		float f = Vector2.Dot(Vector2.up, (Vector2)collision.normal);
		float num = (!(collision.normal.x < 0)) ? (6.28318548f - Mathf.Acos(f)) : Mathf.Acos(f);
		Quaternion overrideRotation = Quaternion.AngleAxis(num * 57.29578f, Vector3.forward);
		Fixed other = (collision.CollisionSurfaceType != SurfaceType.Floor) ? this.config.knockbackConfig.bounceNongroundHitstunReduction : this.config.knockbackConfig.bounceGroundHitstunReduction;
		int frames = (int)(this.StunFrames * other);
		this.playerDelegate.ReduceStunFrames(frames);
		bool flag = collision.CollisionSurfaceType == SurfaceType.Floor;
		if (this.playerDelegate.Model.stunIsSpike && flag)
		{
			this.startSpikeBounceHitLag(collision);
		}
		ParticleData particle;
		if (this.playerDelegate.State.IsTechableMode || !flag)
		{
			particle = this.config.defaultCharacterEffects.knockDown;
		}
		else
		{
			this.playerDelegate.Model.untechableBounceUsed = true;
			this.playerDelegate.LastTechFrame = 0;
			particle = this.config.defaultCharacterEffects.untechableKnockdown;
		}
		this.playerDelegate.GameVFX.PlayDelayedParticle(new ParticlePlayData
		{
			particle = particle,
			shouldOverrideRotation = true,
			overrideRotation = overrideRotation,
			shouldOverridePosition = true,
			overridePosition = (Vector2)collision.point
		});
	}

	private int calculateActionFrames(ActionState action)
	{
		return this.playerDelegate.AnimationPlayer.GetActionGameFramelength(this.playerDelegate.MoveSet.Actions.GetAction(action, false));
	}
}
