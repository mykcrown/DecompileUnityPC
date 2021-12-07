using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000686 RID: 1670
[ExecuteInEditMode]
public class CharacterInputProcessor : ICharacterInputProcessor, IInputProcessor
{
	// Token: 0x0600293B RID: 10555 RVA: 0x000DB0B6 File Offset: 0x000D94B6
	public CharacterInputProcessor()
	{
		this.tumbleWiggleButtons.Add(ButtonPress.ForwardTap);
		this.tumbleWiggleButtons.Add(ButtonPress.BackwardTap);
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x0600293C RID: 10556 RVA: 0x000DB0EE File Offset: 0x000D94EE
	// (set) Token: 0x0600293D RID: 10557 RVA: 0x000DB0F6 File Offset: 0x000D94F6
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x0600293E RID: 10558 RVA: 0x000DB0FF File Offset: 0x000D94FF
	// (set) Token: 0x0600293F RID: 10559 RVA: 0x000DB107 File Offset: 0x000D9507
	[Inject]
	public IPlayerTauntsFinder tauntFinder { get; set; }

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x06002940 RID: 10560 RVA: 0x000DB110 File Offset: 0x000D9510
	// (set) Token: 0x06002941 RID: 10561 RVA: 0x000DB118 File Offset: 0x000D9518
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x06002942 RID: 10562 RVA: 0x000DB121 File Offset: 0x000D9521
	// (set) Token: 0x06002943 RID: 10563 RVA: 0x000DB129 File Offset: 0x000D9529
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x06002944 RID: 10564 RVA: 0x000DB132 File Offset: 0x000D9532
	// (set) Token: 0x06002945 RID: 10565 RVA: 0x000DB13A File Offset: 0x000D953A
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x06002946 RID: 10566 RVA: 0x000DB143 File Offset: 0x000D9543
	InputButtonsData ICharacterInputProcessor.CurrentInputData
	{
		get
		{
			return this.cachedInputButtonsData;
		}
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x000DB14B File Offset: 0x000D954B
	public CharacterInputProcessor Setup(IPlayerInputActor actor, IPlayerStateActor stateActor, IAudioOwner audioOwner, ConfigData config, IFrameOwner frameOwner)
	{
		this.stateActor = stateActor;
		this.actor = actor;
		this.config = config;
		this.frameOwner = frameOwner;
		this.audioOwner = audioOwner;
		return this;
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x000DB174 File Offset: 0x000D9574
	private void gatherInput(InputData inputData, int currentFrame, InputController inputController)
	{
		if (inputData.inputType == InputType.HorizontalAxis || inputData.inputType == InputType.VerticalAxis)
		{
			Fixed axis = inputController.GetAxis(inputData);
			bool tapped = false;
			bool tapped2 = inputController.GetTapped(inputData);
			int framesHeldDown = inputController.GetFramesHeldDown(inputData);
			if (InputUtils.IsTappedInputValue(this.config.inputConfig, axis, framesHeldDown) && !inputController.GetButton(ButtonPress.Tilt))
			{
				tapped = true;
			}
			else if (tapped2 && axis == 0)
			{
				tapped = false;
			}
			inputController.UpdateTapped(inputData, tapped, currentFrame);
			this.gatherLeftAxisInput(inputData.inputType, axis, tapped, framesHeldDown, inputData, inputController);
		}
		else if (inputData.inputType == InputType.Button && !InputController.IsMetagameInput(inputData.button, false))
		{
			ButtonPress buttonPress = inputData.button;
			if (InputUtils.IsHorizontal(buttonPress) && this.actor.Facing == HorizontalDirection.Left)
			{
				buttonPress = InputUtils.GetOppositeHorizontalButton(buttonPress);
			}
			ButtonPress item = buttonPress;
			ButtonPress press = buttonPress;
			if (buttonPress == ButtonPress.GustShield)
			{
				item = ButtonPress.Shield1;
				press = ButtonPress.Special;
			}
			if (inputController.GetButton(inputData))
			{
				this.cachedInputButtonsData.buttonsHeld.Add(item);
			}
			if (inputController.GetButtonDown(inputData))
			{
				this.cachedInputButtonsData.AddButtonPressed(press, false);
			}
		}
		if (this.actor.TriggerHeldInputAsTaps && this.actor.Model.actionStateFrame == 1 && inputController.GetFramesHeldDown(inputData) > 0)
		{
			this.cachedInputButtonsData.AddButtonPressed(inputData.button, false);
		}
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x000DB2F0 File Offset: 0x000D96F0
	private void gatherLeftAxisInput(InputType axisType, Fixed value, bool tapped, int framesHeldDown, InputData inputData, InputController inputController)
	{
		if (value == 0)
		{
			return;
		}
		ButtonPress buttonPress = ButtonPress.None;
		bool flag = tapped && framesHeldDown <= this.config.inputConfig.tapFrames;
		if (axisType == InputType.HorizontalAxis)
		{
			buttonPress = InputUtils.GetButtonFromHorizontalValue(this.actor.Facing, value);
			if (flag && buttonPress != ButtonPress.None)
			{
				buttonPress = InputUtils.GetTapped(buttonPress);
			}
		}
		else
		{
			if (value > 0)
			{
				buttonPress = ButtonPress.Up;
			}
			else if (value < 0)
			{
				buttonPress = ButtonPress.Down;
			}
			if (flag && buttonPress != ButtonPress.None)
			{
				buttonPress = InputUtils.GetTapped(buttonPress);
			}
		}
		inputData.button = buttonPress;
		this.readAxisAsButtonPress(inputData, inputController, this.cachedInputButtonsData);
		this.cachedInputButtonsData.buttonsHeld.Add(inputData.button);
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x000DB3C8 File Offset: 0x000D97C8
	private bool processMovementInputMain(InputButtonsData inputButtonsData, int frame, bool allowTapJumping, bool allowRecoveryJumping, bool requireDoubleTapToRun)
	{
		bool flag = false;
		HorizontalDirection horizontalDirection = HorizontalDirection.None;
		if (inputButtonsData.horizontalAxisValue > 0)
		{
			horizontalDirection = HorizontalDirection.Right;
		}
		else if (inputButtonsData.horizontalAxisValue < 0)
		{
			horizontalDirection = HorizontalDirection.Left;
		}
		if (horizontalDirection != HorizontalDirection.None)
		{
			if (horizontalDirection != inputButtonsData.facing && (this.actor.State.IsLedgeGrabbing || this.actor.State.IsLedgeHangingState) && this.actor.State.CanReleaseLedge && inputButtonsData.movementButtonsPressed.Contains(ButtonPress.BackwardTap))
			{
				this.actor.LedgeGrabController.ReleaseGrabbedLedge(true, true);
				flag = true;
			}
			bool flag2 = false;
			if (this.actor.State.CanBeginCrouching && this.actor.ActiveMove.IsActive && this.actor.ActiveMove.Data.label == MoveLabel.DownAttack && InputUtils.IsCrouchInput(inputButtonsData.verticalAxisValue, inputButtonsData.horizontalAxisValue, this.config.inputConfig))
			{
				flag2 = true;
			}
			bool isDoubleTap = false;
			if (ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.ForwardTap) || ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.BackwardTap))
			{
				if (frame - this.actor.Model.lastConsumedTapInput < this.config.inputConfig.doubleTapFrameThreshold && frame - this.actor.Model.lastConsumedTapInput > 1 && this.actor.Model.lastTapDirection == horizontalDirection)
				{
					isDoubleTap = true;
				}
				else
				{
					this.actor.Model.lastConsumedTapInput = frame;
					this.actor.Model.lastTapDirection = horizontalDirection;
				}
			}
			if (this.actor.State.CanMove && !flag2)
			{
				if (this.shouldBeginDashing(inputButtonsData.movementButtonsPressed, horizontalDirection, isDoubleTap, requireDoubleTapToRun) && FixedMath.Abs(inputButtonsData.verticalAxisValue) < Fixed.Create(0.3))
				{
					if (inputButtonsData.facing == horizontalDirection || this.actor.State.IsIdling)
					{
						if (inputButtonsData.facing != horizontalDirection)
						{
							this.actor.SetFacingAndRotation(horizontalDirection);
						}
						this.stateActor.BeginDashing(horizontalDirection);
						flag = true;
					}
					else
					{
						this.stateActor.BeginDashPivot(horizontalDirection);
					}
				}
				else if (this.actor.State.ActionState == ActionState.Dash && horizontalDirection != this.actor.Facing && FixedMath.Abs(inputButtonsData.horizontalAxisValue) > Fixed.Create(0.5))
				{
					Fixed x = (horizontalDirection != HorizontalDirection.Left) ? Fixed.Create(1.5) : Fixed.Create(-1.5);
					Vector3F v = new Vector2F(x, 0);
					this.actor.Physics.AddVelocity(v, 1, VelocityType.Movement);
				}
				else if (this.actor.State.CanBeginRunPivot(horizontalDirection))
				{
					this.stateActor.BeginRunPivot(horizontalDirection);
				}
				else if (this.actor.State.CanBeginPivot(horizontalDirection))
				{
					this.stateActor.BeginPivot(horizontalDirection);
				}
				else if (this.actor.State.CanBeginWalking || this.actor.State.IsWalking)
				{
					this.stateActor.BeginWalking(inputButtonsData.horizontalAxisValue);
				}
				if (this.actor.ActiveMove.IsActive && this.actor.ActiveMove.Data.allowReversal && this.actor.ActiveMove.Model.internalFrame <= this.actor.ActiveMove.Data.reversalFrames)
				{
					bool flag3 = FixedMath.Abs(inputButtonsData.horizontalAxisValue) > this.config.inputConfig.specialReverseThreshold;
					if (flag3)
					{
						this.actor.ActiveMove.Model.reversalDirection = horizontalDirection;
					}
				}
				if ((!this.actor.State.IsBusyWithMove || !this.actor.State.IsBlockMovement) && !this.actor.State.IsBraking && !this.actor.State.IsDashBraking)
				{
					this.actor.Physics.ApplyAcceleration(horizontalDirection, inputButtonsData.horizontalAxisValue);
				}
			}
		}
		else
		{
			this.stateActor.TryBeginBraking();
		}
		if (this.actor.ActiveMove.IsActive && this.actor.ActiveMove.Data.allowReversal && this.actor.ActiveMove.Model.internalFrame == this.actor.ActiveMove.Data.reversalFrames && this.actor.State.CanMove && this.actor.ActiveMove.Model.reversalDirection != HorizontalDirection.None && this.actor.ActiveMove.Model.reversalDirection != this.actor.Facing)
		{
			this.actor.SetFacingAndRotation(this.actor.ActiveMove.Model.reversalDirection);
			this.actor.Physics.ReverseHorizontalMovement();
			this.actor.ActiveMove.WasReversed();
		}
		IPhysicsCollider collider;
		if (this.shouldDropThroughPlatform(inputButtonsData.movementButtonsPressed, inputButtonsData.buttonsHeld) && this.actor.State.IsShieldingState && this.actor.State.CanReleaseShieldVoluntarily && this.actor.Physics.CheckIsOnPlatform(out collider) && !this.actor.State.IsPlatformDropping)
		{
			this.stateActor.BeginPlatformDrop(collider);
			flag = true;
		}
		if (inputButtonsData.verticalAxisValue < 0 && !this.actor.State.IsHitLagPaused && !this.actor.State.IsStunned && !this.actor.State.IsDazed && !this.actor.State.IsTakingOff)
		{
			IPhysicsCollider collider2;
			if (this.shouldDropThroughPlatform(inputButtonsData.movementButtonsPressed, inputButtonsData.buttonsHeld) && !this.actor.State.IsShieldingState && this.actor.Physics.CheckIsOnPlatform(out collider2))
			{
				this.actor.FallThroughPlatformHeldFrames++;
				if (!this.actor.State.IsPlatformDropping && this.actor.FallThroughPlatformHeldFrames > this.config.lagConfig.platformDropLagFrames)
				{
					this.stateActor.BeginPlatformDrop(collider2);
				}
			}
			if (this.isDropInput(inputButtonsData.movementButtonsPressed))
			{
				if (this.canFastFall())
				{
					this.actor.Audio.PlayGameSound(new AudioRequest(this.config.defaultCharacterEffects.fastFallSound, this.audioOwner, null));
					this.actor.GameVFX.PlayParticle(this.config.defaultCharacterEffects.fastfall, false, TeamNum.None);
					this.actor.Physics.BeginFastFalling();
				}
				this.actor.OnDropInput();
			}
			if (this.actor.State.CanReleaseLedge && ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.DownTap))
			{
				this.actor.LedgeGrabController.ReleaseGrabbedLedge(true, true);
			}
		}
		if ((ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, ButtonPress.DownStrike) || ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, ButtonPress.BackwardStrike)) && (this.actor.State.IsLedgeGrabbing || this.actor.State.IsLedgeHangingState) && this.actor.State.CanReleaseLedge)
		{
			this.actor.LedgeGrabController.ReleaseGrabbedLedge(true, true);
			flag |= true;
		}
		if (inputButtonsData.verticalAxisValue >= 0)
		{
			this.actor.FallThroughPlatformHeldFrames = 0;
		}
		if (this.shouldEndCrouch(inputButtonsData.verticalAxisValue))
		{
			this.stateActor.StartCharacterAction(ActionState.CrouchEnd, null, null, true, 0, false);
		}
		if (ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, ButtonPress.Shield1) || ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, ButtonPress.Shield2))
		{
			this.stateActor.TryBeginShield(false);
		}
		else if (this.actor.State.CanReleaseShieldVoluntarily && this.actor.State.IsShieldingState)
		{
			this.stateActor.ReleaseShield(true, true);
		}
		if (this.actor.State.IsTumbling && !this.actor.State.IsStunned && this.processTumbleWiggleInput(inputButtonsData))
		{
			this.actor.Model.tumbleWiggleBreakInputCount++;
			if (this.actor.Model.tumbleWiggleBreakInputCount >= this.config.knockbackConfig.tumbleWiggleOutInputCount)
			{
				this.actor.Model.ClearLastTumbleData();
				this.stateActor.StartCharacterAction(ActionState.FallStraight, null, null, true, 0, false);
			}
		}
		if (ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.Wavedash))
		{
			flag |= this.stateActor.AttemptWavedash();
		}
		return flag;
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000DBD90 File Offset: 0x000DA190
	private void processAerialJumpCancel(InputButtonsData inputButtonsData)
	{
		int num = this.frameOwner.Frame - this.actor.Model.jumpBeginFrame;
		if (this.actor.Model.jumpBeginFrame > 0 && num <= this.config.inputConfig.inputShorthopFrames)
		{
			ButtonPress jumpButtonSource = this.actor.Model.jumpButtonSource;
			bool flag;
			if (jumpButtonSource == ButtonPress.UpTap)
			{
				flag = (inputButtonsData.verticalAxisValue <= 0);
			}
			else
			{
				flag = !ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, jumpButtonSource);
			}
			if (flag)
			{
				this.actor.Model.jumpBeginFrame = 0;
			}
		}
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000DBE38 File Offset: 0x000DA238
	private ButtonPress processJumping(InputButtonsData inputButtonsData, int frame, bool allowTapJumping, bool allowRecoveryJumping)
	{
		ButtonPress buttonPress = ButtonPress.None;
		ButtonPress buttonPress2 = ButtonPress.None;
		if (ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.FullJump))
		{
			buttonPress2 = ButtonPress.FullJump;
		}
		else if (ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.ShortJump))
		{
			buttonPress2 = ButtonPress.ShortJump;
		}
		else if (ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.Jump))
		{
			buttonPress2 = ButtonPress.Jump;
		}
		if (buttonPress2 != ButtonPress.None)
		{
			if (allowRecoveryJumping && this.stateActor.AttemptRecoveryJump())
			{
				buttonPress = buttonPress2;
			}
			if (buttonPress == ButtonPress.None && this.stateActor.AttemptJump(buttonPress2))
			{
				buttonPress = buttonPress2;
			}
		}
		bool flag = allowTapJumping && ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, ButtonPress.UpTap);
		if (buttonPress == ButtonPress.None)
		{
			if (flag)
			{
				if (allowRecoveryJumping && this.stateActor.AttemptRecoveryJump())
				{
					buttonPress = ButtonPress.UpTap;
				}
				if (buttonPress == ButtonPress.None && this.stateActor.AttemptJump(ButtonPress.UpTap))
				{
					buttonPress = ButtonPress.UpTap;
				}
			}
			else if (this.actor.State.MetaState == MetaState.Shielding && ButtonPressUtil.ListContains(inputButtonsData.buttonsHeld, ButtonPress.UpStrike) && this.stateActor.AttemptJump(ButtonPress.UpStrike))
			{
				buttonPress = ButtonPress.UpStrike;
			}
		}
		return buttonPress;
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000DBF70 File Offset: 0x000DA370
	private bool processTumbleWiggleInput(InputButtonsData inputButtonsData)
	{
		foreach (ButtonPress buttonPress in this.tumbleWiggleButtons)
		{
			if (this.actor.Model.tumbleWiggleLastButton != buttonPress && ButtonPressUtil.ListContains(inputButtonsData.movementButtonsPressed, buttonPress))
			{
				this.actor.Model.tumbleWiggleLastButton = buttonPress;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000DC008 File Offset: 0x000DA408
	public void Cache()
	{
		this.actor.Cache(this.cachedInputButtonsData);
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000DC01C File Offset: 0x000DA41C
	public void ProcessInput(int currentFrame, InputController inputController, PlayerReference reference, bool retainBuffer)
	{
		this.cachedInputButtonsData.Clear();
		this.cachedInputButtonsData.facing = this.actor.Facing;
		this.cachedInputButtonsData.currentFrame = currentFrame;
		if (this.actor.AreInputsLocked)
		{
			return;
		}
		Fixed axis = inputController.GetAxis(inputController.horizontalAxis);
		Fixed axis2 = inputController.GetAxis(inputController.verticalAxis);
		this.cachedInputButtonsData.horizontalAxisValue = axis;
		this.cachedInputButtonsData.verticalAxisValue = axis2;
		for (int i = 0; i < inputController.allInputData.Count; i++)
		{
			InputData inputData = inputController.allInputData[i];
			this.gatherInput(inputData, currentFrame, inputController);
		}
		this.updateTechCooldown(this.cachedInputButtonsData, currentFrame, reference);
		this.tryHoloOrVoiceline(this.cachedInputButtonsData, reference);
		if (!this.actor.State.IsHitLagPaused)
		{
			bool allowTapJumpThisFrame = inputController.AllowTapJumpThisFrame;
			bool allowRecoveryJumpingThisFrame = inputController.AllowRecoveryJumpingThisFrame;
			bool requireDoubleTapToRunThisFrame = inputController.RequireDoubleTapToRunThisFrame;
			ProcessInputStateResult processInputStateResult = this.ProcessInputState(this.cachedInputButtonsData, currentFrame, allowTapJumpThisFrame, allowRecoveryJumpingThisFrame, requireDoubleTapToRunThisFrame);
			bool flag = processInputStateResult.triggeredNonJump || processInputStateResult.consumeAll;
			if (processInputStateResult.triggeredJump != ButtonPress.None)
			{
				ButtonPressUtil.ListRemove(this.cachedInputButtonsData.movementButtonsPressed, processInputStateResult.triggeredJump);
				ButtonPressUtil.ListRemove(this.cachedInputButtonsData.moveButtonsPressed, processInputStateResult.triggeredJump);
				ButtonPressUtil.ListRemove(this.cachedInputButtonsData.buttonsHeld, processInputStateResult.triggeredJump);
			}
			if (flag)
			{
				if (!retainBuffer)
				{
					this.actor.Model.bufferedInput.Clear();
				}
			}
			else if (this.cachedInputButtonsData.moveButtonsPressed.Count > 0 || this.cachedInputButtonsData.movementButtonsPressed.Count > 0)
			{
				List<ButtonPress> bufferableInput = this.actor.GetBufferableInput(this.cachedInputButtonsData);
				if (bufferableInput != null)
				{
					if (this.actor.State.IsLanding && this.actor.Model.landedWithAirDodge)
					{
						if (this.cachedInputButtonsData.buttonsHeld.Contains(ButtonPress.Shield1) || this.cachedInputButtonsData.buttonsHeld.Contains(ButtonPress.Shield2))
						{
							bufferableInput.Clear();
						}
						if (bufferableInput.Count == 1 && (bufferableInput[0] == ButtonPress.BackwardTap || bufferableInput[0] == ButtonPress.ForwardTap))
						{
							bufferableInput.Clear();
						}
					}
					if (this.actor.Model.previousFrameBufferTap && bufferableInput.Count == 1 && (bufferableInput[0] == ButtonPress.BackwardTap || bufferableInput[0] == ButtonPress.ForwardTap))
					{
						bufferableInput.Clear();
					}
					if (bufferableInput.Count > 0)
					{
						int currentFrame2 = this.actor.Model.bufferedInput.inputButtonsData.currentFrame;
						if (this.cachedInputButtonsData.currentFrame - currentFrame2 <= this.config.inputConfig.inputBufferCollationFrames)
						{
							this.actor.Model.bufferedInput.AdditiveLoad(this.cachedInputButtonsData, bufferableInput);
						}
						else
						{
							this.actor.Model.bufferedInput.Load(this.cachedInputButtonsData, bufferableInput);
						}
					}
				}
			}
			this.actor.Model.previousFrameBufferTap = false;
			foreach (ButtonPress buttonPress in this.cachedInputButtonsData.moveButtonsPressed)
			{
				if (buttonPress == ButtonPress.ForwardTap || buttonPress == ButtonPress.BackwardTap)
				{
					this.actor.Model.previousFrameBufferTap = true;
				}
			}
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000DC3E8 File Offset: 0x000DA7E8
	private void tryHoloOrVoiceline(InputButtonsData inputButtonsData, PlayerReference reference)
	{
		if (inputButtonsData.moveButtonsPressed.Contains(ButtonPress.TauntLeft))
		{
			this.tryTaunt(TauntSlot.LEFT, reference);
		}
		else if (inputButtonsData.moveButtonsPressed.Contains(ButtonPress.TauntRight))
		{
			this.tryTaunt(TauntSlot.RIGHT, reference);
		}
		else if (inputButtonsData.moveButtonsPressed.Contains(ButtonPress.TauntUp))
		{
			this.tryTaunt(TauntSlot.UP, reference);
		}
		else if (inputButtonsData.moveButtonsPressed.Contains(ButtonPress.TauntDown))
		{
			this.tryTaunt(TauntSlot.DOWN, reference);
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000DC46C File Offset: 0x000DA86C
	private void tryTaunt(TauntSlot slot, PlayerReference reference)
	{
		UserTaunts forPlayer = this.tauntFinder.GetForPlayer(reference.PlayerNum);
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = forPlayer.GetSlotsForCharacter(reference.Controller.CharacterData.characterID);
		EquipmentID id;
		if (slotsForCharacter.TryGetValue(slot, out id) && !id.IsNull())
		{
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item != null && !reference.Controller.IsRateLimited())
			{
				if (item.type == EquipmentTypes.HOLOGRAM)
				{
					reference.Controller.FreeHologram(this.itemLoader.LoadAsset<HologramData>(item));
				}
				else if (item.type == EquipmentTypes.VOICE_TAUNT)
				{
					reference.Controller.FreeVoiceTaunt(this.itemLoader.LoadAsset<VoiceTauntData>(item));
				}
			}
		}
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x000DC52C File Offset: 0x000DA92C
	private void updateTechCooldown(InputButtonsData inputButtonsData, int currentFrame, PlayerReference reference)
	{
		if ((ButtonPressUtil.ListContains(inputButtonsData.moveButtonsPressed, ButtonPress.Shield1) || ButtonPressUtil.ListContains(inputButtonsData.moveButtonsPressed, ButtonPress.Shield2) || (this.gameDataManager.IsFeatureEnabled(FeatureID.AttackAssist) && reference.InputController.AttackAssistThisFrame)) && currentFrame - this.actor.LastTechFrame > this.config.knockbackConfig.techCooldownFrames)
		{
			this.actor.LastTechFrame = currentFrame;
		}
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000DC5B4 File Offset: 0x000DA9B4
	public ProcessInputStateResult ProcessInputState(InputButtonsData inputButtonsData, int frame, bool allowTapJumping, bool allowRecoveryJumping, bool requireDoubleTapToRun)
	{
		ProcessInputStateResult result = default(ProcessInputStateResult);
		result.triggeredJump = ButtonPress.None;
		if (this.actor.State.IsGrabbedState)
		{
			this.actor.ButtonsPressedThisFrame = inputButtonsData.movementButtonsPressed.Count;
			result.consumeAll = true;
			return result;
		}
		if (this.actor.State.IsRespawning && !this.actor.State.IsBusyRespawning && ((inputButtonsData.movementButtonsPressed.Count > 0 && this.containsNonTaunt(inputButtonsData.movementButtonsPressed)) || (inputButtonsData.buttonsHeld.Count > 0 && this.containsNonTaunt(inputButtonsData.buttonsHeld))))
		{
			this.actor.DismountRespawnPlatform();
		}
		if (this.actor.TryBeginBufferedInterrupt(inputButtonsData, true))
		{
			result.triggeredNonJump = true;
			return result;
		}
		result.triggeredJump = this.processJumping(inputButtonsData, frame, allowTapJumping, allowRecoveryJumping);
		this.processAerialJumpCancel(inputButtonsData);
		if ((inputButtonsData.moveButtonsPressed.Count > 0 || inputButtonsData.buttonsHeld.Count > 0) && this.actor.TryBeginMove(inputButtonsData))
		{
			result.triggeredNonJump = true;
			return result;
		}
		if (this.actor.TryBeginBufferedInterrupt(inputButtonsData, false))
		{
			result.triggeredNonJump = true;
			return result;
		}
		if (this.actor.ActiveMove != null && this.actor.ActiveMove.TryToLinkMove(this.actor.ActiveMove.Model, inputButtonsData))
		{
			result.triggeredNonJump = true;
			return result;
		}
		result.triggeredNonJump |= this.processMovementInputMain(inputButtonsData, frame, allowTapJumping, allowRecoveryJumping, requireDoubleTapToRun);
		return result;
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x000DC76C File Offset: 0x000DAB6C
	private bool containsNonTaunt(List<ButtonPress> buttons)
	{
		foreach (ButtonPress button in buttons)
		{
			if (!InputUtils.IsTaunt(button))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000DC7D4 File Offset: 0x000DABD4
	private bool retriggerInput(InputData inputData, InputController inputController)
	{
		return !this.actor.State.IsLanding && !this.actor.State.IsTakingOff && !this.actor.State.IsJumpingState && (!this.actor.ActiveMove.IsActive || !(this.actor.ActiveMove.Data != null) || this.actor.ActiveMove.Data.label != MoveLabel.AirDodge) && inputController.GetTapped(inputData) && this.actor.Model.actionStateFrame == 1;
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000DC894 File Offset: 0x000DAC94
	private void readAxisAsButtonPress(InputData inputData, InputController inputController, InputButtonsData buttons)
	{
		if (inputController.GetFramesHeldDown(inputData) == 1 || inputController.GetJustTapped(inputData))
		{
			buttons.AddButtonPressed(inputData.button, false);
		}
		else if (this.retriggerInput(inputData, inputController))
		{
			buttons.AddButtonPressed(inputData.button, true);
		}
		if (inputData.button == ButtonPress.BackwardTap)
		{
			this.actor.Model.lastBackTapFrame = this.frameOwner.Frame;
		}
		else if (inputData.button == ButtonPress.ForwardTap)
		{
			this.actor.Model.lastBackTapFrame = 0;
		}
		if (buttons.horizontalAxisValue != 0)
		{
			HorizontalDirection direction = InputUtils.GetDirection(buttons.horizontalAxisValue);
			if (direction == HorizontalDirection.Right)
			{
				this.actor.Model.lastRightInputFrame = this.frameOwner.Frame;
			}
			else if (direction == HorizontalDirection.Left)
			{
				this.actor.Model.lastLeftInputFrame = this.frameOwner.Frame;
			}
		}
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x000DC997 File Offset: 0x000DAD97
	private bool isDropInput(List<ButtonPress> pressed)
	{
		return ButtonPressUtil.ListContains(pressed, ButtonPress.DownTap);
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000DC9A4 File Offset: 0x000DADA4
	private bool canFastFall()
	{
		return (!this.actor.ActiveMove.IsActive || !this.actor.State.IsBlockFastFall) && this.actor.Physics.Velocity.y <= 0 && !this.actor.State.IsGrounded && !this.actor.State.IsRespawning && !this.actor.State.IsDead && !this.actor.Physics.IsFastFalling && !this.actor.State.IsLedgeGrabbing && this.actor.Physics.State.platformFallPreventFastfall == 0 && this.actor.AllowFastFall;
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x000DCA90 File Offset: 0x000DAE90
	private bool shouldFallThroughPlatform(List<ButtonPress> pressed, List<ButtonPress> held)
	{
		return this.actor.State.CanDropThroughPlatform && !this.actor.State.IsBusyWithMove && (ButtonPressUtil.ListContains(pressed, ButtonPress.DownTap) || ButtonPressUtil.ListContains(held, ButtonPress.Down));
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000DCAE4 File Offset: 0x000DAEE4
	private bool shouldDropThroughPlatform(List<ButtonPress> pressed, List<ButtonPress> held)
	{
		return (this.actor.State.CanDropThroughPlatform && (ButtonPressUtil.ListContains(pressed, ButtonPress.DownTap) || this.actor.FallThroughPlatformHeldFrames > 0)) || this.actor.State.IsPlatformDropping || (ButtonPressUtil.ListContains(pressed, ButtonPress.Strike) && this.actor.State.IsShieldingState);
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x000DCB5C File Offset: 0x000DAF5C
	private bool shouldEndCrouch(Fixed verticalAxis)
	{
		return (FixedMath.ApproximatelyEqual(verticalAxis, 0) || verticalAxis >= 0) && this.actor.State.CanEndCrouch;
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x000DCB90 File Offset: 0x000DAF90
	private bool shouldBeginDashing(List<ButtonPress> pressed, HorizontalDirection direction, bool isDoubleTap, bool requireDoubleTapToRun)
	{
		bool flag = ButtonPressUtil.ListContains(pressed, ButtonPress.ForwardTap) || ButtonPressUtil.ListContains(pressed, ButtonPress.BackwardTap);
		if (requireDoubleTapToRun)
		{
			flag = (flag && isDoubleTap);
		}
		return this.actor.State.CanBeginDashing(direction) && flag;
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x000DCBDB File Offset: 0x000DAFDB
	private bool shouldBeginRunPivot(List<ButtonPress> pressed, HorizontalDirection direction)
	{
		return this.actor.State.CanBeginRunPivot(direction);
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000DCBEE File Offset: 0x000DAFEE
	private bool shouldBeginIdling(InputController inputController)
	{
		return this.actor.State.CanBeginIdling && !inputController.IsHorizontalDirectionHeld(HorizontalDirection.Any);
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000DCC14 File Offset: 0x000DB014
	public void ChangeStateIfNecessary(InputController inputController, MoveLabel fromMove = MoveLabel.None)
	{
		if (inputController.IsCrouchingInputPressed && this.actor.State.CanBeginCrouching)
		{
			if (fromMove == MoveLabel.DownAttack && !this.actor.ActiveMove.IsActive)
			{
				this.stateActor.StartCharacterAction(ActionState.Crouching, null, null, true, 0, false);
			}
			else if (fromMove != MoveLabel.DownAttack)
			{
				this.stateActor.TryBeginCrouching();
			}
		}
		else if (!inputController.IsCrouchingInputPressed && !this.actor.ActiveMove.IsActive && fromMove == MoveLabel.DownAttack)
		{
			this.stateActor.StartCharacterAction(ActionState.CrouchEnd, null, null, true, 0, false);
		}
		else if (this.actor.State.IsStandardGrabbingState && this.actor.State.CanResumeGrabbing)
		{
			this.stateActor.StartCharacterAction(ActionState.Grabbing, null, null, true, 0, false);
		}
		else if (inputController.IsShieldInputPressed && this.actor.State.CanBeginShield)
		{
			this.stateActor.TryBeginShield(false);
		}
		else if (this.actor.State.CanBeginTeetering)
		{
			this.stateActor.BeginTeetering();
		}
		else if (this.shouldBeginIdling(inputController))
		{
			this.stateActor.BeginIdling();
		}
		else if (this.actor.State.CanBeginFalling)
		{
			this.stateActor.BeginFalling(ActionState.FallStraight, false);
		}
		else if (this.actor.State.IsFalling)
		{
			Fixed axis = inputController.GetAxis(inputController.horizontalAxis);
			ActionState fallState = this.getFallState(this.actor.Facing, axis);
			if (fallState != this.actor.State.ActionState)
			{
				this.stateActor.BeginFalling(fallState, true);
			}
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x000DCE00 File Offset: 0x000DB200
	private ActionState getFallState(HorizontalDirection facing, Fixed horizontalAxis)
	{
		if (facing == HorizontalDirection.Left || facing != HorizontalDirection.Right)
		{
			if (horizontalAxis < 0)
			{
				return ActionState.FallForward;
			}
			if (horizontalAxis > 0)
			{
				return ActionState.FallBack;
			}
			return this.actor.State.ActionState;
		}
		else
		{
			if (horizontalAxis < 0)
			{
				return ActionState.FallBack;
			}
			if (horizontalAxis > 0)
			{
				return ActionState.FallForward;
			}
			return this.actor.State.ActionState;
		}
	}

	// Token: 0x04001DDF RID: 7647
	private InputButtonsData cachedInputButtonsData = new InputButtonsData();

	// Token: 0x04001DE0 RID: 7648
	private ConfigData config;

	// Token: 0x04001DE1 RID: 7649
	private IPlayerInputActor actor;

	// Token: 0x04001DE2 RID: 7650
	private IPlayerStateActor stateActor;

	// Token: 0x04001DE3 RID: 7651
	private IFrameOwner frameOwner;

	// Token: 0x04001DE4 RID: 7652
	private IAudioOwner audioOwner;

	// Token: 0x04001DE5 RID: 7653
	private List<ButtonPress> tumbleWiggleButtons = new List<ButtonPress>();
}
