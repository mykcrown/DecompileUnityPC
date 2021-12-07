using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x02000AA6 RID: 2726
public class CloneUtil
{
	// Token: 0x0600500A RID: 20490 RVA: 0x0014E120 File Offset: 0x0014C520
	public static T SlowDeepClone<T>(T obj)
	{
		return (T)((object)CloneUtil.ReflectionClone(obj, BindingFlags.Instance | BindingFlags.Public));
	}

	// Token: 0x0600500B RID: 20491 RVA: 0x0014E134 File Offset: 0x0014C534
	public static T SerializationClone<T>(T obj)
	{
		T result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			result = (T)((object)binaryFormatter.Deserialize(memoryStream));
		}
		return result;
	}

	// Token: 0x0600500C RID: 20492 RVA: 0x0014E194 File Offset: 0x0014C594
	public static object ReflectionClone(object target, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
	{
		Type type = target.GetType();
		if (type.IsArray)
		{
			return CloneUtil.ReflectionCloneArray((object[])target);
		}
		object obj = Activator.CreateInstance(type);
		FieldInfo[] fields = type.GetFields(flags);
		foreach (FieldInfo fieldInfo in fields)
		{
			object value = fieldInfo.GetValue(target);
			if (value == null || fieldInfo.FieldType.IsEnum || fieldInfo.FieldType.IsValueType || fieldInfo.FieldType.Equals(typeof(string)) || fieldInfo.FieldType.GetInterface("ICloneable", true) == null || fieldInfo.FieldType.IsSubclassOf(typeof(ScriptableObject)))
			{
				fieldInfo.SetValue(obj, value);
			}
			else if (value is ICloneable && !fieldInfo.FieldType.IsArray)
			{
				fieldInfo.SetValue(obj, ((ICloneable)value).Clone());
			}
			else
			{
				fieldInfo.SetValue(obj, CloneUtil.ReflectionClone(value, BindingFlags.Instance | BindingFlags.Public));
			}
		}
		return obj;
	}

	// Token: 0x0600500D RID: 20493 RVA: 0x0014E2C4 File Offset: 0x0014C6C4
	public static bool IsList(object o)
	{
		return o != null && (o is IList && o.GetType().IsGenericType) && o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
	}

	// Token: 0x0600500E RID: 20494 RVA: 0x0014E314 File Offset: 0x0014C714
	public static bool IsDictionary(object o)
	{
		if (o == null)
		{
			return false;
		}
		if (!o.GetType().IsGenericType)
		{
			return false;
		}
		Type genericTypeDefinition = o.GetType().GetGenericTypeDefinition();
		return o is IDictionary && o.GetType().IsGenericType && (genericTypeDefinition.IsAssignableFrom(typeof(Dictionary<, >)) || genericTypeDefinition.IsAssignableFrom(typeof(SerializableDictionary<, >)));
	}

	// Token: 0x0600500F RID: 20495 RVA: 0x0014E390 File Offset: 0x0014C790
	public static object[] ReflectionCloneArray(object[] target)
	{
		object[] array = (object[])Array.CreateInstance(target.GetType().GetElementType(), target.Length);
		for (int i = 0; i < target.Length; i++)
		{
			if (target[i] == null || target[i].GetType().IsEnum || target[i].GetType().IsValueType || target[i].GetType().IsGenericType || target[i].GetType().Equals(typeof(string)) || target[i].GetType().IsSubclassOf(typeof(ScriptableObject)))
			{
				array[i] = target[i];
			}
			else if (target[i] is ICloneable && !target[i].GetType().IsArray)
			{
				array[i] = ((ICloneable)target[i]).Clone();
			}
			else
			{
				array[i] = CloneUtil.ReflectionClone(target[i], BindingFlags.Instance | BindingFlags.Public);
			}
		}
		return array;
	}
}
