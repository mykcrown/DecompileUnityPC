// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;
using UnityEngine;

public class RollbackTimekeeper : ITimekeeper
{
	private static int SPECTATOR_MODE_DELAY = 25;

	private double startTimeMs;

	private int milestoneFrame;

	private bool hasStarted;

	private IRollbackInputStatus rollbackInputStatus;

	private int frameWithAllInputs;

	private float playbackSpeed = 1f;

	private int maxPlaybackSpeed = 4;

	private float minPlaybackSpeed = 0.25f;

	private int targetFrame;

	private bool isSpectating;

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServer
	{
		get;
		set;
	}

	[Inject]
	public RollbackStatePoolContainer rollbackStatePool
	{
		get;
		set;
	}

	[Inject]
	public ITimeSynchronizer timeSynchronizer
	{
		private get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		private get;
		set;
	}

	public double MsSinceStart
	{
		get
		{
			return WTime.precisionTimeSinceStartup - this.startTimeMs;
		}
	}

	public bool AllClientsSynchronized
	{
		get
		{
			return !this.battleServer.IsConnected || this.timeSynchronizer.IsSynchronizationComplete;
		}
	}

	private int maxFrameBeforeWaiting
	{
		get
		{
			return Math.Max(0, this.frameWithAllInputs + RollbackStatePoolContainer.ROLLBACK_FRAMES - 1);
		}
	}

	public void Init(RollbackSettings settings)
	{
		this.isSpectating = false;
		foreach (RollbackPlayerData current in settings.playerDataList)
		{
			if (current.isLocal)
			{
				this.isSpectating = current.isSpectator;
			}
		}
	}

	public void Start(double startTime)
	{
		this.hasStarted = true;
		this.startTimeMs = startTime;
	}

	public int CalculateTargetFrame()
	{
		if (!this.hasStarted)
		{
			return 0;
		}
		if (!this.AllClientsSynchronized)
		{
			return 0;
		}
		this.targetFrame = this.calculateExpectedFrame();
		if ((!this.isSpectating || this.targetFrame > this.gameController.currentGame.GameStartInputFrame + 15) && this.targetFrame > this.maxFrameBeforeWaiting)
		{
			this.targetFrame = this.maxFrameBeforeWaiting;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Enforce wait ",
				this.targetFrame,
				" ",
				this.maxFrameBeforeWaiting
			}));
		}
		return Mathf.Max(this.targetFrame, 0);
	}

	public double GetMSFrameOffset()
	{
		int num = (int)(this.MsSinceStart * (double)WTime.fps * (double)this.playbackSpeed / 1000.0) + this.milestoneFrame;
		double num2 = (double)((float)(num - this.milestoneFrame) / (this.playbackSpeed / 1000f) / WTime.fps);
		return this.MsSinceStart - num2;
	}

	private int calculateExpectedFrame()
	{
		int num = (int)(this.MsSinceStart * (double)WTime.fps * (double)this.playbackSpeed / 1000.0) + this.milestoneFrame;
		if (this.isSpectating)
		{
			num -= RollbackTimekeeper.SPECTATOR_MODE_DELAY;
		}
		return num;
	}

	public void ResetMilestone(int currentFrame)
	{
		if (!this.hasStarted)
		{
			UnityEngine.Debug.LogWarning("Attempted to reset milestone before timekeeper was started");
			return;
		}
		this.milestoneFrame = currentFrame;
		this.startTimeMs = WTime.precisionTimeSinceStartup;
	}

	public void IncreasePlaybackSpeed()
	{
		this.SetPlaybackSpeed(this.playbackSpeed * 2f);
	}

	public void DecreasePlaybackSpeed()
	{
		this.SetPlaybackSpeed(this.playbackSpeed / 2f);
	}

	public void SetPlaybackSpeed(float speed)
	{
		this.ResetMilestone(this.CalculateTargetFrame());
		this.playbackSpeed = Mathf.Min((float)this.maxPlaybackSpeed, Mathf.Max(speed, this.minPlaybackSpeed));
	}

	public void OnAllInputsFrameUpdated(IRollbackClient client, int frame)
	{
		this.frameWithAllInputs = frame;
		this.rollbackStatePool.CachedFrameWithAllInputs = frame;
	}

	public void Destroy()
	{
	}
}
