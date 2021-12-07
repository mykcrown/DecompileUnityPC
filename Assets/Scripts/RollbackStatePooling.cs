// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RollbackStatePooling : IRollbackStatePooling
{
	public static int DEFAULT_BUFFER_SIZE = 2048;

	private Dictionary<Type, IStatePool> statePools = new Dictionary<Type, IStatePool>();

	private Dictionary<Type, int> poolSizeMultiplier = new Dictionary<Type, int>();

	private List<Type> typeIndex = new List<Type>();

	private static Func<Assembly, IEnumerable<Type>> __f__am_cache0;

	private static Func<Assembly, Type, ____AnonType0<Assembly, Type>> __f__am_cache1;

	private static Func<____AnonType0<Assembly, Type>, bool> __f__am_cache2;

	private static Func<____AnonType0<Assembly, Type>, bool> __f__am_cache3;

	private static Func<____AnonType0<Assembly, Type>, Type> __f__am_cache4;

	private static Func<Type, bool> __f__am_cache5;

	private int getPoolSizeMultiplier(Type type)
	{
		object[] customAttributes = type.GetCustomAttributes(typeof(RollbackStatePoolMultiplier), false);
		if (customAttributes.Length > 0)
		{
			return Math.Max((customAttributes[0] as RollbackStatePoolMultiplier).PoolSizeMultiplier, 1);
		}
		return 1;
	}

	private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
	{
		while (toCheck != null && toCheck != typeof(object))
		{
			Type right = (!toCheck.IsGenericType) ? toCheck : toCheck.GetGenericTypeDefinition();
			if (generic == right)
			{
				return true;
			}
			toCheck = toCheck.BaseType;
		}
		return false;
	}

	public void Init()
	{
		IEnumerable<Assembly> arg_44_0 = AppDomain.CurrentDomain.GetAssemblies();
		if (RollbackStatePooling.__f__am_cache0 == null)
		{
			RollbackStatePooling.__f__am_cache0 = new Func<Assembly, IEnumerable<Type>>(RollbackStatePooling._Init_m__0);
		}
		Func<Assembly, IEnumerable<Type>> arg_44_1 = RollbackStatePooling.__f__am_cache0;
		if (RollbackStatePooling.__f__am_cache1 == null)
		{
			RollbackStatePooling.__f__am_cache1 = new Func<Assembly, Type, ____AnonType0<Assembly, Type>>(RollbackStatePooling._Init_m__1);
		}
		var arg_66_0 = arg_44_0.SelectMany(arg_44_1, RollbackStatePooling.__f__am_cache1);
		if (RollbackStatePooling.__f__am_cache2 == null)
		{
			RollbackStatePooling.__f__am_cache2 = new Func<____AnonType0<Assembly, Type>, bool>(RollbackStatePooling._Init_m__2);
		}
		var arg_88_0 = arg_66_0.Where(RollbackStatePooling.__f__am_cache2);
		if (RollbackStatePooling.__f__am_cache3 == null)
		{
			RollbackStatePooling.__f__am_cache3 = new Func<____AnonType0<Assembly, Type>, bool>(RollbackStatePooling._Init_m__3);
		}
		var arg_AA_0 = arg_88_0.Where(RollbackStatePooling.__f__am_cache3);
		if (RollbackStatePooling.__f__am_cache4 == null)
		{
			RollbackStatePooling.__f__am_cache4 = new Func<____AnonType0<Assembly, Type>, Type>(RollbackStatePooling._Init_m__4);
		}
		IEnumerable<Type> enumerable = arg_AA_0.Select(RollbackStatePooling.__f__am_cache4);
		foreach (Type current in enumerable)
		{
			this.setupType(current, this.getPoolSizeMultiplier(current));
		}
	}

	private void setupType(Type type, int poolMulti = 1)
	{
		IEnumerable<Type> arg_23_0 = type.GetInterfaces();
		if (RollbackStatePooling.__f__am_cache5 == null)
		{
			RollbackStatePooling.__f__am_cache5 = new Func<Type, bool>(RollbackStatePooling._setupType_m__5);
		}
		if (!arg_23_0.Any(RollbackStatePooling.__f__am_cache5))
		{
			throw new UnityException("Invalid pool type " + type.Name);
		}
		if (this.statePools.ContainsKey(type))
		{
			throw new UnityException("Already have a state pool for type of " + type.Name);
		}
		this.typeIndex.Add(type);
		this.poolSizeMultiplier[type] = poolMulti;
		Type type2 = typeof(StatePool<>).MakeGenericType(new Type[]
		{
			type
		});
		this.statePools[type] = (Activator.CreateInstance(type2, new object[]
		{
			poolMulti
		}) as IStatePool);
	}

	public T Clone<T>(T source) where T : RollbackStateTyped<T>
	{
		IStatePool<T> statePool = this.statePools[typeof(T)] as IStatePool<T>;
		ICopyable<T> copyable = statePool.GetNext();
		if (source == copyable as T)
		{
			throw new Exception("same source and target");
		}
		source.CopyTo((T)((object)copyable));
		return (T)((object)copyable);
	}

	public bool HasPool(Type theType)
	{
		return this.statePools.ContainsKey(theType);
	}

	private static IEnumerable<Type> _Init_m__0(Assembly a)
	{
		return a.GetTypes();
	}

	private static ____AnonType0<Assembly, Type> _Init_m__1(Assembly a, Type t)
	{
		return new
		{
			a,
			t
		};
	}

	private static bool _Init_m__2(____AnonType0<Assembly, Type> ____TranspIdent1)
	{
		return !____TranspIdent1.t.IsGenericType;
	}

	private static bool _Init_m__3(____AnonType0<Assembly, Type> ____TranspIdent1)
	{
		return RollbackStatePooling.IsSubclassOfRawGeneric(typeof(RollbackStateTyped<>), ____TranspIdent1.t);
	}

	private static Type _Init_m__4(____AnonType0<Assembly, Type> ____TranspIdent1)
	{
		return ____TranspIdent1.t;
	}

	private static bool _setupType_m__5(Type x)
	{
		return x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICopyable<>);
	}
}
