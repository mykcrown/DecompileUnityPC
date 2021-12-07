using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using P2P;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class BattleServerController : IBattleServerAPI
{
	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x06003027 RID: 12327 RVA: 0x000EF7D5 File Offset: 0x000EDBD5
	// (set) Token: 0x06003028 RID: 12328 RVA: 0x000EF7DD File Offset: 0x000EDBDD
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06003029 RID: 12329 RVA: 0x000EF7E6 File Offset: 0x000EDBE6
	// (set) Token: 0x0600302A RID: 12330 RVA: 0x000EF7EE File Offset: 0x000EDBEE
	[Inject]
	public IBattleServerStagingAPI staging { get; set; }

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x0600302B RID: 12331 RVA: 0x000EF7F7 File Offset: 0x000EDBF7
	// (set) Token: 0x0600302C RID: 12332 RVA: 0x000EF7FF File Offset: 0x000EDBFF
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x0600302D RID: 12333 RVA: 0x000EF808 File Offset: 0x000EDC08
	// (set) Token: 0x0600302E RID: 12334 RVA: 0x000EF810 File Offset: 0x000EDC10
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x0600302F RID: 12335 RVA: 0x000EF819 File Offset: 0x000EDC19
	// (set) Token: 0x06003030 RID: 12336 RVA: 0x000EF821 File Offset: 0x000EDC21
	[Inject]
	public IStageDataHelper stageDataHelper { get; set; }

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06003031 RID: 12337 RVA: 0x000EF82A File Offset: 0x000EDC2A
	// (set) Token: 0x06003032 RID: 12338 RVA: 0x000EF832 File Offset: 0x000EDC32
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06003033 RID: 12339 RVA: 0x000EF83B File Offset: 0x000EDC3B
	// (set) Token: 0x06003034 RID: 12340 RVA: 0x000EF843 File Offset: 0x000EDC43
	[Inject]
	public ICustomLobbyController customLobby { get; set; }

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06003035 RID: 12341 RVA: 0x000EF84C File Offset: 0x000EDC4C
	// (set) Token: 0x06003036 RID: 12342 RVA: 0x000EF854 File Offset: 0x000EDC54
	[Inject]
	public IPingManager pingManager { private get; set; }

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06003037 RID: 12343 RVA: 0x000EF85D File Offset: 0x000EDC5D
	// (set) Token: 0x06003038 RID: 12344 RVA: 0x000EF865 File Offset: 0x000EDC65
	[Inject]
	public ITimeSynchronizer timeSynchronizer { private get; set; }

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06003039 RID: 12345 RVA: 0x000EF86E File Offset: 0x000EDC6E
	// (set) Token: 0x0600303A RID: 12346 RVA: 0x000EF876 File Offset: 0x000EDC76
	[Inject]
	public SteamManager steamManager { private get; set; }

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x0600303B RID: 12347 RVA: 0x000EF87F File Offset: 0x000EDC7F
	public bool IsConnected
	{
		get
		{
			return this.isConnected;
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x0600303C RID: 12348 RVA: 0x000EF887 File Offset: 0x000EDC87
	public bool IsOnlineMatchReady
	{
		get
		{
			return this.isConnected && this.hasMatchData;
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x0600303D RID: 12349 RVA: 0x000EF89D File Offset: 0x000EDC9D
	public bool IsSinglePlayerNetworkGame
	{
		get
		{
			return this.LocalPlayerNumIDs != null && this.LocalPlayerNumIDs.Count == 1;
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x0600303E RID: 12350 RVA: 0x000EF8BB File Offset: 0x000EDCBB
	// (set) Token: 0x0600303F RID: 12351 RVA: 0x000EF8C3 File Offset: 0x000EDCC3
	public int CurrentMatchIndex { get; private set; }

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x06003040 RID: 12352 RVA: 0x000EF8CC File Offset: 0x000EDCCC
	// (set) Token: 0x06003041 RID: 12353 RVA: 0x000EF8D4 File Offset: 0x000EDCD4
	public int NumMatches { get; private set; }

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x06003042 RID: 12354 RVA: 0x000EF8DD File Offset: 0x000EDCDD
	public List<PlayerNum> LocalPlayerNumIDs
	{
		get
		{
			return (!this.isConnected) ? null : this.localPlayerNumIds;
		}
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06003043 RID: 12355 RVA: 0x000EF8F6 File Offset: 0x000EDCF6
	public PlayerNum GetPrimaryLocalPlayer
	{
		get
		{
			return this.LocalPlayerNumIDs[0];
		}
	}

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06003044 RID: 12356 RVA: 0x000EF904 File Offset: 0x000EDD04
	// (set) Token: 0x06003045 RID: 12357 RVA: 0x000EF90C File Offset: 0x000EDD0C
	public int PlayerCount { get; private set; }

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06003046 RID: 12358 RVA: 0x000EF915 File Offset: 0x000EDD15
	// (set) Token: 0x06003047 RID: 12359 RVA: 0x000EF91D File Offset: 0x000EDD1D
	public int NumTeams { get; private set; }

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06003048 RID: 12360 RVA: 0x000EF926 File Offset: 0x000EDD26
	// (set) Token: 0x06003049 RID: 12361 RVA: 0x000EF92E File Offset: 0x000EDD2E
	public int MatchTime { get; private set; }

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x0600304A RID: 12362 RVA: 0x000EF937 File Offset: 0x000EDD37
	// (set) Token: 0x0600304B RID: 12363 RVA: 0x000EF93F File Offset: 0x000EDD3F
	public int NumLives { get; private set; }

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x0600304C RID: 12364 RVA: 0x000EF948 File Offset: 0x000EDD48
	// (set) Token: 0x0600304D RID: 12365 RVA: 0x000EF950 File Offset: 0x000EDD50
	public int AssistCount { get; private set; }

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x0600304E RID: 12366 RVA: 0x000EF959 File Offset: 0x000EDD59
	// (set) Token: 0x0600304F RID: 12367 RVA: 0x000EF961 File Offset: 0x000EDD61
	public GameMode MatchGameMode { get; private set; }

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06003050 RID: 12368 RVA: 0x000EF96A File Offset: 0x000EDD6A
	// (set) Token: 0x06003051 RID: 12369 RVA: 0x000EF972 File Offset: 0x000EDD72
	public GameRules MatchRules { get; private set; }

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06003052 RID: 12370 RVA: 0x000EF97B File Offset: 0x000EDD7B
	// (set) Token: 0x06003053 RID: 12371 RVA: 0x000EF983 File Offset: 0x000EDD83
	public bool TeamAttack { get; private set; }

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x06003054 RID: 12372 RVA: 0x000EF98C File Offset: 0x000EDD8C
	// (set) Token: 0x06003055 RID: 12373 RVA: 0x000EF994 File Offset: 0x000EDD94
	public Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers { get; private set; }

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x06003056 RID: 12374 RVA: 0x000EF99D File Offset: 0x000EDD9D
	// (set) Token: 0x06003057 RID: 12375 RVA: 0x000EF9A5 File Offset: 0x000EDDA5
	public int NetworkBytesSent { get; private set; }

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x06003058 RID: 12376 RVA: 0x000EF9AE File Offset: 0x000EDDAE
	// (set) Token: 0x06003059 RID: 12377 RVA: 0x000EF9B6 File Offset: 0x000EDDB6
	public int NetworkBytesReceived { get; private set; }

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x0600305A RID: 12378 RVA: 0x000EF9BF File Offset: 0x000EDDBF
	// (set) Token: 0x0600305B RID: 12379 RVA: 0x000EF9C7 File Offset: 0x000EDDC7
	public Guid MatchID { get; private set; }

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x0600305C RID: 12380 RVA: 0x000EF9D0 File Offset: 0x000EDDD0
	// (set) Token: 0x0600305D RID: 12381 RVA: 0x000EF9D8 File Offset: 0x000EDDD8
	public List<StageID> Stages { get; private set; }

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x0600305E RID: 12382 RVA: 0x000EF9E1 File Offset: 0x000EDDE1
	// (set) Token: 0x0600305F RID: 12383 RVA: 0x000EF9E9 File Offset: 0x000EDDE9
	public float SelectionTime { get; private set; }

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06003060 RID: 12384 RVA: 0x000EF9F2 File Offset: 0x000EDDF2
	// (set) Token: 0x06003061 RID: 12385 RVA: 0x000EF9FA File Offset: 0x000EDDFA
	public Action<SP2PMatchBasicPlayerDesc[]> OnMatchReady { get; set; }

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06003062 RID: 12386 RVA: 0x000EFA03 File Offset: 0x000EDE03
	// (set) Token: 0x06003063 RID: 12387 RVA: 0x000EFA0B File Offset: 0x000EDE0B
	public Action<bool> OnLeftRoom { get; set; }

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06003064 RID: 12388 RVA: 0x000EFA14 File Offset: 0x000EDE14
	// (set) Token: 0x06003065 RID: 12389 RVA: 0x000EFA1C File Offset: 0x000EDE1C
	public bool ReceivedMatchResults { get; private set; }

	// Token: 0x06003066 RID: 12390 RVA: 0x000EFA25 File Offset: 0x000EDE25
	public bool ResultsForMatch(int MatchIndex)
	{
		return this.MatchResult[MatchIndex];
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000EFA30 File Offset: 0x000EDE30
	public void Initialize()
	{
		this.Stages = new List<StageID>();
		this.TeamPlayers = new Dictionary<TeamNum, List<SBasicMatchPlayerDesc>>();
		this.events.Subscribe(typeof(LeaveRoomCommand), new Events.EventHandler(this.onLeaveRoomCommand));
		this.iconsServerAPI.ListenForServerEvents<BatchEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.ListenForServerEvents<MatchConnectEvent>(new Action<ServerEvent>(this.onMatchConnect));
		this.iconsServerAPI.ListenForServerEvents<MatchFailureEvent>(new Action<ServerEvent>(this.onMatchFailure));
		this.iconsServerAPI.ListenForServerEvents<MatchCharacterStagingEvent>(new Action<ServerEvent>(this.onCharacterStaging));
		this.iconsServerAPI.ListenForServerEvents<MatchResultsEvent>(new Action<ServerEvent>(this.onMatchResult));
		this.iconsServerAPI.ListenForServerEvents<LockInSelectionAckEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.ListenForServerEvents<MatchDetailsEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.ListenForServerEvents<MatchBeginEvent>(new Action<ServerEvent>(this.onMatchBegin));
		this.signalBus.AddListener(RollbackStatePoolContainer.DESYNCED, new Action(this.onDesynced));
		for (int i = 0; i < BattleServerController.ringMsgBufferSize; i++)
		{
			this.inputMsgBuffer[i] = new InputMsg();
			this.inputMsgBuffer[i].SetAsBufferable(250U);
			this.inputAckMsgBuffer[i] = new InputAckMsg();
			this.inputAckMsgBuffer[i].SetAsBufferable(20U);
			this.hashCodeMsgBuffer[i] = new HashCodeMsg();
			this.hashCodeMsgBuffer[i].SetAsBufferable(20U);
			this.requestMissingInputMsgBuffer[i] = new RequestMissingInputMsg();
			this.requestMissingInputMsgBuffer[i].SetAsBufferable(50U);
			this.disconnectMsgBuffer[i] = new DisconnectMsg();
			this.disconnectMsgBuffer[i].SetAsBufferable(20U);
			this.disconnectAckMsgBuffer[i] = new DisconnectAckMsg();
			this.disconnectAckMsgBuffer[i].SetAsBufferable(20U);
		}
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000EFC0C File Offset: 0x000EE00C
	public void OnDestroy()
	{
		this.events.Unsubscribe(typeof(LeaveRoomCommand), new Events.EventHandler(this.onLeaveRoomCommand));
		this.iconsServerAPI.StopListeningForServerEvents<MatchCharacterStagingEvent>(new Action<ServerEvent>(this.onCharacterStaging));
		this.iconsServerAPI.StopListeningForServerEvents<MatchResultsEvent>(new Action<ServerEvent>(this.onMatchResult));
		this.iconsServerAPI.StopListeningForServerEvents<MatchFailureEvent>(new Action<ServerEvent>(this.onMatchFailure));
		this.iconsServerAPI.StopListeningForServerEvents<MatchConnectEvent>(new Action<ServerEvent>(this.onMatchConnect));
		this.iconsServerAPI.StopListeningForServerEvents<BatchEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.StopListeningForServerEvents<LockInSelectionAckEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.StopListeningForServerEvents<MatchDetailsEvent>(new Action<ServerEvent>(this.handleServerEvent));
		this.iconsServerAPI.StopListeningForServerEvents<MatchBeginEvent>(new Action<ServerEvent>(this.onMatchBegin));
		this.signalBus.RemoveListener(RollbackStatePoolContainer.DESYNCED, new Action(this.onDesynced));
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000EFD10 File Offset: 0x000EE110
	public void QueueUnreliableMessage(BatchEvent serverEvent)
	{
		INetMsg queuedMessage = null;
		if (serverEvent is InputEvent)
		{
			this.inputMsgBuffer[this.inputMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.inputMsgBuffer[this.inputMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.inputMsgRingBufferIndex++;
		}
		else if (serverEvent is InputAckEvent)
		{
			this.inputAckMsgBuffer[this.inputAckMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.inputAckMsgBuffer[this.inputAckMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.inputAckMsgRingBufferIndex++;
		}
		else if (serverEvent is HashCodeEvent)
		{
			this.hashCodeMsgBuffer[this.hashCodeMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.hashCodeMsgBuffer[this.hashCodeMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.hashCodeMsgRingBufferIndex++;
		}
		else if (serverEvent is RequestMissingInputEvent)
		{
			this.requestMissingInputMsgBuffer[this.requestMissingInputMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.requestMissingInputMsgBuffer[this.requestMissingInputMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.requestMissingInputMsgRingBufferIndex++;
		}
		else if (serverEvent is DisconnectEvent)
		{
			this.disconnectMsgBuffer[this.disconnectMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.disconnectMsgBuffer[this.disconnectMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.disconnectMsgRingBufferIndex++;
		}
		else
		{
			if (!(serverEvent is DisconnectAckEvent))
			{
				throw new Exception("Unsupported message");
			}
			this.disconnectAckMsgBuffer[this.disconnectAckMsgRingBufferIndex % BattleServerController.ringMsgBufferSize].ResetBuffer();
			queuedMessage = this.disconnectAckMsgBuffer[this.disconnectAckMsgRingBufferIndex % BattleServerController.ringMsgBufferSize];
			this.disconnectAckMsgRingBufferIndex++;
		}
		serverEvent.UpdateNetMessage(ref queuedMessage);
		if (this.IsConnected && !this.ReceivedMatchResults)
		{
			this.iconsServerAPI.EnqueueMessage(queuedMessage);
		}
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000EFF14 File Offset: 0x000EE314
	public void Listen<T>(ServerEventHandler handler) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		this.Listen(typeFromHandle, handler);
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000EFF34 File Offset: 0x000EE334
	public void Listen(Type type, ServerEventHandler handler)
	{
		if (!this.serverEventHandlers.ContainsKey(type))
		{
			this.serverEventHandlers.Add(type, null);
		}
		Dictionary<Type, ServerEventHandler> dictionary;
		(dictionary = this.serverEventHandlers)[type] = (ServerEventHandler)Delegate.Combine(dictionary[type], handler);
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x000EFF84 File Offset: 0x000EE384
	public void Unsubscribe<T>(ServerEventHandler handler) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		this.Unsubscribe(typeFromHandle, handler);
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x000EFFA4 File Offset: 0x000EE3A4
	public void Unsubscribe(Type type, ServerEventHandler handler)
	{
		if (!this.serverEventHandlers.ContainsKey(type))
		{
			Debug.LogError("Attempted to unsubscribe type " + type + " unsuccessfully");
			return;
		}
		Dictionary<Type, ServerEventHandler> dictionary;
		(dictionary = this.serverEventHandlers)[type] = (ServerEventHandler)Delegate.Remove(dictionary[type], handler);
		if (this.serverEventHandlers[type] == null)
		{
			this.serverEventHandlers.Remove(type);
		}
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x000F0018 File Offset: 0x000EE418
	private void onMatchConnect(ServerEvent message)
	{
		if (this.isConnected)
		{
			this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientInExistingMatch));
			if (this.MatchID != MatchMaking.Constants.skInvalidMatchId)
			{
				this.onLeaveRoom(false);
			}
			return;
		}
		MatchConnectEvent matchConnectEvent = message as MatchConnectEvent;
		this.Stages.Clear();
		this.isConnected = true;
		this.inMatchSet = true;
		this.CurrentMatchIndex = 0;
		this.NumMatches = matchConnectEvent.stages.Length;
		this.NumLives = (int)matchConnectEvent.numberOfLives;
		this.AssistCount = (int)matchConnectEvent.assistCount;
		this.MatchTime = (int)matchConnectEvent.matchLengthSeconds;
		if (this.MatchTime * 60 > 63735)
		{
			Debug.LogError("There is not enough bytes to represent the inputs when we send the frames since we're sending them as a uint16");
			this.onLeaveRoom(false);
			throw new Exception("Invalid Match Time");
		}
		this.MatchResult = new bool[this.NumMatches];
		for (int i = 0; i < this.NumMatches; i++)
		{
			this.MatchResult[i] = false;
			this.Stages.Add(this.stageDataHelper.GetStageIDFromIconStage(matchConnectEvent.stages[i]));
		}
		this.localPlayerNumIds.Clear();
		this.TeamPlayers.Clear();
		for (int j = 0; j < matchConnectEvent.players.Length; j++)
		{
			SBasicMatchPlayerDesc sbasicMatchPlayerDesc = matchConnectEvent.players[j];
			TeamNum team = (TeamNum)sbasicMatchPlayerDesc.team;
			if (!this.TeamPlayers.ContainsKey(team))
			{
				this.TeamPlayers.Add(team, new List<SBasicMatchPlayerDesc>());
			}
			this.TeamPlayers[team].Add(sbasicMatchPlayerDesc);
			if (sbasicMatchPlayerDesc.userID == this.customLobby.MyUserID)
			{
				this.myPlayerInfo = sbasicMatchPlayerDesc;
			}
		}
		this.PlayerCount = matchConnectEvent.players.Length;
		this.NumTeams = this.TeamPlayers.Count;
		this.MatchGameMode = this.GetGameModeFromLobbyGameMode(matchConnectEvent.gameMode);
		this.MatchRules = this.GetGameRulesFromLobbyGameMode(matchConnectEvent.gameMode);
		this.TeamAttack = this.GetTeamAttackFromMatchmakingTeamAttack(matchConnectEvent.teamAttack);
		this.handleServerEvent(message);
		this.signalBus.Dispatch(BattleServerController.UPDATE_STATE);
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x000F0238 File Offset: 0x000EE638
	private void onCharacterStaging(ServerEvent message)
	{
		if (!this.isConnected)
		{
			return;
		}
		MatchCharacterStagingEvent matchCharacterStagingEvent = message as MatchCharacterStagingEvent;
		this.localPlayerNumIds.Clear();
		this.localPlayerNumIds.Add(this.GetPlayerNum(matchCharacterStagingEvent.playerIndex));
		Debug.Log("ADD LOCAL PLAYER " + this.GetPlayerNum(matchCharacterStagingEvent.playerIndex));
		this.MatchID = matchCharacterStagingEvent.matchId;
		this.SelectionTime = matchCharacterStagingEvent.characterSelectSeconds;
		this.staging.ResetSelectionTimer();
		this.hasMatchData = true;
		this.OnMatchReady(matchCharacterStagingEvent.players);
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000F02D7 File Offset: 0x000EE6D7
	public void StageLoaded()
	{
		this.iconsServerAPI.StageLoaded();
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x000F02E5 File Offset: 0x000EE6E5
	private void onLeaveRoomCommand(GameEvent message)
	{
		Debug.Log("Command");
		this.LeaveRoom(false);
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000F02F8 File Offset: 0x000EE6F8
	private void onDesynced()
	{
		this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDesync));
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x000F030B File Offset: 0x000EE70B
	public void LeaveRoom(bool clientExpectsSetToEnd)
	{
		Debug.Log("LEave room");
		if (this.MatchID != MatchMaking.Constants.skInvalidMatchId)
		{
			this.iconsServerAPI.LeaveMatch(this.MatchID);
		}
		this.onLeaveRoom(clientExpectsSetToEnd);
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000F0345 File Offset: 0x000EE745
	public void ResetRoom()
	{
		this.MatchID = MatchMaking.Constants.skInvalidMatchId;
		this.isConnected = false;
		this.hasMatchData = false;
		this.timeSynchronizer.Reset();
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000F036B File Offset: 0x000EE76B
	public void onLeaveRoom(bool clientExpectsSetEnd)
	{
		this.ResetRoom();
		if (this.OnLeftRoom != null)
		{
			this.OnLeftRoom(!clientExpectsSetEnd);
		}
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000F0390 File Offset: 0x000EE790
	private void handleServerEvent(ServerEvent serverEvent)
	{
		if (!this.isConnected)
		{
			return;
		}
		Type type = serverEvent.GetType();
		if (this.serverEventHandlers.ContainsKey(type))
		{
			if (this.serverEventHandlers[type] == null)
			{
				Debug.LogWarning("Tried to call null callback");
			}
			this.serverEventHandlers[type](serverEvent);
		}
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000F03EE File Offset: 0x000EE7EE
	private void onMatchBegin(ServerEvent serverEvent)
	{
		if (!this.isConnected)
		{
			return;
		}
		this.ReceivedMatchResults = false;
		this.handleServerEvent(serverEvent);
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000F040C File Offset: 0x000EE80C
	private void onMatchFailure(ServerEvent serverMessage)
	{
		Debug.Log("MATCH FAILURE");
		this.LeaveRoom(true);
		MatchFailureEvent matchFailureEvent = serverMessage as MatchFailureEvent;
		MatchFailureEvent.EReason reason = matchFailureEvent.reason;
		if (reason != MatchFailureEvent.EReason.PlayerLeft)
		{
			if (reason != MatchFailureEvent.EReason.InternalFailure)
			{
				if (this.inMatchSet)
				{
					this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientServerMatchFailure));
				}
			}
			else if (this.inMatchSet)
			{
				Debug.LogError("Unhandled match error:" + matchFailureEvent.reason);
				this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientServerMatchFailure));
			}
		}
		else
		{
			this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDisconnected));
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000F04BE File Offset: 0x000EE8BE
	public void ClearNetUsage()
	{
		this.NetworkBytesSent = 0;
		this.NetworkBytesReceived = 0;
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000F04D0 File Offset: 0x000EE8D0
	public void SendWinner(List<TeamNum> winningTeams)
	{
		byte b = 0;
		if (winningTeams.Count > 0)
		{
			foreach (TeamNum teamNum in winningTeams)
			{
				int num = (int)teamNum;
				b |= (byte)(1 << num);
			}
		}
		else
		{
			Debug.LogError("No winners " + b);
		}
		this.iconsServerAPI.SendClientWinner(b);
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x000F0564 File Offset: 0x000EE964
	private GameMode GetGameModeFromLobbyGameMode(LobbyGameMode gameMode)
	{
		if (gameMode != LobbyGameMode.Teams)
		{
			return GameMode.FreeForAll;
		}
		return GameMode.Teams;
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000F0575 File Offset: 0x000EE975
	private GameRules GetGameRulesFromLobbyGameMode(LobbyGameMode gameMode)
	{
		if (gameMode != LobbyGameMode.Time)
		{
			return GameRules.Stock;
		}
		return GameRules.Time;
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000F0586 File Offset: 0x000EE986
	private bool GetTeamAttackFromMatchmakingTeamAttack(ETeamAttack teamAttack)
	{
		return teamAttack == ETeamAttack.Enabled || (teamAttack != ETeamAttack.Disabled && false);
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x000F05A0 File Offset: 0x000EE9A0
	private void onMatchResult(ServerEvent message)
	{
		MatchResultsEvent matchResultsEvent = message as MatchResultsEvent;
		if (this.NumMatches > 1)
		{
			if (this.NumTeams == 2)
			{
				if (((int)matchResultsEvent.winningTeamMask & 1 << (int)this.myPlayerInfo.team) != 0)
				{
					this.MatchResult[this.CurrentMatchIndex] = true;
				}
			}
			else
			{
				Debug.LogError("Unhandled match type for results");
			}
		}
		this.ReceivedMatchResults = true;
		List<TeamNum> list = new List<TeamNum>();
		for (int i = 0; i < 2; i++)
		{
			if (((int)matchResultsEvent.winningTeamMask & 1 << i) != 0)
			{
				list.Add((TeamNum)i);
			}
		}
		this.events.Broadcast(new MatchResultMessage(list));
		this.CurrentMatchIndex++;
		this.LeaveRoom(true);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x000F0664 File Offset: 0x000EEA64
	public bool IsSetComplete()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.CurrentMatchIndex; i++)
		{
			if (this.MatchResult[i])
			{
				num++;
			}
			else
			{
				num2++;
			}
		}
		return num > this.NumMatches / 2 || num2 > this.NumMatches / 2;
	}

	// Token: 0x06003080 RID: 12416 RVA: 0x000F06C2 File Offset: 0x000EEAC2
	public PlayerNum GetPlayerNum(int index)
	{
		return (PlayerNum)index;
	}

	// Token: 0x06003081 RID: 12417 RVA: 0x000F06C5 File Offset: 0x000EEAC5
	public void LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
	{
		this.iconsServerAPI.LockInSelection(characterID, characterIndex, skinID, isRandom);
	}

	// Token: 0x06003082 RID: 12418 RVA: 0x000F06D8 File Offset: 0x000EEAD8
	public bool IsLocalPlayer(PlayerNum playerNum)
	{
		return !this.isConnected || this.LocalPlayerNumIDs.Contains(playerNum);
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06003083 RID: 12419 RVA: 0x000F06F3 File Offset: 0x000EEAF3
	public int ServerTimestepDelta
	{
		get
		{
			return this.iconsServerAPI.ServerTimestepDelta;
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06003084 RID: 12420 RVA: 0x000F0700 File Offset: 0x000EEB00
	public ulong ServerPing
	{
		get
		{
			return (ulong)this.pingManager.LatencyMs;
		}
	}

	// Token: 0x040021AD RID: 8621
	public static string UPDATE_STATE = "BattleServerController.UPDATE_STATE";

	// Token: 0x040021B8 RID: 8632
	private bool isConnected;

	// Token: 0x040021B9 RID: 8633
	private bool hasMatchData;

	// Token: 0x040021BA RID: 8634
	private List<PlayerNum> localPlayerNumIds = new List<PlayerNum>();

	// Token: 0x040021CA RID: 8650
	public bool[] MatchResult;

	// Token: 0x040021CF RID: 8655
	private bool inMatchSet;

	// Token: 0x040021D0 RID: 8656
	private Dictionary<Type, ServerEventHandler> serverEventHandlers = new Dictionary<Type, ServerEventHandler>();

	// Token: 0x040021D1 RID: 8657
	private SBasicMatchPlayerDesc myPlayerInfo;

	// Token: 0x040021D2 RID: 8658
	private static int ringMsgBufferSize = 100;

	// Token: 0x040021D3 RID: 8659
	private InputMsg[] inputMsgBuffer = new InputMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021D4 RID: 8660
	private int inputMsgRingBufferIndex;

	// Token: 0x040021D5 RID: 8661
	private InputAckMsg[] inputAckMsgBuffer = new InputAckMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021D6 RID: 8662
	private int inputAckMsgRingBufferIndex;

	// Token: 0x040021D7 RID: 8663
	private HashCodeMsg[] hashCodeMsgBuffer = new HashCodeMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021D8 RID: 8664
	private int hashCodeMsgRingBufferIndex;

	// Token: 0x040021D9 RID: 8665
	private RequestMissingInputMsg[] requestMissingInputMsgBuffer = new RequestMissingInputMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021DA RID: 8666
	private int requestMissingInputMsgRingBufferIndex;

	// Token: 0x040021DB RID: 8667
	private DisconnectMsg[] disconnectMsgBuffer = new DisconnectMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021DC RID: 8668
	private int disconnectMsgRingBufferIndex;

	// Token: 0x040021DD RID: 8669
	private DisconnectAckMsg[] disconnectAckMsgBuffer = new DisconnectAckMsg[BattleServerController.ringMsgBufferSize];

	// Token: 0x040021DE RID: 8670
	private int disconnectAckMsgRingBufferIndex;
}
