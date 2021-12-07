// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;

public class ReflectEqualityObject
{
	private static Dictionary<Type, PropertyInfo[]> _properties;

	static ReflectEqualityObject()
	{
		ReflectEqualityObject._properties = new Dictionary<Type, PropertyInfo[]>();
	}

	public ReflectEqualityObject()
	{
		Type type = base.GetType();
		if (!ReflectEqualityObject._properties.ContainsKey(type))
		{
			ReflectEqualityObject._properties[type] = type.GetProperties();
		}
	}

	public override bool Equals(object obj)
	{
		Type type = base.GetType();
		if (type.IsAssignableFrom(obj.GetType()))
		{
			PropertyInfo[] array = ReflectEqualityObject._properties[type];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				if (!propertyInfo.GetValue(this, null).Equals(propertyInfo.GetValue(obj, null)))
				{
					return false;
				}
			}
			return true;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		int num = 0;
		PropertyInfo[] array = ReflectEqualityObject._properties[base.GetType()];
		for (int i = 0; i < array.Length; i++)
		{
			PropertyInfo propertyInfo = array[i];
			num ^= propertyInfo.GetValue(this, null).GetHashCode();
		}
		return num;
	}
}
