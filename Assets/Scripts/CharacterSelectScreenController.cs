// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScreenController : UIScreenController
{
	private BattleSettings moreOptionsRevertCache;

	private OptionsProfileSet moreOptionsProfileRevertCache;

	private CharacterSelectScene3D characterSelectScene;

	[Inject]
	public ICharacterSelectModel model
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
	{
		get;
		set;
	}

	[Inject]
	public IPlayerJoinGameController playerJoinGameController
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
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
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	private GameModeData modeData
	{
		get
		{
			return base.gameDataManager.GameModeData.GetDataByType(this.gamePayload.battleConfig.mode);
		}
	}

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

	protected override void removeListeners()
	{
		base.signalBus.GetSignal<SetPlayerProfileNameSignal>().RemoveListener(new Action<PlayerNum, string>(this.onSetPlayerProfileName));
		base.signalBus.GetSignal<QuickSelectDebugCPUSignal>().RemoveListener(new Action<CharacterID>(this.onQuickSelectDebugCPU));
	}

	private void onLoadBattleSettings(GameEvent message)
	{
		LoadBattleSettings loadBattleSettings = message as LoadBattleSettings;
		this.gamePayload.battleConfig = loadBattleSettings.settings;
		this.setGameMode(this.gamePayload.battleConfig.mode);
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
		this.onPlayerDataChanged();
	}

	private void onMoreOptionsOpened(GameEvent message)
	{
		this.moreOptionsRevertCache = this.gamePayload.battleConfig.Clone();
		this.moreOptionsProfileRevertCache = this.optionsProfileAPI.GetStateClone();
	}

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

	private Dictionary<TeamNum, List<PlayerNum>> getTeamPlayers(GameMode mode)
	{
		if (this.gamePayload.teamPlayerMap.ContainsKey(mode))
		{
			return this.gamePayload.teamPlayerMap[mode];
		}
		return null;
	}

	private Dictionary<TeamNum, List<PlayerNum>> getTeamPlayers()
	{
		return this.getTeamPlayers(this.gamePayload.battleConfig.mode);
	}

	protected override void setup()
	{
		base.setup();
		this.storeTabsModel.Reset();
		this.syncRandomSelect();
		this.initializeOptions();
	}

	private void initializeOptions()
	{
		BattleSettings initialSettings = this.optionsProfileAPI.GetInitialSettings();
		this.gamePayload.battleConfig.Load(initialSettings.settings);
		this.optionsProfileAPI.UpdatePayload(this.gamePayload);
		this.setGameMode(this.gamePayload.battleConfig.mode);
	}

	private void syncRandomSelect()
	{
		IEnumerator enumerator = ((IEnumerable)this.gamePayload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
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

	private void onPlayerDataChanged()
	{
		this.optionsProfileAPI.UpdatePayload(this.gamePayload);
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
	}

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

	private void attemptSelectCharacter(PlayerNum playerNum, CharacterID characterID)
	{
		this.selectCharacter(playerNum, characterID);
	}

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

	private void onSetPlayerTeamCommand(GameEvent message)
	{
		SetPlayerTeamRequest setPlayerTeamRequest = message as SetPlayerTeamRequest;
		this.setPlayerTeam(setPlayerTeamRequest.playerNum, setPlayerTeamRequest.teamNum);
	}

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
			UnityEngine.Debug.LogWarning("Tried to set team while not in team mode");
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

	private void onCyclePlayerTeamCommand(GameEvent message)
	{
		CyclePlayerTeamRequest cyclePlayerTeamRequest = message as CyclePlayerTeamRequest;
		this.cycleTeam(cyclePlayerTeamRequest.playerNum, cyclePlayerTeamRequest.direction);
	}

	private void onCyclePlayerSkinCommand(GameEvent message)
	{
		CyclePlayerSkinRequest cyclePlayerSkinRequest = message as CyclePlayerSkinRequest;
		this.cycleSkin(cyclePlayerSkinRequest.playerNum, cyclePlayerSkinRequest.direction);
	}

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

	private bool cycleTeam(PlayerNum playerNum, int direction)
	{
		if (!this.modeData.settings.usesTeams)
		{
			return false;
		}
		Dictionary<TeamNum, List<PlayerNum>> teamPlayers = this.getTeamPlayers();
		if (teamPlayers == null)
		{
			UnityEngine.Debug.LogWarning("Tried to cycle team while not in teams mode");
			return false;
		}
		TeamMode teamMode = this.modeData.settings.teamMode;
		List<TeamNum> list;
		if (teamMode != TeamMode.FreeTeams)
		{
			if (teamMode != TeamMode.TwoTeams)
			{
				UnityEngine.Debug.LogWarning("Invalid team mode; cannot cycle team");
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
			foreach (PlayerNum current in teamPlayers[teamNum])
			{
				PlayerSelectionInfo playerSelectionInfo2 = this.findPlayerInfo(current);
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

	private void onCycleCharacterIndexCommand(GameEvent message)
	{
		CycleCharacterIndex cycleCharacterIndex = message as CycleCharacterIndex;
		this.cycleCharacterIndex(cycleCharacterIndex.playerNum);
	}

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
				PlayerSelectionInfo playerSelectionInfo3 = (PlayerSelectionInfo)enumerator.Current;
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

	private void onQuickSelectDebugCPU(CharacterID characterID)
	{
		PlayerNum nextOpenSlot = this.getNextOpenSlot();
		if (nextOpenSlot != PlayerNum.None)
		{
			this.setPlayerType(nextOpenSlot, PlayerType.CPU);
			this.selectCharacter(nextOpenSlot, characterID);
		}
	}

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

	private void onSetPlayerProfileName(PlayerNum playerNum, string name)
	{
		PlayerSelectionInfo playerSelectionInfo = this.findPlayerInfo(playerNum);
		if (playerSelectionInfo != null)
		{
			this.model.SetPlayerLocalName(playerSelectionInfo, name);
			this.onPlayerDataChanged();
		}
	}

	private void onSetPlayerTypeCommand(GameEvent message)
	{
		SetPlayerTypeRequest setPlayerTypeRequest = message as SetPlayerTypeRequest;
		this.setPlayerType(setPlayerTypeRequest.playerNum, setPlayerTypeRequest.playerType);
	}

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

	private void onNextScreenRequest(GameEvent message)
	{
		if (this.model.IsValidPayload(this.modeData, this.gamePayload))
		{
			this.richPresence.SetPresence("InStageSelect", null, null, null);
			this.events.Broadcast(new LoadScreenCommand(ScreenType.SelectStage, null, ScreenUpdateType.Next));
		}
	}

	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence(null, null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Previous));
	}

	private PlayerSelectionInfo findPlayerInfo(PlayerNum playerNum)
	{
		return this.gamePayload.FindPlayerInfo(playerNum);
	}
}
