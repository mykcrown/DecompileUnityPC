using System;

// Token: 0x020003BC RID: 956
public class HitContextPool : IHitContextPool
{
	// Token: 0x060014A1 RID: 5281 RVA: 0x000732B0 File Offset: 0x000716B0
	public HitContextPool()
	{
		for (int i = 0; i < this.arr.Length; i++)
		{
			this.arr[i] = new HitContext();
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000732F9 File Offset: 0x000716F9
	public HitContext GetNext()
	{
		this.index = (this.index + 1) % this.arr.Length;
		this.arr[this.index].Clear();
		return this.arr[this.index];
	}

	// Token: 0x04000DC2 RID: 3522
	private HitContext[] arr = new HitContext[256];

	// Token: 0x04000DC3 RID: 3523
	private int index;
}
