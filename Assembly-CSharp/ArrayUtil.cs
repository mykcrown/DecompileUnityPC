using System;
using System.Collections.Generic;

// Token: 0x02000A9A RID: 2714
public static class ArrayUtil
{
	// Token: 0x06004F98 RID: 20376 RVA: 0x0014CD11 File Offset: 0x0014B111
	public static void Sort<T>(T[] arr, ArrayUtil.LessThanDelegate<T> lessThanFunc)
	{
		Array.Sort<T>(arr, new ArrayUtil.DelegateComparer<T>(lessThanFunc));
	}

	// Token: 0x06004F99 RID: 20377 RVA: 0x0014CD24 File Offset: 0x0014B124
	public static void Swap<T>(IList<T> arr, int indexA, int indexB)
	{
		T value = arr[indexA];
		arr[indexA] = arr[indexB];
		arr[indexB] = value;
	}

	// Token: 0x06004F9A RID: 20378 RVA: 0x0014CD50 File Offset: 0x0014B150
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

	// Token: 0x06004F9B RID: 20379 RVA: 0x0014CDB0 File Offset: 0x0014B1B0
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

	// Token: 0x06004F9C RID: 20380 RVA: 0x0014CE2C File Offset: 0x0014B22C
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

	// Token: 0x06004F9D RID: 20381 RVA: 0x0014CEA0 File Offset: 0x0014B2A0
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

	// Token: 0x02000A9B RID: 2715
	private struct DelegateComparer<T> : IComparer<T>
	{
		// Token: 0x06004F9E RID: 20382 RVA: 0x0014CF00 File Offset: 0x0014B300
		public DelegateComparer(ArrayUtil.LessThanDelegate<T> lessThanFunc)
		{
			this.lessThanFunc = lessThanFunc;
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x0014CF09 File Offset: 0x0014B309
		public int Compare(T x, T y)
		{
			if (this.lessThanFunc(x, y))
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x040033A3 RID: 13219
		private ArrayUtil.LessThanDelegate<T> lessThanFunc;
	}

	// Token: 0x02000A9C RID: 2716
	// (Invoke) Token: 0x06004FA1 RID: 20385
	public delegate bool LessThanDelegate<T>(T a, T b);
}
