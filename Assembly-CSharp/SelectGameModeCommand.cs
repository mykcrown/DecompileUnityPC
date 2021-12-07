using System;

// Token: 0x02000AC4 RID: 2756
public class SelectGameModeCommand : GameEvent
{
	// Token: 0x0600509A RID: 20634 RVA: 0x001504A7 File Offset: 0x0014E8A7
	public SelectGameModeCommand(GameMode mode)
	{
		this.mode = mode;
	}

	// Token: 0x170012F0 RID: 4848
	// (get) Token: 0x0600509B RID: 20635 RVA: 0x001504B6 File Offset: 0x0014E8B6
	// (set) Token: 0x0600509C RID: 20636 RVA: 0x001504BE File Offset: 0x0014E8BE
	public GameMode mode { get; private set; }
}
