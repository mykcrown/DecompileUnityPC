// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RollbackInputTracker : IRollbackInputTracker, IRollbackInputStatus
{
	private IBattleServerAPI battleServerAPI;

	private ITimekeeper timeKeeper;

	private int latencyMs;

	private InputEvent[] inputEventRingBuffer;

	private int inputEventRingBufferIndex;

	private Dictionary<int, InputAckEvent> cachedInputAckEvents;

	private Dictionary<int, RollbackPlayerData> playerDataList;

	private RollbackPlayerData localPlayer;

	private Dictionary<int, double> frameSendTime;

	int IRollbackInputStatus.CalculatedLatencyMs
	{
		get
		{
			return this.latencyMs;
		}
	}

	public Dictionary<int, int> PlayerInputAckStatus
	{
		get
		{
			return this._PlayerInputAckStatus_k__BackingField;
		}
	}

	public Dictionary<int, int> PlayerFrameReceived
	{
		get
		{
			return this._PlayerFrameReceived_k__BackingField;
		}
	}

	public Dictionary<int, InputMsg.InputArrayData[]> StoredRemoteInputs
	{
		get
		{
			return this._StoredRemoteInputs_k__BackingField;
		}
	}

	public List<InputEvent> StoredSentInputsLastFrame
	{
		get
		{
			return this._StoredSentInputsLastFrame_k__BackingField;
		}
	}

	public int FrameWithAllInputs
	{
		get;
		private set;
	}

	public int LowestInputAckFrame
	{
		get
		{
			int num = 2147483647;
			foreach (KeyValuePair<int, int> current in this.PlayerInputAckStatus)
			{
				int num2 = this.PlayerInputAckStatus[current.Key];
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

	public RollbackInputTracker(int playerCount, IBattleServerAPI battleServerAPI, RollbackSettings settings, ITimekeeper timeKeeper)
	{
		this._PlayerInputAckStatus_k__BackingField = new Dictionary<int, int>();
		this._PlayerFrameReceived_k__BackingField = new Dictionary<int, int>();
		this._StoredRemoteInputs_k__BackingField = new Dictionary<int, InputMsg.InputArrayData[]>();
		this._StoredSentInputsLastFrame_k__BackingField = new List<InputEvent>(128);
		this.inputEventRingBuffer = new InputEvent[1024];
		this.cachedInputAckEvents = new Dictionary<int, InputAckEvent>();
		this.playerDataList = new Dictionary<int, RollbackPlayerData>();
		this.frameSendTime = new Dictionary<int, double>(30600);
		
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
		foreach (RollbackPlayerData current in settings.playerDataList)
		{
			if (current.isLocal)
			{
				this.localPlayer = current;
			}
		}
		foreach (RollbackPlayerData current2 in settings.playerDataList)
		{
			int playerID = current2.playerID;
			this.playerDataList[playerID] = current2;
			if (!current2.isSpectator)
			{
				this.PlayerFrameReceived[playerID] = -1;
			}
			if (!current2.isLocal)
			{
				if (!this.localPlayer.isSpectator)
				{
					this.PlayerInputAckStatus[playerID] = settings.inputDelayFrames;
				}
				this.StoredRemoteInputs[playerID] = new InputMsg.InputArrayData[30600];
				this.cachedInputAckEvents[playerID] = new InputAckEvent();
				this.cachedInputAckEvents[playerID].senderPlayerID = (byte)this.localPlayer.playerID;
				this.cachedInputAckEvents[playerID]._targetUserID = current2.userID;
			}
		}
		if (this.localPlayer.isSpectator)
		{
			this.latencyMs = 0;
		}
		battleServerAPI.Listen<InputAckEvent>(new ServerEventHandler(this.onInputAck));
	}

	void IRollbackInputTracker.Destroy()
	{
		this.battleServerAPI.Unsubscribe<InputAckEvent>(new ServerEventHandler(this.onInputAck));
	}

	private bool isLocal(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isLocal;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

	private bool isSpectator(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.isSpectator;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

	private int getDisconnectFrame(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.disconnectFrame;
		}
		throw new UnityException("Tried to check local status on invalid player " + playerID);
	}

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
			UnityEngine.Debug.LogError("Received ack for frame " + inputAckEvent.latestAckedFrame + " but we never sent it.");
		}
		this.latencyMs = (int)(WTime.precisionTimeSinceStartup - this.frameSendTime[inputAckEvent.latestAckedFrame]);
	}

	public void ResetLatestSentInput()
	{
		this.StoredSentInputsLastFrame.Clear();
	}

	public void AddLatestSendInput(InputEvent evt)
	{
		this.inputEventRingBufferIndex = (this.inputEventRingBufferIndex + 1) % this.inputEventRingBuffer.Length;
		InputEvent inputEvent = this.inputEventRingBuffer[this.inputEventRingBufferIndex];
		inputEvent.LoadFrom(evt);
		this.StoredSentInputsLastFrame.Add(inputEvent);
	}

	bool IRollbackInputTracker.HasPlayerInputForFrame(int frame, int playerID)
	{
		return frame <= this.PlayerFrameReceived[playerID];
	}

	int IRollbackInputTracker.LatestFrameFrom(int playerID)
	{
		return this.PlayerFrameReceived[playerID];
	}

	public void ResetToFrame(int frame)
	{
		for (int i = 0; i < this.PlayerFrameReceived.Count; i++)
		{
			this.PlayerFrameReceived[i] = frame;
		}
		foreach (KeyValuePair<int, int> current in this.PlayerInputAckStatus)
		{
			this.PlayerInputAckStatus[current.Key] = frame;
		}
		this.FrameWithAllInputs = frame;
	}

	void IRollbackInputTracker.ConfirmInput(IRollbackClient client, int frame, int playerID, int d_startFrame, int d_index)
	{
		if (frame <= this.PlayerFrameReceived[playerID])
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	public void SyncFrameWithAllInputs(IRollbackClient client)
	{
		int num = 2147483647;
		foreach (KeyValuePair<int, int> current in this.PlayerFrameReceived)
		{
			if (!this.isSpectator(current.Key) && current.Value < num)
			{
				int disconnectFrame = this.getDisconnectFrame(current.Key);
				if (disconnectFrame != -1)
				{
					if (current.Value < disconnectFrame - 1)
					{
						num = current.Value;
					}
				}
				else
				{
					num = current.Value;
				}
			}
		}
		if (num != 2147483647 && num > this.FrameWithAllInputs)
		{
			this.FrameWithAllInputs = num;
			this.timeKeeper.OnAllInputsFrameUpdated(client, this.FrameWithAllInputs);
		}
	}

	void IRollbackInputTracker.RecordInputAck(int playerID, InputEvent inputEvent)
	{
		int num = inputEvent.startFrame + (int)inputEvent.numInputs - 1;
		InputAckEvent inputAckEvent;
		if (this.cachedInputAckEvents.TryGetValue(playerID, out inputAckEvent) && num > inputAckEvent.latestAckedFrame)
		{
			inputAckEvent.latestAckedFrame = num;
		}
	}

	void IRollbackInputTracker.SendAllInputAcks()
	{
		foreach (InputAckEvent current in this.cachedInputAckEvents.Values)
		{
			if (current.latestAckedFrame > 0)
			{
				this.battleServerAPI.QueueUnreliableMessage(current);
			}
		}
	}

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

	void IRollbackInputTracker.StoreReceivedInputData(int frame, int playerID, InputMsg.InputArrayData inputData)
	{
		this.StoredRemoteInputs[playerID][frame] = inputData;
	}
}
