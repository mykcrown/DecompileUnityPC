using System;
using System.Collections.Generic;
using IconsServer;
using MatchMaking;
using P2P;
using Steamworks;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public class CustomLobbyController : ICustomLobbyController
{
	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06003170 RID: 12656 RVA: 0x000F1161 File Offset: 0x000EF561
	// (set) Token: 0x06003171 RID: 12657 RVA: 0x000F1169 File Offset: 0x000EF569
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06003172 RID: 12658 RVA: 0x000F1172 File Offset: 0x000EF572
	// (set) Token: 0x06003173 RID: 12659 RVA: 0x000F117A File Offset: 0x000EF57A
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x06003174 RID: 12660 RVA: 0x000F1183 File Offset: 0x000EF583
	// (set) Token: 0x06003175 RID: 12661 RVA: 0x000F118B File Offset: 0x000EF58B
	[Inject]
	public IAccountAPI accountAPI { get; set; }

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x06003176 RID: 12662 RVA: 0x000F1194 File Offset: 0x000EF594
	// (set) Token: 0x06003177 RID: 12663 RVA: 0x000F119C File Offset: 0x000EF59C
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x06003178 RID: 12664 RVA: 0x000F11A5 File Offset: 0x000EF5A5
	// (set) Token: 0x06003179 RID: 12665 RVA: 0x000F11AD File Offset: 0x000EF5AD
	[Inject]
	public IStageDataHelper stageDataHelper { get; set; }

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x0600317A RID: 12666 RVA: 0x000F11B6 File Offset: 0x000EF5B6
	// (set) Token: 0x0600317B RID: 12667 RVA: 0x000F11BE File Offset: 0x000EF5BE
	[Inject]
	public ICustomLobbyEventNotifier customLobbyEventNotifier { get; set; }

	// Token: 0x17000C07 RID: 3079
	// (get) Token: 0x0600317C RID: 12668 RVA: 0x000F11C7 File Offset: 0x000EF5C7
	// (set) Token: 0x0600317D RID: 12669 RVA: 0x000F11CF File Offset: 0x000EF5CF
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x0600317E RID: 12670 RVA: 0x000F11D8 File Offset: 0x000EF5D8
	// (set) Token: 0x0600317F RID: 12671 RVA: 0x000F11E0 File Offset: 0x000EF5E0
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x06003180 RID: 12672 RVA: 0x000F11E9 File Offset: 0x000EF5E9
	// (set) Token: 0x06003181 RID: 12673 RVA: 0x000F11F1 File Offset: 0x000EF5F1
	[Inject]
	public IDialogController dialog { get; set; }

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x06003182 RID: 12674 RVA: 0x000F11FA File Offset: 0x000EF5FA
	// (set) Token: 0x06003183 RID: 12675 RVA: 0x000F1202 File Offset: 0x000EF602
	[Inject]
	public SteamManager steamManager { get; set; }

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x06003184 RID: 12676 RVA: 0x000F120B File Offset: 0x000EF60B
	// (set) Token: 0x06003185 RID: 12677 RVA: 0x000F1213 File Offset: 0x000EF613
	[Inject]
	public P2PClient p2pClient { private get; set; }

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x06003186 RID: 12678 RVA: 0x000F121C File Offset: 0x000EF61C
	// (set) Token: 0x06003187 RID: 12679 RVA: 0x000F1224 File Offset: 0x000EF624
	[Inject]
	public P2PHost p2pHost { private get; set; }

	// Token: 0x17000C0D RID: 3085
	// (get) Token: 0x06003188 RID: 12680 RVA: 0x000F122D File Offset: 0x000EF62D
	// (set) Token: 0x06003189 RID: 12681 RVA: 0x000F1235 File Offset: 0x000EF635
	[Inject]
	public IBattleServerAPI battleServer { private get; set; }

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x0600318A RID: 12682 RVA: 0x000F123E File Offset: 0x000EF63E
	// (set) Token: 0x0600318B RID: 12683 RVA: 0x000F1246 File Offset: 0x000EF646
	[Inject]
	public IUIAdapter uiAdapter { private get; set; }

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x0600318C RID: 12684 RVA: 0x000F124F File Offset: 0x000EF64F
	public bool IsInLobby
	{
		get
		{
			return this.lobbyState == CustomLobbyController.LobbyState.InLobby;
		}
	}

	// Token: 0x17000C10 RID: 3088
	// (get) Token: 0x0600318D RID: 12685 RVA: 0x000F125A File Offset: 0x000EF65A
	public string LobbyName
	{
		get
		{
			return this.lobbyName;
		}
	}

	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x0600318E RID: 12686 RVA: 0x000F1262 File Offset: 0x000EF662
	public string LobbyPassword
	{
		get
		{
			return this.lobbyPass;
		}
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x0600318F RID: 12687 RVA: 0x000F126A File Offset: 0x000EF66A
	public StageID StageID
	{
		get
		{
			return this.stageID;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x06003190 RID: 12688 RVA: 0x000F1272 File Offset: 0x000EF672
	public LobbyGameMode ModeID
	{
		get
		{
			return this.modeID;
		}
	}

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x06003191 RID: 12689 RVA: 0x000F127A File Offset: 0x000EF67A
	public bool IsLobbyLeader
	{
		get
		{
			return this.MyUserID == this.hostUserId;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x06003192 RID: 12690 RVA: 0x000F128A File Offset: 0x000EF68A
	public ulong HostUserId
	{
		get
		{
			return this.hostUserId;
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06003193 RID: 12691 RVA: 0x000F1294 File Offset: 0x000EF694
	public ulong MyUserID
	{
		get
		{
			return this.steamManager.MySteamID().m_SteamID;
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x000F12B4 File Offset: 0x000EF6B4
	public bool IsTeams
	{
		get
		{
			return this.ModeID == LobbyGameMode.Teams;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x000F12BF File Offset: 0x000EF6BF
	public LobbyEvent LastEvent
	{
		get
		{
			return this.lobbyEvent;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x06003196 RID: 12694 RVA: 0x000F12C8 File Offset: 0x000EF6C8
	public int NumLobbyPlayers
	{
		get
		{
			int num = 0;
			foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
			{
				if (lobbyPlayerData != null)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x06003197 RID: 12695 RVA: 0x000F1330 File Offset: 0x000EF730
	public bool IsLobbyInMatch
	{
		get
		{
			return this.isLobbyInMatch;
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06003198 RID: 12696 RVA: 0x000F1338 File Offset: 0x000EF738
	public Dictionary<ulong, LobbyPlayerData> Players
	{
		get
		{
			return this.players;
		}
	}

	// Token: 0x17000C1C RID: 3100
	// (get) Token: 0x06003199 RID: 12697 RVA: 0x000F1340 File Offset: 0x000EF740
	public bool IsSpectator
	{
		get
		{
			foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
			{
				if (lobbyPlayerData.userID == this.steamManager.MySteamID().m_SteamID)
				{
					return lobbyPlayerData.isSpectator;
				}
			}
			return false;
		}
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x000F13C8 File Offset: 0x000EF7C8
	public bool IsValidPlayerConfiguration()
	{
		if (this.IsLobbyInMatch)
		{
			return false;
		}
		int num = 0;
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (!lobbyPlayerData.isSpectator)
			{
				num++;
			}
		}
		return num >= this.gameDataManager.ConfigData.lobbySettings.minPlayerNum && this.IsInLobby && this.IsLobbyLeader;
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x000F1470 File Offset: 0x000EF870
	public bool IsAllPlayersReadyForCSS()
	{
		if (this.IsLobbyInMatch)
		{
			return false;
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (lobbyPlayerData.currentScreen != ScreenType.CustomLobbyScreen)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x000F14F0 File Offset: 0x000EF8F0
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

	// Token: 0x0600319D RID: 12701 RVA: 0x000F1592 File Offset: 0x000EF992
	private void onScreenOpened()
	{
		this.p2pClient.InformScreenChanged(this.MyUserID, this.uiAdapter.CurrentScreen);
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x000F15B0 File Offset: 0x000EF9B0
	public void Create(string lobbyName, string lobbyPass, ELobbyType lobbyType)
	{
		if (this.lobbyState != CustomLobbyController.LobbyState.None)
		{
			return;
		}
		this.lobbyState = CustomLobbyController.LobbyState.PendingCreating;
		this.stageID = StageID.Random;
		this.steamManager.CreateLobby(lobbyName, lobbyPass, lobbyType, this.stageDataHelper.GetIconStageFromStageID(this.stageID).ToString(), delegate(ulong steamLobbyId)
		{
			if (steamLobbyId != 0UL)
			{
				Debug.Log("Steam Lobby Created With ID: " + steamLobbyId);
			}
			else
			{
				Debug.Log("Steam Lobby Creation Failed, Steam Invites Won't Work.");
			}
			this.lobbyName = lobbyName;
			this.lobbyPass = lobbyPass;
			this.battleServer.ResetRoom();
			this.p2pHost.OnCreateLobby();
		});
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x000F1638 File Offset: 0x000EFA38
	private SCustomLobbyParams GetDefaultLobbyParams()
	{
		return new SCustomLobbyParams
		{
			type = ECustomMatchType.Private,
			setCount = 1UL,
			numberOfLives = 3UL,
			maxPlayers = (ulong)this.gameDataManager.ConfigData.lobbySettings.maxPlayerNum,
			stages = new List<EIconStages>(),
			lobbyName = string.Empty,
			lobbyPass = string.Empty,
			gameMode = LobbyGameMode.Stock
		};
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x000F16A7 File Offset: 0x000EFAA7
	public void SetStage(StageID stageID)
	{
		if (!this.IsInLobby || this.stageID == stageID)
		{
			return;
		}
		this.stageID = stageID;
		this.updateLobbySettings();
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x000F16CE File Offset: 0x000EFACE
	public void SetMode(LobbyGameMode modeID)
	{
		if (!this.IsInLobby || this.modeID == modeID)
		{
			return;
		}
		this.modeID = modeID;
		this.updateLobbySettings();
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x000F16F8 File Offset: 0x000EFAF8
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

	// Token: 0x060031A3 RID: 12707 RVA: 0x000F175C File Offset: 0x000EFB5C
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

	// Token: 0x060031A4 RID: 12708 RVA: 0x000F17B4 File Offset: 0x000EFBB4
	public void HostChangePlayer(ulong userID, bool isSpectating, int team)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (lobbyPlayerData.userID == userID)
			{
				lobbyPlayerData.isSpectator = isSpectating;
				lobbyPlayerData.team = team;
			}
		}
		this.p2pHost.ConfirmPlayerChange(userID, isSpectating, team);
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x000F1854 File Offset: 0x000EFC54
	public void ReceivedPlayerChange(ulong userID, bool isSpectating, int team)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (lobbyPlayerData.userID == userID)
			{
				lobbyPlayerData.isSpectator = isSpectating;
				lobbyPlayerData.team = team;
			}
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x000F18E4 File Offset: 0x000EFCE4
	public void ReceivedScreenChanged(ulong userID, int screenID)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (lobbyPlayerData.userID == userID)
			{
				lobbyPlayerData.currentScreen = (ScreenType)screenID;
			}
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x000F1970 File Offset: 0x000EFD70
	public void HostReceivedScreenChanged(ulong userID, int screenID)
	{
		if (!this.IsInLobby)
		{
			return;
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
		{
			if (lobbyPlayerData.userID == userID)
			{
				lobbyPlayerData.currentScreen = (ScreenType)screenID;
			}
		}
		this.p2pHost.ConfirmScreenChanged(userID, screenID);
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000F1A08 File Offset: 0x000EFE08
	public void ReceivedIsLobbyInMatch(bool isInMatch)
	{
		this.isLobbyInMatch = isInMatch;
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x000F1A21 File Offset: 0x000EFE21
	public void Leave()
	{
		if (this.lobbyState != CustomLobbyController.LobbyState.InLobby)
		{
			return;
		}
		this.ResetState();
		this.iconsServerAPI.LeaveCustomMatch();
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000F1A42 File Offset: 0x000EFE42
	public void StartMatch()
	{
		if (!this.IsValidPlayerConfiguration())
		{
			return;
		}
		this.iconsServerAPI.StartCustomMatch(this.players, this.modeID);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x000F1A68 File Offset: 0x000EFE68
	private int getTeamForNewPlayer()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		dictionary[0] = 0;
		dictionary[1] = 0;
		foreach (KeyValuePair<ulong, LobbyPlayerData> keyValuePair in this.players)
		{
			Dictionary<int, int> dictionary2;
			int team;
			(dictionary2 = dictionary)[team = keyValuePair.Value.team] = dictionary2[team] + 1;
		}
		if (dictionary[0] > dictionary[1])
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x000F1B0C File Offset: 0x000EFF0C
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
			LobbyPlayerData lobbyPlayerData;
			foreach (SBasicMatchPlayerDesc sbasicMatchPlayerDesc in joinCustomMatchEvent.players)
			{
				if (!this.players.TryGetValue(sbasicMatchPlayerDesc.userID, out lobbyPlayerData))
				{
					lobbyPlayerData = new LobbyPlayerData(sbasicMatchPlayerDesc.name, sbasicMatchPlayerDesc.userID, ScreenType.CustomLobbyScreen);
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
			foreach (ulong item in this.players.Keys)
			{
				if (!list.Contains(item))
				{
					list2.Add(item);
				}
			}
			foreach (ulong key in list2)
			{
				this.players.Remove(key);
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

	// Token: 0x060031AD RID: 12717 RVA: 0x000F1EEC File Offset: 0x000F02EC
	private void SyncAllPlayers()
	{
		if (this.IsLobbyLeader)
		{
			foreach (LobbyPlayerData lobbyPlayerData in this.players.Values)
			{
				this.p2pHost.ConfirmPlayerChange(lobbyPlayerData.userID, lobbyPlayerData.isSpectator, lobbyPlayerData.team);
				this.p2pHost.ConfirmScreenChanged(lobbyPlayerData.userID, (int)lobbyPlayerData.currentScreen);
			}
			this.p2pHost.SyncLobbyData();
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x000F1F90 File Offset: 0x000F0390
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

	// Token: 0x060031AF RID: 12719 RVA: 0x000F2050 File Offset: 0x000F0450
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

	// Token: 0x060031B0 RID: 12720 RVA: 0x000F20AC File Offset: 0x000F04AC
	private void onCustomMatchParamsChanged(ServerEvent message)
	{
		CustomMatchParamsChangedEvent customMatchParamsChangedEvent = (CustomMatchParamsChangedEvent)message;
		EIconStages stage = customMatchParamsChangedEvent.stageList[0];
		this.stageID = this.stageDataHelper.GetStageIDFromIconStage(stage);
		this.modeID = customMatchParamsChangedEvent.gameMode;
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x000F20F7 File Offset: 0x000F04F7
	private void onMatchConnect(ServerEvent message)
	{
		if (this.IsInLobby)
		{
			this.lobbyEvent = LobbyEvent.StartingLobbyMatch;
			this.signalBus.Dispatch(CustomLobbyController.EVENT);
		}
		this.signalBus.Dispatch(CustomLobbyController.UPDATED);
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x000F212C File Offset: 0x000F052C
	private void ResetState()
	{
		this.lobbyName = null;
		this.players.Clear();
		this.hostUserId = 0UL;
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

	// Token: 0x040022AC RID: 8876
	public static string UPDATED = "CustomLobbyController.UPDATED";

	// Token: 0x040022AD RID: 8877
	public static string EVENT = "CustomLobbyController.EVENT";

	// Token: 0x040022BC RID: 8892
	private string lobbyName;

	// Token: 0x040022BD RID: 8893
	private string lobbyPass;

	// Token: 0x040022BE RID: 8894
	private StageID stageID;

	// Token: 0x040022BF RID: 8895
	private LobbyGameMode modeID;

	// Token: 0x040022C0 RID: 8896
	private Dictionary<ulong, LobbyPlayerData> players = new Dictionary<ulong, LobbyPlayerData>();

	// Token: 0x040022C1 RID: 8897
	private ulong hostUserId;

	// Token: 0x040022C2 RID: 8898
	private CustomLobbyController.LobbyState lobbyState;

	// Token: 0x040022C3 RID: 8899
	private LobbyEvent lobbyEvent;

	// Token: 0x040022C4 RID: 8900
	private bool isLobbyInMatch;

	// Token: 0x040022C5 RID: 8901
	private PresenceLobbyParameters presenceLobbyParams = default(PresenceLobbyParameters);

	// Token: 0x020007D2 RID: 2002
	private enum LobbyState
	{
		// Token: 0x040022C7 RID: 8903
		None,
		// Token: 0x040022C8 RID: 8904
		PendingCreating,
		// Token: 0x040022C9 RID: 8905
		PendingJoining,
		// Token: 0x040022CA RID: 8906
		InLobby
	}
}
