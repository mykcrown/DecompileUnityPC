using System;

// Token: 0x02000A4E RID: 2638
[Serializable]
public class CyclePlayerTeamRequest : UIEvent, IUIRequest
{
	// Token: 0x06004CF9 RID: 19705 RVA: 0x001457DA File Offset: 0x00143BDA
	public CyclePlayerTeamRequest(PlayerNum playerNum, int direction = 1)
	{
		this.playerNum = playerNum;
		this.direction = direction;
	}

	// Token: 0x0400327B RID: 12923
	public PlayerNum playerNum;

	// Token: 0x0400327C RID: 12924
	public int direction;
}
