using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class PlayerState : IPlayerState
{
	// Token: 0x06002429 RID: 9257 RVA: 0x000B601D File Offset: 0x000B441D
	public PlayerState(IPlayerStateActor actor, IPlayerDataOwner data, ConfigData config, IBattleServerAPI battleServerAPI, IFrameOwner frameOwner)
	{
		this.actor = actor;
		this.data = data;
		this.config = config;
		this.frameOwner = frameOwner;
		this.battleServerAPI = battleServerAPI;
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x0600242A RID: 9258 RVA: 0x000B604A File Offset: 0x000B444A
	// (set) Token: 0x0600242B RID: 9259 RVA: 0x000B605C File Offset: 0x000B445C
	public ActionState ActionState
	{
		get
		{
			return this.data.Model.actionState;
		}
		set
		{
			this.data.Model.actionState = value;
		}
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x0600242C RID: 9260 RVA: 0x000B606F File Offset: 0x000B446F
	// (set) Token: 0x0600242D RID: 9261 RVA: 0x000B6081 File Offset: 0x000B4481
	public MetaState MetaState
	{
		get
		{
			return this.data.Model.state;
		}
		set
		{
			this.setState(value);
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x0600242E RID: 9262 RVA: 0x000B608A File Offset: 0x000B448A
	// (set) Token: 0x0600242F RID: 9263 RVA: 0x000B609C File Offset: 0x000B449C
	public SubStates SubState
	{
		get
		{
			return this.data.Model.subState;
		}
		set
		{
			this.setSubState(value);
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06002430 RID: 9264 RVA: 0x000B60A5 File Offset: 0x000B44A5
	public int ActionStateFrame
	{
		get
		{
			return this.data.Model.actionStateFrame;
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x06002431 RID: 9265 RVA: 0x000B60B7 File Offset: 0x000B44B7
	public bool IsShieldingState
	{
		get
		{
			return this.MetaState == MetaState.Shielding;
		}
	}

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06002432 RID: 9266 RVA: 0x000B60C2 File Offset: 0x000B44C2
	public bool IsStandingState
	{
		get
		{
			return this.MetaState == MetaState.Stand;
		}
	}

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06002433 RID: 9267 RVA: 0x000B60CD File Offset: 0x000B44CD
	public bool IsJumpingState
	{
		get
		{
			return this.MetaState == MetaState.Jump;
		}
	}

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x06002434 RID: 9268 RVA: 0x000B60D8 File Offset: 0x000B44D8
	public bool IsGrabbedState
	{
		get
		{
			return this.MetaState == MetaState.Grabbed;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x06002435 RID: 9269 RVA: 0x000B60E3 File Offset: 0x000B44E3
	public bool IsStandardGrabbingState
	{
		get
		{
			return this.MetaState == MetaState.StandardGrabbing;
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x06002436 RID: 9270 RVA: 0x000B60EE File Offset: 0x000B44EE
	public bool IsLedgeHangingState
	{
		get
		{
			return this.MetaState == MetaState.LedgeHang;
		}
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x06002437 RID: 9271 RVA: 0x000B60F9 File Offset: 0x000B44F9
	public bool IsDownState
	{
		get
		{
			return this.MetaState == MetaState.Down;
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x06002438 RID: 9272 RVA: 0x000B6104 File Offset: 0x000B4504
	public bool IsCrouching
	{
		get
		{
			return this.ActionState == ActionState.Crouching;
		}
	}

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x06002439 RID: 9273 RVA: 0x000B6110 File Offset: 0x000B4510
	public bool IsBeginningCrouching
	{
		get
		{
			return this.ActionState == ActionState.CrouchBegin;
		}
	}

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x0600243A RID: 9274 RVA: 0x000B611C File Offset: 0x000B451C
	public bool IsEndCrouching
	{
		get
		{
			return this.ActionState == ActionState.CrouchEnd;
		}
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x0600243B RID: 9275 RVA: 0x000B6128 File Offset: 0x000B4528
	public bool IsBraking
	{
		get
		{
			return this.ActionState == ActionState.Brake;
		}
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x0600243C RID: 9276 RVA: 0x000B6134 File Offset: 0x000B4534
	public bool IsDashBraking
	{
		get
		{
			return this.ActionState == ActionState.DashBrake;
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x0600243D RID: 9277 RVA: 0x000B6140 File Offset: 0x000B4540
	public bool IsRunPivotBraking
	{
		get
		{
			return this.ActionState == ActionState.RunPivotBrake;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x0600243E RID: 9278 RVA: 0x000B614C File Offset: 0x000B454C
	public bool IsJumpingUp
	{
		get
		{
			return this.ActionState == ActionState.JumpStraight;
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x0600243F RID: 9279 RVA: 0x000B6157 File Offset: 0x000B4557
	public bool IsAirJumping
	{
		get
		{
			return this.ActionState == ActionState.AirJump;
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x06002440 RID: 9280 RVA: 0x000B6163 File Offset: 0x000B4563
	public bool IsFalling
	{
		get
		{
			return this.ActionState == ActionState.FallStraight || this.ActionState == ActionState.FallForward || this.ActionState == ActionState.FallBack;
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06002441 RID: 9281 RVA: 0x000B618A File Offset: 0x000B458A
	public bool IsRunPivoting
	{
		get
		{
			return this.ActionState == ActionState.RunPivot;
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x06002442 RID: 9282 RVA: 0x000B6196 File Offset: 0x000B4596
	public bool IsPivoting
	{
		get
		{
			return this.ActionState == ActionState.Pivot;
		}
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06002443 RID: 9283 RVA: 0x000B61A2 File Offset: 0x000B45A2
	public bool IsLanding
	{
		get
		{
			return this.ActionState == ActionState.Landing;
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06002444 RID: 9284 RVA: 0x000B61AE File Offset: 0x000B45AE
	public bool IsTakingOff
	{
		get
		{
			return this.ActionState == ActionState.TakeOff;
		}
	}

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06002445 RID: 9285 RVA: 0x000B61B9 File Offset: 0x000B45B9
	public bool IsRecoiling
	{
		get
		{
			return this.ActionState == ActionState.Recoil;
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06002446 RID: 9286 RVA: 0x000B61C5 File Offset: 0x000B45C5
	public bool IsRunning
	{
		get
		{
			return this.ActionState == ActionState.Run;
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06002447 RID: 9287 RVA: 0x000B61D1 File Offset: 0x000B45D1
	public bool IsDashing
	{
		get
		{
			return this.ActionState == ActionState.Dash;
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06002448 RID: 9288 RVA: 0x000B61DD File Offset: 0x000B45DD
	public bool IsDashPivoting
	{
		get
		{
			return this.ActionState == ActionState.DashPivot;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06002449 RID: 9289 RVA: 0x000B61E9 File Offset: 0x000B45E9
	public bool IsIdling
	{
		get
		{
			return this.ActionState == ActionState.Idle;
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x0600244A RID: 9290 RVA: 0x000B61F4 File Offset: 0x000B45F4
	public bool IsWalking
	{
		get
		{
			return this.ActionState == ActionState.WalkFast || this.ActionState == ActionState.WalkSlow || this.ActionState == ActionState.WalkMedium;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x0600244B RID: 9291 RVA: 0x000B621C File Offset: 0x000B461C
	public bool IsReleasingShield
	{
		get
		{
			return this.ActionState == ActionState.ShieldEnd;
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x0600244C RID: 9292 RVA: 0x000B6228 File Offset: 0x000B4628
	public bool IsLedgeGrabbing
	{
		get
		{
			return this.ActionState == ActionState.EdgeGrab;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x0600244D RID: 9293 RVA: 0x000B6234 File Offset: 0x000B4634
	public bool IsLedgeHanging
	{
		get
		{
			return this.ActionState == ActionState.EdgeHang;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x0600244E RID: 9294 RVA: 0x000B6240 File Offset: 0x000B4640
	public bool IsTeetering
	{
		get
		{
			return this.ActionState == ActionState.TeeterLoop || this.ActionState == ActionState.TeeterBegin;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x0600244F RID: 9295 RVA: 0x000B625C File Offset: 0x000B465C
	public bool IsGrabbing
	{
		get
		{
			return this.ActionState == ActionState.Grabbing;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x06002450 RID: 9296 RVA: 0x000B6268 File Offset: 0x000B4668
	public bool IsGrabReleasing
	{
		get
		{
			return this.ActionState == ActionState.GrabRelease;
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x06002451 RID: 9297 RVA: 0x000B6274 File Offset: 0x000B4674
	public bool IsGrabEscaping
	{
		get
		{
			return this.ActionState == ActionState.GrabEscapeGround || this.ActionState == ActionState.GrabEscapeAir;
		}
	}

	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x06002452 RID: 9298 RVA: 0x000B6290 File Offset: 0x000B4690
	public bool IsDazed
	{
		get
		{
			return this.ActionState == ActionState.DazedBegin || this.ActionState == ActionState.DazedLoop || this.ActionState == ActionState.DazedEnd;
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x06002453 RID: 9299 RVA: 0x000B62B9 File Offset: 0x000B46B9
	public bool IsDownedLooping
	{
		get
		{
			return this.ActionState == ActionState.DownedLoop;
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x06002454 RID: 9300 RVA: 0x000B62C5 File Offset: 0x000B46C5
	public bool IsThrown
	{
		get
		{
			return this.ActionState == ActionState.Thrown;
		}
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x06002455 RID: 9301 RVA: 0x000B62D4 File Offset: 0x000B46D4
	public bool IsTumbling
	{
		get
		{
			ActionState actionState = this.ActionState;
			switch (actionState)
			{
			case ActionState.HitTumbleHigh:
			case ActionState.HitTumbleLow:
			case ActionState.HitTumbleNeutral:
			case ActionState.HitTumbleSpin:
			case ActionState.HitTumbleTop:
				break;
			default:
				if (actionState != ActionState.Tumble)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x06002456 RID: 9302 RVA: 0x000B6315 File Offset: 0x000B4715
	public bool IsBeginningShield
	{
		get
		{
			return this.ActionState == ActionState.ShieldBegin;
		}
	}

	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x06002457 RID: 9303 RVA: 0x000B6321 File Offset: 0x000B4721
	public bool IsPlatformDropping
	{
		get
		{
			return this.data.Physics.PlayerState.platformDropFrames > 0;
		}
	}

	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x06002458 RID: 9304 RVA: 0x000B633C File Offset: 0x000B473C
	public bool IsHitStunned
	{
		get
		{
			switch (this.ActionState)
			{
			case ActionState.HitStunAirS:
			case ActionState.HitStunAirM:
			case ActionState.HitStunAirL:
			case ActionState.HitStunGroundS:
			case ActionState.HitStunGroundM:
			case ActionState.HitStunGroundL:
			case ActionState.HitStunMeteorS:
			case ActionState.HitStunMeteorM:
			case ActionState.HitStunMeteorL:
				return true;
			default:
				return false;
			}
		}
	}

	// Token: 0x1700085C RID: 2140
	// (get) Token: 0x06002459 RID: 9305 RVA: 0x000B6388 File Offset: 0x000B4788
	bool IPlayerState.CanMove
	{
		get
		{
			return !this.IsDownState && !this.IsLedgeHangingState && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsGrabbedState && !this.IsStandardGrabbingState && !this.IsShieldingState && !this.IsHitLagPaused && !this.IsStunned && !this.IsDead && !this.IsDashPivoting && (!this.IsBusyWithMove || !this.isBlockMovement() || (this.data.ActiveMove.Data.allowReversal && this.data.ActiveMove.Model.internalFrame <= this.data.ActiveMove.Data.reversalFrames)) && !this.IsDownState && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsReleasingShield && !this.IsBusyRespawning;
		}
	}

	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x0600245A RID: 9306 RVA: 0x000B64A1 File Offset: 0x000B48A1
	bool IPlayerState.IsBlockMovement
	{
		get
		{
			return this.isBlockMovement();
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x0600245B RID: 9307 RVA: 0x000B64AC File Offset: 0x000B48AC
	bool IPlayerState.IsBlockFastFall
	{
		get
		{
			if (this.isBlockMovement())
			{
				return true;
			}
			foreach (BlockMovementData blockMovementData2 in this.data.ActiveMove.Data.blockMovementData)
			{
				if (this.data.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.data.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame && blockMovementData2.blockFastFall)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000B6544 File Offset: 0x000B4944
	private bool isBlockMovement()
	{
		if (this.data.ActiveMove.Data.blockMovement)
		{
			return true;
		}
		foreach (BlockMovementData blockMovementData2 in this.data.ActiveMove.Data.blockMovementData)
		{
			if (this.data.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.data.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame && blockMovementData2.blockAllMovement)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x0600245D RID: 9309 RVA: 0x000B65EC File Offset: 0x000B49EC
	public bool CanUseMoves
	{
		get
		{
			return !this.IsHelpless && !this.IsStunned && !this.IsLedgeGrabbing && !this.IsDead && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsBusyRespawning && !this.IsReadyToGrabRelease;
		}
	}

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x0600245E RID: 9310 RVA: 0x000B6650 File Offset: 0x000B4A50
	public bool CanUseEmotes
	{
		get
		{
			return this.data.Model.emoteCooldownFrames <= 0 && (!this.config.tauntSettings.useEmotesPerTime || !this.battleServerAPI.IsSinglePlayerNetworkGame || this.data.Model.emoteFrameLimitStart == 0 || this.frameOwner.Frame - this.data.Model.emoteFrameLimitStart > this.config.tauntSettings.emotesPerTimeFrames || this.data.Model.emoteFrameLimitCounter < this.config.tauntSettings.emotesPerTimeMax);
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x0600245F RID: 9311 RVA: 0x000B6708 File Offset: 0x000B4B08
	bool IPlayerState.CanGrabLedge
	{
		get
		{
			return !this.IsGrounded && !this.IsStunned && ((this.data.GameInput.VerticalAxisValue >= 0 && !this.IsBusyWithMove) || this.IsHelpless || (this.IsBusyWithMove && this.data.ActiveMove.IsActive && this.data.ActiveMove.IsLedgeGrabEnabled)) && !this.data.LedgeGrabController.IsLedgeGrabbing && this.frameOwner.Frame - this.data.Model.ledgeReleaseFrame > this.config.ledgeConfig.ledgeCooldownFrames;
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x06002460 RID: 9312 RVA: 0x000B67D8 File Offset: 0x000B4BD8
	bool IPlayerState.CanDropThroughPlatform
	{
		get
		{
			return (!this.IsBusyWithMove && (!this.IsLanding || !this.IsBusyWithAction) && !this.IsShieldingState && !this.IsReleasingShield && !this.IsTakingOff && !this.IsJumpingState && !this.IsDownState && !this.IsDownedLooping) || this.IsShieldingState;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x06002461 RID: 9313 RVA: 0x000B6854 File Offset: 0x000B4C54
	bool IPlayerState.ShouldUseRecoveryJump
	{
		get
		{
			return this.data.Physics.UsedAirJump && this.data.Physics.Velocity.y <= 0 && !this.data.Physics.IsAboveStage;
		}
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x06002462 RID: 9314 RVA: 0x000B68B0 File Offset: 0x000B4CB0
	public bool CanJump
	{
		get
		{
			return !this.IsHitLagPaused && (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Jump)) && !this.IsHelpless && !this.IsDownState && !this.IsShieldBroken && !this.IsLedgeHangingState && !this.IsTakingOff && !this.IsGrabbedState && !this.IsDead && !this.IsBusyRespawning && !this.actionBlocksStateChange(this.ActionState, false) && (this.IsGrounded || !this.data.Physics.UsedAirJump) && (!this.IsGrounded || !this.data.Physics.UsedGroundJump) && ((!this.IsJumpStunned && !this.IsGrounded) || (!this.IsStunned && this.IsGrounded));
		}
	}

	// Token: 0x17000862 RID: 2146
	// (get) Token: 0x06002463 RID: 9315 RVA: 0x000B69CE File Offset: 0x000B4DCE
	bool IPlayerState.CanReleaseLedge
	{
		get
		{
			return !this.IsLedgeGrabbing && this.IsLedgeHangingState && this.data.Model.ledgeLagFrames <= 0;
		}
	}

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x06002464 RID: 9316 RVA: 0x000B69FF File Offset: 0x000B4DFF
	bool IPlayerState.CanFallThroughPlatforms
	{
		get
		{
			return this.IsHelpless || this.IsFalling || this.IsAirJumping || this.IsJumpingUp || this.IsPlatformDropping;
		}
	}

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06002465 RID: 9317 RVA: 0x000B6A38 File Offset: 0x000B4E38
	bool IPlayerState.CanDieOffTop
	{
		get
		{
			return this.data.Physics.KnockbackVelocity.sqrMagnitude > 0 && (this.IsTumbling || this.data.State.IsStunned);
		}
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x000B6A8C File Offset: 0x000B4E8C
	bool IPlayerState.CanBeginPivot(HorizontalDirection direction)
	{
		return direction != this.data.Facing && this.IsGrounded && !this.IsJumpingState && !this.IsRunPivoting && !this.IsPivoting && !this.IsDashing && !this.IsShieldingState && !this.CanBeginShield && !this.IsReleasingShield && !this.IsBusyWithMove && !this.IsTakingOff;
	}

	// Token: 0x17000865 RID: 2149
	// (get) Token: 0x06002467 RID: 9319 RVA: 0x000B6B1C File Offset: 0x000B4F1C
	bool IPlayerState.CanBeginWalking
	{
		get
		{
			return (!this.IsBusyWithMove || (!this.isBlockMovement() && !this.IsGrounded)) && !this.IsHelpless && !this.IsStunned && !this.IsDashing && !this.IsRunning && !this.IsWalking && !this.IsBraking && !this.IsDashBraking && !this.IsRunPivoting && !this.IsPivoting && !this.IsCrouching && this.IsGrounded && !this.IsJumpingState && !this.CanBeginShield && !this.IsReleasingShield && !this.IsTakingOff;
		}
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000B6BF0 File Offset: 0x000B4FF0
	bool IPlayerState.CanBeginRunPivot(HorizontalDirection newDirection)
	{
		return newDirection != this.data.Facing && this.IsGrounded && !this.IsJumpingState && (this.IsRunning || this.IsBraking) && !this.IsRunPivoting && !this.IsTakingOff;
	}

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x06002469 RID: 9321 RVA: 0x000B6C54 File Offset: 0x000B5054
	public bool CanBeginIdling
	{
		get
		{
			return !this.IsHitLagPaused && !this.IsDead && !this.IsStunned && !this.IsHitStunned && this.IsGrounded && !this.data.ActiveMove.IsActive && !this.IsIdling && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsEndCrouching && !this.IsShieldingState && !this.IsStandardGrabbingState && !this.IsGrabbedState && !this.IsDashing && !this.IsRunning && !this.IsRunPivoting && !this.IsDashPivoting && !this.IsPivoting && !this.IsDownState && !this.CanBeginShield && !this.IsShieldBroken && !this.IsTakingOff && !this.IsBraking && !this.IsDashBraking && !this.IsRunPivotBraking && !this.IsReleasingShield && !this.actionBlocksStateChange(this.ActionState, true) && !this.IsTeetering && (!this.IsHelpless || Vector3F.Dot(this.data.Physics.Velocity.normalized, this.data.Physics.GroundedNormal) < (Fixed)0.1);
		}
	}

	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x0600246A RID: 9322 RVA: 0x000B6DF8 File Offset: 0x000B51F8
	bool IPlayerState.CanBeginFalling
	{
		get
		{
			return !this.IsGrounded && !this.IsTumbling && !this.IsHitStunned && !this.IsStunned && !this.IsLedgeHangingState && !this.IsGrabbedState && !this.IsFalling && !this.IsReleasingShield && !this.data.LedgeGrabController.IsLedgeGrabbing && (!this.data.ActiveMove.IsActive || this.data.ActiveMove.CancelOnFall) && this.ShouldPlayFallOrLandAction && !this.IsHelpless && !this.IsJumpingUp && !this.IsAirJumping && !this.IsRespawning && !this.IsDead && !this.IsGrabEscaping && !this.IsPlatformDropping;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x0600246B RID: 9323 RVA: 0x000B6EF5 File Offset: 0x000B52F5
	bool IPlayerState.CanResumeGrabbing
	{
		get
		{
			return !this.IsMoveActive && this.IsStandardGrabbingState && !this.IsGrabbing;
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x0600246C RID: 9324 RVA: 0x000B6F19 File Offset: 0x000B5319
	bool IPlayerState.CanEndCrouch
	{
		get
		{
			return this.IsCrouching && !this.IsBusyWithMove;
		}
	}

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x0600246D RID: 9325 RVA: 0x000B6F34 File Offset: 0x000B5334
	public bool CanBeginCrouching
	{
		get
		{
			return (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Crouch)) && this.IsGrounded && this.IsStandingState && !this.CanBeginShield && !this.IsStunned && !this.IsShieldingState && !this.IsCrouching && !this.IsBeginningCrouching && !this.IsEndCrouching && !this.IsTakingOff && !this.IsDashing && !this.IsDashPivoting && !this.IsReleasingShield && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsHelpless && (!this.IsRunPivoting || !this.IsBusyWithAction) && !this.IsRespawning;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x0600246E RID: 9326 RVA: 0x000B7030 File Offset: 0x000B5430
	bool IPlayerState.CanBeginBraking
	{
		get
		{
			return this.IsRunning && !this.IsBraking && !this.IsDashBraking && (!this.data.GameInput.IsCrouchingInputPressed || !this.CanBeginCrouching);
		}
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000B7084 File Offset: 0x000B5484
	bool IPlayerState.CanBeginDashing(HorizontalDirection direction)
	{
		return this.IsGrounded && !this.IsJumpingState && !this.IsRunning && (!this.IsBraking || !this.IsBusyWithAction) && (!this.IsRunPivoting || !this.IsBusyWithAction) && (!this.IsDashing || direction == this.data.OppositeFacing) && !this.CanBeginShield && !this.IsGrabbedState && !this.IsStandardGrabbingState && !this.IsBusyWithMove && !this.IsReleasingShield && !this.IsBeginningCrouching && !this.IsTakingOff;
	}

	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x06002470 RID: 9328 RVA: 0x000B7148 File Offset: 0x000B5548
	bool IPlayerState.CanBeginTeetering
	{
		get
		{
			return !this.IsTeetering && this.data.Physics.TeeteringDirection == this.data.Facing && !this.IsMoveActive && (this.IsWalking || this.IsIdling || this.IsBraking || this.IsDashBraking);
		}
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x06002471 RID: 9329 RVA: 0x000B71B8 File Offset: 0x000B55B8
	bool IPlayerState.CanReleaseShieldVoluntarily
	{
		get
		{
			return !this.IsTakingOff && !this.IsLedgeHangingState && !this.data.Shield.IsGusting && !this.IsStunned && !this.IsBeginningShield;
		}
	}

	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x06002472 RID: 9330 RVA: 0x000B7208 File Offset: 0x000B5608
	public bool CanMaintainShield
	{
		get
		{
			return (!this.IsBusyWithMove || this.data.ActiveMove.Data.label == MoveLabel.ShieldGust) && this.IsGrounded && this.IsShieldingState && !this.IsShieldBroken && !this.IsTakingOff && (!this.IsStunned || this.data.Model.stunType == StunType.ShieldStun);
		}
	}

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x06002473 RID: 9331 RVA: 0x000B728C File Offset: 0x000B568C
	public bool CanBeginShield
	{
		get
		{
			return this.data.GameInput.IsShieldInputPressed && !this.IsDead && !this.IsHitLagPaused && !this.IsStunned && !this.IsShieldingState && this.IsStandingState && !this.IsTakingOff && (!this.IsLanding || !this.IsBusyWithAction) && this.IsGrounded && (!this.IsBusyWithMove || this.data.ActiveMove.Model.IsInterruptibleByAction(this.data, PlayerMovementAction.Shield)) && !this.IsShieldingState && !this.IsReleasingShield && !this.IsRecoiling && !this.IsRunPivoting && !this.IsDashPivoting && !this.actionBlocksStateChange(this.ActionState, false) && !this.IsRespawning;
		}
	}

	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x06002474 RID: 9332 RVA: 0x000B7392 File Offset: 0x000B5792
	public bool IsTechOffCooldown
	{
		get
		{
			return this.frameOwner.Frame - this.data.Model.lastTechFrame < this.config.knockbackConfig.techInputThresholdFrames;
		}
	}

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x06002475 RID: 9333 RVA: 0x000B73C4 File Offset: 0x000B57C4
	public bool IsTechableMode
	{
		get
		{
			return this.data.Model.stunTechMode == StunTechMode.Techable || (this.data.Model.stunTechMode == StunTechMode.FirstBounceUntechable && this.data.Model.untechableBounceUsed);
		}
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000B7414 File Offset: 0x000B5814
	public bool CanWallJump(HorizontalDirection wallJumpDirection)
	{
		if (!this.IsGrounded && this.frameOwner.Frame - this.data.Model.ledgeReleaseFrame >= this.config.wallJumpConfig.ledgeReleaseLockoutFrames && !this.IsWallJumping)
		{
			if (wallJumpDirection != HorizontalDirection.Left)
			{
				if (wallJumpDirection == HorizontalDirection.Right)
				{
					int num = this.data.Model.lastLeftInputFrame;
				}
			}
			else
			{
				int num = this.data.Model.lastRightInputFrame;
			}
			HorizontalDirection direction = InputUtils.GetDirection(this.data.Model.bufferedInput.inputButtonsData.horizontalAxisValue);
			return wallJumpDirection != direction;
		}
		return false;
	}

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x06002477 RID: 9335 RVA: 0x000B74D8 File Offset: 0x000B58D8
	public bool IsInUntechableStun
	{
		get
		{
			return this.IsStunned && (this.data.Model.stunType == StunType.ShieldStun || this.data.Model.stunType == StunType.ShieldBreakStun || (!this.config.knockbackConfig.enableSdiTeching && this.IsHitLagPaused));
		}
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000B7540 File Offset: 0x000B5940
	public bool CanTech(SurfaceType surfaceType)
	{
		return this.IsTumbling && this.IsTechOffCooldown && !this.IsInUntechableStun && (surfaceType != SurfaceType.Floor || this.IsTechableMode);
	}

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x06002479 RID: 9337 RVA: 0x000B7575 File Offset: 0x000B5975
	public bool IsRecovered
	{
		get
		{
			return this.IsIdling && this.CanUseMoves;
		}
	}

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x0600247A RID: 9338 RVA: 0x000B758C File Offset: 0x000B598C
	public bool IsBusyWithAction
	{
		get
		{
			return this.data.ActionData != null && (this.data.ActionData.wrapMode == WrapMode.Loop || this.data.Model.actionStateFrame < this.data.AnimationPlayer.CurrentAnimationGameFramelength - Mathf.Max(this.data.Model.overrideActionStateInterruptibilityFrames, this.data.ActionData.interruptibleFrames));
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x0600247B RID: 9339 RVA: 0x000B760D File Offset: 0x000B5A0D
	public bool IsBusyWithMove
	{
		get
		{
			return this.data.ActiveMove.IsActive && !this.data.ActiveMove.Model.IsInterruptibleByAnything(this.data);
		}
	}

	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x0600247C RID: 9340 RVA: 0x000B7645 File Offset: 0x000B5A45
	public bool IsMoveActive
	{
		get
		{
			return this.data.ActiveMove.IsActive;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x0600247D RID: 9341 RVA: 0x000B7657 File Offset: 0x000B5A57
	bool IPlayerState.IsInControl
	{
		get
		{
			return !this.IsDownState && (!this.IsHelpless && !this.IsStunned);
		}
	}

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x0600247E RID: 9342 RVA: 0x000B767E File Offset: 0x000B5A7E
	public bool IsStunned
	{
		get
		{
			return this.data.Model.stunFrames > 0;
		}
	}

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x0600247F RID: 9343 RVA: 0x000B7693 File Offset: 0x000B5A93
	public bool IsJumpStunned
	{
		get
		{
			return this.data.Model.jumpStunFrames > 0;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06002480 RID: 9344 RVA: 0x000B76A8 File Offset: 0x000B5AA8
	public bool IsHitLagPaused
	{
		get
		{
			return this.data.Model.hitLagFrames > 0 && this.data.Model.ignoreHitLagFrames <= 0;
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06002481 RID: 9345 RVA: 0x000B76D9 File Offset: 0x000B5AD9
	public bool IsUnderChainGrabPrevention
	{
		get
		{
			return this.data.Model.chainGrabPreventionFrames > 0;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06002482 RID: 9346 RVA: 0x000B76EE File Offset: 0x000B5AEE
	public bool IsCameraFlourishMode
	{
		get
		{
			return this.IsHitLagPaused && this.data.Model.isKillCamHitlag;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06002483 RID: 9347 RVA: 0x000B770E File Offset: 0x000B5B0E
	public bool IsCameraZoomMode
	{
		get
		{
			return this.IsHitLagPaused && this.data.Model.isCameraZoomHitLag;
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06002484 RID: 9348 RVA: 0x000B772E File Offset: 0x000B5B2E
	public bool IsGrounded
	{
		get
		{
			return this.data.Physics.IsGrounded;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06002485 RID: 9349 RVA: 0x000B7740 File Offset: 0x000B5B40
	public bool IsShieldBroken
	{
		get
		{
			return this.data.Shield.IsBroken;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06002486 RID: 9350 RVA: 0x000B7752 File Offset: 0x000B5B52
	public bool IsReadyToGrabRelease
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06002487 RID: 9351 RVA: 0x000B7755 File Offset: 0x000B5B55
	public bool IsHelpless
	{
		get
		{
			return this.data.Model.subState == SubStates.Helpless;
		}
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06002488 RID: 9352 RVA: 0x000B776A File Offset: 0x000B5B6A
	public bool IsBusyRespawning
	{
		get
		{
			return this.IsRespawning && !this.data.RespawnController.HasArrived;
		}
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06002489 RID: 9353 RVA: 0x000B778D File Offset: 0x000B5B8D
	public bool IsRespawning
	{
		get
		{
			return this.data.Model.isRespawning;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x0600248A RID: 9354 RVA: 0x000B779F File Offset: 0x000B5B9F
	public bool IsDead
	{
		get
		{
			return this.data.Model.isDead;
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x0600248B RID: 9355 RVA: 0x000B77B1 File Offset: 0x000B5BB1
	public bool IsImmuneToBlastZone
	{
		get
		{
			return this.data.Model.blastZoneImmunityFrames > 0;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x0600248C RID: 9356 RVA: 0x000B77C6 File Offset: 0x000B5BC6
	public bool IsAffectedByUnflinchingKnockback
	{
		get
		{
			return !this.IsLedgeHangingState && (!this.data.ActiveMove.IsActive || !this.data.ActiveMove.Data.IsLedgeRecovery);
		}
	}

	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x0600248D RID: 9357 RVA: 0x000B7808 File Offset: 0x000B5C08
	public bool IsLedgeRecovering
	{
		get
		{
			if (!this.data.ActiveMove.IsActive)
			{
				return false;
			}
			switch (this.data.ActiveMove.Data.label)
			{
			case MoveLabel.LedgeAttack:
			case MoveLabel.LedgeStand:
			case MoveLabel.LedgeRoll:
				return true;
			}
			return false;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x0600248E RID: 9358 RVA: 0x000B7863 File Offset: 0x000B5C63
	public bool IsWallJumping
	{
		get
		{
			return this.data.ActiveMove.IsActive && this.data.ActiveMove.Data.GetComponent<WallJumpComponent>() != null;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x0600248F RID: 9359 RVA: 0x000B7898 File Offset: 0x000B5C98
	public bool ShouldIgnoreForces
	{
		get
		{
			return this.data.LedgeGrabController.IsLedgeGrabbing || this.IsGrabbedState || this.IsRespawning || this.IsHitLagPaused || this.IsDead;
		}
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000B78E4 File Offset: 0x000B5CE4
	private void setState(MetaState newState)
	{
		if (newState == this.data.Model.state)
		{
			return;
		}
		if (newState != MetaState.Down)
		{
			if (newState != MetaState.Stand)
			{
				if (newState != MetaState.Shielding)
				{
				}
			}
			else
			{
				this.data.Model.ClearLastHitData();
			}
		}
		else
		{
			this.data.Model.downedFrames = 0;
		}
		MetaState state = this.data.Model.state;
		if (state != MetaState.Down)
		{
			if (state == MetaState.Shielding)
			{
				if (newState != MetaState.Shielding)
				{
					this.actor.ReleaseShield(false, false);
				}
			}
		}
		else
		{
			this.data.Model.downedFrames = 0;
		}
		this.data.Model.state = newState;
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000B79B8 File Offset: 0x000B5DB8
	private void setSubState(SubStates newSubState)
	{
		if (newSubState != SubStates.Helpless)
		{
			if (newSubState == SubStates.Tumbling)
			{
				this.data.Renderer.SetColorModeFlag(ColorMode.Tumbling, true);
			}
		}
		else
		{
			this.data.Renderer.SetColorModeFlag(ColorMode.Helpless, true);
		}
		SubStates subState = this.data.Model.subState;
		if (subState != SubStates.Helpless)
		{
			if (subState == SubStates.Tumbling)
			{
				this.data.Renderer.SetColorModeFlag(ColorMode.Tumbling, false);
			}
		}
		else
		{
			this.data.Renderer.SetColorModeFlag(ColorMode.Helpless, false);
		}
		this.data.Model.subState = newSubState;
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x000B7A6C File Offset: 0x000B5E6C
	private bool actionBlocksStateChange(ActionState characterAction, bool toIdle = false)
	{
		switch (characterAction)
		{
		case ActionState.GrabEscapeAir:
			return this.IsBusyWithAction;
		default:
			switch (characterAction)
			{
			case ActionState.DazedBegin:
			case ActionState.Recoil:
			case ActionState.GrabRelease:
			case ActionState.GrabEscapeGround:
				break;
			default:
				if (characterAction != ActionState.Landing && characterAction != ActionState.EdgeGrab)
				{
					return false;
				}
				break;
			}
			break;
		case ActionState.DazedLoop:
		case ActionState.DazedEnd:
		case ActionState.FallDown:
			break;
		}
		return toIdle || this.IsBusyWithAction;
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06002493 RID: 9363 RVA: 0x000B7AF4 File Offset: 0x000B5EF4
	public bool ShouldPlayFallOrLandAction
	{
		get
		{
			return !this.IsGrabReleasing && this.ActionState != ActionState.GrabEscapeGround;
		}
	}

	// Token: 0x04001B7B RID: 7035
	private IPlayerDataOwner data;

	// Token: 0x04001B7C RID: 7036
	private IPlayerStateActor actor;

	// Token: 0x04001B7D RID: 7037
	private IFrameOwner frameOwner;

	// Token: 0x04001B7E RID: 7038
	private ConfigData config;

	// Token: 0x04001B7F RID: 7039
	private IBattleServerAPI battleServerAPI;
}
