// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class InterruptData : ICloneable, IPreloadedGameAsset
{
	public InterruptType interruptType;

	public bool skipIfPreventHelpless;

	public bool allowOnSuccessfulGust;

	public bool transferHitDisabledTargets;

	public bool allowGroundMovement;

	public bool transferChargeData;

	[FormerlySerializedAs("linkType")]
	public InterruptTriggerType triggerType;

	public ButtonPress repeatButton = ButtonPress.None;

	public int repeatButtonPressMaxCount = -1;

	public ButtonPress immediateButton = ButtonPress.None;

	[FormerlySerializedAs("activeFramesBegins")]
	public int startFrame;

	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	public bool linkOnNoInput;

	public int triggerOnStickDirectionHeldMinAngle;

	public int triggerOnStickDirectionHeldMaxAngle;

	public Fixed triggerThreshhold = (Fixed)0.1;

	public RelativeDirection relativeDirectionPressed = RelativeDirection.Forward;

	public bool reverseFacing;

	public int nextMoveStartupFrame;

	public int interruptMinFrame;

	public bool interruptHighPriority;

	public MoveData[] linkableMoves = new MoveData[0];

	public PlayerMovementAction[] interruptActions = new PlayerMovementAction[0];

	public bool autoActivate;

	public bool allowFacingDirectionChange;

	public bool ignoreHit = true;

	public bool faceHit = true;

	public bool seedCounteredDamage = true;

	public bool getLinkableMoveFromComponent;

	public bool startMoveTransitionAtCurrentFrame;

	public bool haltMovementOnTransition;

	public Fixed maxHorizontalLandSpeed = 0;

	public Fixed bounceSpeed = 0;

	public ParticleData bounceParticle;

	public MoveData bounceCancelMove;

	public int noAutoCancelFramesBegin;

	public int noAutoCancelFramesEnd;

	public AnimationClip landCancelClip;

	public AnimationClip leftLandCancelClip;

	public int landLagFrames;

	public int landLagVisualFrames;

	public bool linkMovesEditorToggle
	{
		get;
		set;
	}

	public bool hitEditorToggle
	{
		get;
		set;
	}

	public bool counterEditorToggle
	{
		get;
		set;
	}

	public object Clone()
	{
		return CloneUtil.SlowDeepClone<InterruptData>(this);
	}

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

	private bool isDirectionPressed(IPlayerDelegate playerDelegate, InputButtonsData input, RelativeDirection direction)
	{
		switch (direction)
		{
		case RelativeDirection.Up:
			return input.movementButtonsPressed.Contains(ButtonPress.UpTap);
		case RelativeDirection.Down:
			return input.movementButtonsPressed.Contains(ButtonPress.DownTap);
		case RelativeDirection.Forward:
			IL_1B:
			return input.movementButtonsPressed.Contains(ButtonPress.ForwardTap);
		case RelativeDirection.Backward:
			return input.movementButtonsPressed.Contains(ButtonPress.BackwardTap);
		}
		goto IL_1B;
	}

	public bool RepeatButtonsPressed(MoveModel model, InputButtonsData input)
	{
		return this.interruptType == InterruptType.Move && this.linkableMoves.Length > 0 && this.triggerType == InterruptTriggerType.OnRepeatButtonPress && !model.repeatButtonPressed && this.repeatButton != ButtonPress.None && this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && input.moveButtonsPressed.Contains(this.repeatButton);
	}

	public bool ImmediateButtonPressed(MoveModel model, InputButtonsData input)
	{
		return this.interruptType == InterruptType.Move && (this.linkableMoves.Length > 0 || this.getLinkableMoveFromComponent) && this.triggerType == InterruptTriggerType.OnButtonPressedImmediate && !model.immediateButtonPressed && this.immediateButton != ButtonPress.None && this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && model.firstFrameOfMove != model.internalFrame && input.moveButtonsPressed.Contains(this.immediateButton);
	}

	private bool canLinkWithoutLinkableMoves()
	{
		return this.getLinkableMoveFromComponent || this.triggerType == InterruptTriggerType.OnLand || this.triggerType == InterruptTriggerType.OnFall;
	}

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
					goto IL_1AD;
				case InterruptTriggerType.OnRepeatButtonPress:
					if (this.endFrame + 1 == model.internalFrame && model.repeatButtonPressed)
					{
						return true;
					}
					goto IL_1AD;
				case InterruptTriggerType.OnMoveEnd:
				case InterruptTriggerType.GustSucceeded:
				case InterruptTriggerType.GustFailed:
					IL_D7:
					if (interruptTriggerType != InterruptTriggerType.None)
					{
						if (interruptTriggerType != InterruptTriggerType.OnDirectionPressedImmediate)
						{
							goto IL_1AD;
						}
						if (this.startFrame <= model.internalFrame && this.endFrame >= model.internalFrame && this.isDirectionPressed(playerDelegate, input, this.relativeDirectionPressed))
						{
							return true;
						}
						goto IL_1AD;
					}
					else
					{
						if (this.startFrame == model.internalFrame && this.autoActivate)
						{
							return true;
						}
						goto IL_1AD;
					}
					break;
				case InterruptTriggerType.OnButtonPressedImmediate:
					if (model.immediateButtonPressed)
					{
						return true;
					}
					goto IL_1AD;
				}
				goto IL_D7;
			}
			IL_1AD:
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

	public void RegisterPreload(PreloadContext context)
	{
		MoveData[] array = this.linkableMoves;
		for (int i = 0; i < array.Length; i++)
		{
			MoveData moveData = array[i];
			if (moveData != null)
			{
				moveData.RegisterPreload(context);
			}
		}
	}

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

	public FallCancelOptions GetFallCancelOptions()
	{
		return new FallCancelOptions
		{
			fallMove = null,
			startFallAtCurrentFrame = this.startMoveTransitionAtCurrentFrame,
			transferHitDisableTargets = this.transferHitDisabledTargets
		};
	}
}
