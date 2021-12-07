// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.Collections.Generic;

public static class DictionaryUtil
{
	[SkipRename]
	public static bool DictionariesAreEqual<T, U>(Dictionary<T, U> b1, Dictionary<T, U> b2)
	{
		if (object.ReferenceEquals(b1, b2))
		{
			return true;
		}
		if (b1 == null || b2 == null)
		{
			return false;
		}
		if (b1.Count != b2.Count)
		{
			return false;
		}
		Dictionary<T, U>.KeyCollection keys = b1.Keys;
		foreach (T current in keys)
		{
			if (!b2.ContainsKey(current))
			{
				bool result = false;
				return result;
			}
			U u = b2[current];
			if (!u.Equals(b1[current]))
			{
				bool result = false;
				return result;
			}
		}
		return true;
	}

	[SkipRename]
	public static int GetDictionaryHashCode<T, U>(Dictionary<T, U> dictionary)
	{
		int num = 0;
		foreach (KeyValuePair<T, U> current in dictionary)
		{
			int arg_2C_0 = num;
			T key = current.Key;
			num = (arg_2C_0 ^ key.GetHashCode());
			int arg_45_0 = num;
			U value = current.Value;
			num = (arg_45_0 ^ value.GetHashCode());
		}
		return num;
	}
}
