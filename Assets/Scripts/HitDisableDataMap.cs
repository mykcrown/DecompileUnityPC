// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class HitDisableDataMap
{
	private Dictionary<int, MoveModel.HitDisableData> map = new Dictionary<int, MoveModel.HitDisableData>(8);

	private int totalInternalFrames;

	private List<Hit> hits = new List<Hit>();

	public bool disabledForAll;

	private List<int> playerIdBuffer = new List<int>(8);

	public void Clear()
	{
		this.map.Clear();
		this.totalInternalFrames = 0;
		this.hits.Clear();
		this.disabledForAll = false;
	}

	public void CopyTo(HitDisableDataMap target)
	{
		target.Clear();
		foreach (KeyValuePair<int, MoveModel.HitDisableData> current in this.map)
		{
			target.map[current.Key] = this.map[current.Key];
		}
		target.totalInternalFrames = this.totalInternalFrames;
		foreach (Hit current2 in this.hits)
		{
			target.hits.Add(current2);
		}
		target.disabledForAll = this.disabledForAll;
	}

	public void Renew(int previousMoveFrame, int internalFrame)
	{
		this.playerIdBuffer.Clear();
		foreach (int current in this.map.Keys)
		{
			this.playerIdBuffer.Add(current);
		}
		foreach (int current2 in this.playerIdBuffer)
		{
			switch (this.map[current2].disableType)
			{
			case HitDisableType.Basic:
				break;
			case HitDisableType.UntilNextGap:
				this.DisableUntilGap(current2, internalFrame);
				break;
			case HitDisableType.FixedFrameCount:
			{
				int nextEnabledFrame = this.map[current2].nextEnabledFrame;
				this.DisableForFrames(current2, internalFrame, nextEnabledFrame - previousMoveFrame);
				break;
			}
			case HitDisableType.AlwaysForVictim:
				this.DisableForPlayer(current2);
				break;
			default:
				UnityEngine.Debug.LogError("Unsupported Hit Disable type on Hit Disable Data: " + this.map[current2].disableType);
				break;
			}
		}
	}

	public void Init(int totalInternalFrames, List<Hit> hits, bool clearData = true)
	{
		if (clearData)
		{
			this.Clear();
		}
		this.totalInternalFrames = totalInternalFrames;
		this.hits.Clear();
		foreach (Hit current in hits)
		{
			this.hits.Add(current);
		}
	}

	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		if (other == null)
		{
			return !this.disabledForAll;
		}
		return !this.disabledForAll && (!this.map.ContainsKey(other.HitOwnerID) || this.map[other.HitOwnerID].nextEnabledFrame <= currentFrame);
	}

	public void Disable(HitData hitData, IHitOwner other, int currentFrame)
	{
		switch (hitData.disableType)
		{
		case HitDisableType.Basic:
		{
			int framesDisabled = hitData.endFrame + 1 - currentFrame;
			this.DisableForFrames(other.HitOwnerID, currentFrame, framesDisabled);
			break;
		}
		case HitDisableType.UntilNextGap:
			this.DisableUntilGap(other.HitOwnerID, currentFrame);
			break;
		case HitDisableType.FixedFrameCount:
			this.DisableForFrames(other.HitOwnerID, currentFrame, hitData.fixedDisabledFrames);
			break;
		case HitDisableType.AlwaysForVictim:
			this.DisableForPlayer(other.HitOwnerID);
			break;
		case HitDisableType.AlwaysForAll:
			this.DisableForAll();
			break;
		default:
			UnityEngine.Debug.LogWarning("Unhandled hitDisableType " + hitData.disableType);
			break;
		}
	}

	public void DisableForFrames(int hitOwnerId, int internalFrame, int framesDisabled)
	{
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.FixedFrameCount,
			nextEnabledFrame = internalFrame + framesDisabled
		};
		this.map[hitOwnerId] = value;
	}

	public void DisableForPlayer(int hitOwnerId)
	{
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.AlwaysForVictim,
			nextEnabledFrame = this.totalInternalFrames
		};
		this.map[hitOwnerId] = value;
	}

	public void DisableUntilGap(int hitOwnerId, int currentInternalFrame)
	{
		int num = 10;
		int num2 = currentInternalFrame;
		while (num2 < this.totalInternalFrames && num > 0)
		{
			bool flag = false;
			foreach (Hit current in this.hits)
			{
				if (num2 < current.data.endFrame && current.data.IsActiveOnFrame(num2 + 1))
				{
					num2 = current.data.endFrame;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			num--;
		}
		if (num == 0)
		{
			UnityEngine.Debug.LogError("Failed to properly resolve 'disable until gap': did not find a gap after 10 hits");
			num2 = currentInternalFrame;
		}
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.UntilNextGap,
			nextEnabledFrame = num2 + 1
		};
		this.map[hitOwnerId] = value;
	}

	public void DisableForAll()
	{
		this.disabledForAll = true;
	}
}
