using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using Steamworks;
using UnityEngine;

namespace P2P
{
	// Token: 0x0200080D RID: 2061
	public class P2PClient
	{
		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06003304 RID: 13060 RVA: 0x000F3472 File Offset: 0x000F1872
		// (set) Token: 0x06003305 RID: 13061 RVA: 0x000F347A File Offset: 0x000F187A
		[Inject]
		public SteamManager steam { get; set; }

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06003306 RID: 13062 RVA: 0x000F3483 File Offset: 0x000F1883
		// (set) Token: 0x06003307 RID: 13063 RVA: 0x000F348B File Offset: 0x000F188B
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x000F3494 File Offset: 0x000F1894
		// (set) Token: 0x06003309 RID: 13065 RVA: 0x000F349C File Offset: 0x000F189C
		[Inject]
		public IStageDataHelper stageDataHelper { get; set; }

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x000F34A5 File Offset: 0x000F18A5
		// (set) Token: 0x0600330B RID: 13067 RVA: 0x000F34AD File Offset: 0x000F18AD
		[Inject]
		public IPingManager pingManager { private get; set; }

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x000F34B6 File Offset: 0x000F18B6
		// (set) Token: 0x0600330D RID: 13069 RVA: 0x000F34BE File Offset: 0x000F18BE
		[Inject]
		public ITimeSynchronizer timeSynchronizer { private get; set; }

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x000F34C7 File Offset: 0x000F18C7
		// (set) Token: 0x0600330F RID: 13071 RVA: 0x000F34CF File Offset: 0x000F18CF
		[Inject]
		public IDialogController dialogController { private get; set; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x000F34D8 File Offset: 0x000F18D8
		// (set) Token: 0x06003311 RID: 13073 RVA: 0x000F34E0 File Offset: 0x000F18E0
		[Inject]
		public ConfigData config { private get; set; }

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x000F34E9 File Offset: 0x000F18E9
		// (set) Token: 0x06003313 RID: 13075 RVA: 0x000F34F1 File Offset: 0x000F18F1
		[Inject]
		public ILocalization localization { private get; set; }

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x000F34FA File Offset: 0x000F18FA
		public bool InMatch
		{
			get
			{
				return this.MatchID != Guid.Empty;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x000F350C File Offset: 0x000F190C
		// (set) Token: 0x06003316 RID: 13078 RVA: 0x000F3514 File Offset: 0x000F1914
		public Guid MatchID { get; private set; }

		// Token: 0x06003317 RID: 13079 RVA: 0x000F351D File Offset: 0x000F191D
		[PostConstruct]
		public void Init()
		{
			this.MatchID = Guid.Empty;
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000F352C File Offset: 0x000F192C
		public void OnMatchConnect(Guid MatchID, ulong hostSteamID, EIconStages[] stages, uint characterSelectSeconds, uint matchLengthSeconds, uint numberOfLives, uint assistCount, SP2PMatchBasicPlayerDesc[] players, LobbyGameMode gameMode, ETeamAttack teamAttack)
		{
			this.MatchID = MatchID;
			this.hostSteamID = hostSteamID;
			this.SetTimeoutTime(20000L);
			this.p2pServerMgr.DoBroadcast(new MatchConnectEvent(stages, matchLengthSeconds, numberOfLives, assistCount, players, gameMode, teamAttack));
			int num = 0;
			foreach (SP2PMatchBasicPlayerDesc sp2PMatchBasicPlayerDesc in players)
			{
				if (sp2PMatchBasicPlayerDesc.steamID == this.steam.MySteamID().m_SteamID)
				{
					this.OnCharacterStaging(sp2PMatchBasicPlayerDesc.userID, num, players, characterSelectSeconds);
				}
				num++;
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000F35C4 File Offset: 0x000F19C4
		public void OnCharacterSelectAck(ulong steamID, bool accepted)
		{
			if (steamID == this.steam.MySteamID().m_SteamID)
			{
				this.p2pServerMgr.DoBroadcast(new LockInSelectionAckEvent(this.MatchID, accepted));
			}
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000F3601 File Offset: 0x000F1A01
		public void OnCharacterStaging(ulong userID, int playerIndex, SP2PMatchBasicPlayerDesc[] players, uint characterSelectSeconds)
		{
			this.p2pServerMgr.DoBroadcast(new MatchCharacterStagingEvent(this.MatchID, userID, playerIndex, players, characterSelectSeconds));
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000F3620 File Offset: 0x000F1A20
		public bool LockInSelection(CSteamID senderSteamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equippeds)
		{
			P2PLockCharacterSelectMsg queuedMessage = new P2PLockCharacterSelectMsg(senderSteamID.m_SteamID, characterID, characterIndex, skinID, isRandom, equippeds);
			this.p2pServerMgr.EnqueueMessage(queuedMessage);
			return true;
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000F3650 File Offset: 0x000F1A50
		public bool StageLoaded(CSteamID senderSteamID)
		{
			P2PStageLoadedMsg p2PStageLoadedMsg = new P2PStageLoadedMsg();
			p2PStageLoadedMsg.steamID = senderSteamID.m_SteamID;
			this.p2pServerMgr.EnqueueMessage(p2PStageLoadedMsg);
			return true;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000F3680 File Offset: 0x000F1A80
		public void SendWinner(byte winningTeamBitMask)
		{
			P2PSendWinnerMsg p2PSendWinnerMsg = new P2PSendWinnerMsg();
			p2PSendWinnerMsg.reportedWinningTeams = winningTeamBitMask;
			this.p2pServerMgr.EnqueueMessage(p2PSendWinnerMsg);
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000F36A8 File Offset: 0x000F1AA8
		public bool LeaveMatch()
		{
			if (this.InMatch)
			{
				P2PForfeitMatchMsg p2PForfeitMatchMsg = new P2PForfeitMatchMsg();
				p2PForfeitMatchMsg.senderSteamID = this.steam.MySteamID().m_SteamID;
				this.p2pServerMgr.EnqueueMessage(p2PForfeitMatchMsg);
			}
			return true;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000F36ED File Offset: 0x000F1AED
		public void OnMatchDetails(P2PMatchDetailsMsg.SPlayerDesc[] players)
		{
			this.p2pServerMgr.DoBroadcast(new MatchDetailsEvent(this.MatchID, players));
			this.SetTimeoutTime(180000L);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000F3714 File Offset: 0x000F1B14
		public void OnMatchCountdown(ulong serverStartTimeMs, uint countdownSeconds)
		{
			long timeOffsetMs = this.timeSynchronizer.GetTimeOffsetMs();
			Debug.Log("Time offset from host: " + timeOffsetMs);
			long matchStartTime = (long)(serverStartTimeMs + (ulong)timeOffsetMs);
			this.SetTimeoutTime(10000L);
			this.p2pServerMgr.DoBroadcast(new MatchBeginEvent(this.MatchID, matchStartTime, countdownSeconds));
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000F376C File Offset: 0x000F1B6C
		public void OnMatchResults(EMatchResult reason, byte winningTeamBitMask)
		{
			switch (reason)
			{
			case EMatchResult.Success:
				this.p2pServerMgr.DoBroadcast(new MatchResultsEvent(this.MatchID, winningTeamBitMask));
				goto IL_6F;
			case EMatchResult.PlayerLeft:
			case EMatchResult.Forfeit:
				this.p2pServerMgr.DoBroadcast(new MatchFailureEvent(this.MatchID, MatchFailureEvent.EReason.PlayerLeft));
				goto IL_6F;
			}
			this.p2pServerMgr.DoBroadcast(new MatchFailureEvent(this.MatchID, MatchFailureEvent.EReason.InternalFailure));
			IL_6F:
			this.MatchID = Guid.Empty;
			this.hostSteamID = 0UL;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000F37FB File Offset: 0x000F1BFB
		private void SetTimeoutTime(long timeoutTimeMs)
		{
			this.timeoutTimeMs = timeoutTimeMs;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000F3804 File Offset: 0x000F1C04
		public void OnDisconnected()
		{
			this.hostSteamID = 0UL;
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000F380E File Offset: 0x000F1C0E
		private void onLobbyMembersUpdated()
		{
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000F3810 File Offset: 0x000F1C10
		private bool checkIfHostLeft(List<SteamManager.SteamLobbyData> members)
		{
			if (this.hostSteamID != 0UL)
			{
				bool flag = false;
				foreach (SteamManager.SteamLobbyData steamLobbyData in members)
				{
					if (steamLobbyData.steamID.m_SteamID == this.hostSteamID)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
			return false;
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000F3894 File Offset: 0x000F1C94
		public bool HasDisconnected()
		{
			long lastMessageReceivedMs = this.p2pServerMgr.LastMessageReceivedMs;
			return lastMessageReceivedMs != 0L && WTime.currentTimeMs - lastMessageReceivedMs > this.timeoutTimeMs;
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000F38C8 File Offset: 0x000F1CC8
		public void UpdateLobbyists(List<SteamManager.SteamLobbyData> members, bool forceDisconnect = false)
		{
			if (this.checkIfHostLeft(members))
			{
				this.OnDisconnected();
				this.OnLeftLobby(true);
				return;
			}
			SBasicMatchPlayerDesc[] array = new SBasicMatchPlayerDesc[members.Count];
			ulong hostUserId = 0UL;
			string lobbyValue = this.steam.GetLobbyValue(SteamManager.LobbyStageKey);
			string lobbyValue2 = this.steam.GetLobbyValue(SteamManager.LobbyVersionKey);
			string lobbyValue3 = this.steam.GetLobbyValue(SteamManager.LobbyGameModeKey);
			int num = 0;
			int.TryParse(lobbyValue3, out num);
			LobbyGameMode gameMode = (LobbyGameMode)num;
			if (string.IsNullOrEmpty(lobbyValue2))
			{
				this.OnLeftLobby(false);
				return;
			}
			if (lobbyValue2 != BuildConfigUtil.GetCompareVersion(this.config))
			{
				this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.JoinRoomFail.VersionMismatch.body"), this.localization.GetText("dialog.OnlineGame.Error.Mismatch.body"), this.localization.GetText("dialog.JoinQueueFail.ServiceNotReady.ok"), WindowTransition.STANDARD_FADE, false, default(AudioData));
				this.OnLeftLobby(false);
				return;
			}
			EIconStages[] stageList;
			if (lobbyValue != null)
			{
				stageList = new EIconStages[]
				{
					(EIconStages)Enum.Parse(typeof(EIconStages), lobbyValue)
				};
			}
			else
			{
				stageList = new EIconStages[0];
			}
			int num2 = 0;
			foreach (SteamManager.SteamLobbyData steamLobbyData in members)
			{
				array[num2] = new SBasicMatchPlayerDesc();
				array[num2].name = this.steam.GetUserName(steamLobbyData.steamID);
				array[num2].userID = steamLobbyData.steamID.m_SteamID;
				if (steamLobbyData.isHost)
				{
					hostUserId = array[num2].userID;
				}
				num2++;
			}
			JoinCustomMatchEvent joinCustomMatchEvent = new JoinCustomMatchEvent(JoinCustomMatchEvent.EResult.Result_Ok, hostUserId, array, gameMode, stageList);
			joinCustomMatchEvent.lobbyName = this.steam.GetLobbyValue(SteamManager.LobbyNameKey);
			this.p2pServerMgr.DoBroadcast(joinCustomMatchEvent);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000F3AC0 File Offset: 0x000F1EC0
		public void OnLeftLobby(bool hostLeft)
		{
			if (hostLeft)
			{
				this.p2pServerMgr.DoBroadcast(new CustomMatchDestroyedEvent(CustomMatchDestroyedEvent.EReason.Reason_OwnerDestroyed, 0UL, null));
			}
			else
			{
				this.p2pServerMgr.DoBroadcast(new LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult.Result_Ok));
			}
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000F3AF4 File Offset: 0x000F1EF4
		public void ChangePlayer(ulong userID, bool isSpectating, int team)
		{
			P2PRequestChangePlayerMsg p2PRequestChangePlayerMsg = new P2PRequestChangePlayerMsg();
			p2PRequestChangePlayerMsg.userID = userID;
			p2PRequestChangePlayerMsg.isSpectating = isSpectating;
			p2PRequestChangePlayerMsg.team = (byte)team;
			this.p2pServerMgr.EnqueueMessage(p2PRequestChangePlayerMsg);
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000F3B2C File Offset: 0x000F1F2C
		public void InformScreenChanged(ulong userID, ScreenType screen)
		{
			P2PInformScreenChangedMsg p2PInformScreenChangedMsg = new P2PInformScreenChangedMsg();
			p2PInformScreenChangedMsg.userID = userID;
			p2PInformScreenChangedMsg.screenID = (int)screen;
			this.p2pServerMgr.EnqueueMessage(p2PInformScreenChangedMsg);
		}

		// Token: 0x04002399 RID: 9113
		public ulong hostSteamID;

		// Token: 0x0400239A RID: 9114
		private long timeoutTimeMs;

		// Token: 0x0400239B RID: 9115
		private const long normalTimeoutTimeSecs = 20L;

		// Token: 0x0400239C RID: 9116
		private const long stageTimeoutTimeSecs = 180L;

		// Token: 0x0400239D RID: 9117
		private const long inMatchTimeoutTimeSecs = 10L;
	}
}
