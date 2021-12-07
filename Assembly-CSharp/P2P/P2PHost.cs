using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using UnityEngine;

namespace P2P
{
	// Token: 0x0200080E RID: 2062
	public class P2PHost
	{
		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x000F3B7E File Offset: 0x000F1F7E
		// (set) Token: 0x0600332D RID: 13101 RVA: 0x000F3B86 File Offset: 0x000F1F86
		[Inject]
		public SteamManager steam { get; set; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x000F3B8F File Offset: 0x000F1F8F
		// (set) Token: 0x0600332F RID: 13103 RVA: 0x000F3B97 File Offset: 0x000F1F97
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x000F3BA0 File Offset: 0x000F1FA0
		// (set) Token: 0x06003331 RID: 13105 RVA: 0x000F3BA8 File Offset: 0x000F1FA8
		[Inject]
		public IPingManager pingManager { private get; set; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x06003332 RID: 13106 RVA: 0x000F3BB1 File Offset: 0x000F1FB1
		// (set) Token: 0x06003333 RID: 13107 RVA: 0x000F3BB9 File Offset: 0x000F1FB9
		[Inject]
		public IStageDataHelper stageDataHelper { get; set; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x000F3BC2 File Offset: 0x000F1FC2
		// (set) Token: 0x06003335 RID: 13109 RVA: 0x000F3BCA File Offset: 0x000F1FCA
		[Inject]
		public ITimeSynchronizer timeSynchronizer { private get; set; }

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x000F3BD3 File Offset: 0x000F1FD3
		// (set) Token: 0x06003337 RID: 13111 RVA: 0x000F3BDB File Offset: 0x000F1FDB
		[Inject]
		public IMainThreadTimer timer { private get; set; }

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x000F3BE4 File Offset: 0x000F1FE4
		public bool InMatch
		{
			get
			{
				return this.matchHostData.MatchID != Guid.Empty;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x000F3BFB File Offset: 0x000F1FFB
		// (set) Token: 0x0600333A RID: 13114 RVA: 0x000F3C03 File Offset: 0x000F2003
		public Dictionary<int, RollbackPlayerData> playerDataList { private get; set; }

		// Token: 0x0600333B RID: 13115 RVA: 0x000F3C0C File Offset: 0x000F200C
		[PostConstruct]
		public void Init()
		{
			this.matchHostData.MatchID = Guid.Empty;
			this.matchHostData.battleData = default(P2PHost.SP2PMatchBattleData);
			this.timeSynchronizer.SetUpdateCallback(new Action(this.onTimeSyncComplete));
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x0600333C RID: 13116 RVA: 0x000F3C54 File Offset: 0x000F2054
		public bool IsHost
		{
			get
			{
				return this.p2pServerMgr.IsHost;
			}
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000F3C64 File Offset: 0x000F2064
		public void OnCreateLobby()
		{
			this.matchHostData.MatchID = Guid.Empty;
			this.matchHostData.battleData = default(P2PHost.SP2PMatchBattleData);
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000F3C98 File Offset: 0x000F2098
		public void StartCustomMatch(List<SteamManager.SteamLobbyData> lobbyists, Dictionary<ulong, LobbyPlayerData> players, StageID selectedStage, LobbyGameMode gameMode)
		{
			this.matchHostData.MatchID = Guid.NewGuid();
			this.matchHostData.gameMode = gameMode;
			this.matchHostData.teamAttack = ETeamAttack.Enabled;
			this.matchHostData.matchLengthSeconds = ((gameMode != LobbyGameMode.Time) ? 480U : 180U);
			this.matchHostData.characterSelectSeconds = 60U;
			this.matchHostData.assistCount = 99U;
			this.matchHostData.numLives = 3U;
			this.matchHostData.stages = new List<EIconStages>();
			if (selectedStage == StageID.Random)
			{
				selectedStage = this.stageDataHelper.GetRandomPossibleStage();
			}
			this.matchHostData.stages.Add(this.stageDataHelper.GetIconStageFromStageID(selectedStage));
			this.matchHostData.players = new List<P2PHost.SP2PMatchFullPlayerDesc>();
			this.matchHostData.winningTeamMask = 0;
			this.matchHostData.reportedWinners = 0U;
			this.matchHostData.minLatencyMs = 0L;
			this.matchHostData.battleData.Init();
			List<SP2PMatchBasicPlayerDesc> list = new List<SP2PMatchBasicPlayerDesc>();
			Dictionary<ulong, P2PHost.SP2PMatchFullPlayerDesc> dictionary = new Dictionary<ulong, P2PHost.SP2PMatchFullPlayerDesc>();
			foreach (SteamManager.SteamLobbyData steamLobbyData in lobbyists)
			{
				LobbyPlayerData lobbyPlayerData;
				players.TryGetValue(steamLobbyData.steamID.m_SteamID, out lobbyPlayerData);
				P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc = new P2PHost.SP2PMatchFullPlayerDesc();
				sp2PMatchFullPlayerDesc.Init();
				sp2PMatchFullPlayerDesc.baseData.steamID = steamLobbyData.steamID.m_SteamID;
				sp2PMatchFullPlayerDesc.baseData.name = this.steam.GetUserName(steamLobbyData.steamID);
				sp2PMatchFullPlayerDesc.baseData.userID = steamLobbyData.steamID.m_SteamID;
				sp2PMatchFullPlayerDesc.baseData.team = (byte)lobbyPlayerData.team;
				sp2PMatchFullPlayerDesc.baseData.isSpectator = lobbyPlayerData.isSpectator;
				sp2PMatchFullPlayerDesc.detailData.characterID = ECharacterType.CharacterTypeCount;
				sp2PMatchFullPlayerDesc.battleData.Init();
				list.Add(sp2PMatchFullPlayerDesc.baseData);
				dictionary[sp2PMatchFullPlayerDesc.baseData.userID] = sp2PMatchFullPlayerDesc;
			}
			list.Sort(new Comparison<SP2PMatchBasicPlayerDesc>(this.sortList));
			foreach (SP2PMatchBasicPlayerDesc sp2PMatchBasicPlayerDesc in list)
			{
				this.matchHostData.players.Add(dictionary[sp2PMatchBasicPlayerDesc.userID]);
			}
			P2PMatchConnectBattleMsg p2PMatchConnectBattleMsg = new P2PMatchConnectBattleMsg();
			p2PMatchConnectBattleMsg.MatchID = this.matchHostData.MatchID;
			p2PMatchConnectBattleMsg.hostSteamID = this.steam.MySteamID().m_SteamID;
			p2PMatchConnectBattleMsg.matchLengthSeconds = this.matchHostData.matchLengthSeconds;
			p2PMatchConnectBattleMsg.numberOfLives = this.matchHostData.numLives;
			p2PMatchConnectBattleMsg.assistCount = this.matchHostData.assistCount;
			p2PMatchConnectBattleMsg.characterSelectSeconds = this.matchHostData.characterSelectSeconds;
			p2PMatchConnectBattleMsg.gameMode = (byte)this.matchHostData.gameMode;
			p2PMatchConnectBattleMsg.teamAttack = this.matchHostData.teamAttack;
			p2PMatchConnectBattleMsg.stages = this.matchHostData.stages;
			p2PMatchConnectBattleMsg.players = list;
			this.p2pServerMgr.EnqueueMessage(p2PMatchConnectBattleMsg);
			this.waitingForStageLoad = false;
			this.waitingForLockIns = true;
			this.SyncLobbyData();
			this.pingManager.Ping();
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000F4014 File Offset: 0x000F2414
		private int sortList(SP2PMatchBasicPlayerDesc a, SP2PMatchBasicPlayerDesc b)
		{
			if (a.isSpectator && !b.isSpectator)
			{
				return 1;
			}
			if (b.isSpectator && !a.isSpectator)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000F4047 File Offset: 0x000F2447
		private void onTimeSyncComplete()
		{
			Debug.Log("On time sync complete " + this.p2pServerMgr.IsHost);
			if (this.p2pServerMgr.IsHost)
			{
				this.checkStartLoadingStage();
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000F4080 File Offset: 0x000F2480
		private P2PHost.SP2PMatchFullPlayerDesc getFullPlayerDesc(ulong steamID)
		{
			foreach (P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc in this.matchHostData.players)
			{
				if (sp2PMatchFullPlayerDesc.baseData.steamID == steamID)
				{
					return sp2PMatchFullPlayerDesc;
				}
			}
			return null;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x000F40F8 File Offset: 0x000F24F8
		public bool LockInSelection(ulong senderSteamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equipped)
		{
			P2PLockCharacterSelectAckMsg p2PLockCharacterSelectAckMsg = new P2PLockCharacterSelectAckMsg();
			p2PLockCharacterSelectAckMsg.steamID = senderSteamID;
			p2PLockCharacterSelectAckMsg.accepted = false;
			P2PHost.SP2PMatchFullPlayerDesc fullPlayerDesc = this.getFullPlayerDesc(senderSteamID);
			if (fullPlayerDesc.detailData.characterID == ECharacterType.CharacterTypeCount)
			{
				fullPlayerDesc.detailData.characterID = characterID;
				fullPlayerDesc.detailData.characterIndex = (ushort)characterIndex;
				fullPlayerDesc.detailData.skinID = (ushort)skinID;
				fullPlayerDesc.detailData.equippedCharacterItemIds.Clear();
				foreach (ulong num in equipped)
				{
					fullPlayerDesc.detailData.equippedCharacterItemIds.Add((ushort)num);
				}
				fullPlayerDesc.detailData.equippedPlayerItemIds = new List<ushort>
				{
					0,
					0,
					0,
					0,
					0,
					0
				};
				p2PLockCharacterSelectAckMsg.accepted = true;
			}
			this.p2pServerMgr.EnqueueMessage(p2PLockCharacterSelectAckMsg);
			this.waitingForLockIns = true;
			Debug.Log("Lock in selection");
			this.checkStartLoadingStage();
			return true;
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000F420C File Offset: 0x000F260C
		private void checkStartLoadingStage()
		{
			Debug.Log("Check start loading stage");
			if (this.matchHostData.players == null || !this.waitingForLockIns)
			{
				Debug.Log("Bad time, abort " + this.waitingForLockIns);
				return;
			}
			bool flag = true;
			foreach (P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc in this.matchHostData.players)
			{
				if (sp2PMatchFullPlayerDesc.detailData.characterID == ECharacterType.CharacterTypeCount && !sp2PMatchFullPlayerDesc.baseData.isSpectator)
				{
					Debug.Log("Unlocked " + sp2PMatchFullPlayerDesc.baseData.name);
					flag = false;
				}
			}
			Debug.Log(string.Concat(new object[]
			{
				"Are we ready? ",
				flag,
				" ",
				this.timeSynchronizer.IsSynchronizationComplete
			}));
			if (flag && this.timeSynchronizer.IsSynchronizationComplete)
			{
				P2PMatchDetailsMsg p2PMatchDetailsMsg = new P2PMatchDetailsMsg();
				Debug.Log("Let's start " + this.matchHostData.players.Count);
				foreach (P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc2 in this.matchHostData.players)
				{
					P2PMatchDetailsMsg.SPlayerDesc splayerDesc = new P2PMatchDetailsMsg.SPlayerDesc();
					splayerDesc.team = sp2PMatchFullPlayerDesc2.baseData.team;
					splayerDesc.name = sp2PMatchFullPlayerDesc2.baseData.name;
					splayerDesc.userID = sp2PMatchFullPlayerDesc2.baseData.userID;
					splayerDesc.isSpectator = sp2PMatchFullPlayerDesc2.baseData.isSpectator;
					splayerDesc.characterSelection = sp2PMatchFullPlayerDesc2.detailData.characterID;
					splayerDesc.characterIndex = sp2PMatchFullPlayerDesc2.detailData.characterIndex;
					splayerDesc.skinId = sp2PMatchFullPlayerDesc2.detailData.skinID;
					splayerDesc.equippedCharacterItemIds = sp2PMatchFullPlayerDesc2.detailData.equippedCharacterItemIds;
					splayerDesc.equippedPlayerItemIds = sp2PMatchFullPlayerDesc2.detailData.equippedPlayerItemIds;
					p2PMatchDetailsMsg.Players.Add(splayerDesc);
					this.primaryPlayersLoadedTime = 0.0;
					this.waitingForStageLoad = true;
					this.waitingForLockIns = false;
				}
				this.p2pServerMgr.EnqueueMessage(p2PMatchDetailsMsg);
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000F4498 File Offset: 0x000F2898
		public void UsersLeft(List<ulong> missingUsers)
		{
			if (this.matchHostData.players == null)
			{
				return;
			}
			bool flag = false;
			foreach (ulong steamID in missingUsers)
			{
				P2PHost.SP2PMatchFullPlayerDesc fullPlayerDesc = this.getFullPlayerDesc(steamID);
				if (fullPlayerDesc != null)
				{
					fullPlayerDesc.detailData.isDisconnected = true;
					if (!fullPlayerDesc.baseData.isSpectator)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				bool flag2 = true;
				foreach (P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc in this.matchHostData.players)
				{
					if (!sp2PMatchFullPlayerDesc.detailData.stageLoaded && !sp2PMatchFullPlayerDesc.baseData.isSpectator)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					Debug.Log("Players not loaded");
					this.endMatch(EMatchResult.PlayerLeft, 0);
					return;
				}
			}
			this.checkReadyToStart();
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000F45C4 File Offset: 0x000F29C4
		public bool StageLoaded(ulong senderSteamID)
		{
			P2PHost.SP2PMatchFullPlayerDesc fullPlayerDesc = this.getFullPlayerDesc(senderSteamID);
			if (fullPlayerDesc != null)
			{
				fullPlayerDesc.detailData.stageLoaded = true;
			}
			return this.checkReadyToStart();
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000F45F1 File Offset: 0x000F29F1
		private void timedCheckReadyToStart()
		{
			this.checkReadyToStart();
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000F45FC File Offset: 0x000F29FC
		private bool checkReadyToStart()
		{
			if (this.matchHostData.players == null || !this.waitingForStageLoad)
			{
				return false;
			}
			this.timer.CancelTimeout(new Action(this.timedCheckReadyToStart));
			bool flag = true;
			bool flag2 = true;
			foreach (P2PHost.SP2PMatchFullPlayerDesc sp2PMatchFullPlayerDesc in this.matchHostData.players)
			{
				if (!sp2PMatchFullPlayerDesc.detailData.stageLoaded && !sp2PMatchFullPlayerDesc.detailData.isDisconnected)
				{
					flag = false;
					if (!sp2PMatchFullPlayerDesc.baseData.isSpectator)
					{
						flag2 = false;
					}
				}
			}
			if (flag2 && this.primaryPlayersLoadedTime == 0.0)
			{
				this.primaryPlayersLoadedTime = WTime.precisionTimeSinceStartup;
			}
			int num = 10000;
			if (flag)
			{
				P2PMatchCountdownMsg p2PMatchCountdownMsg = new P2PMatchCountdownMsg();
				p2PMatchCountdownMsg.countDownSeconds = 1U;
				p2PMatchCountdownMsg.serverStartTimeMs = (ulong)(WTime.precisionTimeSinceStartup + p2PMatchCountdownMsg.countDownSeconds * 1000U);
				this.p2pServerMgr.EnqueueMessage(p2PMatchCountdownMsg);
				this.waitingForStageLoad = false;
			}
			this.timer.SetOrReplaceTimeout(num + 500, new Action(this.timedCheckReadyToStart));
			return true;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000F4758 File Offset: 0x000F2B58
		public void ReceiveWinner(byte reportedWinningTeamMask)
		{
			if (this.matchHostData.winningTeamMask != 0 && this.matchHostData.winningTeamMask != reportedWinningTeamMask)
			{
				this.endMatch(EMatchResult.Forfeit, reportedWinningTeamMask);
				Debug.LogError("Mismatched winners. FAil this match");
				return;
			}
			this.matchHostData.winningTeamMask = reportedWinningTeamMask;
			this.matchHostData.reportedWinners = this.matchHostData.reportedWinners + 1U;
			Debug.Log(string.Concat(new object[]
			{
				"Reported winners ",
				this.matchHostData.reportedWinners,
				" ",
				this.matchHostData.players.Count
			}));
			this.endMatch(EMatchResult.Success, reportedWinningTeamMask);
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000F480B File Offset: 0x000F2C0B
		public bool OnPlayerAbandonMatch(ulong senderSteamID)
		{
			if (this.InMatch)
			{
				this.endMatch(EMatchResult.PlayerLeft, 0);
			}
			return true;
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000F4824 File Offset: 0x000F2C24
		private void endMatch(EMatchResult reason, byte winningTeamBitMask)
		{
			P2PMatchResultsMsg p2PMatchResultsMsg = new P2PMatchResultsMsg();
			p2PMatchResultsMsg.reason = reason;
			p2PMatchResultsMsg.winningTeamBitMask = winningTeamBitMask;
			this.p2pServerMgr.EnqueueMessage(p2PMatchResultsMsg);
			this.matchHostData.MatchID = Guid.Empty;
			this.steam.SetJoinable(true);
			this.SyncLobbyData();
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000F4874 File Offset: 0x000F2C74
		private P2PHost.SP2PFrameData getFrameData(int frame)
		{
			int num = frame % this.matchHostData.battleData.frames.Length;
			if (this.matchHostData.battleData.frames[num].frame < frame)
			{
				this.matchHostData.battleData.frames[num].frame = frame;
				this.matchHostData.battleData.frames[num].hashCodeSet = false;
			}
			else if (this.matchHostData.battleData.frames[num].frame != frame)
			{
				return null;
			}
			return this.matchHostData.battleData.frames[num];
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000F491C File Offset: 0x000F2D1C
		public void OnHashCode(int senderId, int frame, int hashCode)
		{
			if (this.playerDataList == null || !this.playerDataList.ContainsKey(senderId) || this.playerDataList[senderId].disconnectFrame != -1)
			{
				return;
			}
			if (this.InMatch && this.matchHostData.reportedWinners == 0U)
			{
				P2PHost.SP2PFrameData frameData = this.getFrameData(frame);
				if (frameData != null && frameData.frame == frame)
				{
					if (frameData.hashCodeSet)
					{
						if (frameData.hashCode != hashCode)
						{
						}
					}
					else
					{
						frameData.hashCode = hashCode;
						frameData.hashCodeSet = true;
					}
				}
			}
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000F49BC File Offset: 0x000F2DBC
		public void ConfirmPlayerChange(ulong userID, bool isSpectating, int team)
		{
			P2PChangePlayerConfirmedMsg p2PChangePlayerConfirmedMsg = new P2PChangePlayerConfirmedMsg();
			p2PChangePlayerConfirmedMsg.userID = userID;
			p2PChangePlayerConfirmedMsg.isSpectating = isSpectating;
			p2PChangePlayerConfirmedMsg.team = (byte)team;
			this.p2pServerMgr.EnqueueMessage(p2PChangePlayerConfirmedMsg);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000F49F4 File Offset: 0x000F2DF4
		public void ConfirmScreenChanged(ulong userID, int screenID)
		{
			P2PConfirmScreenChangedMsg p2PConfirmScreenChangedMsg = new P2PConfirmScreenChangedMsg();
			p2PConfirmScreenChangedMsg.userID = userID;
			p2PConfirmScreenChangedMsg.screenID = screenID;
			this.p2pServerMgr.EnqueueMessage(p2PConfirmScreenChangedMsg);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x000F4A24 File Offset: 0x000F2E24
		public void SyncLobbyData()
		{
			P2PSyncLobbyDataMsg p2PSyncLobbyDataMsg = new P2PSyncLobbyDataMsg();
			p2PSyncLobbyDataMsg.isInMatch = this.InMatch;
			this.p2pServerMgr.EnqueueMessage(p2PSyncLobbyDataMsg);
		}

		// Token: 0x040023A4 RID: 9124
		private const int INVALID_FRAME = -1;

		// Token: 0x040023A6 RID: 9126
		private P2PHost.SHostData matchHostData = default(P2PHost.SHostData);

		// Token: 0x040023A7 RID: 9127
		private double primaryPlayersLoadedTime;

		// Token: 0x040023A8 RID: 9128
		private bool waitingForStageLoad;

		// Token: 0x040023A9 RID: 9129
		private bool waitingForLockIns;

		// Token: 0x0200080F RID: 2063
		private struct SP2PMatchBattlePlayerData
		{
			// Token: 0x06003350 RID: 13136 RVA: 0x000F4A50 File Offset: 0x000F2E50
			public void Init()
			{
				this.furthestAckedFrame = -1;
				this.furthestReceivedFrame = -1;
			}

			// Token: 0x040023AA RID: 9130
			public int furthestReceivedFrame;

			// Token: 0x040023AB RID: 9131
			public int furthestAckedFrame;
		}

		// Token: 0x02000810 RID: 2064
		private class SP2PMatchFullPlayerDesc
		{
			// Token: 0x06003352 RID: 13138 RVA: 0x000F4A68 File Offset: 0x000F2E68
			public void Init()
			{
				this.baseData = new SP2PMatchBasicPlayerDesc();
				this.detailData = new SP2PMatchDetailPlayerDesc();
				this.metaData = new SP2PMatchMetaPlayerData();
				this.battleData = default(P2PHost.SP2PMatchBattlePlayerData);
			}

			// Token: 0x040023AC RID: 9132
			public SP2PMatchBasicPlayerDesc baseData;

			// Token: 0x040023AD RID: 9133
			public SP2PMatchDetailPlayerDesc detailData;

			// Token: 0x040023AE RID: 9134
			public SP2PMatchMetaPlayerData metaData;

			// Token: 0x040023AF RID: 9135
			public P2PHost.SP2PMatchBattlePlayerData battleData;
		}

		// Token: 0x02000811 RID: 2065
		private class SP2PFrameData
		{
			// Token: 0x040023B0 RID: 9136
			public int frame;

			// Token: 0x040023B1 RID: 9137
			public int hashCode;

			// Token: 0x040023B2 RID: 9138
			public bool hashCodeSet;
		}

		// Token: 0x02000812 RID: 2066
		private struct SP2PMatchBattleData
		{
			// Token: 0x06003354 RID: 13140 RVA: 0x000F4AB0 File Offset: 0x000F2EB0
			public void Init()
			{
				this.oldestFrame = -1;
				this.frames = new P2PHost.SP2PFrameData[InputMsg.MaxInputArraySize * 3];
				for (int i = 0; i < this.frames.Length; i++)
				{
					this.frames[i] = new P2PHost.SP2PFrameData();
				}
			}

			// Token: 0x040023B3 RID: 9139
			public int oldestFrame;

			// Token: 0x040023B4 RID: 9140
			public P2PHost.SP2PFrameData[] frames;
		}

		// Token: 0x02000813 RID: 2067
		private struct SHostData
		{
			// Token: 0x040023B5 RID: 9141
			public Guid MatchID;

			// Token: 0x040023B6 RID: 9142
			public LobbyGameMode gameMode;

			// Token: 0x040023B7 RID: 9143
			public ETeamAttack teamAttack;

			// Token: 0x040023B8 RID: 9144
			public uint matchLengthSeconds;

			// Token: 0x040023B9 RID: 9145
			public uint characterSelectSeconds;

			// Token: 0x040023BA RID: 9146
			public uint numLives;

			// Token: 0x040023BB RID: 9147
			public uint assistCount;

			// Token: 0x040023BC RID: 9148
			public byte winningTeamMask;

			// Token: 0x040023BD RID: 9149
			public uint reportedWinners;

			// Token: 0x040023BE RID: 9150
			public long minLatencyMs;

			// Token: 0x040023BF RID: 9151
			public List<EIconStages> stages;

			// Token: 0x040023C0 RID: 9152
			public List<P2PHost.SP2PMatchFullPlayerDesc> players;

			// Token: 0x040023C1 RID: 9153
			public P2PHost.SP2PMatchBattleData battleData;
		}
	}
}
