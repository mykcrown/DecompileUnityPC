using System;
using System.Collections.Generic;

// Token: 0x02000654 RID: 1620
public interface IStageTriggerDependency : IFrameOwner
{
	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x060027B4 RID: 10164
	GameModeData ModeData { get; }

	// Token: 0x060027B5 RID: 10165
	List<PlayerController> GetPlayers();
}
