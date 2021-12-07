using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000678 RID: 1656
public class StatsTracker : IRollbackStateOwner
{
	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x060028F1 RID: 10481 RVA: 0x000C5FB1 File Offset: 0x000C43B1
	// (set) Token: 0x060028F2 RID: 10482 RVA: 0x000C5FB9 File Offset: 0x000C43B9
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x060028F3 RID: 10483 RVA: 0x000C5FC2 File Offset: 0x000C43C2
	// (set) Token: 0x060028F4 RID: 10484 RVA: 0x000C5FCA File Offset: 0x000C43CA
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x060028F5 RID: 10485 RVA: 0x000C5FD3 File Offset: 0x000C43D3
	public List<PlayerStats> PlayerStats
	{
		get
		{
			return this.model.PlayerStats;
		}
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x000C5FE0 File Offset: 0x000C43E0
	public void Init(PlayerSelectionList players)
	{
		this.model.PlayerStats.Clear();
		int num = 0;
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
				this.PlayerNumToStatIndex[playerSelectionInfo.playerNum] = num;
				PlayerStats playerStats = new PlayerStats();
				playerStats.playerInfo = playerSelectionInfo;
				this.model.PlayerStats.Add(playerStats);
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.events.Subscribe(typeof(LogStatEvent), new Events.EventHandler(this.logStat));
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x000C60A4 File Offset: 0x000C44A4
	public void Destroy()
	{
		this.events.Unsubscribe(typeof(LogStatEvent), new Events.EventHandler(this.logStat));
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x000C60C8 File Offset: 0x000C44C8
	private void logStat(GameEvent message)
	{
		LogStatEvent logStatEvent = message as LogStatEvent;
		this.PlayerStats[this.PlayerNumToStatIndex[logStatEvent.player]].LogStat(logStatEvent.stat, logStatEvent.value, logStatEvent.operation);
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000C610F File Offset: 0x000C450F
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StatTrackerModel>(ref this.model);
		return true;
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000C611F File Offset: 0x000C451F
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<StatTrackerModel>(this.model));
	}

	// Token: 0x04001DA3 RID: 7587
	private StatTrackerModel model = new StatTrackerModel();

	// Token: 0x04001DA4 RID: 7588
	public Dictionary<PlayerNum, int> PlayerNumToStatIndex = new Dictionary<PlayerNum, int>();
}
