// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using System;
using UnityEngine;

public class TimestepSynchronizer : ITimestepSynchronizer
{
	private struct TimeKeeperData
	{
		public bool syncronizationComplete;

		public int[] compensationOffsets;

		public int trackerLength;

		public int calculatedLatencyMs;

		public int MessageLatencyMs;
	}

	public static readonly int COMPENSATION_TRACKER_LENGTH = 100;

	public static readonly double TIMESYNC_INTERVAL = 30.0;

	private int ownerClientId;

	private ITimekeeper timeKeeper;

	private TimestepSynchronizer.TimeKeeperData[] playerTimesyncs;

	private double lastSendTime;

	public int CompensationOffsetMs
	{
		get;
		private set;
	}

	public bool SyncronizationComplete
	{
		get
		{
			for (int i = 0; i < this.playerTimesyncs.Length; i++)
			{
				if (i != this.ownerClientId && this.playerTimesyncs[i].trackerLength < TimestepSynchronizer.COMPENSATION_TRACKER_LENGTH)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool AllRemoteClientsSyncronized
	{
		get;
		private set;
	}

	public bool ShouldSend
	{
		get
		{
			return WTime.precisionTimeSinceStartup - this.lastSendTime > TimestepSynchronizer.TIMESYNC_INTERVAL;
		}
	}

	public TimestepSynchronizer(ITimekeeper timekeeper, int clientId, int numClients)
	{
		this.timeKeeper = timekeeper;
		this.CompensationOffsetMs = 0;
		this.AllRemoteClientsSyncronized = false;
		this.lastSendTime = 0.0;
		this.ownerClientId = clientId;
		this.playerTimesyncs = new TimestepSynchronizer.TimeKeeperData[numClients];
		for (int i = 0; i < numClients; i++)
		{
			this.playerTimesyncs[i].compensationOffsets = new int[TimestepSynchronizer.COMPENSATION_TRACKER_LENGTH];
		}
	}

	public int MessageLatencyMsTo(int clientId)
	{
		return (this.playerTimesyncs == null) ? 0 : this.playerTimesyncs[clientId].MessageLatencyMs;
	}

	private void updateMessageLatency(int clientID, int latency, int remoteLatencyMs)
	{
		if (this.playerTimesyncs[clientID].trackerLength < TimestepSynchronizer.COMPENSATION_TRACKER_LENGTH)
		{
			this.playerTimesyncs[clientID].MessageLatencyMs = latency;
			this.playerTimesyncs[clientID].calculatedLatencyMs = (latency + remoteLatencyMs) / 2;
			int num = latency - this.playerTimesyncs[clientID].calculatedLatencyMs;
			int trackerLength = this.playerTimesyncs[clientID].trackerLength;
			this.playerTimesyncs[clientID].compensationOffsets[trackerLength] = num;
			TimestepSynchronizer.TimeKeeperData[] expr_89_cp_0 = this.playerTimesyncs;
			expr_89_cp_0[clientID].trackerLength = expr_89_cp_0[clientID].trackerLength + 1;
			if (this.SyncronizationComplete)
			{
				this.updateTracking();
			}
		}
	}

	private void checkAllRemoteClientsSyncronized()
	{
		this.AllRemoteClientsSyncronized = true;
		for (int i = 0; i < this.playerTimesyncs.Length; i++)
		{
			if (i != this.ownerClientId && !this.playerTimesyncs[i].syncronizationComplete)
			{
				this.AllRemoteClientsSyncronized = false;
			}
		}
	}

	public void RecordTimestepSyncEvent(int clientID, long remoteInputMs, int remoteLatencyMs, bool remoteSyncronizationComplete)
	{
		if (this.playerTimesyncs == null)
		{
			return;
		}
		if (BattleConnection.StartReceiveTime != DateTime.MinValue)
		{
			DateTime utcNow = DateTime.UtcNow;
			double totalMilliseconds = (utcNow - BattleConnection.StartReceiveTime).TotalMilliseconds;
			if (totalMilliseconds > 10.0)
			{
				UnityEngine.Debug.LogError("Took more than " + totalMilliseconds + " ms to process receipt");
			}
		}
		long num = (long)this.timeKeeper.MsSinceStart;
		int latency = (int)(num - remoteInputMs);
		this.updateMessageLatency(clientID, latency, remoteLatencyMs);
		if (remoteSyncronizationComplete && this.playerTimesyncs[clientID].syncronizationComplete != remoteSyncronizationComplete)
		{
			this.playerTimesyncs[clientID].syncronizationComplete = remoteSyncronizationComplete;
			this.checkAllRemoteClientsSyncronized();
		}
	}

	public void Reset()
	{
		this.CompensationOffsetMs = 0;
		for (int i = 0; i < this.playerTimesyncs.Length; i++)
		{
			this.playerTimesyncs[i].trackerLength = 0;
		}
	}

	private void updateTracking()
	{
		int num = -2147483648;
		for (int i = 0; i < this.playerTimesyncs.Length; i++)
		{
			if (i != this.ownerClientId)
			{
				Array.Sort<int>(this.playerTimesyncs[i].compensationOffsets);
				int num2 = this.playerTimesyncs[i].compensationOffsets[(TimestepSynchronizer.COMPENSATION_TRACKER_LENGTH - 1) / 2];
				if (num2 > num)
				{
					num = num2;
				}
			}
		}
		this.CompensationOffsetMs = num;
	}

	public void OnSend()
	{
		this.lastSendTime = WTime.precisionTimeSinceStartup;
	}
}
