// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SurfaceTrigger : StageTrigger
{
	public enum SurfaceTriggerType
	{
		OnLand,
		OnLeave,
		OnLandAndLeave,
		Continuous
	}

	public StageSurface Surface;

	public SurfaceTrigger.SurfaceTriggerType Type;

	private SurfaceTriggerModel model;

	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.model = new SurfaceTriggerModel();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	private void onGameInit(GameEvent message)
	{
		if (this.model.playerOnSurface == null)
		{
			this.model.playerOnSurface = new bool[12];
		}
	}

	public override void TickFrame()
	{
		base.TickFrame();
		bool flag = false;
		foreach (PlayerController current in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(current.PlayerNum, true);
			bool flag2 = this.model.playerOnSurface[intFromPlayerNum];
			bool flag3 = current.Physics.GroundedMovingObject == this.Surface && current.Physics.IsGrounded;
			switch (this.Type)
			{
			case SurfaceTrigger.SurfaceTriggerType.OnLand:
				flag |= (!flag2 && flag3);
				break;
			case SurfaceTrigger.SurfaceTriggerType.OnLeave:
				flag |= (flag2 && !flag3);
				break;
			case SurfaceTrigger.SurfaceTriggerType.OnLandAndLeave:
				flag |= ((!flag2 && flag3) || (flag2 && !flag3));
				break;
			case SurfaceTrigger.SurfaceTriggerType.Continuous:
				flag |= flag3;
				break;
			}
			this.model.playerOnSurface[intFromPlayerNum] = flag3;
		}
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(null);
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<SurfaceTriggerModel>(this.model));
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<SurfaceTriggerModel>(ref this.model);
		return true;
	}

	public override void Destroy()
	{
		base.Destroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}
}
