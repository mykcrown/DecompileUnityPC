using System;
using System.Collections.Generic;

// Token: 0x02000672 RID: 1650
[Serializable]
public class EndGameConditionModel : RollbackStateTyped<EndGameConditionModel>, IEndGameConditionModel
{
	// Token: 0x060028C0 RID: 10432 RVA: 0x000C55CB File Offset: 0x000C39CB
	public override void CopyTo(EndGameConditionModel target)
	{
		target.isFinished = this.isFinished;
		base.copyList<TeamNum>(this.winningTeams, target.winningTeams);
		base.copyList<PlayerNum>(this.victors, target.victors);
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000C5600 File Offset: 0x000C3A00
	public override object Clone()
	{
		EndGameConditionModel endGameConditionModel = new EndGameConditionModel();
		this.CopyTo(endGameConditionModel);
		return endGameConditionModel;
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000C561B File Offset: 0x000C3A1B
	// (set) Token: 0x060028C3 RID: 10435 RVA: 0x000C5623 File Offset: 0x000C3A23
	public bool IsFinished
	{
		get
		{
			return this.isFinished;
		}
		set
		{
			this.isFinished = value;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000C562C File Offset: 0x000C3A2C
	public List<PlayerNum> Victors
	{
		get
		{
			return this.victors;
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000C5634 File Offset: 0x000C3A34
	public List<TeamNum> WinningTeams
	{
		get
		{
			return this.winningTeams;
		}
	}

	// Token: 0x04001D8C RID: 7564
	public bool isFinished;

	// Token: 0x04001D8D RID: 7565
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<PlayerNum> victors = new List<PlayerNum>(8);

	// Token: 0x04001D8E RID: 7566
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<TeamNum> winningTeams = new List<TeamNum>(8);
}
