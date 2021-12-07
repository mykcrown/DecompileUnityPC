// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class ResumeResumableCharge : MoveComponent, IMoveStartComponent, IMoveEndComponent, IMoveLinkProvider, IMoveSkipAheadComponent, IMoveOverrideChargeComponent
{
	public List<ChargeThresholdMoveData> chargeThresholds = new List<ChargeThresholdMoveData>();

	private ResumableChargeComponent chargeComponent;

	public bool ShouldSkipToFrame
	{
		get
		{
			return false;
		}
	}

	public int SkipToFrame
	{
		get
		{
			return 0;
		}
	}

	public bool ShouldSkipToMove
	{
		get
		{
			return this.chargeComponent != null && this.chargeComponent.IsFullyCharged;
		}
	}

	public MoveData SkipToMove
	{
		get
		{
			return this.chargeThresholds[this.chargeThresholds.Count - 1].moveData;
		}
	}

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

	private bool chargeThresholdMoveLessThan(ChargeThresholdMoveData a, ChargeThresholdMoveData b)
	{
		return a.chargeFramesNeeded < b.chargeFramesNeeded;
	}

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		this.chargeComponent.StartCharging();
	}

	public void OnEnd()
	{
		this.chargeComponent.EndCharging(false);
	}

	public MoveData GetLinkedMove(InterruptData link)
	{
		return this.getHighestPriorityMove(this.chargeThresholds);
	}

	private MoveData getHighestPriorityMove(List<ChargeThresholdMoveData> thresholds)
	{
		ChargeThresholdMoveData highestPriorityThreshold = this.getHighestPriorityThreshold(thresholds);
		if (highestPriorityThreshold == null)
		{
			return null;
		}
		return highestPriorityThreshold.moveData;
	}

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

	public override void RegisterPreload(PreloadContext context)
	{
		foreach (ChargeThresholdMoveData current in this.chargeThresholds)
		{
			foreach (ParticleData current2 in current.particles)
			{
				current2.RegisterPreload(context);
			}
		}
	}
}
