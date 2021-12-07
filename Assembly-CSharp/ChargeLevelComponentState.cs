using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
[Serializable]
public class ChargeLevelComponentState : RollbackStateTyped<ChargeLevelComponentState>
{
	// Token: 0x060020C1 RID: 8385 RVA: 0x000A4978 File Offset: 0x000A2D78
	public override void CopyTo(ChargeLevelComponentState target)
	{
		target.lastChargeGainFrame = this.lastChargeGainFrame;
		target.lastChargeLossFrame = this.lastChargeLossFrame;
		target.currentChargeLevel = this.currentChargeLevel;
		target.damageTowardsNextCharge = this.damageTowardsNextCharge;
		target.damageTowardsChargeLoss = this.damageTowardsChargeLoss;
		target.hasWarnedChargeLoss = this.hasWarnedChargeLoss;
		target.frameDamage.Clear();
		for (int i = 0; i < this.frameDamage.Count; i++)
		{
			target.frameDamage.Add(this.frameDamage[i]);
		}
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000A4A0C File Offset: 0x000A2E0C
	public override object Clone()
	{
		ChargeLevelComponentState chargeLevelComponentState = new ChargeLevelComponentState();
		this.CopyTo(chargeLevelComponentState);
		return chargeLevelComponentState;
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x000A4A27 File Offset: 0x000A2E27
	public override int GetHashCode()
	{
		Debug.Log("My hash code " + base.GetHashCode());
		return 0;
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000A4A44 File Offset: 0x000A2E44
	public override void Clear()
	{
		base.Clear();
		this.frameDamage.Clear();
		this.currentChargeLevel = 0;
		this.lastChargeGainFrame = 0;
		this.lastChargeLossFrame = 0;
		this.damageTowardsChargeLoss = 0;
	}

	// Token: 0x040019F0 RID: 6640
	public int lastChargeGainFrame;

	// Token: 0x040019F1 RID: 6641
	public int lastChargeLossFrame;

	// Token: 0x040019F2 RID: 6642
	public Fixed currentChargeLevel = 0;

	// Token: 0x040019F3 RID: 6643
	public Fixed damageTowardsNextCharge = 0;

	// Token: 0x040019F4 RID: 6644
	public bool hasWarnedChargeLoss;

	// Token: 0x040019F5 RID: 6645
	public Fixed damageTowardsChargeLoss = 0;

	// Token: 0x040019F6 RID: 6646
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<FrameDamage> frameDamage = new List<FrameDamage>(64);
}
