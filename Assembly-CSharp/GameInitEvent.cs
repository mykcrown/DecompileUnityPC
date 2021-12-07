using System;

// Token: 0x02000AEC RID: 2796
public class GameInitEvent : GameEvent
{
	// Token: 0x060050D2 RID: 20690 RVA: 0x001508BD File Offset: 0x0014ECBD
	public GameInitEvent(GameManager gameManager)
	{
		this.gameManager = gameManager;
	}

	// Token: 0x170012F8 RID: 4856
	// (get) Token: 0x060050D3 RID: 20691 RVA: 0x001508CC File Offset: 0x0014ECCC
	// (set) Token: 0x060050D4 RID: 20692 RVA: 0x001508D4 File Offset: 0x0014ECD4
	public GameManager gameManager { get; private set; }
}
