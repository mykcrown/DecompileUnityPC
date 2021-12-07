// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CloneUtil
{
	public static T SlowDeepClone<T>(T obj)
	{
		return (T)((object)CloneUtil.ReflectionClone(obj, BindingFlags.Instance | BindingFlags.Public));
	}

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

	public static object ReflectionClone(object target, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
	{
		Type type = target.GetType();
		if (type.IsArray)
		{
			return CloneUtil.ReflectionCloneArray((object[])target);
		}
		object obj = Activator.CreateInstance(type);
		FieldInfo[] fields = type.GetFields(flags);
		FieldInfo[] array = fields;
		for (int i = 0; i < array.Length; i++)
		{
			FieldInfo fieldInfo = array[i];
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

	public static bool IsList(object o)
	{
		return o != null && (o is IList && o.GetType().IsGenericType) && o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
	}

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
