using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class RollbackStatePooling : IRollbackStatePooling
{
	// Token: 0x060036DA RID: 14042 RVA: 0x00100370 File Offset: 0x000FE770
	private int getPoolSizeMultiplier(Type type)
	{
		object[] customAttributes = type.GetCustomAttributes(typeof(RollbackStatePoolMultiplier), false);
		if (customAttributes.Length > 0)
		{
			return Math.Max((customAttributes[0] as RollbackStatePoolMultiplier).PoolSizeMultiplier, 1);
		}
		return 1;
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x001003B0 File Offset: 0x000FE7B0
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

	// Token: 0x060036DC RID: 14044 RVA: 0x00100414 File Offset: 0x000FE814
	public void Init()
	{
		IEnumerable<Type> enumerable = from a in AppDomain.CurrentDomain.GetAssemblies()
		from t in a.GetTypes()
		select new
		{
			a,
			t
		} into <>__TranspIdent1
		where !<>__TranspIdent1.t.IsGenericType
		where RollbackStatePooling.IsSubclassOfRawGeneric(typeof(RollbackStateTyped<>), <>__TranspIdent1.t)
		select <>__TranspIdent1.t;
		foreach (Type type in enumerable)
		{
			this.setupType(type, this.getPoolSizeMultiplier(type));
		}
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x00100520 File Offset: 0x000FE920
	private void setupType(Type type, int poolMulti = 1)
	{
		if (!type.GetInterfaces().Any((Type x) => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICopyable<>)))
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

	// Token: 0x060036DE RID: 14046 RVA: 0x001005F4 File Offset: 0x000FE9F4
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

	// Token: 0x060036DF RID: 14047 RVA: 0x00100667 File Offset: 0x000FEA67
	public bool HasPool(Type theType)
	{
		return this.statePools.ContainsKey(theType);
	}

	// Token: 0x04002551 RID: 9553
	public static int DEFAULT_BUFFER_SIZE = 2048;

	// Token: 0x04002552 RID: 9554
	private Dictionary<Type, IStatePool> statePools = new Dictionary<Type, IStatePool>();

	// Token: 0x04002553 RID: 9555
	private Dictionary<Type, int> poolSizeMultiplier = new Dictionary<Type, int>();

	// Token: 0x04002554 RID: 9556
	private List<Type> typeIndex = new List<Type>();
}
