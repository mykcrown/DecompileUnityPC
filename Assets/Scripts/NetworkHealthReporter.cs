// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using UnityEngine;

public class NetworkHealthReporter : ClientBehavior
{
	private double lastHealthTimeSeconds;

	private int calculatedLatencyMs;

	private int frameDelay;

	private TimeStatTracker calculatedLatencyStats = new TimeStatTracker(1000, true, false, string.Empty);

	private TimeStatTracker frameDelayStats = new TimeStatTracker(1000, true, false, string.Empty);

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IGameClient gameClient
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatus rollbackStatus
	{
		get;
		set;
	}

	private void Update()
	{
		double totalSeconds = DateTime.UtcNow.TimeOfDay.TotalSeconds;
		if (totalSeconds - this.lastHealthTimeSeconds >= 1.0)
		{
			this.lastHealthTimeSeconds = totalSeconds;
		}
	}

	public void ReportHealth(NetworkHealthReport health)
	{
		this.calculatedLatencyMs = health.calculatedLatencyMs;
		this.calculatedLatencyStats.RecordValue((float)this.calculatedLatencyMs);
		this.frameDelay = health.messageDelay;
		this.frameDelayStats.RecordValue((float)this.frameDelay);
	}

	private void OnGUI()
	{
		if (base.battleServerAPI.IsOnlineMatchReady && BuildConfig.environmentType == BuildEnvironment.Local)
		{
			GUI.contentColor = Color.white;
		}
	}

	private Color getPingColor(int ping)
	{
		if (ping < 40)
		{
			return Color.green;
		}
		if (ping < 60)
		{
			return Color.yellow;
		}
		if (ping < 100)
		{
			return new Color(1f, 0.5f, 0f);
		}
		if (ping < 200)
		{
			return Color.red;
		}
		return new Color(0.3f, 0f, 0f);
	}
}
