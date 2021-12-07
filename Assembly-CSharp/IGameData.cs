using System;

// Token: 0x02000415 RID: 1045
public interface IGameData
{
	// Token: 0x1700043B RID: 1083
	// (get) Token: 0x060015CB RID: 5579
	StageDataStore StageData { get; }

	// Token: 0x1700043C RID: 1084
	// (get) Token: 0x060015CC RID: 5580
	GameModeDataStore GameModeData { get; }

	// Token: 0x1700043D RID: 1085
	// (get) Token: 0x060015CD RID: 5581
	ConfigData ConfigData { get; }

	// Token: 0x060015CE RID: 5582
	bool IsFeatureEnabled(FeatureID feature);
}
