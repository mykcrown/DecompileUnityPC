using System;

// Token: 0x02000B35 RID: 2869
public class Pair<T1, T2>
{
	// Token: 0x06005339 RID: 21305 RVA: 0x001AEAD6 File Offset: 0x001ACED6
	internal Pair(T1 first, T2 second)
	{
		this.First = first;
		this.Second = second;
	}

	// Token: 0x1700133C RID: 4924
	// (get) Token: 0x0600533A RID: 21306 RVA: 0x001AEAEC File Offset: 0x001ACEEC
	// (set) Token: 0x0600533B RID: 21307 RVA: 0x001AEAF4 File Offset: 0x001ACEF4
	public T1 First { get; private set; }

	// Token: 0x1700133D RID: 4925
	// (get) Token: 0x0600533C RID: 21308 RVA: 0x001AEAFD File Offset: 0x001ACEFD
	// (set) Token: 0x0600533D RID: 21309 RVA: 0x001AEB05 File Offset: 0x001ACF05
	public T2 Second { get; private set; }
}
