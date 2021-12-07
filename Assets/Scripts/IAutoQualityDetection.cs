// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IAutoQualityDetection
{
	float QualityPercentFallback
	{
		get;
	}

	int PassmarkCpuScore
	{
		get;
	}

	int PassmarkGpuScore
	{
		get;
	}

	int PassmarkCpuQualityLevel
	{
		get;
	}

	int PassmarkGpuQualityLevel
	{
		get;
	}

	int FallbackQualityLevel
	{
		get;
	}

	string SanitizedGpuName
	{
		get;
	}

	string SanitizedCpuName
	{
		get;
	}
}
