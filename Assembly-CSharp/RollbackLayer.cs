using System;
using System.Collections.Generic;
using IconsServer;
using P2P;
using RollbackDebug;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class RollbackLayer : IRollbackLayer, IRollbackStatus
{
	// Token: 0x17000D34 RID: 3380
	// (get) Token: 0x06003629 RID: 13865 RVA: 0x000F9BB5 File Offset: 0x000F7FB5
	// (set) Token: 0x0600362A RID: 13866 RVA: 0x000F9BBD File Offset: 0x000F7FBD
	[Inject]
	public IBattleServerAPI battleServer { get; set; }

	// Token: 0x17000D35 RID: 3381
	// (get) Token: 0x0600362B RID: 13867 RVA: 0x000F9BC6 File Offset: 0x000F7FC6
	// (set) Token: 0x0600362C RID: 13868 RVA: 0x000F9BCE File Offset: 0x000F7FCE
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000D36 RID: 3382
	// (get) Token: 0x0600362D RID: 13869 RVA: 0x000F9BD7 File Offset: 0x000F7FD7
	// (set) Token: 0x0600362E RID: 13870 RVA: 0x000F9BDF File Offset: 0x000F7FDF
	[Inject]
	public IPerformanceTracker performanceTracker { get; set; }

	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x0600362F RID: 13871 RVA: 0x000F9BE8 File Offset: 0x000F7FE8
	// (set) Token: 0x06003630 RID: 13872 RVA: 0x000F9BF0 File Offset: 0x000F7FF0
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06003631 RID: 13873 RVA: 0x000F9BF9 File Offset: 0x000F7FF9
	// (set) Token: 0x06003632 RID: 13874 RVA: 0x000F9C01 File Offset: 0x000F8001
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x06003633 RID: 13875 RVA: 0x000F9C0A File Offset: 0x000F800A
	// (set) Token: 0x06003634 RID: 13876 RVA: 0x000F9C12 File Offset: 0x000F8012
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000D3A RID: 3386
	// (get) Token: 0x06003635 RID: 13877 RVA: 0x000F9C1B File Offset: 0x000F801B
	// (set) Token: 0x06003636 RID: 13878 RVA: 0x000F9C23 File Offset: 0x000F8023
	[Inject]
	public IMatchDeepLogging deepLogging { get; set; }

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x06003637 RID: 13879 RVA: 0x000F9C2C File Offset: 0x000F802C
	// (set) Token: 0x06003638 RID: 13880 RVA: 0x000F9C34 File Offset: 0x000F8034
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x06003639 RID: 13881 RVA: 0x000F9C3D File Offset: 0x000F803D
	// (set) Token: 0x0600363A RID: 13882 RVA: 0x000F9C45 File Offset: 0x000F8045
	[Inject]
	public RollbackStatePoolContainer rollbackStatePool { get; set; }

	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x0600363B RID: 13883 RVA: 0x000F9C4E File Offset: 0x000F804E
	// (set) Token: 0x0600363C RID: 13884 RVA: 0x000F9C56 File Offset: 0x000F8056
	[Inject]
	public P2PHost p2pHost { private get; set; }

	// Token: 0x17000D3E RID: 3390
	// (get) Token: 0x0600363D RID: 13885 RVA: 0x000F9C5F File Offset: 0x000F805F
	// (set) Token: 0x0600363E RID: 13886 RVA: 0x000F9C67 File Offset: 0x000F8067
	[Inject]
	public IEvents events { private get; set; }

	// Token: 0x17000D2C RID: 3372
	// (get) Token: 0x0600363F RID: 13887 RVA: 0x000F9C70 File Offset: 0x000F8070
	int IRollbackStatus.FullyConfirmedFrame
	{
		get
		{
			return this.inputTracker.FrameWithAllInputs;
		}
	}

	// Token: 0x17000D2D RID: 3373
	// (get) Token: 0x06003640 RID: 13888 RVA: 0x000F9C7D File Offset: 0x000F807D
	int IRollbackStatus.LowestInputAckFrame
	{
		get
		{
			return this.inputTracker.LowestInputAckFrame;
		}
	}

	// Token: 0x17000D2E RID: 3374
	// (get) Token: 0x06003641 RID: 13889 RVA: 0x000F9C8A File Offset: 0x000F808A
	int IRollbackStatus.CalculatedLatencyMs
	{
		get
		{
			return this.inputTracker.CalculatedLatencyMs;
		}
	}

	// Token: 0x17000D2F RID: 3375
	// (get) Token: 0x06003642 RID: 13890 RVA: 0x000F9C97 File Offset: 0x000F8097
	long IRollbackStatus.MsSinceStart
	{
		get
		{
			return (long)this.Timekeeper.MsSinceStart;
		}
	}

	// Token: 0x17000D3F RID: 3391
	// (get) Token: 0x06003643 RID: 13891 RVA: 0x000F9CA5 File Offset: 0x000F80A5
	// (set) Token: 0x06003644 RID: 13892 RVA: 0x000F9CAD File Offset: 0x000F80AD
	public bool HasDesynced { get; private set; }

	// Token: 0x17000D40 RID: 3392
	// (get) Token: 0x06003645 RID: 13893 RVA: 0x000F9CB6 File Offset: 0x000F80B6
	// (set) Token: 0x06003646 RID: 13894 RVA: 0x000F9CBE File Offset: 0x000F80BE
	public ITimekeeper Timekeeper { get; private set; }

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x06003647 RID: 13895 RVA: 0x000F9CC7 File Offset: 0x000F80C7
	public bool RollbackEnabled
	{
		get
		{
			return this.battleServer.IsConnected || this.settings.networkSettings.simulateRollback || this.settings.networkSettings.simulatedLatencyFrames > 0;
		}
	}

	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06003648 RID: 13896 RVA: 0x000F9D04 File Offset: 0x000F8104
	bool IRollbackLayer.HasStarted
	{
		get
		{
			return this.isSimulating || this.replaySystem.IsReplaying;
		}
	}

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x06003649 RID: 13897 RVA: 0x000F9D1F File Offset: 0x000F811F
	public bool IsRollingBack
	{
		get
		{
			return this.rollbackToFrame != 0 && this.rollbackToFrame <= this.inputTracker.FrameWithAllInputs;
		}
	}

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x0600364A RID: 13898 RVA: 0x000F9D45 File Offset: 0x000F8145
	int IRollbackStatus.InputDelayFrames
	{
		get
		{
			return this.settings.inputDelayFrames;
		}
	}

	// Token: 0x17000D32 RID: 3378
	// (get) Token: 0x0600364B RID: 13899 RVA: 0x000F9D52 File Offset: 0x000F8152
	int IRollbackStatus.InputDelayPing
	{
		get
		{
			return this.settings.inputDelayPing;
		}
	}

	// Token: 0x17000D33 RID: 3379
	// (get) Token: 0x0600364C RID: 13900 RVA: 0x000F9D5F File Offset: 0x000F815F
	int IRollbackStatus.InitialTimestepDelta
	{
		get
		{
			return this.settings.initialTimestepDelta;
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x000F9D6C File Offset: 0x000F816C
	private bool debugSimulatingLatencyForPlayerID(int playerID)
	{
		return Application.isEditor && this.settings.networkSettings.simulatedLatencyFrames > 0;
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x0600364E RID: 13902 RVA: 0x000F9D8D File Offset: 0x000F818D
	private bool recordStates
	{
		get
		{
			return this.RollbackEnabled;
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x0600364F RID: 13903 RVA: 0x000F9D95 File Offset: 0x000F8195
	protected IRollbackClient client
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x000F9DA4 File Offset: 0x000F81A4
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

	// Token: 0x06003651 RID: 13905 RVA: 0x000F9FE0 File Offset: 0x000F83E0
	public void StartSession(int localPort, string remoteip, int remoteport)
	{
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x000F9FE4 File Offset: 0x000F83E4
	private int getProbableHost()
	{
		int num = int.MaxValue;
		foreach (KeyValuePair<int, RollbackPlayerData> keyValuePair in this.playerDataList)
		{
			RollbackPlayerData value = keyValuePair.Value;
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

	// Token: 0x06003653 RID: 13907 RVA: 0x000FA080 File Offset: 0x000F8480
	private bool isProbablyHost()
	{
		return this.getProbableHost() == this.localPlayer.playerID;
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x000FA098 File Offset: 0x000F8498
	private bool isSpectator(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isSpectator;
		}
		throw new Exception("Tried to check spectator status on invalid player " + playerID);
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x000FA0D4 File Offset: 0x000F84D4
	private bool isDisconnected(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame != -1;
		}
		throw new Exception("Tried to check disconnect status on invalid player " + playerID);
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x000FA118 File Offset: 0x000F8518
	private int getDisconnectFrame(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame;
		}
		throw new Exception("Tried to check disconnect status on invalid player " + playerID);
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x000FA154 File Offset: 0x000F8554
	private bool isLocal(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isLocal;
		}
		throw new Exception("Tried to check local status on invalid player " + playerID);
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x000FA190 File Offset: 0x000F8590
	private ulong getUserID(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.userID;
		}
		throw new Exception("Tried to check spectator status on invalid player " + playerID);
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x000FA1CC File Offset: 0x000F85CC
	private RollbackInputFrame getQueuedInputsForFrame(int frame)
	{
		if (frame < this.client.GameStartInputFrame)
		{
			return this.nullInputFrame;
		}
		return this.rollbackStatePool.GetQueuedInputsForFrame(frame);
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x000FA1F2 File Offset: 0x000F85F2
	public void ResetToFrame(int frame)
	{
		this.Timekeeper.ResetMilestone(frame);
		this.rollbackStatePool.ResetQueuedInputs(frame);
		this.inputTracker.ResetToFrame(frame);
		this.updateCurrentFrame(frame);
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x000FA21F File Offset: 0x000F861F
	private void updateCurrentFrame(int frame)
	{
		this.currentFrame = frame;
		this.rollbackStatePool.CachedCurrentFrame = this.currentFrame;
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x000FA23C File Offset: 0x000F863C
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
		Debug.Log(string.Concat(new object[]
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

	// Token: 0x0600365D RID: 13917 RVA: 0x000FA394 File Offset: 0x000F8794
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
			Debug.LogWarning("Ignoring input from disconnected player");
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

	// Token: 0x0600365E RID: 13918 RVA: 0x000FA5A4 File Offset: 0x000F89A4
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

	// Token: 0x0600365F RID: 13919 RVA: 0x000FA650 File Offset: 0x000F8A50
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

	// Token: 0x06003660 RID: 13920 RVA: 0x000FA698 File Offset: 0x000F8A98
	private bool addInput(RollbackInput input, int recordedFrame)
	{
		int num = this.playerIDtoIndex[input.playerID];
		RollbackInput[] inputsForFrame = this.getInputsForFrame(recordedFrame);
		inputsForFrame[num].values.CopyFrom(input.values);
		inputsForFrame[num].usedPreviousFrame = input.usedPreviousFrame;
		return true;
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x000FA6E4 File Offset: 0x000F8AE4
	public void FillSkippedLocalInput(int frame, RollbackInput input)
	{
		int frame2 = frame + this.settings.inputDelayFrames;
		int playerIndex = this.playerIDtoIndex[input.playerID];
		this.predictRemoteInput(playerIndex, frame2, input);
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x000FA71C File Offset: 0x000F8B1C
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

	// Token: 0x06003663 RID: 13923 RVA: 0x000FA8BC File Offset: 0x000F8CBC
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

	// Token: 0x06003664 RID: 13924 RVA: 0x000FA960 File Offset: 0x000F8D60
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

	// Token: 0x06003665 RID: 13925 RVA: 0x000FA9B0 File Offset: 0x000F8DB0
	private void autoRollback()
	{
		if (this.settings.simulate && this.settings.autoRollbackSimulationRate > 0 && !this.IsRollingBack && this.client.Frame % this.settings.autoRollbackSimulationRate == 0 && this.client.Frame > this.settings.autoRollbackSimulationAmount)
		{
			this.rollbackToFrame = this.client.Frame - this.settings.autoRollbackSimulationAmount;
		}
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x000FAA3D File Offset: 0x000F8E3D
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

	// Token: 0x06003667 RID: 13927 RVA: 0x000FAA7C File Offset: 0x000F8E7C
	private bool loadRollbackFrame()
	{
		if (this.currentFrame - this.rollbackToFrame >= RollbackStatePoolContainer.ROLLBACK_FRAMES)
		{
			Debug.LogError(string.Concat(new object[]
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
			Debug.LogError(string.Concat(new object[]
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

	// Token: 0x06003668 RID: 13928 RVA: 0x000FAC48 File Offset: 0x000F9048
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

	// Token: 0x06003669 RID: 13929 RVA: 0x000FAD18 File Offset: 0x000F9118
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

	// Token: 0x0600366A RID: 13930 RVA: 0x000FAE54 File Offset: 0x000F9254
	private void onFinishRollback(int currentIterations, int maxTicks)
	{
		if (currentIterations == maxTicks && maxTicks == RollbackStatePoolContainer.ROLLBACK_FRAMES)
		{
			Debug.Log("Infinite loop! The rollback layer cannot correctly catch up to the current state.");
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

	// Token: 0x0600366B RID: 13931 RVA: 0x000FAEE4 File Offset: 0x000F92E4
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

	// Token: 0x0600366C RID: 13932 RVA: 0x000FAF57 File Offset: 0x000F9357
	public void CheckMissingInputs()
	{
		this.checkInputRerouteFromOtherPlayers();
		this.checkForceDisconnect();
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x000FAF68 File Offset: 0x000F9368
	private bool canForceDisconnectThisPlayer(int playerID)
	{
		int num = int.MaxValue;
		foreach (KeyValuePair<int, RollbackPlayerData> keyValuePair in this.playerDataList)
		{
			RollbackPlayerData value = keyValuePair.Value;
			if (!value.isSpectator && value.disconnectFrame == -1 && value.playerID != playerID && value.playerID < num)
			{
				num = value.playerID;
			}
		}
		return num == this.localPlayer.playerID || num == int.MaxValue;
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x000FB020 File Offset: 0x000F9420
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
		foreach (KeyValuePair<int, RollbackPlayerData> keyValuePair in this.playerDataList)
		{
			if (!keyValuePair.Value.isSpectator && keyValuePair.Value.disconnectFrame == -1)
			{
				num3++;
			}
		}
		if (num3 > 2)
		{
			foreach (KeyValuePair<int, int> keyValuePair2 in this.inputTracker.PlayerFrameReceived)
			{
				int key = keyValuePair2.Key;
				int num4 = keyValuePair2.Value;
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
							Debug.LogError(string.Concat(new object[]
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

	// Token: 0x0600366F RID: 13935 RVA: 0x000FB214 File Offset: 0x000F9614
	private void forcePlayerQuit(int playerID, int frame)
	{
		if (!this.quitGameHelper.IsQuitting(playerID))
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, int> keyValuePair in this.inputTracker.PlayerInputAckStatus)
			{
				if (!this.isDisconnected(keyValuePair.Key) && !this.isLocal(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
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

	// Token: 0x06003670 RID: 13936 RVA: 0x000FB300 File Offset: 0x000F9700
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
			foreach (KeyValuePair<int, int> keyValuePair in this.inputTracker.PlayerFrameReceived)
			{
				int key = keyValuePair.Key;
				int num2 = keyValuePair.Value;
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
						foreach (KeyValuePair<int, int> keyValuePair2 in this.inputTracker.PlayerFrameReceived)
						{
							int key2 = keyValuePair2.Key;
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

	// Token: 0x06003671 RID: 13937 RVA: 0x000FB52C File Offset: 0x000F992C
	public void TickGameQuit()
	{
		this.quitGameHelper.Tick();
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x000FB53C File Offset: 0x000F993C
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
					foreach (KeyValuePair<int, int> keyValuePair in this.inputTracker.PlayerInputAckStatus)
					{
						int key = keyValuePair.Key;
						int value = keyValuePair.Value;
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

	// Token: 0x06003673 RID: 13939 RVA: 0x000FB6D0 File Offset: 0x000F9AD0
	public void ValidFrame()
	{
		this.notFrameTimeBeginMs = -1L;
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x000FB6DC File Offset: 0x000F9ADC
	public void TickNotFrame()
	{
		this.inputTracker.SendAllInputAcks();
		if (this.inputTracker.StoredSentInputsLastFrame.Count > 0)
		{
			foreach (InputEvent msg in this.inputTracker.StoredSentInputsLastFrame)
			{
				this.battleServer.QueueUnreliableMessage(msg);
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
				Debug.LogError("Quitting after 8 seconds frozen");
				this.didFailsafeExit = true;
				this.events.Broadcast(new DisconnectUpdate(NetErrorCode.ClientDisconnected));
			}
		}
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x000FB7C4 File Offset: 0x000F9BC4
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
				foreach (int key in this.playerIndextoID.Keys)
				{
					int playerID = this.playerIndextoID[key];
					if (!this.isLocal(playerID))
					{
						this.inputTracker.ConfirmInput(this.client, this.client.Frame, this.playerIndextoID[key], this.client.Frame, 0);
					}
				}
			}
		}
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x000FB8FC File Offset: 0x000F9CFC
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

	// Token: 0x06003677 RID: 13943 RVA: 0x000FB9AC File Offset: 0x000F9DAC
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
		foreach (int num2 in this.playerIndextoID.Keys)
		{
			int num3 = this.playerIndextoID[num2];
			if (this.isLocal(num3))
			{
				if (this.debugSimulatingLatencyForPlayerID(num3))
				{
					inputEvent = new InputEvent();
					inputEvent.startFrame = startFrame;
					inputEvent.numInputs = (byte)(endFrame - startFrame + 1);
				}
				inputEvent.playerID = (byte)num3;
				int num4 = 0;
				int localPlayerIndex = num2;
				for (int i = startFrame; i <= endFrame; i++)
				{
					RollbackInputFrame queuedInputsForFrame = this.getQueuedInputsForFrame(i);
					if (!queuedInputsForFrame.hasInputsSet)
					{
						Debug.LogError(string.Concat(new object[]
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
					this.addLocalInputToInputEvent(inputs, inputEvent, localPlayerIndex, num4);
					num4++;
				}
				if (this.debugSimulatingLatencyForPlayerID(num3))
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

	// Token: 0x06003678 RID: 13944 RVA: 0x000FBBD0 File Offset: 0x000F9FD0
	private void quitGameSignal()
	{
		this.beginLocalPlayerQuit = true;
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x000FBBD9 File Offset: 0x000F9FD9
	private void localBeginQuitGame(int frame)
	{
		this.beginLocalPlayerQuit = false;
		this.forcePlayerQuit(this.localPlayer.playerID, frame);
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x000FBBF4 File Offset: 0x000F9FF4
	private void onDisconnect(ServerEvent message)
	{
		if (this.client == null)
		{
			return;
		}
		DisconnectEvent disconnectEvent = message as DisconnectEvent;
		int playerID = disconnectEvent.playerID;
		int frame = disconnectEvent.frame;
		Debug.Log(string.Concat(new object[]
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

	// Token: 0x0600367B RID: 13947 RVA: 0x000FBD08 File Offset: 0x000FA108
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

	// Token: 0x0600367C RID: 13948 RVA: 0x000FBD6C File Offset: 0x000FA16C
	private void delayedRemoteInputPost(InputEvent inputEvent)
	{
		this.timer.SetOrReplaceTimeout((int)((float)this.settings.networkSettings.simulatedLatencyFrames * WTime.frameTime * 1000f), delegate
		{
			this.onRemoteInput(inputEvent);
		});
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x000FBDC2 File Offset: 0x000FA1C2
	public void ForceRollback(int toFrame)
	{
		this.rollbackToFrame = toFrame;
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x000FBDCC File Offset: 0x000FA1CC
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

	// Token: 0x040024F1 RID: 9457
	public const int MAX_FRAMES = 30600;

	// Token: 0x040024FD RID: 9469
	protected bool enablePrediction = true;

	// Token: 0x040024FE RID: 9470
	protected bool isSimulating;

	// Token: 0x040024FF RID: 9471
	protected RollbackSettings settings;

	// Token: 0x04002500 RID: 9472
	private Dictionary<int, InputEvent> inputEventBufferToPlayer = new Dictionary<int, InputEvent>();

	// Token: 0x04002501 RID: 9473
	private RequestMissingInputEvent requestMissingInputEventBuffer = new RequestMissingInputEvent();

	// Token: 0x04002502 RID: 9474
	private DisconnectAckEvent disconnectAckEventBuffer = new DisconnectAckEvent();

	// Token: 0x04002503 RID: 9475
	private RollbackInputFrame nullInputFrame = new RollbackInputFrame();

	// Token: 0x04002504 RID: 9476
	protected Dictionary<int, int> playerIDtoIndex = new Dictionary<int, int>();

	// Token: 0x04002505 RID: 9477
	protected Dictionary<int, int> playerIndextoID = new Dictionary<int, int>();

	// Token: 0x04002506 RID: 9478
	private Dictionary<int, RollbackPlayerData> playerDataList = new Dictionary<int, RollbackPlayerData>();

	// Token: 0x04002507 RID: 9479
	private RollbackPlayerData localPlayer;

	// Token: 0x04002508 RID: 9480
	protected int fullySynchronizedFrame;

	// Token: 0x0400250A RID: 9482
	protected List<string> errors = new List<string>();

	// Token: 0x0400250B RID: 9483
	protected int playerCount = 2;

	// Token: 0x0400250C RID: 9484
	protected int rollbackToFrame;

	// Token: 0x0400250D RID: 9485
	protected int currentFrame;

	// Token: 0x0400250E RID: 9486
	private long notFrameTimeBeginMs = -1L;

	// Token: 0x0400250F RID: 9487
	private bool didFailsafeExit;

	// Token: 0x04002510 RID: 9488
	protected bool beginLocalPlayerQuit;

	// Token: 0x04002511 RID: 9489
	protected IReplaySystem replaySystem;

	// Token: 0x04002512 RID: 9490
	protected IRollbackLayerDebugger rollbackDebugger;

	// Token: 0x04002514 RID: 9492
	private IRollbackInputTracker inputTracker;

	// Token: 0x04002515 RID: 9493
	private IRollbackQuitGame quitGameHelper;

	// Token: 0x04002516 RID: 9494
	private GenericResetObjectPool<RollbackInput> rollbackInputPool;
}
