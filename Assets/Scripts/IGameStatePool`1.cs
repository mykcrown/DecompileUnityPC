// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGameStatePool<TState>
{
	TState Acquire();

	void Release(TState state);
}
