// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using Commerce;
using MatchMaking;
using network;
using System;
using System.Collections.Generic;

namespace IconsServer
{
	public class OfflineServerManager : IIconsServerAPI
	{
		public ulong AccountId
		{
			get
			{
				return 0uL;
			}
		}

		public ulong SessionId
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
				return Environment.UserName;
			}
		}

		public ulong Ping
		{
			get
			{
				return 0uL;
			}
		}

		public int ServerTimestepDelta
		{
			get
			{
				return 0;
			}
		}

		public bool UsingUDP
		{
			get
			{
				return false;
			}
		}

		public bool ConnectToAuth(string endPoint, uint port)
		{
			return false;
		}

		public bool CreateAccount(string wavedashEmail, string username, string password)
		{
			return false;
		}

		public bool CreateCustomMatch(SCustomLobbyParams cmparams)
		{
			return false;
		}

		public float CurrentAverageReceivedPacketSize()
		{
			throw new NotImplementedException();
		}

		public float CurrentAverageSentPacketSize()
		{
			throw new NotImplementedException();
		}

		public float CurrentReceivedPacketRate()
		{
			throw new NotImplementedException();
		}

		public float CurrentReceivedUdpPacketRate()
		{
			throw new NotImplementedException();
		}

		public float CurrentSentPacketRate()
		{
			throw new NotImplementedException();
		}

		public float CurrentUDPDropRate()
		{
			throw new NotImplementedException();
		}

		public float CurrentUDPOOORate()
		{
			throw new NotImplementedException();
		}

		public bool CustomMatchSetParams(SCustomLobbyParams cmparmas)
		{
			return false;
		}

		public bool DestroyCustomMatch()
		{
			return false;
		}

		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			return false;
		}

		public bool GetCatalog()
		{
			throw new NotImplementedException();
		}

		public bool GetCharacterXPInfo()
		{
			return false;
		}

		public bool GetPackagesForSale()
		{
			throw new NotImplementedException();
		}

		public bool GetPlayerInventory()
		{
			throw new NotImplementedException();
		}

		public bool GetPlayersCharactersXP()
		{
			return false;
		}

		public bool GetPlayersXP()
		{
			return false;
		}

		public bool GetPlayerXPInfo()
		{
			return false;
		}

		public UdpMatchStats GetUDPMatchStats()
		{
			throw new NotImplementedException();
		}

		public bool JoinPrivateCustomMatch(string lobbyName, string lobbyPass)
		{
			return false;
		}

		public bool LeaveCustomMatch()
		{
			return false;
		}

		public bool LeaveMatch(Guid matchId)
		{
			return false;
		}

		public bool LeaveQueue()
		{
			return false;
		}

		public void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
		}

		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return false;
		}

		public void Logout()
		{
		}

		public void NetworkPoll()
		{
		}

		public void Poll()
		{
		}

		public bool PurchasePackage(ulong packageId, ECurrencyType currencyType, ulong currencyAmount)
		{
			throw new NotImplementedException();
		}

		public bool OpenLootBox(ulong lootBoxId)
		{
			throw new NotImplementedException();
		}

		public bool UnlockToken(ulong tokenItemId, EItemType unlockItemType, long unlockItem)
		{
			throw new NotImplementedException();
		}

		public bool SetRatingVisibility(bool show)
		{
			throw new NotImplementedException();
		}

		public bool GetPlayerEquipment()
		{
			return false;
		}

		public bool EquipPlayerItem(ulong itemId, uint slotIndex)
		{
			return false;
		}

		public bool EquipCharacterItem(ulong itemId, uint slotIndex, ECharacterType characterType)
		{
			return false;
		}

		public bool NexusForfeitMatch()
		{
			return false;
		}

		public bool QueueForMatch(EQueueTypes queueType)
		{
			return false;
		}

		public void ReceiveNetworkMessages()
		{
		}

		public bool SendClientWinner(byte winningTeamMask)
		{
			return false;
		}

		public bool SendDesync(string matchId, byte[] bytes)
		{
			return false;
		}

		public bool SendMessage(NetMsgBase message, EServer server)
		{
			return false;
		}

		public void SendNetworkMessages()
		{
		}

		public bool SendPlayerFeedback(string feedback)
		{
			return false;
		}

		public void Shutdown()
		{
		}

		public bool StageLoaded()
		{
			return false;
		}

		public bool StartCustomMatch()
		{
			return false;
		}

		public bool Startup(int networkPollTimerMs = 10)
		{
			return false;
		}

		public void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent
		{
		}

		public void CloseAllConnections()
		{
		}

		public bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode)
		{
			throw new NotImplementedException();
		}
	}
}
