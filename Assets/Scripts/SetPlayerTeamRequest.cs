// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SetPlayerTeamRequest : UIEvent, IUIRequest
{
	public PlayerNum playerNum;

	public TeamNum teamNum;

	public bool playSound;

	public SetPlayerTeamRequest(PlayerNum playerNum, TeamNum teamNum, bool playSound = true)
	{
		this.playerNum = playerNum;
		this.teamNum = teamNum;
		this.playSound = playSound;
	}
}
