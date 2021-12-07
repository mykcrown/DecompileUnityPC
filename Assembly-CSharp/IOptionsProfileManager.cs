using System;

// Token: 0x02000539 RID: 1337
public interface IOptionsProfileManager
{
	// Token: 0x06001D37 RID: 7479
	void SaveToCurrent(BattleSettings settings, Action<SaveOptionsProfileResult> callback);

	// Token: 0x06001D38 RID: 7480
	void SaveAndSwitchToDefault(BattleSettings settings, Action<SaveOptionsProfileResult> callback);

	// Token: 0x06001D39 RID: 7481
	void LoadAll(Action<LoadOptionsProfileListResult> callback);

	// Token: 0x06001D3A RID: 7482
	OptionsProfile[] GetAll();

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06001D3B RID: 7483
	// (set) Token: 0x06001D3C RID: 7484
	OptionsProfile CurrentProfile { get; set; }

	// Token: 0x06001D3D RID: 7485
	void CreateNewProfile(string name, Action<CreateNewProfileResult> callback);

	// Token: 0x06001D3E RID: 7486
	void DeleteProfile(OptionsProfile profile, Action<DeleteProfileResult> callback);

	// Token: 0x06001D3F RID: 7487
	DeleteProfileResult CanDeleteProfile(OptionsProfile profile);

	// Token: 0x06001D40 RID: 7488
	void DeleteDefaultState(Action<SaveOptionsProfileResult> callback);

	// Token: 0x06001D41 RID: 7489
	void SetDefaultGameMode(GameMode mode);

	// Token: 0x06001D42 RID: 7490
	void Select(OptionsProfile profile);

	// Token: 0x06001D43 RID: 7491
	int GetMinNameLength();

	// Token: 0x06001D44 RID: 7492
	int GetMaxNameLength();

	// Token: 0x06001D45 RID: 7493
	int GetMaxProfiles();

	// Token: 0x06001D46 RID: 7494
	OptionsProfileSet GetStateCopy();

	// Token: 0x06001D47 RID: 7495
	void RevertState(OptionsProfileSet copy);
}
