// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using P2P;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class P2PServerMgr
{
	private static readonly int MAX_BATCH_MESSAGE_BYTES = 1000;

	private List<SteamManager.SteamLobbyData> m_receivers = new List<SteamManager.SteamLobbyData>();

	private Dictionary<ulong, CSteamID> userIDToSteamID = new Dictionary<ulong, CSteamID>();

	private Dictionary<ulong, List<INetMsg>> m_queuedTCPMessages = new Dictionary<ulong, List<INetMsg>>();

	private Dictionary<ulong, List<INetMsg>> m_queuedUDPMessages = new Dictionary<ulong, List<INetMsg>>();

	private Dictionary<Type, Action<ServerEvent>> m_messageHandlers = new Dictionary<Type, Action<ServerEvent>>();

	public BufferedBattleMsgs bufferedBattleMsgs = new BufferedBattleMsgs();

	private static readonly int MAX_BATCH_PER_MESSAGE_BYTES = 1000;

	private SMsg batchMsgBuffer = new SMsg();

	private byte[] recievedMessageBuffer;

	private byte[][] sendBuffers;

	[Inject]
	public SteamManager steam
	{
		get;
		set;
	}

	[Inject]
	public P2PHost p2pHost
	{
		get;
		set;
	}

	[Inject]
	public P2PClient p2pClient
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IUserTauntsModel tauntModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel characterEquipModel
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
	public IUDPWarming udpWarming
	{
		private get;
		set;
	}

	[Inject]
	public ICustomLobbyController customLobbyController
	{
		private get;
		set;
	}

	[Inject]
	public ISoundFileManager soundFileManager
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

	public ulong SessionId
	{
		get
		{
			return 0uL;
		}
	}

	public ulong AccountId
	{
		get
		{
			return 0uL;
		}
	}

	public string Username
	{
		get
		{
			return (!this.steam.Initialized) ? string.Empty : this.steam.UserName;
		}
	}

	public List<CSteamID> communicationIdBuffer
	{
		get;
		private set;
	}

	public long LastMessageReceivedMs
	{
		get;
		private set;
	}

	public bool IsHost
	{
		get
		{
			return this.steam.IsHost;
		}
	}

	public bool IsInBattle
	{
		get
		{
			return this.gameController.currentGame != null && this.gameController.currentGame.FrameController != null && this.gameController.currentGame.FrameController.IsBattleReady;
		}
	}

	public bool HasReceivers
	{
		get
		{
			return this.m_receivers.Count > 0;
		}
	}

	public P2PServerMgr()
	{
		this.communicationIdBuffer = new List<CSteamID>(8);
		List<byte[]> list = new List<byte[]>();
		list.Add(null);
		for (int i = 1; i <= 1024; i++)
		{
			list.Add(new byte[i]);
		}
		this.sendBuffers = list.ToArray();
	}

	public void Init()
	{
		this.recievedMessageBuffer = new byte[10000];
		this.batchMsgBuffer.buffer = new byte[P2PServerMgr.MAX_BATCH_PER_MESSAGE_BYTES];
		this.bufferedBattleMsgs.Init();
	}

	private bool isRequiredUserMissing()
	{
		List<ulong> list = new List<ulong>();
		foreach (LobbyPlayerData current in this.customLobbyController.Players.Values)
		{
			if (!current.isSpectator)
			{
				list.Add(current.userID);
			}
		}
		List<ulong> list2 = new List<ulong>();
		foreach (SteamManager.SteamLobbyData current2 in this.steam.GetReceivers())
		{
			list2.Add(current2.steamID.m_SteamID);
		}
		foreach (ulong current3 in list)
		{
			if (!list2.Contains(current3))
			{
				return true;
			}
		}
		return false;
	}

	private List<ulong> getMissingUsers()
	{
		List<ulong> list = new List<ulong>();
		List<ulong> list2 = new List<ulong>();
		foreach (SteamManager.SteamLobbyData current in this.steam.GetReceivers())
		{
			list2.Add(current.steamID.m_SteamID);
		}
		foreach (LobbyPlayerData current2 in this.customLobbyController.Players.Values)
		{
			if (!list2.Contains(current2.userID))
			{
				list.Add(current2.userID);
			}
		}
		return list;
	}

	public void OnUpdateCustomLobby()
	{
		bool forceDisconnect = this.isRequiredUserMissing();
		List<ulong> missingUsers = this.getMissingUsers();
		bool flag = this.m_receivers.Count < this.steam.GetReceivers().Count;
		this.m_receivers = this.steam.GetReceivers();
		this.cacheCommunicationList();
		this.UpdateLobbyists(forceDisconnect);
		if (this.m_receivers.Count <= 0)
		{
			this.timeSynchronizer.Reset();
			this.udpWarming.Reset();
		}
		this.timeSynchronizer.KeepOnlyThesePlayers(this.m_receivers);
		this.p2pHost.UsersLeft(missingUsers);
		if (flag)
		{
			this.audioManager.PlayMenuSound(this.soundFileManager.GetSoundAsAudioData(SoundKey.customLobby_customLobbyPlayerJoined), 0f);
		}
		else if (missingUsers.Count > 0)
		{
			this.audioManager.PlayMenuSound(this.soundFileManager.GetSoundAsAudioData(SoundKey.customLobby_customLobbyPlayerLeft), 0f);
		}
	}

	public void CleanupLobby()
	{
		this.m_receivers = this.steam.GetReceivers();
		this.cacheCommunicationList();
		if (this.m_receivers.Count <= 0)
		{
			this.timeSynchronizer.Reset();
			this.udpWarming.Reset();
		}
	}

	public void OnUpdateCustomLobbyData()
	{
		string lobbyValue = this.steam.GetLobbyValue(SteamManager.LobbyStageKey);
		string lobbyValue2 = this.steam.GetLobbyValue(SteamManager.LobbyGameModeKey);
		int num = 0;
		int.TryParse(lobbyValue2, out num);
		LobbyGameMode gameMode = (LobbyGameMode)num;
		if (!string.IsNullOrEmpty(lobbyValue))
		{
			EIconStages[] stageList = new EIconStages[]
			{
				(EIconStages)Enum.Parse(typeof(EIconStages), lobbyValue)
			};
			this.DoBroadcast(new CustomMatchParamsChangedEvent(stageList, gameMode));
		}
		else
		{
			this.LeaveMatch();
		}
	}

	public bool EnqueueMessage(INetMsg queuedMessage)
	{
		if (!(queuedMessage is IP2PMessage))
		{
			throw new Exception("Non p2p message" + queuedMessage);
		}
		ulong key = 0uL;
		if (queuedMessage is IMessageToSpecificUser)
		{
			key = (queuedMessage as IMessageToSpecificUser).TargetUserID;
		}
		List<INetMsg> list2;
		if (queuedMessage is IUDPMessage)
		{
			List<INetMsg> list;
			if (this.m_queuedUDPMessages.TryGetValue(key, out list))
			{
				list.Add(queuedMessage);
			}
		}
		else if (this.m_queuedTCPMessages.TryGetValue(key, out list2))
		{
			list2.Add(queuedMessage);
		}
		return true;
	}

	public void ClearNetworkMessages()
	{
		foreach (List<INetMsg> current in this.m_queuedUDPMessages.Values)
		{
			current.Clear();
		}
		foreach (List<INetMsg> current2 in this.m_queuedTCPMessages.Values)
		{
			current2.Clear();
		}
	}

	private void debugBytes(byte[] buffer, int size)
	{
		string text = string.Empty;
		for (int i = 0; i < size; i++)
		{
			text += buffer[i].ToString();
		}
		UnityEngine.Debug.LogError("Debug bytes: " + text);
	}

	public void ReceiveNetworkMessages()
	{
		uint msgSize = 0u;
		CSteamID nil = CSteamID.Nil;
		while (this.steam.GetMessage(ref this.recievedMessageBuffer, ref msgSize, ref nil))
		{
			if (this.steam.CurrentSteamLobbyId != 0uL)
			{
				BatchMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<BatchMsg>();
				bufferedNetMessage.DeserializeToBuffer(this.recievedMessageBuffer, msgSize);
				this.onReceiveBatch(bufferedNetMessage, nil);
			}
		}
	}

	private void onReceiveBatch(BatchMsg batchedMsg, CSteamID senderSteamID)
	{
		int i = 0;
		while (i < batchedMsg.numBatchedMessages)
		{
			BatchMsg.BatchedMsg batchedMsg2 = batchedMsg.m_batchedMessages[i];
			this.batchMsgBuffer.msgId = batchedMsg2.msgId;
			this.batchMsgBuffer.bufferSize = batchedMsg2.msgSize - 1u;
			if ((ulong)this.batchMsgBuffer.bufferSize > (ulong)((long)batchedMsg2.msgBuffer.Length))
			{
				if (batchedMsg2.msgSize == 0u)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Batched message with zero size, tricky ",
						i,
						" . ",
						this.batchMsgBuffer.msgId,
						" . ",
						this.batchMsgBuffer.bufferSize.ToString(),
						" . ",
						batchedMsg2.msgBuffer.Length.ToString()
					}));
					return;
				}
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"BOFlow ",
					i,
					" . ",
					this.batchMsgBuffer.msgId,
					" . ",
					this.batchMsgBuffer.bufferSize.ToString(),
					" . ",
					batchedMsg2.msgBuffer.Length.ToString()
				}));
				return;
			}
			else
			{
				try
				{
					Buffer.BlockCopy(batchedMsg2.msgBuffer, 1, this.batchMsgBuffer.buffer, 0, (int)this.batchMsgBuffer.bufferSize);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Buffer copy error ",
						this.batchMsgBuffer.msgId,
						" ",
						(int)this.batchMsgBuffer.bufferSize,
						" ",
						ex.Message
					}));
					break;
				}
				this.OnBattleGameMsg(this.batchMsgBuffer.msgId, this.batchMsgBuffer.buffer, this.batchMsgBuffer.bufferSize, senderSteamID);
				i++;
			}
		}
	}

	private void OnBattleGameMsg(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID)
	{
		this.LastMessageReceivedMs = WTime.currentTimeMs;
		if (this.pingManager.ScanIncomingMessage(msgId, buffer, bufferSize, senderSteamID))
		{
			return;
		}
		if (this.timeSynchronizer.ScanIncomingMessage(msgId, buffer, bufferSize, senderSteamID))
		{
			return;
		}
		if (this.udpWarming.ScanIncomingMessage(msgId, buffer, bufferSize, senderSteamID))
		{
			return;
		}
		switch (msgId)
		{
		case 0:
		{
			RelayMsg relayMsg = new RelayMsg(buffer, bufferSize);
			relayMsg.DeserializeMsg();
			this.onReceiveRelay(relayMsg);
			return;
		}
		case 1:
		{
			InputMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<InputMsg>();
			bufferedNetMessage.DeserializeToBuffer(buffer, bufferSize);
			this.onInput(bufferedNetMessage, senderSteamID);
			return;
		}
		case 2:
		{
			HashCodeMsg bufferedNetMessage2 = this.bufferedBattleMsgs.GetBufferedNetMessage<HashCodeMsg>();
			bufferedNetMessage2.DeserializeToBuffer(buffer, bufferSize);
			this.onHashCode(bufferedNetMessage2);
			return;
		}
		case 3:
		{
			InputAckMsg bufferedNetMessage3 = this.bufferedBattleMsgs.GetBufferedNetMessage<InputAckMsg>();
			bufferedNetMessage3.DeserializeToBuffer(buffer, bufferSize);
			this.onInputAck(bufferedNetMessage3, senderSteamID);
			return;
		}
		case 4:
		{
			RequestMissingInputMsg bufferedNetMessage4 = this.bufferedBattleMsgs.GetBufferedNetMessage<RequestMissingInputMsg>();
			bufferedNetMessage4.DeserializeToBuffer(buffer, bufferSize);
			this.onRequestMissingInput(bufferedNetMessage4);
			return;
		}
		case 11:
		{
			DisconnectMsg bufferedNetMessage5 = this.bufferedBattleMsgs.GetBufferedNetMessage<DisconnectMsg>();
			bufferedNetMessage5.DeserializeToBuffer(buffer, bufferSize);
			this.onDisconnect(bufferedNetMessage5);
			return;
		}
		case 12:
		{
			DisconnectAckMsg bufferedNetMessage6 = this.bufferedBattleMsgs.GetBufferedNetMessage<DisconnectAckMsg>();
			bufferedNetMessage6.DeserializeToBuffer(buffer, bufferSize);
			this.onDisconnectAck(bufferedNetMessage6);
			return;
		}
		case 13:
		{
			P2PLockCharacterSelectMsg p2PLockCharacterSelectMsg = new P2PLockCharacterSelectMsg(buffer, bufferSize);
			p2PLockCharacterSelectMsg.DeserializeMsg();
			this.onLockInCharacter(p2PLockCharacterSelectMsg.steamID, p2PLockCharacterSelectMsg.characterID, p2PLockCharacterSelectMsg.characterIndex, p2PLockCharacterSelectMsg.skinID, p2PLockCharacterSelectMsg.isRandom, p2PLockCharacterSelectMsg.equipped);
			return;
		}
		case 14:
		{
			P2PStageLoadedMsg p2PStageLoadedMsg = new P2PStageLoadedMsg(buffer, bufferSize);
			p2PStageLoadedMsg.DeserializeMsg();
			this.onStageLoaded(p2PStageLoadedMsg.steamID);
			return;
		}
		case 15:
		{
			P2PForfeitMatchMsg p2PForfeitMatchMsg = new P2PForfeitMatchMsg(buffer, bufferSize);
			p2PForfeitMatchMsg.DeserializeMsg();
			this.onForfeitMatch(p2PForfeitMatchMsg.senderSteamID);
			return;
		}
		case 16:
		{
			P2PSendWinnerMsg p2PSendWinnerMsg = new P2PSendWinnerMsg(buffer, bufferSize);
			p2PSendWinnerMsg.DeserializeMsg();
			this.onSendWinner(p2PSendWinnerMsg.reportedWinningTeams);
			return;
		}
		case 17:
		{
			P2PRequestChangePlayerMsg p2PRequestChangePlayerMsg = new P2PRequestChangePlayerMsg(buffer, bufferSize);
			p2PRequestChangePlayerMsg.DeserializeMsg();
			this.onRequestChangePlayer(p2PRequestChangePlayerMsg.userID, p2PRequestChangePlayerMsg.isSpectating, (int)p2PRequestChangePlayerMsg.team);
			return;
		}
		case 18:
		{
			P2PInformScreenChangedMsg p2PInformScreenChangedMsg = new P2PInformScreenChangedMsg(buffer, bufferSize);
			p2PInformScreenChangedMsg.DeserializeMsg();
			this.onInformScreenChanged(p2PInformScreenChangedMsg.userID, p2PInformScreenChangedMsg.screenID);
			return;
		}
		case 19:
		{
			P2PMatchConnectBattleMsg p2PMatchConnectBattleMsg = new P2PMatchConnectBattleMsg(buffer, bufferSize);
			p2PMatchConnectBattleMsg.DeserializeMsg();
			this.onMatchConnectBattle(p2PMatchConnectBattleMsg.MatchID, p2PMatchConnectBattleMsg.hostSteamID, p2PMatchConnectBattleMsg.stages.ToArray(), p2PMatchConnectBattleMsg.characterSelectSeconds, p2PMatchConnectBattleMsg.matchLengthSeconds, p2PMatchConnectBattleMsg.numberOfLives, p2PMatchConnectBattleMsg.assistCount, p2PMatchConnectBattleMsg.players.ToArray(), (LobbyGameMode)p2PMatchConnectBattleMsg.gameMode, p2PMatchConnectBattleMsg.teamAttack);
			return;
		}
		case 20:
		{
			P2PLockCharacterSelectAckMsg p2PLockCharacterSelectAckMsg = new P2PLockCharacterSelectAckMsg(buffer, bufferSize);
			p2PLockCharacterSelectAckMsg.DeserializeMsg();
			this.onCharacterSelectAck(p2PLockCharacterSelectAckMsg.steamID, p2PLockCharacterSelectAckMsg.accepted);
			return;
		}
		case 21:
		{
			P2PMatchDetailsMsg p2PMatchDetailsMsg = new P2PMatchDetailsMsg(buffer, bufferSize);
			p2PMatchDetailsMsg.DeserializeMsg();
			this.onMatchDetails(p2PMatchDetailsMsg.Players.ToArray());
			return;
		}
		case 22:
		{
			P2PMatchCountdownMsg p2PMatchCountdownMsg = new P2PMatchCountdownMsg(buffer, bufferSize);
			p2PMatchCountdownMsg.DeserializeMsg();
			this.onMatchCountdown(p2PMatchCountdownMsg.serverStartTimeMs, p2PMatchCountdownMsg.countDownSeconds);
			return;
		}
		case 23:
		{
			P2PMatchResultsMsg p2PMatchResultsMsg = new P2PMatchResultsMsg(buffer, bufferSize);
			p2PMatchResultsMsg.DeserializeMsg();
			this.onMatchResults(p2PMatchResultsMsg.reason, p2PMatchResultsMsg.winningTeamBitMask);
			return;
		}
		case 24:
		{
			P2PRequestDesyncMsg p2PRequestDesyncMsg = new P2PRequestDesyncMsg(buffer, bufferSize);
			p2PRequestDesyncMsg.DeserializeMsg();
			this.DoBroadcast(new DesyncEvent((int)p2PRequestDesyncMsg.desyncFrame));
			return;
		}
		case 25:
		{
			P2PChangePlayerConfirmedMsg p2PChangePlayerConfirmedMsg = new P2PChangePlayerConfirmedMsg(buffer, bufferSize);
			p2PChangePlayerConfirmedMsg.DeserializeMsg();
			this.onPlayerChanged(p2PChangePlayerConfirmedMsg.userID, p2PChangePlayerConfirmedMsg.isSpectating, (int)p2PChangePlayerConfirmedMsg.team);
			return;
		}
		case 26:
		{
			P2PConfirmScreenChangedMsg p2PConfirmScreenChangedMsg = new P2PConfirmScreenChangedMsg(buffer, bufferSize);
			p2PConfirmScreenChangedMsg.DeserializeMsg();
			this.onConfirmScreenChanged(p2PConfirmScreenChangedMsg.userID, p2PConfirmScreenChangedMsg.screenID);
			return;
		}
		case 27:
		{
			P2PSyncLobbyDataMsg p2PSyncLobbyDataMsg = new P2PSyncLobbyDataMsg(buffer, bufferSize);
			p2PSyncLobbyDataMsg.DeserializeMsg();
			this.onSyncLobbyData(p2PSyncLobbyDataMsg.isInMatch);
			return;
		}
		}
		UnityEngine.Debug.LogError(this.ToString() + " Unhandled Game MsgId:" + msgId);
	}

	private void onInput(InputMsg inEvent, CSteamID senderSteamID)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		if (inEvent.numInputs == 0)
		{
			throw new Exception("inputMessageInputs");
		}
		InputEvent bufferedServerEvent = this.bufferedBattleMsgs.GetBufferedServerEvent<InputEvent>();
		bufferedServerEvent.LoadFrom(inEvent);
		this.DoBroadcast(bufferedServerEvent);
	}

	private void onHashCode(HashCodeMsg hashMsg)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		this.p2pHost.OnHashCode((int)hashMsg.senderId, (int)hashMsg.frame, (int)hashMsg.hashCode);
	}

	private void onRequestMissingInput(RequestMissingInputMsg inEvent)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		RequestMissingInputEvent bufferedServerEvent = this.bufferedBattleMsgs.GetBufferedServerEvent<RequestMissingInputEvent>();
		bufferedServerEvent.LoadFrom(inEvent);
		this.DoBroadcast(bufferedServerEvent);
	}

	private void onMatchConnectBattle(Guid matchID, ulong hostSteamID, EIconStages[] stages, uint characterSelectSeconds, uint matchLengthSeconds, uint numberOfLives, uint assistCount, SP2PMatchBasicPlayerDesc[] players, LobbyGameMode gameMode, ETeamAttack teamAttack)
	{
		UnityEngine.Debug.Log("Match Connect Battle.NumMatches:" + stages.Length);
		this.p2pClient.OnMatchConnect(matchID, hostSteamID, stages, characterSelectSeconds, matchLengthSeconds, numberOfLives, assistCount, players, gameMode, teamAttack);
	}

	private void onCharacterSelectAck(ulong steamID, bool accepted)
	{
		this.p2pClient.OnCharacterSelectAck(steamID, accepted);
	}

	private void onMatchDetails(P2PMatchDetailsMsg.SPlayerDesc[] players)
	{
		this.p2pClient.OnMatchDetails(players);
	}

	private void onMatchCountdown(ulong serverStartTimeMs, uint countdownSeconds)
	{
		this.p2pClient.OnMatchCountdown(serverStartTimeMs, countdownSeconds);
	}

	private void onMatchResults(EMatchResult reason, byte winningTeamBitMask)
	{
		this.p2pClient.OnMatchResults(reason, winningTeamBitMask);
	}

	private void onLockInCharacter(ulong steamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equipped)
	{
		if (this.IsHost)
		{
			this.p2pHost.LockInSelection(steamID, characterID, characterIndex, skinID, isRandom, equipped);
		}
		else
		{
			UnityEngine.Debug.LogError("Lock character received on non host");
		}
	}

	private void onStageLoaded(ulong steamID)
	{
		if (this.IsHost)
		{
			this.p2pHost.StageLoaded(steamID);
		}
		else
		{
			UnityEngine.Debug.LogError("Stage Loaded received on non host");
		}
	}

	public void SendWinner(byte reportedWinningTeamMask)
	{
		if (this.IsHost)
		{
			this.p2pHost.ReceiveWinner(reportedWinningTeamMask);
		}
		else
		{
			this.p2pClient.SendWinner(reportedWinningTeamMask);
		}
	}

	private void onSendWinner(byte reportedWinningTeamMask)
	{
		if (this.IsHost)
		{
			this.p2pHost.ReceiveWinner(reportedWinningTeamMask);
		}
		else
		{
			UnityEngine.Debug.LogError("Send Winner received on non host");
		}
	}

	private void onForfeitMatch(ulong senderSteamID)
	{
		if (this.IsHost)
		{
			this.p2pHost.OnPlayerAbandonMatch(senderSteamID);
		}
		else
		{
			UnityEngine.Debug.LogError("Stage Loaded received on non host");
		}
	}

	private void onRequestChangePlayer(ulong userID, bool isSpectating, int team)
	{
		if (this.IsHost)
		{
			this.customLobbyController.HostChangePlayer(userID, isSpectating, team);
		}
	}

	private void onInformScreenChanged(ulong userID, int screenID)
	{
		if (this.IsHost)
		{
			this.customLobbyController.HostReceivedScreenChanged(userID, screenID);
		}
	}

	private void onConfirmScreenChanged(ulong userID, int screenID)
	{
		this.customLobbyController.ReceivedScreenChanged(userID, screenID);
	}

	private void onPlayerChanged(ulong userID, bool isSpectating, int team)
	{
		this.customLobbyController.ReceivedPlayerChange(userID, isSpectating, team);
	}

	private void onSyncLobbyData(bool isInMatch)
	{
		this.customLobbyController.ReceivedIsLobbyInMatch(isInMatch);
	}

	private void onDisconnect(DisconnectMsg msg)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		DisconnectEvent bufferedServerEvent = this.bufferedBattleMsgs.GetBufferedServerEvent<DisconnectEvent>();
		bufferedServerEvent.LoadFrom(msg);
		this.DoBroadcast(bufferedServerEvent);
	}

	private void onDisconnectAck(DisconnectAckMsg msg)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		DisconnectAckEvent bufferedServerEvent = this.bufferedBattleMsgs.GetBufferedServerEvent<DisconnectAckEvent>();
		bufferedServerEvent.LoadFrom(msg);
		this.DoBroadcast(bufferedServerEvent);
	}

	private void onInputAck(InputAckMsg ackMsg, CSteamID senderSteamID)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		InputAckEvent bufferedServerEvent = this.bufferedBattleMsgs.GetBufferedServerEvent<InputAckEvent>();
		bufferedServerEvent.LoadFrom(ackMsg);
		this.DoBroadcast(bufferedServerEvent);
	}

	public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		if (typeof(T).IsSubclassOf(typeof(BatchEvent)))
		{
			UnityEngine.Debug.LogError("Use the 'Listen'. Not ListenForServerEvents because the battleservercontroller will handle it specially");
		}
		if (!this.m_messageHandlers.ContainsKey(typeFromHandle))
		{
			this.m_messageHandlers.Add(typeFromHandle, null);
		}
		Dictionary<Type, Action<ServerEvent>> messageHandlers;
		Type key;
		(messageHandlers = this.m_messageHandlers)[key = typeFromHandle] = (Action<ServerEvent>)Delegate.Combine(messageHandlers[key], callback);
	}

	public void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		if (!this.m_messageHandlers.ContainsKey(typeFromHandle))
		{
			return;
		}
		Dictionary<Type, Action<ServerEvent>> messageHandlers;
		Type key;
		(messageHandlers = this.m_messageHandlers)[key = typeFromHandle] = (Action<ServerEvent>)Delegate.Remove(messageHandlers[key], callback);
	}

	private void onReceiveRelay(RelayMsg relayMsg)
	{
		if (relayMsg.GetBroadcastMode() == BroadcastType.ToHost)
		{
			RelayMsg relayMsg2 = new RelayMsg();
			relayMsg2.byteSize = (uint)relayMsg.bytes.Length;
			relayMsg2.bytes = relayMsg.bytes;
			relayMsg2.SetBroadcastMode(BroadcastType.ToClient);
			this.EnqueueMessage(relayMsg2);
		}
		else
		{
			this.DoBroadcast(new RelayDataEvent
			{
				matchId = this.p2pClient.MatchID,
				bytes = relayMsg.bytes
			});
		}
	}

	public void DoBroadcast(ServerEvent message)
	{
		Type key = message.GetType();
		if (message is BatchEvent)
		{
			key = typeof(BatchEvent);
		}
		if (this.m_messageHandlers.ContainsKey(key) && this.m_messageHandlers[key] != null)
		{
			this.m_messageHandlers[key](message);
		}
	}

	private void packageMessages(List<INetMsg> messages, bool useTCP, ulong targetUser = 0uL)
	{
		BatchMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<BatchMsg>();
		bufferedNetMessage.ResetBuffer();
		int num = 0;
		uint num2 = 0u;
		int count = messages.Count;
		int i = 0;
		while (i < count)
		{
			INetMsg netMsg = messages[i];
			byte msgId = (byte)netMsg.MsgID();
			if (netMsg.MsgSize == 0u)
			{
				netMsg.Serialize();
			}
			if ((ulong)(netMsg.MsgSize + num2) >= (ulong)((long)P2PServerMgr.MAX_BATCH_MESSAGE_BYTES))
			{
				break;
			}
			bufferedNetMessage.m_batchedMessages[bufferedNetMessage.numBatchedMessages].CopyTo(msgId, netMsg.MsgSize, netMsg.MsgBuffer);
			if (!this.IsHost)
			{
				goto IL_FE;
			}
			IP2PMessage iP2PMessage = netMsg as IP2PMessage;
			BroadcastType broadcastMode = iP2PMessage.GetBroadcastMode();
			if (broadcastMode == BroadcastType.Relay)
			{
				goto IL_FE;
			}
			byte[] array = this.sendBuffers[(int)((UIntPtr)(netMsg.MsgSize - 1u))];
			Buffer.BlockCopy(netMsg.MsgBuffer, 1, array, 0, (int)(netMsg.MsgSize - 1u));
			this.OnBattleGameMsg(msgId, array, (uint)array.Length, this.steam.MySteamID());
			if (broadcastMode != BroadcastType.ToHost)
			{
				goto IL_FE;
			}
			num++;
			IL_13B:
			i++;
			continue;
			IL_FE:
			bufferedNetMessage.numBatchedMessages++;
			num2 += netMsg.MsgSize;
			num++;
			if ((long)bufferedNetMessage.numBatchedMessages >= (long)((ulong)BatchMsg.maxBatchMsgs))
			{
				break;
			}
			goto IL_13B;
		}
		bufferedNetMessage.Serialize();
		byte[] array2 = this.sendBuffers[(int)((UIntPtr)(bufferedNetMessage.MsgSize - 1u))];
		Buffer.BlockCopy(bufferedNetMessage.MsgBuffer, 1, array2, 0, (int)(bufferedNetMessage.MsgSize - 1u));
		if (targetUser == 0uL)
		{
			foreach (CSteamID current in this.communicationIdBuffer)
			{
				this.steam.Send(current, array2, (uint)array2.Length, useTCP);
			}
		}
		else
		{
			CSteamID steamIDRemote;
			if (!this.userIDToSteamID.TryGetValue(targetUser, out steamIDRemote))
			{
				throw new UnityException("INVALID USER ID " + targetUser);
			}
			this.steam.Send(steamIDRemote, array2, (uint)array2.Length, useTCP);
		}
		messages.RemoveRange(0, num);
	}

	private void cacheCommunicationList()
	{
		this.communicationIdBuffer.Clear();
		this.userIDToSteamID.Clear();
		if (!this.m_queuedUDPMessages.ContainsKey(0uL))
		{
			this.m_queuedUDPMessages[0uL] = new List<INetMsg>(8);
		}
		if (!this.m_queuedTCPMessages.ContainsKey(0uL))
		{
			this.m_queuedTCPMessages[0uL] = new List<INetMsg>(8);
		}
		foreach (SteamManager.SteamLobbyData current in this.m_receivers)
		{
			ulong steamID = current.steamID.m_SteamID;
			this.userIDToSteamID[steamID] = current.steamID;
			if (!this.m_queuedUDPMessages.ContainsKey(steamID))
			{
				this.m_queuedUDPMessages[steamID] = new List<INetMsg>(8);
			}
			if (!this.m_queuedTCPMessages.ContainsKey(steamID))
			{
				this.m_queuedTCPMessages[steamID] = new List<INetMsg>(8);
			}
			if (this.IsHost || current.isHost)
			{
				if (!current.isSelf)
				{
					this.communicationIdBuffer.Add(current.steamID);
				}
			}
		}
		List<ulong> list = new List<ulong>();
		foreach (ulong current2 in this.m_queuedUDPMessages.Keys)
		{
			if (current2 != 0uL && !this.userIDToSteamID.ContainsKey(current2))
			{
				list.Add(current2);
			}
		}
		foreach (ulong current3 in list)
		{
			this.m_queuedUDPMessages.Remove(current3);
			this.m_queuedTCPMessages.Remove(current3);
		}
	}

	public void SendNetworkMessages()
	{
		this.pingManager.Tick();
		this.timeSynchronizer.Tick();
		this.udpWarming.Tick();
		if (!this.IsHost && this.p2pClient.InMatch && this.p2pClient.HasDisconnected())
		{
			this.p2pClient.OnDisconnected();
			this.p2pClient.OnLeftLobby(false);
			return;
		}
		if (this.m_queuedTCPMessages.Count > 0)
		{
			foreach (KeyValuePair<ulong, List<INetMsg>> current in this.m_queuedTCPMessages)
			{
				ulong key = current.Key;
				List<INetMsg> value = current.Value;
				if (value.Count > 0)
				{
					this.packageMessages(value, true, key);
				}
			}
		}
		if (this.m_queuedUDPMessages.Count > 0)
		{
			foreach (KeyValuePair<ulong, List<INetMsg>> current2 in this.m_queuedUDPMessages)
			{
				ulong key2 = current2.Key;
				List<INetMsg> value2 = current2.Value;
				if (value2.Count > 0)
				{
					this.packageMessages(value2, false, key2);
				}
			}
		}
	}

	public float CurrentUDPDropRate()
	{
		return 0f;
	}

	public float CurrentUDPOOORate()
	{
		return 0f;
	}

	public float CurrentAverageReceivedPacketSize()
	{
		return 0f;
	}

	public float CurrentAverageSentPacketSize()
	{
		return 0f;
	}

	public float CurrentSentPacketRate()
	{
		return 0f;
	}

	public float CurrentReceivedPacketRate()
	{
		return 0f;
	}

	public float CurrentReceivedUdpPacketRate()
	{
		return 0f;
	}

	public UdpMatchStats GetUDPMatchStats()
	{
		return default(UdpMatchStats);
	}

	public void StartCustomMatch(StageID selectedStage, Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
	{
		if (!this.IsHost)
		{
			UnityEngine.Debug.LogError("Non host trying to start a match");
			return;
		}
		this.p2pHost.StartCustomMatch(this.m_receivers, players, selectedStage, gameMode);
	}

	public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
	{
		List<ulong> list = new List<ulong>();
		CharacterID characterIDFromCharacterType = PlayerUtil.GetCharacterIDFromCharacterType(characterID);
		int bestPortId = this.userInputManager.GetBestPortId(PlayerNum.Player1);
		list.Add(0uL);
		list.Add((ulong)this.characterEquipModel.GetEquippedByType(EquipmentTypes.PLATFORM, characterIDFromCharacterType, bestPortId).id);
		list.Add((ulong)this.characterEquipModel.GetEquippedByType(EquipmentTypes.VICTORY_POSE, characterIDFromCharacterType, bestPortId).id);
		Dictionary<TauntSlot, EquipmentID> slotsForCharacter = this.tauntModel.GetSlotsForCharacter(characterIDFromCharacterType, bestPortId);
		for (int i = 0; i < 4; i++)
		{
			list.Add((ulong)((slotsForCharacter == null || !slotsForCharacter.ContainsKey((TauntSlot)i) || slotsForCharacter[(TauntSlot)i].IsNull()) ? 0L : slotsForCharacter[(TauntSlot)i].id));
		}
		if (this.IsHost)
		{
			return this.p2pHost.LockInSelection(this.steam.MySteamID().m_SteamID, characterID, characterIndex, skinID, isRandom, list.ToArray());
		}
		return this.p2pClient.LockInSelection(this.steam.MySteamID(), characterID, characterIndex, skinID, isRandom, list.ToArray());
	}

	public bool StageLoaded()
	{
		if (this.IsHost)
		{
			return this.p2pHost.StageLoaded(this.steam.MySteamID().m_SteamID);
		}
		return this.p2pClient.StageLoaded(this.steam.MySteamID());
	}

	public bool LeaveMatch()
	{
		if (this.IsHost)
		{
			return this.p2pHost.OnPlayerAbandonMatch(this.steam.MySteamID().m_SteamID);
		}
		return this.p2pClient.LeaveMatch();
	}

	public void SetParams(SCustomLobbyParams cmparams)
	{
		this.steam.SetLobbyValue(SteamManager.LobbyNameKey, cmparams.lobbyName);
		this.steam.SetLobbyValue(SteamManager.LobbyStageKey, cmparams.stages[0].ToString());
		this.steam.SetLobbyValue(SteamManager.LobbyGameModeKey, (byte)cmparams.gameMode + string.Empty);
	}

	public void UpdateLobbyists(bool forceDisconnect = false)
	{
		this.p2pClient.UpdateLobbyists(this.m_receivers, forceDisconnect);
	}

	public void OnLeftLobby()
	{
		this.timeSynchronizer.Reset();
		this.udpWarming.Reset();
		this.p2pClient.OnLeftLobby(false);
	}
}
