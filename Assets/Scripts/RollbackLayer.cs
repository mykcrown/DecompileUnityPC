// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using P2P;
using RollbackDebug;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RollbackLayer : IRollbackLayer, IRollbackStatus
{
	private sealed class _delayedRemoteInputPost_c__AnonStorey0
	{
		internal InputEvent inputEvent;

		internal RollbackLayer _this;

		internal void __m__0()
		{
			this._this.onRemoteInput(this.inputEvent);
		}
	}

	public const int MAX_FRAMES = 30600;

	protected bool enablePrediction = true;

	protected bool isSimulating;

	protected RollbackSettings settings;

	private Dictionary<int, InputEvent> inputEventBufferToPlayer = new Dictionary<int, InputEvent>();

	private RequestMissingInputEvent requestMissingInputEventBuffer = new RequestMissingInputEvent();

	private DisconnectAckEvent disconnectAckEventBuffer = new DisconnectAckEvent();

	private RollbackInputFrame nullInputFrame = new RollbackInputFrame();

	protected Dictionary<int, int> playerIDtoIndex = new Dictionary<int, int>();

	protected Dictionary<int, int> playerIndextoID = new Dictionary<int, int>();

	private Dictionary<int, RollbackPlayerData> playerDataList = new Dictionary<int, RollbackPlayerData>();

	private RollbackPlayerData localPlayer;

	protected int fullySynchronizedFrame;

	protected List<string> errors = new List<string>();

	protected int playerCount = 2;

	protected int rollbackToFrame;

	protected int currentFrame;

	private long notFrameTimeBeginMs = -1L;

	private bool didFailsafeExit;

	protected bool beginLocalPlayerQuit;

	protected IReplaySystem replaySystem;

	protected IRollbackLayerDebugger rollbackDebugger;

	private IRollbackInputTracker inputTracker;

	private IRollbackQuitGame quitGameHelper;

	private GenericResetObjectPool<RollbackInput> rollbackInputPool;

	int IRollbackStatus.FullyConfirmedFrame
	{
		get
		{
			return this.inputTracker.FrameWithAllInputs;
		}
	}

	int IRollbackStatus.LowestInputAckFrame
	{
		get
		{
			return this.inputTracker.LowestInputAckFrame;
		}
	}

	int IRollbackStatus.CalculatedLatencyMs
	{
		get
		{
			return this.inputTracker.CalculatedLatencyMs;
		}
	}

	long IRollbackStatus.MsSinceStart
	{
		get
		{
			return (long)this.Timekeeper.MsSinceStart;
		}
	}

	bool IRollbackLayer.HasStarted
	{
		get
		{
			return this.isSimulating || this.replaySystem.IsReplaying;
		}
	}

	int IRollbackStatus.InputDelayFrames
	{
		get
		{
			return this.settings.inputDelayFrames;
		}
	}

	int IRollbackStatus.InputDelayPing
	{
		get
		{
			return this.settings.inputDelayPing;
		}
	}

	int IRollbackStatus.InitialTimestepDelta
	{
		get
		{
			return this.settings.initialTimestepDelta;
		}
	}

	[Inject]
	public IBattleServerAPI battleServer
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IPerformanceTracker performanceTracker
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

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
	public IMatchDeepLogging deepLogging
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
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
	public P2PHost p2pHost
	{
		private get;
		set;
	}

	[Inject]
	public IEvents events
	{
		private get;
		set;
	}

	public bool HasDesynced
	{
		get;
		private set;
	}

	public ITimekeeper Timekeeper
	{
		get;
		private set;
	}

	public bool RollbackEnabled
	{
		get
		{
			return this.battleServer.IsConnected || this.settings.networkSettings.simulateRollback || this.settings.networkSettings.simulatedLatencyFrames > 0;
		}
	}

	public bool IsRollingBack
	{
		get
		{
			return this.rollbackToFrame != 0 && this.rollbackToFrame <= this.inputTracker.FrameWithAllInputs;
		}
	}

	private bool recordStates
	{
		get
		{
			return this.RollbackEnabled;
		}
	}

	protected IRollbackClient client
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	private bool debugSimulatingLatencyForPlayerID(int playerID)
	{
		return Application.isEditor && this.settings.networkSettings.simulatedLatencyFrames > 0;
	}

	public void Init(RollbackSettings settings, IRollbackLayerDebugger debugger)
	{
		this.settings = settings;
		this.beginLocalPlayerQuit = false;
		this.rollbackDebugger = debugger;
		this.rollbackInputPool = settings.rollbackInputPool;
		this.playerCount = settings.playerCount;
		this.replaySystem = settings.replaySystem;
		this.notFrameTimeBeginMs = -1L;
		this.didFailsafeExit = false;
		this.p2pHost.playerDataList = this.playerDataList;
		this.playerDataList.Clear();
		for (int i = 0; i < settings.playerDataList.Count; i++)
		{
			RollbackPlayerData rollbackPlayerData = settings.playerDataList[i];
			this.playerDataList[rollbackPlayerData.playerID] = rollbackPlayerData;
			this.inputEventBufferToPlayer[rollbackPlayerData.playerID] = new InputEvent();
			this.playerIDtoIndex[rollbackPlayerData.playerID] = i;
			this.playerIndextoID[i] = rollbackPlayerData.playerID;
			if (rollbackPlayerData.isLocal)
			{
				this.localPlayer = rollbackPlayerData;
			}
		}
		this.isSimulating = settings.simulate;
		this.battleServer.Listen<InputEvent>(new ServerEventHandler(this.onRemoteInput));
		this.battleServer.Listen<RequestMissingInputEvent>(new ServerEventHandler(this.onRequestMissingInput));
		this.battleServer.Listen<DisconnectEvent>(new ServerEventHandler(this.onDisconnect));
		this.signalBus.GetSignal<QuitGameSignal>().AddListener(new Action(this.quitGameSignal));
		this.Timekeeper = new RollbackTimekeeper();
		this.injector.Inject(this.Timekeeper);
		this.Timekeeper.Init(settings);
		this.quitGameHelper = this.injector.GetInstance<IRollbackQuitGame>();
		this.quitGameHelper.Init(this.battleServer, settings);
		this.inputTracker = new RollbackInputTracker(this.playerCount, this.battleServer, settings, this.Timekeeper);
		this.nullInputFrame.inputs = new RollbackInput[this.playerCount];
		for (int j = 0; j < this.nullInputFrame.inputs.Length; j++)
		{
			this.nullInputFrame.inputs[j] = new RollbackInput();
			this.nullInputFrame.inputs[j].playerID = 255;
		}
		this.rollbackStatePool.InitiailizeForMatch(settings.playerCount);
	}

	public void StartSession(int localPort, string remoteip, int remoteport)
	{
	}

	private int getProbableHost()
	{
		int num = 2147483647;
		foreach (KeyValuePair<int, RollbackPlayerData> current in this.playerDataList)
		{
			RollbackPlayerData value = current.Value;
			if (!value.isSpectator && value.disconnectFrame == -1 && value.playerID < num)
			{
				num = value.playerID;
			}
		}
		if (num == 2147483647)
		{
			return -1;
		}
		return num;
	}

	private bool isProbablyHost()
	{
		return this.getProbableHost() == this.localPlayer.playerID;
	}

	private bool isSpectator(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isSpectator;
		}
		throw new Exception("Tried to check spectator status on invalid player " + playerID);
	}

	private bool isDisconnected(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame != -1;
		}
		throw new Exception("Tried to check disconnect status on invalid player " + playerID);
	}

	private int getDisconnectFrame(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame;
		}
		throw new Exception("Tried to check disconnect status on invalid player " + playerID);
	}

	private bool isLocal(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isLocal;
		}
		throw new Exception("Tried to check local status on invalid player " + playerID);
	}

	private ulong getUserID(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.userID;
		}
		throw new Exception("Tried to check spectator status on invalid player " + playerID);
	}

	private RollbackInputFrame getQueuedInputsForFrame(int frame)
	{
		if (frame < this.client.GameStartInputFrame)
		{
			return this.nullInputFrame;
		}
		return this.rollbackStatePool.GetQueuedInputsForFrame(frame);
	}

	public void ResetToFrame(int frame)
	{
		this.Timekeeper.ResetMilestone(frame);
		this.rollbackStatePool.ResetQueuedInputs(frame);
		this.inputTracker.ResetToFrame(frame);
		this.updateCurrentFrame(frame);
	}

	private void updateCurrentFrame(int frame)
	{
		this.currentFrame = frame;
		this.rollbackStatePool.CachedCurrentFrame = this.currentFrame;
	}

	private void onRequestMissingInput(ServerEvent message)
	{
		if (this.client == null)
		{
			return;
		}
		RequestMissingInputEvent requestMissingInputEvent = message as RequestMissingInputEvent;
		int forPlayer = requestMissingInputEvent.forPlayer;
		int fromPlayer = requestMissingInputEvent.fromPlayer;
		int startFrame = requestMissingInputEvent.startFrame;
		int num = this.inputTracker.PlayerFrameReceived[forPlayer];
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Received request missing input event, send inputs for player ",
			forPlayer,
			" to ",
			fromPlayer,
			": frames ",
			startFrame,
			" - ",
			num
		}));
		if (startFrame <= num)
		{
			int num2 = 30;
			if (num - startFrame > num2)
			{
				num = startFrame + num2 - 1;
			}
			InputEvent inputEvent = null;
			this.inputEventBufferToPlayer.TryGetValue(fromPlayer, out inputEvent);
			inputEvent.startFrame = startFrame;
			inputEvent.playerID = (byte)forPlayer;
			inputEvent.numInputs = (byte)(num - startFrame + 1);
			int num3 = 0;
			for (int i = startFrame; i <= num; i++)
			{
				inputEvent.inputArray[num3] = this.inputTracker.StoredRemoteInputs[forPlayer][i];
				num3++;
			}
			inputEvent._targetUserID = this.getUserID(fromPlayer);
			this.battleServer.QueueUnreliableMessage(inputEvent);
		}
	}

	private void onRemoteInput(ServerEvent message)
	{
		InputEvent inputEvent = message as InputEvent;
		this.deepLogging.RecordRemoteInput((int)inputEvent.playerID, inputEvent.startFrame, (int)inputEvent.numInputs);
		if (this.client == null)
		{
			return;
		}
		RollbackInput rollbackInput = this.rollbackInputPool.New();
		rollbackInput.playerID = (int)inputEvent.playerID;
		if (this.isLocal(rollbackInput.playerID))
		{
			throw new Exception("Should never get local input " + rollbackInput.playerID);
		}
		if (this.isDisconnected(rollbackInput.playerID))
		{
			UnityEngine.Debug.LogWarning("Ignoring input from disconnected player");
			return;
		}
		int num = this.playerIDtoIndex[rollbackInput.playerID];
		for (int i = 0; i < (int)inputEvent.numInputs; i++)
		{
			int frame = inputEvent.startFrame + i;
			if (!this.inputTracker.HasPlayerInputForFrame(frame, (int)inputEvent.playerID))
			{
				this.inputTracker.StoreReceivedInputData(frame, (int)inputEvent.playerID, inputEvent.inputArray[i]);
				rollbackInput.frame = frame;
				if (inputEvent.inputArray[i].usePreviousFrame)
				{
					RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(rollbackInput.frame - 1);
					if (!queuedInputsForFrame.hasInputsSet)
					{
						throw new Exception("FrameBufferTooSmall");
					}
					rollbackInput.values.CopyFrom(queuedInputsForFrame.inputs[num].values);
				}
				else
				{
					rollbackInput.values.buttons = inputEvent.inputArray[i].buttons;
					rollbackInput.values.axes = inputEvent.inputArray[i].axes;
					rollbackInput.values.inputFlags = inputEvent.inputArray[i].flags;
				}
				rollbackInput.usedPreviousFrame = inputEvent.inputArray[i].usePreviousFrame;
				this.receiveRemoteInput(rollbackInput, inputEvent.startFrame, i);
			}
		}
		this.inputTracker.RecordInputAck(rollbackInput.playerID, inputEvent);
		this.rollbackInputPool.Store(rollbackInput);
	}

	private RollbackInput[] getInputsForFrame(int frame)
	{
		RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(frame);
		if (!queuedInputsForFrame.hasInputsSet)
		{
			queuedInputsForFrame.frame = frame;
			queuedInputsForFrame.hasInputsSet = true;
			for (int i = 0; i < this.playerCount; i++)
			{
				int num = this.playerIndextoID[i];
				queuedInputsForFrame.inputs[i].Reset();
				if (!this.isLocal(num) && this.enablePrediction)
				{
					this.predictRemoteInput(i, frame, queuedInputsForFrame.inputs[i]);
				}
				queuedInputsForFrame.inputs[i].frame = frame;
				queuedInputsForFrame.inputs[i].playerID = (int)((byte)num);
			}
		}
		return queuedInputsForFrame.inputs;
	}

	private void predictRemoteInput(int playerIndex, int frame, RollbackInput newInput)
	{
		if (frame > 0)
		{
			RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(frame - 1);
			if (queuedInputsForFrame.hasInputsSet)
			{
				newInput.values.CopyFrom(queuedInputsForFrame.inputs[playerIndex].values);
				newInput.usedPreviousFrame = true;
			}
		}
	}

	private bool addInput(RollbackInput input, int recordedFrame)
	{
		int num = this.playerIDtoIndex[input.playerID];
		RollbackInput[] inputsForFrame = this.getInputsForFrame(recordedFrame);
		inputsForFrame[num].values.CopyFrom(input.values);
		inputsForFrame[num].usedPreviousFrame = input.usedPreviousFrame;
		return true;
	}

	public void FillSkippedLocalInput(int frame, RollbackInput input)
	{
		int frame2 = frame + this.settings.inputDelayFrames;
		int playerIndex = this.playerIDtoIndex[input.playerID];
		this.predictRemoteInput(playerIndex, frame2, input);
	}

	private void receiveRemoteInput(RollbackInput remoteInput, int d_startFrame, int d_index)
	{
		bool flag = true;
		if (this.client != null && this.client.Frame < this.currentFrame)
		{
			return;
		}
		if (remoteInput.frame < this.currentFrame)
		{
			int num = this.playerIDtoIndex[remoteInput.playerID];
			RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(remoteInput.frame);
			if (queuedInputsForFrame.hasInputsSet && queuedInputsForFrame.inputs[num].Equals(remoteInput) && !this.debugSimulatingLatencyForPlayerID(remoteInput.playerID))
			{
				flag = false;
				this.inputTracker.ConfirmInput(this.client, remoteInput.frame, remoteInput.playerID, d_startFrame, d_index);
			}
			else
			{
				this.addRollback(remoteInput.frame);
			}
		}
		this.replaySystem.RecordRemoteRollbackInput(this.currentFrame, remoteInput);
		if (flag)
		{
			if (remoteInput.frame <= this.currentFrame)
			{
				int num2 = this.currentFrame + this.settings.inputDelayFrames;
				for (int i = remoteInput.frame; i <= num2; i++)
				{
					if (this.inputTracker.HasPlayerInputForFrame(i, remoteInput.playerID))
					{
						throw new Exception("StompingInput2");
					}
					this.addInput(remoteInput, i);
				}
			}
			else
			{
				if (this.inputTracker.HasPlayerInputForFrame(remoteInput.frame, remoteInput.playerID))
				{
					throw new Exception("StompingInput");
				}
				this.addInput(remoteInput, remoteInput.frame);
			}
			this.inputTracker.ConfirmInput(this.client, remoteInput.frame, remoteInput.playerID, d_startFrame, d_index);
		}
	}

	private void recordReplayState()
	{
		if (this.recordStates)
		{
			RollbackStateContainer rollbackStateContainer = null;
			if (this.client.Frame % 1 == 0)
			{
				this.rollbackStatePool.GetRollbackState(this.client.Frame, ref rollbackStateContainer);
				this.client.ExportState(ref rollbackStateContainer);
			}
			if (this.replaySystem.IsRecording)
			{
				if (this.replaySystem.RecordStates || rollbackStateContainer == null)
				{
					rollbackStateContainer = new RollbackStateContainer(true);
					this.client.ExportState(ref rollbackStateContainer);
				}
				this.replaySystem.RecordState(this.client.Frame, rollbackStateContainer);
			}
		}
	}

	private void replayRemoteInputs()
	{
		List<RollbackInput> remoteInputForFrame = this.replaySystem.GetRemoteInputForFrame(this.client.Frame);
		if (remoteInputForFrame != null)
		{
			for (int i = 0; i < remoteInputForFrame.Count; i++)
			{
				this.receiveRemoteInput(remoteInputForFrame[i], 0, i);
			}
		}
	}

	private void autoRollback()
	{
		if (this.settings.simulate && this.settings.autoRollbackSimulationRate > 0 && !this.IsRollingBack && this.client.Frame % this.settings.autoRollbackSimulationRate == 0 && this.client.Frame > this.settings.autoRollbackSimulationAmount)
		{
			this.rollbackToFrame = this.client.Frame - this.settings.autoRollbackSimulationAmount;
		}
	}

	public bool OnFrameAdvanced()
	{
		this.recordReplayState();
		if (this.client.Frame > this.currentFrame)
		{
			this.updateCurrentFrame(this.client.Frame);
			this.replayRemoteInputs();
			this.autoRollback();
		}
		return true;
	}

	private bool loadRollbackFrame()
	{
		if (this.currentFrame - this.rollbackToFrame >= RollbackStatePoolContainer.ROLLBACK_FRAMES)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Exceeded rollback frame amount! ",
				this.currentFrame,
				" - ",
				this.rollbackToFrame,
				" > ",
				RollbackStatePoolContainer.ROLLBACK_FRAMES,
				" * ",
				1
			}));
		}
		this.client.IsRollingBack = true;
		this.client.LoadState(this.rollbackStatePool.GetRollbackState(this.rollbackToFrame));
		if (this.replaySystem.EnableRuntimeValidation)
		{
			RollbackStateContainer activeState = new RollbackStateContainer(true);
			this.client.ExportState(ref activeState);
			RollbackMismatchReport rollbackMismatchReport;
			if (!this.rollbackDebugger.TestStates(activeState, this.client.Frame, out rollbackMismatchReport))
			{
				this.client.Halt();
				this.fullySynchronizedFrame = this.currentFrame;
				return false;
			}
			this.errors.Add("Tested rolledback state " + this.client.Frame + ", checks out");
		}
		this.rollbackStatePool.GetRollbackState(this.rollbackToFrame).ResetIndex();
		if (this.rollbackToFrame - this.client.Frame >= 1 || this.client.Frame > this.rollbackToFrame)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Rollback frame mismatch: ",
				this.client.Frame,
				" != ",
				this.rollbackToFrame,
				" (granularity: ",
				1
			}));
		}
		return true;
	}

	private void catchupRollbackFrames(int currentIterations, int maxTicks, out bool clientTicked)
	{
		clientTicked = false;
		if (maxTicks < 0)
		{
			maxTicks = RollbackStatePoolContainer.ROLLBACK_FRAMES;
		}
		DateTime utcNow = DateTime.UtcNow;
		int num = 0;
		while (this.client.Frame < this.currentFrame && currentIterations < maxTicks)
		{
			currentIterations++;
			num++;
			this.client.TickFrame();
			clientTicked = true;
			if (this.rollbackToFrame <= this.inputTracker.FrameWithAllInputs)
			{
				this.rollbackToFrame++;
			}
		}
		if (num > 0)
		{
			double totalMilliseconds = (DateTime.UtcNow - utcNow).TotalMilliseconds;
			this.performanceTracker.RecordRollbackDuration(totalMilliseconds);
			this.performanceTracker.RecordRollbackFrames(num);
			this.deepLogging.RecordRollback(num, totalMilliseconds);
		}
	}

	private bool fullySynchronizeFrame()
	{
		if (this.inputTracker.FrameWithAllInputs > this.fullySynchronizedFrame)
		{
			int num = this.fullySynchronizedFrame;
			int num2 = Mathf.Min(this.client.Frame, this.inputTracker.FrameWithAllInputs);
			for (int i = this.fullySynchronizedFrame + 1; i <= num2; i++)
			{
				if (i != 0)
				{
					num = i;
					if (this.recordStates && i % 1 == 0 && this.currentFrame - i < RollbackStatePoolContainer.ROLLBACK_FRAMES)
					{
						RollbackStateContainer rollbackState = this.rollbackStatePool.GetRollbackState(i);
						if (this.replaySystem.IsReplaying && this.replaySystem.EnableRuntimeValidation)
						{
							RollbackMismatchReport rollbackMismatchReport;
							if (!this.rollbackDebugger.TestStates(rollbackState, i, out rollbackMismatchReport))
							{
								this.client.Halt();
								this.fullySynchronizedFrame = this.currentFrame;
								return false;
							}
							this.errors.Add("Tested confirmed state " + i + ", checks out");
						}
						short memberwiseHashCode = rollbackState.GetMemberwiseHashCode();
						if (i < this.client.GameStartInputFrame || !this.localPlayer.isSpectator)
						{
						}
					}
				}
			}
			this.fullySynchronizedFrame = num;
		}
		return true;
	}

	private void onFinishRollback(int currentIterations, int maxTicks)
	{
		if (currentIterations == maxTicks && maxTicks == RollbackStatePoolContainer.ROLLBACK_FRAMES)
		{
			UnityEngine.Debug.Log("Infinite loop! The rollback layer cannot correctly catch up to the current state.");
		}
		if (this.client.Frame == this.currentFrame)
		{
			if (this.rollbackToFrame == this.currentFrame)
			{
				this.rollbackToFrame = 0;
			}
			this.client.IsRollingBack = false;
		}
		if (this.errors.Count > 0)
		{
			this.client.ReportErrors(this.errors);
			this.errors.Clear();
		}
	}

	public bool Idle(int timeoutMs, out bool clientTicked, int maxTicks = -1)
	{
		clientTicked = false;
		int currentIterations = 0;
		if (this.IsRollingBack && this.rollbackToFrame < this.client.Frame)
		{
			currentIterations = 1;
			if (!this.loadRollbackFrame())
			{
				return false;
			}
		}
		this.catchupRollbackFrames(currentIterations, maxTicks, out clientTicked);
		if (!this.fullySynchronizeFrame())
		{
			return false;
		}
		this.onFinishRollback(currentIterations, maxTicks);
		return this.client.Frame == this.currentFrame;
	}

	public void CheckMissingInputs()
	{
		this.checkInputRerouteFromOtherPlayers();
		this.checkForceDisconnect();
	}

	private bool canForceDisconnectThisPlayer(int playerID)
	{
		int num = 2147483647;
		foreach (KeyValuePair<int, RollbackPlayerData> current in this.playerDataList)
		{
			RollbackPlayerData value = current.Value;
			if (!value.isSpectator && value.disconnectFrame == -1 && value.playerID != playerID && value.playerID < num)
			{
				num = value.playerID;
			}
		}
		return num == this.localPlayer.playerID || num == 2147483647;
	}

	private void checkForceDisconnect()
	{
		int num = 20;
		int num2 = Mathf.Max(num, RollbackStatePoolContainer.ROLLBACK_FRAMES - num);
		if (this.client == null || this.client.Frame <= this.client.GameStartInputFrame + num2)
		{
			return;
		}
		int frame = this.client.Frame;
		int num3 = 0;
		foreach (KeyValuePair<int, RollbackPlayerData> current in this.playerDataList)
		{
			if (!current.Value.isSpectator && current.Value.disconnectFrame == -1)
			{
				num3++;
			}
		}
		if (num3 > 2)
		{
			foreach (KeyValuePair<int, int> current2 in this.inputTracker.PlayerFrameReceived)
			{
				int key = current2.Key;
				int num4 = current2.Value;
				if (key != this.localPlayer.playerID && !this.isSpectator(key) && !this.isDisconnected(key))
				{
					if (this.canForceDisconnectThisPlayer(key))
					{
						if (num4 < this.client.GameStartInputFrame - 1)
						{
							num4 = this.client.GameStartInputFrame - 1;
						}
						int num5 = frame - num4;
						if (num5 >= num2)
						{
							UnityEngine.Debug.LogError(string.Concat(new object[]
							{
								"I, ",
								this.localPlayer.playerID,
								", am taking host initiative and will disconnect ",
								key
							}));
							int frame2 = num4 - 15;
							this.forcePlayerQuit(key, frame2);
							this.addRollback(frame2);
						}
					}
				}
			}
		}
	}

	private void forcePlayerQuit(int playerID, int frame)
	{
		if (!this.quitGameHelper.IsQuitting(playerID))
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, int> current in this.inputTracker.PlayerInputAckStatus)
			{
				if (!this.isDisconnected(current.Key) && !this.isLocal(current.Key))
				{
					list.Add(current.Key);
				}
			}
			RollbackPlayerData rollbackPlayerData;
			if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
			{
				rollbackPlayerData.disconnectFrame = frame;
			}
			this.quitGameHelper.Setup(playerID, frame, list);
			this.gameController.currentGame.events.Broadcast(new DisconnectPlayerCommand(PlayerUtil.GetPlayerNumFromInt(playerID, false), frame));
		}
	}

	private void checkInputRerouteFromOtherPlayers()
	{
		int num = 20;
		if (this.client == null || this.client.Frame <= this.client.GameStartInputFrame + num)
		{
			return;
		}
		if (this.inputTracker.PlayerFrameReceived.Count > 2)
		{
			int frame = this.client.Frame;
			foreach (KeyValuePair<int, int> current in this.inputTracker.PlayerFrameReceived)
			{
				int key = current.Key;
				int num2 = current.Value;
				if (key != this.localPlayer.playerID && !this.isSpectator(key))
				{
					if (num2 < this.client.GameStartInputFrame - 1)
					{
						num2 = this.client.GameStartInputFrame - 1;
					}
					int num3 = frame - num2;
					int disconnectFrame = this.getDisconnectFrame(key);
					if (disconnectFrame != -1 && num2 >= disconnectFrame - 1)
					{
						num3 = 0;
					}
					if (num3 >= num || (num3 > 0 && disconnectFrame != -1))
					{
						foreach (KeyValuePair<int, int> current2 in this.inputTracker.PlayerFrameReceived)
						{
							int key2 = current2.Key;
							if (key2 != this.localPlayer.playerID && key2 != key && !this.isSpectator(key2) && !this.isDisconnected(key2))
							{
								RequestMissingInputEvent requestMissingInputEvent = this.requestMissingInputEventBuffer;
								requestMissingInputEvent.fromPlayer = this.localPlayer.playerID;
								requestMissingInputEvent.forPlayer = key;
								requestMissingInputEvent.startFrame = num2 + 1;
								requestMissingInputEvent._targetUserID = this.getUserID(key2);
								this.battleServer.QueueUnreliableMessage(requestMissingInputEvent);
							}
						}
					}
				}
			}
		}
	}

	public void TickGameQuit()
	{
		this.quitGameHelper.Tick();
	}

	public void SaveLocalInputs(int frame, List<RollbackInput> localInputs, bool broadcast)
	{
		int num = frame + this.settings.inputDelayFrames;
		if (num >= this.currentFrame)
		{
			for (int i = 0; i < localInputs.Count; i++)
			{
				RollbackInput rollbackInput = localInputs[i];
				rollbackInput.frame = num;
				rollbackInput.usedPreviousFrame = false;
				this.addInput(rollbackInput, rollbackInput.frame);
				if (!this.debugSimulatingLatencyForPlayerID(rollbackInput.playerID) || rollbackInput.frame < this.client.GameStartInputFrame)
				{
					this.inputTracker.ConfirmInput(this.client, rollbackInput.frame, rollbackInput.playerID, rollbackInput.frame, 0);
				}
			}
			if (broadcast && (this.battleServer.IsConnected || this.debugSimulatingLatencyForPlayerID(0)))
			{
				if (this.beginLocalPlayerQuit)
				{
					this.localBeginQuitGame(num);
				}
				else if (this.localPlayer.disconnectFrame == -1)
				{
					foreach (KeyValuePair<int, int> current in this.inputTracker.PlayerInputAckStatus)
					{
						int key = current.Key;
						int value = current.Value;
						if (!this.isDisconnected(key))
						{
							this.deepLogging.RecordBroadcastLocalInputs(this.localPlayer.playerID, key, value);
							this.broadcastLocalInputSet(value, num, key);
						}
					}
				}
				this.inputTracker.SendAllInputAcks();
			}
		}
	}

	public void ValidFrame()
	{
		this.notFrameTimeBeginMs = -1L;
	}

	public void TickNotFrame()
	{
		this.inputTracker.SendAllInputAcks();
		if (this.inputTracker.StoredSentInputsLastFrame.Count > 0)
		{
			foreach (InputEvent current in this.inputTracker.StoredSentInputsLastFrame)
			{
				this.battleServer.QueueUnreliableMessage(current);
			}
		}
		if (!this.didFailsafeExit)
		{
			if (this.notFrameTimeBeginMs == -1L)
			{
				this.notFrameTimeBeginMs = WTime.currentTimeMs;
			}
			long num = WTime.currentTimeMs - this.notFrameTimeBeginMs;
			if (num >= 8000L)
			{
				UnityEngine.Debug.LogError("Quitting after 8 seconds frozen");
				this.didFailsafeExit = true;
				this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDisconnected));
			}
		}
	}

	public void SyncInputForFrame(ref RollbackInput[] frameInputs)
	{
		frameInputs = this.getInputsForFrame(this.client.Frame);
		this.replaySystem.SynchronizeInput(this.client.Frame, ref frameInputs);
		if (this.client.Frame >= this.currentFrame)
		{
			RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(this.client.Frame);
			queuedInputsForFrame.inputs = frameInputs;
			queuedInputsForFrame.hasInputsSet = true;
			if (this.replaySystem.IsReplaying && !this.replaySystem.ContainsRemoteInputs && !this.debugSimulatingLatencyForPlayerID(0))
			{
				foreach (int current in this.playerIndextoID.Keys)
				{
					int playerID = this.playerIndextoID[current];
					if (!this.isLocal(playerID))
					{
						this.inputTracker.ConfirmInput(this.client, this.client.Frame, this.playerIndextoID[current], this.client.Frame, 0);
					}
				}
			}
		}
	}

	private void addLocalInputToInputEvent(RollbackInput[] inputs, InputEvent inputEvent, int localPlayerIndex, int inputIndex)
	{
		RollbackInput rollbackInput = inputs[localPlayerIndex];
		RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(rollbackInput.frame - 1);
		RollbackInput other = queuedInputsForFrame.inputs[localPlayerIndex];
		if (rollbackInput.frame > this.client.GameStartInputFrame && queuedInputsForFrame.hasInputsSet && rollbackInput.Equals(other))
		{
			inputEvent.inputArray[inputIndex].UsePreviousFrame();
			rollbackInput.usedPreviousFrame = true;
		}
		else
		{
			inputEvent.inputArray[inputIndex].Set(rollbackInput.values.buttons, rollbackInput.values.inputFlags, rollbackInput.values.axes);
			rollbackInput.usedPreviousFrame = false;
		}
	}

	private void broadcastLocalInputSet(int startFrame, int endFrame, int targetPlayerID)
	{
		if (startFrame < 0 || startFrame > endFrame)
		{
			startFrame = endFrame;
		}
		startFrame = Mathf.Max(startFrame, this.client.GameStartInputFrame);
		if (startFrame > endFrame)
		{
			return;
		}
		if (endFrame - startFrame >= RollbackStatePoolContainer.INPUT_FRAMES_HISTORY - 1 && this.isSpectator(targetPlayerID))
		{
			return;
		}
		int num = 25;
		if (endFrame - startFrame >= num)
		{
			endFrame = startFrame + num - 1;
		}
		InputEvent inputEvent = null;
		this.inputEventBufferToPlayer.TryGetValue(targetPlayerID, out inputEvent);
		inputEvent.startFrame = startFrame;
		inputEvent.numInputs = (byte)(endFrame - startFrame + 1);
		this.inputTracker.ResetLatestSentInput();
		foreach (int current in this.playerIndextoID.Keys)
		{
			int num2 = this.playerIndextoID[current];
			if (this.isLocal(num2))
			{
				if (this.debugSimulatingLatencyForPlayerID(num2))
				{
					inputEvent = new InputEvent();
					inputEvent.startFrame = startFrame;
					inputEvent.numInputs = (byte)(endFrame - startFrame + 1);
				}
				inputEvent.playerID = (byte)num2;
				int num3 = 0;
				int localPlayerIndex = current;
				for (int i = startFrame; i <= endFrame; i++)
				{
					RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(i);
					if (!queuedInputsForFrame.hasInputsSet)
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
						{
							"Tried to broadcast local inputs for nonexistant frame ",
							i,
							".",
							startFrame,
							".",
							endFrame
						}));
						throw new Exception("Uninitialized Input");
					}
					RollbackInput[] inputs = queuedInputsForFrame.inputs;
					this.addLocalInputToInputEvent(inputs, inputEvent, localPlayerIndex, num3);
					num3++;
				}
				if (this.debugSimulatingLatencyForPlayerID(num2))
				{
					this.delayedRemoteInputPost(inputEvent);
				}
				else
				{
					inputEvent._targetUserID = this.getUserID(targetPlayerID);
					this.battleServer.QueueUnreliableMessage(inputEvent);
					this.inputTracker.RecordFrameSent(inputEvent);
					this.inputTracker.AddLatestSendInput(inputEvent);
				}
			}
		}
	}

	private void quitGameSignal()
	{
		this.beginLocalPlayerQuit = true;
	}

	private void localBeginQuitGame(int frame)
	{
		this.beginLocalPlayerQuit = false;
		this.forcePlayerQuit(this.localPlayer.playerID, frame);
	}

	private void onDisconnect(ServerEvent message)
	{
		if (this.client == null)
		{
			return;
		}
		DisconnectEvent disconnectEvent = message as DisconnectEvent;
		int playerID = disconnectEvent.playerID;
		int frame = disconnectEvent.frame;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Force disconnect from frame ",
			frame,
			" ",
			playerID,
			" at frame ",
			this.currentFrame
		}));
		RollbackPlayerData rollbackPlayerData;
		if (!this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return;
		}
		if (rollbackPlayerData.disconnectFrame == -1)
		{
			rollbackPlayerData.disconnectFrame = frame;
			this.gameController.currentGame.events.Broadcast(new DisconnectPlayerCommand(PlayerUtil.GetPlayerNumFromInt(playerID, false), frame));
			this.addRollback(frame);
		}
		DisconnectAckEvent disconnectAckEvent = this.disconnectAckEventBuffer;
		disconnectAckEvent.senderID = this.localPlayer.playerID;
		disconnectAckEvent.quittingPlayerID = playerID;
		disconnectAckEvent._targetUserID = this.getUserID(playerID);
		this.battleServer.QueueUnreliableMessage(disconnectAckEvent);
		this.inputTracker.SyncFrameWithAllInputs(this.client);
	}

	private void addRollback(int frame)
	{
		int num = this.rollbackToFrame;
		if (this.rollbackToFrame > 0)
		{
			this.rollbackToFrame = Math.Min(this.rollbackToFrame, frame);
		}
		else
		{
			this.rollbackToFrame = frame;
		}
		if (this.rollbackToFrame != num)
		{
			this.rollbackStatePool.RecordRollback(this.rollbackToFrame, this.currentFrame);
		}
	}

	private void delayedRemoteInputPost(InputEvent inputEvent)
	{
		RollbackLayer._delayedRemoteInputPost_c__AnonStorey0 _delayedRemoteInputPost_c__AnonStorey = new RollbackLayer._delayedRemoteInputPost_c__AnonStorey0();
		_delayedRemoteInputPost_c__AnonStorey.inputEvent = inputEvent;
		_delayedRemoteInputPost_c__AnonStorey._this = this;
		this.timer.SetOrReplaceTimeout((int)((float)this.settings.networkSettings.simulatedLatencyFrames * WTime.frameTime * 1000f), new Action(_delayedRemoteInputPost_c__AnonStorey.__m__0));
	}

	public void ForceRollback(int toFrame)
	{
		this.rollbackToFrame = toFrame;
	}

	void IRollbackLayer.Destroy()
	{
		if (this.battleServer != null)
		{
			this.battleServer.Unsubscribe<InputEvent>(new ServerEventHandler(this.onRemoteInput));
			this.battleServer.Unsubscribe<RequestMissingInputEvent>(new ServerEventHandler(this.onRequestMissingInput));
			this.battleServer.Unsubscribe<DisconnectEvent>(new ServerEventHandler(this.onDisconnect));
		}
		if (this.signalBus != null)
		{
			this.signalBus.GetSignal<QuitGameSignal>().RemoveListener(new Action(this.quitGameSignal));
		}
		if (this.quitGameHelper != null)
		{
			this.quitGameHelper.Destroy();
		}
		this.Timekeeper.Destroy();
		this.inputTracker.Destroy();
	}
}
