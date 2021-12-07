// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using network;
using System;
using System.Collections.Generic;

namespace IconsServer
{
	public class IconsServerManager : IIconsServerAPI
	{
		private ulong m_sessionId;

		private ulong m_accountId;

		private string m_username;

		private bool isInit;

		[Inject]
		public IDependencyInjection injector
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
		public IBattleServerAPI battleServerAPI
		{
			get;
			set;
		}

		[Inject]
		public DeveloperConfig devConfig
		{
			get;
			set;
		}

		[Inject]
		public IDebugLatencyManager latencyManager
		{
			get;
			set;
		}

		[Inject]
		public IPreviousCrashDetector previousCrashDetector
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
		public SteamManager steamManager
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
		public ICustomLobbyController customLobby
		{
			get;
			set;
		}

		public ulong SessionId
		{
			get
			{
				return this.m_sessionId;
			}
			private set
			{
			}
		}

		public ulong AccountId
		{
			get
			{
				return this.m_accountId;
			}
			private set
			{
			}
		}

		public string Username
		{
			get
			{
				return this.p2pServerMgr.Username;
			}
			private set
			{
			}
		}

		public int ServerTimestepDelta
		{
			get
			{
				return 0;
			}
		}

		public bool Startup(int networkPollTimerMs = 10)
		{
			if (this.isInit)
			{
				return false;
			}
			this.isInit = true;
			this.p2pServerMgr.Init();
			return true;
		}

		public void Shutdown()
		{
			this.LeaveCustomMatch();
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.SafeShutdown);
		}

		public void CloseAllConnections()
		{
		}

		public bool LeaveMatch(Guid matchId)
		{
			return this.p2pServerMgr.LeaveMatch();
		}

		public bool CustomMatchSetParams(SCustomLobbyParams cmparams)
		{
			this.p2pServerMgr.SetParams(cmparams);
			return true;
		}

		public bool LeaveCustomMatch()
		{
			this.p2pServerMgr.OnLeftLobby();
			return true;
		}

		public bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
		{
			this.p2pServerMgr.StartCustomMatch(this.customLobby.StageID, players, gameMode);
			return true;
		}

		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			this.p2pServerMgr.EnqueueMessage(queuedMessage);
			return true;
		}

		public bool SendClientWinner(byte winningTeamMask)
		{
			this.p2pServerMgr.SendWinner(winningTeamMask);
			return true;
		}

		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return this.p2pServerMgr.LockInSelection(characterID, characterIndex, skinID, isRandom);
		}

		public bool StageLoaded()
		{
			return this.p2pServerMgr.StageLoaded();
		}

		public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
			this.p2pServerMgr.ListenForServerEvents<T>(callback);
		}

		public void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
			this.p2pServerMgr.StopListeningForServerEvents<T>(callback);
		}
	}
}
