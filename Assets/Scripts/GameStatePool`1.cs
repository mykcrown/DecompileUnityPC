// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GameStatePool<TState> : IPooledGameStateOwner, IGameStatePool<TState> where TState : GameState, new()
{
	private GameStatePoolManager manager;

	private List<TState> states = new List<TState>();

	private Queue<int> freeIndexQueue = new Queue<int>();

	private HashSet<int> acquiredIndices = new HashSet<int>();

	public int Capacity
	{
		get;
		private set;
	}

	public int AcquiredStateCount
	{
		get
		{
			return this.acquiredIndices.Count;
		}
	}

	private IPooledGameStateOwner owner
	{
		get
		{
			return this;
		}
	}

	private ILogger logger
	{
		get
		{
			return this.manager.logger;
		}
	}

	private NetGraphVisualizer netGraph
	{
		get
		{
			return this.manager.netGraph;
		}
	}

	public GameStatePool(GameStatePoolManager manager, int capacity)
	{
		this.manager = manager;
		this.Reserve(capacity);
	}

	public void Reserve(int capacity)
	{
		if (capacity < this.Capacity)
		{
			throw new ArgumentException("Capacity of GameStatePool cannot be reduced.");
		}
		int capacity2 = this.Capacity;
		this.Capacity = capacity;
		for (int i = capacity2; i < this.Capacity; i++)
		{
			TState tState = Activator.CreateInstance<TState>();
			IOwnedPooledGameState ownedPooledGameState = tState;
			ownedPooledGameState.Init(this, i);
			this.states.Add(tState);
			this.freeIndexQueue.Enqueue(i);
		}
	}

	void IPooledGameStateOwner.ReleaseState(IOwnedPooledGameState state)
	{
		if (!this.acquiredIndices.Contains(state.PoolIndex))
		{
			throw new GameStateNotAcquiredException();
		}
		this.acquiredIndices.Remove(state.PoolIndex);
		this.freeIndexQueue.Enqueue(state.PoolIndex);
		state.DoRelease();
	}

	bool IPooledGameStateOwner.IsAcquired(IOwnedPooledGameState state)
	{
		return this.acquiredIndices.Contains(state.PoolIndex);
	}

	public TState Acquire()
	{
		if (this.freeIndexQueue.Count <= 0)
		{
			int capacity = this.Capacity;
			this.Reserve(this.Capacity * 2);
			this.logger.LogFormat(LogLevel.Warning, "GameStatePool for GameState: {0} ran out of pool space.  Increasing pool size from {1} to {2}.", new object[]
			{
				typeof(TState).ToString(),
				capacity,
				this.Capacity
			});
		}
		int num = this.freeIndexQueue.Dequeue();
		this.acquiredIndices.Add(num);
		TState tState = this.states[num];
		IOwnedPooledGameState ownedPooledGameState = tState;
		ownedPooledGameState.DoAcquire();
		return tState;
	}

	public void Release(TState state)
	{
		this.owner.ReleaseState(state);
	}
}
