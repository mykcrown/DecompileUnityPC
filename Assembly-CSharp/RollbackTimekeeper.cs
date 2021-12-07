using System;
using network;
using UnityEngine;

// Token: 0x02000890 RID: 2192
public class RollbackTimekeeper : ITimekeeper
{
	// Token: 0x17000D66 RID: 3430
	// (get) Token: 0x06003709 RID: 14089 RVA: 0x00100911 File Offset: 0x000FED11
	// (set) Token: 0x0600370A RID: 14090 RVA: 0x00100919 File Offset: 0x000FED19
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000D67 RID: 3431
	// (get) Token: 0x0600370B RID: 14091 RVA: 0x00100922 File Offset: 0x000FED22
	// (set) Token: 0x0600370C RID: 14092 RVA: 0x0010092A File Offset: 0x000FED2A
	[Inject]
	public IBattleServerAPI battleServer { get; set; }

	// Token: 0x17000D68 RID: 3432
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x00100933 File Offset: 0x000FED33
	// (set) Token: 0x0600370E RID: 14094 RVA: 0x0010093B File Offset: 0x000FED3B
	[Inject]
	public RollbackStatePoolContainer rollbackStatePool { get; set; }

	// Token: 0x17000D69 RID: 3433
	// (get) Token: 0x0600370F RID: 14095 RVA: 0x00100944 File Offset: 0x000FED44
	// (set) Token: 0x06003710 RID: 14096 RVA: 0x0010094C File Offset: 0x000FED4C
	[Inject]
	public ITimeSynchronizer timeSynchronizer { private get; set; }

	// Token: 0x17000D6A RID: 3434
	// (get) Token: 0x06003711 RID: 14097 RVA: 0x00100955 File Offset: 0x000FED55
	// (set) Token: 0x06003712 RID: 14098 RVA: 0x0010095D File Offset: 0x000FED5D
	[Inject]
	public GameController gameController { private get; set; }

	// Token: 0x17000D6B RID: 3435
	// (get) Token: 0x06003713 RID: 14099 RVA: 0x00100966 File Offset: 0x000FED66
	public double MsSinceStart
	{
		get
		{
			return WTime.precisionTimeSinceStartup - this.startTimeMs;
		}
	}

	// Token: 0x17000D6C RID: 3436
	// (get) Token: 0x06003714 RID: 14100 RVA: 0x00100974 File Offset: 0x000FED74
	public bool AllClientsSynchronized
	{
		get
		{
			return !this.battleServer.IsConnected || this.timeSynchronizer.IsSynchronizationComplete;
		}
	}

	// Token: 0x17000D6D RID: 3437
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x00100994 File Offset: 0x000FED94
	private int maxFrameBeforeWaiting
	{
		get
		{
			return Math.Max(0, this.frameWithAllInputs + RollbackStatePoolContainer.ROLLBACK_FRAMES - 1);
		}
	}

	// Token: 0x06003716 RID: 14102 RVA: 0x001009AC File Offset: 0x000FEDAC
	public void Init(RollbackSettings settings)
	{
		this.isSpectating = false;
		foreach (RollbackPlayerData rollbackPlayerData in settings.playerDataList)
		{
			if (rollbackPlayerData.isLocal)
			{
				this.isSpectating = rollbackPlayerData.isSpectator;
			}
		}
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x00100A20 File Offset: 0x000FEE20
	public void Start(double startTime)
	{
		this.hasStarted = true;
		this.startTimeMs = startTime;
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x00100A30 File Offset: 0x000FEE30
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
			Debug.Log(string.Concat(new object[]
			{
				"Enforce wait ",
				this.targetFrame,
				" ",
				this.maxFrameBeforeWaiting
			}));
		}
		return Mathf.Max(this.targetFrame, 0);
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x00100AF4 File Offset: 0x000FEEF4
	public double GetMSFrameOffset()
	{
		int num = (int)(this.MsSinceStart * (double)WTime.fps * (double)this.playbackSpeed / 1000.0) + this.milestoneFrame;
		double num2 = (double)((float)(num - this.milestoneFrame) / (this.playbackSpeed / 1000f) / WTime.fps);
		return this.MsSinceStart - num2;
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x00100B50 File Offset: 0x000FEF50
	private int calculateExpectedFrame()
	{
		int num = (int)(this.MsSinceStart * (double)WTime.fps * (double)this.playbackSpeed / 1000.0) + this.milestoneFrame;
		if (this.isSpectating)
		{
			num -= RollbackTimekeeper.SPECTATOR_MODE_DELAY;
		}
		return num;
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x00100B99 File Offset: 0x000FEF99
	public void ResetMilestone(int currentFrame)
	{
		if (!this.hasStarted)
		{
			Debug.LogWarning("Attempted to reset milestone before timekeeper was started");
			return;
		}
		this.milestoneFrame = currentFrame;
		this.startTimeMs = WTime.precisionTimeSinceStartup;
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x00100BC3 File Offset: 0x000FEFC3
	public void IncreasePlaybackSpeed()
	{
		this.SetPlaybackSpeed(this.playbackSpeed * 2f);
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x00100BD7 File Offset: 0x000FEFD7
	public void DecreasePlaybackSpeed()
	{
		this.SetPlaybackSpeed(this.playbackSpeed / 2f);
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x00100BEB File Offset: 0x000FEFEB
	public void SetPlaybackSpeed(float speed)
	{
		this.ResetMilestone(this.CalculateTargetFrame());
		this.playbackSpeed = Mathf.Min((float)this.maxPlaybackSpeed, Mathf.Max(speed, this.minPlaybackSpeed));
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x00100C17 File Offset: 0x000FF017
	public void OnAllInputsFrameUpdated(IRollbackClient client, int frame)
	{
		this.frameWithAllInputs = frame;
		this.rollbackStatePool.CachedFrameWithAllInputs = frame;
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x00100C2C File Offset: 0x000FF02C
	public void Destroy()
	{
	}

	// Token: 0x0400255F RID: 9567
	private static int SPECTATOR_MODE_DELAY = 25;

	// Token: 0x04002565 RID: 9573
	private double startTimeMs;

	// Token: 0x04002566 RID: 9574
	private int milestoneFrame;

	// Token: 0x04002567 RID: 9575
	private bool hasStarted;

	// Token: 0x04002568 RID: 9576
	private IRollbackInputStatus rollbackInputStatus;

	// Token: 0x04002569 RID: 9577
	private int frameWithAllInputs;

	// Token: 0x0400256A RID: 9578
	private float playbackSpeed = 1f;

	// Token: 0x0400256B RID: 9579
	private int maxPlaybackSpeed = 4;

	// Token: 0x0400256C RID: 9580
	private float minPlaybackSpeed = 0.25f;

	// Token: 0x0400256D RID: 9581
	private int targetFrame;

	// Token: 0x0400256E RID: 9582
	private bool isSpectating;
}
