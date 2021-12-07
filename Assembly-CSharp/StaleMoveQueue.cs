using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000663 RID: 1635
public class StaleMoveQueue : IRollbackStateOwner
{
	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06002800 RID: 10240 RVA: 0x000C28EA File Offset: 0x000C0CEA
	// (set) Token: 0x06002801 RID: 10241 RVA: 0x000C28F2 File Offset: 0x000C0CF2
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06002802 RID: 10242 RVA: 0x000C28FB File Offset: 0x000C0CFB
	public void Init(StaleMoveQueueConfig config)
	{
		this.config = config;
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000C2904 File Offset: 0x000C0D04
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StaleMoveQueueState>(ref this.state);
		return true;
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000C2914 File Offset: 0x000C0D14
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<StaleMoveQueueState>(this.state));
		return true;
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000C2930 File Offset: 0x000C0D30
	public void OnMoveHit(MoveLabel label, string name, int uid)
	{
		if (label == MoveLabel.None)
		{
			return;
		}
		for (int i = 0; i < this.state.staleMoves.Count; i++)
		{
			StaleEntry staleEntry = this.state.staleMoves[i];
			if (uid == staleEntry.uid)
			{
				return;
			}
		}
		this.state.staleMoves.Insert(0, new StaleEntry(label, name, uid));
		while (this.state.staleMoves.Count > this.config.queueSize)
		{
			this.state.staleMoves.RemoveAt(this.state.staleMoves.Count - 1);
		}
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x000C29E4 File Offset: 0x000C0DE4
	public Fixed GetDamageMultiplier(MoveData move)
	{
		int num = 0;
		for (int i = 0; i < this.state.staleMoves.Count; i++)
		{
			StaleEntry staleEntry = this.state.staleMoves[i];
			if (move.label == staleEntry.label && (string.IsNullOrEmpty(move.moveName) || move.moveName == staleEntry.name))
			{
				num += this.config.queueReductionPercent[i];
			}
		}
		if (num > 100)
		{
			Debug.Log("Total reduction for move " + move.label + " was more than 100!");
			num = 100;
		}
		return (100 - num) / 100;
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x000C2AB0 File Offset: 0x000C0EB0
	public void Clear()
	{
		this.state.staleMoves.Clear();
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x000C2AC4 File Offset: 0x000C0EC4
	public string GenerateDebugString()
	{
		string text = string.Empty;
		for (int i = 0; i < this.config.queueSize; i++)
		{
			text = text + (i + 1) + ". ";
			if (this.state.staleMoves.Count <= i)
			{
				text += "(empty)";
			}
			else
			{
				text += this.state.staleMoves[i].label;
			}
			if (i != this.config.queueSize - 1)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x04001D34 RID: 7476
	private StaleMoveQueueState state = new StaleMoveQueueState();

	// Token: 0x04001D35 RID: 7477
	private StaleMoveQueueConfig config;
}
