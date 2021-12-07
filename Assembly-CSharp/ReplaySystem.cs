using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MemberwiseEquality;
using RollbackDebug;
using UnityEngine;

// Token: 0x0200083D RID: 2109
public class ReplaySystem : IReplaySystem, IDebugReplaySystem
{
	// Token: 0x17000CC6 RID: 3270
	// (get) Token: 0x060034BD RID: 13501 RVA: 0x000F859D File Offset: 0x000F699D
	// (set) Token: 0x060034BE RID: 13502 RVA: 0x000F85A5 File Offset: 0x000F69A5
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x17000CC7 RID: 3271
	// (get) Token: 0x060034BF RID: 13503 RVA: 0x000F85AE File Offset: 0x000F69AE
	// (set) Token: 0x060034C0 RID: 13504 RVA: 0x000F85B6 File Offset: 0x000F69B6
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000CC8 RID: 3272
	// (get) Token: 0x060034C1 RID: 13505 RVA: 0x000F85BF File Offset: 0x000F69BF
	// (set) Token: 0x060034C2 RID: 13506 RVA: 0x000F85C7 File Offset: 0x000F69C7
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000CC9 RID: 3273
	// (get) Token: 0x060034C3 RID: 13507 RVA: 0x000F85D0 File Offset: 0x000F69D0
	// (set) Token: 0x060034C4 RID: 13508 RVA: 0x000F85D8 File Offset: 0x000F69D8
	[Inject]
	public IRollbackLayerDebugger rollbackDebugger { get; set; }

	// Token: 0x17000CCA RID: 3274
	// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000F85E1 File Offset: 0x000F69E1
	// (set) Token: 0x060034C6 RID: 13510 RVA: 0x000F85E9 File Offset: 0x000F69E9
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000CCB RID: 3275
	// (get) Token: 0x060034C7 RID: 13511 RVA: 0x000F85F2 File Offset: 0x000F69F2
	// (set) Token: 0x060034C8 RID: 13512 RVA: 0x000F85FA File Offset: 0x000F69FA
	[Inject]
	public IPlayerTauntsFinder tauntsFinder { get; set; }

	// Token: 0x17000CCC RID: 3276
	// (get) Token: 0x060034C9 RID: 13513 RVA: 0x000F8603 File Offset: 0x000F6A03
	private bool exceedsMaxLength
	{
		get
		{
			return this.replay.recordedInputs.Count >= this.maxFrameLength;
		}
	}

	// Token: 0x060034CA RID: 13514 RVA: 0x000F8620 File Offset: 0x000F6A20
	public void Init(ReplaySettings settings)
	{
		this.LoadSettings(settings);
		if (!Directory.Exists(ReplaySystem.REPLAY_DIR))
		{
			Directory.CreateDirectory(ReplaySystem.REPLAY_DIR);
		}
		this.rollbackDebugger.LoadReplaySystem(this);
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x000F8650 File Offset: 0x000F6A50
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

	// Token: 0x17000CCD RID: 3277
	// (get) Token: 0x060034CC RID: 13516 RVA: 0x000F86CC File Offset: 0x000F6ACC
	// (set) Token: 0x060034CD RID: 13517 RVA: 0x000F86D4 File Offset: 0x000F6AD4
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

	// Token: 0x17000CCE RID: 3278
	// (get) Token: 0x060034CE RID: 13518 RVA: 0x000F86DD File Offset: 0x000F6ADD
	public bool IsRemoteMode
	{
		get
		{
			return this.replay.replayType == ReplayType.Rollback && !this.flattenRollbackInput;
		}
	}

	// Token: 0x17000CCF RID: 3279
	// (get) Token: 0x060034CF RID: 13519 RVA: 0x000F86FC File Offset: 0x000F6AFC
	// (set) Token: 0x060034D0 RID: 13520 RVA: 0x000F8704 File Offset: 0x000F6B04
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

	// Token: 0x17000CD0 RID: 3280
	// (get) Token: 0x060034D1 RID: 13521 RVA: 0x000F8713 File Offset: 0x000F6B13
	public bool RecordHashes
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000CD1 RID: 3281
	// (get) Token: 0x060034D2 RID: 13522 RVA: 0x000F8716 File Offset: 0x000F6B16
	// (set) Token: 0x060034D3 RID: 13523 RVA: 0x000F871E File Offset: 0x000F6B1E
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

	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x060034D4 RID: 13524 RVA: 0x000F8727 File Offset: 0x000F6B27
	public bool IsRecording
	{
		get
		{
			return this.replayMode == ReplayMode.Record;
		}
	}

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000F8732 File Offset: 0x000F6B32
	public bool IsReplaying
	{
		get
		{
			return this.replayMode == ReplayMode.Replay;
		}
	}

	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x060034D6 RID: 13526 RVA: 0x000F873D File Offset: 0x000F6B3D
	// (set) Token: 0x060034D7 RID: 13527 RVA: 0x000F8745 File Offset: 0x000F6B45
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

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x060034D8 RID: 13528 RVA: 0x000F874E File Offset: 0x000F6B4E
	bool IReplaySystem.EnableRuntimeValidation
	{
		get
		{
			return this.enableRuntimeValidation;
		}
	}

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x060034D9 RID: 13529 RVA: 0x000F8756 File Offset: 0x000F6B56
	public Replay Replay
	{
		get
		{
			return this.replay;
		}
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x000F875E File Offset: 0x000F6B5E
	private string getReplayPath(string replayName)
	{
		return string.Format("{0}{1}{2}.log", ReplaySystem.REPLAY_DIR, Path.DirectorySeparatorChar, replayName);
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x000F877C File Offset: 0x000F6B7C
	private void writeReplay(string replayName)
	{
		if (this.exceedsMaxLength)
		{
			Debug.LogError(string.Concat(new object[]
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

	// Token: 0x060034DC RID: 13532 RVA: 0x000F8828 File Offset: 0x000F6C28
	private Replay readReplay(string replayName)
	{
		string replayPath = this.getReplayPath(replayName);
		return Serialization.Read<Replay>(replayPath, this.serializeType, true);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x000F884C File Offset: 0x000F6C4C
	bool IReplaySystem.HasFile(string replayName)
	{
		string replayPath = this.getReplayPath(replayName);
		return File.Exists(replayPath);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x000F8868 File Offset: 0x000F6C68
	bool IReplaySystem.LoadFromFile(string replayName)
	{
		this.replay = this.readReplay(replayName);
		if (this.replay == null)
		{
			return false;
		}
		if (this.exceedsMaxLength)
		{
			Debug.LogError(string.Concat(new object[]
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
			foreach (int key in this.replay.remoteInputsByFrame.Keys)
			{
				foreach (RollbackInput rollbackInput in this.replay.remoteInputsByFrame[key])
				{
					this.replay.recordedInputs[rollbackInput.frame][rollbackInput.playerID - 1] = rollbackInput;
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

	// Token: 0x060034DF RID: 13535 RVA: 0x000F89F8 File Offset: 0x000F6DF8
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
					object obj = enumerator.Current;
					PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x060034E0 RID: 13536 RVA: 0x000F8B10 File Offset: 0x000F6F10
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

	// Token: 0x060034E1 RID: 13537 RVA: 0x000F8B8A File Offset: 0x000F6F8A
	void IReplaySystem.Tick(int frame, IEndableClient client)
	{
		if (this.IsReplaying && this.replay.forfeitFrame > 0 && frame == this.replay.forfeitFrame)
		{
			client.EndGame();
		}
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x000F8BC0 File Offset: 0x000F6FC0
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

	// Token: 0x060034E3 RID: 13539 RVA: 0x000F8FB0 File Offset: 0x000F73B0
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

	// Token: 0x060034E4 RID: 13540 RVA: 0x000F9014 File Offset: 0x000F7414
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

	// Token: 0x060034E5 RID: 13541 RVA: 0x000F90C8 File Offset: 0x000F74C8
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
			Debug.LogError("Replay does not have a record of inputs for frame " + frame);
		}
		frameInputs = this.replay.recordedInputs[frame];
		return true;
	}

	// Token: 0x060034E6 RID: 13542 RVA: 0x000F9214 File Offset: 0x000F7614
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

	// Token: 0x060034E7 RID: 13543 RVA: 0x000F92DC File Offset: 0x000F76DC
	public void Clear()
	{
		this.replay.Clear();
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x000F92EC File Offset: 0x000F76EC
	private void setupReplayPlayerMappings()
	{
		this.userInputManager.ResetPlayerMapping();
		for (int i = 0; i < this.Replay.startPayload.players.Length; i++)
		{
			this.userInputManager.ForceBindAvailablePortToPlayerNoUser(this.Replay.startPayload.players[i].playerNum);
		}
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x000F9354 File Offset: 0x000F7754
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

	// Token: 0x060034EA RID: 13546 RVA: 0x000F93A2 File Offset: 0x000F77A2
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

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x060034EB RID: 13547 RVA: 0x000F93DA File Offset: 0x000F77DA
	bool IReplaySystem.ContainsRemoteInputs
	{
		get
		{
			return this.IsRemoteMode && this.replay.replayType == ReplayType.Rollback && this.replay.remoteInputsByFrame.Count > 0;
		}
	}

	// Token: 0x0400246A RID: 9322
	private Replay replay;

	// Token: 0x0400246B RID: 9323
	private Serialization.SerializeType serializeType = Serialization.SerializeType.XML;

	// Token: 0x0400246C RID: 9324
	private ReplayMode replayMode = ReplayMode.Record;

	// Token: 0x0400246D RID: 9325
	public bool recordStates;

	// Token: 0x0400246E RID: 9326
	private bool flattenRollbackInput;

	// Token: 0x0400246F RID: 9327
	private bool enableRuntimeValidation;

	// Token: 0x04002470 RID: 9328
	private bool autoValidateOverwrites;

	// Token: 0x04002471 RID: 9329
	private bool isDirty = true;

	// Token: 0x04002472 RID: 9330
	private int maxFrameLength;

	// Token: 0x04002473 RID: 9331
	private static readonly string REPLAY_DIR = "Replays";
}
