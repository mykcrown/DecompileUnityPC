using System;

// Token: 0x02000656 RID: 1622
public class SurfaceTrigger : StageTrigger
{
	// Token: 0x060027BA RID: 10170 RVA: 0x000C1A23 File Offset: 0x000BFE23
	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.model = new SurfaceTriggerModel();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x000C1A59 File Offset: 0x000BFE59
	private void onGameInit(GameEvent message)
	{
		if (this.model.playerOnSurface == null)
		{
			this.model.playerOnSurface = new bool[12];
		}
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000C1A80 File Offset: 0x000BFE80
	public override void TickFrame()
	{
		base.TickFrame();
		bool flag = false;
		foreach (PlayerController playerController in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerController.PlayerNum, true);
			bool flag2 = this.model.playerOnSurface[intFromPlayerNum];
			bool flag3 = playerController.Physics.GroundedMovingObject == this.Surface && playerController.Physics.IsGrounded;
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
				flag = (flag || flag3);
				break;
			}
			this.model.playerOnSurface[intFromPlayerNum] = flag3;
		}
		if (flag && this.Triggered != null)
		{
			base.CallTriggered(null);
		}
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000C1BCC File Offset: 0x000BFFCC
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<SurfaceTriggerModel>(this.model));
		return true;
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000C1BE8 File Offset: 0x000BFFE8
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<SurfaceTriggerModel>(ref this.model);
		return true;
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000C1BF8 File Offset: 0x000BFFF8
	public override void Destroy()
	{
		base.Destroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x04001D05 RID: 7429
	public StageSurface Surface;

	// Token: 0x04001D06 RID: 7430
	public SurfaceTrigger.SurfaceTriggerType Type;

	// Token: 0x04001D07 RID: 7431
	private SurfaceTriggerModel model;

	// Token: 0x02000657 RID: 1623
	public enum SurfaceTriggerType
	{
		// Token: 0x04001D09 RID: 7433
		OnLand,
		// Token: 0x04001D0A RID: 7434
		OnLeave,
		// Token: 0x04001D0B RID: 7435
		OnLandAndLeave,
		// Token: 0x04001D0C RID: 7436
		Continuous
	}
}
