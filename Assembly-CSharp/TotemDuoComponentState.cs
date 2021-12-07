using System;

// Token: 0x020005CD RID: 1485
public class TotemDuoComponentState : IComponentState
{
	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x0600213A RID: 8506 RVA: 0x000A60F2 File Offset: 0x000A44F2
	// (set) Token: 0x0600213B RID: 8507 RVA: 0x000A60FA File Offset: 0x000A44FA
	public IPlayerDelegate partner { get; set; }

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x0600213C RID: 8508 RVA: 0x000A6103 File Offset: 0x000A4503
	// (set) Token: 0x0600213D RID: 8509 RVA: 0x000A610B File Offset: 0x000A450B
	public TotemDuoComponent partnerComponent { get; set; }

	// Token: 0x04001A38 RID: 6712
	public bool isClone;

	// Token: 0x04001A3B RID: 6715
	public bool isActive;

	// Token: 0x04001A3C RID: 6716
	public bool ignoreNextSwap;
}
