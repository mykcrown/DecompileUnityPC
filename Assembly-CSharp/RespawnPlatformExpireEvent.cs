using System;

// Token: 0x02000AEF RID: 2799
public class RespawnPlatformExpireEvent : GameEvent
{
	// Token: 0x060050D7 RID: 20695 RVA: 0x001508FB File Offset: 0x0014ECFB
	public RespawnPlatformExpireEvent(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}

	// Token: 0x170012F9 RID: 4857
	// (get) Token: 0x060050D8 RID: 20696 RVA: 0x0015090A File Offset: 0x0014ED0A
	// (set) Token: 0x060050D9 RID: 20697 RVA: 0x00150912 File Offset: 0x0014ED12
	public PlayerNum playerNum { get; private set; }
}
