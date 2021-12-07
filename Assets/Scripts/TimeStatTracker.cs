// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class TimeStatTracker : ITimeStatTracker, ITickable
{
	public delegate void OnIntervalHandler(TimeStatTracker tracker);

	private sealed class _AttachToGraph_c__AnonStorey0
	{
		internal DebugGraph graph;

		internal void __m__0(TimeStatTracker tracker)
		{
			this.graph.ReportValue(tracker.AverageValueOverPeriod);
		}
	}

	private int intervalMs;

	private long lastPeriodTimeMs;

	private List<float> valuesOverPeriod = new List<float>();

	private string name;

	public event TimeStatTracker.OnIntervalHandler OnInterval;

	public bool EnabledInRelease
	{
		get;
		set;
	}

	public float MaximumValueOverPeriod
	{
		get;
		private set;
	}

	public float MinimumValueOverPeriod
	{
		get;
		private set;
	}

	public float AverageValueOverPeriod
	{
		get;
		private set;
	}

	public int TotalValuesOverPeriod
	{
		get;
		private set;
	}

	public bool LogOnInterval
	{
		get;
		set;
	}

	public TimeStatTracker(int intervalMs, bool enabledInRelease, bool logAverage = false, string name = "")
	{
		this.intervalMs = intervalMs;
		this.LogOnInterval = logAverage;
		this.name = name;
		this.EnabledInRelease = enabledInRelease;
		this.MinimumValueOverPeriod = 2.14748365E+09f;
		if (TimeStatTrackerManager.Instance != null)
		{
			TimeStatTrackerManager.Instance.Register(this);
		}
	}

	public void TickFrame()
	{
		if (this.intervalMs >= 0 && WTime.currentTimeMs - this.lastPeriodTimeMs > (long)this.intervalMs)
		{
			this.markInterval();
		}
	}

	public void Flush()
	{
		this.markInterval();
	}

	private void markInterval()
	{
		this.lastPeriodTimeMs = WTime.currentTimeMs;
		if (this.valuesOverPeriod.Count == 0)
		{
			return;
		}
		this.MaximumValueOverPeriod = 0f;
		this.MinimumValueOverPeriod = 2.14748365E+09f;
		this.TotalValuesOverPeriod = this.valuesOverPeriod.Count;
		this.AverageValueOverPeriod = 0f;
		float num = 0f;
		foreach (float num2 in this.valuesOverPeriod)
		{
			this.MaximumValueOverPeriod = Mathf.Max(num2, this.MaximumValueOverPeriod);
			this.MinimumValueOverPeriod = Mathf.Min(num2, this.MinimumValueOverPeriod);
			num += num2;
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

	public void RecordValue(float value)
	{
		if (TimeStatTrackerManager.Instance.DebugTrackersEnabled || this.EnabledInRelease)
		{
			this.valuesOverPeriod.Add(value);
		}
	}

	public void AttachToGraph(DebugGraph graph)
	{
		TimeStatTracker._AttachToGraph_c__AnonStorey0 _AttachToGraph_c__AnonStorey = new TimeStatTracker._AttachToGraph_c__AnonStorey0();
		_AttachToGraph_c__AnonStorey.graph = graph;
		this.OnInterval += new TimeStatTracker.OnIntervalHandler(_AttachToGraph_c__AnonStorey.__m__0);
	}
}
