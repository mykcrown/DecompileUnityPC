using System;
using System.Collections.Generic;

// Token: 0x020004A2 RID: 1186
[Serializable]
public class CrewBattlePlayerSpawnerState : RollbackStateTyped<CrewBattlePlayerSpawnerState>
{
	// Token: 0x06001A10 RID: 6672 RVA: 0x00086DB8 File Offset: 0x000851B8
	public CrewBattlePlayerSpawnerState()
	{
		this.tagInFrames[TeamNum.Team1] = 0;
		this.tagInFrames[TeamNum.Team2] = 0;
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x00086E48 File Offset: 0x00085248
	public override void CopyTo(CrewBattlePlayerSpawnerState target)
	{
		target.frameCount = this.frameCount;
		base.copyDictionary<PlayerNum, int>(this.assistsUsed, target.assistsUsed);
		base.copyDictionary<PlayerNum, int>(this.lastAssistEndTime, target.lastAssistEndTime);
		base.copyDictionary<TeamNum, int>(this.lastAssistTeamEndTime, target.lastAssistTeamEndTime);
		base.copyDictionary<TeamNum, int>(this.lastAssistTeamStartTime, target.lastAssistTeamStartTime);
		base.copyDictionary<PlayerNum, bool>(this.didTagIn, target.didTagIn);
		base.copyDictionary<TeamNum, int>(this.tagInFrames, target.tagInFrames);
		base.copyDictionary<TeamNum, PlayerNum>(this.previousPrimaryPlayer, target.previousPrimaryPlayer);
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x00086EE0 File Offset: 0x000852E0
	public override object Clone()
	{
		CrewBattlePlayerSpawnerState crewBattlePlayerSpawnerState = new CrewBattlePlayerSpawnerState();
		this.CopyTo(crewBattlePlayerSpawnerState);
		return crewBattlePlayerSpawnerState;
	}

	// Token: 0x0400137C RID: 4988
	public int frameCount;

	// Token: 0x0400137D RID: 4989
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<PlayerNum, int> assistsUsed = new Dictionary<PlayerNum, int>(8);

	// Token: 0x0400137E RID: 4990
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<PlayerNum, int> lastAssistEndTime = new Dictionary<PlayerNum, int>(8);

	// Token: 0x0400137F RID: 4991
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<TeamNum, int> lastAssistTeamEndTime = new Dictionary<TeamNum, int>(8);

	// Token: 0x04001380 RID: 4992
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<TeamNum, int> lastAssistTeamStartTime = new Dictionary<TeamNum, int>(8);

	// Token: 0x04001381 RID: 4993
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<PlayerNum, bool> didTagIn = new Dictionary<PlayerNum, bool>(8);

	// Token: 0x04001382 RID: 4994
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<TeamNum, int> tagInFrames = new Dictionary<TeamNum, int>(8);

	// Token: 0x04001383 RID: 4995
	[IsClonedManually]
	[IgnoreCopyValidation]
	public Dictionary<TeamNum, PlayerNum> previousPrimaryPlayer = new Dictionary<TeamNum, PlayerNum>(8, default(TeamNumComparer));
}
