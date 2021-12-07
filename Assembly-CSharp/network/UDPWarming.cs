using System;
using System.Collections.Generic;
using BattleServer;
using Steamworks;

namespace network
{
	// Token: 0x0200083B RID: 2107
	public class UDPWarming : IUDPWarming
	{
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060034A9 RID: 13481 RVA: 0x000F81A9 File Offset: 0x000F65A9
		// (set) Token: 0x060034AA RID: 13482 RVA: 0x000F81B1 File Offset: 0x000F65B1
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060034AB RID: 13483 RVA: 0x000F81BA File Offset: 0x000F65BA
		// (set) Token: 0x060034AC RID: 13484 RVA: 0x000F81C2 File Offset: 0x000F65C2
		[Inject]
		public SteamManager steam { get; set; }

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x000F81CB File Offset: 0x000F65CB
		// (set) Token: 0x060034AE RID: 13486 RVA: 0x000F81D3 File Offset: 0x000F65D3
		[Inject]
		public ICustomLobbyController customLobbyController { get; set; }

		// Token: 0x060034AF RID: 13487 RVA: 0x000F81DC File Offset: 0x000F65DC
		public void BeginUdpWarm()
		{
			this.connectionsCompletedMessageCount.Clear();
			this.timeSent.Clear();
			foreach (LobbyPlayerData lobbyPlayerData in this.customLobbyController.Players.Values)
			{
				if (lobbyPlayerData.userID != this.steam.MySteamID().m_SteamID)
				{
					this.connectionsCompletedMessageCount[lobbyPlayerData.userID] = 0;
				}
			}
			foreach (KeyValuePair<ulong, int> keyValuePair in this.connectionsCompletedMessageCount)
			{
				this.sendPing(keyValuePair.Key);
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000F82D4 File Offset: 0x000F66D4
		private void sendPing(ulong target)
		{
			this.timeSent[target] = WTime.precisionTimeSinceStartup;
			UdpPingMsg udpPingMsg = new UdpPingMsg();
			udpPingMsg._targetUserID = target;
			udpPingMsg.senderId = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(udpPingMsg);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000F8325 File Offset: 0x000F6725
		public void Reset()
		{
			this.connectionsCompletedMessageCount.Clear();
			this.timeSent.Clear();
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060034B2 RID: 13490 RVA: 0x000F8340 File Offset: 0x000F6740
		public bool IsAllConnectionsReady
		{
			get
			{
				foreach (KeyValuePair<ulong, int> keyValuePair in this.connectionsCompletedMessageCount)
				{
					if (keyValuePair.Value < 5)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000F83AC File Offset: 0x000F67AC
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

		// Token: 0x060034B4 RID: 13492 RVA: 0x000F8408 File Offset: 0x000F6808
		private void onUdpPing(ulong senderId)
		{
			UdpPongMsg udpPongMsg = new UdpPongMsg();
			udpPongMsg._targetUserID = senderId;
			udpPongMsg.senderId = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(udpPongMsg);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000F8448 File Offset: 0x000F6848
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

		// Token: 0x060034B6 RID: 13494 RVA: 0x000F84BC File Offset: 0x000F68BC
		public void Tick()
		{
			if (this.connectionsCompletedMessageCount.Count > 0)
			{
				foreach (KeyValuePair<ulong, int> keyValuePair in this.connectionsCompletedMessageCount)
				{
					if (keyValuePair.Value < 5)
					{
						ulong key = keyValuePair.Key;
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

		// Token: 0x0400245C RID: 9308
		private const int RESEND_TIME_MS = 200;

		// Token: 0x0400245D RID: 9309
		private const int MIN_LATENCY = 750;

		// Token: 0x0400245E RID: 9310
		private const int MESSAGES_REQUIRED = 5;

		// Token: 0x04002462 RID: 9314
		private Dictionary<ulong, int> connectionsCompletedMessageCount = new Dictionary<ulong, int>();

		// Token: 0x04002463 RID: 9315
		private Dictionary<ulong, double> timeSent = new Dictionary<ulong, double>();
	}
}
