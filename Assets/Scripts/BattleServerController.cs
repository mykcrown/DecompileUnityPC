// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using P2P;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleServerController : IBattleServerAPI
{
	public static string UPDATE_STATE = "BattleServerController.UPDATE_STATE";

	private bool isConnected;

	private bool hasMatchData;

	private List<PlayerNum> localPlayerNumIds = new List<PlayerNum>();

	public bool[] MatchResult;

	private bool inMatchSet;

	private Dictionary<Type, ServerEventHandler> serverEventHandlers = new Dictionary<Type, ServerEventHandler>();

	private SBasicMatchPlayerDesc myPlayerInfo;

	private static int ringMsgBufferSize = 100;

	private InputMsg[] inputMsgBuffer = new InputMsg[BattleServerController.ringMsgBufferSize];

	private int inputMsgRingBufferIndex;

	private InputAckMsg[] inputAckMsgBuffer = new InputAckMsg[BattleServerController.ringMsgBufferSize];

	private int inputAckMsgRingBufferIndex;

	private HashCodeMsg[] hashCodeMsgBuffer = new HashCodeMsg[BattleServerController.ringMsgBufferSize];

	private int hashCodeMsgRingBufferIndex;

	private RequestMissingInputMsg[] requestMissingInputMsgBuffer = new RequestMissingInputMsg[BattleServerController.ringMsgBufferSize];

	private int requestMissingInputMsgRingBufferIndex;

	private DisconnectMsg[] disconnectMsgBuffer = new DisconnectMsg[BattleServerController.ringMsgBufferSize];

	private int disconnectMsgRingBufferIndex;

	private DisconnectAckMsg[] disconnectAckMsgBuffer = new DisconnectAckMsg[BattleServerController.ringMsgBufferSize];

	private int disconnectAckMsgRingBufferIndex;

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerStagingAPI staging
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
	public ConfigData config
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
	public IPingManager pingManager
	{
		private get;
		set;
	}

	[Inject]
	public ITimeSynchronizer timeSynchronizer
	{
		private get;
		set;
	}

	[Inject]
	public SteamManager steamManager
	{
		private get;
		set;
	}

	public bool IsConnected
	{
		get
		{
			return this.isConnected;
		}
	}

	public bool IsOnlineMatchReady
	{
		get
		{
			return this.isConnected && this.hasMatchData;
		}
	}

	public bool IsSinglePlayerNetworkGame
	{
		get
		{
			return this.LocalPlayerNumIDs != null && this.LocalPlayerNumIDs.Count == 1;
		}
	}

	public int CurrentMatchIndex
	{
		get;
		private set;
	}

	public int NumMatches
	{
		get;
		private set;
	}

	public List<PlayerNum> LocalPlayerNumIDs
	{
		get
		{
			return (!this.isConnected) ? null : this.localPlayerNumIds;
		}
	}

	public PlayerNum GetPrimaryLocalPlayer
	{
		get
		{
			return this.LocalPlayerNumIDs[0];
		}
	}

	public int PlayerCount
	{
		get;
		private set;
	}

	public int NumTeams
	{
		get;
		private set;
	}

	public int MatchTime
	{
		get;
		private set;
	}

	public int NumLives
	{
		get;
		private set;
	}

	public int AssistCount
	{
		get;
		private set;
	}

	public GameMode MatchGameMode
	{
		get;
		private set;
	}

	public GameRules MatchRules
	{
		get;
		private set;
	}

	public bool TeamAttack
	{
		get;
		private set;
	}

	public Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers
	{
		get;
		private set;
	}

	public int NetworkBytesSent
	{
		get;
		private set;
	}

	public int NetworkBytesReceived
	{
		get;
		private set;
	}

	public Guid MatchID
	{
		get;
		private set;
	}

	public List<StageID> Stages
	{
		get;
		private set;
	}

	public float SelectionTime
	{
		get;
		private set;
	}

	public Action<SP2PMatchBasicPlayerDesc[]> OnMatchReady
	{
		get;
		set;
	}

	public Action<bool> OnLeftRoom
	{
		get;
		set;
	}

	public bool ReceivedMatchResults
	{
		get;
		private set;
	}

	public int ServerTimestepDelta
	{
		get
		{
			return this.iconsServerAPI.ServerTimestepDelta;
		}
	}

	public ulong ServerPing
	{
		get
		{
			return (ulong)this.pingManager.LatencyMs;
		}
	}

	public bool ResultsForMatch(int MatchIndex)
	{
		return this.MatchResult[MatchIndex];
	}

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
			this.inputMsgBuffer[i].SetAsBufferable(250u);
			this.inputAckMsgBuffer[i] = new InputAckMsg();
			this.inputAckMsgBuffer[i].SetAsBufferable(20u);
			this.hashCodeMsgBuffer[i] = new HashCodeMsg();
			this.hashCodeMsgBuffer[i].SetAsBufferable(20u);
			this.requestMissingInputMsgBuffer[i] = new RequestMissingInputMsg();
			this.requestMissingInputMsgBuffer[i].SetAsBufferable(50u);
			this.disconnectMsgBuffer[i] = new DisconnectMsg();
			this.disconnectMsgBuffer[i].SetAsBufferable(20u);
			this.disconnectAckMsgBuffer[i] = new DisconnectAckMsg();
			this.disconnectAckMsgBuffer[i].SetAsBufferable(20u);
		}
	}

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

	public void Listen<T>(ServerEventHandler handler) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		this.Listen(typeFromHandle, handler);
	}

	public void Listen(Type type, ServerEventHandler handler)
	{
		if (!this.serverEventHandlers.ContainsKey(type))
		{
			this.serverEventHandlers.Add(type, null);
		}
		Dictionary<Type, ServerEventHandler> dictionary;
		(dictionary = this.serverEventHandlers)[type] = (ServerEventHandler)Delegate.Combine(dictionary[type], handler);
	}

	public void Unsubscribe<T>(ServerEventHandler handler) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		this.Unsubscribe(typeFromHandle, handler);
	}

	public void Unsubscribe(Type type, ServerEventHandler handler)
	{
		if (!this.serverEventHandlers.ContainsKey(type))
		{
			UnityEngine.Debug.LogError("Attempted to unsubscribe type " + type + " unsuccessfully");
			return;
		}
		Dictionary<Type, ServerEventHandler> dictionary;
		(dictionary = this.serverEventHandlers)[type] = (ServerEventHandler)Delegate.Remove(dictionary[type], handler);
		if (this.serverEventHandlers[type] == null)
		{
			this.serverEventHandlers.Remove(type);
		}
	}

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
			UnityEngine.Debug.LogError("There is not enough bytes to represent the inputs when we send the frames since we're sending them as a uint16");
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
			SBasicMatchPlayerDesc sBasicMatchPlayerDesc = matchConnectEvent.players[j];
			TeamNum team = (TeamNum)sBasicMatchPlayerDesc.team;
			if (!this.TeamPlayers.ContainsKey(team))
			{
				this.TeamPlayers.Add(team, new List<SBasicMatchPlayerDesc>());
			}
			this.TeamPlayers[team].Add(sBasicMatchPlayerDesc);
			if (sBasicMatchPlayerDesc.userID == this.customLobby.MyUserID)
			{
				this.myPlayerInfo = sBasicMatchPlayerDesc;
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

	private void onCharacterStaging(ServerEvent message)
	{
		if (!this.isConnected)
		{
			return;
		}
		MatchCharacterStagingEvent matchCharacterStagingEvent = message as MatchCharacterStagingEvent;
		this.localPlayerNumIds.Clear();
		this.localPlayerNumIds.Add(this.GetPlayerNum(matchCharacterStagingEvent.playerIndex));
		UnityEngine.Debug.Log("ADD LOCAL PLAYER " + this.GetPlayerNum(matchCharacterStagingEvent.playerIndex));
		this.MatchID = matchCharacterStagingEvent.matchId;
		this.SelectionTime = matchCharacterStagingEvent.characterSelectSeconds;
		this.staging.ResetSelectionTimer();
		this.hasMatchData = true;
		this.OnMatchReady(matchCharacterStagingEvent.players);
	}

	public void StageLoaded()
	{
		this.iconsServerAPI.StageLoaded();
	}

	private void onLeaveRoomCommand(GameEvent message)
	{
		UnityEngine.Debug.Log("Command");
		this.LeaveRoom(false);
	}

	private void onDesynced()
	{
		this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDesync));
	}

	public void LeaveRoom(bool clientExpectsSetToEnd)
	{
		UnityEngine.Debug.Log("LEave room");
		if (this.MatchID != MatchMaking.Constants.skInvalidMatchId)
		{
			this.iconsServerAPI.LeaveMatch(this.MatchID);
		}
		this.onLeaveRoom(clientExpectsSetToEnd);
	}

	public void ResetRoom()
	{
		this.MatchID = MatchMaking.Constants.skInvalidMatchId;
		this.isConnected = false;
		this.hasMatchData = false;
		this.timeSynchronizer.Reset();
	}

	public void onLeaveRoom(bool clientExpectsSetEnd)
	{
		this.ResetRoom();
		if (this.OnLeftRoom != null)
		{
			this.OnLeftRoom(!clientExpectsSetEnd);
		}
	}

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
				UnityEngine.Debug.LogWarning("Tried to call null callback");
			}
			this.serverEventHandlers[type](serverEvent);
		}
	}

	private void onMatchBegin(ServerEvent serverEvent)
	{
		if (!this.isConnected)
		{
			return;
		}
		this.ReceivedMatchResults = false;
		this.handleServerEvent(serverEvent);
	}

	private void onMatchFailure(ServerEvent serverMessage)
	{
		UnityEngine.Debug.Log("MATCH FAILURE");
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
				UnityEngine.Debug.LogError("Unhandled match error:" + matchFailureEvent.reason);
				this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientServerMatchFailure));
			}
		}
		else
		{
			this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDisconnected));
		}
	}

	public void ClearNetUsage()
	{
		this.NetworkBytesSent = 0;
		this.NetworkBytesReceived = 0;
	}

	public void SendWinner(List<TeamNum> winningTeams)
	{
		byte b = 0;
		if (winningTeams.Count > 0)
		{
			foreach (TeamNum current in winningTeams)
			{
				int num = (int)current;
				b |= (byte)(1 << num);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("No winners " + b);
		}
		this.iconsServerAPI.SendClientWinner(b);
	}

	private GameMode GetGameModeFromLobbyGameMode(LobbyGameMode gameMode)
	{
		if (gameMode != LobbyGameMode.Teams)
		{
			return GameMode.FreeForAll;
		}
		return GameMode.Teams;
	}

	private GameRules GetGameRulesFromLobbyGameMode(LobbyGameMode gameMode)
	{
		if (gameMode != LobbyGameMode.Time)
		{
			return GameRules.Stock;
		}
		return GameRules.Time;
	}

	private bool GetTeamAttackFromMatchmakingTeamAttack(ETeamAttack teamAttack)
	{
		return teamAttack == ETeamAttack.Enabled || (teamAttack != ETeamAttack.Disabled && false);
	}

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
				UnityEngine.Debug.LogError("Unhandled match type for results");
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

	public PlayerNum GetPlayerNum(int index)
	{
		return (PlayerNum)index;
	}

	public void LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
	{
		this.iconsServerAPI.LockInSelection(characterID, characterIndex, skinID, isRandom);
	}

	public bool IsLocalPlayer(PlayerNum playerNum)
	{
		return !this.isConnected || this.LocalPlayerNumIDs.Contains(playerNum);
	}
}
