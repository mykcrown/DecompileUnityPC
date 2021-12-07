// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class OptionsProfileManager : IOptionsProfileManager
{
	private sealed class _DeleteDefaultState_c__AnonStorey0
	{
		internal Action<SaveOptionsProfileResult> callback;

		internal OptionsProfileManager _this;

		internal void __m__0(LoadOptionsProfileListResult result)
		{
			if (!result.success)
			{
				this.callback(SaveOptionsProfileResult.FAILURE);
			}
			else
			{
				this._this.state.defaultProfile = this._this.createDefaultProfile();
				this._this.profileSaver.Save(this._this.state, this.callback);
			}
		}
	}

	private sealed class _LoadAll_c__AnonStorey1
	{
		internal Action<LoadOptionsProfileListResult> callback;

		internal OptionsProfileManager _this;

		internal void __m__0(LoadOptionsProfileListResult result)
		{
			if (result.state == null)
			{
				this._this.state = new OptionsProfileSet();
			}
			else
			{
				this._this.state = result.state;
			}
			this._this.syncCurrentProfile();
			this.callback(result);
		}
	}

	public static string PROFILES_UPDATED = "OptionsProfileManager.PROFILES_UPDATED";

	private OptionsProfileSet state;

	private CreateOptionsProfileInProgressTracker createProfileTracker = new CreateOptionsProfileInProgressTracker();

	private GameMode defaultMode;

	[Inject]
	public IOptionProfileSaver profileSaver
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IUserEnteredNameValidator nameValidator
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public OptionsProfile CurrentProfile
	{
		get
		{
			return this.getCurrentProfileFromState();
		}
		set
		{
			string text = (value != null) ? value.id : null;
			if (text != this.state.currentSelectedId)
			{
				this.state.currentSelectedId = text;
				this.onCurrentProfileChanged();
			}
		}
	}

	public void SaveToCurrent(BattleSettings data, Action<SaveOptionsProfileResult> callback)
	{
		this.syncCurrentProfile();
		this.CurrentProfile.settings = data.Clone();
		this.profileSaver.Save(this.state, callback);
	}

	public void SaveAndSwitchToDefault(BattleSettings data, Action<SaveOptionsProfileResult> callback)
	{
		this.syncCurrentProfile();
		this.CurrentProfile.settings = data.Clone();
		if (this.CurrentProfile.id != this.state.defaultProfile.id)
		{
			this.state.defaultProfile.settings = this.CurrentProfile.settings.Clone();
			this.CurrentProfile = this.state.defaultProfile;
		}
		this.profileSaver.Save(this.state, callback);
	}

	public void SetDefaultGameMode(GameMode mode)
	{
		this.defaultMode = mode;
	}

	public void DeleteDefaultState(Action<SaveOptionsProfileResult> callback)
	{
		OptionsProfileManager._DeleteDefaultState_c__AnonStorey0 _DeleteDefaultState_c__AnonStorey = new OptionsProfileManager._DeleteDefaultState_c__AnonStorey0();
		_DeleteDefaultState_c__AnonStorey.callback = callback;
		_DeleteDefaultState_c__AnonStorey._this = this;
		this.LoadAll(new Action<LoadOptionsProfileListResult>(_DeleteDefaultState_c__AnonStorey.__m__0));
	}

	private void onCurrentProfileChanged()
	{
		this.events.Broadcast(new LoadBattleSettings(this.CurrentProfile.settings));
		this.signalBus.Dispatch(OptionsProfileManager.PROFILES_UPDATED);
	}

	private OptionsProfile getCurrentProfileFromState()
	{
		if (this.state.currentSelectedId == this.state.defaultProfile.id)
		{
			return this.state.defaultProfile;
		}
		foreach (OptionsProfile current in this.state.list)
		{
			if (current.id == this.state.currentSelectedId)
			{
				return current;
			}
		}
		return null;
	}

	public void LoadAll(Action<LoadOptionsProfileListResult> callback)
	{
		OptionsProfileManager._LoadAll_c__AnonStorey1 _LoadAll_c__AnonStorey = new OptionsProfileManager._LoadAll_c__AnonStorey1();
		_LoadAll_c__AnonStorey.callback = callback;
		_LoadAll_c__AnonStorey._this = this;
		this.profileSaver.Load(new Action<LoadOptionsProfileListResult>(_LoadAll_c__AnonStorey.__m__0));
	}

	public OptionsProfileSet GetStateCopy()
	{
		return this.state.Clone();
	}

	public void RevertState(OptionsProfileSet copy)
	{
		this.state = copy.Clone();
		this.onCurrentProfileChanged();
	}

	public void Select(OptionsProfile profile)
	{
		this.CurrentProfile = profile;
	}

	private OptionsProfile createDefaultProfile()
	{
		OptionsProfile optionsProfile = new OptionsProfile();
		optionsProfile.isDefault = true;
		optionsProfile.name = this.localization.GetText("optionsProfile.name.default");
		optionsProfile.id = "__default_v2";
		optionsProfile.settings = new BattleSettings();
		optionsProfile.settings.mode = this.defaultMode;
		GameModeData dataByType = this.gameDataManager.GameModeData.GetDataByType(this.defaultMode);
		GameModeData gameModeData = null;
		optionsProfile.settings.rules = ((!dataByType.settings.usesLives) ? GameRules.Time : GameRules.Stock);
		optionsProfile.settings.lives = dataByType.settings.defaultLivesCount;
		optionsProfile.settings.teamAttack = dataByType.settings.defaultTeamAttackOn;
		optionsProfile.settings.durationSeconds = dataByType.settings.defaultTimeMinutes * 60;
		if (gameModeData != null)
		{
			optionsProfile.settings.crewBattle_lives = gameModeData.settings.defaultLivesCount;
			optionsProfile.settings.assists = gameModeData.settings.defaultAssistsCount;
			optionsProfile.settings.crewBattle_teamAttack = gameModeData.settings.defaultTeamAttackOn;
			optionsProfile.settings.crewsBattle_durationSeconds = gameModeData.settings.defaultTimeMinutes * 60;
			CrewBattleCustomData crewBattleCustomData = gameModeData.customData as CrewBattleCustomData;
			optionsProfile.settings.crewBattle_assistSteal = crewBattleCustomData.defaultAssistStealOn;
		}
		return optionsProfile;
	}

	private void syncCurrentProfile()
	{
		if (this.state.defaultProfile == null)
		{
			this.state.defaultProfile = this.createDefaultProfile();
		}
		if (this.state.list.Count == 0)
		{
			this.CurrentProfile = this.state.defaultProfile;
		}
		else if (!this.isProfileSelectionValid())
		{
			this.CurrentProfile = this.state.defaultProfile;
		}
		else
		{
			foreach (OptionsProfile current in this.state.list)
			{
				if (current.id == this.state.currentSelectedId)
				{
					this.CurrentProfile = current;
					break;
				}
			}
		}
	}

	private bool isProfileSelectionValid()
	{
		if (this.state.currentSelectedId != null)
		{
			if (this.state.defaultProfile.id == this.state.currentSelectedId)
			{
				return true;
			}
			foreach (OptionsProfile current in this.state.list)
			{
				if (current.id == this.state.currentSelectedId)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public OptionsProfile[] GetAll()
	{
		return this.state.list.ToArray();
	}

	public int GetMinNameLength()
	{
		return this.nameValidator.GetMinNameLength();
	}

	public int GetMaxNameLength()
	{
		return this.nameValidator.GetMaxOptionsProfileNameLength();
	}

	public int GetMaxProfiles()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxOptionProfiles;
	}

	public void CreateNewProfile(string name, Action<CreateNewProfileResult> callback)
	{
		CreateNewProfileContext createNewProfileContext = new CreateNewProfileContext();
		createNewProfileContext.callback = callback;
		createNewProfileContext.createProfileTracker = this.createProfileTracker;
		if (this.createProfileTracker.InProgress)
		{
			createNewProfileContext.PreviousInProgress();
			return;
		}
		this.createProfileTracker.InProgress = true;
		if (this.state.list.Count >= this.GetMaxProfiles())
		{
			createNewProfileContext.TooManyProfiles();
			return;
		}
		name = this.nameValidator.FixSpaces(name);
		NameValidationResult nameValidationResult = this.nameValidator.CheckOptionsProfile(name);
		if (nameValidationResult != NameValidationResult.OK)
		{
			createNewProfileContext.CompleteWithError(nameValidationResult);
			return;
		}
		Guid guid = Guid.NewGuid();
		OptionsProfile optionsProfile = new OptionsProfile();
		optionsProfile.isDefault = false;
		optionsProfile.name = name;
		optionsProfile.id = "options-" + guid.ToString();
		optionsProfile.settings = this.CurrentProfile.settings.Clone();
		this.state.list.Add(optionsProfile);
		this.CurrentProfile = optionsProfile;
		this.signalBus.Dispatch(OptionsProfileManager.PROFILES_UPDATED);
		createNewProfileContext.Success();
	}

	public DeleteProfileResult CanDeleteProfile(OptionsProfile profile)
	{
		return DeleteProfileResult.SUCCESS;
	}

	public void DeleteProfile(OptionsProfile profile, Action<DeleteProfileResult> callback)
	{
		DeleteProfileResult deleteProfileResult = this.CanDeleteProfile(profile);
		if (deleteProfileResult != DeleteProfileResult.SUCCESS)
		{
			callback(deleteProfileResult);
		}
		else
		{
			for (int i = this.state.list.Count - 1; i >= 0; i--)
			{
				if (this.state.list[i].id == profile.id)
				{
					this.state.list.RemoveAt(i);
				}
			}
			this.syncCurrentProfile();
			this.signalBus.Dispatch(OptionsProfileManager.PROFILES_UPDATED);
			callback(DeleteProfileResult.SUCCESS);
		}
	}
}
