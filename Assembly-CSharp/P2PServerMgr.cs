using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using P2P;
using Steamworks;
using UnityEngine;

// Token: 0x02000831 RID: 2097
public class P2PServerMgr
{
	// Token: 0x060033D7 RID: 13271 RVA: 0x000F5994 File Offset: 0x000F3D94
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

	// Token: 0x17000C79 RID: 3193
	// (get) Token: 0x060033D8 RID: 13272 RVA: 0x000F5A3B File Offset: 0x000F3E3B
	// (set) Token: 0x060033D9 RID: 13273 RVA: 0x000F5A43 File Offset: 0x000F3E43
	[Inject]
	public SteamManager steam { get; set; }

	// Token: 0x17000C7A RID: 3194
	// (get) Token: 0x060033DA RID: 13274 RVA: 0x000F5A4C File Offset: 0x000F3E4C
	// (set) Token: 0x060033DB RID: 13275 RVA: 0x000F5A54 File Offset: 0x000F3E54
	[Inject]
	public P2PHost p2pHost { get; set; }

	// Token: 0x17000C7B RID: 3195
	// (get) Token: 0x060033DC RID: 13276 RVA: 0x000F5A5D File Offset: 0x000F3E5D
	// (set) Token: 0x060033DD RID: 13277 RVA: 0x000F5A65 File Offset: 0x000F3E65
	[Inject]
	public P2PClient p2pClient { get; set; }

	// Token: 0x17000C7C RID: 3196
	// (get) Token: 0x060033DE RID: 13278 RVA: 0x000F5A6E File Offset: 0x000F3E6E
	// (set) Token: 0x060033DF RID: 13279 RVA: 0x000F5A76 File Offset: 0x000F3E76
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000C7D RID: 3197
	// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000F5A7F File Offset: 0x000F3E7F
	// (set) Token: 0x060033E1 RID: 13281 RVA: 0x000F5A87 File Offset: 0x000F3E87
	[Inject]
	public IUserTauntsModel tauntModel { get; set; }

	// Token: 0x17000C7E RID: 3198
	// (get) Token: 0x060033E2 RID: 13282 RVA: 0x000F5A90 File Offset: 0x000F3E90
	// (set) Token: 0x060033E3 RID: 13283 RVA: 0x000F5A98 File Offset: 0x000F3E98
	[Inject]
	public IUserCharacterEquippedModel characterEquipModel { get; set; }

	// Token: 0x17000C7F RID: 3199
	// (get) Token: 0x060033E4 RID: 13284 RVA: 0x000F5AA1 File Offset: 0x000F3EA1
	// (set) Token: 0x060033E5 RID: 13285 RVA: 0x000F5AA9 File Offset: 0x000F3EA9
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x060033E6 RID: 13286 RVA: 0x000F5AB2 File Offset: 0x000F3EB2
	// (set) Token: 0x060033E7 RID: 13287 RVA: 0x000F5ABA File Offset: 0x000F3EBA
	[Inject]
	public IPingManager pingManager { private get; set; }

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x060033E8 RID: 13288 RVA: 0x000F5AC3 File Offset: 0x000F3EC3
	// (set) Token: 0x060033E9 RID: 13289 RVA: 0x000F5ACB File Offset: 0x000F3ECB
	[Inject]
	public ITimeSynchronizer timeSynchronizer { private get; set; }

	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x060033EA RID: 13290 RVA: 0x000F5AD4 File Offset: 0x000F3ED4
	// (set) Token: 0x060033EB RID: 13291 RVA: 0x000F5ADC File Offset: 0x000F3EDC
	[Inject]
	public IUDPWarming udpWarming { private get; set; }

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x060033EC RID: 13292 RVA: 0x000F5AE5 File Offset: 0x000F3EE5
	// (set) Token: 0x060033ED RID: 13293 RVA: 0x000F5AED File Offset: 0x000F3EED
	[Inject]
	public ICustomLobbyController customLobbyController { private get; set; }

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x060033EE RID: 13294 RVA: 0x000F5AF6 File Offset: 0x000F3EF6
	// (set) Token: 0x060033EF RID: 13295 RVA: 0x000F5AFE File Offset: 0x000F3EFE
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x060033F0 RID: 13296 RVA: 0x000F5B07 File Offset: 0x000F3F07
	// (set) Token: 0x060033F1 RID: 13297 RVA: 0x000F5B0F File Offset: 0x000F3F0F
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x060033F2 RID: 13298 RVA: 0x000F5B18 File Offset: 0x000F3F18
	public ulong SessionId
	{
		get
		{
			return 0UL;
		}
	}

	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x060033F3 RID: 13299 RVA: 0x000F5B1C File Offset: 0x000F3F1C
	public ulong AccountId
	{
		get
		{
			return 0UL;
		}
	}

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x060033F4 RID: 13300 RVA: 0x000F5B20 File Offset: 0x000F3F20
	public string Username
	{
		get
		{
			return (!this.steam.Initialized) ? string.Empty : this.steam.UserName;
		}
	}

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x060033F5 RID: 13301 RVA: 0x000F5B47 File Offset: 0x000F3F47
	// (set) Token: 0x060033F6 RID: 13302 RVA: 0x000F5B4F File Offset: 0x000F3F4F
	public List<CSteamID> communicationIdBuffer { get; private set; }

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x060033F7 RID: 13303 RVA: 0x000F5B58 File Offset: 0x000F3F58
	// (set) Token: 0x060033F8 RID: 13304 RVA: 0x000F5B60 File Offset: 0x000F3F60
	public long LastMessageReceivedMs { get; private set; }

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x060033F9 RID: 13305 RVA: 0x000F5B69 File Offset: 0x000F3F69
	public bool IsHost
	{
		get
		{
			return this.steam.IsHost;
		}
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x000F5B76 File Offset: 0x000F3F76
	public void Init()
	{
		this.recievedMessageBuffer = new byte[10000];
		this.batchMsgBuffer.buffer = new byte[P2PServerMgr.MAX_BATCH_PER_MESSAGE_BYTES];
		this.bufferedBattleMsgs.Init();
	}

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x060033FB RID: 13307 RVA: 0x000F5BA8 File Offset: 0x000F3FA8
	public bool IsInBattle
	{
		get
		{
			return this.gameController.currentGame != null && this.gameController.currentGame.FrameController != null && this.gameController.currentGame.FrameController.IsBattleReady;
		}
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x000F5C00 File Offset: 0x000F4000
	private bool isRequiredUserMissing()
	{
		List<ulong> list = new List<ulong>();
		foreach (LobbyPlayerData lobbyPlayerData in this.customLobbyController.Players.Values)
		{
			if (!lobbyPlayerData.isSpectator)
			{
				list.Add(lobbyPlayerData.userID);
			}
		}
		List<ulong> list2 = new List<ulong>();
		foreach (SteamManager.SteamLobbyData steamLobbyData in this.steam.GetReceivers())
		{
			list2.Add(steamLobbyData.steamID.m_SteamID);
		}
		foreach (ulong item in list)
		{
			if (!list2.Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x000F5D3C File Offset: 0x000F413C
	private List<ulong> getMissingUsers()
	{
		List<ulong> list = new List<ulong>();
		List<ulong> list2 = new List<ulong>();
		foreach (SteamManager.SteamLobbyData steamLobbyData in this.steam.GetReceivers())
		{
			list2.Add(steamLobbyData.steamID.m_SteamID);
		}
		foreach (LobbyPlayerData lobbyPlayerData in this.customLobbyController.Players.Values)
		{
			if (!list2.Contains(lobbyPlayerData.userID))
			{
				list.Add(lobbyPlayerData.userID);
			}
		}
		return list;
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x000F5E24 File Offset: 0x000F4224
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

	// Token: 0x060033FF RID: 13311 RVA: 0x000F5F10 File Offset: 0x000F4310
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

	// Token: 0x06003400 RID: 13312 RVA: 0x000F5F50 File Offset: 0x000F4350
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

	// Token: 0x06003401 RID: 13313 RVA: 0x000F5FD4 File Offset: 0x000F43D4
	public bool EnqueueMessage(INetMsg queuedMessage)
	{
		if (!(queuedMessage is IP2PMessage))
		{
			throw new Exception("Non p2p message" + queuedMessage);
		}
		ulong key = 0UL;
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

	// Token: 0x06003402 RID: 13314 RVA: 0x000F605C File Offset: 0x000F445C
	public void ClearNetworkMessages()
	{
		foreach (List<INetMsg> list in this.m_queuedUDPMessages.Values)
		{
			list.Clear();
		}
		foreach (List<INetMsg> list2 in this.m_queuedTCPMessages.Values)
		{
			list2.Clear();
		}
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x000F610C File Offset: 0x000F450C
	private void debugBytes(byte[] buffer, int size)
	{
		string text = string.Empty;
		for (int i = 0; i < size; i++)
		{
			text += buffer[i].ToString();
		}
		Debug.LogError("Debug bytes: " + text);
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x000F615C File Offset: 0x000F455C
	public void ReceiveNetworkMessages()
	{
		uint msgSize = 0U;
		CSteamID nil = CSteamID.Nil;
		while (this.steam.GetMessage(ref this.recievedMessageBuffer, ref msgSize, ref nil))
		{
			if (this.steam.CurrentSteamLobbyId != 0UL)
			{
				BatchMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<BatchMsg>();
				bufferedNetMessage.DeserializeToBuffer(this.recievedMessageBuffer, msgSize);
				this.onReceiveBatch(bufferedNetMessage, nil);
			}
		}
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x000F61C8 File Offset: 0x000F45C8
	private void onReceiveBatch(BatchMsg batchedMsg, CSteamID senderSteamID)
	{
		int i = 0;
		while (i < batchedMsg.numBatchedMessages)
		{
			BatchMsg.BatchedMsg batchedMsg2 = batchedMsg.m_batchedMessages[i];
			this.batchMsgBuffer.msgId = batchedMsg2.msgId;
			this.batchMsgBuffer.bufferSize = batchedMsg2.msgSize - 1U;
			if ((ulong)this.batchMsgBuffer.bufferSize > (ulong)((long)batchedMsg2.msgBuffer.Length))
			{
				if (batchedMsg2.msgSize == 0U)
				{
					Debug.LogError(string.Concat(new object[]
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
				Debug.LogError(string.Concat(new object[]
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
					Debug.LogError(string.Concat(new object[]
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

	// Token: 0x06003406 RID: 13318 RVA: 0x000F63FC File Offset: 0x000F47FC
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
		Debug.LogError(this.ToString() + " Unhandled Game MsgId:" + msgId);
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x000F6830 File Offset: 0x000F4C30
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

	// Token: 0x06003408 RID: 13320 RVA: 0x000F687E File Offset: 0x000F4C7E
	private void onHashCode(HashCodeMsg hashMsg)
	{
		if (!this.p2pClient.InMatch)
		{
			return;
		}
		this.p2pHost.OnHashCode((int)hashMsg.senderId, (int)hashMsg.frame, (int)hashMsg.hashCode);
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x000F68B0 File Offset: 0x000F4CB0
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

	// Token: 0x0600340A RID: 13322 RVA: 0x000F68E8 File Offset: 0x000F4CE8
	private void onMatchConnectBattle(Guid matchID, ulong hostSteamID, EIconStages[] stages, uint characterSelectSeconds, uint matchLengthSeconds, uint numberOfLives, uint assistCount, SP2PMatchBasicPlayerDesc[] players, LobbyGameMode gameMode, ETeamAttack teamAttack)
	{
		Debug.Log("Match Connect Battle.NumMatches:" + stages.Length);
		this.p2pClient.OnMatchConnect(matchID, hostSteamID, stages, characterSelectSeconds, matchLengthSeconds, numberOfLives, assistCount, players, gameMode, teamAttack);
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x000F6928 File Offset: 0x000F4D28
	private void onCharacterSelectAck(ulong steamID, bool accepted)
	{
		this.p2pClient.OnCharacterSelectAck(steamID, accepted);
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x000F6937 File Offset: 0x000F4D37
	private void onMatchDetails(P2PMatchDetailsMsg.SPlayerDesc[] players)
	{
		this.p2pClient.OnMatchDetails(players);
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x000F6945 File Offset: 0x000F4D45
	private void onMatchCountdown(ulong serverStartTimeMs, uint countdownSeconds)
	{
		this.p2pClient.OnMatchCountdown(serverStartTimeMs, countdownSeconds);
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x000F6954 File Offset: 0x000F4D54
	private void onMatchResults(EMatchResult reason, byte winningTeamBitMask)
	{
		this.p2pClient.OnMatchResults(reason, winningTeamBitMask);
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x000F6963 File Offset: 0x000F4D63
	private void onLockInCharacter(ulong steamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equipped)
	{
		if (this.IsHost)
		{
			this.p2pHost.LockInSelection(steamID, characterID, characterIndex, skinID, isRandom, equipped);
		}
		else
		{
			Debug.LogError("Lock character received on non host");
		}
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x000F6994 File Offset: 0x000F4D94
	private void onStageLoaded(ulong steamID)
	{
		if (this.IsHost)
		{
			this.p2pHost.StageLoaded(steamID);
		}
		else
		{
			Debug.LogError("Stage Loaded received on non host");
		}
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x000F69BD File Offset: 0x000F4DBD
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

	// Token: 0x06003412 RID: 13330 RVA: 0x000F69E7 File Offset: 0x000F4DE7
	private void onSendWinner(byte reportedWinningTeamMask)
	{
		if (this.IsHost)
		{
			this.p2pHost.ReceiveWinner(reportedWinningTeamMask);
		}
		else
		{
			Debug.LogError("Send Winner received on non host");
		}
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x000F6A0F File Offset: 0x000F4E0F
	private void onForfeitMatch(ulong senderSteamID)
	{
		if (this.IsHost)
		{
			this.p2pHost.OnPlayerAbandonMatch(senderSteamID);
		}
		else
		{
			Debug.LogError("Stage Loaded received on non host");
		}
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x000F6A38 File Offset: 0x000F4E38
	private void onRequestChangePlayer(ulong userID, bool isSpectating, int team)
	{
		if (this.IsHost)
		{
			this.customLobbyController.HostChangePlayer(userID, isSpectating, team);
		}
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000F6A53 File Offset: 0x000F4E53
	private void onInformScreenChanged(ulong userID, int screenID)
	{
		if (this.IsHost)
		{
			this.customLobbyController.HostReceivedScreenChanged(userID, screenID);
		}
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x000F6A6D File Offset: 0x000F4E6D
	private void onConfirmScreenChanged(ulong userID, int screenID)
	{
		this.customLobbyController.ReceivedScreenChanged(userID, screenID);
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000F6A7C File Offset: 0x000F4E7C
	private void onPlayerChanged(ulong userID, bool isSpectating, int team)
	{
		this.customLobbyController.ReceivedPlayerChange(userID, isSpectating, team);
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x000F6A8C File Offset: 0x000F4E8C
	private void onSyncLobbyData(bool isInMatch)
	{
		this.customLobbyController.ReceivedIsLobbyInMatch(isInMatch);
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000F6A9C File Offset: 0x000F4E9C
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

	// Token: 0x0600341A RID: 13338 RVA: 0x000F6AD4 File Offset: 0x000F4ED4
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

	// Token: 0x0600341B RID: 13339 RVA: 0x000F6B0C File Offset: 0x000F4F0C
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

	// Token: 0x0600341C RID: 13340 RVA: 0x000F6B44 File Offset: 0x000F4F44
	public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
	{
		Type typeFromHandle = typeof(T);
		if (typeof(T).IsSubclassOf(typeof(BatchEvent)))
		{
			Debug.LogError("Use the 'Listen'. Not ListenForServerEvents because the battleservercontroller will handle it specially");
		}
		if (!this.m_messageHandlers.ContainsKey(typeFromHandle))
		{
			this.m_messageHandlers.Add(typeFromHandle, null);
		}
		Dictionary<Type, Action<ServerEvent>> messageHandlers;
		Type key;
		(messageHandlers = this.m_messageHandlers)[key = typeFromHandle] = (Action<ServerEvent>)Delegate.Combine(messageHandlers[key], callback);
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000F6BC4 File Offset: 0x000F4FC4
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

	// Token: 0x0600341E RID: 13342 RVA: 0x000F6C10 File Offset: 0x000F5010
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

	// Token: 0x0600341F RID: 13343 RVA: 0x000F6C88 File Offset: 0x000F5088
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

	// Token: 0x06003420 RID: 13344 RVA: 0x000F6CE8 File Offset: 0x000F50E8
	private void packageMessages(List<INetMsg> messages, bool useTCP, ulong targetUser = 0UL)
	{
		BatchMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<BatchMsg>();
		bufferedNetMessage.ResetBuffer();
		int num = 0;
		uint num2 = 0U;
		int count = messages.Count;
		int i = 0;
		while (i < count)
		{
			INetMsg netMsg = messages[i];
			byte msgId = (byte)netMsg.MsgID();
			if (netMsg.MsgSize == 0U)
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
			IP2PMessage ip2PMessage = netMsg as IP2PMessage;
			BroadcastType broadcastMode = ip2PMessage.GetBroadcastMode();
			if (broadcastMode == BroadcastType.Relay)
			{
				goto IL_FE;
			}
			byte[] array = this.sendBuffers[(int)((UIntPtr)(netMsg.MsgSize - 1U))];
			Buffer.BlockCopy(netMsg.MsgBuffer, 1, array, 0, (int)(netMsg.MsgSize - 1U));
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
		byte[] array2 = this.sendBuffers[(int)((UIntPtr)(bufferedNetMessage.MsgSize - 1U))];
		Buffer.BlockCopy(bufferedNetMessage.MsgBuffer, 1, array2, 0, (int)(bufferedNetMessage.MsgSize - 1U));
		if (targetUser == 0UL)
		{
			foreach (CSteamID steamIDRemote in this.communicationIdBuffer)
			{
				this.steam.Send(steamIDRemote, array2, (uint)array2.Length, useTCP);
			}
		}
		else
		{
			CSteamID steamIDRemote2;
			if (!this.userIDToSteamID.TryGetValue(targetUser, out steamIDRemote2))
			{
				throw new UnityException("INVALID USER ID " + targetUser);
			}
			this.steam.Send(steamIDRemote2, array2, (uint)array2.Length, useTCP);
		}
		messages.RemoveRange(0, num);
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x000F6F24 File Offset: 0x000F5324
	private void cacheCommunicationList()
	{
		this.communicationIdBuffer.Clear();
		this.userIDToSteamID.Clear();
		if (!this.m_queuedUDPMessages.ContainsKey(0UL))
		{
			this.m_queuedUDPMessages[0UL] = new List<INetMsg>(8);
		}
		if (!this.m_queuedTCPMessages.ContainsKey(0UL))
		{
			this.m_queuedTCPMessages[0UL] = new List<INetMsg>(8);
		}
		foreach (SteamManager.SteamLobbyData steamLobbyData in this.m_receivers)
		{
			ulong steamID = steamLobbyData.steamID.m_SteamID;
			this.userIDToSteamID[steamID] = steamLobbyData.steamID;
			if (!this.m_queuedUDPMessages.ContainsKey(steamID))
			{
				this.m_queuedUDPMessages[steamID] = new List<INetMsg>(8);
			}
			if (!this.m_queuedTCPMessages.ContainsKey(steamID))
			{
				this.m_queuedTCPMessages[steamID] = new List<INetMsg>(8);
			}
			if (this.IsHost || steamLobbyData.isHost)
			{
				if (!steamLobbyData.isSelf)
				{
					this.communicationIdBuffer.Add(steamLobbyData.steamID);
				}
			}
		}
		List<ulong> list = new List<ulong>();
		foreach (ulong num in this.m_queuedUDPMessages.Keys)
		{
			if (num != 0UL && !this.userIDToSteamID.ContainsKey(num))
			{
				list.Add(num);
			}
		}
		foreach (ulong key in list)
		{
			this.m_queuedUDPMessages.Remove(key);
			this.m_queuedTCPMessages.Remove(key);
		}
	}

	// Token: 0x17000C8D RID: 3213
	// (get) Token: 0x06003422 RID: 13346 RVA: 0x000F7150 File Offset: 0x000F5550
	public bool HasReceivers
	{
		get
		{
			return this.m_receivers.Count > 0;
		}
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x000F7160 File Offset: 0x000F5560
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
			foreach (KeyValuePair<ulong, List<INetMsg>> keyValuePair in this.m_queuedTCPMessages)
			{
				ulong key = keyValuePair.Key;
				List<INetMsg> value = keyValuePair.Value;
				if (value.Count > 0)
				{
					this.packageMessages(value, true, key);
				}
			}
		}
		if (this.m_queuedUDPMessages.Count > 0)
		{
			foreach (KeyValuePair<ulong, List<INetMsg>> keyValuePair2 in this.m_queuedUDPMessages)
			{
				ulong key2 = keyValuePair2.Key;
				List<INetMsg> value2 = keyValuePair2.Value;
				if (value2.Count > 0)
				{
					this.packageMessages(value2, false, key2);
				}
			}
		}
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x000F72D0 File Offset: 0x000F56D0
	public float CurrentUDPDropRate()
	{
		return 0f;
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x000F72D7 File Offset: 0x000F56D7
	public float CurrentUDPOOORate()
	{
		return 0f;
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x000F72DE File Offset: 0x000F56DE
	public float CurrentAverageReceivedPacketSize()
	{
		return 0f;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000F72E5 File Offset: 0x000F56E5
	public float CurrentAverageSentPacketSize()
	{
		return 0f;
	}

	// Token: 0x06003428 RID: 13352 RVA: 0x000F72EC File Offset: 0x000F56EC
	public float CurrentSentPacketRate()
	{
		return 0f;
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x000F72F3 File Offset: 0x000F56F3
	public float CurrentReceivedPacketRate()
	{
		return 0f;
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x000F72FA File Offset: 0x000F56FA
	public float CurrentReceivedUdpPacketRate()
	{
		return 0f;
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x000F7304 File Offset: 0x000F5704
	public UdpMatchStats GetUDPMatchStats()
	{
		return default(UdpMatchStats);
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x000F731A File Offset: 0x000F571A
	public void StartCustomMatch(StageID selectedStage, Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
	{
		if (!this.IsHost)
		{
			Debug.LogError("Non host trying to start a match");
			return;
		}
		this.p2pHost.StartCustomMatch(this.m_receivers, players, selectedStage, gameMode);
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x000F7348 File Offset: 0x000F5748
	public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
	{
		List<ulong> list = new List<ulong>();
		CharacterID characterIDFromCharacterType = PlayerUtil.GetCharacterIDFromCharacterType(characterID);
		int bestPortId = this.userInputManager.GetBestPortId(PlayerNum.Player1);
		list.Add(0UL);
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

	// Token: 0x0600342E RID: 13358 RVA: 0x000F7478 File Offset: 0x000F5878
	public bool StageLoaded()
	{
		if (this.IsHost)
		{
			return this.p2pHost.StageLoaded(this.steam.MySteamID().m_SteamID);
		}
		return this.p2pClient.StageLoaded(this.steam.MySteamID());
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x000F74C8 File Offset: 0x000F58C8
	public bool LeaveMatch()
	{
		if (this.IsHost)
		{
			return this.p2pHost.OnPlayerAbandonMatch(this.steam.MySteamID().m_SteamID);
		}
		return this.p2pClient.LeaveMatch();
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x000F750C File Offset: 0x000F590C
	public void SetParams(SCustomLobbyParams cmparams)
	{
		this.steam.SetLobbyValue(SteamManager.LobbyNameKey, cmparams.lobbyName);
		this.steam.SetLobbyValue(SteamManager.LobbyStageKey, cmparams.stages[0].ToString());
		this.steam.SetLobbyValue(SteamManager.LobbyGameModeKey, (byte)cmparams.gameMode + string.Empty);
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x000F757F File Offset: 0x000F597F
	public void UpdateLobbyists(bool forceDisconnect = false)
	{
		this.p2pClient.UpdateLobbyists(this.m_receivers, forceDisconnect);
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x000F7593 File Offset: 0x000F5993
	public void OnLeftLobby()
	{
		this.timeSynchronizer.Reset();
		this.udpWarming.Reset();
		this.p2pClient.OnLeftLobby(false);
	}

	// Token: 0x0400241A RID: 9242
	private static readonly int MAX_BATCH_MESSAGE_BYTES = 1000;

	// Token: 0x04002428 RID: 9256
	private List<SteamManager.SteamLobbyData> m_receivers = new List<SteamManager.SteamLobbyData>();

	// Token: 0x04002429 RID: 9257
	private Dictionary<ulong, CSteamID> userIDToSteamID = new Dictionary<ulong, CSteamID>();

	// Token: 0x0400242A RID: 9258
	private Dictionary<ulong, List<INetMsg>> m_queuedTCPMessages = new Dictionary<ulong, List<INetMsg>>();

	// Token: 0x0400242B RID: 9259
	private Dictionary<ulong, List<INetMsg>> m_queuedUDPMessages = new Dictionary<ulong, List<INetMsg>>();

	// Token: 0x0400242C RID: 9260
	private Dictionary<Type, Action<ServerEvent>> m_messageHandlers = new Dictionary<Type, Action<ServerEvent>>();

	// Token: 0x0400242D RID: 9261
	public BufferedBattleMsgs bufferedBattleMsgs = new BufferedBattleMsgs();

	// Token: 0x0400242E RID: 9262
	private static readonly int MAX_BATCH_PER_MESSAGE_BYTES = 1000;

	// Token: 0x0400242F RID: 9263
	private SMsg batchMsgBuffer = new SMsg();

	// Token: 0x04002430 RID: 9264
	private byte[] recievedMessageBuffer;

	// Token: 0x04002431 RID: 9265
	private byte[][] sendBuffers;
}
