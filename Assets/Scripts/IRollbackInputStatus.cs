// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IRollbackInputStatus
{
	Dictionary<int, int> PlayerInputAckStatus
	{
		get;
	}

	Dictionary<int, int> PlayerFrameReceived
	{
		get;
	}

	int FrameWithAllInputs
	{
		get;
	}

	int LowestInputAckFrame
	{
		get;
	}

	int CalculatedLatencyMs
	{
		get;
	}
}
