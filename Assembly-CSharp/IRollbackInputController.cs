using System;

// Token: 0x0200069A RID: 1690
public interface IRollbackInputController
{
	// Token: 0x06002A03 RID: 10755
	void ReadPlayerInputValues(ref InputValuesSnapshot state, bool tauntsOnly);

	// Token: 0x06002A04 RID: 10756
	void LoadInputValues(InputValuesSnapshot state);
}
