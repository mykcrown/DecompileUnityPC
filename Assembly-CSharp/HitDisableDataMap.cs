using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class HitDisableDataMap
{
	// Token: 0x06001C0D RID: 7181 RVA: 0x0008DA20 File Offset: 0x0008BE20
	public void Clear()
	{
		this.map.Clear();
		this.totalInternalFrames = 0;
		this.hits.Clear();
		this.disabledForAll = false;
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x0008DA48 File Offset: 0x0008BE48
	public void CopyTo(HitDisableDataMap target)
	{
		target.Clear();
		foreach (KeyValuePair<int, MoveModel.HitDisableData> keyValuePair in this.map)
		{
			target.map[keyValuePair.Key] = this.map[keyValuePair.Key];
		}
		target.totalInternalFrames = this.totalInternalFrames;
		foreach (Hit item in this.hits)
		{
			target.hits.Add(item);
		}
		target.disabledForAll = this.disabledForAll;
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x0008DB30 File Offset: 0x0008BF30
	public void Renew(int previousMoveFrame, int internalFrame)
	{
		this.playerIdBuffer.Clear();
		foreach (int item in this.map.Keys)
		{
			this.playerIdBuffer.Add(item);
		}
		foreach (int num in this.playerIdBuffer)
		{
			switch (this.map[num].disableType)
			{
			case HitDisableType.Basic:
				break;
			case HitDisableType.UntilNextGap:
				this.DisableUntilGap(num, internalFrame);
				break;
			case HitDisableType.FixedFrameCount:
			{
				int nextEnabledFrame = this.map[num].nextEnabledFrame;
				this.DisableForFrames(num, internalFrame, nextEnabledFrame - previousMoveFrame);
				break;
			}
			case HitDisableType.AlwaysForVictim:
				this.DisableForPlayer(num);
				break;
			default:
				Debug.LogError("Unsupported Hit Disable type on Hit Disable Data: " + this.map[num].disableType);
				break;
			}
		}
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x0008DC8C File Offset: 0x0008C08C
	public void Init(int totalInternalFrames, List<Hit> hits, bool clearData = true)
	{
		if (clearData)
		{
			this.Clear();
		}
		this.totalInternalFrames = totalInternalFrames;
		this.hits.Clear();
		foreach (Hit item in hits)
		{
			this.hits.Add(item);
		}
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x0008DD08 File Offset: 0x0008C108
	public bool IsActiveFor(IHitOwner other, int currentFrame)
	{
		if (other == null)
		{
			return !this.disabledForAll;
		}
		return !this.disabledForAll && (!this.map.ContainsKey(other.HitOwnerID) || this.map[other.HitOwnerID].nextEnabledFrame <= currentFrame);
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x0008DD6C File Offset: 0x0008C16C
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
			Debug.LogWarning("Unhandled hitDisableType " + hitData.disableType);
			break;
		}
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x0008DE24 File Offset: 0x0008C224
	public void DisableForFrames(int hitOwnerId, int internalFrame, int framesDisabled)
	{
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.FixedFrameCount,
			nextEnabledFrame = internalFrame + framesDisabled
		};
		this.map[hitOwnerId] = value;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x0008DE5C File Offset: 0x0008C25C
	public void DisableForPlayer(int hitOwnerId)
	{
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.AlwaysForVictim,
			nextEnabledFrame = this.totalInternalFrames
		};
		this.map[hitOwnerId] = value;
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x0008DE98 File Offset: 0x0008C298
	public void DisableUntilGap(int hitOwnerId, int currentInternalFrame)
	{
		int num = 10;
		int num2 = currentInternalFrame;
		while (num2 < this.totalInternalFrames && num > 0)
		{
			bool flag = false;
			foreach (Hit hit in this.hits)
			{
				if (num2 < hit.data.endFrame && hit.data.IsActiveOnFrame(num2 + 1))
				{
					num2 = hit.data.endFrame;
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
			Debug.LogError("Failed to properly resolve 'disable until gap': did not find a gap after 10 hits");
			num2 = currentInternalFrame;
		}
		MoveModel.HitDisableData value = new MoveModel.HitDisableData
		{
			disableType = HitDisableType.UntilNextGap,
			nextEnabledFrame = num2 + 1
		};
		this.map[hitOwnerId] = value;
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x0008DF94 File Offset: 0x0008C394
	public void DisableForAll()
	{
		this.disabledForAll = true;
	}

	// Token: 0x04001732 RID: 5938
	private Dictionary<int, MoveModel.HitDisableData> map = new Dictionary<int, MoveModel.HitDisableData>(8);

	// Token: 0x04001733 RID: 5939
	private int totalInternalFrames;

	// Token: 0x04001734 RID: 5940
	private List<Hit> hits = new List<Hit>();

	// Token: 0x04001735 RID: 5941
	public bool disabledForAll;

	// Token: 0x04001736 RID: 5942
	private List<int> playerIdBuffer = new List<int>(8);
}
