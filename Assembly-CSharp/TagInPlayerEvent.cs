using System;

// Token: 0x02000AEB RID: 2795
public class TagInPlayerEvent : GameEvent
{
	// Token: 0x060050D1 RID: 20689 RVA: 0x001508A7 File Offset: 0x0014ECA7
	public TagInPlayerEvent(TeamNum team, PlayerNum taggedPlayerNum)
	{
		this.team = team;
		this.taggedPlayerNum = taggedPlayerNum;
	}

	// Token: 0x0400342D RID: 13357
	public PlayerNum taggedPlayerNum;

	// Token: 0x0400342E RID: 13358
	public TeamNum team;
}
