using System;

// Token: 0x02000B39 RID: 2873
public interface IAutoQualityDetection
{
	// Token: 0x17001347 RID: 4935
	// (get) Token: 0x0600534F RID: 21327
	float QualityPercentFallback { get; }

	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x06005350 RID: 21328
	int PassmarkCpuScore { get; }

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x06005351 RID: 21329
	int PassmarkGpuScore { get; }

	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x06005352 RID: 21330
	int PassmarkCpuQualityLevel { get; }

	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x06005353 RID: 21331
	int PassmarkGpuQualityLevel { get; }

	// Token: 0x1700134C RID: 4940
	// (get) Token: 0x06005354 RID: 21332
	int FallbackQualityLevel { get; }

	// Token: 0x1700134D RID: 4941
	// (get) Token: 0x06005355 RID: 21333
	string SanitizedGpuName { get; }

	// Token: 0x1700134E RID: 4942
	// (get) Token: 0x06005356 RID: 21334
	string SanitizedCpuName { get; }
}
