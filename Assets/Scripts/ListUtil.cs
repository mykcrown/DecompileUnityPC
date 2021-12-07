// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using MemberwiseEquality;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListUtil
{
	[SkipRename]
	private class EnumerableMemberwiseElementEqualityComparer<T> : EqualityComparer<T> where T : MemberwiseEqualityObject
	{
		public override bool Equals(T val1, T val2)
		{
			return val1.MemberwiseEquals(val2);
		}

		public override int GetHashCode(T obj)
		{
			return obj.GetMemberwiseHashCode();
		}
	}

	private struct DelegateComparer<T> : IComparer<T>
	{
		private ListUtil.LessThanDelegate<T> lessThanFunc;

		public DelegateComparer(ListUtil.LessThanDelegate<T> lessThanFunc)
		{
			this.lessThanFunc = lessThanFunc;
		}

		public int Compare(T x, T y)
		{
			if (this.lessThanFunc(x, y))
			{
				return -1;
			}
			return 1;
		}
	}

	public delegate bool LessThanDelegate<T>(T a, T b);

	public static List<T> GenerateCompactList<T>(List<T> sourceList, bool addBuffer)
	{
		List<T> list = new List<T>();
		foreach (T current in sourceList)
		{
			if (current != null)
			{
				list.Add(current);
			}
		}
		if (addBuffer)
		{
			list.Add(default(T));
		}
		return list;
	}

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

	[SkipRename]
	public static bool MemberwiseEnumerablesAreEqual<T>(IEnumerable<T> b1, IEnumerable<T> b2) where T : MemberwiseEqualityObject
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.Count<T>() == b2.Count<T>() && b1.SequenceEqual(b2, new ListUtil.EnumerableMemberwiseElementEqualityComparer<T>()));
	}

	[SkipRename]
	public static bool EnumerablesAreEqual<T>(IEnumerable<T> b1, IEnumerable<T> b2)
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.Count<T>() == b2.Count<T>() && b1.SequenceEqual(b2));
	}

	[SkipRename]
	public static bool FixedCapacityListAreEqual<T>(FixedCapacityList<T> b1, FixedCapacityList<T> b2)
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.List.Count<T>() == b2.List.Count<T>() && b1.List.SequenceEqual(b2.List));
	}

	[SkipRename]
	public static bool MemberwiseFixedCapacityListAreEqual<T>(FixedCapacityList<T> b1, FixedCapacityList<T> b2) where T : MemberwiseEqualityObject
	{
		return object.ReferenceEquals(b1, b2) || (b1 != null && b2 != null && b1.List.Count<T>() == b2.List.Count<T>() && b1.List.SequenceEqual(b2.List, new ListUtil.EnumerableMemberwiseElementEqualityComparer<T>()));
	}

	[SkipRename]
	public static int GetMemberwiseArrayHashCode<T>(T[] array) where T : MemberwiseEqualityObject
	{
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			T t = array[i];
			num ^= t.GetMemberwiseHashCode();
		}
		return num;
	}

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

	[SkipRename]
	public static int GetMemberwiseEnumerableHashCode<T>(IEnumerable<T> list) where T : MemberwiseEqualityObject
	{
		int num = 0;
		foreach (T current in list)
		{
			num ^= current.GetMemberwiseHashCode();
		}
		return num;
	}

	[SkipRename]
	public static int GetListHashCode<T>(List<T> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int arg_1F_0 = num;
			T t = list[i];
			num = (arg_1F_0 ^ t.GetHashCode());
		}
		return num;
	}

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

	[SkipRename]
	public static int GetEnumerableHashCode<T>(IEnumerable<T> list)
	{
		int num = 0;
		foreach (T current in list)
		{
			num ^= current.GetHashCode();
		}
		return num;
	}

	[SkipRename]
	public static int GetFixedCapacityListHashCode<T>(FixedCapacityList<T> list)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			int arg_1F_0 = num;
			T t = list[i];
			num = (arg_1F_0 ^ t.GetHashCode());
		}
		return num;
	}

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

	public static void Sort<T>(List<T> list, ListUtil.LessThanDelegate<T> lessThanFunc)
	{
		list.Sort(new ListUtil.DelegateComparer<T>(lessThanFunc));
	}

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

	public static T SelectRandom<T>(this List<T> list)
	{
		return list[UnityEngine.Random.Range(0, list.Count)];
	}
}
