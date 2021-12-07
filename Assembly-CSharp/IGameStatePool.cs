using System;

// Token: 0x02000868 RID: 2152
public interface IGameStatePool<TState>
{
	// Token: 0x060035C3 RID: 13763
	TState Acquire();

	// Token: 0x060035C4 RID: 13764
	void Release(TState state);
}
