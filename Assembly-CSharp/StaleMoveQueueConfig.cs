using System;
using System.Collections.Generic;

// Token: 0x02000664 RID: 1636
[Serializable]
public class StaleMoveQueueConfig
{
	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x0600280A RID: 10250 RVA: 0x000C2B84 File Offset: 0x000C0F84
	public int queueSize
	{
		get
		{
			return this.queueReductionPercent.Length;
		}
	}

	// Token: 0x0600280B RID: 10251 RVA: 0x000C2B90 File Offset: 0x000C0F90
	public void Resize(int size)
	{
		List<int> list = new List<int>(this.queueReductionPercent);
		if (list.Count < size)
		{
			for (int i = list.Count; i < size; i++)
			{
				list.Add(0);
			}
		}
		else if (list.Count > size)
		{
			list.RemoveRange(size, list.Count - size);
		}
		this.queueReductionPercent = list.ToArray();
	}

	// Token: 0x04001D36 RID: 7478
	public int[] queueReductionPercent = new int[0];
}
