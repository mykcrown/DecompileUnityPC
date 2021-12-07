using System;

// Token: 0x02000351 RID: 849
[Serializable]
public class AnnouncementStatData
{
	// Token: 0x04000B79 RID: 2937
	public int statQuantityRequired;

	// Token: 0x04000B7A RID: 2938
	public StatType stat;

	// Token: 0x04000B7B RID: 2939
	public float intervalSeconds;

	// Token: 0x04000B7C RID: 2940
	public string announcement;

	// Token: 0x04000B7D RID: 2941
	public bool perPlayer;

	// Token: 0x04000B7E RID: 2942
	public StatTriggerMode triggerMode;
}
