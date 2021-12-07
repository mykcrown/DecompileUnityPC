using System;
using System.Collections.Generic;
using System.Linq;
using Beebyte.Obfuscator;
using MemberwiseEquality;
using UnityEngine;

// Token: 0x02000B0A RID: 2826
public static class ListUtil
{
	// Token: 0x06005128 RID: 20776 RVA: 0x00151890 File Offset: 0x0014FC90
	public static List<T> GenerateCompactList<T>(List<T> sourceList, bool addBuffer)
	{
		List<T> list = new List<T>();
		foreach (T t in sourceList)
		{
			if (t != null)
			{
				list.Add(t);
			}
		}
		if (addBuffer)
		{
			list.Add(default(T));
		}
		return list;
	}

	// Token: 0x06005129 RID: 20777 RVA: 0x00151910 File Offset: 0x0014FD10
	public static void Resize<T>(this List<T> self, int size, Func<T> createDefault)
	{
		if (size <= 0)
		{
			self.Clear();
			return;
		}
		if (size < self.Count)
		{
			self.RemoveRange(size, self.Count - size);
		}
		else if (size > self.Count)
		{
			int num = size - self.Count;
			for (int i = 0; i < num; i++)
			{
				self.Add(createDefault());
			}
		}
	}

	// Token: 0x0600512A RID: 20778 RVA: 0x0015197E File Offset: 0x0014FD7E
	[SkipRename]
	public static bool MemberwiseEnumerablesAreEqual<T>(IEnumerable<T> b1, IEnumerable<T> b2) where T : MemberwiseEqualityObject
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.Count<T>() == b2.Count<T>() && b1.SequenceEqual(b2, new ListUtil.EnumerableMemberwiseElementEqualityComparer<T>()));
	}

	// Token: 0x0600512B RID: 20779 RVA: 0x001519BB File Offset: 0x0014FDBB
	[SkipRename]
	public static bool EnumerablesAreEqual<T>(IEnumerable<T> b1, IEnumerable<T> b2)
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.Count<T>() == b2.Count<T>() && b1.SequenceEqual(b2));
	}

	// Token: 0x0600512C RID: 20780 RVA: 0x001519F4 File Offset: 0x0014FDF4
	[SkipRename]
	public static bool FixedCapacityListAreEqual<T>(FixedCapacityList<T> b1, FixedCapacityList<T> b2)
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.List.Count<T>() == b2.List.Count<T>() && b1.List.SequenceEqual(b2.List));
	}

	// Token: 0x0600512D RID: 20781 RVA: 0x00151A4C File Offset: 0x0014FE4C
	[SkipRename]
	public static bool MemberwiseFixedCapacityListAreEqual<T>(FixedCapacityList<T> b1, FixedCapacityList<T> b2) where T : MemberwiseEqualityObject
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.List.Count<T>() == b2.List.Count<T>() && b1.List.SequenceEqual(b2.List, new ListUtil.EnumerableMemberwiseElementEqualityComparer<T>()));
	}

	// Token: 0x0600512E RID: 20782 RVA: 0x00151AA8 File Offset: 0x0014FEA8
	[SkipRename]
	public static int GetMemberwiseArrayHashCode<T>(T[] array) where T : MemberwiseEqualityObject
	{
		int num = 0;
		foreach (T t in array)
		{
			num ^= t.GetMemberwiseHashCode();
		}
		return num;
	}

	// Token: 0x0600512F RID: 20783 RVA: 0x00151AE4 File Offset: 0x0014FEE4
	[SkipRename]
	public static int GetMemberwiseListHashCode<T>(List<T> list) where T : MemberwiseEqualityObject
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			T t = list[i];
			num ^= t.GetMemberwiseHashCode();
		}
		return num;
	}

	// Token: 0x06005130 RID: 20784 RVA: 0x00151B24 File Offset: 0x0014FF24
	[SkipRename]
	public static int GetMemberwiseEnumerableHashCode<T>(IEnumerable<T> list) where T : MemberwiseEqualityObject
	{
		int num = 0;
		foreach (T t in list)
		{
			num ^= t.GetMemberwiseHashCode();
		}
		return num;
	}

	// Token: 0x06005131 RID: 20785 RVA: 0x00151B84 File Offset: 0x0014FF84
	[SkipRename]
	public static int GetListHashCode<T>(List<T> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = num;
			T t = list[i];
			num = (num2 ^ t.GetHashCode());
		}
		return num;
	}

	// Token: 0x06005132 RID: 20786 RVA: 0x00151BC4 File Offset: 0x0014FFC4
	[SkipRename]
	public static int GetEnumListHashCode<T>(List<T> list) where T : struct, IConvertible
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num ^= CastTo<int>.From<T>(list[i]);
		}
		return num;
	}

	// Token: 0x06005133 RID: 20787 RVA: 0x00151BFC File Offset: 0x0014FFFC
	[SkipRename]
	public static int GetArrayHashCode<T>(T[] array)
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num ^= array[i].GetHashCode();
		}
		return num;
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x00151C38 File Offset: 0x00150038
	[SkipRename]
	public static int GetEnumArrayHashCode<T>(T[] array) where T : struct, IConvertible
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num ^= CastTo<int>.From<T>(array[i]);
		}
		return num;
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x00151C6C File Offset: 0x0015006C
	[SkipRename]
	public static int GetEnumerableHashCode<T>(IEnumerable<T> list)
	{
		int num = 0;
		foreach (T t in list)
		{
			num ^= t.GetHashCode();
		}
		return num;
	}

	// Token: 0x06005136 RID: 20790 RVA: 0x00151CCC File Offset: 0x001500CC
	[SkipRename]
	public static int GetFixedCapacityListHashCode<T>(FixedCapacityList<T> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int num2 = num;
			T t = list[i];
			num = (num2 ^ t.GetHashCode());
		}
		return num;
	}

	// Token: 0x06005137 RID: 20791 RVA: 0x00151D0C File Offset: 0x0015010C
	[SkipRename]
	public static int GetFixedCapacityMemberwiseListHashCode<T>(FixedCapacityList<T> list) where T : MemberwiseEqualityObject
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			T t = list[i];
			num ^= t.GetMemberwiseHashCode();
		}
		return num;
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x00151D4B File Offset: 0x0015014B
	public static void Sort<T>(List<T> list, ListUtil.LessThanDelegate<T> lessThanFunc)
	{
		list.Sort(new ListUtil.DelegateComparer<T>(lessThanFunc));
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x00151D60 File Offset: 0x00150160
	public static void InsertionSort<T>(List<T> list, ListUtil.LessThanDelegate<T> lessThanFunc)
	{
		for (int i = 1; i < list.Count; i++)
		{
			int num = i;
			while (num > 0 && lessThanFunc(list[num], list[num - 1]))
			{
				T value = list[num];
				list[num] = list[num - 1];
				list[num - 1] = value;
				num--;
			}
		}
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x00151DD4 File Offset: 0x001501D4
	public static bool RemoveFirst<T>(this List<T> list, Predicate<T> predicate)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (predicate(list[i]))
			{
				list.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x00151E14 File Offset: 0x00150214
	public static bool RemoveLast<T>(this List<T> list, Predicate<T> predicate)
	{
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (predicate(list[i]))
			{
				list.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x00151E56 File Offset: 0x00150256
	public static T SelectRandom<T>(this List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	// Token: 0x02000B0B RID: 2827
	[SkipRename]
	private class EnumerableMemberwiseElementEqualityComparer<T> : EqualityComparer<T> where T : MemberwiseEqualityObject
	{
		// Token: 0x0600513E RID: 20798 RVA: 0x00151E72 File Offset: 0x00150272
		public override bool Equals(T val1, T val2)
		{
			return val1.MemberwiseEquals(val2);
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x00151E87 File Offset: 0x00150287
		public override int GetHashCode(T obj)
		{
			return obj.GetMemberwiseHashCode();
		}
	}

	// Token: 0x02000B0C RID: 2828
	private struct DelegateComparer<T> : IComparer<T>
	{
		// Token: 0x06005140 RID: 20800 RVA: 0x00151E96 File Offset: 0x00150296
		public DelegateComparer(ListUtil.LessThanDelegate<T> lessThanFunc)
		{
			this.lessThanFunc = lessThanFunc;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x00151E9F File Offset: 0x0015029F
		public int Compare(T x, T y)
		{
			if (this.lessThanFunc(x, y))
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x04003461 RID: 13409
		private ListUtil.LessThanDelegate<T> lessThanFunc;
	}

	// Token: 0x02000B0D RID: 2829
	// (Invoke) Token: 0x06005143 RID: 20803
	public delegate bool LessThanDelegate<T>(T a, T b);
}
