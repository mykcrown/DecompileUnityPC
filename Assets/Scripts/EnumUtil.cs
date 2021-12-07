// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class EnumUtil
{
	private static Dictionary<Type, int> cachedEnumLengths = new Dictionary<Type, int>();

	public static int GetLength<T>()
	{
		if (!EnumUtil.cachedEnumLengths.ContainsKey(typeof(T)))
		{
			EnumUtil.cachedEnumLengths[typeof(T)] = EnumUtil.GetValues<T>().Length;
		}
		return EnumUtil.cachedEnumLengths[typeof(T)];
	}

	public static T[] GetValues<T>()
	{
		return (T[])Enum.GetValues(typeof(T));
	}

	public static bool TryParse<T>(string str, ref T valueOut, bool ignoreCase = false, bool allowInts = false, bool allowMulti = false)
	{
		if (!typeof(T).IsEnum)
		{
			return false;
		}
		if (allowInts)
		{
			int num = 0;
			if (int.TryParse(str, out num) && Enum.IsDefined(typeof(T), num))
			{
				valueOut = (T)((object)num);
				return true;
			}
		}
		T[] values = EnumUtil.GetValues<T>();
		for (int i = 0; i < values.Length; i++)
		{
			T t = values[i];
			string text = t.ToString();
			bool flag = (!ignoreCase) ? text.Equals(str) : text.EqualsIgnoreCase(str);
			if (flag)
			{
				valueOut = t;
				return true;
			}
		}
		if (allowMulti)
		{
			string[] array = str.Split(new char[]
			{
				'|',
				','
			});
			int num2 = 0;
			string[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				string str2 = array2[j];
				T t2 = default(T);
				if (!EnumUtil.TryParse<T>(str2, ref t2, ignoreCase, false, false))
				{
					return false;
				}
				num2 |= (int)((object)t2);
			}
			valueOut = (T)((object)num2);
			return true;
		}
		return false;
	}
}
