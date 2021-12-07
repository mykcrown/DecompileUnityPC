// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using IconsServer;
using System;
using System.Collections.Generic;

public interface IRollbackInputTracker : IRollbackInputStatus
{
	List<InputEvent> StoredSentInputsLastFrame
	{
		get;
	}

	Dictionary<int, InputMsg.InputArrayData[]> StoredRemoteInputs
	{
		get;
	}

	void ConfirmInput(IRollbackClient client, int frame, int playerID, int startFrame, int index);

	void SyncFrameWithAllInputs(IRollbackClient client);

	void RecordInputAck(int playerID, InputEvent inputEvent);

	void SendAllInputAcks();

	int LatestFrameFrom(int playerID);

	bool HasPlayerInputForFrame(int frame, int playerID);

	void ResetToFrame(int frame);

	void RecordFrameSent(InputEvent inputEvent);

	void StoreReceivedInputData(int frame, int playerID, InputMsg.InputArrayData inputData);

	void ResetLatestSentInput();

	void AddLatestSendInput(InputEvent evt);

	void Destroy();
}
