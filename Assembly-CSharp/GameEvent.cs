using System;

// Token: 0x02000ABE RID: 2750
[Serializable]
public class GameEvent
{
	// Token: 0x0600508F RID: 20623 RVA: 0x000F1058 File Offset: 0x000EF458
	public GameEvent()
	{
		this.frame = 0;
	}

	// Token: 0x170012EF RID: 4847
	// (get) Token: 0x06005090 RID: 20624 RVA: 0x000F1067 File Offset: 0x000EF467
	// (set) Token: 0x06005091 RID: 20625 RVA: 0x000F106F File Offset: 0x000EF46F
	public int frame { get; protected set; }
}
