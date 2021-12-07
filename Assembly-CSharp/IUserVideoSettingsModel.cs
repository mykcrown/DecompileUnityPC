using System;
using System.Collections.Generic;

// Token: 0x02000B97 RID: 2967
public interface IUserVideoSettingsModel
{
	// Token: 0x060055A6 RID: 21926
	void Init();

	// Token: 0x060055A7 RID: 21927
	void Reset();

	// Token: 0x060055A8 RID: 21928
	void ApplySecondarySettingsFromQualityIndex();

	// Token: 0x060055A9 RID: 21929
	void Update();

	// Token: 0x060055AA RID: 21930
	void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer);

	// Token: 0x170013BB RID: 5051
	// (get) Token: 0x060055AB RID: 21931
	// (set) Token: 0x060055AC RID: 21932
	int QualityIndex { get; set; }

	// Token: 0x170013BC RID: 5052
	// (get) Token: 0x060055AD RID: 21933
	// (set) Token: 0x060055AE RID: 21934
	WD_Resolution Resolution { get; set; }

	// Token: 0x170013BD RID: 5053
	// (get) Token: 0x060055AF RID: 21935
	// (set) Token: 0x060055B0 RID: 21936
	int DisplayIndex { get; set; }

	// Token: 0x170013BE RID: 5054
	// (get) Token: 0x060055B1 RID: 21937
	// (set) Token: 0x060055B2 RID: 21938
	bool MultiSampleAntiAliasing { get; set; }

	// Token: 0x170013BF RID: 5055
	// (get) Token: 0x060055B3 RID: 21939
	// (set) Token: 0x060055B4 RID: 21940
	bool VisibleInBackground { get; set; }

	// Token: 0x170013C0 RID: 5056
	// (get) Token: 0x060055B5 RID: 21941
	// (set) Token: 0x060055B6 RID: 21942
	bool FullScreen { get; set; }

	// Token: 0x170013C1 RID: 5057
	// (get) Token: 0x060055B7 RID: 21943
	// (set) Token: 0x060055B8 RID: 21944
	bool ShowPerformance { get; set; }

	// Token: 0x170013C2 RID: 5058
	// (get) Token: 0x060055B9 RID: 21945
	// (set) Token: 0x060055BA RID: 21946
	bool ShowSystemClock { get; set; }

	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x060055BB RID: 21947
	// (set) Token: 0x060055BC RID: 21948
	bool PostProcessing { get; set; }

	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x060055BD RID: 21949
	// (set) Token: 0x060055BE RID: 21950
	bool MotionBlur { get; set; }

	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x060055BF RID: 21951
	// (set) Token: 0x060055C0 RID: 21952
	bool Vsync { get; set; }

	// Token: 0x170013C6 RID: 5062
	// (get) Token: 0x060055C1 RID: 21953
	// (set) Token: 0x060055C2 RID: 21954
	ThreeTierQualityLevel ParticleQuality { get; set; }

	// Token: 0x170013C7 RID: 5063
	// (get) Token: 0x060055C3 RID: 21955
	// (set) Token: 0x060055C4 RID: 21956
	ThreeTierQualityLevel StageQuality { get; set; }

	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x060055C5 RID: 21957
	// (set) Token: 0x060055C6 RID: 21958
	ThreeTierQualityLevel MaterialQuality { get; set; }

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x060055C7 RID: 21959
	// (set) Token: 0x060055C8 RID: 21960
	ThreeTierQualityLevel GraphicsQuality { get; set; }
}
