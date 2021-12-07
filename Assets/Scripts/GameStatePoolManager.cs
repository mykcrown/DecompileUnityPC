// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GameStatePoolManager : IGameStatePoolManager
{
	public static int DEFAULT_POOL_SIZE = 8;

	private Dictionary<Type, object> statePools;

	[Inject]
	public ILogger logger
	{
		get;
		set;
	}

	[Inject]
	public NetGraphVisualizer netGraph
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.statePools = new Dictionary<Type, object>();
		foreach (Type current in GameState.GetAllGameStateTypes())
		{
			this.addPool(current);
		}
	}

	private int getPoolSize(Type type)
	{
		object[] customAttributes = type.GetCustomAttributes(typeof(GameStatePoolSize), false);
		if (customAttributes != null && customAttributes.Length > 0)
		{
			return (customAttributes[0] as GameStatePoolSize).PoolSize;
		}
		return GameStatePoolManager.DEFAULT_POOL_SIZE;
	}

	private void addPool(Type stateType)
	{
		if (!stateType.IsSubclassOf(typeof(GameState)))
		{
			this.logger.LogFormat(LogLevel.Error, "Cannot add a GameStatePool for Type '{0}' because it is not derived from GameState.", new object[]
			{
				stateType
			});
			return;
		}
		if (this.statePools.ContainsKey(stateType))
		{
			this.logger.LogFormat(LogLevel.Error, "Already added a GameStatePool of Type '{0}'", new object[]
			{
				stateType
			});
		}
		Type type = typeof(GameStatePool<>).MakeGenericType(new Type[]
		{
			stateType
		});
		object value = Activator.CreateInstance(type, new object[]
		{
			this,
			this.getPoolSize(stateType)
		});
		this.statePools.Add(stateType, value);
	}

	private void addPool<TState>() where TState : GameState, new()
	{
		Type typeFromHandle = typeof(TState);
		if (this.statePools.ContainsKey(typeFromHandle))
		{
			this.logger.LogFormat(LogLevel.Error, "Already added a GameStatePool of Type '{0}'", new object[]
			{
				typeFromHandle
			});
		}
		this.statePools.Add(typeFromHandle, new GameStatePool<TState>(this, this.getPoolSize(typeFromHandle)));
	}

	public TState Acquire<TState>() where TState : GameState, new()
	{
		if (!this.statePools.ContainsKey(typeof(TState)))
		{
			this.addPool<TState>();
		}
		GameStatePool<TState> gameStatePool = this.statePools[typeof(TState)] as GameStatePool<TState>;
		return gameStatePool.Acquire();
	}
}
