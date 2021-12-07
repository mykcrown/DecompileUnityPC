// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CycleCharacterIndex : UIEvent, IUIRequest
{
	public PlayerNum playerNum;

	public CycleCharacterIndex(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}
}
