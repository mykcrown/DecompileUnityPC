using System;

// Token: 0x02000ACF RID: 2767
public class HologramCancelCommand : GameEvent
{
	// Token: 0x060050B0 RID: 20656 RVA: 0x001505F0 File Offset: 0x0014E9F0
	public HologramCancelCommand(PlayerNum playerNum)
	{
		this.playerNum = playerNum;
	}

	// Token: 0x040033F5 RID: 13301
	public PlayerNum playerNum;
}
