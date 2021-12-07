// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGameStatePoolManager
{
	TState Acquire<TState>() where TState : GameState, new();
}
