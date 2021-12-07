using System;
using System.Collections.Generic;

// Token: 0x02000AB2 RID: 2738
public static class EnumUtil
{
	// Token: 0x0600505B RID: 20571 RVA: 0x0014F408 File Offset: 0x0014D808
	public static int GetLength<T>()
	{
		if (!EnumUtil.cachedEnumLengths.ContainsKey(typeof(T)))
		{
			EnumUtil.cachedEnumLengths[typeof(T)] = EnumUtil.GetValues<T>().Length;
		}
		return EnumUtil.cachedEnumLengths[typeof(T)];
	}

	// Token: 0x0600505C RID: 20572 RVA: 0x0014F45D File Offset: 0x0014D85D
	public static T[] GetValues<T>()
	{
		return (T[])Enum.GetValues(typeof(T));
	}

	// Token: 0x0600505D RID: 20573 RVA: 0x0014F474 File Offset: 0x0014D874
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
		foreach (T t in EnumUtil.GetValues<T>())
		{
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
			foreach (string str2 in array)
			{
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

	// Token: 0x040033BB RID: 13243
	private static Dictionary<Type, int> cachedEnumLengths = new Dictionary<Type, int>();
}
