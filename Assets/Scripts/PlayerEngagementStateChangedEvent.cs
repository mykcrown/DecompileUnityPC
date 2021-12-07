// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PlayerEngagementStateChangedEvent : GameEvent
{
	public PlayerEngagementState engagement;

	public PlayerNum playerNum;

	public TeamNum team;

	public PlayerEngagementStateChangedEvent(PlayerNum player, TeamNum team, PlayerEngagementState engagement)
	{
		this.engagement = engagement;
		this.playerNum = player;
		this.team = team;
	}
}
