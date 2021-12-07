// Decompile from assembly: Assembly-CSharp.dll

using System;

public struct HashCode
{
	private readonly int value;

	private HashCode(int value)
	{
		this.value = value;
	}

	public static implicit operator int(HashCode hashCode)
	{
		return hashCode.value;
	}

	public static HashCode Of<T>(T item)
	{
		return new HashCode(HashCode.GetHashCode<T>(item));
	}

	public HashCode And<T>(T item)
	{
		return new HashCode(HashCode.CombineHashCodes(this.value, HashCode.GetHashCode<T>(item)));
	}

	private static int CombineHashCodes(int h1, int h2)
	{
		return (h1 << 5) + h1 ^ h2;
	}

	private static int GetHashCode<T>(T item)
	{
		return (item != null) ? item.GetHashCode() : 0;
	}
}
