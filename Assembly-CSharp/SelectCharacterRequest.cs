using System;

// Token: 0x02000A46 RID: 2630
[Serializable]
public class SelectCharacterRequest : UIEvent, IUIRequest
{
	// Token: 0x06004CF0 RID: 19696 RVA: 0x00145737 File Offset: 0x00143B37
	public SelectCharacterRequest(PlayerNum playerNum, CharacterID characterID)
	{
		this.playerNum = playerNum;
		this.characterID = characterID;
	}

	// Token: 0x06004CF1 RID: 19697 RVA: 0x0014574D File Offset: 0x00143B4D
	public SelectCharacterRequest(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
		this.useDefaultCharacter = true;
	}

	// Token: 0x0400326F RID: 12911
	public PlayerNum playerNum;

	// Token: 0x04003270 RID: 12912
	public CharacterID characterID;

	// Token: 0x04003271 RID: 12913
	public bool useDefaultCharacter;
}
