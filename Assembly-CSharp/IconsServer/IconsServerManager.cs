using System;
using System.Collections.Generic;
using MatchMaking;
using network;

namespace IconsServer
{
	// Token: 0x020007F5 RID: 2037
	public class IconsServerManager : IIconsServerAPI
	{
		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x000F2A47 File Offset: 0x000F0E47
		// (set) Token: 0x06003222 RID: 12834 RVA: 0x000F2A4F File Offset: 0x000F0E4F
		[Inject]
		public IDependencyInjection injector { get; set; }

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003223 RID: 12835 RVA: 0x000F2A58 File Offset: 0x000F0E58
		// (set) Token: 0x06003224 RID: 12836 RVA: 0x000F2A60 File Offset: 0x000F0E60
		[Inject]
		public ConfigData config { get; set; }

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x000F2A69 File Offset: 0x000F0E69
		// (set) Token: 0x06003226 RID: 12838 RVA: 0x000F2A71 File Offset: 0x000F0E71
		[Inject]
		public IBattleServerAPI battleServerAPI { get; set; }

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x000F2A7A File Offset: 0x000F0E7A
		// (set) Token: 0x06003228 RID: 12840 RVA: 0x000F2A82 File Offset: 0x000F0E82
		[Inject]
		public DeveloperConfig devConfig { get; set; }

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003229 RID: 12841 RVA: 0x000F2A8B File Offset: 0x000F0E8B
		// (set) Token: 0x0600322A RID: 12842 RVA: 0x000F2A93 File Offset: 0x000F0E93
		[Inject]
		public IDebugLatencyManager latencyManager { get; set; }

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x000F2A9C File Offset: 0x000F0E9C
		// (set) Token: 0x0600322C RID: 12844 RVA: 0x000F2AA4 File Offset: 0x000F0EA4
		[Inject]
		public IPreviousCrashDetector previousCrashDetector { get; set; }

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000F2AAD File Offset: 0x000F0EAD
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x000F2AB5 File Offset: 0x000F0EB5
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000F2ABE File Offset: 0x000F0EBE
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x000F2AC6 File Offset: 0x000F0EC6
		[Inject]
		public SteamManager steamManager { get; set; }

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x000F2ACF File Offset: 0x000F0ECF
		// (set) Token: 0x06003232 RID: 12850 RVA: 0x000F2AD7 File Offset: 0x000F0ED7
		[Inject]
		public GameDataManager gameDataManager { get; set; }

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000F2AE0 File Offset: 0x000F0EE0
		// (set) Token: 0x06003234 RID: 12852 RVA: 0x000F2AE8 File Offset: 0x000F0EE8
		[Inject]
		public ICustomLobbyController customLobby { get; set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x000F2AF1 File Offset: 0x000F0EF1
		// (set) Token: 0x06003236 RID: 12854 RVA: 0x000F2AF9 File Offset: 0x000F0EF9
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

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x000F2AFB File Offset: 0x000F0EFB
		// (set) Token: 0x06003238 RID: 12856 RVA: 0x000F2B03 File Offset: 0x000F0F03
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

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x000F2B05 File Offset: 0x000F0F05
		// (set) Token: 0x0600323A RID: 12858 RVA: 0x000F2B12 File Offset: 0x000F0F12
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

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x000F2B14 File Offset: 0x000F0F14
		public int ServerTimestepDelta
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000F2B17 File Offset: 0x000F0F17
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

		// Token: 0x0600323D RID: 12861 RVA: 0x000F2B39 File Offset: 0x000F0F39
		public void Shutdown()
		{
			this.LeaveCustomMatch();
			this.previousCrashDetector.UpdateStatus(PreviousCrashDetector.GameStatus.SafeShutdown);
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000F2B4E File Offset: 0x000F0F4E
		public void CloseAllConnections()
		{
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x000F2B50 File Offset: 0x000F0F50
		public bool LeaveMatch(Guid matchId)
		{
			return this.p2pServerMgr.LeaveMatch();
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000F2B5D File Offset: 0x000F0F5D
		public bool CustomMatchSetParams(SCustomLobbyParams cmparams)
		{
			this.p2pServerMgr.SetParams(cmparams);
			return true;
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000F2B6C File Offset: 0x000F0F6C
		public bool LeaveCustomMatch()
		{
			this.p2pServerMgr.OnLeftLobby();
			return true;
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000F2B7A File Offset: 0x000F0F7A
		public bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
		{
			this.p2pServerMgr.StartCustomMatch(this.customLobby.StageID, players, gameMode);
			return true;
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000F2B95 File Offset: 0x000F0F95
		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			this.p2pServerMgr.EnqueueMessage(queuedMessage);
			return true;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000F2BA5 File Offset: 0x000F0FA5
		public bool SendClientWinner(byte winningTeamMask)
		{
			this.p2pServerMgr.SendWinner(winningTeamMask);
			return true;
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000F2BB4 File Offset: 0x000F0FB4
		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return this.p2pServerMgr.LockInSelection(characterID, characterIndex, skinID, isRandom);
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000F2BC6 File Offset: 0x000F0FC6
		public bool StageLoaded()
		{
			return this.p2pServerMgr.StageLoaded();
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000F2BD3 File Offset: 0x000F0FD3
		public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
			this.p2pServerMgr.ListenForServerEvents<T>(callback);
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000F2BE1 File Offset: 0x000F0FE1
		public void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
			this.p2pServerMgr.StopListeningForServerEvents<T>(callback);
		}

		// Token: 0x0400235F RID: 9055
		private ulong m_sessionId;

		// Token: 0x04002360 RID: 9056
		private ulong m_accountId;

		// Token: 0x04002361 RID: 9057
		private string m_username;

		// Token: 0x04002362 RID: 9058
		private bool isInit;
	}
}
