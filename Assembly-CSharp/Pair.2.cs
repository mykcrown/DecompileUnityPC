using System;

// Token: 0x02000B36 RID: 2870
public static class Pair
{
	// Token: 0x0600533E RID: 21310 RVA: 0x001AEB10 File Offset: 0x001ACF10
	public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second)
	{
		return new Pair<T1, T2>(first, second);
	}
}
