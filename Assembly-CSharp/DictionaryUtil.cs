using System;
using System.Collections.Generic;
using Beebyte.Obfuscator;

// Token: 0x02000AB1 RID: 2737
public static class DictionaryUtil
{
	// Token: 0x06005059 RID: 20569 RVA: 0x0014F2B8 File Offset: 0x0014D6B8
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
		foreach (T key in keys)
		{
			if (!b2.ContainsKey(key))
			{
				return false;
			}
			U u = b2[key];
			if (!u.Equals(b1[key]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600505A RID: 20570 RVA: 0x0014F384 File Offset: 0x0014D784
	[SkipRename]
	public static int GetDictionaryHashCode<T, U>(Dictionary<T, U> dictionary)
	{
		int num = 0;
		foreach (KeyValuePair<T, U> keyValuePair in dictionary)
		{
			int num2 = num;
			T key = keyValuePair.Key;
			num = (num2 ^ key.GetHashCode());
			int num3 = num;
			U value = keyValuePair.Value;
			num = (num3 ^ value.GetHashCode());
		}
		return num;
	}
}
