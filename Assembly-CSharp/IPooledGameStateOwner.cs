using System;

// Token: 0x02000869 RID: 2153
public interface IPooledGameStateOwner
{
	// Token: 0x060035C5 RID: 13765
	void ReleaseState(IOwnedPooledGameState state);

	// Token: 0x060035C6 RID: 13766
	bool IsAcquired(IOwnedPooledGameState state);
}
