// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRollbackStatus
{
	bool IsRollingBack
	{
		get;
	}

	int FullyConfirmedFrame
	{
		get;
	}

	bool RollbackEnabled
	{
		get;
	}

	int InputDelayFrames
	{
		get;
	}

	int InputDelayPing
	{
		get;
	}

	int InitialTimestepDelta
	{
		get;
	}

	int CalculatedLatencyMs
	{
		get;
	}

	long MsSinceStart
	{
		get;
	}

	int LowestInputAckFrame
	{
		get;
	}
}
