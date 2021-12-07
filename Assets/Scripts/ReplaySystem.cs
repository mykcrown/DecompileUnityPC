// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using RollbackDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReplaySystem : IReplaySystem, IDebugReplaySystem
{
	private Replay replay;

	private Serialization.SerializeType serializeType = Serialization.SerializeType.XML;

	private ReplayMode replayMode = ReplayMode.Record;

	public bool recordStates;

	private bool flattenRollbackInput;

	private bool enableRuntimeValidation;

	private bool autoValidateOverwrites;

	private bool isDirty = true;

	private int maxFrameLength;

	private static readonly string REPLAY_DIR = "Replays";

	bool IReplaySystem.EnableRuntimeValidation
	{
		get
		{
			return this.enableRuntimeValidation;
		}
	}

	bool IReplaySystem.ContainsRemoteInputs
	{
		get
		{
			return this.IsRemoteMode && this.replay.replayType == ReplayType.Rollback && this.replay.remoteInputsByFrame.Count > 0;
		}
	}

	[Inject]
	public IDevConsole devConsole
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

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IRollbackLayerDebugger rollbackDebugger
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
	public IPlayerTauntsFinder tauntsFinder
	{
		get;
		set;
	}

	private bool exceedsMaxLength
	{
		get
		{
			return this.replay.recordedInputs.Count >= this.maxFrameLength;
		}
	}

	public ReplayMode Mode
	{
		get
		{
			return this.replayMode;
		}
		set
		{
			this.replayMode = value;
		}
	}

	public bool IsRemoteMode
	{
		get
		{
			return this.replay.replayType == ReplayType.Rollback && !this.flattenRollbackInput;
		}
	}

	public bool RecordStates
	{
		get
		{
			return this.recordStates;
		}
		set
		{
			this.recordStates = value;
			MemberwiseEqualityObject.MemberwiseEqualityEnabled = true;
		}
	}

	public bool RecordHashes
	{
		get
		{
			return true;
		}
	}

	public Serialization.SerializeType SerializeType
	{
		get
		{
			return this.serializeType;
		}
		set
		{
			this.serializeType = value;
		}
	}

	public bool IsRecording
	{
		get
		{
			return this.replayMode == ReplayMode.Record;
		}
	}

	public bool IsReplaying
	{
		get
		{
			return this.replayMode == ReplayMode.Replay;
		}
	}

	public bool IsDirty
	{
		get
		{
			return this.isDirty;
		}
		set
		{
			this.isDirty = value;
		}
	}

	public Replay Replay
	{
		get
		{
			return this.replay;
		}
	}

	public void Init(ReplaySettings settings)
	{
		this.LoadSettings(settings);
		if (!Directory.Exists(ReplaySystem.REPLAY_DIR))
		{
			Directory.CreateDirectory(ReplaySystem.REPLAY_DIR);
		}
		this.rollbackDebugger.LoadReplaySystem(this);
	}

	public void LoadSettings(ReplaySettings settings)
	{
		if (this.replay == null)
		{
			this.replay = new Replay();
		}
		this.RecordStates = settings.recordStates;
		this.replay.replayType = settings.replayType;
		this.SerializeType = settings.serializeType;
		this.flattenRollbackInput = settings.flattenRollbackInput;
		this.enableRuntimeValidation = settings.enableRuntimeReplayValidation;
		this.autoValidateOverwrites = settings.autoValidateOverwrites;
		this.maxFrameLength = settings.maxReplayLength;
	}

	private string getReplayPath(string replayName)
	{
		return string.Format("{0}{1}{2}.log", ReplaySystem.REPLAY_DIR, Path.DirectorySeparatorChar, replayName);
	}

	private void writeReplay(string replayName)
	{
		if (this.exceedsMaxLength)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Replay file is too large (",
				this.replay.recordedInputs.Count,
				" of ",
				this.maxFrameLength,
				") and can't be written"
			}));
		}
		else
		{
			string replayPath = this.getReplayPath(replayName);
			Serialization.Write<Replay>(replayPath, this.replay, this.serializeType, false, true);
			if (replayName != "replay")
			{
				string replayPath2 = this.getReplayPath("replay");
				File.Copy(replayPath, replayPath2, true);
			}
		}
	}

	private Replay readReplay(string replayName)
	{
		string replayPath = this.getReplayPath(replayName);
		return Serialization.Read<Replay>(replayPath, this.serializeType, true);
	}

	bool IReplaySystem.HasFile(string replayName)
	{
		string replayPath = this.getReplayPath(replayName);
		return File.Exists(replayPath);
	}

	bool IReplaySystem.LoadFromFile(string replayName)
	{
		this.replay = this.readReplay(replayName);
		if (this.replay == null)
		{
			return false;
		}
		if (this.exceedsMaxLength)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Replay file is too large (",
				this.replay.recordedInputs.Count,
				" of ",
				this.maxFrameLength,
				") and can't be read"
			}));
			return false;
		}
		if (this.flattenRollbackInput && this.replay.replayType == ReplayType.Rollback)
		{
			foreach (int current in this.replay.remoteInputsByFrame.Keys)
			{
				foreach (RollbackInput current2 in this.replay.remoteInputsByFrame[current])
				{
					this.replay.recordedInputs[current2.frame][current2.playerID - 1] = current2;
				}
			}
			this.replay.remoteInputsByFrame.Clear();
		}
		if (this.replay != null)
		{
			this.isDirty = false;
		}
		return this.replay != null;
	}

	void IReplaySystem.OnGameStarted(ref GameLoadPayload payload)
	{
		if (this.replay == null)
		{
			this.replay = new Replay();
		}
		ReplayMode replayMode = this.replayMode;
		if (replayMode != ReplayMode.Record)
		{
			if (replayMode != ReplayMode.Replay)
			{
				if (replayMode == ReplayMode.Disabled)
				{
					payload.isReplay = false;
				}
			}
			else
			{
				this.isDirty = true;
				payload.isReplay = true;
				this.setupReplayPlayerMappings();
			}
		}
		else
		{
			this.replay.Clear();
			IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
					if (playerSelectionInfo.isLocal)
					{
						UserTaunts forPlayer = this.tauntsFinder.GetForPlayer(playerSelectionInfo.playerNum);
						playerSelectionInfo.tauntData = forPlayer;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.replay.startPayload = payload;
			this.replay.startPayload.isReplay = true;
			payload.isReplay = false;
		}
	}

	void IReplaySystem.OnGameFinished(int frame, VictoryScreenPayload payload, string replayName)
	{
		if (this.IsRecording)
		{
			if (payload.wasForfeited || payload.wasExited)
			{
				this.replay.forfeitFrame = frame;
			}
			this.replay.timestamp = DateTime.Now;
			this.writeReplay(replayName);
			this.isDirty = true;
		}
		else if (this.IsReplaying)
		{
			this.replayMode = ReplayMode.Record;
			this.userInputManager.ResetPlayerMapping();
		}
	}

	void IReplaySystem.Tick(int frame, IEndableClient client)
	{
		if (this.IsReplaying && this.replay.forfeitFrame > 0 && frame == this.replay.forfeitFrame)
		{
			client.EndGame();
		}
	}

	bool IReplaySystem.TestReplayEquality(string filePath1, string filePath2, ref string error)
	{
		MemberwiseEqualityObject.MemberwiseEqualityEnabled = true;
		Replay replay = Serialization.Read<Replay>(filePath1, this.serializeType, true);
		Replay replay2 = Serialization.Read<Replay>(filePath2, this.serializeType, true);
		if (replay == null || replay2 == null)
		{
			error = string.Concat(new object[]
			{
				"Null replays: ",
				replay == null,
				", ",
				replay2 == null
			});
			return false;
		}
		bool result = true;
		if (replay.recordedInputs.Count != replay2.recordedInputs.Count)
		{
			result = false;
			string text = error;
			error = string.Concat(new object[]
			{
				text,
				"Different input count: ",
				replay.recordedInputs.Count,
				" != ",
				replay2.recordedInputs.Count,
				"\n"
			});
		}
		if (replay2.savedHashes.Count != replay2.savedHashes.Count)
		{
			result = false;
			string text = error;
			error = string.Concat(new object[]
			{
				text,
				"Different hash count: ",
				replay.savedHashes.Count,
				" != ",
				replay2.savedHashes.Count,
				"\n"
			});
		}
		if (replay2.savedStates.Count != replay2.savedStates.Count)
		{
			result = false;
			string text = error;
			error = string.Concat(new object[]
			{
				text,
				"Different state count: ",
				replay.savedStates.Count,
				" != ",
				replay2.savedStates.Count,
				"\n"
			});
		}
		int num = Math.Min(replay.recordedInputs.Count, replay2.recordedInputs.Count);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < replay.recordedInputs[i].Length; j++)
			{
				if (!replay.recordedInputs[i][j].Equals(replay2.recordedInputs[i][j]))
				{
					string text = error;
					error = string.Concat(new object[]
					{
						text,
						"Frame ",
						i,
						" playerIndex ",
						j,
						": Input mismatch ",
						replay.recordedInputs[i][j].ToString(),
						" != ",
						replay2.recordedInputs[i][j].ToString(),
						"\n"
					});
					result = false;
					break;
				}
			}
		}
		int num2 = Math.Min(replay.savedHashes.Count, replay2.savedHashes.Count);
		for (int k = 0; k < num2; k++)
		{
			if (replay.savedHashes[k] != replay2.savedHashes[k])
			{
				string text = error;
				error = string.Concat(new object[]
				{
					text,
					"Frame ",
					k,
					": Hash mismatch: ",
					replay.savedHashes[k],
					" != ",
					replay2.savedHashes[k],
					"\n"
				});
				result = false;
				break;
			}
		}
		if (replay.savedStates != null && replay.savedStates.Count > 0)
		{
			for (int l = 0; l < replay.savedStates[0].Count; l++)
			{
				MemberwiseEqualityObject.RecursivelyRegisterMemberwiseTypes(replay.savedStates[0].GetState(l).GetType());
			}
		}
		return result;
	}

	List<RollbackInput> IReplaySystem.GetRemoteInputForFrame(int frame)
	{
		if (this.replayMode != ReplayMode.Replay)
		{
			return null;
		}
		ReplayType replayType = this.replay.replayType;
		if (replayType == ReplayType.Normal || replayType != ReplayType.Rollback)
		{
			return null;
		}
		if (!this.replay.remoteInputsByFrame.ContainsKey(frame))
		{
			return null;
		}
		return this.replay.remoteInputsByFrame[frame];
	}

	void IReplaySystem.RecordRemoteRollbackInput(int frame, RollbackInput remoteInput)
	{
		if (this.replayMode != ReplayMode.Record)
		{
			return;
		}
		ReplayType replayType = this.replay.replayType;
		if (replayType != ReplayType.Normal)
		{
			if (replayType == ReplayType.Rollback)
			{
				if (!this.replay.remoteInputsByFrame.ContainsKey(frame))
				{
					this.replay.remoteInputsByFrame[frame] = new List<RollbackInput>();
				}
				RollbackInput rollbackInput = new RollbackInput();
				rollbackInput.frame = remoteInput.frame;
				rollbackInput.playerID = remoteInput.playerID;
				rollbackInput.values.CopyFrom(remoteInput.values);
				this.replay.remoteInputsByFrame[frame].Add(rollbackInput);
			}
		}
	}

	bool IReplaySystem.SynchronizeInput(int frame, ref RollbackInput[] frameInputs)
	{
		ReplayMode replayMode = this.replayMode;
		if (replayMode == ReplayMode.Disabled)
		{
			return false;
		}
		if (replayMode == ReplayMode.Record)
		{
			bool flag = true;
			if (this.replay.recordedInputs.Count == frame)
			{
				this.replay.recordedInputs.Add(new RollbackInput[frameInputs.Length]);
			}
			else
			{
				flag = (this.replay.replayType != ReplayType.Rollback);
				if (flag)
				{
					this.replay.recordedInputs[frame] = new RollbackInput[frameInputs.Length];
				}
			}
			if (flag)
			{
				for (int i = 0; i < frameInputs.Length; i++)
				{
					if (this.replay.recordedInputs[frame][i] == null)
					{
						this.replay.recordedInputs[frame][i] = new RollbackInput(frameInputs[i]);
					}
					else
					{
						this.replay.recordedInputs[frame][i].CopyFrom(frameInputs[i]);
					}
				}
			}
			return false;
		}
		if (replayMode != ReplayMode.Replay)
		{
			return false;
		}
		if (this.replay.recordedInputs.Count <= frame)
		{
			UnityEngine.Debug.LogError("Replay does not have a record of inputs for frame " + frame);
		}
		frameInputs = this.replay.recordedInputs[frame];
		return true;
	}

	void IReplaySystem.RecordState(int frame, RollbackStateContainer state)
	{
		int num = frame - 1;
		if (this.recordStates)
		{
			if (this.replay.savedStates.Count == num)
			{
				this.replay.savedStates.Add(state);
			}
			else
			{
				if (this.enableRuntimeValidation && this.autoValidateOverwrites)
				{
					RollbackMismatchReport rollbackMismatchReport;
					this.rollbackDebugger.TestStates(state, frame, out rollbackMismatchReport);
				}
				this.replay.savedStates[num] = state;
			}
		}
		if (this.replay.savedHashes.Count == num)
		{
			this.replay.savedHashes.Add(state.GetMemberwiseHashCode());
		}
		else
		{
			this.replay.savedHashes[num] = state.GetMemberwiseHashCode();
		}
	}

	public void Clear()
	{
		this.replay.Clear();
	}

	private void setupReplayPlayerMappings()
	{
		this.userInputManager.ResetPlayerMapping();
		for (int i = 0; i < this.Replay.startPayload.players.Length; i++)
		{
			this.userInputManager.ForceBindAvailablePortToPlayerNoUser(this.Replay.startPayload.players[i].playerNum);
		}
	}

	RollbackStateContainer IDebugReplaySystem.GetStateAtFrameEnd(int frame)
	{
		if (this.replay == null || !this.recordStates)
		{
			return null;
		}
		if (this.replay.savedStates.Count <= frame)
		{
			return null;
		}
		return this.replay.savedStates[frame];
	}

	short IDebugReplaySystem.GetHashAtFrameEnd(int frame)
	{
		if (this.replay == null)
		{
			return 0;
		}
		if (this.replay.savedHashes.Count <= frame)
		{
			return 0;
		}
		return this.replay.savedHashes[frame];
	}
}
