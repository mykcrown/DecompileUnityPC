using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class ProximityTrigger : StageTrigger
{
	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06002701 RID: 9985 RVA: 0x000BEC76 File Offset: 0x000BD076
	public Vector3F TargetPosition
	{
		get
		{
			return (!(this.TargetObject == null)) ? ((Vector3F)this.TargetObject.transform.position) : Vector3F.zero;
		}
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000BECA8 File Offset: 0x000BD0A8
	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.bounds = new FixedRect(this.TargetPosition + this.OffsetFromTarget, this.Width, this.Height);
		this.model = new ProximityTriggerModel();
		this.model.shouldValidate = isSimulation;
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000BED04 File Offset: 0x000BD104
	public override void TickFrame()
	{
		if (this.TargetObject != null)
		{
			this.bounds.position = (Vector2F)this.TargetObject.transform.position + this.OffsetFromTarget;
		}
		if (this.model.playerInsideLookup == null)
		{
			this.model.playerInsideLookup = new bool[12];
		}
		bool flag = false;
		PlayerNum playerNum = PlayerNum.None;
		foreach (PlayerController playerController in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerController.PlayerNum, true);
			bool flag2 = this.model.playerInsideLookup[intFromPlayerNum];
			bool flag3 = false;
			ProximityTrigger.ProximityTriggerShape shape = this.Shape;
			if (shape != ProximityTrigger.ProximityTriggerShape.Circle)
			{
				if (shape == ProximityTrigger.ProximityTriggerShape.Rectangle)
				{
					flag3 = this.IsInsideRectangle(playerController.Position);
				}
			}
			else
			{
				flag3 = this.IsInsideCircle(playerController.Position);
			}
			switch (this.Type)
			{
			case ProximityTrigger.ProximityTriggerType.OnEnter:
				flag |= (!flag2 && flag3);
				break;
			case ProximityTrigger.ProximityTriggerType.OnExit:
				flag |= (flag2 && !flag3);
				break;
			case ProximityTrigger.ProximityTriggerType.OnEnterAndExit:
				flag |= ((!flag2 && flag3) || (flag2 && !flag3));
				break;
			case ProximityTrigger.ProximityTriggerType.Continuous:
				flag = (flag || flag3);
				break;
			}
			if (flag)
			{
				playerNum = playerController.PlayerNum;
			}
			this.model.playerInsideLookup[intFromPlayerNum] = flag3;
		}
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(playerNum);
		}
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000BEEEC File Offset: 0x000BD2EC
	private bool IsInsideRectangle(Vector3F point)
	{
		return this.bounds.ContainsPoint(point);
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000BEF00 File Offset: 0x000BD300
	private bool IsInsideCircle(Vector3F point)
	{
		return (this.TargetPosition + this.OffsetFromTarget - point).sqrMagnitude <= this.Radius * this.Radius;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000BEF48 File Offset: 0x000BD348
	private void OnDrawGizmos()
	{
		Vector3 a = (!(this.TargetObject == null)) ? this.TargetObject.transform.position : Vector3.zero;
		ProximityTrigger.ProximityTriggerShape shape = this.Shape;
		if (shape != ProximityTrigger.ProximityTriggerShape.Circle)
		{
			if (shape == ProximityTrigger.ProximityTriggerShape.Rectangle)
			{
				GizmoUtil.GizmosDrawRectangle(new Rect(a + (Vector3)this.OffsetFromTarget - 0.5f * new Vector3((float)this.Width, (float)this.Height, 0f), new Vector2((float)this.Width, (float)this.Height)), Color.magenta, false);
			}
		}
		else
		{
			GizmoUtil.GizmosDrawCircle(a + (Vector3)this.OffsetFromTarget, (float)this.Radius, Color.magenta, 10);
		}
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000BF041 File Offset: 0x000BD441
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ProximityTriggerModel>(this.model));
		return true;
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x000BF05D File Offset: 0x000BD45D
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ProximityTriggerModel>(ref this.model);
		return true;
	}

	// Token: 0x04001C8E RID: 7310
	public ProximityTrigger.ProximityTriggerType Type;

	// Token: 0x04001C8F RID: 7311
	public GameObject TargetObject;

	// Token: 0x04001C90 RID: 7312
	public Vector2F OffsetFromTarget;

	// Token: 0x04001C91 RID: 7313
	public ProximityTrigger.ProximityTriggerShape Shape = ProximityTrigger.ProximityTriggerShape.Rectangle;

	// Token: 0x04001C92 RID: 7314
	public Fixed Width;

	// Token: 0x04001C93 RID: 7315
	public Fixed Height;

	// Token: 0x04001C94 RID: 7316
	public Fixed Radius;

	// Token: 0x04001C95 RID: 7317
	private ProximityTriggerModel model;

	// Token: 0x04001C96 RID: 7318
	private FixedRect bounds;

	// Token: 0x02000638 RID: 1592
	public enum ProximityTriggerType
	{
		// Token: 0x04001C98 RID: 7320
		OnEnter,
		// Token: 0x04001C99 RID: 7321
		OnExit,
		// Token: 0x04001C9A RID: 7322
		OnEnterAndExit,
		// Token: 0x04001C9B RID: 7323
		Continuous
	}

	// Token: 0x02000639 RID: 1593
	public enum ProximityTriggerShape
	{
		// Token: 0x04001C9D RID: 7325
		Circle,
		// Token: 0x04001C9E RID: 7326
		Rectangle
	}
}
