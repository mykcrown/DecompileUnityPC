using System;

// Token: 0x02000538 RID: 1336
public class OptionsProfileManager : IOptionsProfileManager
{
	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06001D14 RID: 7444 RVA: 0x0009576D File Offset: 0x00093B6D
	// (set) Token: 0x06001D15 RID: 7445 RVA: 0x00095775 File Offset: 0x00093B75
	[Inject]
	public IOptionProfileSaver profileSaver { get; set; }

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06001D16 RID: 7446 RVA: 0x0009577E File Offset: 0x00093B7E
	// (set) Token: 0x06001D17 RID: 7447 RVA: 0x00095786 File Offset: 0x00093B86
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06001D18 RID: 7448 RVA: 0x0009578F File Offset: 0x00093B8F
	// (set) Token: 0x06001D19 RID: 7449 RVA: 0x00095797 File Offset: 0x00093B97
	[Inject]
	public IUserEnteredNameValidator nameValidator { get; set; }

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06001D1A RID: 7450 RVA: 0x000957A0 File Offset: 0x00093BA0
	// (set) Token: 0x06001D1B RID: 7451 RVA: 0x000957A8 File Offset: 0x00093BA8
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06001D1C RID: 7452 RVA: 0x000957B1 File Offset: 0x00093BB1
	// (set) Token: 0x06001D1D RID: 7453 RVA: 0x000957B9 File Offset: 0x00093BB9
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06001D1E RID: 7454 RVA: 0x000957C2 File Offset: 0x00093BC2
	// (set) Token: 0x06001D1F RID: 7455 RVA: 0x000957CA File Offset: 0x00093BCA
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06001D20 RID: 7456 RVA: 0x000957D3 File Offset: 0x00093BD3
	public void SaveToCurrent(BattleSettings data, Action<SaveOptionsProfileResult> callback)
	{
		this.syncCurrentProfile();
		this.CurrentProfile.settings = data.Clone();
		this.profileSaver.Save(this.state, callback);
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x00095800 File Offset: 0x00093C00
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

	// Token: 0x06001D22 RID: 7458 RVA: 0x0009588C File Offset: 0x00093C8C
	public void SetDefaultGameMode(GameMode mode)
	{
		this.defaultMode = mode;
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x00095898 File Offset: 0x00093C98
	public void DeleteDefaultState(Action<SaveOptionsProfileResult> callback)
	{
		this.LoadAll(delegate(LoadOptionsProfileListResult result)
		{
			if (!result.success)
			{
				callback(SaveOptionsProfileResult.FAILURE);
			}
			else
			{
				this.state.defaultProfile = this.createDefaultProfile();
				this.profileSaver.Save(this.state, callback);
			}
		});
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06001D24 RID: 7460 RVA: 0x000958CB File Offset: 0x00093CCB
	// (set) Token: 0x06001D25 RID: 7461 RVA: 0x000958D4 File Offset: 0x00093CD4
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

	// Token: 0x06001D26 RID: 7462 RVA: 0x0009591C File Offset: 0x00093D1C
	private void onCurrentProfileChanged()
	{
		this.events.Broadcast(new LoadBattleSettings(this.CurrentProfile.settings));
		this.signalBus.Dispatch(OptionsProfileManager.PROFILES_UPDATED);
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0009594C File Offset: 0x00093D4C
	private OptionsProfile getCurrentProfileFromState()
	{
		if (this.state.currentSelectedId == this.state.defaultProfile.id)
		{
			return this.state.defaultProfile;
		}
		foreach (OptionsProfile optionsProfile in this.state.list)
		{
			if (optionsProfile.id == this.state.currentSelectedId)
			{
				return optionsProfile;
			}
		}
		return null;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x000959FC File Offset: 0x00093DFC
	public void LoadAll(Action<LoadOptionsProfileListResult> callback)
	{
		this.profileSaver.Load(delegate(LoadOptionsProfileListResult result)
		{
			if (result.state == null)
			{
				this.state = new OptionsProfileSet();
			}
			else
			{
				this.state = result.state;
			}
			this.syncCurrentProfile();
			callback(result);
		});
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x00095A34 File Offset: 0x00093E34
	public OptionsProfileSet GetStateCopy()
	{
		return this.state.Clone();
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x00095A41 File Offset: 0x00093E41
	public void RevertState(OptionsProfileSet copy)
	{
		this.state = copy.Clone();
		this.onCurrentProfileChanged();
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x00095A55 File Offset: 0x00093E55
	public void Select(OptionsProfile profile)
	{
		this.CurrentProfile = profile;
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x00095A60 File Offset: 0x00093E60
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

	// Token: 0x06001D2D RID: 7469 RVA: 0x00095BBC File Offset: 0x00093FBC
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
			foreach (OptionsProfile optionsProfile in this.state.list)
			{
				if (optionsProfile.id == this.state.currentSelectedId)
				{
					this.CurrentProfile = optionsProfile;
					break;
				}
			}
		}
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x00095CAC File Offset: 0x000940AC
	private bool isProfileSelectionValid()
	{
		if (this.state.currentSelectedId != null)
		{
			if (this.state.defaultProfile.id == this.state.currentSelectedId)
			{
				return true;
			}
			foreach (OptionsProfile optionsProfile in this.state.list)
			{
				if (optionsProfile.id == this.state.currentSelectedId)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06001D2F RID: 7471 RVA: 0x00095D64 File Offset: 0x00094164
	public OptionsProfile[] GetAll()
	{
		return this.state.list.ToArray();
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x00095D76 File Offset: 0x00094176
	public int GetMinNameLength()
	{
		return this.nameValidator.GetMinNameLength();
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x00095D83 File Offset: 0x00094183
	public int GetMaxNameLength()
	{
		return this.nameValidator.GetMaxOptionsProfileNameLength();
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x00095D90 File Offset: 0x00094190
	public int GetMaxProfiles()
	{
		return this.gameDataManager.ConfigData.userProfileSettings.maxOptionProfiles;
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x00095DA8 File Offset: 0x000941A8
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

	// Token: 0x06001D34 RID: 7476 RVA: 0x00095EB7 File Offset: 0x000942B7
	public DeleteProfileResult CanDeleteProfile(OptionsProfile profile)
	{
		return DeleteProfileResult.SUCCESS;
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x00095EBC File Offset: 0x000942BC
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

	// Token: 0x040017D2 RID: 6098
	public static string PROFILES_UPDATED = "OptionsProfileManager.PROFILES_UPDATED";

	// Token: 0x040017D9 RID: 6105
	private OptionsProfileSet state;

	// Token: 0x040017DA RID: 6106
	private CreateOptionsProfileInProgressTracker createProfileTracker = new CreateOptionsProfileInProgressTracker();

	// Token: 0x040017DB RID: 6107
	private GameMode defaultMode;
}
