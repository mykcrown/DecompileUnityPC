using System;

// Token: 0x02000863 RID: 2147
public interface IOwnedPooledGameState : IPooledGameState
{
	// Token: 0x17000D0A RID: 3338
	// (get) Token: 0x060035A1 RID: 13729
	// (set) Token: 0x060035A2 RID: 13730
	int PoolIndex { get; set; }

	// Token: 0x17000D0B RID: 3339
	// (get) Token: 0x060035A3 RID: 13731
	// (set) Token: 0x060035A4 RID: 13732
	IPooledGameStateOwner Pool { get; set; }

	// Token: 0x060035A5 RID: 13733
	void Init(IPooledGameStateOwner pool, int index);

	// Token: 0x060035A6 RID: 13734
	void DoAcquire();

	// Token: 0x060035A7 RID: 13735
	void DoRelease();
}
