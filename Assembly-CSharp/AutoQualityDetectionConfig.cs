using System;

// Token: 0x020003DA RID: 986
[Serializable]
public class AutoQualityDetectionConfig
{
	// Token: 0x04000EBE RID: 3774
	public int[] PassmarkCpuThresholds = new int[]
	{
		1500,
		3500,
		5000,
		8000,
		11000,
		18000
	};

	// Token: 0x04000EBF RID: 3775
	public int[] PassmarkGpuThresholds = new int[]
	{
		1100,
		2000,
		4000,
		8000,
		10000,
		12000
	};

	// Token: 0x04000EC0 RID: 3776
	public int MaxFallbackQualityLevel = 5;

	// Token: 0x04000EC1 RID: 3777
	public int MaxGraphicsMemoryToScan = 8;

	// Token: 0x04000EC2 RID: 3778
	public float GraphicsMemoryWeight = 0.75f;

	// Token: 0x04000EC3 RID: 3779
	public int MaxSystemMemoryToScan = 16;

	// Token: 0x04000EC4 RID: 3780
	public float SystemMemoryWeight = 0.5f;

	// Token: 0x04000EC5 RID: 3781
	public int MaxShaderLevelToScan = 50;

	// Token: 0x04000EC6 RID: 3782
	public float ShaderWeight = 0.75f;

	// Token: 0x04000EC7 RID: 3783
	public int MaxProcessorFrequencyToScan = 3000;

	// Token: 0x04000EC8 RID: 3784
	public float ProcessorFrequencyWeight = 1f;

	// Token: 0x04000EC9 RID: 3785
	public int MaxProcessorCountToScan = 8;

	// Token: 0x04000ECA RID: 3786
	public float ProcessorCountWeight = 0.75f;
}
