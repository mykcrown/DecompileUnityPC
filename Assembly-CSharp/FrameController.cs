using System;
using System.Collections.Generic;
using RollbackDebug;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class FrameController : GameBehavior, IRollbackStateOwner, IFrameOwner
{
	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x0007D3CD File Offset: 0x0007B7CD
	// (set) Token: 0x0600172C RID: 5932 RVA: 0x0007D3D5 File Offset: 0x0007B7D5
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x0600172D RID: 5933 RVA: 0x0007D3DE File Offset: 0x0007B7DE
	// (set) Token: 0x0600172E RID: 5934 RVA: 0x0007D3E6 File Offset: 0x0007B7E6
	[Inject]
	public IRollbackStatus rollbackStatus { get; set; }

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x0600172F RID: 5935 RVA: 0x0007D3EF File Offset: 0x0007B7EF
	// (set) Token: 0x06001730 RID: 5936 RVA: 0x0007D3F7 File Offset: 0x0007B7F7
	[Inject]
	public INetworkHealthReport networkHealthReport { get; set; }

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06001731 RID: 5937 RVA: 0x0007D400 File Offset: 0x0007B800
	// (set) Token: 0x06001732 RID: 5938 RVA: 0x0007D408 File Offset: 0x0007B808
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06001733 RID: 5939 RVA: 0x0007D411 File Offset: 0x0007B811
	// (set) Token: 0x06001734 RID: 5940 RVA: 0x0007D419 File Offset: 0x0007B819
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06001735 RID: 5941 RVA: 0x0007D422 File Offset: 0x0007B822
	// (set) Token: 0x06001736 RID: 5942 RVA: 0x0007D42A File Offset: 0x0007B82A
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06001737 RID: 5943 RVA: 0x0007D433 File Offset: 0x0007B833
	// (set) Token: 0x06001738 RID: 5944 RVA: 0x0007D43B File Offset: 0x0007B83B
	[Inject]
	public IExceptionParser exceptionParser { get; set; }

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06001739 RID: 5945 RVA: 0x0007D444 File Offset: 0x0007B844
	// (set) Token: 0x0600173A RID: 5946 RVA: 0x0007D44C File Offset: 0x0007B84C
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x0600173B RID: 5947 RVA: 0x0007D455 File Offset: 0x0007B855
	// (set) Token: 0x0600173C RID: 5948 RVA: 0x0007D45D File Offset: 0x0007B85D
	[Inject]
	public IMatchDeepLogging deepLogging { get; set; }

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x0600173D RID: 5949 RVA: 0x0007D466 File Offset: 0x0007B866
	// (set) Token: 0x0600173E RID: 5950 RVA: 0x0007D46E File Offset: 0x0007B86E
	[Inject]
	public IApplicationFramerateManager framerateManager { get; set; }

	// Token: 0x0600173F RID: 5951 RVA: 0x0007D477 File Offset: 0x0007B877
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<FrameControllerState>(this.state));
		return true;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x0007D493 File Offset: 0x0007B893
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<FrameControllerState>(ref this.state);
		return true;
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06001741 RID: 5953 RVA: 0x0007D4A3 File Offset: 0x0007B8A3
	public int Frame
	{
		get
		{
			return this.state.currentFrame;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06001742 RID: 5954 RVA: 0x0007D4B0 File Offset: 0x0007B8B0
	private IRollbackGameClient game
	{
		get
		{
			return base.gameController.currentGame;
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06001743 RID: 5955 RVA: 0x0007D4BD File Offset: 0x0007B8BD
	// (set) Token: 0x06001744 RID: 5956 RVA: 0x0007D4C5 File Offset: 0x0007B8C5
	public IRollbackLayer rollbackLayer { get; private set; }

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06001745 RID: 5957 RVA: 0x0007D4CE File Offset: 0x0007B8CE
	// (set) Token: 0x06001746 RID: 5958 RVA: 0x0007D4D5 File Offset: 0x0007B8D5
	public static float FrameDeltaTime { get; private set; }

	// Token: 0x06001747 RID: 5959 RVA: 0x0007D4E0 File Offset: 0x0007B8E0
	public void Init(RollbackSettings settings, RollbackDebugSettings debugSettings, IRollbackLayerDebugger debugger, bool isReplaying)
	{
		this.isReplaying = isReplaying;
		if (debugSettings.enableRollbackDebug)
		{
			this.rollbackLayer = new DebugRollbackLayer();
			base.injector.Inject(this.rollbackLayer);
			(this.rollbackLayer as DebugRollbackLayer).Init(settings, this, debugger, debugSettings);
		}
		else
		{
			this.rollbackLayer = new RollbackLayer();
			base.injector.Inject(this.rollbackLayer);
			(this.rollbackLayer as RollbackLayer).Init(settings, debugger);
		}
		this.rollbackLayer.StartSession(0, string.Empty, 0);
		if (this.game.IsNetworkGame)
		{
			this.syncMode = SyncMode.Rollback;
		}
		this.renderTracker = base.gameManager.Camera.current.GetComponent<RenderTracker>();
		RenderTracker renderTracker = this.renderTracker;
		renderTracker.PreRenderCallback = (Action)Delegate.Combine(renderTracker.PreRenderCallback, new Action(this.onPreRender));
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x0007D5CE File Offset: 0x0007B9CE
	public void OnEndGame()
	{
		if (this.renderTracker != null)
		{
			RenderTracker renderTracker = this.renderTracker;
			renderTracker.PreRenderCallback = (Action)Delegate.Remove(renderTracker.PreRenderCallback, new Action(this.onPreRender));
		}
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x0007D608 File Offset: 0x0007BA08
	public override void Awake()
	{
		base.Awake();
		base.gameManager.events.Subscribe(typeof(ForceRollbackCommand), new Events.EventHandler(this.onForceRollback));
		base.gameManager.events.Subscribe(typeof(AllPlayersReadyMessage), new Events.EventHandler(this.onAllPlayersReady));
		base.gameManager.events.Subscribe(typeof(ToggleFrameControlModeCommand), new Events.EventHandler(this.onToggleFrameControlMode));
		base.gameManager.events.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onGamePaused));
		base.gameManager.events.Subscribe(typeof(DebugAdvanceFrameEvent), new Events.EventHandler(this.onDebugAdvanceFrame));
		base.gameManager.events.Subscribe(typeof(DebugThrowExceptionEvent), new Events.EventHandler(this.onDebugThrowExceptionEvent));
		base.gameManager.events.Subscribe(typeof(ChangePlaybackSpeedCommand), new Events.EventHandler(this.onChangePlaybackSpeedCommand));
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x0600174A RID: 5962 RVA: 0x0007D725 File Offset: 0x0007BB25
	public bool IsManual
	{
		get
		{
			return this.controlMode == FrameControlMode.Manual;
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x0600174B RID: 5963 RVA: 0x0007D730 File Offset: 0x0007BB30
	public bool IsAuto
	{
		get
		{
			return this.controlMode == FrameControlMode.Auto;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x0600174C RID: 5964 RVA: 0x0007D73B File Offset: 0x0007BB3B
	public bool IsLocal
	{
		get
		{
			return this.syncMode == SyncMode.Local;
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x0600174D RID: 5965 RVA: 0x0007D746 File Offset: 0x0007BB46
	public bool IsRollback
	{
		get
		{
			return this.syncMode == SyncMode.Rollback || base.config.networkSettings.simulateRollback;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600174E RID: 5966 RVA: 0x0007D767 File Offset: 0x0007BB67
	public bool IsCurrentFrame
	{
		get
		{
			return this.isCurrentFrame;
		}
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x0007D770 File Offset: 0x0007BB70
	private void tryReceiveMessages()
	{
		try
		{
			this.serverManager.ReceiveAllMessages();
		}
		catch (Exception e)
		{
			this.handleThrownException(e);
			this.serverManager.ClearAllMessages();
		}
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x0007D7B8 File Offset: 0x0007BBB8
	private void trySendMessages()
	{
		try
		{
			this.serverManager.SendAllMessages();
		}
		catch (Exception e)
		{
			this.handleThrownException(e);
			this.serverManager.ClearAllMessages();
		}
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x0007D800 File Offset: 0x0007BC00
	private void Update()
	{
		this.deepLogging.UpdateLoopBegin();
		this.tryReceiveMessages();
		if (this.isSimulateGame())
		{
			if (this.framerateManager.frameSyncMode == FrameSyncMode.RENDER_WAIT)
			{
				if (this.needInitialSync)
				{
					this.waitForFrameMidpoint();
					this.needInitialSync = false;
				}
			}
			else if (this.framerateManager.frameSyncMode == FrameSyncMode.UPDATE_FORCE)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				while (this.rollbackLayer.Timekeeper.CalculateTargetFrame() == this.state.currentFrame && Time.realtimeSinceStartup - realtimeSinceStartup < this.maxWaitTime)
				{
					this.tryReceiveMessages();
					this.trySendMessages();
				}
				if (Time.realtimeSinceStartup - realtimeSinceStartup >= this.maxWaitTime)
				{
					this.deepLogging.RecordFrameWaitError();
				}
			}
			int num = this.rollbackLayer.Timekeeper.CalculateTargetFrame();
			int num2 = num - this.state.currentFrame;
			if (this.IsRollback)
			{
				num2 = Math.Min(num2, RollbackStatePoolContainer.ROLLBACK_FRAMES - 1);
			}
			if (this.framerateManager.frameSyncMode == FrameSyncMode.RENDER_WAIT && num2 == 0)
			{
				this.waitForFrameMidpoint();
				num = this.rollbackLayer.Timekeeper.CalculateTargetFrame();
				num2 = num - this.state.currentFrame;
				this.deepLogging.RecordSyncWait();
			}
			num2 = Math.Min(num2, FrameController.MAX_TICKS);
			if (num2 > 0)
			{
				if (this.state.currentFrame == 0)
				{
					base.gameController.OnGameSynced();
				}
				this.needRender = true;
				this.deepLogging.RecordTickStart(num2, this.state.currentFrame);
				this.deepLogging.RecordRollbackStatus();
				try
				{
					if (this.shouldThrowException)
					{
						throw new Exception("DebugCrash");
					}
					for (int i = 0; i < num2; i++)
					{
						bool flag = i != num2 - 1;
						this.isCurrentFrame = !flag;
						this.game.TickInput(this.state.currentFrame + i, flag);
					}
					this.trySendMessages();
					if (base.gameManager != null)
					{
						for (int j = 0; j < num2; j++)
						{
							this.isCurrentFrame = (j == num2 - 1);
							if (!base.gameManager.IsPaused)
							{
								this.rollbackLayer.Idle(20, out this.unused, -1);
							}
							FrameController.FrameDeltaTime += WTime.frameTime;
							this.game.TickFrame();
							if (base.gameManager == null || !this.IsAuto)
							{
								break;
							}
						}
					}
					this.rollbackLayer.CheckMissingInputs();
					this.rollbackLayer.TickGameQuit();
				}
				catch (Exception e)
				{
					this.handleThrownException(e);
				}
				this.deepLogging.RecordProcessingEnd();
				this.rollbackLayer.ValidFrame();
			}
			else
			{
				this.rollbackLayer.CheckMissingInputs();
				this.rollbackLayer.TickNotFrame();
			}
			if (this.game != null)
			{
				this.game.TickUpdate();
			}
			if (base.battleServerAPI.IsConnected)
			{
				this.networkHealthReport.Update(base.gameManager, num2);
			}
		}
		this.tryReceiveMessages();
		this.trySendMessages();
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x0007DB54 File Offset: 0x0007BF54
	private void handleThrownException(Exception e)
	{
		if (base.gameManager != null)
		{
			string matchID = base.gameManager.MatchID;
		}
		this.dialogController.ShowOneButtonDialog(this.localization.GetText("dialog.FrameController.FatalError.title"), this.localization.GetText("dialog.FrameController.FatalError.body"), this.localization.GetText("dialog.continue"), WindowTransition.STANDARD_FADE, false, default(AudioData));
		if (base.gameManager != null)
		{
			base.gameManager.ForfeitGame(ScreenType.MainMenu);
		}
		if (Application.isEditor)
		{
			Debug.LogError(e.StackTrace);
			throw e;
		}
		Debug.LogError(e);
		Debug.LogError(e.StackTrace);
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x0007DC10 File Offset: 0x0007C010
	private void waitForFrameMidpoint()
	{
		double num = (double)(WTime.frameTime / 2f * 1000f);
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (this.rollbackLayer.Timekeeper.GetMSFrameOffset() < num)
		{
			while (this.rollbackLayer.Timekeeper.GetMSFrameOffset() < num && Time.realtimeSinceStartup - realtimeSinceStartup < this.maxWaitTime)
			{
			}
		}
		else
		{
			double num2 = (double)(WTime.frameTime * 1000f - (float)this.rollbackLayer.Timekeeper.GetMSFrameOffset()) + num;
			double precisionTimeSinceStartup = WTime.precisionTimeSinceStartup;
			while (WTime.precisionTimeSinceStartup - precisionTimeSinceStartup < num2)
			{
			}
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x0007DCB7 File Offset: 0x0007C0B7
	private void LateUpdate()
	{
		FrameController.FrameDeltaTime = 0f;
		this.deepLogging.UpdateLoopEnd();
	}

	// Token: 0x06001755 RID: 5973 RVA: 0x0007DCD0 File Offset: 0x0007C0D0
	private void onPreRender()
	{
		if (this.needRender && this.framerateManager.UseRenderWait)
		{
			double num = this.prevRenderTime + (double)(WTime.frameTime * 1000f);
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			while (WTime.precisionTimeSinceStartup < num && Time.realtimeSinceStartup - realtimeSinceStartup <= this.maxRenderWaitTime)
			{
			}
			if (Time.realtimeSinceStartup - realtimeSinceStartup >= this.maxRenderWaitTime)
			{
				Debug.LogError("Warning, max render wait exceeded. This should never happen and indicates a timing bug!!");
				this.deepLogging.RecordRenderWaitError();
			}
			if (WTime.precisionTimeSinceStartup - this.prevRenderTime >= 17.7)
			{
				this.deepLogging.RecordRenderDelay();
			}
			this.prevRenderTime = WTime.precisionTimeSinceStartup;
			this.deepLogging.RecordRenderFrame();
			this.needRender = false;
		}
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x0007DD9D File Offset: 0x0007C19D
	private bool isSimulateGame()
	{
		return this.IsAuto && this.game != null && this.rollbackLayer != null && !this.rollbackLayer.HasDesynced && this.IsBattleReady;
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06001757 RID: 5975 RVA: 0x0007DDD9 File Offset: 0x0007C1D9
	public bool IsBattleReady
	{
		get
		{
			return !this.IsRollback || this.allPlayersReady || base.config.networkSettings.simulateRollback;
		}
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0007DE04 File Offset: 0x0007C204
	public void SetControlMode(FrameControlMode newMode)
	{
		this.controlMode = newMode;
		if (this.IsAuto && this.IsLocal)
		{
			this.rollbackLayer.Timekeeper.ResetMilestone(this.Frame);
		}
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0007DE3C File Offset: 0x0007C23C
	public void AdvanceFrame()
	{
		if (this.IsAuto && this.IsLocal)
		{
			Debug.LogError("Attempted to manually advance a frame while running normally");
			return;
		}
		bool flag = false;
		if (this.rollbackLayer.Idle(20, out flag, (!base.config.networkSettings.debugStepIntoRollback) ? -1 : 1) && !flag)
		{
			FrameController.FrameDeltaTime += WTime.frameTime;
			this.game.TickInput(this.state.currentFrame, false);
			this.game.TickFrame();
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0007DEDC File Offset: 0x0007C2DC
	public void OnFrameAdvanced()
	{
		this.state.currentFrame++;
		if (!this.rollbackLayer.OnFrameAdvanced())
		{
			Debug.LogError("Failed to step forward; found an error. Aborting");
			this.controlMode = FrameControlMode.Manual;
		}
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0007DF1F File Offset: 0x0007C31F
	public void SaveLocalInputs(int frame, List<RollbackInput> localInputs, bool broadcast)
	{
		this.rollbackLayer.SaveLocalInputs(frame, localInputs, broadcast);
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x0007DF2F File Offset: 0x0007C32F
	public void FillSkippedLocalInputFrame(int frame, RollbackInput input)
	{
		this.rollbackLayer.FillSkippedLocalInput(frame, input);
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x0007DF3E File Offset: 0x0007C33E
	public void SyncInputForFrame(ref RollbackInput[] frameInputs)
	{
		this.rollbackLayer.SyncInputForFrame(ref frameInputs);
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0007DF4C File Offset: 0x0007C34C
	private void onAllPlayersReady(GameEvent message)
	{
		this.allPlayersReady = true;
		this.rollbackLayer.Timekeeper.Start((!base.battleServerAPI.IsConnected) ? WTime.precisionTimeSinceStartup : ((double)base.gameController.matchStartTime));
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x0007DF8C File Offset: 0x0007C38C
	private void onGamePaused(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = message as GamePausedEvent;
		if (!gamePausedEvent.paused && this.IsLocal)
		{
			this.rollbackLayer.Timekeeper.ResetMilestone(this.Frame);
		}
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x0007DFCC File Offset: 0x0007C3CC
	private void onToggleFrameControlMode(GameEvent message)
	{
		if (this.IsManual)
		{
			this.SetControlMode(FrameControlMode.Auto);
		}
		else
		{
			this.SetControlMode(FrameControlMode.Manual);
		}
		base.events.Broadcast(new FrameControlModeChangedEvent(this.controlMode));
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x0007E004 File Offset: 0x0007C404
	private void onDebugAdvanceFrame(GameEvent message)
	{
		if (this.IsManual)
		{
			DebugAdvanceFrameEvent debugAdvanceFrameEvent = message as DebugAdvanceFrameEvent;
			for (int i = 0; i < debugAdvanceFrameEvent.frameCount; i++)
			{
				this.AdvanceFrame();
			}
		}
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x0007E040 File Offset: 0x0007C440
	private void onChangePlaybackSpeedCommand(GameEvent message)
	{
		if (!this.isReplaying)
		{
			return;
		}
		ChangePlaybackSpeedCommand changePlaybackSpeedCommand = message as ChangePlaybackSpeedCommand;
		ChangePlaybackSpeedType type = changePlaybackSpeedCommand.type;
		if (type != ChangePlaybackSpeedType.Decrease)
		{
			if (type != ChangePlaybackSpeedType.Increase)
			{
				if (type == ChangePlaybackSpeedType.Set)
				{
					this.rollbackLayer.Timekeeper.SetPlaybackSpeed(changePlaybackSpeedCommand.newSpeed);
				}
			}
			else
			{
				this.rollbackLayer.Timekeeper.IncreasePlaybackSpeed();
			}
		}
		else
		{
			this.rollbackLayer.Timekeeper.DecreasePlaybackSpeed();
		}
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x0007E0C8 File Offset: 0x0007C4C8
	private void onForceRollback(GameEvent message)
	{
		if (base.gameManager.IsPaused)
		{
			Debug.LogWarning("Cannot rollback while paused");
			return;
		}
		ForceRollbackCommand forceRollbackCommand = message as ForceRollbackCommand;
		int num = forceRollbackCommand.toFrame;
		if (forceRollbackCommand.delta < 0)
		{
			num = this.Frame + forceRollbackCommand.delta;
		}
		if (num > 0)
		{
			this.rollbackLayer.ForceRollback(num);
		}
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x0007E12C File Offset: 0x0007C52C
	public override void OnDestroy()
	{
		base.OnDestroy();
		base.events.Unsubscribe(typeof(ForceRollbackCommand), new Events.EventHandler(this.onForceRollback));
		base.events.Unsubscribe(typeof(AllPlayersReadyMessage), new Events.EventHandler(this.onAllPlayersReady));
		base.events.Unsubscribe(typeof(ToggleFrameControlModeCommand), new Events.EventHandler(this.onToggleFrameControlMode));
		base.events.Unsubscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onGamePaused));
		base.events.Unsubscribe(typeof(DebugAdvanceFrameEvent), new Events.EventHandler(this.onDebugAdvanceFrame));
		base.events.Unsubscribe(typeof(DebugThrowExceptionEvent), new Events.EventHandler(this.onDebugThrowExceptionEvent));
		base.events.Unsubscribe(typeof(ChangePlaybackSpeedCommand), new Events.EventHandler(this.onChangePlaybackSpeedCommand));
		this.rollbackLayer.Destroy();
		base.audioManager.OnGameDestroyed(this.Frame);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x0007E242 File Offset: 0x0007C642
	public void OnGameManagerDestroyed()
	{
		base.enabled = false;
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x0007E24B File Offset: 0x0007C64B
	private void onDebugThrowExceptionEvent(GameEvent message)
	{
		this.shouldThrowException = true;
	}

	// Token: 0x04001206 RID: 4614
	private static int MAX_TICKS = 12;

	// Token: 0x04001207 RID: 4615
	private FrameControllerState state = new FrameControllerState();

	// Token: 0x04001208 RID: 4616
	private FrameControlMode controlMode = FrameControlMode.Auto;

	// Token: 0x04001209 RID: 4617
	private SyncMode syncMode;

	// Token: 0x0400120A RID: 4618
	private bool isReplaying;

	// Token: 0x0400120B RID: 4619
	private bool shouldThrowException;

	// Token: 0x0400120D RID: 4621
	private bool needRender;

	// Token: 0x0400120E RID: 4622
	private float maxWaitTime = 0.016f;

	// Token: 0x0400120F RID: 4623
	private float maxRenderWaitTime = 0.017f;

	// Token: 0x04001210 RID: 4624
	private double prevRenderTime;

	// Token: 0x04001211 RID: 4625
	private RenderTracker renderTracker;

	// Token: 0x04001213 RID: 4627
	private bool allPlayersReady;

	// Token: 0x04001214 RID: 4628
	private bool needInitialSync = true;

	// Token: 0x04001215 RID: 4629
	private bool unused;

	// Token: 0x04001216 RID: 4630
	private bool isCurrentFrame = true;
}
