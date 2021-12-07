using System;
using System.Collections.Generic;

// Token: 0x0200086B RID: 2155
public class GameStatePoolManager : IGameStatePoolManager
{
	// Token: 0x17000D11 RID: 3345
	// (get) Token: 0x060035C9 RID: 13769 RVA: 0x000FE4B6 File Offset: 0x000FC8B6
	// (set) Token: 0x060035CA RID: 13770 RVA: 0x000FE4BE File Offset: 0x000FC8BE
	[Inject]
	public ILogger logger { get; set; }

	// Token: 0x17000D12 RID: 3346
	// (get) Token: 0x060035CB RID: 13771 RVA: 0x000FE4C7 File Offset: 0x000FC8C7
	// (set) Token: 0x060035CC RID: 13772 RVA: 0x000FE4CF File Offset: 0x000FC8CF
	[Inject]
	public NetGraphVisualizer netGraph { get; set; }

	// Token: 0x060035CD RID: 13773 RVA: 0x000FE4D8 File Offset: 0x000FC8D8
	[PostConstruct]
	public void Init()
	{
		this.statePools = new Dictionary<Type, object>();
		foreach (Type stateType in GameState.GetAllGameStateTypes())
		{
			this.addPool(stateType);
		}
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x000FE53C File Offset: 0x000FC93C
	private int getPoolSize(Type type)
	{
		object[] customAttributes = type.GetCustomAttributes(typeof(GameStatePoolSize), false);
		if (customAttributes != null && customAttributes.Length > 0)
		{
			return (customAttributes[0] as GameStatePoolSize).PoolSize;
		}
		return GameStatePoolManager.DEFAULT_POOL_SIZE;
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x000FE580 File Offset: 0x000FC980
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

	// Token: 0x060035D0 RID: 13776 RVA: 0x000FE634 File Offset: 0x000FCA34
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

	// Token: 0x060035D1 RID: 13777 RVA: 0x000FE694 File Offset: 0x000FCA94
	public TState Acquire<TState>() where TState : GameState, new()
	{
		if (!this.statePools.ContainsKey(typeof(TState)))
		{
			this.addPool<TState>();
		}
		GameStatePool<TState> gameStatePool = this.statePools[typeof(TState)] as GameStatePool<TState>;
		return gameStatePool.Acquire();
	}

	// Token: 0x040024D9 RID: 9433
	public static int DEFAULT_POOL_SIZE = 8;

	// Token: 0x040024DA RID: 9434
	private Dictionary<Type, object> statePools;
}
