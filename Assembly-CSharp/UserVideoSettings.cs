using System;
using Beebyte.Obfuscator;

// Token: 0x02000B94 RID: 2964
[Skip]
[Serializable]
public class UserVideoSettings
{
	// Token: 0x04003664 RID: 13924
	public static int CURRENT_VERSION = 6;

	// Token: 0x04003665 RID: 13925
	public int qualityIndex = 3;

	// Token: 0x04003666 RID: 13926
	public WD_Resolution fullScreenResolution;

	// Token: 0x04003667 RID: 13927
	public WD_Resolution windowedResolution;

	// Token: 0x04003668 RID: 13928
	public int displayIndex;

	// Token: 0x04003669 RID: 13929
	public bool multiSampleAntiAliasing;

	// Token: 0x0400366A RID: 13930
	public bool visibleInBackground;

	// Token: 0x0400366B RID: 13931
	public bool fullScreen = true;

	// Token: 0x0400366C RID: 13932
	public bool showPerformance;

	// Token: 0x0400366D RID: 13933
	public bool showSystemClock;

	// Token: 0x0400366E RID: 13934
	public bool postProcessing = true;

	// Token: 0x0400366F RID: 13935
	public bool motionBlur;

	// Token: 0x04003670 RID: 13936
	public bool vsync = true;

	// Token: 0x04003671 RID: 13937
	public int version;

	// Token: 0x04003672 RID: 13938
	public ThreeTierQualityLevel particleQualityLevel = ThreeTierQualityLevel.Medium;

	// Token: 0x04003673 RID: 13939
	public ThreeTierQualityLevel stageQualityLevel = ThreeTierQualityLevel.High;

	// Token: 0x04003674 RID: 13940
	public ThreeTierQualityLevel materialQualityLevel = ThreeTierQualityLevel.High;

	// Token: 0x04003675 RID: 13941
	public ThreeTierQualityLevel graphicsQualityLevel = ThreeTierQualityLevel.Medium;
}
