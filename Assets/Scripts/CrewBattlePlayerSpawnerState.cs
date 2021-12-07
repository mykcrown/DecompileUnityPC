// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class CrewBattlePlayerSpawnerState : RollbackStateTyped<CrewBattlePlayerSpawnerState>
{
	public int frameCount;

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<PlayerNum, int> assistsUsed = new Dictionary<PlayerNum, int>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<PlayerNum, int> lastAssistEndTime = new Dictionary<PlayerNum, int>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<TeamNum, int> lastAssistTeamEndTime = new Dictionary<TeamNum, int>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<TeamNum, int> lastAssistTeamStartTime = new Dictionary<TeamNum, int>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<PlayerNum, bool> didTagIn = new Dictionary<PlayerNum, bool>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<TeamNum, int> tagInFrames = new Dictionary<TeamNum, int>(8);

	[IgnoreCopyValidation, IsClonedManually]
	public Dictionary<TeamNum, PlayerNum> previousPrimaryPlayer = new Dictionary<TeamNum, PlayerNum>(8, default(TeamNumComparer));

	public CrewBattlePlayerSpawnerState()
	{
		this.tagInFrames[TeamNum.Team1] = 0;
		this.tagInFrames[TeamNum.Team2] = 0;
	}

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

	public override object Clone()
	{
		CrewBattlePlayerSpawnerState crewBattlePlayerSpawnerState = new CrewBattlePlayerSpawnerState();
		this.CopyTo(crewBattlePlayerSpawnerState);
		return crewBattlePlayerSpawnerState;
	}
}
