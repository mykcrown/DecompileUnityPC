// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace P2P
{
	public class P2PHost
	{
		private struct SP2PMatchBattlePlayerData
		{
			public int furthestReceivedFrame;

			public int furthestAckedFrame;

			public void Init()
			{
				this.furthestAckedFrame = -1;
				this.furthestReceivedFrame = -1;
			}
		}

		private class SP2PMatchFullPlayerDesc
		{
			public SP2PMatchBasicPlayerDesc baseData;

			public SP2PMatchDetailPlayerDesc detailData;

			public SP2PMatchMetaPlayerData metaData;

			public P2PHost.SP2PMatchBattlePlayerData battleData;

			public void Init()
			{
				this.baseData = new SP2PMatchBasicPlayerDesc();
				this.detailData = new SP2PMatchDetailPlayerDesc();
				this.metaData = new SP2PMatchMetaPlayerData();
				this.battleData = default(P2PHost.SP2PMatchBattlePlayerData);
			}
		}

		private class SP2PFrameData
		{
			public int frame;

			public int hashCode;

			public bool hashCodeSet;
		}

		private struct SP2PMatchBattleData
		{
			public int oldestFrame;

			public P2PHost.SP2PFrameData[] frames;

			public void Init()
			{
				this.oldestFrame = -1;
				this.frames = new P2PHost.SP2PFrameData[InputMsg.MaxInputArraySize * 3];
				for (int i = 0; i < this.frames.Length; i++)
				{
					this.frames[i] = new P2PHost.SP2PFrameData();
				}
			}
		}

		private struct SHostData
		{
			public Guid MatchID;

			public LobbyGameMode gameMode;

			public ETeamAttack teamAttack;

			public uint matchLengthSeconds;

			public uint characterSelectSeconds;

			public uint numLives;

			public uint assistCount;

			public byte winningTeamMask;

			public uint reportedWinners;

			public long minLatencyMs;

			public List<EIconStages> stages;

			public List<P2PHost.SP2PMatchFullPlayerDesc> players;

			public P2PHost.SP2PMatchBattleData battleData;
		}

		private const int INVALID_FRAME = -1;

		private P2PHost.SHostData matchHostData = default(P2PHost.SHostData);

		private double primaryPlayersLoadedTime;

		private bool waitingForStageLoad;

		private bool waitingForLockIns;

		[Inject]
		public SteamManager steam
		{
			get;
			set;
		}

		[Inject]
		public P2PServerMgr p2pServerMgr
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
		public IStageDataHelper stageDataHelper
		{
			get;
			set;
		}

		[Inject]
		public ITimeSynchronizer timeSynchronizer
		{
			private get;
			set;
		}

		[Inject]
		public IMainThreadTimer timer
		{
			private get;
			set;
		}

		public bool InMatch
		{
			get
			{
				return this.matchHostData.MatchID != Guid.Empty;
			}
		}

		public Dictionary<int, RollbackPlayerData> playerDataList
		{
			private get;
			set;
		}

		public bool IsHost
		{
			get
			{
				return this.p2pServerMgr.IsHost;
			}
		}

		[PostConstruct]
		public void Init()
		{
			this.matchHostData.MatchID = Guid.Empty;
			this.matchHostData.battleData = default(P2PHost.SP2PMatchBattleData);
			this.timeSynchronizer.SetUpdateCallback(new Action(this.onTimeSyncComplete));
		}

		public void OnCreateLobby()
		{
			this.matchHostData.MatchID = Guid.Empty;
			this.matchHostData.battleData = default(P2PHost.SP2PMatchBattleData);
		}

		public void StartCustomMatch(List<SteamManager.SteamLobbyData> lobbyists, Dictionary<ulong, LobbyPlayerData> players, StageID selectedStage, LobbyGameMode gameMode)
		{
			this.matchHostData.MatchID = Guid.NewGuid();
			this.matchHostData.gameMode = gameMode;
			this.matchHostData.teamAttack = ETeamAttack.Enabled;
			this.matchHostData.matchLengthSeconds = ((gameMode != LobbyGameMode.Time) ? 480u : 180u);
			this.matchHostData.characterSelectSeconds = 60u;
			this.matchHostData.assistCount = 99u;
			this.matchHostData.numLives = 3u;
			this.matchHostData.stages = new List<EIconStages>();
			if (selectedStage == StageID.Random)
			{
				selectedStage = this.stageDataHelper.GetRandomPossibleStage();
			}
			this.matchHostData.stages.Add(this.stageDataHelper.GetIconStageFromStageID(selectedStage));
			this.matchHostData.players = new List<P2PHost.SP2PMatchFullPlayerDesc>();
			this.matchHostData.winningTeamMask = 0;
			this.matchHostData.reportedWinners = 0u;
			this.matchHostData.minLatencyMs = 0L;
			this.matchHostData.battleData.Init();
			List<SP2PMatchBasicPlayerDesc> list = new List<SP2PMatchBasicPlayerDesc>();
			Dictionary<ulong, P2PHost.SP2PMatchFullPlayerDesc> dictionary = new Dictionary<ulong, P2PHost.SP2PMatchFullPlayerDesc>();
			foreach (SteamManager.SteamLobbyData current in lobbyists)
			{
				LobbyPlayerData lobbyPlayerData;
				players.TryGetValue(current.steamID.m_SteamID, out lobbyPlayerData);
				P2PHost.SP2PMatchFullPlayerDesc sP2PMatchFullPlayerDesc = new P2PHost.SP2PMatchFullPlayerDesc();
				sP2PMatchFullPlayerDesc.Init();
				sP2PMatchFullPlayerDesc.baseData.steamID = current.steamID.m_SteamID;
				sP2PMatchFullPlayerDesc.baseData.name = this.steam.GetUserName(current.steamID);
				sP2PMatchFullPlayerDesc.baseData.userID = current.steamID.m_SteamID;
				sP2PMatchFullPlayerDesc.baseData.team = (byte)lobbyPlayerData.team;
				sP2PMatchFullPlayerDesc.baseData.isSpectator = lobbyPlayerData.isSpectator;
				sP2PMatchFullPlayerDesc.detailData.characterID = ECharacterType.CharacterTypeCount;
				sP2PMatchFullPlayerDesc.battleData.Init();
				list.Add(sP2PMatchFullPlayerDesc.baseData);
				dictionary[sP2PMatchFullPlayerDesc.baseData.userID] = sP2PMatchFullPlayerDesc;
			}
			list.Sort(new Comparison<SP2PMatchBasicPlayerDesc>(this.sortList));
			foreach (SP2PMatchBasicPlayerDesc current2 in list)
			{
				this.matchHostData.players.Add(dictionary[current2.userID]);
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

		private void onTimeSyncComplete()
		{
			UnityEngine.Debug.Log("On time sync complete " + this.p2pServerMgr.IsHost);
			if (this.p2pServerMgr.IsHost)
			{
				this.checkStartLoadingStage();
			}
		}

		private P2PHost.SP2PMatchFullPlayerDesc getFullPlayerDesc(ulong steamID)
		{
			foreach (P2PHost.SP2PMatchFullPlayerDesc current in this.matchHostData.players)
			{
				if (current.baseData.steamID == steamID)
				{
					return current;
				}
			}
			return null;
		}

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
				for (int i = 0; i < equipped.Length; i++)
				{
					ulong num = equipped[i];
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
			UnityEngine.Debug.Log("Lock in selection");
			this.checkStartLoadingStage();
			return true;
		}

		private void checkStartLoadingStage()
		{
			UnityEngine.Debug.Log("Check start loading stage");
			if (this.matchHostData.players == null || !this.waitingForLockIns)
			{
				UnityEngine.Debug.Log("Bad time, abort " + this.waitingForLockIns);
				return;
			}
			bool flag = true;
			foreach (P2PHost.SP2PMatchFullPlayerDesc current in this.matchHostData.players)
			{
				if (current.detailData.characterID == ECharacterType.CharacterTypeCount && !current.baseData.isSpectator)
				{
					UnityEngine.Debug.Log("Unlocked " + current.baseData.name);
					flag = false;
				}
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Are we ready? ",
				flag,
				" ",
				this.timeSynchronizer.IsSynchronizationComplete
			}));
			if (flag && this.timeSynchronizer.IsSynchronizationComplete)
			{
				P2PMatchDetailsMsg p2PMatchDetailsMsg = new P2PMatchDetailsMsg();
				UnityEngine.Debug.Log("Let's start " + this.matchHostData.players.Count);
				foreach (P2PHost.SP2PMatchFullPlayerDesc current2 in this.matchHostData.players)
				{
					P2PMatchDetailsMsg.SPlayerDesc sPlayerDesc = new P2PMatchDetailsMsg.SPlayerDesc();
					sPlayerDesc.team = current2.baseData.team;
					sPlayerDesc.name = current2.baseData.name;
					sPlayerDesc.userID = current2.baseData.userID;
					sPlayerDesc.isSpectator = current2.baseData.isSpectator;
					sPlayerDesc.characterSelection = current2.detailData.characterID;
					sPlayerDesc.characterIndex = current2.detailData.characterIndex;
					sPlayerDesc.skinId = current2.detailData.skinID;
					sPlayerDesc.equippedCharacterItemIds = current2.detailData.equippedCharacterItemIds;
					sPlayerDesc.equippedPlayerItemIds = current2.detailData.equippedPlayerItemIds;
					p2PMatchDetailsMsg.Players.Add(sPlayerDesc);
					this.primaryPlayersLoadedTime = 0.0;
					this.waitingForStageLoad = true;
					this.waitingForLockIns = false;
				}
				this.p2pServerMgr.EnqueueMessage(p2PMatchDetailsMsg);
			}
		}

		public void UsersLeft(List<ulong> missingUsers)
		{
			if (this.matchHostData.players == null)
			{
				return;
			}
			bool flag = false;
			foreach (ulong current in missingUsers)
			{
				P2PHost.SP2PMatchFullPlayerDesc fullPlayerDesc = this.getFullPlayerDesc(current);
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
				foreach (P2PHost.SP2PMatchFullPlayerDesc current2 in this.matchHostData.players)
				{
					if (!current2.detailData.stageLoaded && !current2.baseData.isSpectator)
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					UnityEngine.Debug.Log("Players not loaded");
					this.endMatch(EMatchResult.PlayerLeft, 0);
					return;
				}
			}
			this.checkReadyToStart();
		}

		public bool StageLoaded(ulong senderSteamID)
		{
			P2PHost.SP2PMatchFullPlayerDesc fullPlayerDesc = this.getFullPlayerDesc(senderSteamID);
			if (fullPlayerDesc != null)
			{
				fullPlayerDesc.detailData.stageLoaded = true;
			}
			return this.checkReadyToStart();
		}

		private void timedCheckReadyToStart()
		{
			this.checkReadyToStart();
		}

		private bool checkReadyToStart()
		{
			if (this.matchHostData.players == null || !this.waitingForStageLoad)
			{
				return false;
			}
			this.timer.CancelTimeout(new Action(this.timedCheckReadyToStart));
			bool flag = true;
			bool flag2 = true;
			foreach (P2PHost.SP2PMatchFullPlayerDesc current in this.matchHostData.players)
			{
				if (!current.detailData.stageLoaded && !current.detailData.isDisconnected)
				{
					flag = false;
					if (!current.baseData.isSpectator)
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
				p2PMatchCountdownMsg.countDownSeconds = 1u;
				p2PMatchCountdownMsg.serverStartTimeMs = (ulong)(WTime.precisionTimeSinceStartup + p2PMatchCountdownMsg.countDownSeconds * 1000u);
				this.p2pServerMgr.EnqueueMessage(p2PMatchCountdownMsg);
				this.waitingForStageLoad = false;
			}
			this.timer.SetOrReplaceTimeout(num + 500, new Action(this.timedCheckReadyToStart));
			return true;
		}

		public void ReceiveWinner(byte reportedWinningTeamMask)
		{
			if (this.matchHostData.winningTeamMask != 0 && this.matchHostData.winningTeamMask != reportedWinningTeamMask)
			{
				this.endMatch(EMatchResult.Forfeit, reportedWinningTeamMask);
				UnityEngine.Debug.LogError("Mismatched winners. FAil this match");
				return;
			}
			this.matchHostData.winningTeamMask = reportedWinningTeamMask;
			this.matchHostData.reportedWinners = this.matchHostData.reportedWinners + 1u;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Reported winners ",
				this.matchHostData.reportedWinners,
				" ",
				this.matchHostData.players.Count
			}));
			this.endMatch(EMatchResult.Success, reportedWinningTeamMask);
		}

		public bool OnPlayerAbandonMatch(ulong senderSteamID)
		{
			if (this.InMatch)
			{
				this.endMatch(EMatchResult.PlayerLeft, 0);
			}
			return true;
		}

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

		public void OnHashCode(int senderId, int frame, int hashCode)
		{
			if (this.playerDataList == null || !this.playerDataList.ContainsKey(senderId) || this.playerDataList[senderId].disconnectFrame != -1)
			{
				return;
			}
			if (this.InMatch && this.matchHostData.reportedWinners == 0u)
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

		public void ConfirmPlayerChange(ulong userID, bool isSpectating, int team)
		{
			P2PChangePlayerConfirmedMsg p2PChangePlayerConfirmedMsg = new P2PChangePlayerConfirmedMsg();
			p2PChangePlayerConfirmedMsg.userID = userID;
			p2PChangePlayerConfirmedMsg.isSpectating = isSpectating;
			p2PChangePlayerConfirmedMsg.team = (byte)team;
			this.p2pServerMgr.EnqueueMessage(p2PChangePlayerConfirmedMsg);
		}

		public void ConfirmScreenChanged(ulong userID, int screenID)
		{
			P2PConfirmScreenChangedMsg p2PConfirmScreenChangedMsg = new P2PConfirmScreenChangedMsg();
			p2PConfirmScreenChangedMsg.userID = userID;
			p2PConfirmScreenChangedMsg.screenID = screenID;
			this.p2pServerMgr.EnqueueMessage(p2PConfirmScreenChangedMsg);
		}

		public void SyncLobbyData()
		{
			P2PSyncLobbyDataMsg p2PSyncLobbyDataMsg = new P2PSyncLobbyDataMsg();
			p2PSyncLobbyDataMsg.isInMatch = this.InMatch;
			this.p2pServerMgr.EnqueueMessage(p2PSyncLobbyDataMsg);
		}
	}
}
