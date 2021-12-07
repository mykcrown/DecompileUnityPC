// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SetPlayerTypeRequest : GameEvent, IUIRequest
{
	public PlayerNum playerNum;

	public PlayerType playerType;

	public bool playSound;

	public SetPlayerTypeRequest(PlayerNum playerNum, PlayerType playerType, bool playSound = true)
	{
		this.playerNum = playerNum;
		this.playerType = playerType;
		this.playSound = playSound;
	}
}
