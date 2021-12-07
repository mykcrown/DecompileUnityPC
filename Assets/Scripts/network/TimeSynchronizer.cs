// Decompile from assembly: Assembly-CSharp.dll

using P2P;
using Steamworks;
using System;
using System.Collections.Generic;

namespace network
{
	public class TimeSynchronizer : ITimeSynchronizer
	{
		private int RESULT_COUNT = 20;

		private Dictionary<ulong, double> timesyncRequests = new Dictionary<ulong, double>();

		private Dictionary<ulong, List<long>> results = new Dictionary<ulong, List<long>>();

		private long timeOffset;

		private Action updateCallback;

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
		public IUIAdapter uiAdapter
		{
			get;
			set;
		}

		public bool IsSynchronizationComplete
		{
			get
			{
				if (this.p2pServerMgr.IsHost)
				{
					foreach (KeyValuePair<ulong, List<long>> current in this.results)
					{
						if (current.Value.Count < this.RESULT_COUNT)
						{
							return false;
						}
					}
					return true;
				}
				return true;
			}
		}

		public void Reset()
		{
			this.timeOffset = 0L;
			this.results.Clear();
			this.timesyncRequests.Clear();
		}

		public void SetUpdateCallback(Action updateCallback)
		{
			this.updateCallback = updateCallback;
		}

		public long GetTimeOffsetMs()
		{
			if (this.p2pServerMgr.IsHost)
			{
				return 0L;
			}
			return this.timeOffset;
		}

		public void KeepOnlyThesePlayers(List<SteamManager.SteamLobbyData> list)
		{
			List<ulong> list2 = new List<ulong>();
			foreach (SteamManager.SteamLobbyData current in list)
			{
				list2.Add(current.steamID.m_SteamID);
			}
			List<ulong> list3 = new List<ulong>();
			foreach (ulong current2 in this.results.Keys)
			{
				if (!list2.Contains(current2) && !list3.Contains(current2))
				{
					list3.Add(current2);
				}
			}
			foreach (ulong current3 in this.timesyncRequests.Keys)
			{
				if (!list2.Contains(current3) && !list3.Contains(current3))
				{
					list3.Add(current3);
				}
			}
			foreach (ulong current4 in list3)
			{
				this.resetPlayer(current4);
			}
		}

		private void resetPlayer(ulong steamID)
		{
			this.results.Remove(steamID);
			this.timesyncRequests.Remove(steamID);
		}

		public void Tick()
		{
			if (this.p2pServerMgr.IsHost && this.uiAdapter.CurrentScreen != ScreenType.BattleGUI)
			{
				foreach (CSteamID current in this.p2pServerMgr.communicationIdBuffer)
				{
					this.checkTimeSync(current);
				}
			}
		}

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

		private void onTimesyncMsg(long timeOffset, long sendTimeLocalMs)
		{
			this.timeOffset = timeOffset;
			P2PTimesyncResponseMsg p2PTimesyncResponseMsg = new P2PTimesyncResponseMsg();
			p2PTimesyncResponseMsg.senderSteamID = this.steam.MySteamID().m_SteamID;
			p2PTimesyncResponseMsg.localTimeMs = (long)WTime.precisionTimeSinceStartup;
			this.p2pServerMgr.EnqueueMessage(p2PTimesyncResponseMsg);
		}

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

		private int sortFn(long a, long b)
		{
			return (int)(a - b);
		}
	}
}
