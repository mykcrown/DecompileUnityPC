using System;

// Token: 0x02000467 RID: 1127
public interface IGameClient
{
	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x060017D8 RID: 6104
	FPSCounter DisplayFPSCounter { get; }

	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x060017D9 RID: 6105
	FPSCounter GameTickFPSCounter { get; }
}
