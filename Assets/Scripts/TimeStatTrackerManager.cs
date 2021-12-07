// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class TimeStatTrackerManager : ITimeStatTrackerManager, ITickable
{
	private HashSet<TimeStatTracker> trackers = new HashSet<TimeStatTracker>();

	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	public bool DebugTrackersEnabled
	{
		get;
		private set;
	}

	public static TimeStatTrackerManager Instance
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		TimeStatTrackerManager.Instance = this;
		this.DebugTrackersEnabled = true;
	}

	public void InitDebug()
	{
		this.devConsole.AddConsoleVariable<bool>("debug", "enable_debug_trackers", "Debug Trackers Enabled", "If enabled, debug stat trackers will record stats and report at their set intervals.", new Func<bool>(this.get_DebugTrackersEnabled), new Action<bool>(this._InitDebug_m__0));
	}

	public void Register(TimeStatTracker tracker)
	{
		this.trackers.Add(tracker);
	}

	public void Unregister(TimeStatTracker tracker)
	{
		this.trackers.Remove(tracker);
	}

	public void TickFrame()
	{
		foreach (TimeStatTracker current in this.trackers)
		{
			if (current.EnabledInRelease || this.DebugTrackersEnabled)
			{
				current.TickFrame();
			}
		}
	}

	private void _InitDebug_m__0(bool value)
	{
		this.DebugTrackersEnabled = value;
	}
}
