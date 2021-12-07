// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class ArrayUtil
{
	private struct DelegateComparer<T> : IComparer<T>
	{
		private ArrayUtil.LessThanDelegate<T> lessThanFunc;

		public DelegateComparer(ArrayUtil.LessThanDelegate<T> lessThanFunc)
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

	public static void Sort<T>(T[] arr, ArrayUtil.LessThanDelegate<T> lessThanFunc)
	{
		Array.Sort<T>(arr, new ArrayUtil.DelegateComparer<T>(lessThanFunc));
	}

	public static void Swap<T>(IList<T> arr, int indexA, int indexB)
	{
		T value = arr[indexA];
		arr[indexA] = arr[indexB];
		arr[indexB] = value;
	}

	public static void BlockSwap<T>(IList<T> arr, int indexA, int indexB, int count)
	{
		if (indexA < 0 || indexB < 0 || indexA + count > arr.Count || indexB + count > arr.Count)
		{
			throw new ArgumentException("Combination of indices and block size would lead to invalid index.");
		}
		for (int i = 0; i < count; i++)
		{
			ArrayUtil.Swap<T>(arr, indexA + i, indexB + i);
		}
	}

	public static void RotateRight<T>(IList<T> arr, int count)
	{
		if (count < 0)
		{
			ArrayUtil.RotateRight<T>(arr, -count);
			return;
		}
		if (count == 0 || count == arr.Count)
		{
			return;
		}
		count %= arr.Count;
		List<T> list = new List<T>(arr);
		for (int i = 0; i < list.Count; i++)
		{
			int num = i - count;
			if (num < 0)
			{
				num += arr.Count;
			}
			arr[i] = list[num];
		}
	}

	public static void RotateLeft<T>(IList<T> arr, int count)
	{
		if (count < 0)
		{
			ArrayUtil.RotateRight<T>(arr, -count);
			return;
		}
		if (count == 0 || count == arr.Count)
		{
			return;
		}
		count %= arr.Count;
		List<T> list = new List<T>(arr);
		for (int i = 0; i < list.Count; i++)
		{
			int index = (i + count) % arr.Count;
			arr[i] = list[index];
		}
	}

	public static bool AreElementsIdentical<T>(IList<T> arrayA, IList<T> arrayB)
	{
		if (arrayA.Count != arrayB.Count)
		{
			return false;
		}
		for (int i = 0; i < arrayA.Count; i++)
		{
			T t = arrayA[i];
			if (!t.Equals(arrayB[i]))
			{
				return false;
			}
		}
		return true;
	}
}
