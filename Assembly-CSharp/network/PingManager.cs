using System;
using System.Collections.Generic;
using BattleServer;
using P2P;
using Steamworks;

namespace network
{
	// Token: 0x02000837 RID: 2103
	public class PingManager : IPingManager
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003473 RID: 13427 RVA: 0x000F78AA File Offset: 0x000F5CAA
		// (set) Token: 0x06003474 RID: 13428 RVA: 0x000F78B2 File Offset: 0x000F5CB2
		[Inject]
		public P2PHost p2pHost { get; set; }

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003475 RID: 13429 RVA: 0x000F78BB File Offset: 0x000F5CBB
		// (set) Token: 0x06003476 RID: 13430 RVA: 0x000F78C3 File Offset: 0x000F5CC3
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003477 RID: 13431 RVA: 0x000F78CC File Offset: 0x000F5CCC
		// (set) Token: 0x06003478 RID: 13432 RVA: 0x000F78D4 File Offset: 0x000F5CD4
		[Inject]
		public SteamManager steam { get; set; }

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06003479 RID: 13433 RVA: 0x000F78DD File Offset: 0x000F5CDD
		// (set) Token: 0x0600347A RID: 13434 RVA: 0x000F78E5 File Offset: 0x000F5CE5
		[Inject]
		public P2PClient p2pClient { get; set; }

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x000F78EE File Offset: 0x000F5CEE
		// (set) Token: 0x0600347C RID: 13436 RVA: 0x000F78F6 File Offset: 0x000F5CF6
		public long PingTimeMs { get; private set; }

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x000F78FF File Offset: 0x000F5CFF
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x000F7907 File Offset: 0x000F5D07
		public long LatencyMs { get; private set; }

		// Token: 0x0600347F RID: 13439 RVA: 0x000F7910 File Offset: 0x000F5D10
		[PostConstruct]
		public void Init()
		{
			this.bufferedBattleMsgs.Init();
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x06003480 RID: 13440 RVA: 0x000F7920 File Offset: 0x000F5D20
		private bool pongPending
		{
			get
			{
				foreach (KeyValuePair<ulong, bool> keyValuePair in this.pongsPending)
				{
					if (keyValuePair.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x000F798C File Offset: 0x000F5D8C
		public void Ping()
		{
			if (!this.p2pServerMgr.HasReceivers || (this.pongPending && !this.isPingExpired))
			{
				return;
			}
			this.PingTimeMs = WTime.currentTimeMs;
			this.pongsPending.Clear();
			foreach (CSteamID csteamID in this.p2pServerMgr.communicationIdBuffer)
			{
				this.pongsPending[csteamID.m_SteamID] = true;
			}
			P2PPingMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPingMsg>();
			this.p2pServerMgr.EnqueueMessage(bufferedNetMessage);
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x06003482 RID: 13442 RVA: 0x000F7A50 File Offset: 0x000F5E50
		private bool isPingExpired
		{
			get
			{
				return WTime.currentTimeMs - this.PingTimeMs >= 10000L;
			}
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x000F7A6C File Offset: 0x000F5E6C
		private void onPingMsg()
		{
			P2PPongMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPongMsg>();
			bufferedNetMessage.senderSteamID = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(bufferedNetMessage);
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000F7AAB File Offset: 0x000F5EAB
		private void onPongMsg(ulong senderSteamID)
		{
			if (!this.pongPending)
			{
				return;
			}
			this.LatencyMs = WTime.currentTimeMs - this.PingTimeMs;
			if (this.pongsPending.ContainsKey(senderSteamID))
			{
				this.pongsPending[senderSteamID] = false;
			}
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000F7AEC File Offset: 0x000F5EEC
		public bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID)
		{
			if (msgId == 5)
			{
				P2PPingMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPingMsg>();
				bufferedNetMessage.DeserializeToBuffer(buffer, bufferSize);
				this.onPingMsg();
				return true;
			}
			if (msgId != 6)
			{
				return false;
			}
			P2PPongMsg bufferedNetMessage2 = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPongMsg>();
			bufferedNetMessage2.DeserializeToBuffer(buffer, bufferSize);
			this.onPongMsg(bufferedNetMessage2.senderSteamID);
			return true;
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000F7B4B File Offset: 0x000F5F4B
		public void Tick()
		{
			if (!this.p2pServerMgr.IsInBattle && WTime.currentTimeMs - this.PingTimeMs > 2000L)
			{
				this.Ping();
			}
		}

		// Token: 0x04002450 RID: 9296
		private BufferedBattleMsgs bufferedBattleMsgs = new BufferedBattleMsgs();

		// Token: 0x04002451 RID: 9297
		private Dictionary<ulong, bool> pongsPending = new Dictionary<ulong, bool>();
	}
}
