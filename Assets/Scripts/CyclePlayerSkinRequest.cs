// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CyclePlayerSkinRequest : UIEvent, IUIRequest
{
	public PlayerNum playerNum;

	public int direction;

	public CyclePlayerSkinRequest(PlayerNum playerNum, int direction = 1)
	{
		this.playerNum = playerNum;
		this.direction = direction;
	}
}
