using System;
using System.Collections.Generic;

// Token: 0x0200098C RID: 2444
public interface IVideoTabAPI
{
	// Token: 0x06004264 RID: 16996
	void Reset();

	// Token: 0x06004265 RID: 16997
	void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer);

	// Token: 0x17000FA7 RID: 4007
	// (get) Token: 0x06004266 RID: 16998
	// (set) Token: 0x06004267 RID: 16999
	int QualityIndex { get; set; }

	// Token: 0x17000FA8 RID: 4008
	// (get) Token: 0x06004268 RID: 17000
	// (set) Token: 0x06004269 RID: 17001
	WD_Resolution Resolution { get; set; }

	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x0600426A RID: 17002
	// (set) Token: 0x0600426B RID: 17003
	int DisplayIndex { get; set; }

	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x0600426C RID: 17004
	// (set) Token: 0x0600426D RID: 17005
	bool MultiSampleAntiAliasing { get; set; }

	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x0600426E RID: 17006
	// (set) Token: 0x0600426F RID: 17007
	bool VisibleInBackground { get; set; }

	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x06004270 RID: 17008
	// (set) Token: 0x06004271 RID: 17009
	bool ShowPerformance { get; set; }

	// Token: 0x17000FAD RID: 4013
	// (get) Token: 0x06004272 RID: 17010
	// (set) Token: 0x06004273 RID: 17011
	bool ShowSystemClock { get; set; }

	// Token: 0x17000FAE RID: 4014
	// (get) Token: 0x06004274 RID: 17012
	// (set) Token: 0x06004275 RID: 17013
	bool FullScreen { get; set; }

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x06004276 RID: 17014
	// (set) Token: 0x06004277 RID: 17015
	bool PostProcessing { get; set; }

	// Token: 0x17000FB0 RID: 4016
	// (get) Token: 0x06004278 RID: 17016
	// (set) Token: 0x06004279 RID: 17017
	bool MotionBlur { get; set; }

	// Token: 0x17000FB1 RID: 4017
	// (get) Token: 0x0600427A RID: 17018
	// (set) Token: 0x0600427B RID: 17019
	bool Vsync { get; set; }

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x0600427C RID: 17020
	// (set) Token: 0x0600427D RID: 17021
	ThreeTierQualityLevel ParticleQuality { get; set; }

	// Token: 0x17000FB3 RID: 4019
	// (get) Token: 0x0600427E RID: 17022
	// (set) Token: 0x0600427F RID: 17023
	ThreeTierQualityLevel StageQuality { get; set; }

	// Token: 0x17000FB4 RID: 4020
	// (get) Token: 0x06004280 RID: 17024
	// (set) Token: 0x06004281 RID: 17025
	ThreeTierQualityLevel MaterialQuality { get; set; }

	// Token: 0x17000FB5 RID: 4021
	// (get) Token: 0x06004282 RID: 17026
	// (set) Token: 0x06004283 RID: 17027
	ThreeTierQualityLevel GraphicsQuality { get; set; }
}
