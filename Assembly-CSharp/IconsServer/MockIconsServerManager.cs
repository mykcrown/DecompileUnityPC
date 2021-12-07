using System;
using System.Collections.Generic;
using BattleServer;
using Commerce;
using MatchMaking;
using network;
using UnityEngine;

namespace IconsServer
{
	// Token: 0x020007F8 RID: 2040
	public class MockIconsServerManager : IIconsServerAPI
	{
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003259 RID: 12889 RVA: 0x000F2D19 File Offset: 0x000F1119
		// (set) Token: 0x0600325A RID: 12890 RVA: 0x000F2D21 File Offset: 0x000F1121
		[Inject]
		public IStageDataHelper stageDataHelper { get; set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x000F2D2A File Offset: 0x000F112A
		// (set) Token: 0x0600325C RID: 12892 RVA: 0x000F2D32 File Offset: 0x000F1132
		[Inject]
		public IEnterNewGame enterNewGame { get; set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x000F2D3B File Offset: 0x000F113B
		// (set) Token: 0x0600325E RID: 12894 RVA: 0x000F2D43 File Offset: 0x000F1143
		[Inject]
		public IMainThreadTimer timer { get; set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x000F2D4C File Offset: 0x000F114C
		public ulong SessionId
		{
			get
			{
				return 10UL;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x000F2D51 File Offset: 0x000F1151
		public ulong AccountId
		{
			get
			{
				return 10UL;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003261 RID: 12897 RVA: 0x000F2D56 File Offset: 0x000F1156
		public string Username
		{
			get
			{
				return "FakeUser";
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x000F2D5D File Offset: 0x000F115D
		public ulong Ping
		{
			get
			{
				return 0UL;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x000F2D61 File Offset: 0x000F1161
		public int ServerTimestepDelta
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x000F2D64 File Offset: 0x000F1164
		public bool UsingUDP
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x000F2D67 File Offset: 0x000F1167
		public bool Startup(int networkPollTimerMs = 10)
		{
			return true;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x000F2D6A File Offset: 0x000F116A
		public void Shutdown()
		{
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000F2D6C File Offset: 0x000F116C
		public bool SendDesync(string matchId, byte[] bytes)
		{
			Debug.Log("Sent Mock Desync");
			return true;
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x000F2D79 File Offset: 0x000F1179
		public bool SendPlayerFeedback(string feedback)
		{
			Debug.Log("Sent Mock Player Feedback");
			return true;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x000F2D86 File Offset: 0x000F1186
		public void Logout()
		{
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x000F2D88 File Offset: 0x000F1188
		public bool ConnectToAuth(string endPoint, uint port)
		{
			return true;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x000F2D8B File Offset: 0x000F118B
		public bool CreateAccount(string wavedashEmail, string username, string password)
		{
			return true;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x000F2D8E File Offset: 0x000F118E
		public bool LeaveMatch(Guid matchId)
		{
			return true;
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x000F2D91 File Offset: 0x000F1191
		public bool NexusForfeitMatch()
		{
			return true;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x000F2D94 File Offset: 0x000F1194
		private void onQueueFail()
		{
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x000F2D98 File Offset: 0x000F1198
		private void onMatchConnect()
		{
			int num = 1;
			List<StageID> legalStages = this.enterNewGame.GamePayload.stagePayloadData.legalStages;
			EIconStages[] array = new EIconStages[num];
			for (int i = 0; i < array.Length; i++)
			{
				if (legalStages[i] != StageID.Random)
				{
					array[i] = this.stageDataHelper.GetIconStageFromStageID(legalStages[i]);
				}
			}
			this.MatchID = Guid.NewGuid();
			List<SBasicMatchPlayerDesc> list = new List<SBasicMatchPlayerDesc>();
			list.Add(new SBasicMatchPlayerDesc
			{
				name = "FakePlayer1"
			});
			list.Add(new SBasicMatchPlayerDesc
			{
				name = this.Username
			});
			if (this.testType == MockIconsServerManager.TestType.TestingDoubleClients)
			{
				list.Add(new SBasicMatchPlayerDesc
				{
					name = "FakePlayer3"
				});
				list.Add(new SBasicMatchPlayerDesc
				{
					name = "FakePlayer4"
				});
			}
			else if (this.testType == MockIconsServerManager.TestType.Testing3PFFA || this.testType == MockIconsServerManager.TestType.Testing4PFFA)
			{
				list.Add(new SBasicMatchPlayerDesc
				{
					name = "FakePlayer3"
				});
				if (this.testType == MockIconsServerManager.TestType.Testing4PFFA)
				{
					list.Add(new SBasicMatchPlayerDesc
					{
						name = "FakePlayer4"
					});
				}
			}
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000F2EEA File Offset: 0x000F12EA
		public bool LeaveQueue()
		{
			return true;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000F2EED File Offset: 0x000F12ED
		public bool DestroyCustomMatch()
		{
			return true;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000F2EF0 File Offset: 0x000F12F0
		public bool CustomMatchSetParams(SCustomLobbyParams cmparmas)
		{
			return true;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000F2EF4 File Offset: 0x000F12F4
		public bool JoinPrivateCustomMatch(string lobbyName, string lobbyPass)
		{
			SBasicMatchPlayerDesc[] array = new SBasicMatchPlayerDesc[2];
			array[0] = new SBasicMatchPlayerDesc();
			array[0].name = "FakePlayer2";
			array[1] = new SBasicMatchPlayerDesc();
			array[1].name = this.Username;
			JoinCustomMatchEvent message = new JoinCustomMatchEvent(JoinCustomMatchEvent.EResult.Result_Ok, 0UL, array, LobbyGameMode.Stock, new EIconStages[]
			{
				EIconStages.Arena,
				EIconStages.Zenith,
				EIconStages.CombatLab
			});
			this.doBroadcast(message);
			return true;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000F2F5C File Offset: 0x000F135C
		public bool LeaveCustomMatch()
		{
			LeaveCustomMatchEvent message = new LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult.Result_Ok);
			this.doBroadcast(message);
			return true;
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000F2F78 File Offset: 0x000F1378
		private void delayStartMatch()
		{
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000F2F7A File Offset: 0x000F137A
		public bool StartCustomMatch()
		{
			return true;
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000F2F7D File Offset: 0x000F137D
		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return true;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000F2F80 File Offset: 0x000F1380
		public bool StageLoaded()
		{
			uint num = 3U;
			MatchBeginEvent message = new MatchBeginEvent(this.MatchID, WTime.currentTimeMs + (long)((ulong)(num * 1000U)), num);
			this.doBroadcast(message);
			return true;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000F2FB2 File Offset: 0x000F13B2
		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			return true;
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000F2FB8 File Offset: 0x000F13B8
		public bool SendClientWinner(byte winningTeamMask)
		{
			MatchResultsEvent message = new MatchResultsEvent(this.MatchID, winningTeamMask);
			this.doBroadcast(message);
			return true;
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000F2FDA File Offset: 0x000F13DA
		public bool SendMessage(NetMsgBase message, EServer server)
		{
			return true;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000F2FDD File Offset: 0x000F13DD
		public void Poll()
		{
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000F2FE0 File Offset: 0x000F13E0
		public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
			Type typeFromHandle = typeof(T);
			if (!this.m_messageHandlers.ContainsKey(typeFromHandle))
			{
				this.m_messageHandlers.Add(typeFromHandle, null);
			}
			Dictionary<Type, Action<ServerEvent>> messageHandlers;
			Type key;
			(messageHandlers = this.m_messageHandlers)[key = typeFromHandle] = (Action<ServerEvent>)Delegate.Combine(messageHandlers[key], callback);
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000F3038 File Offset: 0x000F1438
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

		// Token: 0x0600327F RID: 12927 RVA: 0x000F3084 File Offset: 0x000F1484
		private void doBroadcast(ServerEvent message)
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

		// Token: 0x06003280 RID: 12928 RVA: 0x000F30E2 File Offset: 0x000F14E2
		public void ReceiveNetworkMessages()
		{
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000F30E4 File Offset: 0x000F14E4
		public void SendNetworkMessages()
		{
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000F30E6 File Offset: 0x000F14E6
		public bool GetPlayersXP()
		{
			return true;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x000F30E9 File Offset: 0x000F14E9
		public bool GetPlayerXPInfo()
		{
			return true;
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000F30EC File Offset: 0x000F14EC
		public bool GetPlayersCharactersXP()
		{
			return true;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000F30EF File Offset: 0x000F14EF
		public bool GetCharacterXPInfo()
		{
			return true;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000F30F2 File Offset: 0x000F14F2
		public float CurrentUDPDropRate()
		{
			return 0f;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000F30F9 File Offset: 0x000F14F9
		public float CurrentUDPOOORate()
		{
			return 0f;
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000F3100 File Offset: 0x000F1500
		public float CurrentAverageReceivedPacketSize()
		{
			return 0f;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000F3107 File Offset: 0x000F1507
		public float CurrentAverageSentPacketSize()
		{
			return 0f;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000F310E File Offset: 0x000F150E
		public float CurrentSentPacketRate()
		{
			return 0f;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000F3115 File Offset: 0x000F1515
		public float CurrentReceivedPacketRate()
		{
			return 0f;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000F311C File Offset: 0x000F151C
		public float CurrentReceivedUdpPacketRate()
		{
			return 0f;
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000F3124 File Offset: 0x000F1524
		public UdpMatchStats GetUDPMatchStats()
		{
			return default(UdpMatchStats);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000F313A File Offset: 0x000F153A
		public bool GetCatalog()
		{
			return true;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x000F313D File Offset: 0x000F153D
		public bool GetPlayerInventory()
		{
			return true;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x000F3140 File Offset: 0x000F1540
		public bool GetPackagesForSale()
		{
			return true;
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x000F3143 File Offset: 0x000F1543
		public bool PurchasePackage(ulong packageId, ECurrencyType currencyType, ulong currencyAmount)
		{
			return true;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x000F3146 File Offset: 0x000F1546
		public bool OpenLootBox(ulong lootBoxId)
		{
			return true;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x000F3149 File Offset: 0x000F1549
		public bool UnlockToken(ulong tokenItemId, EItemType unlockItemType, long unlockItem)
		{
			return true;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000F314C File Offset: 0x000F154C
		public bool SetRatingVisibility(bool show)
		{
			return true;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000F314F File Offset: 0x000F154F
		public bool GetPlayerEquipment()
		{
			return true;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000F3152 File Offset: 0x000F1552
		public bool EquipPlayerItem(ulong itemId, uint slotIndex)
		{
			return true;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x000F3155 File Offset: 0x000F1555
		public bool EquipCharacterItem(ulong itemId, uint slotIndex, ECharacterType characterType)
		{
			return true;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x000F3158 File Offset: 0x000F1558
		public void CloseAllConnections()
		{
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x000F315A File Offset: 0x000F155A
		public bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400236A RID: 9066
		private MockIconsServerManager.TestType testType = MockIconsServerManager.TestType.Testing4PFFA;

		// Token: 0x0400236B RID: 9067
		private Guid MatchID;

		// Token: 0x0400236F RID: 9071
		private Dictionary<Type, Action<ServerEvent>> m_messageHandlers = new Dictionary<Type, Action<ServerEvent>>();

		// Token: 0x020007F9 RID: 2041
		private enum TestType
		{
			// Token: 0x04002371 RID: 9073
			Testing1v1,
			// Token: 0x04002372 RID: 9074
			Testing3PFFA,
			// Token: 0x04002373 RID: 9075
			Testing4PFFA,
			// Token: 0x04002374 RID: 9076
			TestingDoubleClients
		}
	}
}
