// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IOwnedPooledGameState : IPooledGameState
{
	int PoolIndex
	{
		get;
		set;
	}

	IPooledGameStateOwner Pool
	{
		get;
		set;
	}

	void Init(IPooledGameStateOwner pool, int index);

	void DoAcquire();

	void DoRelease();
}
