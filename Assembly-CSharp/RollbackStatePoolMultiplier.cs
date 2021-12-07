using System;

// Token: 0x0200084A RID: 2122
[AttributeUsage(AttributeTargets.Class)]
public class RollbackStatePoolMultiplier : Attribute
{
	// Token: 0x06003517 RID: 13591 RVA: 0x000F952A File Offset: 0x000F792A
	public RollbackStatePoolMultiplier(int poolSizeMultiplier)
	{
		this.PoolSizeMultiplier = poolSizeMultiplier;
	}

	// Token: 0x17000CE4 RID: 3300
	// (get) Token: 0x06003518 RID: 13592 RVA: 0x000F9539 File Offset: 0x000F7939
	// (set) Token: 0x06003519 RID: 13593 RVA: 0x000F9541 File Offset: 0x000F7941
	public int PoolSizeMultiplier { get; private set; }
}
