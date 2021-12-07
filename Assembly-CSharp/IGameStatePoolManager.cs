using System;

// Token: 0x0200086C RID: 2156
public interface IGameStatePoolManager
{
	// Token: 0x060035D3 RID: 13779
	TState Acquire<TState>() where TState : GameState, new();
}
