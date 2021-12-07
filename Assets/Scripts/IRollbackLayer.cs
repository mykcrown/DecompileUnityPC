// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IRollbackLayer
{
	bool HasStarted
	{
		get;
	}

	bool HasDesynced
	{
		get;
	}

	ITimekeeper Timekeeper
	{
		get;
	}

	void StartSession(int localport, string remoteip, int remoteport);

	bool Idle(int timeout, out bool clientTicked, int maxFrames);

	void SaveLocalInputs(int frame, List<RollbackInput> localInputs, bool broadcast);

	void TickGameQuit();

	void TickNotFrame();

	void ValidFrame();

	void FillSkippedLocalInput(int frame, RollbackInput input);

	void CheckMissingInputs();

	void SyncInputForFrame(ref RollbackInput[] frameInputs);

	bool OnFrameAdvanced();

	void ResetToFrame(int frame);

	void ForceRollback(int toFrame);

	void Destroy();
}
