// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialAnimationController
{
	private sealed class _addMoveTransitionCancelOptionEndFrames_c__AnonStorey0
	{
		internal MoveData move;

		internal bool __m__0(MaterialAnimationData.EndFrameData ef)
		{
			return ef.move == this.move;
		}
	}

	private static Dictionary<PlayerController.InteractionSignalData.Type, MaterialAnimationData.TriggerType> NotificationTypeMap = new Dictionary<PlayerController.InteractionSignalData.Type, MaterialAnimationData.TriggerType>
	{
		{
			PlayerController.InteractionSignalData.Type.DealDamage,
			MaterialAnimationData.TriggerType.DealDamage
		},
		{
			PlayerController.InteractionSignalData.Type.TakeDamage,
			MaterialAnimationData.TriggerType.TakeDamage
		},
		{
			PlayerController.InteractionSignalData.Type.Land,
			MaterialAnimationData.TriggerType.Land
		},
		{
			PlayerController.InteractionSignalData.Type.MoveEnd,
			MaterialAnimationData.TriggerType.MoveEnd
		},
		{
			PlayerController.InteractionSignalData.Type.Grabbed,
			MaterialAnimationData.TriggerType.Grabbed
		},
		{
			PlayerController.InteractionSignalData.Type.Flinched,
			MaterialAnimationData.TriggerType.Flinched
		},
		{
			PlayerController.InteractionSignalData.Type.Died,
			MaterialAnimationData.TriggerType.Died
		}
	};

	private IMoveOwner moveOwner;

	private List<MaterialAnimationData.EndFrameData> endFrames = new List<MaterialAnimationData.EndFrameData>(10);

	private Texture cachedTexture;

	private Material cachedMaterial;

	private Color cachedColor;

	private Color cachedSecondaryColor;

	private ColorGradientId cachedGradient;

	public bool PendingRemoval;

	public MaterialAnimationData data;

	private bool removed;

	private bool initialized;

	private MaterialTargetsData targets;

	private int currentFrame;

	private bool pingPongToggle;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public void Init(MaterialTargetsData targets, MaterialAnimationData data, IMoveOwner moveOwner)
	{
		this.targets = targets;
		this.data = data;
		this.moveOwner = moveOwner;
		bool flag = (MaterialAnimationData.TriggerType.MoveFrame & data.endCondition) != (MaterialAnimationData.TriggerType)0;
		if (flag)
		{
			foreach (MaterialAnimationData.EndFrameData current in data.endFrames)
			{
				if (current.move == null || current.move == moveOwner.MoveData)
				{
					this.endFrames.Add(new MaterialAnimationData.EndFrameData
					{
						move = moveOwner.MoveData,
						frame = current.frame
					});
					this.addLegacyCancelOptionEndFrames(current);
					this.addMoveTransitionCancelOptionEndFrames(current);
				}
				else
				{
					this.endFrames.Add(current);
				}
			}
		}
		this.initialized = false;
		if (data.endCondition != (MaterialAnimationData.TriggerType)0)
		{
			this.signalBus.GetSignal<PlayerController.InteractionSignal>().AddListener(new Action<PlayerController.InteractionSignalData>(this.EndForNotification));
		}
		this.CacheValues();
	}

	private void addLegacyCancelOptionEndFrames(MaterialAnimationData.EndFrameData endFrame)
	{
		if (this.moveOwner.MoveData.cancelOptions.cancelOnLand && this.moveOwner.MoveData.cancelOptions.startLandMoveAtCurrentFrame && this.moveOwner.MoveData.cancelOptions.landMove != null && this.data.endFrames.FindIndex(new Predicate<MaterialAnimationData.EndFrameData>(this._addLegacyCancelOptionEndFrames_m__0)) == -1)
		{
			this.endFrames.Add(new MaterialAnimationData.EndFrameData
			{
				move = this.moveOwner.MoveData.cancelOptions.landMove,
				frame = endFrame.frame
			});
		}
		if (this.moveOwner.MoveData.cancelOptions.cancelOnFall && this.moveOwner.MoveData.cancelOptions.startFallMoveAtCurrentFrame && this.moveOwner.MoveData.cancelOptions.fallMove != null && this.data.endFrames.FindIndex(new Predicate<MaterialAnimationData.EndFrameData>(this._addLegacyCancelOptionEndFrames_m__1)) == -1)
		{
			this.endFrames.Add(new MaterialAnimationData.EndFrameData
			{
				move = this.moveOwner.MoveData.cancelOptions.fallMove,
				frame = endFrame.frame
			});
		}
	}

	private void addMoveTransitionCancelOptionEndFrames(MaterialAnimationData.EndFrameData endFrame)
	{
		for (int i = 0; i < this.moveOwner.MoveData.interrupts.Length; i++)
		{
			InterruptData interruptData = this.moveOwner.MoveData.interrupts[i];
			if ((interruptData.triggerType == InterruptTriggerType.OnLand || interruptData.triggerType == InterruptTriggerType.OnFall) && interruptData.startMoveTransitionAtCurrentFrame && interruptData.linkableMoves.Length > 0)
			{
				MaterialAnimationController._addMoveTransitionCancelOptionEndFrames_c__AnonStorey0 _addMoveTransitionCancelOptionEndFrames_c__AnonStorey = new MaterialAnimationController._addMoveTransitionCancelOptionEndFrames_c__AnonStorey0();
				_addMoveTransitionCancelOptionEndFrames_c__AnonStorey.move = interruptData.linkableMoves[0];
				if (_addMoveTransitionCancelOptionEndFrames_c__AnonStorey.move.totalInternalFrames >= this.moveOwner.MoveData.totalInternalFrames && this.data.endFrames.FindIndex(new Predicate<MaterialAnimationData.EndFrameData>(_addMoveTransitionCancelOptionEndFrames_c__AnonStorey.__m__0)) == -1)
				{
					this.endFrames.Add(new MaterialAnimationData.EndFrameData
					{
						move = _addMoveTransitionCancelOptionEndFrames_c__AnonStorey.move,
						frame = endFrame.frame
					});
				}
			}
		}
	}

	public void EndForNotification(PlayerController.InteractionSignalData data)
	{
		MaterialAnimationData.TriggerType type;
		if (this.moveOwner == data.moveOwner && MaterialAnimationController.NotificationTypeMap.TryGetValue(data.trigger, out type))
		{
			this.EndForNotification(type);
		}
	}

	public bool EndForNotification(MaterialAnimationData.TriggerType type)
	{
		bool flag = (type & this.data.endCondition) != (MaterialAnimationData.TriggerType)0;
		if (!flag && type == MaterialAnimationData.TriggerType.MoveEnd)
		{
			bool flag2 = (MaterialAnimationData.TriggerType.MoveFrame & this.data.endCondition) != (MaterialAnimationData.TriggerType)0;
			if (flag2)
			{
				bool flag3 = this.endFrames.Count == 0;
				bool flag4 = this.endFrames.Count == 1 && this.endFrames[0].move == this.moveOwner.MoveData;
				if (flag3 || flag4)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.OnRemove();
			return true;
		}
		return false;
	}

	public bool TickFrame()
	{
		bool flag = (MaterialAnimationData.TriggerType.MoveFrame & this.data.endCondition) != (MaterialAnimationData.TriggerType)0;
		if (flag && this.moveOwner.MoveIsValid)
		{
			foreach (MaterialAnimationData.EndFrameData current in this.endFrames)
			{
				if (current.move == this.moveOwner.MoveData && this.moveOwner.InternalFrame >= current.frame)
				{
					this.PendingRemoval = true;
					break;
				}
			}
		}
		if (this.removed || this.PendingRemoval)
		{
			this.OnRemove();
			return false;
		}
		if (!this.initialized)
		{
			this.OnAdd();
		}
		if (GameClient.IsCurrentFrame)
		{
			this.AssignAnimatingValueToMaterial();
		}
		return this.wrapAndContinue();
	}

	private void OnAdd()
	{
		this.initialized = true;
		if (!this.data.affectAllMaterialsOnModel)
		{
			this.targets.MarkActive(this.data.materialTargetId);
		}
		else
		{
			this.targets.MarkAllActive();
		}
	}

	public void OnDestroy()
	{
		this.OnRemove();
	}

	private void OnRemove()
	{
		if (!this.removed)
		{
			if (this.data.endCondition != (MaterialAnimationData.TriggerType)0)
			{
				this.signalBus.GetSignal<PlayerController.InteractionSignal>().RemoveListener(new Action<PlayerController.InteractionSignalData>(this.EndForNotification));
			}
			if (this.initialized)
			{
				this.removed = true;
				if (!this.data.affectAllMaterialsOnModel)
				{
					this.targets.Restore(this.data.materialTargetId);
				}
				else
				{
					this.targets.RestoreAll();
				}
			}
		}
	}

	private void AssignAnimatingValueToMaterial()
	{
		float num = Mathf.Min(1f, (float)this.currentFrame / (float)this.data.totalFrames);
		if (this.pingPongToggle)
		{
			num = 1f - num;
		}
		if (this.currentFrame < this.data.totalFrames)
		{
			this.currentFrame++;
		}
		if (this.data.valueType == MaterialAnimationData.Type.Float)
		{
			float value = this.data.curve.Evaluate(num);
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetFloat(this.data.materialTargetId, this.data.shaderVariableName, value);
			}
			else
			{
				this.targets.SetFloatAll(this.data.shaderVariableName, value);
			}
		}
		else if (this.data.valueType == MaterialAnimationData.Type.ColorTween)
		{
			float t = this.data.curve.Evaluate(num);
			Color value2 = Color.Lerp(this.cachedColor, this.cachedSecondaryColor, t);
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetColor(this.data.materialTargetId, this.data.shaderVariableName, value2);
			}
			else
			{
				this.targets.SetColorAll(this.data.shaderVariableName, value2);
			}
		}
		else if (this.data.valueType == MaterialAnimationData.Type.ColorGradient)
		{
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetColor(this.data.materialTargetId, this.data.shaderVariableName, this.cachedGradient.Evaluate(num));
			}
			else
			{
				this.targets.SetColorAll(this.data.shaderVariableName, this.cachedGradient.Evaluate(num));
			}
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Color)
		{
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetColor(this.data.materialTargetId, this.data.shaderVariableName, this.cachedColor);
			}
			else
			{
				this.targets.SetColorAll(this.data.shaderVariableName, this.cachedColor);
			}
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Texture)
		{
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetTexture(this.data.materialTargetId, this.data.shaderVariableName, this.cachedTexture);
			}
			else
			{
				this.targets.SetTextureAll(this.data.shaderVariableName, this.cachedTexture);
			}
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Material)
		{
			if (!this.data.affectAllMaterialsOnModel)
			{
				this.targets.SetMaterial(this.data.materialTargetId, this.data.shaderVariableName, this.cachedMaterial);
			}
			else
			{
				this.targets.SetMaterialAll(this.data.shaderVariableName, this.cachedMaterial, false);
			}
		}
	}

	private bool wrapAndContinue()
	{
		bool flag = this.currentFrame == this.data.totalFrames;
		if (this.data.wrap == MaterialAnimationData.WrapMode.None)
		{
			bool flag2 = flag;
			if (flag2)
			{
				this.OnRemove();
			}
			return !flag2;
		}
		if (this.data.wrap == MaterialAnimationData.WrapMode.Loop && flag)
		{
			this.currentFrame = 0;
		}
		else if (this.data.wrap == MaterialAnimationData.WrapMode.PingPong && flag)
		{
			this.currentFrame = 0;
			this.pingPongToggle = !this.pingPongToggle;
		}
		return true;
	}

	private void CacheValues()
	{
		if (this.data.valueType == MaterialAnimationData.Type.ColorTween)
		{
			this.cachedColor = this.targets.ColorForId(this.data.stringValue);
			this.cachedSecondaryColor = this.targets.ColorForId(this.data.stringValueSecondary);
		}
		else if (this.data.valueType == MaterialAnimationData.Type.ColorGradient)
		{
			this.cachedGradient = this.targets.GradientForId(this.data.stringValue);
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Color)
		{
			this.cachedColor = this.targets.ColorForId(this.data.stringValue);
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Texture)
		{
			this.cachedTexture = this.targets.TextureForId(this.data.stringValue);
		}
		else if (this.data.valueType == MaterialAnimationData.Type.Material)
		{
			this.cachedMaterial = new Material(this.targets.MaterialForId(this.data.stringValue));
		}
	}

	private bool _addLegacyCancelOptionEndFrames_m__0(MaterialAnimationData.EndFrameData ef)
	{
		return ef.move == this.moveOwner.MoveData.cancelOptions.landMove;
	}

	private bool _addLegacyCancelOptionEndFrames_m__1(MaterialAnimationData.EndFrameData ef)
	{
		return ef.move == this.moveOwner.MoveData.cancelOptions.fallMove;
	}
}
