using System;

// Token: 0x02000B3C RID: 2876
public class PerformanceTracker : IPerformanceTracker, ITickable
{
	// Token: 0x1700134F RID: 4943
	// (get) Token: 0x06005360 RID: 21344 RVA: 0x001AF465 File Offset: 0x001AD865
	public bool HasStarted
	{
		get
		{
			return this.isStarted;
		}
	}

	// Token: 0x17001350 RID: 4944
	// (get) Token: 0x06005361 RID: 21345 RVA: 0x001AF46D File Offset: 0x001AD86D
	// (set) Token: 0x06005362 RID: 21346 RVA: 0x001AF475 File Offset: 0x001AD875
	[Inject]
	public GameController gameLocator { get; set; }

	// Token: 0x06005363 RID: 21347 RVA: 0x001AF480 File Offset: 0x001AD880
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

	// Token: 0x06005364 RID: 21348 RVA: 0x001AF4E8 File Offset: 0x001AD8E8
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

	// Token: 0x06005365 RID: 21349 RVA: 0x001AF564 File Offset: 0x001AD964
	void IPerformanceTracker.RecordWaiting(double waitDuration)
	{
		if (this.isStarted)
		{
			this.waitingDurationTracker.RecordValue((float)waitDuration);
		}
	}

	// Token: 0x06005366 RID: 21350 RVA: 0x001AF57E File Offset: 0x001AD97E
	void IPerformanceTracker.RecordFPS(float displayFPS, float gameFPS)
	{
		if (this.isStarted)
		{
			this.displayFPSTracker.RecordValue(displayFPS);
			this.gameFPSTracker.RecordValue(gameFPS);
		}
	}

	// Token: 0x06005367 RID: 21351 RVA: 0x001AF5A3 File Offset: 0x001AD9A3
	void IPerformanceTracker.RecordPing(float ping, float serverPing)
	{
		if (this.isStarted)
		{
			this.pingTracker.RecordValue(ping);
			this.serverPingTracker.RecordValue(serverPing);
		}
	}

	// Token: 0x06005368 RID: 21352 RVA: 0x001AF5C8 File Offset: 0x001AD9C8
	void IPerformanceTracker.RecordSkippedFrames(int skippedFrames)
	{
		if (this.isStarted && skippedFrames > 0)
		{
			this.skippedFramesTracker.RecordValue((float)skippedFrames);
		}
	}

	// Token: 0x06005369 RID: 21353 RVA: 0x001AF5E9 File Offset: 0x001AD9E9
	void IPerformanceTracker.RecordRollbackDuration(double durationMs)
	{
		if (this.isStarted)
		{
			this.rollbackDurationTracker.RecordValue((float)durationMs);
		}
	}

	// Token: 0x0600536A RID: 21354 RVA: 0x001AF603 File Offset: 0x001ADA03
	void IPerformanceTracker.RecordRollbackFrames(int rollbackFrames)
	{
		if (this.isStarted)
		{
			this.rollbackFramesTracker.RecordValue((float)rollbackFrames);
		}
	}

	// Token: 0x0600536B RID: 21355 RVA: 0x001AF620 File Offset: 0x001ADA20
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

	// Token: 0x0600536C RID: 21356 RVA: 0x001AF6A8 File Offset: 0x001ADAA8
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

	// Token: 0x040034F0 RID: 13552
	public static readonly int START_PERFORMANCE_TRACK_FRAME = 180;

	// Token: 0x040034F1 RID: 13553
	private StatTracker gameFPSTracker;

	// Token: 0x040034F2 RID: 13554
	private StatTracker displayFPSTracker;

	// Token: 0x040034F3 RID: 13555
	private StatTracker pingTracker;

	// Token: 0x040034F4 RID: 13556
	private StatTracker serverPingTracker;

	// Token: 0x040034F5 RID: 13557
	private StatTracker skippedFramesTracker;

	// Token: 0x040034F6 RID: 13558
	private StatTracker rollbackDurationTracker;

	// Token: 0x040034F7 RID: 13559
	private StatTracker rollbackFramesTracker;

	// Token: 0x040034F8 RID: 13560
	private StatTracker waitingDurationTracker;

	// Token: 0x040034F9 RID: 13561
	private int garbageCollectionStartCount;

	// Token: 0x040034FA RID: 13562
	private bool isStarted;

	// Token: 0x02000B3D RID: 2877
	public struct PerformanceReport
	{
		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x0600536E RID: 21358 RVA: 0x001AF7FE File Offset: 0x001ADBFE
		// (set) Token: 0x0600536F RID: 21359 RVA: 0x001AF806 File Offset: 0x001ADC06
		public float AvgDisplayFPS { get; private set; }

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06005370 RID: 21360 RVA: 0x001AF80F File Offset: 0x001ADC0F
		// (set) Token: 0x06005371 RID: 21361 RVA: 0x001AF817 File Offset: 0x001ADC17
		public float WorstDisplayFPS { get; private set; }

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06005372 RID: 21362 RVA: 0x001AF820 File Offset: 0x001ADC20
		// (set) Token: 0x06005373 RID: 21363 RVA: 0x001AF828 File Offset: 0x001ADC28
		public float AvgGameFPS { get; private set; }

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06005374 RID: 21364 RVA: 0x001AF831 File Offset: 0x001ADC31
		// (set) Token: 0x06005375 RID: 21365 RVA: 0x001AF839 File Offset: 0x001ADC39
		public float WorstGameFPS { get; private set; }

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06005376 RID: 21366 RVA: 0x001AF842 File Offset: 0x001ADC42
		// (set) Token: 0x06005377 RID: 21367 RVA: 0x001AF84A File Offset: 0x001ADC4A
		public float AvgGC { get; private set; }

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06005378 RID: 21368 RVA: 0x001AF853 File Offset: 0x001ADC53
		// (set) Token: 0x06005379 RID: 21369 RVA: 0x001AF85B File Offset: 0x001ADC5B
		public int TotalGC { get; private set; }

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x0600537A RID: 21370 RVA: 0x001AF864 File Offset: 0x001ADC64
		// (set) Token: 0x0600537B RID: 21371 RVA: 0x001AF86C File Offset: 0x001ADC6C
		public int Stutters { get; private set; }

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x0600537C RID: 21372 RVA: 0x001AF875 File Offset: 0x001ADC75
		// (set) Token: 0x0600537D RID: 21373 RVA: 0x001AF87D File Offset: 0x001ADC7D
		public long Memory { get; private set; }

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x0600537E RID: 21374 RVA: 0x001AF886 File Offset: 0x001ADC86
		// (set) Token: 0x0600537F RID: 21375 RVA: 0x001AF88E File Offset: 0x001ADC8E
		public float AvgPing { get; private set; }

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06005380 RID: 21376 RVA: 0x001AF897 File Offset: 0x001ADC97
		// (set) Token: 0x06005381 RID: 21377 RVA: 0x001AF89F File Offset: 0x001ADC9F
		public float WorstPing { get; private set; }

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06005382 RID: 21378 RVA: 0x001AF8A8 File Offset: 0x001ADCA8
		// (set) Token: 0x06005383 RID: 21379 RVA: 0x001AF8B0 File Offset: 0x001ADCB0
		public float AvgServerPing { get; private set; }

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x001AF8B9 File Offset: 0x001ADCB9
		// (set) Token: 0x06005385 RID: 21381 RVA: 0x001AF8C1 File Offset: 0x001ADCC1
		public float AvgRollbackFrames { get; private set; }

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06005386 RID: 21382 RVA: 0x001AF8CA File Offset: 0x001ADCCA
		// (set) Token: 0x06005387 RID: 21383 RVA: 0x001AF8D2 File Offset: 0x001ADCD2
		public float AvgRollbackDuration { get; private set; }

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x001AF8DB File Offset: 0x001ADCDB
		// (set) Token: 0x06005389 RID: 21385 RVA: 0x001AF8E3 File Offset: 0x001ADCE3
		public float TotalRollbackDuration { get; private set; }

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x0600538A RID: 21386 RVA: 0x001AF8EC File Offset: 0x001ADCEC
		// (set) Token: 0x0600538B RID: 21387 RVA: 0x001AF8F4 File Offset: 0x001ADCF4
		public float AvgWaitDuration { get; private set; }

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x0600538C RID: 21388 RVA: 0x001AF8FD File Offset: 0x001ADCFD
		// (set) Token: 0x0600538D RID: 21389 RVA: 0x001AF905 File Offset: 0x001ADD05
		public float TotalWaitDuration { get; private set; }

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x0600538E RID: 21390 RVA: 0x001AF90E File Offset: 0x001ADD0E
		// (set) Token: 0x0600538F RID: 21391 RVA: 0x001AF916 File Offset: 0x001ADD16
		public float AvgSkippedFrames { get; private set; }

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x001AF91F File Offset: 0x001ADD1F
		// (set) Token: 0x06005391 RID: 21393 RVA: 0x001AF927 File Offset: 0x001ADD27
		public int MaxSkippedFrames { get; private set; }

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x001AF930 File Offset: 0x001ADD30
		// (set) Token: 0x06005393 RID: 21395 RVA: 0x001AF938 File Offset: 0x001ADD38
		public int TotalSkippedFrames { get; private set; }

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06005394 RID: 21396 RVA: 0x001AF941 File Offset: 0x001ADD41
		// (set) Token: 0x06005395 RID: 21397 RVA: 0x001AF949 File Offset: 0x001ADD49
		public int SkippedFrameOccurrance { get; private set; }

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06005396 RID: 21398 RVA: 0x001AF952 File Offset: 0x001ADD52
		// (set) Token: 0x06005397 RID: 21399 RVA: 0x001AF95A File Offset: 0x001ADD5A
		public int TotalWaits { get; private set; }

		// Token: 0x06005398 RID: 21400 RVA: 0x001AF963 File Offset: 0x001ADD63
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

		// Token: 0x06005399 RID: 21401 RVA: 0x001AF9A2 File Offset: 0x001ADDA2
		public void RecordGC(long memory, float avgGC, int totalGC)
		{
			this.Memory = memory;
			this.AvgGC = avgGC;
			this.TotalGC = totalGC;
		}

		// Token: 0x0600539A RID: 21402 RVA: 0x001AF9B9 File Offset: 0x001ADDB9
		public void RecordPing(float avgPing, float worstPing, float avgServerPing)
		{
			this.AvgPing = avgPing;
			this.WorstPing = worstPing;
			this.AvgServerPing = avgServerPing;
		}

		// Token: 0x0600539B RID: 21403 RVA: 0x001AF9D0 File Offset: 0x001ADDD0
		public void RecordRollback(float avgRollbackDuration, float totalRollbackDuration, float avgRollbackFrames)
		{
			this.AvgRollbackDuration = avgRollbackDuration;
			this.TotalRollbackDuration = totalRollbackDuration;
			this.AvgRollbackFrames = avgRollbackFrames;
		}

		// Token: 0x0600539C RID: 21404 RVA: 0x001AF9E7 File Offset: 0x001ADDE7
		public void RecordWaiting(int stutters, float avgWaitDuration, float totalWaitDuration)
		{
			this.Stutters = stutters;
			this.AvgWaitDuration = avgWaitDuration;
			this.TotalWaitDuration = totalWaitDuration;
		}
	}
}
