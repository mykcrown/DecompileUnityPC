using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020004E8 RID: 1256
public class ResumeResumableCharge : MoveComponent, IMoveStartComponent, IMoveEndComponent, IMoveLinkProvider, IMoveSkipAheadComponent, IMoveOverrideChargeComponent
{
	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06001B6E RID: 7022 RVA: 0x0008B28C File Offset: 0x0008968C
	public bool ShouldSkipToFrame
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06001B6F RID: 7023 RVA: 0x0008B28F File Offset: 0x0008968F
	public int SkipToFrame
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06001B70 RID: 7024 RVA: 0x0008B292 File Offset: 0x00089692
	public bool ShouldSkipToMove
	{
		get
		{
			return this.chargeComponent != null && this.chargeComponent.IsFullyCharged;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06001B71 RID: 7025 RVA: 0x0008B2B3 File Offset: 0x000896B3
	public MoveData SkipToMove
	{
		get
		{
			return this.chargeThresholds[this.chargeThresholds.Count - 1].moveData;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06001B72 RID: 7026 RVA: 0x0008B2D4 File Offset: 0x000896D4
	public Fixed OverrideChargeFraction
	{
		get
		{
			int chargeFrames = this.chargeComponent.ChargeFrames;
			ChargeThresholdMoveData highestPriorityThreshold = this.getHighestPriorityThreshold(this.chargeThresholds);
			int num = this.chargeThresholds.IndexOf(highestPriorityThreshold);
			int chargeFramesNeeded = highestPriorityThreshold.chargeFramesNeeded;
			int num2 = this.chargeComponent.MaxChargeFrames;
			if (num < this.chargeThresholds.Count - 1)
			{
				num2 = this.chargeThresholds[num + 1].chargeFramesNeeded;
			}
			if (num2 - chargeFramesNeeded == 0)
			{
				return 1;
			}
			return (chargeFrames - chargeFramesNeeded) / (num2 - chargeFramesNeeded);
		}
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x0008B368 File Offset: 0x00089768
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		ListUtil.Sort<ChargeThresholdMoveData>(this.chargeThresholds, new ListUtil.LessThanDelegate<ChargeThresholdMoveData>(this.chargeThresholdMoveLessThan));
		this.chargeComponent = playerDelegate.GetCharacterComponent<ResumableChargeComponent>();
		if (this.chargeComponent == null)
		{
			throw new Exception("Character requires ResumableChargeComponent for ResumableCharge move components to work.");
		}
		this.chargeComponent.SetThresholds(this.chargeThresholds);
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0008B3CE File Offset: 0x000897CE
	private bool chargeThresholdMoveLessThan(ChargeThresholdMoveData a, ChargeThresholdMoveData b)
	{
		return a.chargeFramesNeeded < b.chargeFramesNeeded;
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x0008B3DE File Offset: 0x000897DE
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.chargeComponent.StartCharging();
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x0008B3EB File Offset: 0x000897EB
	public void OnEnd()
	{
		this.chargeComponent.EndCharging(false);
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x0008B3F9 File Offset: 0x000897F9
	public MoveData GetLinkedMove(InterruptData link)
	{
		return this.getHighestPriorityMove(this.chargeThresholds);
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x0008B408 File Offset: 0x00089808
	private MoveData getHighestPriorityMove(List<ChargeThresholdMoveData> thresholds)
	{
		ChargeThresholdMoveData highestPriorityThreshold = this.getHighestPriorityThreshold(thresholds);
		if (highestPriorityThreshold == null)
		{
			return null;
		}
		return highestPriorityThreshold.moveData;
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x0008B42C File Offset: 0x0008982C
	private ChargeThresholdMoveData getHighestPriorityThreshold(List<ChargeThresholdMoveData> thresholds)
	{
		if (thresholds == null || thresholds.Count == 0)
		{
			return null;
		}
		ChargeThresholdMoveData chargeThresholdMoveData = thresholds[0];
		for (int i = 1; i < thresholds.Count; i++)
		{
			ChargeThresholdMoveData chargeThresholdMoveData2 = thresholds[i];
			int chargeFrames = this.chargeComponent.ChargeFrames;
			if (chargeThresholdMoveData2.chargeFramesNeeded > chargeFrames)
			{
				break;
			}
			if (chargeThresholdMoveData2.chargeFramesNeeded > chargeThresholdMoveData.chargeFramesNeeded)
			{
				chargeThresholdMoveData = chargeThresholdMoveData2;
			}
		}
		return chargeThresholdMoveData;
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x0008B4A4 File Offset: 0x000898A4
	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ChargeThresholdMoveData chargeThresholdMoveData in this.chargeThresholds)
		{
			foreach (ParticleData particleData in chargeThresholdMoveData.particles)
			{
				particleData.RegisterPreload(context);
			}
		}
	}

	// Token: 0x040014BC RID: 5308
	public List<ChargeThresholdMoveData> chargeThresholds = new List<ChargeThresholdMoveData>();

	// Token: 0x040014BD RID: 5309
	private ResumableChargeComponent chargeComponent;
}
