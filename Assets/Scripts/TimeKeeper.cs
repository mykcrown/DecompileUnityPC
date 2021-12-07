// Decompile from assembly: Assembly-CSharp.dll

using System;

public class TimeKeeper : ITickable, IRollbackStateOwner
{
	private TimeKeeperModel model;

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	public int TotalSeconds
	{
		get;
		private set;
	}

	public float SecondsElapsed
	{
		get
		{
			return (float)this.model.ticksElapsed / WTime.fps;
		}
	}

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

	public bool ShouldDisplay
	{
		get
		{
			return this.TotalSeconds > 0 && this.model.hasStarted;
		}
	}

	public void Init(int totalSeconds = 0)
	{
		this.TotalSeconds = totalSeconds;
		this.model = new TimeKeeperModel();
		this.events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	public void TickFrame()
	{
		if (!this.model.hasStarted)
		{
			return;
		}
		this.model.ticksElapsed++;
	}

	private void onGameStart(GameEvent message)
	{
		this.model.hasStarted = true;
	}

	public void Destroy()
	{
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<TimeKeeperModel>(this.model));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TimeKeeperModel>(ref this.model);
		return true;
	}
}
