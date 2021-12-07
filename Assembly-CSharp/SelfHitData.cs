using System;

// Token: 0x020004FF RID: 1279
[Serializable]
public class SelfHitData
{
	// Token: 0x0400162C RID: 5676
	public KnockbackType knockbackType = KnockbackType.AwayUp;

	// Token: 0x0400162D RID: 5677
	public float damage;

	// Token: 0x0400162E RID: 5678
	public float baseKnockback = 20f;

	// Token: 0x0400162F RID: 5679
	public bool interruptMove;

	// Token: 0x04001630 RID: 5680
	public bool applyHitStun;

	// Token: 0x04001631 RID: 5681
	public int angleGranularity = 360;
}
