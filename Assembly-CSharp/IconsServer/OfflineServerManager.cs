using System;
using System.Collections.Generic;
using BattleServer;
using Commerce;
using MatchMaking;
using network;

namespace IconsServer
{
	// Token: 0x0200080C RID: 2060
	public class OfflineServerManager : IIconsServerAPI
	{
		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x000F337E File Offset: 0x000F177E
		public ulong AccountId
		{
			get
			{
				return 0UL;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060032CA RID: 13002 RVA: 0x000F3382 File Offset: 0x000F1782
		public ulong SessionId
		{
			get
			{
				return 0UL;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x000F3386 File Offset: 0x000F1786
		public string Username
		{
			get
			{
				return Environment.UserName;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x000F338D File Offset: 0x000F178D
		public ulong Ping
		{
			get
			{
				return 0UL;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000F3391 File Offset: 0x000F1791
		public int ServerTimestepDelta
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x000F3394 File Offset: 0x000F1794
		public bool UsingUDP
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x000F3397 File Offset: 0x000F1797
		public bool ConnectToAuth(string endPoint, uint port)
		{
			return false;
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000F339A File Offset: 0x000F179A
		public bool CreateAccount(string wavedashEmail, string username, string password)
		{
			return false;
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000F339D File Offset: 0x000F179D
		public bool CreateCustomMatch(SCustomLobbyParams cmparams)
		{
			return false;
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000F33A0 File Offset: 0x000F17A0
		public float CurrentAverageReceivedPacketSize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000F33A7 File Offset: 0x000F17A7
		public float CurrentAverageSentPacketSize()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000F33AE File Offset: 0x000F17AE
		public float CurrentReceivedPacketRate()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000F33B5 File Offset: 0x000F17B5
		public float CurrentReceivedUdpPacketRate()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000F33BC File Offset: 0x000F17BC
		public float CurrentSentPacketRate()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000F33C3 File Offset: 0x000F17C3
		public float CurrentUDPDropRate()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000F33CA File Offset: 0x000F17CA
		public float CurrentUDPOOORate()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000F33D1 File Offset: 0x000F17D1
		public bool CustomMatchSetParams(SCustomLobbyParams cmparmas)
		{
			return false;
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x000F33D4 File Offset: 0x000F17D4
		public bool DestroyCustomMatch()
		{
			return false;
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000F33D7 File Offset: 0x000F17D7
		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			return false;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000F33DA File Offset: 0x000F17DA
		public bool GetCatalog()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000F33E1 File Offset: 0x000F17E1
		public bool GetCharacterXPInfo()
		{
			return false;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000F33E4 File Offset: 0x000F17E4
		public bool GetPackagesForSale()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000F33EB File Offset: 0x000F17EB
		public bool GetPlayerInventory()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000F33F2 File Offset: 0x000F17F2
		public bool GetPlayersCharactersXP()
		{
			return false;
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000F33F5 File Offset: 0x000F17F5
		public bool GetPlayersXP()
		{
			return false;
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000F33F8 File Offset: 0x000F17F8
		public bool GetPlayerXPInfo()
		{
			return false;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000F33FB File Offset: 0x000F17FB
		public UdpMatchStats GetUDPMatchStats()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000F3402 File Offset: 0x000F1802
		public bool JoinPrivateCustomMatch(string lobbyName, string lobbyPass)
		{
			return false;
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000F3405 File Offset: 0x000F1805
		public bool LeaveCustomMatch()
		{
			return false;
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000F3408 File Offset: 0x000F1808
		public bool LeaveMatch(Guid matchId)
		{
			return false;
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000F340B File Offset: 0x000F180B
		public bool LeaveQueue()
		{
			return false;
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x000F340E File Offset: 0x000F180E
		public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x000F3410 File Offset: 0x000F1810
		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return false;
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x000F3413 File Offset: 0x000F1813
		public void Logout()
		{
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000F3415 File Offset: 0x000F1815
		public void NetworkPoll()
		{
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x000F3417 File Offset: 0x000F1817
		public void Poll()
		{
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x000F3419 File Offset: 0x000F1819
		public bool PurchasePackage(ulong packageId, ECurrencyType currencyType, ulong currencyAmount)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x000F3420 File Offset: 0x000F1820
		public bool OpenLootBox(ulong lootBoxId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000F3427 File Offset: 0x000F1827
		public bool UnlockToken(ulong tokenItemId, EItemType unlockItemType, long unlockItem)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x000F342E File Offset: 0x000F182E
		public bool SetRatingVisibility(bool show)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x000F3435 File Offset: 0x000F1835
		public bool GetPlayerEquipment()
		{
			return false;
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x000F3438 File Offset: 0x000F1838
		public bool EquipPlayerItem(ulong itemId, uint slotIndex)
		{
			return false;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x000F343B File Offset: 0x000F183B
		public bool EquipCharacterItem(ulong itemId, uint slotIndex, ECharacterType characterType)
		{
			return false;
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000F343E File Offset: 0x000F183E
		public bool NexusForfeitMatch()
		{
			return false;
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000F3441 File Offset: 0x000F1841
		public bool QueueForMatch(EQueueTypes queueType)
		{
			return false;
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000F3444 File Offset: 0x000F1844
		public void ReceiveNetworkMessages()
		{
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x000F3446 File Offset: 0x000F1846
		public bool SendClientWinner(byte winningTeamMask)
		{
			return false;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000F3449 File Offset: 0x000F1849
		public bool SendDesync(string matchId, byte[] bytes)
		{
			return false;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000F344C File Offset: 0x000F184C
		public bool SendMessage(NetMsgBase message, EServer server)
		{
			return false;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000F344F File Offset: 0x000F184F
		public void SendNetworkMessages()
		{
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000F3451 File Offset: 0x000F1851
		public bool SendPlayerFeedback(string feedback)
		{
			return false;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000F3454 File Offset: 0x000F1854
		public void Shutdown()
		{
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x000F3456 File Offset: 0x000F1856
		public bool StageLoaded()
		{
			return false;
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x000F3459 File Offset: 0x000F1859
		public bool StartCustomMatch()
		{
			return false;
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x000F345C File Offset: 0x000F185C
		public bool Startup(int networkPollTimerMs = 10)
		{
			return false;
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x000F345F File Offset: 0x000F185F
		public void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000F3461 File Offset: 0x000F1861
		public void CloseAllConnections()
		{
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000F3463 File Offset: 0x000F1863
		public bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
		{
			throw new NotImplementedException();
		}
	}
}
