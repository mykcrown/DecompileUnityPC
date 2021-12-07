using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;
using UnityEngine;

// Token: 0x02000875 RID: 2165
public class RollbackInputTracker : IRollbackInputTracker, IRollbackInputStatus
{
	// Token: 0x060035FC RID: 13820 RVA: 0x000FEB80 File Offset: 0x000FCF80
	public RollbackInputTracker(int playerCount, IBattleServerAPI battleServerAPI, RollbackSettings settings, ITimekeeper timeKeeper)
	{
		this.timeKeeper = timeKeeper;
		this.battleServerAPI = battleServerAPI;
		for (int i = 0; i < this.inputEventRingBuffer.Length; i++)
		{
			this.inputEventRingBuffer[i] = new InputEvent();
		}
		this.PlayerInputAckStatus.Clear();
		this.PlayerFrameReceived.Clear();
		this.StoredRemoteInputs.Clear();
		this.playerDataList.Clear();
		this.frameSendTime.Clear();
		foreach (RollbackPlayerData rollbackPlayerData in settings.playerDataList)
		{
			if (rollbackPlayerData.isLocal)
			{
				this.localPlayer = rollbackPlayerData;
			}
		}
		foreach (RollbackPlayerData rollbackPlayerData2 in settings.playerDataList)
		{
			int playerID = rollbackPlayerData2.playerID;
			this.playerDataList[playerID] = rollbackPlayerData2;
			if (!rollbackPlayerData2.isSpectator)
			{
				this.PlayerFrameReceived[playerID] = -1;
			}
			if (!rollbackPlayerData2.isLocal)
			{
				if (!this.localPlayer.isSpectator)
				{
					this.PlayerInputAckStatus[playerID] = settings.inputDelayFrames;
				}
				this.StoredRemoteInputs[playerID] = new InputMsg.InputArrayData[30600];
				this.cachedInputAckEvents[playerID] = new InputAckEvent();
				this.cachedInputAckEvents[playerID].senderPlayerID = (byte)this.localPlayer.playerID;
				this.cachedInputAckEvents[playerID]._targetUserID = rollbackPlayerData2.userID;
			}
		}
		if (this.localPlayer.isSpectator)
		{
			this.latencyMs = 0;
		}
		battleServerAPI.Listen<InputAckEvent>(new ServerEventHandler(this.onInputAck));
	}

	// Token: 0x17000D1E RID: 3358
	// (get) Token: 0x060035FD RID: 13821 RVA: 0x000FEDEC File Offset: 0x000FD1EC
	int IRollbackInputStatus.CalculatedLatencyMs
	{
		get
		{
			return this.latencyMs;
		}
	}

	// Token: 0x17000D1F RID: 3359
	// (get) Token: 0x060035FE RID: 13822 RVA: 0x000FEDF4 File Offset: 0x000FD1F4
	public Dictionary<int, int> PlayerInputAckStatus { get; } = new Dictionary<int, int>();

	// Token: 0x17000D20 RID: 3360
	// (get) Token: 0x060035FF RID: 13823 RVA: 0x000FEDFC File Offset: 0x000FD1FC
	public Dictionary<int, int> PlayerFrameReceived { get; } = new Dictionary<int, int>();

	// Token: 0x17000D21 RID: 3361
	// (get) Token: 0x06003600 RID: 13824 RVA: 0x000FEE04 File Offset: 0x000FD204
	public Dictionary<int, InputMsg.InputArrayData[]> StoredRemoteInputs { get; } = new Dictionary<int, InputMsg.InputArrayData[]>();

	// Token: 0x17000D22 RID: 3362
	// (get) Token: 0x06003601 RID: 13825 RVA: 0x000FEE0C File Offset: 0x000FD20C
	public List<InputEvent> StoredSentInputsLastFrame { get; } = new List<InputEvent>(128);

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x06003602 RID: 13826 RVA: 0x000FEE14 File Offset: 0x000FD214
	// (set) Token: 0x06003603 RID: 13827 RVA: 0x000FEE1C File Offset: 0x000FD21C
	public int FrameWithAllInputs { get; private set; }

	// Token: 0x06003604 RID: 13828 RVA: 0x000FEE25 File Offset: 0x000FD225
	void IRollbackInputTracker.Destroy()
	{
		this.battleServerAPI.Unsubscribe<InputAckEvent>(new ServerEventHandler(this.onInputAck));
	}

	// Token: 0x06003605 RID: 13829 RVA: 0x000FEE40 File Offset: 0x000FD240
	private bool isLocal(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isLocal;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

	// Token: 0x06003606 RID: 13830 RVA: 0x000FEE7C File Offset: 0x000FD27C
	private bool isSpectator(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isSpectator;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

	// Token: 0x06003607 RID: 13831 RVA: 0x000FEEB8 File Offset: 0x000FD2B8
	private int getDisconnectFrame(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

	// Token: 0x06003608 RID: 13832 RVA: 0x000FEEF4 File Offset: 0x000FD2F4
	private void onInputAck(ServerEvent message)
	{
		InputAckEvent inputAckEvent = message as InputAckEvent;
		int num;
		if (this.PlayerInputAckStatus.TryGetValue((int)inputAckEvent.senderPlayerID, out num) && inputAckEvent.latestAckedFrame + 1 > num)
		{
			this.PlayerInputAckStatus[(int)inputAckEvent.senderPlayerID] = inputAckEvent.latestAckedFrame + 1;
		}
		if (!this.frameSendTime.ContainsKey(inputAckEvent.latestAckedFrame))
		{
			Debug.LogError("Received ack for frame " + inputAckEvent.latestAckedFrame + " but we never sent it.");
		}
		this.latencyMs = (int)(WTime.precisionTimeSinceStartup - this.frameSendTime[inputAckEvent.latestAckedFrame]);
	}

	// Token: 0x06003609 RID: 13833 RVA: 0x000FEF9A File Offset: 0x000FD39A
	public void ResetLatestSentInput()
	{
		this.StoredSentInputsLastFrame.Clear();
	}

	// Token: 0x0600360A RID: 13834 RVA: 0x000FEFA8 File Offset: 0x000FD3A8
	public void AddLatestSendInput(InputEvent evt)
	{
		this.inputEventRingBufferIndex = (this.inputEventRingBufferIndex + 1) % this.inputEventRingBuffer.Length;
		InputEvent inputEvent = this.inputEventRingBuffer[this.inputEventRingBufferIndex];
		inputEvent.LoadFrom(evt);
		this.StoredSentInputsLastFrame.Add(inputEvent);
	}

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x0600360B RID: 13835 RVA: 0x000FEFF0 File Offset: 0x000FD3F0
	public int LowestInputAckFrame
	{
		get
		{
			int num = int.MaxValue;
			foreach (KeyValuePair<int, int> keyValuePair in this.PlayerInputAckStatus)
			{
				int num2 = this.PlayerInputAckStatus[keyValuePair.Key];
				if (num2 < num)
				{
					num = num2;
				}
			}
			if (num == 2147483647)
			{
				return -1;
			}
			return num;
		}
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x000FF078 File Offset: 0x000FD478
	bool IRollbackInputTracker.HasPlayerInputForFrame(int frame, int playerID)
	{
		return frame <= this.PlayerFrameReceived[playerID];
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x000FF08C File Offset: 0x000FD48C
	int IRollbackInputTracker.LatestFrameFrom(int playerID)
	{
		return this.PlayerFrameReceived[playerID];
	}

	// Token: 0x0600360E RID: 13838 RVA: 0x000FF09C File Offset: 0x000FD49C
	public void ResetToFrame(int frame)
	{
		for (int i = 0; i < this.PlayerFrameReceived.Count; i++)
		{
			this.PlayerFrameReceived[i] = frame;
		}
		foreach (KeyValuePair<int, int> keyValuePair in this.PlayerInputAckStatus)
		{
			this.PlayerInputAckStatus[keyValuePair.Key] = frame;
		}
		this.FrameWithAllInputs = frame;
	}

	// Token: 0x0600360F RID: 13839 RVA: 0x000FF134 File Offset: 0x000FD534
	void IRollbackInputTracker.ConfirmInput(IRollbackClient client, int frame, int playerID, int d_startFrame, int d_index)
	{
		if (frame <= this.PlayerFrameReceived[playerID])
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Duplicate inputs (confirmed for frame ",
				frame,
				" for player ",
				playerID,
				")"
			}));
			return;
		}
		if (this.PlayerFrameReceived[playerID] != -1 && frame != this.PlayerFrameReceived[playerID] + 1)
		{
			throw new Exception(string.Concat(new object[]
			{
				"Input",
				frame,
				".",
				playerID,
				".",
				this.PlayerFrameReceived[playerID],
				".",
				d_startFrame,
				".",
				d_index
			}));
		}
		this.PlayerFrameReceived[playerID] = frame;
		if (frame >= client.GameStartInputFrame)
		{
			this.SyncFrameWithAllInputs(client);
		}
		else if (frame > this.FrameWithAllInputs)
		{
			this.FrameWithAllInputs = frame;
			this.timeKeeper.OnAllInputsFrameUpdated(client, this.FrameWithAllInputs);
		}
	}

	// Token: 0x06003610 RID: 13840 RVA: 0x000FF278 File Offset: 0x000FD678
	public void SyncFrameWithAllInputs(IRollbackClient client)
	{
		int num = int.MaxValue;
		foreach (KeyValuePair<int, int> keyValuePair in this.PlayerFrameReceived)
		{
			if (!this.isSpectator(keyValuePair.Key) && keyValuePair.Value < num)
			{
				int disconnectFrame = this.getDisconnectFrame(keyValuePair.Key);
				if (disconnectFrame != -1)
				{
					if (keyValuePair.Value < disconnectFrame - 1)
					{
						num = keyValuePair.Value;
					}
				}
				else
				{
					num = keyValuePair.Value;
				}
			}
		}
		if (num != 2147483647 && num > this.FrameWithAllInputs)
		{
			this.FrameWithAllInputs = num;
			this.timeKeeper.OnAllInputsFrameUpdated(client, this.FrameWithAllInputs);
		}
	}

	// Token: 0x06003611 RID: 13841 RVA: 0x000FF35C File Offset: 0x000FD75C
	void IRollbackInputTracker.RecordInputAck(int playerID, InputEvent inputEvent)
	{
		int num = inputEvent.startFrame + (int)inputEvent.numInputs - 1;
		InputAckEvent inputAckEvent;
		if (this.cachedInputAckEvents.TryGetValue(playerID, out inputAckEvent) && num > inputAckEvent.latestAckedFrame)
		{
			inputAckEvent.latestAckedFrame = num;
		}
	}

	// Token: 0x06003612 RID: 13842 RVA: 0x000FF3A0 File Offset: 0x000FD7A0
	void IRollbackInputTracker.SendAllInputAcks()
	{
		foreach (InputAckEvent inputAckEvent in this.cachedInputAckEvents.Values)
		{
			if (inputAckEvent.latestAckedFrame > 0)
			{
				this.battleServerAPI.QueueUnreliableMessage(inputAckEvent);
			}
		}
	}

	// Token: 0x06003613 RID: 13843 RVA: 0x000FF414 File Offset: 0x000FD814
	void IRollbackInputTracker.RecordFrameSent(InputEvent inputEvent)
	{
		for (int i = inputEvent.startFrame; i <= inputEvent.startFrame + (int)inputEvent.numInputs - 1; i++)
		{
			if (!this.frameSendTime.ContainsKey(i))
			{
				this.frameSendTime[i] = WTime.precisionTimeSinceStartup;
			}
		}
	}

	// Token: 0x06003614 RID: 13844 RVA: 0x000FF468 File Offset: 0x000FD868
	void IRollbackInputTracker.StoreReceivedInputData(int frame, int playerID, InputMsg.InputArrayData inputData)
	{
		this.StoredRemoteInputs[playerID][frame] = inputData;
	}

	// Token: 0x040024E3 RID: 9443
	private IBattleServerAPI battleServerAPI;

	// Token: 0x040024E4 RID: 9444
	private ITimekeeper timeKeeper;

	// Token: 0x040024E5 RID: 9445
	private int latencyMs;

	// Token: 0x040024EA RID: 9450
	private InputEvent[] inputEventRingBuffer = new InputEvent[1024];

	// Token: 0x040024EB RID: 9451
	private int inputEventRingBufferIndex;

	// Token: 0x040024EC RID: 9452
	private Dictionary<int, InputAckEvent> cachedInputAckEvents = new Dictionary<int, InputAckEvent>();

	// Token: 0x040024EE RID: 9454
	private Dictionary<int, RollbackPlayerData> playerDataList = new Dictionary<int, RollbackPlayerData>();

	// Token: 0x040024EF RID: 9455
	private RollbackPlayerData localPlayer;

	// Token: 0x040024F0 RID: 9456
	private Dictionary<int, double> frameSendTime = new Dictionary<int, double>(30600);
}
