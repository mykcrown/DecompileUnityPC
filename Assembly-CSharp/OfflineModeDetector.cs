using System;

// Token: 0x0200080A RID: 2058
public class OfflineModeDetector : IOfflineModeDetector
{
	// Token: 0x060032C5 RID: 12997 RVA: 0x000F336B File Offset: 0x000F176B
	public OfflineModeDetector(MasterContext.InitContext initContext)
	{
	}

	// Token: 0x060032C6 RID: 12998 RVA: 0x000F3373 File Offset: 0x000F1773
	public bool IsOfflineMode()
	{
		return true;
	}
}
