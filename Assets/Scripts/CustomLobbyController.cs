// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using MatchMaking;
using P2P;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CustomLobbyController : ICustomLobbyController
{
	private enum LobbyState
	{
		None,
		PendingCreating,
		PendingJoining,
		InLobby
	}

	private sealed class _Create_c__AnonStorey0
	{
		internal string lobbyName;

		internal string lobbyPass;

		internal CustomLobbyController _this;

		internal void __m__0(ulong steamLobbyId)
		{
			if (steamLobbyId != 0uL)
			{
				UnityEngine.Debug.Log("Steam Lobby Created With ID: " + steamLobbyId);
			}
			else
			{
				UnityEngine.Debug.Log("Steam Lobby Creation Failed, Steam Invites Won't Work.");
			}
			this._this.lobbyName = this.lobbyName;
			this._this.lobbyPass = this.lobbyPass;
			this._this.battleServer.ResetRoom();
			this._this.p2pHost.OnCreateLobby();
		}
	}

	public static string UPDATED = "CustomLobbyController.UPDATED";

	public static string EVENT = "CustomLobbyController.EVENT";

	private string lobbyName;

	private string lobbyPass;

	private StageID stageID;

	private LobbyGameMode modeID;

	private Dictionary<ulong, LobbyPlayerData> players = new Dictionary<ulong, LobbyPlayerData>();

	private ulong hostUserId;

	private CustomLobbyController.LobbyState lobbyState;

	private LobbyEvent lobbyEvent;

	private bool isLobbyInMatch;

	private PresenceLobbyParameters presenceLobbyParams = default(PresenceLobbyParameters);

	[Inject]
	public IIconsServerAPI iconsServerAPI
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
	public IAccountAPI accountAPI
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
	public IStageDataHelper stageDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyEventNotifier customLobbyEventNotifier
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

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialog
	{
		get;
		set;
	}

	[Inject]
	public SteamManager steamManager
	{
		get;
		set;
	}

	[Inject]
	public P2PClient p2pClient
	{
		private get;
		set;
	}

	[Inject]
	public P2PHost p2pHost
	{
		private get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServer
	{
		private get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		private get;
		set;
	}

	public bool IsInLobby
	{
		get
		{
			return this.lobbyState == CustomLobbyController.LobbyState.InLobby;
		}
	}

	public string LobbyName
	{
		get
		{
			return this.lobbyName;
		}
	}

	public string LobbyPassword
	{
		get
		{
			return this.lobbyPass;
		}
	}

	public StageID StageID
	{
		get
		{
			return this.stageID;
		}
	}

	public LobbyGameMode ModeID
	{
		get
		{
			return this.modeID;
		}
	}

	public bool IsLobbyLeader
	{
		get
		{
			return this.MyUserID == this.hostUserId;
		}
	}

	public ulong HostUserId
	{
		get
		{
			return this.hostUserId;
		}
	}

	public ulong MyUserID
	{
		get
		{
			return this.steamManager.MySteamID().m_SteamID;
		}
	}

	public bool IsTeams
	{
		get
		{
			return this.ModeID == LobbyGameMode.Teams;
		}
	}

	public LobbyEvent LastEvent
	{
		get
		{
			return this.lobbyEvent;
		}
	}

	public int NumLobbyPlayers
	{
		get
		{
			int num = 0;
			foreach (LobbyPlayerData current in this.players.Values)
			{
				if (current != null)
				{
					num++;
				}
			}
			return num;
		}
	}

	public bool IsLobbyInMatch
	{
		get
		{
			return this.isLobbyInMatch;
		}
	}

	public Dictionary<ulong, LobbyPlayerData> Players
	{
		get
		{
			return this.players;
		}
	}

	public bool IsSpectator
	{
		get
		{
			foreach (LobbyPlayerData current in this.players.Values)
			{
				if (current.userID == this.steamManager.MySteamID().m_SteamID)
				{
					return current.isSpectator;
				}
			}
			return false;
		}
	}

	public bool IsValidPlayerConfiguration()
	{
		if (this.IsLobbyInMatch)
		{
			return false;
		}
		int num = 0;
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (!current.isSpectator)
			{
				num++;
			}
		}
		return num >= this.gameDataManager.ConfigData.lobbySettings.minPlayerNum && this.IsInLobby && this.IsLobbyLeader;
	}

	public bool IsAllPlayersReadyForCSS()
	{
		if (this.IsLobbyInMatch)
		{
			return false;
		}
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (current.currentScreen != ScreenType.CustomLobbyScreen)
			{
				return false;
			}
		}
		return true;
	}

	public void Initialize()
	{
		this.ResetState();
		this.iconsServerAPI.ListenForServerEvents<JoinCustomMatchEvent>(new Action<ServerEvent>(this.onJoinedCustomLobby));
		this.iconsServerAPI.ListenForServerEvents<LeaveCustomMatchEvent>(new Action<ServerEvent>(this.onLeaveCustomMatch));
		this.iconsServerAPI.ListenForServerEvents<CustomMatchDestroyedEvent>(new Action<ServerEvent>(this.onCustomMatchDestroyed));
		this.iconsServerAPI.ListenForServerEvents<CustomMatchParamsChangedEvent>(new Action<ServerEvent>(this.onCustomMatchParamsChanged));
		this.iconsServerAPI.ListenForServerEvents<MatchConnectEvent>(new Action<ServerEvent>(this.onMatchConnect));
		this.signalBus.AddListener(UIManager.SCREEN_OPENED, new Action(this.onScreenOpened));
	}

	private void onScreenOpened()
	{
		this.p2pClient.InformScreenChanged(this.MyUserID, this.uiAdapter.CurrentScreen);
	}

	public void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType)
	{
		CustomLobbyController._Create_c__AnonStorey0 _Create_c__AnonStorey = new CustomLobbyController._Create_c__AnonStorey0();
		_Create_c__AnonStorey.lobbyName = lobbyName;
		_Create_c__AnonStorey.lobbyPass = lobbyPass;
		_Create_c__AnonStorey._this = this;
		if (this.lobbyState != CustomLobbyController.LobbyState.None)
		{
			return;
		}
		this.lobbyState = CustomLobbyController.LobbyState.PendingCreating;
		this.stageID = StageID.Random;
		this.steamManager.CreateLobby(_Create_c__AnonStorey.lobbyName, _Create_c__AnonStorey.lobbyPass, lobbyType, this.stageDataHelper.GetIconStageFromStageID(this.stageID).ToString(), new Action<ulong>(_Create_c__AnonStorey.__m__0));
	}

	private SCustomLobbyParams GetDefaultLobbyParams()
	{
		return new SCustomLobbyParams
		{
			type = ECustomMatchType.Private,
			setCount = 1uL,
			numberOfLives = 3uL,
			maxPlayers = (ulong)this.gameDataManager.ConfigData.lobbySettings.maxPlayerNum,
			stages = new List<EIconStages>(),
			lobbyName = string.Empty,
			lobbyPass = string.Empty,
			gameMode = LobbyGameMode.Stock
		};
	}

	public void SetStage(StageID stageID)
	{
		if (!this.IsInLobby || this.stageID == stageID)
		{
			return;
		}
		this.stageID = stageID;
		this.updateLobbySettings();
	}

	public void SetMode(LobbyGameMode modeID)
	{
		if (!this.IsInLobby || this.modeID == modeID)
		{
			return;
		}
		this.modeID = modeID;
		this.updateLobbySettings();
	}

	private void updateLobbySettings()
	{
		EIconStages iconStageFromStageID = this.stageDataHelper.GetIconStageFromStageID(this.stageID);
		SCustomLobbyParams defaultLobbyParams = this.GetDefaultLobbyParams();
		defaultLobbyParams.stages.Add(iconStageFromStageID);
		defaultLobbyParams.lobbyName = this.lobbyName;
		defaultLobbyParams.lobbyPass = this.lobbyPass;
		defaultLobbyParams.gameMode = this.modeID;
		this.iconsServerAPI.CustomMatchSetParams(defaultLobbyParams);
	}

	public void ChangePlayer(ulong userID, bool isSpectating, int team)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		LobbyPlayerData lobbyPlayerData;
		if (this.players.TryGetValue(userID, out lobbyPlayerData))
		{
			lobbyPlayerData.isSpectator = isSpectating;
			lobbyPlayerData.team = team;
		}
		this.p2pClient.ChangePlayer(userID, isSpectating, team);
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void HostChangePlayer(ulong userID, bool isSpectating, int team)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (current.userID == userID)
			{
				current.isSpectator = isSpectating;
				current.team = team;
			}
		}
		this.p2pHost.ConfirmPlayerChange(userID, isSpectating, team);
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void ReceivedPlayerChange(ulong userID, bool isSpectating, int team)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (current.userID == userID)
			{
				current.isSpectator = isSpectating;
				current.team = team;
			}
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void ReceivedScreenChanged(ulong userID, int screenID)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (current.userID == userID)
			{
				current.currentScreen = (ScreenType)screenID;
			}
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void HostReceivedScreenChanged(ulong userID, int screenID)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData current in this.players.Values)
		{
			if (current.userID == userID)
			{
				current.currentScreen = (ScreenType)screenID;
			}
		}
		this.p2pHost.ConfirmScreenChanged(userID, screenID);
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void ReceivedIsLobbyInMatch(bool isInMatch)
	{
		this.isLobbyInMatch = isInMatch;
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	public void Leave()
	{
		if (this.lobbyState != CustomLobbyController.LobbyState.InLobby)
		{
			return;
		}
		this.ResetState();
		this.iconsServerAPI.LeaveCustomMatch();
	}

	public void StartMatch()
	{
		if (!this.IsValidPlayerConfiguration())
		{
			return;
		}
		this.iconsServerAPI.StartCustomMatch(this.players, this.modeID);
	}

	private int getTeamForNewPlayer()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		dictionary[0] = 0;
		dictionary[1] = 0;
		foreach (KeyValuePair<ulong, LobbyPlayerData> current in this.players)
		{
			Dictionary<int, int> dictionary2;
			int team;
			(dictionary2 = dictionary)[team = current.Value.team] = dictionary2[team] + 1;
		}
		if (dictionary[0] > dictionary[1])
		{
			return 1;
		}
		return 0;
	}

	private void onJoinedCustomLobby(ServerEvent message)
	{
		JoinCustomMatchEvent joinCustomMatchEvent = (JoinCustomMatchEvent)message;
		switch (joinCustomMatchEvent.result)
		{
		case JoinCustomMatchEvent.EResult.Result_Ok:
		{
			this.lobbyState = CustomLobbyController.LobbyState.InLobby;
			this.stageID = this.stageDataHelper.GetStageIDFromIconStage(joinCustomMatchEvent.stageList[0]);
			this.lobbyName = joinCustomMatchEvent.lobbyName;
			this.modeID = joinCustomMatchEvent.gameMode;
			List<ulong> list = new List<ulong>();
			SBasicMatchPlayerDesc[] array = joinCustomMatchEvent.players;
			LobbyPlayerData lobbyPlayerData;
			for (int i = 0; i < array.Length; i++)
			{
				SBasicMatchPlayerDesc sBasicMatchPlayerDesc = array[i];
				if (!this.players.TryGetValue(sBasicMatchPlayerDesc.userID, out lobbyPlayerData))
				{
					lobbyPlayerData = new LobbyPlayerData(sBasicMatchPlayerDesc.name, sBasicMatchPlayerDesc.userID, ScreenType.CustomLobbyScreen);
					lobbyPlayerData.team = this.getTeamForNewPlayer();
					this.players[lobbyPlayerData.userID] = lobbyPlayerData;
				}
				list.Add(lobbyPlayerData.userID);
			}
			if (!this.players.TryGetValue(this.steamManager.MySteamID().m_SteamID, out lobbyPlayerData))
			{
				lobbyPlayerData = new LobbyPlayerData(this.accountAPI.UserName, this.steamManager.MySteamID().m_SteamID, this.uiAdapter.CurrentScreen);
				lobbyPlayerData.team = this.getTeamForNewPlayer();
				this.players[lobbyPlayerData.userID] = lobbyPlayerData;
			}
			list.Add(lobbyPlayerData.userID);
			this.hostUserId = joinCustomMatchEvent.hostUserId;
			List<ulong> list2 = new List<ulong>();
			foreach (ulong current in this.players.Keys)
			{
				if (!list.Contains(current))
				{
					list2.Add(current);
				}
			}
			foreach (ulong current2 in list2)
			{
				this.players.Remove(current2);
			}
			this.richPresence.SetPresence("InCustomLobby", null, null, null);
			this.presenceLobbyParams.lobbyGuid = Guid.NewGuid();
			this.presenceLobbyParams.lobbySteamID = this.steamManager.CurrentSteamLobbyId;
			this.presenceLobbyParams.lobbyName = this.lobbyName;
			this.presenceLobbyParams.lobbyPassword = this.lobbyPass;
			this.presenceLobbyParams.numMembers = this.NumLobbyPlayers;
			this.presenceLobbyParams.maxMembers = this.gameDataManager.ConfigData.lobbySettings.maxPlayerNum;
			this.richPresence.SetLobbyParameters(this.presenceLobbyParams);
			this.SyncAllPlayers();
			this.signalBus.Dispatch(CustomLobbyController.UPDATED);
			return;
		}
		case JoinCustomMatchEvent.EResult.Result_InQueue:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedInQueue;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_InMatch:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedInMatch;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_SystemError:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinSystemError;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_TooLate:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedTooLate;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_InvalidCreds:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedInvalidCreds;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_LobbyFull:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedLobbyFull;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case JoinCustomMatchEvent.EResult.Result_MatchRunning:
			this.lobbyState = CustomLobbyController.LobbyState.None;
			this.lobbyEvent = LobbyEvent.JoinFailedMatchRunning;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		default:
			return;
		}
	}

	private void SyncAllPlayers()
	{
		if (this.IsLobbyLeader)
		{
			foreach (LobbyPlayerData current in this.players.Values)
			{
				this.p2pHost.ConfirmPlayerChange(current.userID, current.isSpectator, current.team);
				this.p2pHost.ConfirmScreenChanged(current.userID, (int)current.currentScreen);
			}
			this.p2pHost.SyncLobbyData();
		}
	}

	private void onLeaveCustomMatch(ServerEvent message)
	{
		LeaveCustomMatchEvent leaveCustomMatchEvent = (LeaveCustomMatchEvent)message;
		switch (leaveCustomMatchEvent.result)
		{
		case LeaveCustomMatchEvent.EResult.Result_Ok:
			this.ResetState();
			this.customLobbyEventNotifier.ClearCustomLobbyEventList();
			this.signalBus.Dispatch(CustomLobbyController.UPDATED);
			return;
		case LeaveCustomMatchEvent.EResult.Result_InQueue:
			this.lobbyEvent = LobbyEvent.LeaveFailedInQueue;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case LeaveCustomMatchEvent.EResult.Result_NotInMatch:
			this.lobbyEvent = LobbyEvent.LeaveFailedNotInLobby;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case LeaveCustomMatchEvent.EResult.Result_SystemError:
			this.lobbyEvent = LobbyEvent.LeaveSystemError;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		case LeaveCustomMatchEvent.EResult.Result_TooLate:
			this.lobbyEvent = LobbyEvent.JoinFailedTooLate;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
			return;
		default:
			return;
		}
	}

	private void onCustomMatchDestroyed(ServerEvent message)
	{
		this.ResetState();
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
		CustomMatchDestroyedEvent customMatchDestroyedEvent = (CustomMatchDestroyedEvent)message;
		CustomMatchDestroyedEvent.EReason reason = customMatchDestroyedEvent.reason;
		if (reason == CustomMatchDestroyedEvent.EReason.Reason_OwnerDestroyed)
		{
			this.lobbyEvent = LobbyEvent.DestroyedOwnerLeft;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
		}
	}

	private void onCustomMatchParamsChanged(ServerEvent message)
	{
		CustomMatchParamsChangedEvent customMatchParamsChangedEvent = (CustomMatchParamsChangedEvent)message;
		EIconStages stage = customMatchParamsChangedEvent.stageList[0];
		this.stageID = this.stageDataHelper.GetStageIDFromIconStage(stage);
		this.modeID = customMatchParamsChangedEvent.gameMode;
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	private void onMatchConnect(ServerEvent message)
	{
		if (this.IsInLobby)
		{
			this.lobbyEvent = LobbyEvent.StartingLobbyMatch;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	private void ResetState()
	{
		this.lobbyName = null;
		this.players.Clear();
		this.hostUserId = 0uL;
		this.lobbyState = CustomLobbyController.LobbyState.None;
		this.lobbyEvent = LobbyEvent.None;
		this.presenceLobbyParams.lobbyGuid = Guid.Empty;
		this.presenceLobbyParams.lobbyName = null;
		this.presenceLobbyParams.lobbyPassword = null;
		this.presenceLobbyParams.numMembers = 0;
		this.presenceLobbyParams.maxMembers = 0;
		this.richPresence.SetLobbyParameters(this.presenceLobbyParams);
		this.steamManager.LeaveLobby();
		this.richPresence.SetPresence(null, null, null, null);
	}
}
