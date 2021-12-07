using System;

// Token: 0x0200029C RID: 668
public interface IApplicationFramerateManager
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000E1B RID: 3611
	// (set) Token: 0x06000E1C RID: 3612
	FrameSyncMode frameSyncMode { get; set; }

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000E1D RID: 3613
	// (set) Token: 0x06000E1E RID: 3614
	int overrideTargetFramerate { get; set; }

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000E1F RID: 3615
	bool UseRenderWait { get; }
}
