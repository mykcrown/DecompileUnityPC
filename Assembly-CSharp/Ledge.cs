using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000630 RID: 1584
public class Ledge : StageProp
{
	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x060026E8 RID: 9960 RVA: 0x000BE5E7 File Offset: 0x000BC9E7
	public Vector3F Position
	{
		get
		{
			return this.model.position;
		}
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000BE5F4 File Offset: 0x000BC9F4
	public override bool IsSimulation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x000BE5F7 File Offset: 0x000BC9F7
	public override void Awake()
	{
		base.Awake();
		this.model = new LedgeModel();
		this.model.position = (Vector3F)base.transform.position;
		this.model.shouldValidate = this.IsSimulation;
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x000BE638 File Offset: 0x000BCA38
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

	// Token: 0x060026EC RID: 9964 RVA: 0x000BE694 File Offset: 0x000BCA94
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

	// Token: 0x060026ED RID: 9965 RVA: 0x000BE6F0 File Offset: 0x000BCAF0
	private void OnDrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			Color green = Color.green;
			GizmoUtil.GizmosDrawLedge(base.transform.position, this.facesRight, green, 4f);
		}
	}

	// Token: 0x060026EE RID: 9966 RVA: 0x000BE72F File Offset: 0x000BCB2F
	public override void TickFrame()
	{
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x000BE731 File Offset: 0x000BCB31
	private IPositionOwner getPositionOwner(PlayerNum player)
	{
		return base.gameManager.GetPlayerController(player);
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x000BE740 File Offset: 0x000BCB40
	public int GetPlayerSlot(PlayerNum player)
	{
		foreach (KeyValuePair<int, PlayerNum> keyValuePair in this.model.playerSlots)
		{
			if (keyValuePair.Value == player)
			{
				return keyValuePair.Key;
			}
		}
		return -1;
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x000BE7B8 File Offset: 0x000BCBB8
	public void AddPlayer(PlayerNum player, int slot)
	{
		this.model.playerSlots[slot] = player;
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x000BE7CC File Offset: 0x000BCBCC
	public void RemovePlayer(PlayerNum player)
	{
		foreach (KeyValuePair<int, PlayerNum> keyValuePair in this.model.playerSlots)
		{
			if (keyValuePair.Value == player)
			{
				this._willRemove.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this._willRemove)
		{
			this.model.playerSlots[key] = PlayerNum.None;
		}
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x000BE89C File Offset: 0x000BCC9C
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<LedgeModel>(this.model));
		return true;
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000BE8B8 File Offset: 0x000BCCB8
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<LedgeModel>(ref this.model);
		return true;
	}

	// Token: 0x04001C78 RID: 7288
	[FormerlySerializedAs("invert")]
	public bool facesRight;

	// Token: 0x04001C79 RID: 7289
	private LedgeModel model;

	// Token: 0x04001C7A RID: 7290
	private List<int> _willRemove = new List<int>(8);
}
