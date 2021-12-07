// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class GameStateContainer : IDisposable
{
	private IGameStatePoolManager poolManager;

	private List<GameState> states = new List<GameState>();

	private int index;

	public GameStateContainer(IGameStatePoolManager poolManager)
	{
		this.poolManager = poolManager;
	}

	public void Clear()
	{
		this.index = 0;
		foreach (GameState current in this.states)
		{
			current.Release();
		}
		this.states.Clear();
	}

	public void WriteState<TState>(TState state) where TState : GameState, new()
	{
		TState tState = this.poolManager.Acquire<TState>();
		tState.Assign(state);
		this.states.Add(tState);
	}

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
		TState tState = gameState as TState;
		if (tState == null)
		{
			GameClient.Log(LogLevel.Error, new object[]
			{
				"Attempted to read a GameState of type '{0}' but found type of '{1}'.",
				typeof(TState),
				gameState.GetType()
			});
			return false;
		}
		stateOut.Assign(tState);
		return true;
	}

	public void Dispose()
	{
		this.Clear();
	}

	public void ResetIndex()
	{
		this.index = 0;
	}
}
