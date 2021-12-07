// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[Serializable]
public abstract class GameState : MemberwiseEqualityObject, IPooledGameState, IOwnedPooledGameState
{
	private static IEnumerable<Type> allGameStateTypes;

	private static Func<Assembly, IEnumerable<Type>> __f__am_cache0;

	private static Func<Assembly, Type, ____AnonType0<Assembly, Type>> __f__am_cache1;

	private static Func<____AnonType0<Assembly, Type>, bool> __f__am_cache2;

	private static Func<____AnonType0<Assembly, Type>, Type> __f__am_cache3;

	IPooledGameStateOwner IOwnedPooledGameState.Pool
	{
		get;
		set;
	}

	int IOwnedPooledGameState.PoolIndex
	{
		get;
		set;
	}

	private IOwnedPooledGameState owned
	{
		get
		{
			return this;
		}
	}

	public bool IsAcquired
	{
		get
		{
			return this.owned.Pool.IsAcquired(this.owned);
		}
	}

	public GameState()
	{
	}

	void IOwnedPooledGameState.Init(IPooledGameStateOwner pool, int index)
	{
		this.owned.Pool = pool;
		this.owned.PoolIndex = index;
	}

	protected abstract void Reset();

	protected virtual void OnAcquire()
	{
	}

	protected virtual void OnRelease()
	{
	}

	void IOwnedPooledGameState.DoAcquire()
	{
		this.Reset();
		this.OnAcquire();
	}

	void IOwnedPooledGameState.DoRelease()
	{
		this.OnRelease();
	}

	public void Release()
	{
		this.owned.Pool.ReleaseState(this);
	}

	public static IEnumerable<Type> GetAllGameStateTypes()
	{
		if (GameState.allGameStateTypes == null)
		{
			IEnumerable<Assembly> arg_4E_0 = AppDomain.CurrentDomain.GetAssemblies();
			if (GameState.__f__am_cache0 == null)
			{
				GameState.__f__am_cache0 = new Func<Assembly, IEnumerable<Type>>(GameState._GetAllGameStateTypes_m__0);
			}
			Func<Assembly, IEnumerable<Type>> arg_4E_1 = GameState.__f__am_cache0;
			if (GameState.__f__am_cache1 == null)
			{
				GameState.__f__am_cache1 = new Func<Assembly, Type, ____AnonType0<Assembly, Type>>(GameState._GetAllGameStateTypes_m__1);
			}
			var arg_70_0 = arg_4E_0.SelectMany(arg_4E_1, GameState.__f__am_cache1);
			if (GameState.__f__am_cache2 == null)
			{
				GameState.__f__am_cache2 = new Func<____AnonType0<Assembly, Type>, bool>(GameState._GetAllGameStateTypes_m__2);
			}
			var arg_92_0 = arg_70_0.Where(GameState.__f__am_cache2);
			if (GameState.__f__am_cache3 == null)
			{
				GameState.__f__am_cache3 = new Func<____AnonType0<Assembly, Type>, Type>(GameState._GetAllGameStateTypes_m__3);
			}
			GameState.allGameStateTypes = arg_92_0.Select(GameState.__f__am_cache3);
		}
		return GameState.allGameStateTypes;
	}

	private static IEnumerable<Type> _GetAllGameStateTypes_m__0(Assembly a)
	{
		return a.GetTypes();
	}

	private static ____AnonType0<Assembly, Type> _GetAllGameStateTypes_m__1(Assembly a, Type t)
	{
		return new
		{
			a,
			t
		};
	}

	private static bool _GetAllGameStateTypes_m__2(____AnonType0<Assembly, Type> ____TranspIdent0)
	{
		return ____TranspIdent0.t.IsSubclassOf(typeof(GameState));
	}

	private static Type _GetAllGameStateTypes_m__3(____AnonType0<Assembly, Type> ____TranspIdent0)
	{
		return ____TranspIdent0.t;
	}
}
