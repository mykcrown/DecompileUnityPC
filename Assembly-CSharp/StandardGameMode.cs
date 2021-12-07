using System;
using System.Collections.Generic;

// Token: 0x020004B4 RID: 1204
public class StandardGameMode : GameModeBase, IGameMode, ITickable, IRollbackStateOwner
{
	// Token: 0x06001AAD RID: 6829 RVA: 0x00089414 File Offset: 0x00087814
	public override void Init(GameModeData modeData, ConfigData config, BattleSettings settings, IEvents events, List<PlayerReference> playerReferences, IFrameOwner frameOwner)
	{
		base.Init(modeData, config, settings, events, playerReferences, frameOwner);
		this.timeCondition = new TimeEndGameCondition();
		base.injector.Inject(this.timeCondition);
		this.timeCondition.Init(settings);
		this.livesCondition = new LivesEndGameCondition();
		base.injector.Inject(this.livesCondition);
		this.livesCondition.Init(settings);
		this.EndGameConditions.Add(this.livesCondition);
		this.EndGameConditions.Add(this.timeCondition);
		this.modeInputProcessor = new LivesModeInputProcessor();
		(this.modeInputProcessor as GameModeInputProcessor).Start(modeData, config, events);
		base.injector.Inject(this.modeInputProcessor);
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06001AAE RID: 6830 RVA: 0x000894D2 File Offset: 0x000878D2
	public override float CurrentSeconds
	{
		get
		{
			return this.timeCondition.CurrentSeconds;
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06001AAF RID: 6831 RVA: 0x000894DF File Offset: 0x000878DF
	public override bool ShouldDisplayTimer
	{
		get
		{
			return this.timeCondition.ShouldDisplayTimer;
		}
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000894EC File Offset: 0x000878EC
	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		this.timeCondition.ExportState(ref container);
		this.livesCondition.ExportState(ref container);
		return true;
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x00089511 File Offset: 0x00087911
	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		this.timeCondition.LoadState(container);
		this.livesCondition.LoadState(container);
		return true;
	}

	// Token: 0x040013ED RID: 5101
	private TimeEndGameCondition timeCondition;

	// Token: 0x040013EE RID: 5102
	private LivesEndGameCondition livesCondition;
}
