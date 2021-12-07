// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class Replay
{
	private static int DEFAULT_CAPACITY = 30600;

	public DateTime timestamp;

	public ReplayType replayType;

	public GameLoadPayload startPayload;

	public int forfeitFrame;

	public List<RollbackStateContainer> savedStates = new List<RollbackStateContainer>();

	public List<short> savedHashes = new List<short>(Replay.DEFAULT_CAPACITY);

	public List<RollbackInput[]> recordedInputs = new List<RollbackInput[]>();

	public Dictionary<int, List<RollbackInput>> remoteInputsByFrame = new Dictionary<int, List<RollbackInput>>();

	public List<PlayerNum> localPlayers = new List<PlayerNum>();

	public bool IsValid
	{
		get
		{
			return this.startPayload != null && this.savedStates != null && this.recordedInputs != null;
		}
	}

	public void Clear()
	{
		this.savedHashes.Clear();
		this.savedStates.Clear();
		this.recordedInputs.Clear();
		this.forfeitFrame = 0;
		this.startPayload = null;
		this.remoteInputsByFrame.Clear();
		this.localPlayers.Clear();
	}
}
