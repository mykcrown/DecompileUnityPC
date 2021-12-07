using System;
using System.Collections.Generic;

// Token: 0x0200067B RID: 1659
[Serializable]
public class TimeEndGameConditionModel : RollbackStateTyped<TimeEndGameConditionModel>, IEndGameConditionModel
{
	// Token: 0x060028FE RID: 10494 RVA: 0x000C6170 File Offset: 0x000C4570
	public override void CopyTo(TimeEndGameConditionModel target)
	{
		target.isFinished = this.isFinished;
		base.copyList<TeamNum>(this.winningTeams, target.winningTeams);
		base.copyList<PlayerNum>(this.victors, target.victors);
		for (int i = 0; i < this.scores.Length; i++)
		{
			target.scores[i] = this.scores[i];
		}
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000C61D8 File Offset: 0x000C45D8
	public override object Clone()
	{
		TimeEndGameConditionModel timeEndGameConditionModel = new TimeEndGameConditionModel();
		this.CopyTo(timeEndGameConditionModel);
		return timeEndGameConditionModel;
	}

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06002900 RID: 10496 RVA: 0x000C61F3 File Offset: 0x000C45F3
	// (set) Token: 0x06002901 RID: 10497 RVA: 0x000C61FB File Offset: 0x000C45FB
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

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06002902 RID: 10498 RVA: 0x000C6204 File Offset: 0x000C4604
	public List<PlayerNum> Victors
	{
		get
		{
			return this.victors;
		}
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06002903 RID: 10499 RVA: 0x000C620C File Offset: 0x000C460C
	public List<TeamNum> WinningTeams
	{
		get
		{
			return this.winningTeams;
		}
	}

	// Token: 0x04001DB5 RID: 7605
	public bool isFinished;

	// Token: 0x04001DB6 RID: 7606
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<PlayerNum> victors = new List<PlayerNum>(8);

	// Token: 0x04001DB7 RID: 7607
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<TeamNum> winningTeams = new List<TeamNum>(8);

	// Token: 0x04001DB8 RID: 7608
	[IsClonedManually(IsClonedManuallyType.ShouldAutomate)]
	[IgnoreCopyValidation]
	public int[] scores = new int[8];
}
