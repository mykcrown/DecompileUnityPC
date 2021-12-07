using System;
using System.Collections.Generic;

// Token: 0x0200083F RID: 2111
public interface IReplaySystem
{
	// Token: 0x060034F1 RID: 13553
	void Init(ReplaySettings settings);

	// Token: 0x060034F2 RID: 13554
	void LoadSettings(ReplaySettings settings);

	// Token: 0x060034F3 RID: 13555
	bool HasFile(string replayName);

	// Token: 0x060034F4 RID: 13556
	bool LoadFromFile(string replayName);

	// Token: 0x060034F5 RID: 13557
	void OnGameStarted(ref GameLoadPayload payload);

	// Token: 0x060034F6 RID: 13558
	void OnGameFinished(int frame, VictoryScreenPayload payload, string filePath);

	// Token: 0x060034F7 RID: 13559
	void Tick(int frame, IEndableClient client);

	// Token: 0x17000CD8 RID: 3288
	// (get) Token: 0x060034F8 RID: 13560
	// (set) Token: 0x060034F9 RID: 13561
	ReplayMode Mode { get; set; }

	// Token: 0x17000CD9 RID: 3289
	// (get) Token: 0x060034FA RID: 13562
	// (set) Token: 0x060034FB RID: 13563
	bool RecordStates { get; set; }

	// Token: 0x17000CDA RID: 3290
	// (get) Token: 0x060034FC RID: 13564
	bool RecordHashes { get; }

	// Token: 0x17000CDB RID: 3291
	// (get) Token: 0x060034FD RID: 13565
	// (set) Token: 0x060034FE RID: 13566
	Serialization.SerializeType SerializeType { get; set; }

	// Token: 0x17000CDC RID: 3292
	// (get) Token: 0x060034FF RID: 13567
	bool IsRecording { get; }

	// Token: 0x17000CDD RID: 3293
	// (get) Token: 0x06003500 RID: 13568
	bool IsReplaying { get; }

	// Token: 0x17000CDE RID: 3294
	// (get) Token: 0x06003501 RID: 13569
	bool IsRemoteMode { get; }

	// Token: 0x17000CDF RID: 3295
	// (get) Token: 0x06003502 RID: 13570
	// (set) Token: 0x06003503 RID: 13571
	bool IsDirty { get; set; }

	// Token: 0x17000CE0 RID: 3296
	// (get) Token: 0x06003504 RID: 13572
	bool EnableRuntimeValidation { get; }

	// Token: 0x17000CE1 RID: 3297
	// (get) Token: 0x06003505 RID: 13573
	bool ContainsRemoteInputs { get; }

	// Token: 0x17000CE2 RID: 3298
	// (get) Token: 0x06003506 RID: 13574
	Replay Replay { get; }

	// Token: 0x06003507 RID: 13575
	List<RollbackInput> GetRemoteInputForFrame(int frame);

	// Token: 0x06003508 RID: 13576
	void RecordRemoteRollbackInput(int frame, RollbackInput remoteInput);

	// Token: 0x06003509 RID: 13577
	bool SynchronizeInput(int frame, ref RollbackInput[] frameInputs);

	// Token: 0x0600350A RID: 13578
	void RecordState(int frame, RollbackStateContainer state);

	// Token: 0x0600350B RID: 13579
	bool TestReplayEquality(string filePath1, string filePath2, ref string error);

	// Token: 0x0600350C RID: 13580
	void Clear();
}
