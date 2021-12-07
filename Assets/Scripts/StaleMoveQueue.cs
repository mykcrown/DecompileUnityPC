// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class StaleMoveQueue : IRollbackStateOwner
{
	private StaleMoveQueueState state = new StaleMoveQueueState();

	private StaleMoveQueueConfig config;

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public void Init(StaleMoveQueueConfig config)
	{
		this.config = config;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StaleMoveQueueState>(ref this.state);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<StaleMoveQueueState>(this.state));
		return true;
	}

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
			UnityEngine.Debug.Log("Total reduction for move " + move.label + " was more than 100!");
			num = 100;
		}
		return (100 - num) / 100;
	}

	public void Clear()
	{
		this.state.staleMoves.Clear();
	}

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
}
