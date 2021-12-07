using System;

// Token: 0x0200061B RID: 1563
public interface IUserGameplaySettingsModel
{
	// Token: 0x06002690 RID: 9872
	void Init();

	// Token: 0x06002691 RID: 9873
	void Reset();

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06002692 RID: 9874
	// (set) Token: 0x06002693 RID: 9875
	bool MuteEnemyHolos { get; set; }

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06002694 RID: 9876
	// (set) Token: 0x06002695 RID: 9877
	bool MuteEnemyVoicelines { get; set; }
}
