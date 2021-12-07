// Decompile from assembly: Assembly-CSharp.dll

using P2P;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNewGameController : IEnterNewGame
{
	private sealed class _onOnlineMatchReady_c__AnonStorey0
	{
		internal SP2PMatchBasicPlayerDesc[] players;

		internal EnterNewGameController _this;

		internal void __m__0()
		{
			this._this.InitPayload(GameStartType.FreePlay, this._this.generateOnlineGamePayload(GameStartType.FreePlay, this.players));
			this._this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			this._this.loadScreen(ScreenType.OnlineBlindPick);
		}
	}

	private sealed class _resetOptions_c__AnonStorey1
	{
		internal Action callback;

		internal EnterNewGameController _this;

		internal void __m__0(SaveOptionsProfileResult result)
		{
			if (result != SaveOptionsProfileResult.SUCCESS)
			{
				this._this.dialogController.ShowOneButtonDialog("Placeholder error", "There was an error", "Continue", WindowTransition.STANDARD_FADE, false, default(AudioData));
			}
			else
			{
				this.callback();
			}
		}
	}

	private GameLoadPayload gamePayload;

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
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
	public ILocalization localization
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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
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

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public PlayerInputLocator playerInput
	{
		get;
		set;
	}

	[Inject]
	public IBackgroundLoader backgroundLoader
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
	public GameController gameController
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
	public ICursorManager cursorManager
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
	public ICustomLobbyController customLobby
	{
		get;
		set;
	}

	[Inject]
	public IRichPresence richPresence
	{
		get;
		set;
	}

	public GameLoadPayload GamePayload
	{
		get
		{
			return this.gamePayload;
		}
	}

	[PostConstruct]
	public void Init()
	{
		IBattleServerAPI expr_06 = this.battleServerAPI;
		expr_06.OnMatchReady = (Action<SP2PMatchBasicPlayerDesc[]>)Delegate.Combine(expr_06.OnMatchReady, new Action<SP2PMatchBasicPlayerDesc[]>(this.onOnlineMatchReady));
		this.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.onEndGame));
	}

	private void onEndGame(VictoryScreenPayload payload)
	{
		this.gamePayload = null;
	}

	private GameLoadPayload generateGameLoadPayload(GameStartType startType)
	{
		GameLoadPayload gameLoadPayload = new GameLoadPayload();
		List<StageID> list = new List<StageID>();
		Dictionary<string, string> sceneNamePathMap = SceneUtil.GetSceneNamePathMap();
		foreach (StageData current in this.gameDataManager.StageData.DataList)
		{
			if (!(current == null))
			{
				if (current.stageType == StageType.Random)
				{
					list.Add(current.stageID);
				}
				else if (!sceneNamePathMap.ContainsKey(current.sceneName))
				{
					if (!current.isTemporary)
					{
						UnityEngine.Debug.LogError("No scene mapped in editor for stage " + current.stageName);
					}
				}
				else
				{
					string text = sceneNamePathMap[current.sceneName];
					if (current.enabled && !current.isDev)
					{
						list.Add(current.stageID);
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
		foreach (GameModeData current2 in this.gameDataManager.GameModeData.DataList)
		{
			if (current2 != null && current2.enabled && ((startType == GameStartType.CrewBattle && current2.Type == GameMode.CrewBattle) || (startType == GameStartType.FreePlay && current2.selectableInFreePlay)) && (!current2.debugOnly || Debug.isDebugBuild))
			{
				list2.Add(current2.Type);
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
			foreach (GameMode current3 in gameLoadPayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = gameLoadPayload.teamPlayerMap[current3];
				TeamNum team = playerSelectionInfo.GetTeam(current3);
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

	private void onOnlineMatchReady(SP2PMatchBasicPlayerDesc[] players)
	{
		EnterNewGameController._onOnlineMatchReady_c__AnonStorey0 _onOnlineMatchReady_c__AnonStorey = new EnterNewGameController._onOnlineMatchReady_c__AnonStorey0();
		_onOnlineMatchReady_c__AnonStorey.players = players;
		_onOnlineMatchReady_c__AnonStorey._this = this;
		this.optionsProfileAPI.SetDefaultGameMode(GameMode.FreeForAll);
		this.resetOptions(new Action(_onOnlineMatchReady_c__AnonStorey.__m__0));
	}

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

	private void loadScreen(ScreenType screen)
	{
		this.events.Broadcast(new LoadScreenCommand(screen, null, ScreenUpdateType.Next));
	}

	private void resetOptions(Action callback)
	{
		EnterNewGameController._resetOptions_c__AnonStorey1 _resetOptions_c__AnonStorey = new EnterNewGameController._resetOptions_c__AnonStorey1();
		_resetOptions_c__AnonStorey.callback = callback;
		_resetOptions_c__AnonStorey._this = this;
		this.optionsProfileAPI.DeleteDefaultSettings(new Action<SaveOptionsProfileResult>(_resetOptions_c__AnonStorey.__m__0));
	}

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
			foreach (GameMode current in this.gamePayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = this.gamePayload.teamPlayerMap[current];
				TeamNum team = playerSelectionInfo.GetTeam(current);
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
		foreach (StageData current2 in dataList)
		{
			if (current2.sceneName.Equals(name) || current2.lowDetailSceneName.Equals(name))
			{
				this.gamePayload.stage = (StageID)current2.ID;
			}
		}
		this.backgroundLoader.WaitForSetup(new Action(this._StartPreviewGame_m__0));
	}

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
				UnityEngine.Debug.LogError(string.Format("Invalid CharacterId of {0} in test game settings.  Defaulting to Kidd.", testGamePlayerSettings.characterId));
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
			foreach (GameMode current in this.gamePayload.teamPlayerMap.Keys)
			{
				Dictionary<TeamNum, List<PlayerNum>> dictionary = this.gamePayload.teamPlayerMap[current];
				TeamNum team = playerSelectionInfo.GetTeam(current);
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
		this.backgroundLoader.WaitForSetup(new Action(this._StartTestGame_m__1));
	}

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
		this.backgroundLoader.WaitForSetup(new Action(this._StartReplay_m__2));
		return true;
	}

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

	private void _StartPreviewGame_m__0()
	{
		this.events.Broadcast(new AllPlayersReadyMessage());
		this.events.Broadcast(typeof(CreateGameManagerCommand));
		this.events.Broadcast(new SetupGameCommand(this.gamePayload, -1f));
		this.uiAdapter.PreloadScreen(ScreenType.BattleGUI);
		this.cursorManager.SetDisplay(false);
		this.signalBus.Dispatch(StageLoader.GAME_BEGINNING);
		this.gameController.ClientReadyToStartGame();
	}

	private void _StartTestGame_m__1()
	{
		this.events.Broadcast(new AllPlayersReadyMessage());
		this.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
		this.richPresence.SetPresence("Loading", null, null, null);
	}

	private void _StartReplay_m__2()
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
		this.richPresence.SetPresence("Loading", null, null, null);
	}
}
