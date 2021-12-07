// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CyclePlayerTeamRequest : UIEvent, IUIRequest
{
	public PlayerNum playerNum;

	public int direction;

	public CyclePlayerTeamRequest(PlayerNum playerNum, int direction = 1)
	{
		this.playerNum = playerNum;
		this.direction = direction;
	}
}
