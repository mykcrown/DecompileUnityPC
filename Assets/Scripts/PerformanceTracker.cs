// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PerformanceTracker : IPerformanceTracker, ITickable
{
	public struct PerformanceReport
	{
		public float AvgDisplayFPS
		{
			get;
			private set;
		}

		public float WorstDisplayFPS
		{
			get;
			private set;
		}

		public float AvgGameFPS
		{
			get;
			private set;
		}

		public float WorstGameFPS
		{
			get;
			private set;
		}

		public float AvgGC
		{
			get;
			private set;
		}

		public int TotalGC
		{
			get;
			private set;
		}

		public int Stutters
		{
			get;
			private set;
		}

		public long Memory
		{
			get;
			private set;
		}

		public float AvgPing
		{
			get;
			private set;
		}

		public float WorstPing
		{
			get;
			private set;
		}

		public float AvgServerPing
		{
			get;
			private set;
		}

		public float AvgRollbackFrames
		{
			get;
			private set;
		}

		public float AvgRollbackDuration
		{
			get;
			private set;
		}

		public float TotalRollbackDuration
		{
			get;
			private set;
		}

		public float AvgWaitDuration
		{
			get;
			private set;
		}

		public float TotalWaitDuration
		{
			get;
			private set;
		}

		public float AvgSkippedFrames
		{
			get;
			private set;
		}

		public int MaxSkippedFrames
		{
			get;
			private set;
		}

		public int TotalSkippedFrames
		{
			get;
			private set;
		}

		public int SkippedFrameOccurrance
		{
			get;
			private set;
		}

		public int TotalWaits
		{
			get;
			private set;
		}

		public void RecordFPS(float avgDisplayFPS, float worstDisplayFPS, float avgGameFPS, float worstGameFPS, float avgSkippedFrames, int maxSkippedFrames, int skippedFrameOccurances, int totalSkippedFrames)
		{
			this.AvgDisplayFPS = avgDisplayFPS;
			this.WorstDisplayFPS = worstDisplayFPS;
			this.AvgGameFPS = avgGameFPS;
			this.WorstGameFPS = worstGameFPS;
			this.AvgSkippedFrames = avgSkippedFrames;
			this.MaxSkippedFrames = maxSkippedFrames;
			this.SkippedFrameOccurrance = skippedFrameOccurances;
			this.TotalSkippedFrames = totalSkippedFrames;
		}

		public void RecordGC(long memory, float avgGC, int totalGC)
		{
			this.Memory = memory;
			this.AvgGC = avgGC;
			this.TotalGC = totalGC;
		}

		public void RecordPing(float avgPing, float worstPing, float avgServerPing)
		{
			this.AvgPing = avgPing;
			this.WorstPing = worstPing;
			this.AvgServerPing = avgServerPing;
		}

		public void RecordRollback(float avgRollbackDuration, float totalRollbackDuration, float avgRollbackFrames)
		{
			this.AvgRollbackDuration = avgRollbackDuration;
			this.TotalRollbackDuration = totalRollbackDuration;
			this.AvgRollbackFrames = avgRollbackFrames;
		}

		public void RecordWaiting(int stutters, float avgWaitDuration, float totalWaitDuration)
		{
			this.Stutters = stutters;
			this.AvgWaitDuration = avgWaitDuration;
			this.TotalWaitDuration = totalWaitDuration;
		}
	}

	public static readonly int START_PERFORMANCE_TRACK_FRAME = 180;

	private StatTracker gameFPSTracker;

	private StatTracker displayFPSTracker;

	private StatTracker pingTracker;

	private StatTracker serverPingTracker;

	private StatTracker skippedFramesTracker;

	private StatTracker rollbackDurationTracker;

	private StatTracker rollbackFramesTracker;

	private StatTracker waitingDurationTracker;

	private int garbageCollectionStartCount;

	private bool isStarted;

	public bool HasStarted
	{
		get
		{
			return this.isStarted;
		}
	}

	[Inject]
	public GameController gameLocator
	{
		get;
		set;
	}

	[PostConstruct]
	public void Initialize()
	{
		this.displayFPSTracker = new StatTracker();
		this.gameFPSTracker = new StatTracker();
		this.pingTracker = new StatTracker();
		this.serverPingTracker = new StatTracker();
		this.skippedFramesTracker = new StatTracker();
		this.rollbackFramesTracker = new StatTracker();
		this.rollbackDurationTracker = new StatTracker();
		this.waitingDurationTracker = new StatTracker();
	}

	public void Start()
	{
		this.isStarted = true;
		this.garbageCollectionStartCount = GC.CollectionCount(GC.MaxGeneration);
		this.displayFPSTracker.Reset();
		this.gameFPSTracker.Reset();
		this.pingTracker.Reset();
		this.serverPingTracker.Reset();
		this.skippedFramesTracker.Reset();
		this.rollbackDurationTracker.Reset();
		this.rollbackFramesTracker.Reset();
		this.waitingDurationTracker.Reset();
	}

	void IPerformanceTracker.RecordWaiting(double waitDuration)
	{
		if (this.isStarted)
		{
			this.waitingDurationTracker.RecordValue((float)waitDuration);
		}
	}

	void IPerformanceTracker.RecordFPS(float displayFPS, float gameFPS)
	{
		if (this.isStarted)
		{
			this.displayFPSTracker.RecordValue(displayFPS);
			this.gameFPSTracker.RecordValue(gameFPS);
		}
	}

	void IPerformanceTracker.RecordPing(float ping, float serverPing)
	{
		if (this.isStarted)
		{
			this.pingTracker.RecordValue(ping);
			this.serverPingTracker.RecordValue(serverPing);
		}
	}

	void IPerformanceTracker.RecordSkippedFrames(int skippedFrames)
	{
		if (this.isStarted && skippedFrames > 0)
		{
			this.skippedFramesTracker.RecordValue((float)skippedFrames);
		}
	}

	void IPerformanceTracker.RecordRollbackDuration(double durationMs)
	{
		if (this.isStarted)
		{
			this.rollbackDurationTracker.RecordValue((float)durationMs);
		}
	}

	void IPerformanceTracker.RecordRollbackFrames(int rollbackFrames)
	{
		if (this.isStarted)
		{
			this.rollbackFramesTracker.RecordValue((float)rollbackFrames);
		}
	}

	public void TickFrame()
	{
		if (!this.isStarted && this.gameLocator.currentGame != null && this.gameLocator.currentGame.Frame >= PerformanceTracker.START_PERFORMANCE_TRACK_FRAME)
		{
			this.Start();
		}
		if (!this.isStarted || (this.gameLocator.currentGame != null && this.gameLocator.currentGame.IsPaused))
		{
			return;
		}
	}

	public PerformanceTracker.PerformanceReport End()
	{
		this.isStarted = false;
		int num = GC.CollectionCount(GC.MaxGeneration);
		int num2 = num - this.garbageCollectionStartCount;
		PerformanceTracker.PerformanceReport result = default(PerformanceTracker.PerformanceReport);
		result.RecordFPS(this.displayFPSTracker.AverageValue, this.displayFPSTracker.MinimumValue, this.gameFPSTracker.AverageValue, this.gameFPSTracker.MinimumValue, this.skippedFramesTracker.AverageValue, (int)this.skippedFramesTracker.MaximumValue, this.skippedFramesTracker.TotalValues, (int)this.skippedFramesTracker.Sum);
		result.RecordGC(GC.GetTotalMemory(false), (float)num2 / (float)((this.gameLocator.currentGame.Frame <= 0) ? 1 : this.gameLocator.currentGame.Frame), num2);
		result.RecordPing(this.pingTracker.AverageValue, this.pingTracker.MaximumValue, this.serverPingTracker.AverageValue);
		result.RecordRollback(this.rollbackDurationTracker.AverageValue, this.rollbackDurationTracker.Sum, this.rollbackFramesTracker.AverageValue);
		result.RecordWaiting(this.waitingDurationTracker.TotalValues, this.waitingDurationTracker.AverageValue, (float)((int)this.waitingDurationTracker.Sum));
		return result;
	}
}
