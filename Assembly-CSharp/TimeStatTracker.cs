using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B69 RID: 2921
public class TimeStatTracker : ITimeStatTracker, ITickable
{
	// Token: 0x06005479 RID: 21625 RVA: 0x001B23A8 File Offset: 0x001B07A8
	public TimeStatTracker(int intervalMs, bool enabledInRelease, bool logAverage = false, string name = "")
	{
		this.intervalMs = intervalMs;
		this.LogOnInterval = logAverage;
		this.name = name;
		this.EnabledInRelease = enabledInRelease;
		this.MinimumValueOverPeriod = 2.1474836E+09f;
		if (TimeStatTrackerManager.Instance != null)
		{
			TimeStatTrackerManager.Instance.Register(this);
		}
	}

	// Token: 0x1700137C RID: 4988
	// (get) Token: 0x0600547A RID: 21626 RVA: 0x001B2403 File Offset: 0x001B0803
	// (set) Token: 0x0600547B RID: 21627 RVA: 0x001B240B File Offset: 0x001B080B
	public bool EnabledInRelease { get; set; }

	// Token: 0x1700137D RID: 4989
	// (get) Token: 0x0600547C RID: 21628 RVA: 0x001B2414 File Offset: 0x001B0814
	// (set) Token: 0x0600547D RID: 21629 RVA: 0x001B241C File Offset: 0x001B081C
	public float MaximumValueOverPeriod { get; private set; }

	// Token: 0x1700137E RID: 4990
	// (get) Token: 0x0600547E RID: 21630 RVA: 0x001B2425 File Offset: 0x001B0825
	// (set) Token: 0x0600547F RID: 21631 RVA: 0x001B242D File Offset: 0x001B082D
	public float MinimumValueOverPeriod { get; private set; }

	// Token: 0x1700137F RID: 4991
	// (get) Token: 0x06005480 RID: 21632 RVA: 0x001B2436 File Offset: 0x001B0836
	// (set) Token: 0x06005481 RID: 21633 RVA: 0x001B243E File Offset: 0x001B083E
	public float AverageValueOverPeriod { get; private set; }

	// Token: 0x17001380 RID: 4992
	// (get) Token: 0x06005482 RID: 21634 RVA: 0x001B2447 File Offset: 0x001B0847
	// (set) Token: 0x06005483 RID: 21635 RVA: 0x001B244F File Offset: 0x001B084F
	public int TotalValuesOverPeriod { get; private set; }

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06005484 RID: 21636 RVA: 0x001B2458 File Offset: 0x001B0858
	// (remove) Token: 0x06005485 RID: 21637 RVA: 0x001B2490 File Offset: 0x001B0890
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TimeStatTracker.OnIntervalHandler OnInterval;

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x06005486 RID: 21638 RVA: 0x001B24C6 File Offset: 0x001B08C6
	// (set) Token: 0x06005487 RID: 21639 RVA: 0x001B24CE File Offset: 0x001B08CE
	public bool LogOnInterval { get; set; }

	// Token: 0x06005488 RID: 21640 RVA: 0x001B24D7 File Offset: 0x001B08D7
	public void TickFrame()
	{
		if (this.intervalMs >= 0 && WTime.currentTimeMs - this.lastPeriodTimeMs > (long)this.intervalMs)
		{
			this.markInterval();
		}
	}

	// Token: 0x06005489 RID: 21641 RVA: 0x001B2503 File Offset: 0x001B0903
	public void Flush()
	{
		this.markInterval();
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x001B250C File Offset: 0x001B090C
	private void markInterval()
	{
		this.lastPeriodTimeMs = WTime.currentTimeMs;
		if (this.valuesOverPeriod.Count == 0)
		{
			return;
		}
		this.MaximumValueOverPeriod = 0f;
		this.MinimumValueOverPeriod = 2.1474836E+09f;
		this.TotalValuesOverPeriod = this.valuesOverPeriod.Count;
		this.AverageValueOverPeriod = 0f;
		float num = 0f;
		foreach (float num2 in this.valuesOverPeriod)
		{
			float num3 = num2;
			this.MaximumValueOverPeriod = Mathf.Max(num3, this.MaximumValueOverPeriod);
			this.MinimumValueOverPeriod = Mathf.Min(num3, this.MinimumValueOverPeriod);
			num += num3;
		}
		if (this.TotalValuesOverPeriod == 0)
		{
			this.AverageValueOverPeriod = 0f;
		}
		else
		{
			this.AverageValueOverPeriod = num / (float)this.TotalValuesOverPeriod;
		}
		if (this.LogOnInterval)
		{
			UnityEngine.Debug.Log(string.Format("{0} tracker average: {1}, max: {2}, min: {3}, count: {4}, calculated ticks per second: {5}", new object[]
			{
				this.name,
				this.AverageValueOverPeriod,
				this.MaximumValueOverPeriod,
				this.MinimumValueOverPeriod,
				this.TotalValuesOverPeriod,
				this.AverageValueOverPeriod * (float)this.TotalValuesOverPeriod / 60f
			}));
		}
		if (this.OnInterval != null)
		{
			this.OnInterval(this);
		}
		this.valuesOverPeriod.Clear();
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x001B26A8 File Offset: 0x001B0AA8
	public void RecordValue(float value)
	{
		if (TimeStatTrackerManager.Instance.DebugTrackersEnabled || this.EnabledInRelease)
		{
			this.valuesOverPeriod.Add(value);
		}
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x001B26D0 File Offset: 0x001B0AD0
	public void AttachToGraph(DebugGraph graph)
	{
		this.OnInterval += delegate(TimeStatTracker tracker)
		{
			graph.ReportValue(tracker.AverageValueOverPeriod);
		};
	}

	// Token: 0x04003581 RID: 13697
	private int intervalMs;

	// Token: 0x04003582 RID: 13698
	private long lastPeriodTimeMs;

	// Token: 0x04003587 RID: 13703
	private List<float> valuesOverPeriod = new List<float>();

	// Token: 0x0400358A RID: 13706
	private string name;

	// Token: 0x02000B6A RID: 2922
	// (Invoke) Token: 0x0600548E RID: 21646
	public delegate void OnIntervalHandler(TimeStatTracker tracker);
}
