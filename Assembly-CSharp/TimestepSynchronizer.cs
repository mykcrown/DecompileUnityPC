using System;
using BattleServer;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class TimestepSynchronizer : ITimestepSynchronizer
{
	// Token: 0x0600300F RID: 12303 RVA: 0x000EF374 File Offset: 0x000ED774
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

	// Token: 0x06003010 RID: 12304 RVA: 0x000EF3EB File Offset: 0x000ED7EB
	public int MessageLatencyMsTo(int clientId)
	{
		return (this.playerTimesyncs == null) ? 0 : this.playerTimesyncs[clientId].MessageLatencyMs;
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06003011 RID: 12305 RVA: 0x000EF40F File Offset: 0x000ED80F
	// (set) Token: 0x06003012 RID: 12306 RVA: 0x000EF417 File Offset: 0x000ED817
	public int CompensationOffsetMs { get; private set; }

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06003013 RID: 12307 RVA: 0x000EF420 File Offset: 0x000ED820
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

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x06003014 RID: 12308 RVA: 0x000EF470 File Offset: 0x000ED870
	// (set) Token: 0x06003015 RID: 12309 RVA: 0x000EF478 File Offset: 0x000ED878
	public bool AllRemoteClientsSyncronized { get; private set; }

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06003016 RID: 12310 RVA: 0x000EF481 File Offset: 0x000ED881
	public bool ShouldSend
	{
		get
		{
			return WTime.precisionTimeSinceStartup - this.lastSendTime > TimestepSynchronizer.TIMESYNC_INTERVAL;
		}
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x000EF498 File Offset: 0x000ED898
	private void updateMessageLatency(int clientID, int latency, int remoteLatencyMs)
	{
		if (this.playerTimesyncs[clientID].trackerLength < TimestepSynchronizer.COMPENSATION_TRACKER_LENGTH)
		{
			this.playerTimesyncs[clientID].MessageLatencyMs = latency;
			this.playerTimesyncs[clientID].calculatedLatencyMs = (latency + remoteLatencyMs) / 2;
			int num = latency - this.playerTimesyncs[clientID].calculatedLatencyMs;
			int trackerLength = this.playerTimesyncs[clientID].trackerLength;
			this.playerTimesyncs[clientID].compensationOffsets[trackerLength] = num;
			TimestepSynchronizer.TimeKeeperData[] array = this.playerTimesyncs;
			array[clientID].trackerLength = array[clientID].trackerLength + 1;
			if (this.SyncronizationComplete)
			{
				this.updateTracking();
			}
		}
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000EF54C File Offset: 0x000ED94C
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

	// Token: 0x06003019 RID: 12313 RVA: 0x000EF5A4 File Offset: 0x000ED9A4
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
				Debug.LogError("Took more than " + totalMilliseconds + " ms to process receipt");
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

	// Token: 0x0600301A RID: 12314 RVA: 0x000EF66C File Offset: 0x000EDA6C
	public void Reset()
	{
		this.CompensationOffsetMs = 0;
		for (int i = 0; i < this.playerTimesyncs.Length; i++)
		{
			this.playerTimesyncs[i].trackerLength = 0;
		}
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000EF6AC File Offset: 0x000EDAAC
	private void updateTracking()
	{
		int num = int.MinValue;
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

	// Token: 0x0600301C RID: 12316 RVA: 0x000EF726 File Offset: 0x000EDB26
	public void OnSend()
	{
		this.lastSendTime = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x040021A0 RID: 8608
	public static readonly int COMPENSATION_TRACKER_LENGTH = 100;

	// Token: 0x040021A1 RID: 8609
	public static readonly double TIMESYNC_INTERVAL = 30.0;

	// Token: 0x040021A2 RID: 8610
	private int ownerClientId;

	// Token: 0x040021A3 RID: 8611
	private ITimekeeper timeKeeper;

	// Token: 0x040021A4 RID: 8612
	private TimestepSynchronizer.TimeKeeperData[] playerTimesyncs;

	// Token: 0x040021A7 RID: 8615
	private double lastSendTime;

	// Token: 0x020007A0 RID: 1952
	private struct TimeKeeperData
	{
		// Token: 0x040021A8 RID: 8616
		public bool syncronizationComplete;

		// Token: 0x040021A9 RID: 8617
		public int[] compensationOffsets;

		// Token: 0x040021AA RID: 8618
		public int trackerLength;

		// Token: 0x040021AB RID: 8619
		public int calculatedLatencyMs;

		// Token: 0x040021AC RID: 8620
		public int MessageLatencyMs;
	}
}
