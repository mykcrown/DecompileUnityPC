// Decompile from assembly: Assembly-CSharp.dll

using RollbackDebug;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FrameController : GameBehavior, IRollbackStateOwner, IFrameOwner
{
	private static int MAX_TICKS = 12;

	private FrameControllerState state = new FrameControllerState();

	private FrameControlMode controlMode = FrameControlMode.Auto;

	private SyncMode syncMode;

	private bool isReplaying;

	private bool shouldThrowException;

	private bool needRender;

	private float maxWaitTime = 0.016f;

	private float maxRenderWaitTime = 0.017f;

	private double prevRenderTime;

	private RenderTracker renderTracker;

	private bool allPlayersReady;

	private bool needInitialSync = true;

	private bool unused;

	private bool isCurrentFrame = true;

	[Inject]
	public IServerConnectionManager serverManager
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

	[Inject]
	public INetworkHealthReport networkHealthReport
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

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IExceptionParser exceptionParser
	{
		get;
		set;
	}

	[Inject]
	public ICustomLobbyController lobbyController
	{
		get;
		set;
	}

	[Inject]
	public IMatchDeepLogging deepLogging
	{
		get;
		set;
	}

	[Inject]
	public IApplicationFramerateManager framerateManager
	{
		get;
		set;
	}

	public int Frame
	{
		get
		{
			return this.state.currentFrame;
		}
	}

	private IRollbackGameClient game
	{
		get
		{
			return base.gameController.currentGame;
		}
	}

	public IRollbackLayer rollbackLayer
	{
		get;
		private set;
	}

	public static float FrameDeltaTime
	{
		get;
		private set;
	}

	public bool IsManual
	{
		get
		{
			return this.controlMode == FrameControlMode.Manual;
		}
	}

	public bool IsAuto
	{
		get
		{
			return this.controlMode == FrameControlMode.Auto;
		}
	}

	public bool IsLocal
	{
		get
		{
			return this.syncMode == SyncMode.Local;
		}
	}

	public bool IsRollback
	{
		get
		{
			return this.syncMode == SyncMode.Rollback || base.config.networkSettings.simulateRollback;
		}
	}

	public bool IsCurrentFrame
	{
		get
		{
			return this.isCurrentFrame;
		}
	}

	public bool IsBattleReady
	{
		get
		{
			return !this.IsRollback || this.allPlayersReady || base.config.networkSettings.simulateRollback;
		}
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<FrameControllerState>(this.state));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<FrameControllerState>(ref this.state);
		return true;
	}

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
		RenderTracker expr_C0 = this.renderTracker;
		expr_C0.PreRenderCallback = (Action)Delegate.Combine(expr_C0.PreRenderCallback, new Action(this.onPreRender));
	}

	public void OnEndGame()
	{
		if (this.renderTracker != null)
		{
			RenderTracker expr_17 = this.renderTracker;
			expr_17.PreRenderCallback = (Action)Delegate.Remove(expr_17.PreRenderCallback, new Action(this.onPreRender));
		}
	}

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
			UnityEngine.Debug.LogError(e.StackTrace);
			throw e;
		}
		UnityEngine.Debug.LogError(e);
		UnityEngine.Debug.LogError(e.StackTrace);
	}

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

	private void LateUpdate()
	{
		FrameController.FrameDeltaTime = 0f;
		this.deepLogging.UpdateLoopEnd();
	}

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
				UnityEngine.Debug.LogError("Warning, max render wait exceeded. This should never happen and indicates a timing bug!!");
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

	private bool isSimulateGame()
	{
		return this.IsAuto && this.game != null && this.rollbackLayer != null && !this.rollbackLayer.HasDesynced && this.IsBattleReady;
	}

	public void SetControlMode(FrameControlMode newMode)
	{
		this.controlMode = newMode;
		if (this.IsAuto && this.IsLocal)
		{
			this.rollbackLayer.Timekeeper.ResetMilestone(this.Frame);
		}
	}

	public void AdvanceFrame()
	{
		if (this.IsAuto && this.IsLocal)
		{
			UnityEngine.Debug.LogError("Attempted to manually advance a frame while running normally");
			return;
		}
		bool flag = false;
		if (this.rollbackLayer.Idle(20, out flag, (!base.config.networkSettings.debugStepIntoRollback) ? (-1) : 1) && !flag)
		{
			FrameController.FrameDeltaTime += WTime.frameTime;
			this.game.TickInput(this.state.currentFrame, false);
			this.game.TickFrame();
		}
	}

	public void OnFrameAdvanced()
	{
		this.state.currentFrame++;
		if (!this.rollbackLayer.OnFrameAdvanced())
		{
			UnityEngine.Debug.LogError("Failed to step forward; found an error. Aborting");
			this.controlMode = FrameControlMode.Manual;
		}
	}

	public void SaveLocalInputs(int frame, List<RollbackInput> localInputs, bool broadcast)
	{
		this.rollbackLayer.SaveLocalInputs(frame, localInputs, broadcast);
	}

	public void FillSkippedLocalInputFrame(int frame, RollbackInput input)
	{
		this.rollbackLayer.FillSkippedLocalInput(frame, input);
	}

	public void SyncInputForFrame(ref RollbackInput[] frameInputs)
	{
		this.rollbackLayer.SyncInputForFrame(ref frameInputs);
	}

	private void onAllPlayersReady(GameEvent message)
	{
		this.allPlayersReady = true;
		this.rollbackLayer.Timekeeper.Start((!base.battleServerAPI.IsConnected) ? WTime.precisionTimeSinceStartup : ((double)base.gameController.matchStartTime));
	}

	private void onGamePaused(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = message as GamePausedEvent;
		if (!gamePausedEvent.paused && this.IsLocal)
		{
			this.rollbackLayer.Timekeeper.ResetMilestone(this.Frame);
		}
	}

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

	private void onForceRollback(GameEvent message)
	{
		if (base.gameManager.IsPaused)
		{
			UnityEngine.Debug.LogWarning("Cannot rollback while paused");
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

	public void OnGameManagerDestroyed()
	{
		base.enabled = false;
	}

	private void onDebugThrowExceptionEvent(GameEvent message)
	{
		this.shouldThrowException = true;
	}
}
