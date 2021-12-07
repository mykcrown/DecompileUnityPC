using System;
using System.Collections.Generic;

// Token: 0x02000B6C RID: 2924
public class TimeStatTrackerManager : ITimeStatTrackerManager, ITickable
{
	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x06005494 RID: 21652 RVA: 0x001B272A File Offset: 0x001B0B2A
	// (set) Token: 0x06005495 RID: 21653 RVA: 0x001B2732 File Offset: 0x001B0B32
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x06005496 RID: 21654 RVA: 0x001B273B File Offset: 0x001B0B3B
	// (set) Token: 0x06005497 RID: 21655 RVA: 0x001B2743 File Offset: 0x001B0B43
	public bool DebugTrackersEnabled { get; private set; }

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x06005498 RID: 21656 RVA: 0x001B274C File Offset: 0x001B0B4C
	// (set) Token: 0x06005499 RID: 21657 RVA: 0x001B2753 File Offset: 0x001B0B53
	public static TimeStatTrackerManager Instance { get; private set; }

	// Token: 0x0600549A RID: 21658 RVA: 0x001B275B File Offset: 0x001B0B5B
	[PostConstruct]
	public void Init()
	{
		TimeStatTrackerManager.Instance = this;
		this.DebugTrackersEnabled = true;
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001B276A File Offset: 0x001B0B6A
	public void InitDebug()
	{
		this.devConsole.AddConsoleVariable<bool>("debug", "enable_debug_trackers", "Debug Trackers Enabled", "If enabled, debug stat trackers will record stats and report at their set intervals.", new Func<bool>(this.get_DebugTrackersEnabled), delegate(bool value)
		{
			this.DebugTrackersEnabled = value;
		});
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x001B27A3 File Offset: 0x001B0BA3
	public void Register(TimeStatTracker tracker)
	{
		this.trackers.Add(tracker);
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001B27B2 File Offset: 0x001B0BB2
	public void Unregister(TimeStatTracker tracker)
	{
		this.trackers.Remove(tracker);
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001B27C4 File Offset: 0x001B0BC4
	public void TickFrame()
	{
		foreach (TimeStatTracker timeStatTracker in this.trackers)
		{
			if (timeStatTracker.EnabledInRelease || this.DebugTrackersEnabled)
			{
				timeStatTracker.TickFrame();
			}
		}
	}

	// Token: 0x0400358C RID: 13708
	private HashSet<TimeStatTracker> trackers = new HashSet<TimeStatTracker>();
}
