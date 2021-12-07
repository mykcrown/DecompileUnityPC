// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SelectCharacterRequest : UIEvent, IUIRequest
{
	public PlayerNum playerNum;

	public CharacterID characterID;

	public bool useDefaultCharacter;

	public SelectCharacterRequest(PlayerNum playerNum, CharacterID characterID)
	{
		this.playerNum = playerNum;
		this.characterID = characterID;
	}

	public SelectCharacterRequest(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
		this.useDefaultCharacter = true;
	}
}
