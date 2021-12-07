using System;

// Token: 0x02000ADB RID: 2779
public class DisconnectPlayerCommand : GameEvent
{
	// Token: 0x060050C0 RID: 20672 RVA: 0x00150709 File Offset: 0x0014EB09
	public DisconnectPlayerCommand(PlayerNum playerNum, int despawnFrame)
	{
		this.playerNum = playerNum;
		this.despawnFrame = despawnFrame;
	}

	// Token: 0x0400340A RID: 13322
	public int despawnFrame;

	// Token: 0x0400340B RID: 13323
	public PlayerNum playerNum;
}
