using System;

// Token: 0x02000ACA RID: 2762
public class SetupGameCommand : GameEvent
{
	// Token: 0x060050A7 RID: 20647 RVA: 0x00150575 File Offset: 0x0014E975
	public SetupGameCommand(GameLoadPayload payload, float fadeTime = -1f)
	{
		this.fadeTime = fadeTime;
		this.payload = payload;
	}

	// Token: 0x170012F4 RID: 4852
	// (get) Token: 0x060050A8 RID: 20648 RVA: 0x0015058B File Offset: 0x0014E98B
	// (set) Token: 0x060050A9 RID: 20649 RVA: 0x00150593 File Offset: 0x0014E993
	public float fadeTime { get; private set; }

	// Token: 0x170012F5 RID: 4853
	// (get) Token: 0x060050AA RID: 20650 RVA: 0x0015059C File Offset: 0x0014E99C
	// (set) Token: 0x060050AB RID: 20651 RVA: 0x001505A4 File Offset: 0x0014E9A4
	public GameLoadPayload payload { get; set; }
}
