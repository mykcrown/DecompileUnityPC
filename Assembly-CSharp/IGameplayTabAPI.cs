using System;

// Token: 0x0200097C RID: 2428
public interface IGameplayTabAPI
{
	// Token: 0x0600412C RID: 16684
	void Reset();

	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x0600412D RID: 16685
	// (set) Token: 0x0600412E RID: 16686
	bool MuteEnemyHolos { get; set; }

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x0600412F RID: 16687
	// (set) Token: 0x06004130 RID: 16688
	bool MuteEnemyVoicelines { get; set; }
}
