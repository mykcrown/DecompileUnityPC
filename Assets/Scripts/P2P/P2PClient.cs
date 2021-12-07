// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using MatchMaking;
using network;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace P2P
{
	public class P2PClient
	{
		public ulong hostSteamID;

		private long timeoutTimeMs;

		private const long normalTimeoutTimeSecs = 20L;

		private const long stageTimeoutTimeSecs = 180L;

		private const long inMatchTimeoutTimeSecs = 10L;

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
		public IStageDataHelper stageDataHelper
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
		public IDialogController dialogController
		{
			private get;
			set;
		}

		[Inject]
		public ConfigData config
		{
			private get;
			set;
		}

		[Inject]
		public ILocalization localization
		{
			private get;
			set;
		}

		public bool InMatch
		{
			get
			{
				return this.MatchID != Guid.Empty;
			}
		}

		public Guid MatchID
		{
			get;
			private set;
		}

		[PostConstruct]
		public void Init()
		{
			this.MatchID = Guid.Empty;
		}

		public void OnMatchConnect(Guid MatchID, ulong hostSteamID, EIconStages[] stages, uint characterSelectSeconds, uint matchLengthSeconds, uint numberOfLives, uint assistCount, SP2PMatchBasicPlayerDesc[] players, LobbyGameMode gameMode, ETeamAttack teamAttack)
		{
			this.MatchID = MatchID;
			this.hostSteamID = hostSteamID;
			this.SetTimeoutTime(20000L);
			this.p2pServerMgr.DoBroadcast(new MatchConnectEvent(stages, matchLengthSeconds, numberOfLives, assistCount, players, gameMode, teamAttack));
			int num = 0;
			for (int i = 0; i < players.Length; i++)
			{
				SP2PMatchBasicPlayerDesc sP2PMatchBasicPlayerDesc = players[i];
				if (sP2PMatchBasicPlayerDesc.steamID == this.steam.MySteamID().m_SteamID)
				{
					this.OnCharacterStaging(sP2PMatchBasicPlayerDesc.userID, num, players, characterSelectSeconds);
				}
				num++;
			}
		}

		public void OnCharacterSelectAck(ulong steamID, bool accepted)
		{
			if (steamID == this.steam.MySteamID().m_SteamID)
			{
				this.p2pServerMgr.DoBroadcast(new LockInSelectionAckEvent(this.MatchID, accepted));
			}
		}

		public void OnCharacterStaging(ulong userID, int playerIndex, SP2PMatchBasicPlayerDesc[] players, uint characterSelectSeconds)
		{
			this.p2pServerMgr.DoBroadcast(new MatchCharacterStagingEvent(this.MatchID, userID, playerIndex, players, characterSelectSeconds));
		}

		public bool LockInSelection(CSteamID senderSteamID, ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom, ulong[] equippeds)
		{
			P2PLockCharacterSelectMsg queuedMessage = new P2PLockCharacterSelectMsg(senderSteamID.m_SteamID, characterID, characterIndex, skinID, isRandom, equippeds);
			this.p2pServerMgr.EnqueueMessage(queuedMessage);
			return true;
		}

		public bool StageLoaded(CSteamID senderSteamID)
		{
			P2PStageLoadedMsg p2PStageLoadedMsg = new P2PStageLoadedMsg();
			p2PStageLoadedMsg.steamID = senderSteamID.m_SteamID;
			this.p2pServerMgr.EnqueueMessage(p2PStageLoadedMsg);
			return true;
		}

		public void SendWinner(byte winningTeamBitMask)
		{
			P2PSendWinnerMsg p2PSendWinnerMsg = new P2PSendWinnerMsg();
			p2PSendWinnerMsg.reportedWinningTeams = winningTeamBitMask;
			this.p2pServerMgr.EnqueueMessage(p2PSendWinnerMsg);
		}

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

		public void OnMatchDetails(P2PMatchDetailsMsg.SPlayerDesc[] players)
		{
			this.p2pServerMgr.DoBroadcast(new MatchDetailsEvent(this.MatchID, players));
			this.SetTimeoutTime(180000L);
		}

		public void OnMatchCountdown(ulong serverStartTimeMs, uint countdownSeconds)
		{
			long timeOffsetMs = this.timeSynchronizer.GetTimeOffsetMs();
			UnityEngine.Debug.Log("Time offset from host: " + timeOffsetMs);
			long matchStartTime = (long)(serverStartTimeMs + (ulong)timeOffsetMs);
			this.SetTimeoutTime(10000L);
			this.p2pServerMgr.DoBroadcast(new MatchBeginEvent(this.MatchID, matchStartTime, countdownSeconds));
		}

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
			this.hostSteamID = 0uL;
		}

		private void SetTimeoutTime(long timeoutTimeMs)
		{
			this.timeoutTimeMs = timeoutTimeMs;
		}

		public void OnDisconnected()
		{
			this.hostSteamID = 0uL;
		}

		private void onLobbyMembersUpdated()
		{
		}

		private bool checkIfHostLeft(List<SteamManager.SteamLobbyData> members)
		{
			if (this.hostSteamID != 0uL)
			{
				bool flag = false;
				foreach (SteamManager.SteamLobbyData current in members)
				{
					if (current.steamID.m_SteamID == this.hostSteamID)
					{
						flag = true;
						break;
					}
				}
				return !flag;
			}
			return false;
		}

		public bool HasDisconnected()
		{
			long lastMessageReceivedMs = this.p2pServerMgr.LastMessageReceivedMs;
			return lastMessageReceivedMs != 0L && WTime.currentTimeMs - lastMessageReceivedMs > this.timeoutTimeMs;
		}

		public void UpdateLobbyists(List<SteamManager.SteamLobbyData> members, bool forceDisconnect = false)
		{
			if (this.checkIfHostLeft(members))
			{
				this.OnDisconnected();
				this.OnLeftLobby(true);
				return;
			}
			SBasicMatchPlayerDesc[] array = new SBasicMatchPlayerDesc[members.Count];
			ulong hostUserId = 0uL;
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
			foreach (SteamManager.SteamLobbyData current in members)
			{
				array[num2] = new SBasicMatchPlayerDesc();
				array[num2].name = this.steam.GetUserName(current.steamID);
				array[num2].userID = current.steamID.m_SteamID;
				if (current.isHost)
				{
					hostUserId = array[num2].userID;
				}
				num2++;
			}
			JoinCustomMatchEvent joinCustomMatchEvent = new JoinCustomMatchEvent(JoinCustomMatchEvent.EResult.Result_Ok, hostUserId, array, gameMode, stageList);
			joinCustomMatchEvent.lobbyName = this.steam.GetLobbyValue(SteamManager.LobbyNameKey);
			this.p2pServerMgr.DoBroadcast(joinCustomMatchEvent);
		}

		public void OnLeftLobby(bool hostLeft)
		{
			if (hostLeft)
			{
				this.p2pServerMgr.DoBroadcast(new CustomMatchDestroyedEvent(CustomMatchDestroyedEvent.EReason.Reason_OwnerDestroyed, 0uL, null));
			}
			else
			{
				this.p2pServerMgr.DoBroadcast(new LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult.Result_Ok));
			}
		}

		public void ChangePlayer(ulong userID, bool isSpectating, int team)
		{
			P2PRequestChangePlayerMsg p2PRequestChangePlayerMsg = new P2PRequestChangePlayerMsg();
			p2PRequestChangePlayerMsg.userID = userID;
			p2PRequestChangePlayerMsg.isSpectating = isSpectating;
			p2PRequestChangePlayerMsg.team = (byte)team;
			this.p2pServerMgr.EnqueueMessage(p2PRequestChangePlayerMsg);
		}

		public void InformScreenChanged(ulong userID, ScreenType screen)
		{
			P2PInformScreenChangedMsg p2PInformScreenChangedMsg = new P2PInformScreenChangedMsg();
			p2PInformScreenChangedMsg.userID = userID;
			p2PInformScreenChangedMsg.screenID = (int)screen;
			this.p2pServerMgr.EnqueueMessage(p2PInformScreenChangedMsg);
		}
	}
}
