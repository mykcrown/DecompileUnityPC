// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMatchDeepLogging
{
	void BeginMatch(string matchId, string opponentName);

	void EndMatch();

	void UpdateLoopBegin();

	void UpdateLoopEnd();

	void RecordTickStart(int frameDelta, int currentFrame);

	void RecordTickEnd();

	void RecordProcessingEnd();

	void RecordRenderFrame();

	void RecordRenderWaitError();

	void RecordFrameWaitError();

	void RecordRenderDelay();

	void RecordSyncWait();

	void RecordPreRender();

	void RecordPostRender();

	void RecordRollbackStatus();

	void RecordRemoteInput(int playerID, int startFrame, int numInputs);

	void RecordRollback(int framesTicked, double rollbackDurationMs);

	void RecordBroadcastLocalInputs(int localPlayerID, int playerID, int beginFrame);
}
