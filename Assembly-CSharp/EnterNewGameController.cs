using System;
using System.Collections.Generic;
using P2P;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000533 RID: 1331
public class EnterNewGameController : IEnterNewGame
{
	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0009477C File Offset: 0x00092B7C
	// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x00094784 File Offset: 0x00092B84
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { get; set; }

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x0009478D File Offset: 0x00092B8D
	// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x00094795 File Offset: 0x00092B95
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x0009479E File Offset: 0x00092B9E
	// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x000947A6 File Offset: 0x00092BA6
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x000947AF File Offset: 0x00092BAF
	// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x000947B7 File Offset: 0x00092BB7
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x000947C0 File Offset: 0x00092BC0
	// (set) Token: 0x06001CDA RID: 7386 RVA: 0x000947C8 File Offset: 0x00092BC8
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000947D1 File Offset: 0x00092BD1
	// (set) Token: 0x06001CDC RID: 7388 RVA: 0x000947D9 File Offset: 0x00092BD9
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06001CDD RID: 7389 RVA: 0x000947E2 File Offset: 0x00092BE2
	// (set) Token: 0x06001CDE RID: 7390 RVA: 0x000947EA File Offset: 0x00092BEA
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000947F3 File Offset: 0x00092BF3
	// (set) Token: 0x06001CE0 RID: 7392 RVA: 0x000947FB File Offset: 0x00092BFB
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x00094804 File Offset: 0x00092C04
	// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x0009480C File Offset: 0x00092C0C
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00094815 File Offset: 0x00092C15
	// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x0009481D File Offset: 0x00092C1D
	[Inject]
	public PlayerInputLocator playerInput { get; set; }

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x00094826 File Offset: 0x00092C26
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x0009482E File Offset: 0x00092C2E
	[Inject]
	public IBackgroundLoader backgroundLoader { get; set; }

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x00094837 File Offset: 0x00092C37
	// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x0009483F File Offset: 0x00092C3F
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x00094848 File Offset: 0x00092C48
	// (set) Token: 0x06001CEA RID: 7402 RVA: 0x00094850 File Offset: 0x00092C50
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06001CEB RID: 7403 RVA: 0x00094859 File Offset: 0x00092C59
	// (set) Token: 0x06001CEC RID: 7404 RVA: 0x00094861 File Offset: 0x00092C61
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06001CED RID: 7405 RVA: 0x0009486A File Offset: 0x00092C6A
	// (set) Token: 0x06001CEE RID: 7406 RVA: 0x00094872 File Offset: 0x00092C72
	[Inject]
	public IUIAdapter uiAdapter { get; set; }

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06001CEF RID: 7407 RVA: 0x0009487B File Offset: 0x00092C7B
	// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x00094883 File Offset: 0x00092C83
	[Inject]
	public ICursorManager cursorManager { get; set; }

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x0009488C File Offset: 0x00092C8C
	// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x00094894 File Offset: 0x00092C94
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x0009489D File Offset: 0x00092C9D
	// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x000948A5 File Offset: 0x00092CA5
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000948AE File Offset: 0x00092CAE
	// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x000948B6 File Offset: 0x00092CB6
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000948BF File Offset: 0x00092CBF
	public GameLoadPayload GamePayload
	{
		get
		{
			return this.gamePayload;
		}
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000948C8 File Offset: 0x00092CC8
	[PostConstruct]
	public void Init()
	{
		IBattleServerAPI battleServerAPI = this.battleServerAPI;
		battleServerAPI.OnMatchReady = (Action<SP2PMatchBasicPlayerDesc[]>)Delegate.Combine(battleServerAPI.OnMatchReady, new Action<SP2PMatchBasicPlayerDesc[]>(this.onOnlineMatchReady));
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x00094918 File Offset: 0x00092D18
	private void onEndGame(VictoryScreenPayload payload)
	{
		this.gamePayload = null;
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x00094924 File Offset: 0x00092D24
	private GameLoadPayload generateGameLoadPayload(GameStartType startType)
	{
		GameLoadPayload gameLoadPayload = new GameLoadPayload();
		List<StageID> list = new List<StageID>();
		Dictionary<string, string> sceneNamePathMap = SceneUtil.GetSceneNamePathMap();
		foreach (StageData stageData in this.gameDataManager.StageData.DataList)
		{
			if (!(stageData == null))
			{
				if (stageData.stageType == StageType.Random)
				{
					list.Add(stageData.stageID);
				}
				else if (!sceneNamePathMap.ContainsKey(stageData.sceneName))
				{
					if (!stageData.isTemporary)
					{
						Debug.LogError("No scene mapped in editor for stage " + stageData.stageName);
					}
				}
				else
				{
					string text = sceneNamePathMap[stageData.sceneName];
					if (stageData.enabled && !stageData.isDev)
					{
						list.Add(stageData.stageID);
					}
				}
			}
		}
		gameLoadPayload.teamPlayerMap = new Dictionary<GameMode, Dictionary<TeamNum, List<PlayerNum>>>();
		gameLoadPayload.teamPlayerMap[GameMode.Teams] = new Dictionary<TeamNum, List<PlayerNum>>();
		gameLoadPayload.teamPlayerMap[GameMode.CrewBattle] = new Dictionary<TeamNum, List<PlayerNum>>();
		gameLoadPayload.stagePayloadData = new StagePayloadData();
		gameLoadPayload.stagePayloadData.legalStages = list;
		List<GameMode> list2 = new List<GameMode>();
		foreach (GameModeData gameModeData in this.gameDataManager.GameModeData.DataList)
		{
			if (gameModeData != null && gameModeData.enabled && ((startType == GameStartType.CrewBattle && gameModeData.Type == GameMode.CrewBattle) || (startType == GameStartType.FreePlay && gameModeData.selectableInFreePlay)) && (!gameModeData.debugOnly || Debug.isDebugBuild))
			{
				list2.Add(gameModeData.Type);
			}
		}
		gameLoadPayload.players = new PlayerSelectionList(this.config.maxPlayers);
		for (int i = 0; i < this.config.maxPlayers; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = new PlayerSelectionInfo();
			playerSelectionInfo.curProfile = new PlayerProfile();
			playerSelectionInfo.playerNum = PlayerUtil.GetPlayerNumFromInt(i + 1, false);
			TeamNum teamNumFromInt = PlayerUtil.GetTeamNumFromInt(i % 2 + 1, false);
			playerSelectionInfo.SetTeam(GameMode.CrewBattle, teamNumFromInt);
			playerSelectionInfo.SetTeam(GameMode.Teams, teamNumFromInt);
			playerSelectionInfo.type = PlayerType.None;
			gameLoadPayload.players[i] = playerSelectionInfo;
			foreach (GameMode gameMode in gameLoadPayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = gameLoadPayload.teamPlayerMap[gameMode];
				TeamNum team = playerSelectionInfo.GetTeam(gameMode);
				if (team != TeamNum.None)
				{
					if (!dictionary.ContainsKey(team))
					{
						dictionary.Add(team, new List<PlayerNum>());
					}
					dictionary[team].Add(playerSelectionInfo.playerNum);
				}
			}
		}
		gameLoadPayload.battleConfig = new BattleSettings();
		return gameLoadPayload;
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x00094C68 File Offset: 0x00093068
	public void InitPayload(GameStartType startType, GameLoadPayload copyFromPayload)
	{
		if (copyFromPayload == null)
		{
			this.gamePayload = this.generateGameLoadPayload(startType);
		}
		else
		{
			this.gamePayload = copyFromPayload.Clone();
		}
		this.gamePayload.isOnlineGame = this.battleServerAPI.IsConnected;
		this.gamePayload.customLobbyMatch = this.customLobby.IsInLobby;
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x00094CC8 File Offset: 0x000930C8
	private void onOnlineMatchReady(SP2PMatchBasicPlayerDesc[] players)
	{
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.FreeForAll);
		this.resetOptions(delegate
		{
			this.InitPayload(GameStartType.FreePlay, this.generateOnlineGamePayload(GameStartType.FreePlay, players));
			this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			this.loadScreen(ScreenType.OnlineBlindPick);
		});
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x00094D08 File Offset: 0x00093108
	private GameLoadPayload generateOnlineGamePayload(GameStartType startType, SP2PMatchBasicPlayerDesc[] players)
	{
		GameLoadPayload gameLoadPayload = this.generateGameLoadPayload(startType);
		for (int i = 0; i < players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = gameLoadPayload.FindPlayerInfo((PlayerNum)i);
			playerSelectionInfo.curProfile.profileName = players[i].name;
			playerSelectionInfo.userID = players[i].userID;
			playerSelectionInfo.isSpectator = players[i].isSpectator;
			int team = (int)players[i].team;
			playerSelectionInfo.SetTeam(GameMode.CrewBattle, (TeamNum)team);
			playerSelectionInfo.SetTeam(GameMode.Teams, (TeamNum)team);
			playerSelectionInfo.type = ((!playerSelectionInfo.isSpectator) ? PlayerType.Human : PlayerType.Spectator);
		}
		return gameLoadPayload;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x00094D9A File Offset: 0x0009319A
	private void loadScreen(ScreenType screen)
	{
		this.events.Broadcast(new LoadScreenCommand(screen, null, ScreenUpdateType.Next));
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x00094DB0 File Offset: 0x000931B0
	private void resetOptions(Action callback)
	{
		this.optionsProfileAPI.DeleteDefaultSettings(delegate(SaveOptionsProfileResult result)
		{
			if (result != SaveOptionsProfileResult.SUCCESS)
			{
				this.dialogController.ShowOneButtonDialog("Placeholder error", "There was an error", "Continue", WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else
			{
				callback();
			}
		});
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x00094DE8 File Offset: 0x000931E8
	public void StartPreviewGame(bool skipsToVictory)
	{
		this.gamePayload = this.generateGameLoadPayload(GameStartType.FreePlay);
		this.userInputManager.UnbindAll();
		if (this.config.DebugConfig.testGameSettings.playerSettings.Count == 0)
		{
			TestGamePlayerSettings[] array = new TestGamePlayerSettings[]
			{
				new TestGamePlayerSettings(),
				new TestGamePlayerSettings()
			};
			array[0].characterId = CharacterID.CHARACTER_8;
			array[0].devicePreference = DevicePreference.Gamepad;
			array[0].playerNum = PlayerNum.Player1;
			this.config.DebugConfig.testGameSettings.playerSettings.Add(array[0]);
			array[1].characterId = CharacterID.Raymer;
			array[1].devicePreference = DevicePreference.Gamepad;
			array[1].playerNum = PlayerNum.Player2;
			this.config.DebugConfig.testGameSettings.playerSettings.Add(array[1]);
		}
		for (int i = 0; i < this.config.DebugConfig.testGameSettings.playerSettings.Count; i++)
		{
			TestGamePlayerSettings testGamePlayerSettings = this.config.DebugConfig.testGameSettings.playerSettings[i];
			TeamNum teamNumFromTest = this.getTeamNumFromTest(testGamePlayerSettings.teamNum, i);
			PlayerSelectionInfo playerSelectionInfo = new PlayerSelectionInfo();
			playerSelectionInfo.SetCharacter(testGamePlayerSettings.characterId, this.skinDataManager, null);
			playerSelectionInfo.skinKey = this.skinDataManager.GetDefaultSkin(testGamePlayerSettings.characterId).uniqueKey;
			playerSelectionInfo.playerNum = testGamePlayerSettings.playerNum;
			playerSelectionInfo.SetTeam(GameMode.CrewBattle, teamNumFromTest);
			playerSelectionInfo.SetTeam(GameMode.Teams, teamNumFromTest);
			playerSelectionInfo.type = testGamePlayerSettings.playerType;
			playerSelectionInfo.curProfile = new PlayerProfile();
			playerSelectionInfo.curProfile.profileName = string.Format("P{0}", PlayerUtil.GetIntFromPlayerNum(testGamePlayerSettings.playerNum, false));
			this.gamePayload.players[i] = playerSelectionInfo;
			foreach (GameMode gameMode in this.gamePayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = this.gamePayload.teamPlayerMap[gameMode];
				TeamNum team = playerSelectionInfo.GetTeam(gameMode);
				if (team != TeamNum.None)
				{
					if (!dictionary.ContainsKey(team))
					{
						dictionary.Add(team, new List<PlayerNum>());
					}
					dictionary[team].Add(playerSelectionInfo.playerNum);
				}
			}
			this.userInputManager.ForceBindAvailablePortToPlayerNoUser(playerSelectionInfo.playerNum);
			PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(playerSelectionInfo.playerNum);
			this.playerInput.Input.AssignFirstAvailableDevice(portWithPlayerNum, testGamePlayerSettings.devicePreference);
		}
		BattleSettings battleSettings = new BattleSettings();
		battleSettings.durationSeconds = 0;
		battleSettings.lives = 0;
		battleSettings.mode = ((!skipsToVictory) ? GameMode.Preview : this.config.DebugConfig.testGameSettings.gameMode);
		this.gamePayload.battleConfig = battleSettings;
		this.gamePayload.stage = StageID.None;
		string name = SceneManager.GetActiveScene().name;
		List<StageData> dataList = this.gameDataManager.StageData.DataList;
		foreach (StageData stageData in dataList)
		{
			if (stageData.sceneName.Equals(name) || stageData.lowDetailSceneName.Equals(name))
			{
				this.gamePayload.stage = (StageID)stageData.ID;
			}
		}
		this.backgroundLoader.WaitForSetup(delegate
		{
			this.events.Broadcast(new AllPlayersReadyMessage());
			this.events.Broadcast(typeof(CreateGameManagerCommand));
			this.events.Broadcast(new SetupGameCommand(this.gamePayload, -1f));
			this.uiAdapter.PreloadScreen(ScreenType.BattleGUI);
			this.cursorManager.SetDisplay(false);
			this.signalBus.Dispatch(StageLoader.GAME_BEGINNING);
			this.gameController.ClientReadyToStartGame();
		});
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x000951AC File Offset: 0x000935AC
	public void StartTestGame()
	{
		this.gamePayload = this.generateGameLoadPayload((this.config.DebugConfig.testGameSettings.gameMode != GameMode.CrewBattle) ? GameStartType.FreePlay : GameStartType.CrewBattle);
		BattleSettings battleSettings = new BattleSettings();
		battleSettings.durationSeconds = this.config.DebugConfig.testGameSettings.durationSeconds;
		battleSettings.lives = this.config.DebugConfig.testGameSettings.lives;
		battleSettings.mode = this.config.DebugConfig.testGameSettings.gameMode;
		battleSettings.assists = this.config.DebugConfig.testGameSettings.assists;
		this.gamePayload.battleConfig = battleSettings;
		this.gamePayload.stage = this.config.DebugConfig.testGameSettings.stage;
		this.userInputManager.UnbindAll();
		for (int i = 0; i < this.config.DebugConfig.testGameSettings.playerSettings.Count; i++)
		{
			TestGamePlayerSettings testGamePlayerSettings = this.config.DebugConfig.testGameSettings.playerSettings[i];
			PlayerSelectionInfo playerSelectionInfo = new PlayerSelectionInfo();
			if (testGamePlayerSettings.characterId == CharacterID.None || testGamePlayerSettings.characterId == CharacterID.Random)
			{
				Debug.LogError(string.Format("Invalid CharacterId of {0} in test game settings.  Defaulting to Kidd.", testGamePlayerSettings.characterId));
				testGamePlayerSettings.characterId = CharacterID.Kidd;
			}
			playerSelectionInfo.skinKey = this.skinDataManager.GetDefaultSkin(testGamePlayerSettings.characterId).uniqueKey;
			TeamNum teamNumFromTest = this.getTeamNumFromTest(testGamePlayerSettings.teamNum, i);
			playerSelectionInfo.SetCharacter(testGamePlayerSettings.characterId, this.skinDataManager, null);
			playerSelectionInfo.playerNum = testGamePlayerSettings.playerNum;
			playerSelectionInfo.SetTeam(GameMode.CrewBattle, teamNumFromTest);
			playerSelectionInfo.SetTeam(GameMode.Teams, teamNumFromTest);
			playerSelectionInfo.type = testGamePlayerSettings.playerType;
			playerSelectionInfo.curProfile = new PlayerProfile();
			playerSelectionInfo.curProfile.profileName = string.Format("P{0}", i + 1);
			this.gamePayload.players[i] = playerSelectionInfo;
			foreach (GameMode gameMode in this.gamePayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = this.gamePayload.teamPlayerMap[gameMode];
				TeamNum team = playerSelectionInfo.GetTeam(gameMode);
				if (team != TeamNum.None)
				{
					if (!dictionary.ContainsKey(team))
					{
						dictionary.Add(team, new List<PlayerNum>());
					}
					dictionary[team].Add(playerSelectionInfo.playerNum);
				}
			}
			this.userInputManager.ForceBindAvailablePortToPlayerNoUser(testGamePlayerSettings.playerNum);
			PlayerInputPort portWithPlayerNum = this.userInputManager.GetPortWithPlayerNum(testGamePlayerSettings.playerNum);
			this.playerInput.Input.AssignFirstAvailableDevice(portWithPlayerNum, testGamePlayerSettings.devicePreference);
		}
		this.backgroundLoader.WaitForSetup(delegate
		{
			this.events.Broadcast(new AllPlayersReadyMessage());
			this.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
			this.richPresence.SetPresence("Loading", null, null, null);
		});
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000954B8 File Offset: 0x000938B8
	public bool StartReplay(IReplaySystem replaySystem)
	{
		if (replaySystem.IsDirty)
		{
			replaySystem.LoadFromFile(this.config.replaySettings.replayName);
		}
		if (replaySystem.Replay == null)
		{
			return false;
		}
		replaySystem.Mode = ReplayMode.Replay;
		this.gamePayload = replaySystem.Replay.startPayload;
		this.backgroundLoader.WaitForSetup(delegate
		{
			this.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
			this.richPresence.SetPresence("Loading", null, null, null);
		});
		return true;
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00095524 File Offset: 0x00093924
	private TeamNum getTeamNumFromTest(TestGameTeamNum testTeamNum, int index)
	{
		TeamNum result = TeamNum.Team1;
		if (testTeamNum == TestGameTeamNum.Auto)
		{
			result = ((index % 2 != 0) ? TeamNum.Team2 : TeamNum.Team1);
		}
		else if (testTeamNum == TestGameTeamNum.Team2)
		{
			result = TeamNum.Team2;
		}
		return result;
	}

	// Token: 0x040017CC RID: 6092
	private GameLoadPayload gamePayload;
}
