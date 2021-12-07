// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IReplaySystem
{
	ReplayMode Mode
	{
		get;
		set;
	}

	bool RecordStates
	{
		get;
		set;
	}

	bool RecordHashes
	{
		get;
	}

	Serialization.SerializeType SerializeType
	{
		get;
		set;
	}

	bool IsRecording
	{
		get;
	}

	bool IsReplaying
	{
		get;
	}

	bool IsRemoteMode
	{
		get;
	}

	bool IsDirty
	{
		get;
		set;
	}

	bool EnableRuntimeValidation
	{
		get;
	}

	bool ContainsRemoteInputs
	{
		get;
	}

	Replay Replay
	{
		get;
	}

	void Init(ReplaySettings settings);

	void LoadSettings(ReplaySettings settings);

	bool HasFile(string replayName);

	bool LoadFromFile(string replayName);

	void OnGameStarted(ref GameLoadPayload payload);

	void OnGameFinished(int frame, VictoryScreenPayload payload, string filePath);

	void Tick(int frame, IEndableClient client);

	List<RollbackInput> GetRemoteInputForFrame(int frame);

	void RecordRemoteRollbackInput(int frame, RollbackInput remoteInput);

	bool SynchronizeInput(int frame, ref RollbackInput[] frameInputs);

	void RecordState(int frame, RollbackStateContainer state);

	bool TestReplayEquality(string filePath1, string filePath2, ref string error);

	void Clear();
}
