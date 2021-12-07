using System;

// Token: 0x02000A4F RID: 2639
[Serializable]
public class CyclePlayerSkinRequest : UIEvent, IUIRequest
{
	// Token: 0x06004CFA RID: 19706 RVA: 0x001457F0 File Offset: 0x00143BF0
	public CyclePlayerSkinRequest(PlayerNum playerNum, int direction = 1)
	{
		this.playerNum = playerNum;
		this.direction = direction;
	}

	// Token: 0x0400327D RID: 12925
	public PlayerNum playerNum;

	// Token: 0x0400327E RID: 12926
	public int direction;
}
