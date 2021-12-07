using System;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class AutoQualityDetection : IAutoQualityDetection
{
	// Token: 0x1700133E RID: 4926
	// (get) Token: 0x06005341 RID: 21313 RVA: 0x001AEB69 File Offset: 0x001ACF69
	// (set) Token: 0x06005342 RID: 21314 RVA: 0x001AEB71 File Offset: 0x001ACF71
	[Inject]
	public ConfigData Config { get; set; }

	// Token: 0x06005343 RID: 21315 RVA: 0x001AEB7C File Offset: 0x001ACF7C
	public static string SanitizedDeviceName(string name, bool applyGpuRules, bool applyCpuRules)
	{
		string result;
		try
		{
			name = name.ToLower().Replace("\"", string.Empty).Replace("-", " ");
			int num = name.IndexOf("@");
			if (num > 0)
			{
				name = name.Substring(0, num);
			}
			for (int i = name.IndexOf("("); i > 0; i = name.IndexOf("("))
			{
				int num2 = name.IndexOf(")", i);
				name = name.Substring(0, i) + name.Substring(num2 + 1);
			}
			if (applyGpuRules)
			{
				name = AutoQualityDetection.sanitizedGpuNameMangler(name);
			}
			if (applyCpuRules)
			{
				name = AutoQualityDetection.sanitizedCpuNameMangler(name);
			}
			result = Regex.Replace(name, "\\s+", " ").Trim();
		}
		catch
		{
			result = string.Empty;
		}
		return result;
	}

	// Token: 0x06005344 RID: 21316 RVA: 0x001AEC6C File Offset: 0x001AD06C
	private static string sanitizedGpuNameMangler(string name)
	{
		name = name.Replace("nvidia", string.Empty).Replace("amd", string.Empty).Replace("ati", string.Empty).Replace("graphics", string.Empty).Replace("adapter", string.Empty).Replace("series", string.Empty).Replace("edition", string.Empty).Replace("dual", string.Empty).Replace("ghz", string.Empty).Replace("oem", string.Empty).Replace("firepro 3d v", "firepro ");
		if (name.EndsWith(" gs"))
		{
			name = name.Substring(0, name.Length - 3) + "gs";
		}
		return name;
	}

	// Token: 0x06005345 RID: 21317 RVA: 0x001AED50 File Offset: 0x001AD150
	private static string sanitizedCpuNameMangler(string name)
	{
		int num = name.IndexOf("with");
		if (num > 0)
		{
			name = name.Substring(0, num);
		}
		int num2 = name.IndexOf("r6,");
		if (num2 > 0)
		{
			name = name.Substring(0, num2);
		}
		num2 = name.IndexOf("radeon");
		if (num2 > 0)
		{
			name = name.Substring(0, num2);
		}
		int num3 = name.IndexOf(",");
		if (num3 > 0)
		{
			name = name.Substring(0, num3);
		}
		int num4 = name.IndexOf(" e3");
		if (num4 > 0 && name.Length > num4 + 1 && name[num4 + 4] != ' ')
		{
			name = name.Substring(0, num4 + 3) + ' ' + name.Substring(num4 + 3, name.Length - (num4 + 3));
		}
		name = name.Replace(" 0 ", " ").Replace("intel", string.Empty).Replace("amd", string.Empty).Replace("dual core", string.Empty).Replace("dual cpu", string.Empty).Replace(" dual ", " ").Replace("quad core", string.Empty).Replace("quad cpu", string.Empty).Replace(" quad ", " ").Replace("six core", string.Empty).Replace("eight core", string.Empty).Replace("8 core", string.Empty).Replace("sixteen core", string.Empty).Replace("16 core", string.Empty).Replace("24 core", string.Empty).Replace("32 core", string.Empty).Replace("processor", string.Empty).Replace("apu", string.Empty).Replace("cpu", string.Empty);
		string text = string.Empty;
		for (int i = 1; i < name.Length - 1; i++)
		{
			if (char.IsLetter(name[i]) && name[i - 1] == ' ' && name[i + 1] == ' ')
			{
				text += name[i];
				name = name.Replace(" " + name[i] + " ", string.Empty);
			}
		}
		text = text.Replace("q", "qm");
		name = name.Trim() + text;
		return name;
	}

	// Token: 0x1700133F RID: 4927
	// (get) Token: 0x06005346 RID: 21318 RVA: 0x001AF008 File Offset: 0x001AD408
	public float QualityPercentFallback
	{
		get
		{
			if (!this.qualityPercentFallbackWasCached)
			{
				AutoQualityDetectionConfig autoQualityDetectionConfig = this.Config.autoQualityDetectionConfig;
				this.qualityPercentFallbackWasCached = true;
				float num = Mathf.Min((float)SystemInfo.graphicsMemorySize, 1024f * (float)autoQualityDetectionConfig.MaxGraphicsMemoryToScan) / (1024f * (float)autoQualityDetectionConfig.MaxGraphicsMemoryToScan) * autoQualityDetectionConfig.GraphicsMemoryWeight;
				float num2 = (float)Mathf.Min(SystemInfo.graphicsShaderLevel, autoQualityDetectionConfig.MaxShaderLevelToScan) / (float)autoQualityDetectionConfig.MaxShaderLevelToScan * autoQualityDetectionConfig.ShaderWeight;
				float num3 = Mathf.Min((float)SystemInfo.systemMemorySize, 1024f * (float)autoQualityDetectionConfig.MaxSystemMemoryToScan) / (1024f * (float)autoQualityDetectionConfig.MaxSystemMemoryToScan) * autoQualityDetectionConfig.SystemMemoryWeight;
				float num4 = (float)Mathf.Min(SystemInfo.processorCount, autoQualityDetectionConfig.MaxProcessorCountToScan) / (float)autoQualityDetectionConfig.MaxProcessorCountToScan * autoQualityDetectionConfig.ProcessorCountWeight;
				float num5 = (float)Mathf.Min(SystemInfo.processorFrequency, autoQualityDetectionConfig.MaxProcessorFrequencyToScan) / (float)autoQualityDetectionConfig.MaxProcessorFrequencyToScan * autoQualityDetectionConfig.ProcessorFrequencyWeight;
				float num6 = autoQualityDetectionConfig.GraphicsMemoryWeight + autoQualityDetectionConfig.SystemMemoryWeight + autoQualityDetectionConfig.ShaderWeight + autoQualityDetectionConfig.ProcessorFrequencyWeight + autoQualityDetectionConfig.ProcessorCountWeight;
				this.cachedQualityPercentFallback = (num + num2 + num3 + num4 + num5) / num6;
			}
			return this.cachedQualityPercentFallback;
		}
	}

	// Token: 0x17001340 RID: 4928
	// (get) Token: 0x06005347 RID: 21319 RVA: 0x001AF132 File Offset: 0x001AD532
	public string SanitizedGpuName
	{
		get
		{
			if (this.sanitizedGpuName == null)
			{
				this.sanitizedGpuName = AutoQualityDetection.SanitizedDeviceName(SystemInfo.graphicsDeviceName, true, false);
			}
			return this.sanitizedGpuName;
		}
	}

	// Token: 0x17001341 RID: 4929
	// (get) Token: 0x06005348 RID: 21320 RVA: 0x001AF157 File Offset: 0x001AD557
	public int PassmarkGpuScore
	{
		get
		{
			if (!this.gpuScoreWasCached)
			{
				this.gpuScoreWasCached = true;
				PassmarkGPUScores.Scores.TryGetValue(this.SanitizedGpuName, out this.cachedGpuScore);
			}
			return this.cachedGpuScore;
		}
	}

	// Token: 0x17001342 RID: 4930
	// (get) Token: 0x06005349 RID: 21321 RVA: 0x001AF188 File Offset: 0x001AD588
	public string SanitizedCpuName
	{
		get
		{
			if (this.sanitizedCpuName == null)
			{
				this.sanitizedCpuName = AutoQualityDetection.SanitizedDeviceName(SystemInfo.processorType, false, true);
			}
			return this.sanitizedCpuName;
		}
	}

	// Token: 0x17001343 RID: 4931
	// (get) Token: 0x0600534A RID: 21322 RVA: 0x001AF1AD File Offset: 0x001AD5AD
	public int PassmarkCpuScore
	{
		get
		{
			if (!this.cpuScoreWasCached)
			{
				this.cpuScoreWasCached = true;
				PassmarkCPUScores.Scores.TryGetValue(this.SanitizedCpuName, out this.cachedCpuScore);
			}
			return this.cachedCpuScore;
		}
	}

	// Token: 0x0600534B RID: 21323 RVA: 0x001AF1E0 File Offset: 0x001AD5E0
	private int thresholdForScore(int[] thresholds, int score)
	{
		for (int i = 0; i < thresholds.Length; i++)
		{
			if (thresholds[i] > score)
			{
				return i;
			}
		}
		return thresholds.Length;
	}

	// Token: 0x17001344 RID: 4932
	// (get) Token: 0x0600534C RID: 21324 RVA: 0x001AF20F File Offset: 0x001AD60F
	public int PassmarkCpuQualityLevel
	{
		get
		{
			return (this.PassmarkCpuScore != 0) ? this.thresholdForScore(this.Config.autoQualityDetectionConfig.PassmarkCpuThresholds, this.PassmarkCpuScore) : -1;
		}
	}

	// Token: 0x17001345 RID: 4933
	// (get) Token: 0x0600534D RID: 21325 RVA: 0x001AF23E File Offset: 0x001AD63E
	public int PassmarkGpuQualityLevel
	{
		get
		{
			return (this.PassmarkGpuScore != 0) ? this.thresholdForScore(this.Config.autoQualityDetectionConfig.PassmarkGpuThresholds, this.PassmarkGpuScore) : -1;
		}
	}

	// Token: 0x17001346 RID: 4934
	// (get) Token: 0x0600534E RID: 21326 RVA: 0x001AF26D File Offset: 0x001AD66D
	public int FallbackQualityLevel
	{
		get
		{
			return (int)(this.QualityPercentFallback * (float)this.Config.autoQualityDetectionConfig.MaxFallbackQualityLevel);
		}
	}

	// Token: 0x040034E1 RID: 13537
	private bool qualityPercentFallbackWasCached;

	// Token: 0x040034E2 RID: 13538
	private float cachedQualityPercentFallback;

	// Token: 0x040034E3 RID: 13539
	private string sanitizedGpuName;

	// Token: 0x040034E4 RID: 13540
	private int cachedGpuScore;

	// Token: 0x040034E5 RID: 13541
	private bool gpuScoreWasCached;

	// Token: 0x040034E6 RID: 13542
	private string sanitizedCpuName;

	// Token: 0x040034E7 RID: 13543
	private int cachedCpuScore;

	// Token: 0x040034E8 RID: 13544
	private bool cpuScoreWasCached;
}
