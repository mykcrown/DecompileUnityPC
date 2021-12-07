using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008DF RID: 2271
public class CharacterSelectScreenController : UIScreenController
{
	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x0010F497 File Offset: 0x0010D897
	// (set) Token: 0x060039F4 RID: 14836 RVA: 0x0010F49F File Offset: 0x0010D89F
	[Inject]
	public ICharacterSelectModel model { get; set; }

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x060039F5 RID: 14837 RVA: 0x0010F4A8 File Offset: 0x0010D8A8
	// (set) Token: 0x060039F6 RID: 14838 RVA: 0x0010F4B0 File Offset: 0x0010D8B0
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x060039F7 RID: 14839 RVA: 0x0010F4B9 File Offset: 0x0010D8B9
	// (set) Token: 0x060039F8 RID: 14840 RVA: 0x0010F4C1 File Offset: 0x0010D8C1
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x060039F9 RID: 14841 RVA: 0x0010F4CA File Offset: 0x0010D8CA
	// (set) Token: 0x060039FA RID: 14842 RVA: 0x0010F4D2 File Offset: 0x0010D8D2
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x060039FB RID: 14843 RVA: 0x0010F4DB File Offset: 0x0010D8DB
	// (set) Token: 0x060039FC RID: 14844 RVA: 0x0010F4E3 File Offset: 0x0010D8E3
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { get; set; }

	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x060039FD RID: 14845 RVA: 0x0010F4EC File Offset: 0x0010D8EC
	// (set) Token: 0x060039FE RID: 14846 RVA: 0x0010F4F4 File Offset: 0x0010D8F4
	[Inject]
	public IPlayerJoinGameController playerJoinGameController { get; set; }

	// Token: 0x17000DF8 RID: 3576
	// (get) Token: 0x060039FF RID: 14847 RVA: 0x0010F4FD File Offset: 0x0010D8FD
	// (set) Token: 0x06003A00 RID: 14848 RVA: 0x0010F505 File Offset: 0x0010D905
	[Inject]
	public IStoreTabsModel storeTabsModel { get; set; }

	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x06003A01 RID: 14849 RVA: 0x0010F50E File Offset: 0x0010D90E
	// (set) Token: 0x06003A02 RID: 14850 RVA: 0x0010F516 File Offset: 0x0010D916
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x06003A03 RID: 14851 RVA: 0x0010F51F File Offset: 0x0010D91F
	// (set) Token: 0x06003A04 RID: 14852 RVA: 0x0010F527 File Offset: 0x0010D927
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x06003A05 RID: 14853 RVA: 0x0010F530 File Offset: 0x0010D930
	// (set) Token: 0x06003A06 RID: 14854 RVA: 0x0010F538 File Offset: 0x0010D938
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x06003A07 RID: 14855 RVA: 0x0010F541 File Offset: 0x0010D941
	// (set) Token: 0x06003A08 RID: 14856 RVA: 0x0010F549 File Offset: 0x0010D949
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x06003A09 RID: 14857 RVA: 0x0010F552 File Offset: 0x0010D952
	// (set) Token: 0x06003A0A RID: 14858 RVA: 0x0010F55A File Offset: 0x0010D95A
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x06003A0B RID: 14859 RVA: 0x0010F563 File Offset: 0x0010D963
	// (set) Token: 0x06003A0C RID: 14860 RVA: 0x0010F56B File Offset: 0x0010D96B
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000DFF RID: 3583
	// (get) Token: 0x06003A0D RID: 14861 RVA: 0x0010F574 File Offset: 0x0010D974
	// (set) Token: 0x06003A0E RID: 14862 RVA: 0x0010F57C File Offset: 0x0010D97C
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17000E00 RID: 3584
	// (get) Token: 0x06003A0F RID: 14863 RVA: 0x0010F585 File Offset: 0x0010D985
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0010F594 File Offset: 0x0010D994
	protected override void setupListeners()
	{
		this.characterSelectScene = this.uiAdapter.GetUIScene<CharacterSelectScene3D>();
		base.subscribe(typeof(SelectCharacterRequest), new Action<GameEvent>(this.onCharacterSelected));
		base.subscribe(typeof(SetPlayerTypeRequest), new Action<GameEvent>(this.onSetPlayerTypeCommand));
		base.subscribe(typeof(CyclePlayerTeamRequest), new Action<GameEvent>(this.onCyclePlayerTeamCommand));
		base.subscribe(typeof(SetPlayerTeamRequest), new Action<GameEvent>(this.onSetPlayerTeamCommand));
		base.subscribe(typeof(CyclePlayerSkinRequest), new Action<GameEvent>(this.onCyclePlayerSkinCommand));
		base.subscribe(typeof(CycleCharacterIndex), new Action<GameEvent>(this.onCycleCharacterIndexCommand));
		base.subscribe(typeof(NextScreenRequest), new Action<GameEvent>(this.onNextScreenRequest));
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
		base.subscribe(typeof(SetBattleSettingRequest), new Action<GameEvent>(this.onSetBattleSettingRequest));
		base.subscribe(typeof(MoreOptionsWasOpened), new Action<GameEvent>(this.onMoreOptionsOpened));
		base.subscribe(typeof(MoreOptionsWasClosed), new Action<GameEvent>(this.onMoreOptionsClosed));
		base.subscribe(typeof(LoadBattleSettings), new Action<GameEvent>(this.onLoadBattleSettings));
		base.signalBus.GetSignal<SetPlayerProfileNameSignal>().AddListener(new Action<PlayerNum, string>(this.onSetPlayerProfileName));
		base.signalBus.GetSignal<QuickSelectDebugCPUSignal>().AddListener(new Action<CharacterID>(this.onQuickSelectDebugCPU));
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0010F73A File Offset: 0x0010DB3A
	protected override void removeListeners()
	{
		base.signalBus.GetSignal<SetPlayerProfileNameSignal>().RemoveListener(new Action<PlayerNum, string>(this.onSetPlayerProfileName));
		base.signalBus.GetSignal<QuickSelectDebugCPUSignal>().RemoveListener(new Action<CharacterID>(this.onQuickSelectDebugCPU));
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0010F774 File Offset: 0x0010DB74
	private void onLoadBattleSettings(GameEvent message)
	{
		LoadBattleSettings loadBattleSettings = message as LoadBattleSettings;
		this.gamePayload.battleConfig = loadBattleSettings.settings;
		this.setGameMode(this.gamePayload.battleConfig.mode);
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
		this.onPlayerDataChanged();
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0010F7C6 File Offset: 0x0010DBC6
	private void onMoreOptionsOpened(GameEvent message)
	{
		this.moreOptionsRevertCache = this.gamePayload.battleConfig.Clone();
		this.moreOptionsProfileRevertCache = this.optionsProfileAPI.GetStateClone();
	}

	// Token: 0x06003A14 RID: 14868 RVA: 0x0010F7F0 File Offset: 0x0010DBF0
	private void onMoreOptionsClosed(GameEvent message)
	{
		MoreOptionsWasClosed moreOptionsWasClosed = message as MoreOptionsWasClosed;
		bool flag = false;
		if (moreOptionsWasClosed.revertChanges)
		{
			if (this.moreOptionsRevertCache != null)
			{
				this.gamePayload.battleConfig.Load(this.moreOptionsRevertCache.settings);
				flag |= true;
			}
			if (this.moreOptionsProfileRevertCache != null)
			{
				this.optionsProfileAPI.LoadStateClone(this.moreOptionsProfileRevertCache);
				flag |= true;
			}
		}
		this.moreOptionsRevertCache = null;
		this.moreOptionsProfileRevertCache = null;
		if (flag)
		{
			base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
			this.onPlayerDataChanged();
		}
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0010F886 File Offset: 0x0010DC86
	private Dictionary<TeamNum, List<PlayerNum>> getTeamPlayers(GameMode mode)
	{
		if (this.gamePayload.teamPlayerMap.ContainsKey(mode))
		{
			return this.gamePayload.teamPlayerMap[mode];
		}
		return null;
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x0010F8B1 File Offset: 0x0010DCB1
	private Dictionary<TeamNum, List<PlayerNum>> getTeamPlayers()
	{
		return this.getTeamPlayers(this.gamePayload.battleConfig.mode);
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0010F8C9 File Offset: 0x0010DCC9
	protected override void setup()
	{
		base.setup();
		this.storeTabsModel.Reset();
		this.syncRandomSelect();
		this.initializeOptions();
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0010F8E8 File Offset: 0x0010DCE8
	private void initializeOptions()
	{
		BattleSettings initialSettings = this.optionsProfileAPI.GetInitialSettings();
		this.gamePayload.battleConfig.Load(initialSettings.settings);
		this.optionsProfileAPI.UpdatePayload(this.gamePayload);
		this.setGameMode(this.gamePayload.battleConfig.mode);
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0010F940 File Offset: 0x0010DD40
	private void syncRandomSelect()
	{
		IEnumerator enumerator = ((IEnumerable)this.gamePayload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
				if (playerSelectionInfo.isRandom)
				{
					playerSelectionInfo.SetCharacter(this.characterLists.GetRandom().characterID, this.skinDataManager, null);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0010F9CC File Offset: 0x0010DDCC
	private void onPlayerDataChanged()
	{
		this.optionsProfileAPI.UpdatePayload(this.gamePayload);
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x0010F9F0 File Offset: 0x0010DDF0
	private void onCharacterSelected(GameEvent message)
	{
		SelectCharacterRequest selectCharacterRequest = message as SelectCharacterRequest;
		if (selectCharacterRequest.useDefaultCharacter)
		{
			this.attemptSelectCharacter(selectCharacterRequest.playerNum, this.characterLists.GetRandom().characterID);
		}
		else
		{
			this.attemptSelectCharacter(selectCharacterRequest.playerNum, selectCharacterRequest.characterID);
		}
	}

	// Token: 0x06003A1C RID: 14876 RVA: 0x0010FA42 File Offset: 0x0010DE42
	private void attemptSelectCharacter(PlayerNum playerNum, CharacterID characterID)
	{
		this.selectCharacter(playerNum, characterID);
	}

	// Token: 0x06003A1D RID: 14877 RVA: 0x0010FA4C File Offset: 0x0010DE4C
	private void selectCharacter(PlayerNum playerNum, CharacterID characterID)
	{
		bool flag = false;
		for (int i = 0; i < this.gamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
			if (playerSelectionInfo.playerNum == playerNum)
			{
				CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(playerNum);
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterID);
				SkinDefinition skinDefinition = null;
				string skinKey = highlightInfo.GetSkinKey(characterID);
				if (skinKey != null)
				{
					skinDefinition = this.characterDataHelper.GetSkinDefinition(characterID, skinKey);
					if (!this.model.IsSkinAvailable(characterID, skinDefinition, playerNum, this.gamePayload.players, SkinSelectMode.Offline))
					{
						skinDefinition = null;
					}
				}
				if (skinDefinition == null)
				{
					skinDefinition = this.model.GetAvailableEquippedOrDefaultSkin(characterDefinition, playerSelectionInfo.playerNum, this.gamePayload.players, SkinSelectMode.Offline);
				}
				playerSelectionInfo.SetCharacter(characterID, this.skinDataManager, skinDefinition);
				playerSelectionInfo.isRandom = (characterID != CharacterID.None && characterDefinition.isRandom);
				flag = true;
			}
		}
		if (flag)
		{
			this.onPlayerDataChanged();
		}
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0010FB60 File Offset: 0x0010DF60
	private void onSetPlayerTeamCommand(GameEvent message)
	{
		SetPlayerTeamRequest setPlayerTeamRequest = message as SetPlayerTeamRequest;
		this.setPlayerTeam(setPlayerTeamRequest.playerNum, setPlayerTeamRequest.teamNum);
	}

	// Token: 0x06003A1F RID: 14879 RVA: 0x0010FB88 File Offset: 0x0010DF88
	private void setPlayerTeam(PlayerNum playerNum, TeamNum teamNum)
	{
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return;
		}
		Dictionary<TeamNum, List<PlayerNum>> teamPlayers = this.getTeamPlayers();
		if (teamPlayers == null)
		{
			Debug.LogWarning("Tried to set team while not in team mode");
			return;
		}
		if (!teamPlayers.ContainsKey(teamNum))
		{
			teamPlayers.Add(teamNum, new List<PlayerNum>());
		}
		teamPlayers[playerSelectionInfo.team].Remove(playerSelectionInfo.playerNum);
		int i;
		for (i = 0; i < teamPlayers[teamNum].Count; i++)
		{
			PlayerSelectionInfo playerSelectionInfo2 = this.findPlayerInfo(teamPlayers[teamNum][i]);
			if (playerSelectionInfo2.type == PlayerType.None)
			{
				break;
			}
		}
		teamPlayers[teamNum].Insert(i, playerSelectionInfo.playerNum);
		playerSelectionInfo.SetTeam(this.gamePayload.battleConfig.mode, teamNum);
		this.onPlayerDataChanged();
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0010FC60 File Offset: 0x0010E060
	private void onCyclePlayerTeamCommand(GameEvent message)
	{
		CyclePlayerTeamRequest cyclePlayerTeamRequest = message as CyclePlayerTeamRequest;
		this.cycleTeam(cyclePlayerTeamRequest.playerNum, cyclePlayerTeamRequest.direction);
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0010FC88 File Offset: 0x0010E088
	private void onCyclePlayerSkinCommand(GameEvent message)
	{
		CyclePlayerSkinRequest cyclePlayerSkinRequest = message as CyclePlayerSkinRequest;
		this.cycleSkin(cyclePlayerSkinRequest.playerNum, cyclePlayerSkinRequest.direction);
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x0010FCB0 File Offset: 0x0010E0B0
	private void cycleSkin(PlayerNum playerNum, int direction)
	{
		PlayerSelectionInfo player = this.gamePayload.players.GetPlayer(playerNum);
		if (player == null || player.characterID == CharacterID.Random)
		{
			return;
		}
		if (player.characterID != CharacterID.None)
		{
			SkinDefinition nextSkin = this.model.GetNextSkin(playerNum, direction, player.characterID, player.skinKey, this.gamePayload.players, SkinSelectMode.Offline);
			if (nextSkin != null)
			{
				player.skinKey = nextSkin.uniqueKey;
				this.onPlayerDataChanged();
			}
		}
		else
		{
			CharacterSelectModel.HighlightInfo highlightInfo = this.model.GetHighlightInfo(playerNum);
			if (highlightInfo.characterID != CharacterID.None && highlightInfo.characterID != CharacterID.Random)
			{
				SkinDefinition nextSkin2 = this.model.GetNextSkin(playerNum, direction, highlightInfo.characterID, highlightInfo.skinIDs[highlightInfo.characterID], this.gamePayload.players, SkinSelectMode.Offline);
				if (nextSkin2 != null)
				{
					highlightInfo.skinIDs[highlightInfo.characterID] = nextSkin2.uniqueKey;
					this.onPlayerDataChanged();
				}
			}
		}
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x0010FDB4 File Offset: 0x0010E1B4
	private bool cycleTeam(PlayerNum playerNum, int direction)
	{
		if (!this.modeData.settings.usesTeams)
		{
			return false;
		}
		Dictionary<TeamNum, List<PlayerNum>> teamPlayers = this.getTeamPlayers();
		if (teamPlayers == null)
		{
			Debug.LogWarning("Tried to cycle team while not in teams mode");
			return false;
		}
		TeamMode teamMode = this.modeData.settings.teamMode;
		List<TeamNum> list;
		if (teamMode != TeamMode.FreeTeams)
		{
			if (teamMode != TeamMode.TwoTeams)
			{
				Debug.LogWarning("Invalid team mode; cannot cycle team");
				return false;
			}
			list = new List<TeamNum>
			{
				TeamNum.Team1,
				TeamNum.Team2
			};
		}
		else
		{
			list = new List<TeamNum>(EnumUtil.GetValues<TeamNum>());
			list.Remove(TeamNum.None);
			list.Remove(TeamNum.All);
		}
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return false;
		}
		int num = list.IndexOf(playerSelectionInfo.team);
		if (num < 0)
		{
			num = 0;
		}
		TeamNum teamNum = list[(num + direction) % list.Count];
		if (this.modeData.settings.teamMode == TeamMode.TwoTeams)
		{
			PlayerNum playerNum2 = PlayerNum.None;
			foreach (PlayerNum playerNum3 in teamPlayers[teamNum])
			{
				PlayerSelectionInfo playerSelectionInfo2 = this.findPlayerInfo(playerNum3);
				if (playerSelectionInfo2.type == PlayerType.None && playerNum2 == PlayerNum.None)
				{
					playerNum2 = playerSelectionInfo2.playerNum;
				}
			}
			this.setPlayerTeam(playerNum2, (teamNum != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2);
		}
		this.setPlayerTeam(playerNum, teamNum);
		return true;
	}

	// Token: 0x06003A24 RID: 14884 RVA: 0x0010FF50 File Offset: 0x0010E350
	private void onCycleCharacterIndexCommand(GameEvent message)
	{
		CycleCharacterIndex cycleCharacterIndex = message as CycleCharacterIndex;
		this.cycleCharacterIndex(cycleCharacterIndex.playerNum);
	}

	// Token: 0x06003A25 RID: 14885 RVA: 0x0010FF70 File Offset: 0x0010E370
	private void cycleCharacterIndex(PlayerNum playerNum)
	{
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return;
		}
		if (this.characterSelectScene.IsCharacterSwapping(playerNum))
		{
			return;
		}
		CharacterDefinition characterDefinition = (playerSelectionInfo.characterID != CharacterID.None) ? this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID) : null;
		if (characterDefinition == null || this.characterDataHelper.GetLinkedCharacters(characterDefinition).Length <= 1)
		{
			return;
		}
		this.audioManager.PlayMenuSound(SoundKey.characterSelect_totemPartnerCycle, 0f);
		playerSelectionInfo.characterIndex = (playerSelectionInfo.characterIndex + 1) % 2;
		this.onPlayerDataChanged();
	}

	// Token: 0x06003A26 RID: 14886 RVA: 0x0011000C File Offset: 0x0010E40C
	private void onSetBattleSettingRequest(GameEvent message)
	{
		SetBattleSettingRequest setBattleSettingRequest = message as SetBattleSettingRequest;
		this.gamePayload.battleConfig.settings[setBattleSettingRequest.settingType] = setBattleSettingRequest.value;
		if (setBattleSettingRequest.settingType == BattleSettingType.Mode)
		{
			this.setGameMode((GameMode)setBattleSettingRequest.value);
		}
		else
		{
			base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
		}
		if (this.moreOptionsProfileRevertCache == null)
		{
			this.optionsProfileAPI.SaveCurrent(null);
		}
	}

	// Token: 0x06003A27 RID: 14887 RVA: 0x00110088 File Offset: 0x0010E488
	private void setGameMode(GameMode mode)
	{
		GameModeData dataByType = base.gameDataManager.GameModeData.GetDataByType(mode);
		GameModeSettings settings = dataByType.settings;
		Dictionary<TeamNum, List<PlayerNum>> teamPlayers = this.getTeamPlayers();
		if (settings.usesTeams && teamPlayers != null && teamPlayers.Count == 0)
		{
			for (int i = 0; i < this.gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = this.gamePayload.players[i];
				TeamNum team;
				if (settings.usesTeams)
				{
					team = ((i % 2 != 0) ? TeamNum.Team2 : TeamNum.Team1);
				}
				else
				{
					team = PlayerUtil.GetTeamNumFromInt(i, true);
				}
				playerSelectionInfo.SetTeam(mode, team);
				if (!teamPlayers.ContainsKey(playerSelectionInfo.team))
				{
					teamPlayers.Add(playerSelectionInfo.team, new List<PlayerNum>());
				}
				teamPlayers[playerSelectionInfo.team].Add(playerSelectionInfo.playerNum);
			}
		}
		for (int j = 0; j < this.gamePayload.players.Length; j++)
		{
			PlayerSelectionInfo playerSelectionInfo2 = this.gamePayload.players[j];
			playerSelectionInfo2.SetTeam(mode, playerSelectionInfo2.GetTeam(mode));
		}
		int num = settings.maxPlayerCount;
		IEnumerator enumerator = ((IEnumerable)this.gamePayload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo3 = (PlayerSelectionInfo)obj;
				if (playerSelectionInfo3.type != PlayerType.None)
				{
					bool flag = num == 0 || PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo3.playerNum, false) > settings.maxPlayerCount;
					if (flag)
					{
						this.setPlayerType(playerSelectionInfo3.playerNum, PlayerType.None);
						this.attemptSelectCharacter(playerSelectionInfo3.playerNum, CharacterID.None);
					}
					else
					{
						num--;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
		this.onPlayerDataChanged();
	}

	// Token: 0x06003A28 RID: 14888 RVA: 0x00110298 File Offset: 0x0010E698
	private void onQuickSelectDebugCPU(CharacterID characterID)
	{
		PlayerNum nextOpenSlot = this.getNextOpenSlot();
		if (nextOpenSlot != PlayerNum.None)
		{
			this.setPlayerType(nextOpenSlot, PlayerType.CPU);
			this.selectCharacter(nextOpenSlot, characterID);
		}
	}

	// Token: 0x06003A29 RID: 14889 RVA: 0x001102C4 File Offset: 0x0010E6C4
	private PlayerNum getNextOpenSlot()
	{
		for (int i = 0; i < this.enterNewGame.GamePayload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = this.enterNewGame.GamePayload.players[i];
			if (playerSelectionInfo.type == PlayerType.None)
			{
				return playerSelectionInfo.playerNum;
			}
		}
		return PlayerNum.None;
	}

	// Token: 0x06003A2A RID: 14890 RVA: 0x00110324 File Offset: 0x0010E724
	private void onSetPlayerProfileName(PlayerNum playerNum, string name)
	{
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo != null)
		{
			this.model.SetPlayerLocalName(playerSelectionInfo, name);
			this.onPlayerDataChanged();
		}
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x00110354 File Offset: 0x0010E754
	private void onSetPlayerTypeCommand(GameEvent message)
	{
		SetPlayerTypeRequest setPlayerTypeRequest = message as SetPlayerTypeRequest;
		this.setPlayerType(setPlayerTypeRequest.playerNum, setPlayerTypeRequest.playerType);
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x0011037C File Offset: 0x0010E77C
	private void setPlayerType(PlayerNum playerNum, PlayerType playerType)
	{
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo == null)
		{
			return;
		}
		int maxPlayerCount = this.modeData.settings.maxPlayerCount;
		if (PlayerUtil.GetIntFromPlayerNum(playerSelectionInfo.playerNum, false) > maxPlayerCount && playerType != PlayerType.None)
		{
			return;
		}
		PlayerType type = playerSelectionInfo.type;
		playerSelectionInfo.type = playerType;
		if (playerType == PlayerType.None && type != PlayerType.None && this.modeData.settings.teamMode == TeamMode.TwoTeams)
		{
			int num = -1;
			int num2 = -1;
			Dictionary<TeamNum, List<PlayerNum>> teamPlayers = this.getTeamPlayers();
			if (teamPlayers != null)
			{
				for (int i = 0; i < teamPlayers[TeamNum.Team1].Count; i++)
				{
					PlayerSelectionInfo playerSelectionInfo2 = this.findPlayerInfo(teamPlayers[TeamNum.Team1][i]);
					if (playerSelectionInfo2.type == PlayerType.None)
					{
						num = i;
						break;
					}
				}
				for (int j = 0; j < teamPlayers[TeamNum.Team2].Count; j++)
				{
					PlayerSelectionInfo playerSelectionInfo3 = this.findPlayerInfo(teamPlayers[TeamNum.Team2][j]);
					if (playerSelectionInfo3.type == PlayerType.None)
					{
						num2 = j;
						break;
					}
				}
				if (num != -1 && num2 != -1)
				{
					PlayerSelectionInfo playerSelectionInfo4 = this.findPlayerInfo(teamPlayers[TeamNum.Team1][num]);
					PlayerSelectionInfo playerSelectionInfo5 = this.findPlayerInfo(teamPlayers[TeamNum.Team2][num2]);
					if ((num <= num2 && playerSelectionInfo4.playerNum > playerSelectionInfo5.playerNum) || (num > num2 && playerSelectionInfo4.playerNum < playerSelectionInfo5.playerNum))
					{
						this.setPlayerTeam(playerSelectionInfo5.playerNum, TeamNum.Team1);
						this.setPlayerTeam(playerSelectionInfo4.playerNum, TeamNum.Team2);
					}
				}
			}
		}
		this.onPlayerDataChanged();
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x0011053C File Offset: 0x0010E93C
	private void onNextScreenRequest(GameEvent message)
	{
		if (this.model.IsValidPayload(this.modeData, this.gamePayload))
		{
			this.richPresence.SetPresence("InStageSelect", null, null, null);
			this.events.Broadcast(new LoadScreenCommand(ScreenType.SelectStage, null, ScreenUpdateType.Next));
		}
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0011058B File Offset: 0x0010E98B
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence(null, null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}

	// Token: 0x17000E01 RID: 3585
	// (get) Token: 0x06003A2F RID: 14895 RVA: 0x001105AF File Offset: 0x0010E9AF
	private GameModeData modeData
	{
		get
		{
			return base.gameDataManager.GameModeData.GetDataByType(this.gamePayload.battleConfig.mode);
		}
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x001105D1 File Offset: 0x0010E9D1
	private PlayerSelectionInfo findPlayerInfo(PlayerNum playerNum)
	{
		return this.gamePayload.FindPlayerInfo(playerNum);
	}

	// Token: 0x040027FC RID: 10236
	private BattleSettings moreOptionsRevertCache;

	// Token: 0x040027FD RID: 10237
	private OptionsProfileSet moreOptionsProfileRevertCache;

	// Token: 0x040027FE RID: 10238
	private CharacterSelectScene3D characterSelectScene;
}
