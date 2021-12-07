using System;

// Token: 0x020005DA RID: 1498
public interface IMoveUseTracker
{
	// Token: 0x060021EB RID: 8683
	bool HasMoveUsesLeft(MoveData move);

	// Token: 0x060021EC RID: 8684
	void Grounded();

	// Token: 0x060021ED RID: 8685
	void OnRemovedFromGame();

	// Token: 0x060021EE RID: 8686
	void OnGrabbed();

	// Token: 0x060021EF RID: 8687
	void OnReceiveHit();

	// Token: 0x060021F0 RID: 8688
	void OnGiveHit();

	// Token: 0x060021F1 RID: 8689
	void OnGrabLedge();

	// Token: 0x060021F2 RID: 8690
	void OnMoveStart(MoveData move);

	// Token: 0x060021F3 RID: 8691
	int GetMoveUsedCount(MoveLabel label);
}
