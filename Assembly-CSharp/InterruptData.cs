using System;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004FA RID: 1274
[Serializable]
public class InterruptData : ICloneable, IPreloadedGameAsset
{
	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x0008C0F6 File Offset: 0x0008A4F6
	// (set) Token: 0x06001BB3 RID: 7091 RVA: 0x0008C0FE File Offset: 0x0008A4FE
	public bool linkMovesEditorToggle { get; set; }

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0008C107 File Offset: 0x0008A507
	// (set) Token: 0x06001BB5 RID: 7093 RVA: 0x0008C10F File Offset: 0x0008A50F
	public bool hitEditorToggle { get; set; }

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x0008C118 File Offset: 0x0008A518
	// (set) Token: 0x06001BB7 RID: 7095 RVA: 0x0008C120 File Offset: 0x0008A520
	public bool counterEditorToggle { get; set; }

	// Token: 0x06001BB8 RID: 7096 RVA: 0x0008C129 File Offset: 0x0008A529
	public object Clone()
	{
		return CloneUtil.SlowDeepClone<InterruptData>(this);
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x0008C134 File Offset: 0x0008A534
	private bool isDirectionHeld(IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		Vector2F vector2F = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
		Vector2F normalized = vector2F.normalized;
		if (normalized.magnitude < this.triggerThreshhold)
		{
			return false;
		}
		if (this.triggerOnStickDirectionHeldMinAngle != 0 || this.triggerOnStickDirectionHeldMaxAngle != 0)
		{
			if (playerDelegate.Facing == HorizontalDirection.Left)
			{
				normalized.x = -normalized.x;
			}
			Fixed one = MathUtil.VectorToAngle(ref normalized);
			return one >= this.triggerOnStickDirectionHeldMinAngle && one <= this.triggerOnStickDirectionHeldMaxAngle;
		}
		return true;
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x0008C1D4 File Offset: 0x0008A5D4
	private bool isDirectionPressed(IPlayerDelegate playerDelegate, InputButtonsData input, RelativeDirection direction)
	{
		switch (direction)
		{
		case RelativeDirection.Up:
			return input.movementButtonsPressed.Contains(ButtonPress.UpTap);
		case RelativeDirection.Down:
			return input.movementButtonsPressed.Contains(ButtonPress.DownTap);
		default:
			return input.movementButtonsPressed.Contains(ButtonPress.ForwardTap);
		case RelativeDirection.Backward:
			return input.movementButtonsPressed.Contains(ButtonPress.BackwardTap);
		}
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x0008C234 File Offset: 0x0008A634
	public bool RepeatButtonsPressed(MoveModel model, InputButtonsData input)
	{
		return this.interruptType == InterruptType.Move && this.linkableMoves.Length > 0 && this.triggerType == InterruptTriggerType.OnRepeatButtonPress && !model.repeatButtonPressed && this.repeatButton != ButtonPress.None && this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && input.moveButtonsPressed.Contains(this.repeatButton);
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x0008C2BC File Offset: 0x0008A6BC
	public bool ImmediateButtonPressed(MoveModel model, InputButtonsData input)
	{
		return this.interruptType == InterruptType.Move && (this.linkableMoves.Length > 0 || this.getLinkableMoveFromComponent) && this.triggerType == InterruptTriggerType.OnButtonPressedImmediate && !model.immediateButtonPressed && this.immediateButton != ButtonPress.None && this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && model.firstFrameOfMove != model.internalFrame && input.moveButtonsPressed.Contains(this.immediateButton);
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x0008C358 File Offset: 0x0008A758
	private bool canLinkWithoutLinkableMoves()
	{
		return this.getLinkableMoveFromComponent || this.triggerType == InterruptTriggerType.OnLand || this.triggerType == InterruptTriggerType.OnFall;
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x0008C380 File Offset: 0x0008A780
	public bool ShouldUseLink(LinkCheckType linkCheckType, IPlayerDelegate playerDelegate, MoveModel model, InputButtonsData input)
	{
		if (this.linkableMoves.Length <= 0 && !this.canLinkWithoutLinkableMoves())
		{
			return false;
		}
		if (this.skipIfPreventHelpless && playerDelegate.Model.ignoreNextHelplessness)
		{
			return false;
		}
		switch (linkCheckType)
		{
		case LinkCheckType.MoveFrame:
			if (this.interruptType == InterruptType.Move)
			{
				InterruptTriggerType interruptTriggerType = this.triggerType;
				switch (interruptTriggerType)
				{
				case InterruptTriggerType.OnDirectionHeld:
					if (this.startFrame == model.internalFrame && this.linkOnNoInput != this.isDirectionHeld(playerDelegate, input))
					{
						return true;
					}
					break;
				case InterruptTriggerType.OnRepeatButtonPress:
					if (this.endFrame + 1 == model.internalFrame && model.repeatButtonPressed)
					{
						return true;
					}
					break;
				default:
					if (interruptTriggerType != InterruptTriggerType.None)
					{
						if (interruptTriggerType == InterruptTriggerType.OnDirectionPressedImmediate)
						{
							if (this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && this.isDirectionPressed(playerDelegate, input, this.relativeDirectionPressed))
							{
								return true;
							}
						}
					}
					else if (this.startFrame == model.internalFrame && this.autoActivate)
					{
						return true;
					}
					break;
				case InterruptTriggerType.OnButtonPressedImmediate:
					if (model.immediateButtonPressed)
					{
						return true;
					}
					break;
				}
			}
			break;
		case LinkCheckType.OnHit:
			return this.interruptType == InterruptType.Move && this.autoActivate && this.triggerType == InterruptTriggerType.OnHit;
		case LinkCheckType.Counter:
			return this.interruptType == InterruptType.Move && this.autoActivate && this.triggerType == InterruptTriggerType.OnCounter;
		case LinkCheckType.MoveEnd:
			if (this.triggerType == InterruptTriggerType.OnMoveEnd)
			{
				return true;
			}
			if (this.triggerType == InterruptTriggerType.OnRepeatButtonPress)
			{
				if (this.endFrame == model.internalFrame && model.repeatButtonPressed)
				{
					return true;
				}
			}
			else
			{
				if (this.triggerType == InterruptTriggerType.GustFailed && this.autoActivate)
				{
					return !playerDelegate.Shield.GustSuccess;
				}
				if (this.triggerType == InterruptTriggerType.GustSucceeded && this.autoActivate)
				{
					return playerDelegate.Shield.GustSuccess;
				}
			}
			break;
		case LinkCheckType.GustUpdated:
			if (this.triggerType == InterruptTriggerType.GustFailed && !this.autoActivate)
			{
				return !playerDelegate.Shield.GustSuccess;
			}
			if (this.triggerType == InterruptTriggerType.GustSucceeded && !this.autoActivate)
			{
				return playerDelegate.Shield.GustSuccess;
			}
			break;
		case LinkCheckType.OnLand:
			return this.triggerType == InterruptTriggerType.OnLand && model.internalFrame >= this.startFrame && model.internalFrame <= this.endFrame;
		case LinkCheckType.OnFall:
			return this.triggerType == InterruptTriggerType.OnFall && model.internalFrame >= this.startFrame && model.internalFrame <= this.endFrame;
		}
		return false;
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x0008C680 File Offset: 0x0008AA80
	public void RegisterPreload(PreloadContext context)
	{
		foreach (MoveData moveData in this.linkableMoves)
		{
			if (moveData != null)
			{
				moveData.RegisterPreload(context);
			}
		}
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x0008C6C0 File Offset: 0x0008AAC0
	public LandCancelOptions GetLandCancelOptions()
	{
		return new LandCancelOptions
		{
			landMove = null,
			startLandAtCurrentFrame = this.startMoveTransitionAtCurrentFrame,
			transferHitDisabledTargets = this.transferHitDisabledTargets,
			transferChargeData = this.transferChargeData,
			haltMovementOnLand = this.haltMovementOnTransition,
			maxHorizontalLandSpeed = this.maxHorizontalLandSpeed,
			bounceSpeed = this.bounceSpeed,
			bounceParticle = this.bounceParticle,
			bounceCancelMove = this.bounceCancelMove,
			noAutoCancelFramesBegin = this.noAutoCancelFramesBegin,
			noAutoCancelFramesEnd = this.noAutoCancelFramesEnd,
			landCancelClip = this.landCancelClip,
			leftLandCancelClip = this.leftLandCancelClip,
			landLagFrames = this.landLagFrames,
			landLagVisualFrames = this.landLagVisualFrames
		};
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x0008C794 File Offset: 0x0008AB94
	public FallCancelOptions GetFallCancelOptions()
	{
		return new FallCancelOptions
		{
			fallMove = null,
			startFallAtCurrentFrame = this.startMoveTransitionAtCurrentFrame,
			transferHitDisableTargets = this.transferHitDisabledTargets
		};
	}

	// Token: 0x04001594 RID: 5524
	public InterruptType interruptType;

	// Token: 0x04001595 RID: 5525
	public bool skipIfPreventHelpless;

	// Token: 0x04001596 RID: 5526
	public bool allowOnSuccessfulGust;

	// Token: 0x04001597 RID: 5527
	public bool transferHitDisabledTargets;

	// Token: 0x04001598 RID: 5528
	public bool allowGroundMovement;

	// Token: 0x04001599 RID: 5529
	public bool transferChargeData;

	// Token: 0x0400159A RID: 5530
	[FormerlySerializedAs("linkType")]
	public InterruptTriggerType triggerType;

	// Token: 0x0400159B RID: 5531
	public ButtonPress repeatButton = ButtonPress.None;

	// Token: 0x0400159C RID: 5532
	public int repeatButtonPressMaxCount = -1;

	// Token: 0x0400159D RID: 5533
	public ButtonPress immediateButton = ButtonPress.None;

	// Token: 0x0400159E RID: 5534
	[FormerlySerializedAs("activeFramesBegins")]
	public int startFrame;

	// Token: 0x0400159F RID: 5535
	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	// Token: 0x040015A0 RID: 5536
	public bool linkOnNoInput;

	// Token: 0x040015A1 RID: 5537
	public int triggerOnStickDirectionHeldMinAngle;

	// Token: 0x040015A2 RID: 5538
	public int triggerOnStickDirectionHeldMaxAngle;

	// Token: 0x040015A3 RID: 5539
	public Fixed triggerThreshhold = (Fixed)0.1;

	// Token: 0x040015A4 RID: 5540
	public RelativeDirection relativeDirectionPressed = RelativeDirection.Forward;

	// Token: 0x040015A5 RID: 5541
	public bool reverseFacing;

	// Token: 0x040015A6 RID: 5542
	public int nextMoveStartupFrame;

	// Token: 0x040015A7 RID: 5543
	public int interruptMinFrame;

	// Token: 0x040015A8 RID: 5544
	public bool interruptHighPriority;

	// Token: 0x040015A9 RID: 5545
	public MoveData[] linkableMoves = new MoveData[0];

	// Token: 0x040015AA RID: 5546
	public PlayerMovementAction[] interruptActions = new PlayerMovementAction[0];

	// Token: 0x040015AB RID: 5547
	public bool autoActivate;

	// Token: 0x040015AC RID: 5548
	public bool allowFacingDirectionChange;

	// Token: 0x040015AD RID: 5549
	public bool ignoreHit = true;

	// Token: 0x040015AE RID: 5550
	public bool faceHit = true;

	// Token: 0x040015AF RID: 5551
	public bool seedCounteredDamage = true;

	// Token: 0x040015B0 RID: 5552
	public bool getLinkableMoveFromComponent;

	// Token: 0x040015B1 RID: 5553
	public bool startMoveTransitionAtCurrentFrame;

	// Token: 0x040015B2 RID: 5554
	public bool haltMovementOnTransition;

	// Token: 0x040015B3 RID: 5555
	public Fixed maxHorizontalLandSpeed = 0;

	// Token: 0x040015B4 RID: 5556
	public Fixed bounceSpeed = 0;

	// Token: 0x040015B5 RID: 5557
	public ParticleData bounceParticle;

	// Token: 0x040015B6 RID: 5558
	public MoveData bounceCancelMove;

	// Token: 0x040015B7 RID: 5559
	public int noAutoCancelFramesBegin;

	// Token: 0x040015B8 RID: 5560
	public int noAutoCancelFramesEnd;

	// Token: 0x040015B9 RID: 5561
	public AnimationClip landCancelClip;

	// Token: 0x040015BA RID: 5562
	public AnimationClip leftLandCancelClip;

	// Token: 0x040015BB RID: 5563
	public int landLagFrames;

	// Token: 0x040015BC RID: 5564
	public int landLagVisualFrames;
}
