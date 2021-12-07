using System;
using System.Collections.Generic;

// Token: 0x02000673 RID: 1651
public abstract class EndGameCondition : ITickable, IRollbackStateOwner
{
	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x060028C7 RID: 10439 RVA: 0x000C5644 File Offset: 0x000C3A44
	// (set) Token: 0x060028C8 RID: 10440 RVA: 0x000C564C File Offset: 0x000C3A4C
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x060028C9 RID: 10441 RVA: 0x000C5655 File Offset: 0x000C3A55
	// (set) Token: 0x060028CA RID: 10442 RVA: 0x000C565D File Offset: 0x000C3A5D
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x060028CB RID: 10443 RVA: 0x000C5666 File Offset: 0x000C3A66
	// (set) Token: 0x060028CC RID: 10444 RVA: 0x000C566E File Offset: 0x000C3A6E
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x060028CD RID: 10445 RVA: 0x000C5677 File Offset: 0x000C3A77
	protected virtual IEndGameConditionModel Model
	{
		get
		{
			return this.endGameConditionModel;
		}
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x060028CE RID: 10446 RVA: 0x000C567F File Offset: 0x000C3A7F
	public bool IsFinished
	{
		get
		{
			return this.Model.IsFinished;
		}
	}

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x060028CF RID: 10447 RVA: 0x000C568C File Offset: 0x000C3A8C
	public List<PlayerNum> Victors
	{
		get
		{
			return this.Model.Victors;
		}
	}

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x060028D0 RID: 10448 RVA: 0x000C5699 File Offset: 0x000C3A99
	public List<TeamNum> WinningTeams
	{
		get
		{
			return this.Model.WinningTeams;
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x060028D1 RID: 10449 RVA: 0x000C56A6 File Offset: 0x000C3AA6
	public virtual float CurrentSeconds
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000C56AD File Offset: 0x000C3AAD
	public virtual void TickFrame()
	{
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000C56AF File Offset: 0x000C3AAF
	public virtual void Destroy()
	{
	}

	// Token: 0x060028D4 RID: 10452
	public abstract bool ExportState(ref RollbackStateContainer container);

	// Token: 0x060028D5 RID: 10453
	public abstract bool LoadState(RollbackStateContainer container);

	// Token: 0x04001D92 RID: 7570
	protected EndGameConditionModel endGameConditionModel;
}
