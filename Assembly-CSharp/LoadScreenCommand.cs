using System;

// Token: 0x02000AC8 RID: 2760
public class LoadScreenCommand : GameEvent
{
	// Token: 0x060050A0 RID: 20640 RVA: 0x00150525 File Offset: 0x0014E925
	public LoadScreenCommand(ScreenType type, Payload payload = null, ScreenUpdateType updateType = ScreenUpdateType.Next)
	{
		this.type = type;
		this.payload = payload;
		this.updateType = updateType;
	}

	// Token: 0x170012F1 RID: 4849
	// (get) Token: 0x060050A1 RID: 20641 RVA: 0x00150542 File Offset: 0x0014E942
	// (set) Token: 0x060050A2 RID: 20642 RVA: 0x0015054A File Offset: 0x0014E94A
	public ScreenType type { get; private set; }

	// Token: 0x170012F2 RID: 4850
	// (get) Token: 0x060050A3 RID: 20643 RVA: 0x00150553 File Offset: 0x0014E953
	// (set) Token: 0x060050A4 RID: 20644 RVA: 0x0015055B File Offset: 0x0014E95B
	public Payload payload { get; private set; }

	// Token: 0x170012F3 RID: 4851
	// (get) Token: 0x060050A5 RID: 20645 RVA: 0x00150564 File Offset: 0x0014E964
	// (set) Token: 0x060050A6 RID: 20646 RVA: 0x0015056C File Offset: 0x0014E96C
	public ScreenUpdateType updateType { get; private set; }
}
