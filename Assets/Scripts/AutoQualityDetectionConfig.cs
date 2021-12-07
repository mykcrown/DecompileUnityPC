// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class AutoQualityDetectionConfig
{
	public int[] PassmarkCpuThresholds = new int[]
	{
		1500,
		3500,
		5000,
		8000,
		11000,
		18000
	};

	public int[] PassmarkGpuThresholds = new int[]
	{
		1100,
		2000,
		4000,
		8000,
		10000,
		12000
	};

	public int MaxFallbackQualityLevel = 5;

	public int MaxGraphicsMemoryToScan = 8;

	public float GraphicsMemoryWeight = 0.75f;

	public int MaxSystemMemoryToScan = 16;

	public float SystemMemoryWeight = 0.5f;

	public int MaxShaderLevelToScan = 50;

	public float ShaderWeight = 0.75f;

	public int MaxProcessorFrequencyToScan = 3000;

	public float ProcessorFrequencyWeight = 1f;

	public int MaxProcessorCountToScan = 8;

	public float ProcessorCountWeight = 0.75f;
}
