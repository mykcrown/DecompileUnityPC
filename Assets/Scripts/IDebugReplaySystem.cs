// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IDebugReplaySystem
{
	bool RecordStates
	{
		get;
	}

	bool RecordHashes
	{
		get;
	}

	RollbackStateContainer GetStateAtFrameEnd(int frame);

	short GetHashAtFrameEnd(int frame);
}
