using System;

// Token: 0x02000685 RID: 1669
public interface IKeyBindingManager
{
	// Token: 0x06002938 RID: 10552
	void Begin();

	// Token: 0x06002939 RID: 10553
	void End();

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x0600293A RID: 10554
	bool IsBindingKey { get; }
}
