using System;

// Token: 0x02000A49 RID: 2633
public class HighlightCharacterEvent : UIEvent
{
	// Token: 0x06004CF4 RID: 19700 RVA: 0x0014577A File Offset: 0x00143B7A
	public HighlightCharacterEvent(PlayerNum playerNum, CharacterDefinition characterDef)
	{
		this.playerNum = playerNum;
		this.characterDef = characterDef;
	}

	// Token: 0x04003273 RID: 12915
	public PlayerNum playerNum;

	// Token: 0x04003274 RID: 12916
	public CharacterDefinition characterDef;
}
