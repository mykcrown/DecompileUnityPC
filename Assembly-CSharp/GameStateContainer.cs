using System;
using System.Collections.Generic;

// Token: 0x02000865 RID: 2149
[Serializable]
public class GameStateContainer : IDisposable
{
	// Token: 0x060035AB RID: 13739 RVA: 0x000FE09A File Offset: 0x000FC49A
	public GameStateContainer(IGameStatePoolManager poolManager)
	{
		this.poolManager = poolManager;
	}

	// Token: 0x060035AC RID: 13740 RVA: 0x000FE0B4 File Offset: 0x000FC4B4
	public void Clear()
	{
		this.index = 0;
		foreach (GameState gameState in this.states)
		{
			gameState.Release();
		}
		this.states.Clear();
	}

	// Token: 0x060035AD RID: 13741 RVA: 0x000FE124 File Offset: 0x000FC524
	public void WriteState<TState>(TState state) where TState : GameState, new()
	{
		TState tstate = this.poolManager.Acquire<TState>();
		tstate.Assign(state);
		this.states.Add(tstate);
	}

	// Token: 0x060035AE RID: 13742 RVA: 0x000FE164 File Offset: 0x000FC564
	public bool ReadState<TState>(ref TState stateOut) where TState : GameState, new()
	{
		if (this.index >= this.states.Count)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				"Attempted to read too many elements, index: {0} count: {1}",
				this.index,
				this.states.Count
			});
			return false;
		}
		GameState gameState = this.states[this.index];
		this.index++;
		TState tstate = gameState as TState;
		if (tstate == null)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				"Attempted to read a GameState of type '{0}' but found type of '{1}'.",
				typeof(TState),
				gameState.GetType()
			});
			return false;
		}
		stateOut.Assign(tstate);
		return true;
	}

	// Token: 0x060035AF RID: 13743 RVA: 0x000FE234 File Offset: 0x000FC634
	public void Dispose()
	{
		this.Clear();
	}

	// Token: 0x060035B0 RID: 13744 RVA: 0x000FE23C File Offset: 0x000FC63C
	public void ResetIndex()
	{
		this.index = 0;
	}

	// Token: 0x040024CE RID: 9422
	private IGameStatePoolManager poolManager;

	// Token: 0x040024CF RID: 9423
	private List<GameState> states = new List<GameState>();

	// Token: 0x040024D0 RID: 9424
	private int index;
}
