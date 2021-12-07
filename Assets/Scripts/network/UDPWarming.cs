// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using Steamworks;
using System;
using System.Collections.Generic;

namespace network
{
	public class UDPWarming : IUDPWarming
	{
		private const int RESEND_TIME_MS = 200;

		private const int MIN_LATENCY = 750;

		private const int MESSAGES_REQUIRED = 5;

		private Dictionary<ulong, int> connectionsCompletedMessageCount = new Dictionary<ulong, int>();

		private Dictionary<ulong, double> timeSent = new Dictionary<ulong, double>();

		[Inject]
		public P2PServerMgr p2pServerMgr
		{
			get;
			set;
		}

		[Inject]
		public SteamManager steam
		{
			get;
			set;
		}

		[Inject]
		public ICustomLobbyController customLobbyController
		{
			get;
			set;
		}

		public bool IsAllConnectionsReady
		{
			get
			{
				foreach (KeyValuePair<ulong, int> current in this.connectionsCompletedMessageCount)
				{
					if (current.Value < 5)
					{
						return false;
					}
				}
				return true;
			}
		}

		public void BeginUdpWarm()
		{
			this.connectionsCompletedMessageCount.Clear();
			this.timeSent.Clear();
			foreach (LobbyPlayerData current in this.customLobbyController.Players.Values)
			{
				if (current.userID != this.steam.MySteamID().m_SteamID)
				{
					this.connectionsCompletedMessageCount[current.userID] = 0;
				}
			}
			foreach (KeyValuePair<ulong, int> current2 in this.connectionsCompletedMessageCount)
			{
				this.sendPing(current2.Key);
			}
		}

		private void sendPing(ulong target)
		{
			this.timeSent[target] = WTime.precisionTimeSinceStartup;
			UdpPingMsg udpPingMsg = new UdpPingMsg();
			udpPingMsg._targetUserID = target;
			udpPingMsg.senderId = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(udpPingMsg);
		}

		public void Reset()
		{
			this.connectionsCompletedMessageCount.Clear();
			this.timeSent.Clear();
		}

		public bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID)
		{
			if (msgId == 7)
			{
				UdpPingMsg udpPingMsg = new UdpPingMsg(buffer, bufferSize);
				udpPingMsg.DeserializeMsg();
				this.onUdpPing(udpPingMsg.senderId);
				return true;
			}
			if (msgId != 8)
			{
				return false;
			}
			UdpPongMsg udpPongMsg = new UdpPongMsg(buffer, bufferSize);
			udpPongMsg.DeserializeMsg();
			this.onUdpPong(udpPongMsg.senderId);
			return true;
		}

		private void onUdpPing(ulong senderId)
		{
			UdpPongMsg udpPongMsg = new UdpPongMsg();
			udpPongMsg._targetUserID = senderId;
			udpPongMsg.senderId = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(udpPongMsg);
		}

		private void onUdpPong(ulong senderId)
		{
			double num;
			if (this.timeSent.TryGetValue(senderId, out num))
			{
				double num2 = WTime.precisionTimeSinceStartup - num;
				if (num2 <= 750.0)
				{
					if (this.connectionsCompletedMessageCount.ContainsKey(senderId))
					{
						Dictionary<ulong, int> dictionary;
						(dictionary = this.connectionsCompletedMessageCount)[senderId] = dictionary[senderId] + 1;
					}
					else
					{
						this.connectionsCompletedMessageCount[senderId] = 1;
					}
				}
			}
		}

		public void Tick()
		{
			if (this.connectionsCompletedMessageCount.Count > 0)
			{
				foreach (KeyValuePair<ulong, int> current in this.connectionsCompletedMessageCount)
				{
					if (current.Value < 5)
					{
						ulong key = current.Key;
						double num = 0.0;
						if (!this.timeSent.TryGetValue(key, out num))
						{
							this.sendPing(key);
						}
						else
						{
							double num2 = WTime.precisionTimeSinceStartup - num;
							if (num2 >= 200.0)
							{
								this.sendPing(key);
							}
						}
					}
				}
			}
		}
	}
}
