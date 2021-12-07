using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MemberwiseEquality;

// Token: 0x02000861 RID: 2145
[Serializable]
public abstract class GameState : MemberwiseEqualityObject, IPooledGameState, IOwnedPooledGameState
{
	// Token: 0x0600358C RID: 13708 RVA: 0x000FDF2F File Offset: 0x000FC32F
	public GameState()
	{
	}

	// Token: 0x17000D05 RID: 3333
	// (get) Token: 0x0600358D RID: 13709 RVA: 0x000FDF37 File Offset: 0x000FC337
	// (set) Token: 0x0600358E RID: 13710 RVA: 0x000FDF3F File Offset: 0x000FC33F
	IPooledGameStateOwner IOwnedPooledGameState.Pool { get; set; }

	// Token: 0x17000D06 RID: 3334
	// (get) Token: 0x0600358F RID: 13711 RVA: 0x000FDF48 File Offset: 0x000FC348
	// (set) Token: 0x06003590 RID: 13712 RVA: 0x000FDF50 File Offset: 0x000FC350
	int IOwnedPooledGameState.PoolIndex { get; set; }

	// Token: 0x17000D07 RID: 3335
	// (get) Token: 0x06003591 RID: 13713 RVA: 0x000FDF59 File Offset: 0x000FC359
	private IOwnedPooledGameState owned
	{
		get
		{
			return this;
		}
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x000FDF5C File Offset: 0x000FC35C
	void IOwnedPooledGameState.Init(IPooledGameStateOwner pool, int index)
	{
		this.owned.Pool = pool;
		this.owned.PoolIndex = index;
	}

	// Token: 0x06003593 RID: 13715
	protected abstract void Reset();

	// Token: 0x06003594 RID: 13716 RVA: 0x000FDF76 File Offset: 0x000FC376
	protected virtual void OnAcquire()
	{
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000FDF78 File Offset: 0x000FC378
	protected virtual void OnRelease()
	{
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x000FDF7A File Offset: 0x000FC37A
	void IOwnedPooledGameState.DoAcquire()
	{
		this.Reset();
		this.OnAcquire();
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x000FDF88 File Offset: 0x000FC388
	void IOwnedPooledGameState.DoRelease()
	{
		this.OnRelease();
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x000FDF90 File Offset: 0x000FC390
	public void Release()
	{
		this.owned.Pool.ReleaseState(this);
	}

	// Token: 0x17000D08 RID: 3336
	// (get) Token: 0x06003599 RID: 13721 RVA: 0x000FDFA3 File Offset: 0x000FC3A3
	public bool IsAcquired
	{
		get
		{
			return this.owned.Pool.IsAcquired(this.owned);
		}
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x000FDFBC File Offset: 0x000FC3BC
	public static IEnumerable<Type> GetAllGameStateTypes()
	{
		if (GameState.allGameStateTypes == null)
		{
			GameState.allGameStateTypes = from a in AppDomain.CurrentDomain.GetAssemblies()
			from t in a.GetTypes()
			select new
			{
				a,
				t
			} into <>__TranspIdent0
			where <>__TranspIdent0.t.IsSubclassOf(typeof(GameState))
			select <>__TranspIdent0.t;
		}
		return GameState.allGameStateTypes;
	}

	// Token: 0x040024C9 RID: 9417
	private static IEnumerable<Type> allGameStateTypes;
}
