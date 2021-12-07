using System;
using System.Collections.Generic;

// Token: 0x02000B53 RID: 2899
public class PoolManager
{
	// Token: 0x17001374 RID: 4980
	// (get) Token: 0x0600540F RID: 21519 RVA: 0x001B0E02 File Offset: 0x001AF202
	public static PoolManager Instance
	{
		get
		{
			if (PoolManager._instance == null)
			{
				PoolManager._instance = new PoolManager();
			}
			return PoolManager._instance;
		}
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001B0E20 File Offset: 0x001AF220
	public GenericObjectPool<T> GetPool<T>() where T : class, new()
	{
		if (!this.pools.ContainsKey(typeof(T)))
		{
			this.pools.Add(typeof(T), new GenericObjectPool<T>(0, () => Activator.CreateInstance<T>(), delegate(T val)
			{
			}, null));
		}
		return this.pools[typeof(T)] as GenericObjectPool<T>;
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x001B0E98 File Offset: 0x001AF298
	public T GetObject<T>() where T : class, new()
	{
		GenericObjectPool<T> pool = this.GetPool<T>();
		return pool.New();
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001B0EB4 File Offset: 0x001AF2B4
	public void ReturnObject<T>(T val) where T : class, new()
	{
		GenericObjectPool<T> pool = this.GetPool<T>();
		pool.Store(val);
	}

	// Token: 0x04003553 RID: 13651
	private static PoolManager _instance;

	// Token: 0x04003554 RID: 13652
	private Dictionary<Type, object> pools = new Dictionary<Type, object>();
}
