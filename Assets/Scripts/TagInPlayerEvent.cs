// Decompile from assembly: Assembly-CSharp.dll

using System;

public class TagInPlayerEvent : GameEvent
{
	public PlayerNum taggedPlayerNum;

	public TeamNum team;

	public TagInPlayerEvent(TeamNum team, PlayerNum taggedPlayerNum)
	{
		this.team = team;
		this.taggedPlayerNum = taggedPlayerNum;
	}
}
