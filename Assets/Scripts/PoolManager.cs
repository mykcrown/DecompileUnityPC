// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PoolManager
{
	private static PoolManager _instance;

	private Dictionary<Type, object> pools = new Dictionary<Type, object>();

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

	public GenericObjectPool<T> GetPool<T>() where T : class, new()
	{
		if (!this.pools.ContainsKey(typeof(T)))
		{
			this.pools.Add(typeof(T), new GenericObjectPool<T>(0, new GenericObjectPool<T>.NewCallback(PoolManager._GetPool`1_m__0<T>), new Action<T>(PoolManager._GetPool`1_m__1<T>), null));
		}
		return this.pools[typeof(T)] as GenericObjectPool<T>;
	}

	public T GetObject<T>() where T : class, new()
	{
		GenericObjectPool<T> pool = this.GetPool<T>();
		return pool.New();
	}

	public void ReturnObject<T>(T val) where T : class, new()
	{
		GenericObjectPool<T> pool = this.GetPool<T>();
		pool.Store(val);
	}

	private static T _GetPool<T>() where T : class, new()
	{
		return Activator.CreateInstance<T>();
	}

	private static void _GetPool<T>(T val) where T : class, new()
	{
	}
}
