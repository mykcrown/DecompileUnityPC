// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IOptionsProfileManager
{
	OptionsProfile CurrentProfile
	{
		get;
		set;
	}

	void SaveToCurrent(BattleSettings settings, Action<SaveOptionsProfileResult> callback);

	void SaveAndSwitchToDefault(BattleSettings settings, Action<SaveOptionsProfileResult> callback);

	void LoadAll(Action<LoadOptionsProfileListResult> callback);

	OptionsProfile[] GetAll();

	void CreateNewProfile(string name, Action<CreateNewProfileResult> callback);

	void DeleteProfile(OptionsProfile profile, Action<DeleteProfileResult> callback);

	DeleteProfileResult CanDeleteProfile(OptionsProfile profile);

	void DeleteDefaultState(Action<SaveOptionsProfileResult> callback);

	void SetDefaultGameMode(GameMode mode);

	void Select(OptionsProfile profile);

	int GetMinNameLength();

	int GetMaxNameLength();

	int GetMaxProfiles();

	OptionsProfileSet GetStateCopy();

	void RevertState(OptionsProfileSet copy);
}
