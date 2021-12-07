using System;
using System.Collections.Generic;

// Token: 0x02000867 RID: 2151
public class GameStatePool<TState> : IPooledGameStateOwner, IGameStatePool<TState> where TState : GameState, new()
{
	// Token: 0x060035B7 RID: 13751 RVA: 0x000FE29A File Offset: 0x000FC69A
	public GameStatePool(GameStatePoolManager manager, int capacity)
	{
		this.manager = manager;
		this.Reserve(capacity);
	}

	// Token: 0x17000D0C RID: 3340
	// (get) Token: 0x060035B8 RID: 13752 RVA: 0x000FE2D1 File Offset: 0x000FC6D1
	// (set) Token: 0x060035B9 RID: 13753 RVA: 0x000FE2D9 File Offset: 0x000FC6D9
	public int Capacity { get; private set; }

	// Token: 0x17000D0D RID: 3341
	// (get) Token: 0x060035BA RID: 13754 RVA: 0x000FE2E2 File Offset: 0x000FC6E2
	public int AcquiredStateCount
	{
		get
		{
			return this.acquiredIndices.Count;
		}
	}

	// Token: 0x17000D0E RID: 3342
	// (get) Token: 0x060035BB RID: 13755 RVA: 0x000FE2EF File Offset: 0x000FC6EF
	private IPooledGameStateOwner owner
	{
		get
		{
			return this;
		}
	}

	// Token: 0x17000D0F RID: 3343
	// (get) Token: 0x060035BC RID: 13756 RVA: 0x000FE2F2 File Offset: 0x000FC6F2
	private ILogger logger
	{
		get
		{
			return this.manager.logger;
		}
	}

	// Token: 0x17000D10 RID: 3344
	// (get) Token: 0x060035BD RID: 13757 RVA: 0x000FE2FF File Offset: 0x000FC6FF
	private NetGraphVisualizer netGraph
	{
		get
		{
			return this.manager.netGraph;
		}
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x000FE30C File Offset: 0x000FC70C
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
			TState tstate = Activator.CreateInstance<TState>();
			IOwnedPooledGameState ownedPooledGameState = tstate;
			ownedPooledGameState.Init(this, i);
			this.states.Add(tstate);
			this.freeIndexQueue.Enqueue(i);
		}
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x000FE384 File Offset: 0x000FC784
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

	// Token: 0x060035C0 RID: 13760 RVA: 0x000FE3D6 File Offset: 0x000FC7D6
	bool IPooledGameStateOwner.IsAcquired(IOwnedPooledGameState state)
	{
		return this.acquiredIndices.Contains(state.PoolIndex);
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x000FE3EC File Offset: 0x000FC7EC
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
		TState tstate = this.states[num];
		IOwnedPooledGameState ownedPooledGameState = tstate;
		ownedPooledGameState.DoAcquire();
		return tstate;
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x000FE493 File Offset: 0x000FC893
	public void Release(TState state)
	{
		this.owner.ReleaseState(state);
	}

	// Token: 0x040024D2 RID: 9426
	private GameStatePoolManager manager;

	// Token: 0x040024D3 RID: 9427
	private List<TState> states = new List<TState>();

	// Token: 0x040024D4 RID: 9428
	private Queue<int> freeIndexQueue = new Queue<int>();

	// Token: 0x040024D5 RID: 9429
	private HashSet<int> acquiredIndices = new HashSet<int>();
}
