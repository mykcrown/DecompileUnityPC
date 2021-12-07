using System;
using System.Collections.Generic;
using BattleServer;
using IconsServer;

// Token: 0x02000876 RID: 2166
public interface IRollbackInputTracker : IRollbackInputStatus
{
	// Token: 0x06003615 RID: 13845
	void ConfirmInput(IRollbackClient client, int frame, int playerID, int startFrame, int index);

	// Token: 0x06003616 RID: 13846
	void SyncFrameWithAllInputs(IRollbackClient client);

	// Token: 0x06003617 RID: 13847
	void RecordInputAck(int playerID, InputEvent inputEvent);

	// Token: 0x06003618 RID: 13848
	void SendAllInputAcks();

	// Token: 0x06003619 RID: 13849
	int LatestFrameFrom(int playerID);

	// Token: 0x0600361A RID: 13850
	bool HasPlayerInputForFrame(int frame, int playerID);

	// Token: 0x0600361B RID: 13851
	void ResetToFrame(int frame);

	// Token: 0x0600361C RID: 13852
	void RecordFrameSent(InputEvent inputEvent);

	// Token: 0x0600361D RID: 13853
	void StoreReceivedInputData(int frame, int playerID, InputMsg.InputArrayData inputData);

	// Token: 0x17000D25 RID: 3365
	// (get) Token: 0x0600361E RID: 13854
	List<InputEvent> StoredSentInputsLastFrame { get; }

	// Token: 0x0600361F RID: 13855
	void ResetLatestSentInput();

	// Token: 0x06003620 RID: 13856
	void AddLatestSendInput(InputEvent evt);

	// Token: 0x17000D26 RID: 3366
	// (get) Token: 0x06003621 RID: 13857
	Dictionary<int, InputMsg.InputArrayData[]> StoredRemoteInputs { get; }

	// Token: 0x06003622 RID: 13858
	void Destroy();
}
