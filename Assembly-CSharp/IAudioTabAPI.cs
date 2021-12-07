using System;

// Token: 0x02000969 RID: 2409
public interface IAudioTabAPI
{
	// Token: 0x06004060 RID: 16480
	void Reset();

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x06004061 RID: 16481
	// (set) Token: 0x06004062 RID: 16482
	float MasterVolume { get; set; }

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x06004063 RID: 16483
	// (set) Token: 0x06004064 RID: 16484
	float SoundsEffectsVolume { get; set; }

	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x06004065 RID: 16485
	// (set) Token: 0x06004066 RID: 16486
	float MusicVolume { get; set; }

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x06004067 RID: 16487
	// (set) Token: 0x06004068 RID: 16488
	bool UseAltSounds { get; set; }

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x06004069 RID: 16489
	// (set) Token: 0x0600406A RID: 16490
	bool UseAltMenuMusic { get; set; }

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x0600406B RID: 16491
	// (set) Token: 0x0600406C RID: 16492
	bool UseAltBattleMusic { get; set; }

	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x0600406D RID: 16493
	// (set) Token: 0x0600406E RID: 16494
	float CharacterAnnouncerVolume { get; set; }
}
