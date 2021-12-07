using System;
using System.Collections.Generic;
using P2P;
using Steamworks;

namespace network
{
	// Token: 0x02000839 RID: 2105
	public class TimeSynchronizer : ITimeSynchronizer
	{
		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x000F7BA0 File Offset: 0x000F5FA0
		// (set) Token: 0x0600348E RID: 13454 RVA: 0x000F7BA8 File Offset: 0x000F5FA8
		[Inject]
		public P2PServerMgr p2pServerMgr { get; set; }

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x000F7BB1 File Offset: 0x000F5FB1
		// (set) Token: 0x06003490 RID: 13456 RVA: 0x000F7BB9 File Offset: 0x000F5FB9
		[Inject]
		public SteamManager steam { get; set; }

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x000F7BC2 File Offset: 0x000F5FC2
		// (set) Token: 0x06003492 RID: 13458 RVA: 0x000F7BCA File Offset: 0x000F5FCA
		[Inject]
		public IUIAdapter uiAdapter { get; set; }

		// Token: 0x06003493 RID: 13459 RVA: 0x000F7BD3 File Offset: 0x000F5FD3
		public void Reset()
		{
			this.timeOffset = 0L;
			this.results.Clear();
			this.timesyncRequests.Clear();
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000F7BF3 File Offset: 0x000F5FF3
		public void SetUpdateCallback(Action updateCallback)
		{
			this.updateCallback = updateCallback;
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06003495 RID: 13461 RVA: 0x000F7BFC File Offset: 0x000F5FFC
		public bool IsSynchronizationComplete
		{
			get
			{
				if (this.p2pServerMgr.IsHost)
				{
					foreach (KeyValuePair<ulong, List<long>> keyValuePair in this.results)
					{
						if (keyValuePair.Value.Count < this.RESULT_COUNT)
						{
							return false;
						}
					}
					return true;
				}
				return true;
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000F7C84 File Offset: 0x000F6084
		public long GetTimeOffsetMs()
		{
			if (this.p2pServerMgr.IsHost)
			{
				return 0L;
			}
			return this.timeOffset;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000F7CA0 File Offset: 0x000F60A0
		public void KeepOnlyThesePlayers(List<SteamManager.SteamLobbyData> list)
		{
			List<ulong> list2 = new List<ulong>();
			foreach (SteamManager.SteamLobbyData steamLobbyData in list)
			{
				list2.Add(steamLobbyData.steamID.m_SteamID);
			}
			List<ulong> list3 = new List<ulong>();
			foreach (ulong item in this.results.Keys)
			{
				if (!list2.Contains(item) && !list3.Contains(item))
				{
					list3.Add(item);
				}
			}
			foreach (ulong item2 in this.timesyncRequests.Keys)
			{
				if (!list2.Contains(item2) && !list3.Contains(item2))
				{
					list3.Add(item2);
				}
			}
			foreach (ulong steamID in list3)
			{
				this.resetPlayer(steamID);
			}
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000F7E34 File Offset: 0x000F6234
		private void resetPlayer(ulong steamID)
		{
			this.results.Remove(steamID);
			this.timesyncRequests.Remove(steamID);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000F7E50 File Offset: 0x000F6250
		public void Tick()
		{
			if (this.p2pServerMgr.IsHost && this.uiAdapter.CurrentScreen != ScreenType.BattleGUI)
			{
				foreach (CSteamID steamID in this.p2pServerMgr.communicationIdBuffer)
				{
					this.checkTimeSync(steamID);
				}
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000F7ED4 File Offset: 0x000F62D4
		private void checkTimeSync(CSteamID steamID)
		{
			if (!this.isRequestPending(steamID.m_SteamID))
			{
				List<long> list;
				this.results.TryGetValue(steamID.m_SteamID, out list);
				if (list == null || list.Count <= this.RESULT_COUNT)
				{
					this.timesyncRequests[steamID.m_SteamID] = WTime.precisionTimeSinceStartup;
					P2PTimesyncMsg p2PTimesyncMsg = new P2PTimesyncMsg();
					p2PTimesyncMsg.timeOffset = this.getTimeOffeset(steamID.m_SteamID);
					p2PTimesyncMsg.sendTimeLocalMs = (long)WTime.precisionTimeSinceStartup;
					p2PTimesyncMsg._targetUserID = steamID.m_SteamID;
					this.p2pServerMgr.EnqueueMessage(p2PTimesyncMsg);
				}
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000F7F78 File Offset: 0x000F6378
		private bool isRequestPending(ulong steamID)
		{
			if (this.timesyncRequests.ContainsKey(steamID))
			{
				double num = WTime.precisionTimeSinceStartup - this.timesyncRequests[steamID];
				if (num < 5000.0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000F7FBC File Offset: 0x000F63BC
		private long getTimeOffeset(ulong steamID)
		{
			long result = 0L;
			List<long> list;
			this.results.TryGetValue(steamID, out list);
			if (list != null && list.Count > 0)
			{
				int index = list.Count / 2;
				result = list[index];
			}
			return result;
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000F8000 File Offset: 0x000F6400
		public bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID)
		{
			if (msgId == 9)
			{
				P2PTimesyncMsg p2PTimesyncMsg = new P2PTimesyncMsg(buffer, bufferSize);
				p2PTimesyncMsg.DeserializeMsg();
				this.onTimesyncMsg(p2PTimesyncMsg.timeOffset, p2PTimesyncMsg.sendTimeLocalMs);
				return true;
			}
			if (msgId != 10)
			{
				return false;
			}
			P2PTimesyncResponseMsg p2PTimesyncResponseMsg = new P2PTimesyncResponseMsg(buffer, bufferSize);
			p2PTimesyncResponseMsg.DeserializeMsg();
			this.onTimesyncResponseMsg(p2PTimesyncResponseMsg.senderSteamID, p2PTimesyncResponseMsg.localTimeMs);
			return true;
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000F8068 File Offset: 0x000F6468
		private void onTimesyncMsg(long timeOffset, long sendTimeLocalMs)
		{
			this.timeOffset = timeOffset;
			P2PTimesyncResponseMsg p2PTimesyncResponseMsg = new P2PTimesyncResponseMsg();
			p2PTimesyncResponseMsg.senderSteamID = this.steam.MySteamID().m_SteamID;
			p2PTimesyncResponseMsg.localTimeMs = (long)WTime.precisionTimeSinceStartup;
			this.p2pServerMgr.EnqueueMessage(p2PTimesyncResponseMsg);
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000F80B4 File Offset: 0x000F64B4
		private void onTimesyncResponseMsg(ulong senderSteamID, long senderLocalTimeMs)
		{
			if (this.timesyncRequests.ContainsKey(senderSteamID))
			{
				double num = WTime.precisionTimeSinceStartup - this.timesyncRequests[senderSteamID];
				this.timesyncRequests.Remove(senderSteamID);
				if (!this.results.ContainsKey(senderSteamID))
				{
					this.results[senderSteamID] = new List<long>();
				}
				long num2 = senderLocalTimeMs - (long)WTime.precisionTimeSinceStartup;
				long item = num2 + (long)(num / 2.0);
				if (num <= 500.0)
				{
					this.results[senderSteamID].Add(item);
					this.results[senderSteamID].Sort(new Comparison<long>(this.sortFn));
					if (this.updateCallback != null)
					{
						this.updateCallback();
					}
				}
			}
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000F8185 File Offset: 0x000F6585
		private int sortFn(long a, long b)
		{
			return (int)(a - b);
		}

		// Token: 0x04002454 RID: 9300
		private int RESULT_COUNT = 20;

		// Token: 0x04002458 RID: 9304
		private Dictionary<ulong, double> timesyncRequests = new Dictionary<ulong, double>();

		// Token: 0x04002459 RID: 9305
		private Dictionary<ulong, List<long>> results = new Dictionary<ulong, List<long>>();

		// Token: 0x0400245A RID: 9306
		private long timeOffset;

		// Token: 0x0400245B RID: 9307
		private Action updateCallback;
	}
}
