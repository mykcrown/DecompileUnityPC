using System;

// Token: 0x02000B29 RID: 2857
public struct HashCode
{
	// Token: 0x060052EF RID: 21231 RVA: 0x001AC7F9 File Offset: 0x001AABF9
	private HashCode(int value)
	{
		this.value = value;
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001AC802 File Offset: 0x001AAC02
	public static implicit operator int(HashCode hashCode)
	{
		return hashCode.value;
	}

	// Token: 0x060052F1 RID: 21233 RVA: 0x001AC80B File Offset: 0x001AAC0B
	public static HashCode Of<T>(T item)
	{
		return new HashCode(HashCode.GetHashCode<T>(item));
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001AC818 File Offset: 0x001AAC18
	public HashCode And<T>(T item)
	{
		return new HashCode(HashCode.CombineHashCodes(this.value, HashCode.GetHashCode<T>(item)));
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001AC830 File Offset: 0x001AAC30
	private static int CombineHashCodes(int h1, int h2)
	{
		return (h1 << 5) + h1 ^ h2;
	}

	// Token: 0x060052F4 RID: 21236 RVA: 0x001AC839 File Offset: 0x001AAC39
	private static int GetHashCode<T>(T item)
	{
		return (item != null) ? item.GetHashCode() : 0;
	}

	// Token: 0x040034B2 RID: 13490
	private readonly int value;
}
