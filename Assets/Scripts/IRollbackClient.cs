// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IRollbackClient : IRollbackStateOwner
{
	bool IsNetworkGame
	{
		get;
	}

	int Frame
	{
		get;
	}

	int GameStartInputFrame
	{
		get;
	}

	bool IsGameComplete
	{
		get;
	}

	bool IsRollingBack
	{
		get;
		set;
	}

	void ReportWaiting(double waitDuration);

	void ReportHealth(NetworkHealthReport health);

	void ReportErrors(List<string> errors);

	void Halt();

	void TickInput(int frame, bool isSkippedFrame);

	void TickFrame();

	void TickUpdate();
}
