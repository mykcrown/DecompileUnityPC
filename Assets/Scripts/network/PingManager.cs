// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using P2P;
using Steamworks;
using System;
using System.Collections.Generic;

namespace network
{
	public class PingManager : IPingManager
	{
		private BufferedBattleMsgs bufferedBattleMsgs = new BufferedBattleMsgs();

		private Dictionary<ulong, bool> pongsPending = new Dictionary<ulong, bool>();

		[Inject]
		public P2PHost p2pHost
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
		public SteamManager steam
		{
			get;
			set;
		}

		[Inject]
		public P2PClient p2pClient
		{
			get;
			set;
		}

		public long PingTimeMs
		{
			get;
			private set;
		}

		public long LatencyMs
		{
			get;
			private set;
		}

		private bool pongPending
		{
			get
			{
				foreach (KeyValuePair<ulong, bool> current in this.pongsPending)
				{
					if (current.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		private bool isPingExpired
		{
			get
			{
				return WTime.currentTimeMs - this.PingTimeMs >= 10000L;
			}
		}

		[PostConstruct]
		public void Init()
		{
			this.bufferedBattleMsgs.Init();
		}

		public void Ping()
		{
			if (!this.p2pServerMgr.HasReceivers || (this.pongPending && !this.isPingExpired))
			{
				return;
			}
			this.PingTimeMs = WTime.currentTimeMs;
			this.pongsPending.Clear();
			foreach (CSteamID current in this.p2pServerMgr.communicationIdBuffer)
			{
				this.pongsPending[current.m_SteamID] = true;
			}
			P2PPingMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPingMsg>();
			this.p2pServerMgr.EnqueueMessage(bufferedNetMessage);
		}

		private void onPingMsg()
		{
			P2PPongMsg bufferedNetMessage = this.bufferedBattleMsgs.GetBufferedNetMessage<P2PPongMsg>();
			bufferedNetMessage.senderSteamID = this.steam.MySteamID().m_SteamID;
			this.p2pServerMgr.EnqueueMessage(bufferedNetMessage);
		}

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

		public void Tick()
		{
			if (!this.p2pServerMgr.IsInBattle && WTime.currentTimeMs - this.PingTimeMs > 2000L)
			{
				this.Ping();
			}
		}
	}
}
