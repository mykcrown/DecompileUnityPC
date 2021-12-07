// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ledge : StageProp
{
	[FormerlySerializedAs("invert")]
	public bool facesRight;

	private LedgeModel model;

	private List<int> _willRemove = new List<int>(8);

	public Vector3F Position
	{
		get
		{
			return this.model.position;
		}
	}

	public override bool IsSimulation
	{
		get
		{
			return true;
		}
	}

	public override void Awake()
	{
		base.Awake();
		this.model = new LedgeModel();
		this.model.position = (Vector3F)base.transform.position;
		this.model.shouldValidate = this.IsSimulation;
	}

	public bool IsOccupied()
	{
		if (this.model != null)
		{
			for (int i = 0; i < base.config.maxPlayers; i++)
			{
				PlayerNum playerNum = PlayerNum.None;
				bool flag = this.model.playerSlots.TryGetValue(i, out playerNum);
				if (flag && playerNum != PlayerNum.None)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetOpenSlot()
	{
		if (this.model != null)
		{
			for (int i = 0; i < base.config.maxPlayers; i++)
			{
				PlayerNum playerNum = PlayerNum.None;
				bool flag = this.model.playerSlots.TryGetValue(i, out playerNum);
				if (!flag || playerNum == PlayerNum.None)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private void OnDrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			Color green = Color.green;
			GizmoUtil.GizmosDrawLedge(base.transform.position, this.facesRight, green, 4f);
		}
	}

	public override void TickFrame()
	{
	}

	private IPositionOwner getPositionOwner(PlayerNum player)
	{
		return base.gameManager.GetPlayerController(player);
	}

	public int GetPlayerSlot(PlayerNum player)
	{
		foreach (KeyValuePair<int, PlayerNum> current in this.model.playerSlots)
		{
			if (current.Value == player)
			{
				return current.Key;
			}
		}
		return -1;
	}

	public void AddPlayer(PlayerNum player, int slot)
	{
		this.model.playerSlots[slot] = player;
	}

	public void RemovePlayer(PlayerNum player)
	{
		foreach (KeyValuePair<int, PlayerNum> current in this.model.playerSlots)
		{
			if (current.Value == player)
			{
				this._willRemove.Add(current.Key);
			}
		}
		foreach (int current2 in this._willRemove)
		{
			this.model.playerSlots[current2] = PlayerNum.None;
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<LedgeModel>(this.model));
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<LedgeModel>(ref this.model);
		return true;
	}
}
