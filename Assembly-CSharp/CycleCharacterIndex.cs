using System;

// Token: 0x02000A50 RID: 2640
[Serializable]
public class CycleCharacterIndex : UIEvent, IUIRequest
{
	// Token: 0x06004CFB RID: 19707 RVA: 0x00145806 File Offset: 0x00143C06
	public CycleCharacterIndex(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}

	// Token: 0x0400327F RID: 12927
	public PlayerNum playerNum;
}
