using System;

// Token: 0x02000441 RID: 1089
public interface IMoveOwner
{
	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x060016AC RID: 5804
	MoveData MoveData { get; }

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x060016AD RID: 5805
	bool MoveIsValid { get; }

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x060016AE RID: 5806
	int InternalFrame { get; }
}
