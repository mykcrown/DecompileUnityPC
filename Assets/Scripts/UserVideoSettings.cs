// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

[Skip]
[Serializable]
public class UserVideoSettings
{
	public static int CURRENT_VERSION = 6;

	public int qualityIndex = 3;

	public WD_Resolution fullScreenResolution;

	public WD_Resolution windowedResolution;

	public int displayIndex;

	public bool multiSampleAntiAliasing;

	public bool visibleInBackground;

	public bool fullScreen = true;

	public bool showPerformance;

	public bool showSystemClock;

	public bool postProcessing = true;

	public bool motionBlur;

	public bool vsync = true;

	public int version;

	public ThreeTierQualityLevel particleQualityLevel = ThreeTierQualityLevel.Medium;

	public ThreeTierQualityLevel stageQualityLevel = ThreeTierQualityLevel.High;

	public ThreeTierQualityLevel materialQualityLevel = ThreeTierQualityLevel.High;

	public ThreeTierQualityLevel graphicsQualityLevel = ThreeTierQualityLevel.Medium;
}
