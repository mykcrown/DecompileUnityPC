// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class ProximityTrigger : StageTrigger
{
	public enum ProximityTriggerType
	{
		OnEnter,
		OnExit,
		OnEnterAndExit,
		Continuous
	}

	public enum ProximityTriggerShape
	{
		Circle,
		Rectangle
	}

	public ProximityTrigger.ProximityTriggerType Type;

	public GameObject TargetObject;

	public Vector2F OffsetFromTarget;

	public ProximityTrigger.ProximityTriggerShape Shape = ProximityTrigger.ProximityTriggerShape.Rectangle;

	public Fixed Width;

	public Fixed Height;

	public Fixed Radius;

	private ProximityTriggerModel model;

	private FixedRect bounds;

	public Vector3F TargetPosition
	{
		get
		{
			return (!(this.TargetObject == null)) ? ((Vector3F)this.TargetObject.transform.position) : Vector3F.zero;
		}
	}

	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.bounds = new FixedRect(this.TargetPosition + this.OffsetFromTarget, this.Width, this.Height);
		this.model = new ProximityTriggerModel();
		this.model.shouldValidate = isSimulation;
	}

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
		foreach (PlayerController current in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(current.PlayerNum, true);
			bool flag2 = this.model.playerInsideLookup[intFromPlayerNum];
			bool flag3 = false;
			ProximityTrigger.ProximityTriggerShape shape = this.Shape;
			if (shape != ProximityTrigger.ProximityTriggerShape.Circle)
			{
				if (shape == ProximityTrigger.ProximityTriggerShape.Rectangle)
				{
					flag3 = this.IsInsideRectangle(current.Position);
				}
			}
			else
			{
				flag3 = this.IsInsideCircle(current.Position);
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
				flag |= flag3;
				break;
			}
			if (flag)
			{
				playerNum = current.PlayerNum;
			}
			this.model.playerInsideLookup[intFromPlayerNum] = flag3;
		}
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(playerNum);
		}
	}

	private bool IsInsideRectangle(Vector3F point)
	{
		return this.bounds.ContainsPoint(point);
	}

	private bool IsInsideCircle(Vector3F point)
	{
		return (this.TargetPosition + this.OffsetFromTarget - point).sqrMagnitude <= this.Radius * this.Radius;
	}

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

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ProximityTriggerModel>(this.model));
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ProximityTriggerModel>(ref this.model);
		return true;
	}
}
