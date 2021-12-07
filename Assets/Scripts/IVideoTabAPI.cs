// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IVideoTabAPI
{
	int QualityIndex
	{
		get;
		set;
	}

	WD_Resolution Resolution
	{
		get;
		set;
	}

	int DisplayIndex
	{
		get;
		set;
	}

	bool MultiSampleAntiAliasing
	{
		get;
		set;
	}

	bool VisibleInBackground
	{
		get;
		set;
	}

	bool ShowPerformance
	{
		get;
		set;
	}

	bool ShowSystemClock
	{
		get;
		set;
	}

	bool FullScreen
	{
		get;
		set;
	}

	bool PostProcessing
	{
		get;
		set;
	}

	bool MotionBlur
	{
		get;
		set;
	}

	bool Vsync
	{
		get;
		set;
	}

	ThreeTierQualityLevel ParticleQuality
	{
		get;
		set;
	}

	ThreeTierQualityLevel StageQuality
	{
		get;
		set;
	}

	ThreeTierQualityLevel MaterialQuality
	{
		get;
		set;
	}

	ThreeTierQualityLevel GraphicsQuality
	{
		get;
		set;
	}

	void Reset();

	void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer);
}
