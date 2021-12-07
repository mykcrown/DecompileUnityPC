// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using Commerce;
using MatchMaking;
using network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IconsServer
{
	public class MockIconsServerManager : IIconsServerAPI
	{
		private enum TestType
		{
			Testing1v1,
			Testing3PFFA,
			Testing4PFFA,
			TestingDoubleClients
		}

		private MockIconsServerManager.TestType testType = MockIconsServerManager.TestType.Testing4PFFA;

		private Guid MatchID;

		private Dictionary<Type, Action<ServerEvent>> m_messageHandlers = new Dictionary<Type, Action<ServerEvent>>();

		[Inject]
		public IStageDataHelper stageDataHelper
		{
			get;
			set;
		}

		[Inject]
		public IEnterNewGame enterNewGame
		{
			get;
			set;
		}

		[Inject]
		public IMainThreadTimer timer
		{
			get;
			set;
		}

		public ulong SessionId
		{
			get
			{
				return 10uL;
			}
		}

		public ulong AccountId
		{
			get
			{
				return 10uL;
			}
		}

		public string Username
		{
			get
			{
				return "FakeUser";
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

		public bool Startup(int networkPollTimerMs = 10)
		{
			return true;
		}

		public void Shutdown()
		{
		}

		public bool SendDesync(string matchId, byte[] bytes)
		{
			UnityEngine.Debug.Log("Sent Mock Desync");
			return true;
		}

		public bool SendPlayerFeedback(string feedback)
		{
			UnityEngine.Debug.Log("Sent Mock Player Feedback");
			return true;
		}

		public void Logout()
		{
		}

		public bool ConnectToAuth(string endPoint, uint port)
		{
			return true;
		}

		public bool CreateAccount(string wavedashEmail, string username, string password)
		{
			return true;
		}

		public bool LeaveMatch(Guid matchId)
		{
			return true;
		}

		public bool NexusForfeitMatch()
		{
			return true;
		}

		private void onQueueFail()
		{
		}

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

		public bool LeaveQueue()
		{
			return true;
		}

		public bool DestroyCustomMatch()
		{
			return true;
		}

		public bool CustomMatchSetParams(SCustomLobbyParams cmparmas)
		{
			return true;
		}

		public bool JoinPrivateCustomMatch(string lobbyName, string lobbyPass)
		{
			SBasicMatchPlayerDesc[] array = new SBasicMatchPlayerDesc[2];
			array[0] = new SBasicMatchPlayerDesc();
			array[0].name = "FakePlayer2";
			array[1] = new SBasicMatchPlayerDesc();
			array[1].name = this.Username;
			JoinCustomMatchEvent message = new JoinCustomMatchEvent(JoinCustomMatchEvent.EResult.Result_Ok, 0uL, array, LobbyGameMode.Stock, new EIconStages[]
			{
				EIconStages.Arena,
				EIconStages.Zenith,
				EIconStages.CombatLab
			});
			this.doBroadcast(message);
			return true;
		}

		public bool LeaveCustomMatch()
		{
			LeaveCustomMatchEvent message = new LeaveCustomMatchEvent(LeaveCustomMatchEvent.EResult.Result_Ok);
			this.doBroadcast(message);
			return true;
		}

		private void delayStartMatch()
		{
		}

		public bool StartCustomMatch()
		{
			return true;
		}

		public bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom)
		{
			return true;
		}

		public bool StageLoaded()
		{
			uint num = 3u;
			MatchBeginEvent message = new MatchBeginEvent(this.MatchID, WTime.currentTimeMs + (long)((ulong)(num * 1000u)), num);
			this.doBroadcast(message);
			return true;
		}

		public bool EnqueueMessage(INetMsg queuedMessage)
		{
			return true;
		}

		public bool SendClientWinner(byte winningTeamMask)
		{
			MatchResultsEvent message = new MatchResultsEvent(this.MatchID, winningTeamMask);
			this.doBroadcast(message);
			return true;
		}

		public bool SendMessage(NetMsgBase message, EServer server)
		{
			return true;
		}

		public void Poll()
		{
		}

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

		public void ReceiveNetworkMessages()
		{
		}

		public void SendNetworkMessages()
		{
		}

		public bool GetPlayersXP()
		{
			return true;
		}

		public bool GetPlayerXPInfo()
		{
			return true;
		}

		public bool GetPlayersCharactersXP()
		{
			return true;
		}

		public bool GetCharacterXPInfo()
		{
			return true;
		}

		public float CurrentUDPDropRate()
		{
			return 0f;
		}

		public float CurrentUDPOOORate()
		{
			return 0f;
		}

		public float CurrentAverageReceivedPacketSize()
		{
			return 0f;
		}

		public float CurrentAverageSentPacketSize()
		{
			return 0f;
		}

		public float CurrentSentPacketRate()
		{
			return 0f;
		}

		public float CurrentReceivedPacketRate()
		{
			return 0f;
		}

		public float CurrentReceivedUdpPacketRate()
		{
			return 0f;
		}

		public UdpMatchStats GetUDPMatchStats()
		{
			return default(UdpMatchStats);
		}

		public bool GetCatalog()
		{
			return true;
		}

		public bool GetPlayerInventory()
		{
			return true;
		}

		public bool GetPackagesForSale()
		{
			return true;
		}

		public bool PurchasePackage(ulong packageId, ECurrencyType currencyType, ulong currencyAmount)
		{
			return true;
		}

		public bool OpenLootBox(ulong lootBoxId)
		{
			return true;
		}

		public bool UnlockToken(ulong tokenItemId, EItemType unlockItemType, long unlockItem)
		{
			return true;
		}

		public bool SetRatingVisibility(bool show)
		{
			return true;
		}

		public bool GetPlayerEquipment()
		{
			return true;
		}

		public bool EquipPlayerItem(ulong itemId, uint slotIndex)
		{
			return true;
		}

		public bool EquipCharacterItem(ulong itemId, uint slotIndex, ECharacterType characterType)
		{
			return true;
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
