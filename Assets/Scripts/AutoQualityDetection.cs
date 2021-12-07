// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class AutoQualityDetection : IAutoQualityDetection
{
	private bool qualityPercentFallbackWasCached;

	private float cachedQualityPercentFallback;

	private string sanitizedGpuName;

	private int cachedGpuScore;

	private bool gpuScoreWasCached;

	private string sanitizedCpuName;

	private int cachedCpuScore;

	private bool cpuScoreWasCached;

	[Inject]
	public ConfigData Config
	{
		get;
		set;
	}

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

	public int PassmarkCpuQualityLevel
	{
		get
		{
			return (this.PassmarkCpuScore != 0) ? this.thresholdForScore(this.Config.autoQualityDetectionConfig.PassmarkCpuThresholds, this.PassmarkCpuScore) : (-1);
		}
	}

	public int PassmarkGpuQualityLevel
	{
		get
		{
			return (this.PassmarkGpuScore != 0) ? this.thresholdForScore(this.Config.autoQualityDetectionConfig.PassmarkGpuThresholds, this.PassmarkGpuScore) : (-1);
		}
	}

	public int FallbackQualityLevel
	{
		get
		{
			return (int)(this.QualityPercentFallback * (float)this.Config.autoQualityDetectionConfig.MaxFallbackQualityLevel);
		}
	}

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

	private static string sanitizedGpuNameMangler(string name)
	{
		name = name.Replace("nvidia", string.Empty).Replace("amd", string.Empty).Replace("ati", string.Empty).Replace("graphics", string.Empty).Replace("adapter", string.Empty).Replace("series", string.Empty).Replace("edition", string.Empty).Replace("dual", string.Empty).Replace("ghz", string.Empty).Replace("oem", string.Empty).Replace("firepro 3d v", "firepro ");
		if (name.EndsWith(" gs"))
		{
			name = name.Substring(0, name.Length - 3) + "gs";
		}
		return name;
	}

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
}
