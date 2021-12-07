// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ComboKillEvent : GameEvent
{
	public PlayerNum killedPlayer;

	public TeamNum killedTeam;

	public PlayerNum comboerPlayer;

	public TeamNum comboerTeam;

	public ComboKillEvent(PlayerNum killedPlayer, TeamNum killedTeam, PlayerNum comboerPlayer, TeamNum comboerTeam)
	{
		this.killedPlayer = killedPlayer;
		this.killedTeam = killedTeam;
		this.comboerPlayer = comboerPlayer;
		this.comboerTeam = comboerTeam;
	}
}
