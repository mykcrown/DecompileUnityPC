// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPooledGameStateOwner
{
	void ReleaseState(IOwnedPooledGameState state);

	bool IsAcquired(IOwnedPooledGameState state);
}
