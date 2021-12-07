// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChargeLevelComponentState : RollbackStateTyped<ChargeLevelComponentState>
{
	public int lastChargeGainFrame;

	public int lastChargeLossFrame;

	public Fixed currentChargeLevel = 0;

	public Fixed damageTowardsNextCharge = 0;

	public bool hasWarnedChargeLoss;

	public Fixed damageTowardsChargeLoss = 0;

	[IgnoreCopyValidation, IsClonedManually]
	public List<FrameDamage> frameDamage = new List<FrameDamage>(64);

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

	public override object Clone()
	{
		ChargeLevelComponentState chargeLevelComponentState = new ChargeLevelComponentState();
		this.CopyTo(chargeLevelComponentState);
		return chargeLevelComponentState;
	}

	public override int GetHashCode()
	{
		UnityEngine.Debug.Log("My hash code " + base.GetHashCode());
		return 0;
	}

	public override void Clear()
	{
		base.Clear();
		this.frameDamage.Clear();
		this.currentChargeLevel = 0;
		this.lastChargeGainFrame = 0;
		this.lastChargeLossFrame = 0;
		this.damageTowardsChargeLoss = 0;
	}
}
