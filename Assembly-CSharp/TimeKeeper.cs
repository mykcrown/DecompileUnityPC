using System;

// Token: 0x0200067E RID: 1662
public class TimeKeeper : ITickable, IRollbackStateOwner
{
	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x06002917 RID: 10519 RVA: 0x000C6A90 File Offset: 0x000C4E90
	// (set) Token: 0x06002918 RID: 10520 RVA: 0x000C6A98 File Offset: 0x000C4E98
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x06002919 RID: 10521 RVA: 0x000C6AA1 File Offset: 0x000C4EA1
	// (set) Token: 0x0600291A RID: 10522 RVA: 0x000C6AA9 File Offset: 0x000C4EA9
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x0600291B RID: 10523 RVA: 0x000C6AB2 File Offset: 0x000C4EB2
	// (set) Token: 0x0600291C RID: 10524 RVA: 0x000C6ABA File Offset: 0x000C4EBA
	public int TotalSeconds { get; private set; }

	// Token: 0x0600291D RID: 10525 RVA: 0x000C6AC3 File Offset: 0x000C4EC3
	public void Init(int totalSeconds = 0)
	{
		this.TotalSeconds = totalSeconds;
		this.model = new TimeKeeperModel();
		this.events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x0600291E RID: 10526 RVA: 0x000C6AF8 File Offset: 0x000C4EF8
	public float SecondsElapsed
	{
		get
		{
			return (float)this.model.ticksElapsed / WTime.fps;
		}
	}

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x0600291F RID: 10527 RVA: 0x000C6B0D File Offset: 0x000C4F0D
	public float CurrentSeconds
	{
		get
		{
			if (this.TotalSeconds > 0)
			{
				return (float)this.TotalSeconds - this.SecondsElapsed;
			}
			return this.SecondsElapsed;
		}
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000C6B30 File Offset: 0x000C4F30
	public void TickFrame()
	{
		if (!this.model.hasStarted)
		{
			return;
		}
		this.model.ticksElapsed++;
	}

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x06002921 RID: 10529 RVA: 0x000C6B56 File Offset: 0x000C4F56
	public bool ShouldDisplay
	{
		get
		{
			return this.TotalSeconds > 0 && this.model.hasStarted;
		}
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000C6B72 File Offset: 0x000C4F72
	private void onGameStart(GameEvent message)
	{
		this.model.hasStarted = true;
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x000C6B80 File Offset: 0x000C4F80
	public void Destroy()
	{
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000C6BA3 File Offset: 0x000C4FA3
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<TimeKeeperModel>(this.model));
		return true;
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000C6BBF File Offset: 0x000C4FBF
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TimeKeeperModel>(ref this.model);
		return true;
	}

	// Token: 0x04001DC3 RID: 7619
	private TimeKeeperModel model;
}
