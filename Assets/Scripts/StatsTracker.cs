// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class StatsTracker : IRollbackStateOwner
{
	private StatTrackerModel model = new StatTrackerModel();

	public Dictionary<PlayerNum, int> PlayerNumToStatIndex = new Dictionary<PlayerNum, int>();

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public List<PlayerStats> PlayerStats
	{
		get
		{
			return this.model.PlayerStats;
		}
	}

	public void Init(PlayerSelectionList players)
	{
		this.model.PlayerStats.Clear();
		int num = 0;
		IEnumerator enumerator = ((IEnumerable)players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
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

	public void Destroy()
	{
		this.events.Unsubscribe(typeof(LogStatEvent), new Events.EventHandler(this.logStat));
	}

	private void logStat(GameEvent message)
	{
		LogStatEvent logStatEvent = message as LogStatEvent;
		this.PlayerStats[this.PlayerNumToStatIndex[logStatEvent.player]].LogStat(logStatEvent.stat, logStatEvent.value, logStatEvent.operation);
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StatTrackerModel>(ref this.model);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<StatTrackerModel>(this.model));
	}
}
