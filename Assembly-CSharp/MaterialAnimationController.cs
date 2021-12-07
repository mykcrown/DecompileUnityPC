using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class MaterialAnimationController
{
	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x0600168E RID: 5774 RVA: 0x0007A111 File Offset: 0x00078511
	// (set) Token: 0x0600168F RID: 5775 RVA: 0x0007A119 File Offset: 0x00078519
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06001690 RID: 5776 RVA: 0x0007A124 File Offset: 0x00078524
	public void Init(MaterialTargetsData targets, MaterialAnimationData data, IMoveOwner moveOwner)
	{
		this.targets = targets;
		this.data = data;
		this.moveOwner = moveOwner;
		bool flag = (MaterialAnimationData.TriggerType.MoveFrame & data.endCondition) != (MaterialAnimationData.TriggerType)0;
		if (flag)
		{
			foreach (MaterialAnimationData.EndFrameData endFrameData in data.endFrames)
			{
				if (endFrameData.move == null || endFrameData.move == moveOwner.MoveData)
				{
					this.endFrames.Add(new MaterialAnimationData.EndFrameData
					{
						move = moveOwner.MoveData,
						frame = endFrameData.frame
					});
					this.addLegacyCancelOptionEndFrames(endFrameData);
					this.addMoveTransitionCancelOptionEndFrames(endFrameData);
				}
				else
				{
					this.endFrames.Add(endFrameData);
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

	// Token: 0x06001691 RID: 5777 RVA: 0x0007A248 File Offset: 0x00078648
	private void addLegacyCancelOptionEndFrames(MaterialAnimationData.EndFrameData endFrame)
	{
		if (this.moveOwner.MoveData.cancelOptions.cancelOnLand && this.moveOwner.MoveData.cancelOptions.startLandMoveAtCurrentFrame && this.moveOwner.MoveData.cancelOptions.landMove != null && this.data.endFrames.FindIndex((MaterialAnimationData.EndFrameData ef) => ef.move == this.moveOwner.MoveData.cancelOptions.landMove) == -1)
		{
			this.endFrames.Add(new MaterialAnimationData.EndFrameData
			{
				move = this.moveOwner.MoveData.cancelOptions.landMove,
				frame = endFrame.frame
			});
		}
		if (this.moveOwner.MoveData.cancelOptions.cancelOnFall && this.moveOwner.MoveData.cancelOptions.startFallMoveAtCurrentFrame && this.moveOwner.MoveData.cancelOptions.fallMove != null && this.data.endFrames.FindIndex((MaterialAnimationData.EndFrameData ef) => ef.move == this.moveOwner.MoveData.cancelOptions.fallMove) == -1)
		{
			this.endFrames.Add(new MaterialAnimationData.EndFrameData
			{
				move = this.moveOwner.MoveData.cancelOptions.fallMove,
				frame = endFrame.frame
			});
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0007A3B4 File Offset: 0x000787B4
	private void addMoveTransitionCancelOptionEndFrames(MaterialAnimationData.EndFrameData endFrame)
	{
		for (int i = 0; i < this.moveOwner.MoveData.interrupts.Length; i++)
		{
			InterruptData interruptData = this.moveOwner.MoveData.interrupts[i];
			if ((interruptData.triggerType == InterruptTriggerType.OnLand || interruptData.triggerType == InterruptTriggerType.OnFall) && interruptData.startMoveTransitionAtCurrentFrame && interruptData.linkableMoves.Length > 0)
			{
				MoveData move = interruptData.linkableMoves[0];
				if (move.totalInternalFrames >= this.moveOwner.MoveData.totalInternalFrames && this.data.endFrames.FindIndex((MaterialAnimationData.EndFrameData ef) => ef.move == move) == -1)
				{
					this.endFrames.Add(new MaterialAnimationData.EndFrameData
					{
						move = move,
						frame = endFrame.frame
					});
				}
			}
		}
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x0007A4AC File Offset: 0x000788AC
	public void EndForNotification(PlayerController.InteractionSignalData data)
	{
		MaterialAnimationData.TriggerType type;
		if (this.moveOwner == data.moveOwner && MaterialAnimationController.NotificationTypeMap.TryGetValue(data.trigger, out type))
		{
			this.EndForNotification(type);
		}
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x0007A4EC File Offset: 0x000788EC
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

	// Token: 0x06001695 RID: 5781 RVA: 0x0007A598 File Offset: 0x00078998
	public bool TickFrame()
	{
		bool flag = (MaterialAnimationData.TriggerType.MoveFrame & this.data.endCondition) != (MaterialAnimationData.TriggerType)0;
		if (flag && this.moveOwner.MoveIsValid)
		{
			foreach (MaterialAnimationData.EndFrameData endFrameData in this.endFrames)
			{
				if (endFrameData.move == this.moveOwner.MoveData && this.moveOwner.InternalFrame >= endFrameData.frame)
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

	// Token: 0x06001696 RID: 5782 RVA: 0x0007A69C File Offset: 0x00078A9C
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

	// Token: 0x06001697 RID: 5783 RVA: 0x0007A6DB File Offset: 0x00078ADB
	public void OnDestroy()
	{
		this.OnRemove();
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0007A6E4 File Offset: 0x00078AE4
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

	// Token: 0x06001699 RID: 5785 RVA: 0x0007A770 File Offset: 0x00078B70
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

	// Token: 0x0600169A RID: 5786 RVA: 0x0007AA94 File Offset: 0x00078E94
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

	// Token: 0x0600169B RID: 5787 RVA: 0x0007AB2C File Offset: 0x00078F2C
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

	// Token: 0x04001147 RID: 4423
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

	// Token: 0x04001148 RID: 4424
	private IMoveOwner moveOwner;

	// Token: 0x04001149 RID: 4425
	private List<MaterialAnimationData.EndFrameData> endFrames = new List<MaterialAnimationData.EndFrameData>(10);

	// Token: 0x0400114A RID: 4426
	private Texture cachedTexture;

	// Token: 0x0400114B RID: 4427
	private Material cachedMaterial;

	// Token: 0x0400114C RID: 4428
	private Color cachedColor;

	// Token: 0x0400114D RID: 4429
	private Color cachedSecondaryColor;

	// Token: 0x0400114E RID: 4430
	private ColorGradientId cachedGradient;

	// Token: 0x0400114F RID: 4431
	public bool PendingRemoval;

	// Token: 0x04001150 RID: 4432
	public MaterialAnimationData data;

	// Token: 0x04001151 RID: 4433
	private bool removed;

	// Token: 0x04001152 RID: 4434
	private bool initialized;

	// Token: 0x04001153 RID: 4435
	private MaterialTargetsData targets;

	// Token: 0x04001154 RID: 4436
	private int currentFrame;

	// Token: 0x04001155 RID: 4437
	private bool pingPongToggle;
}
