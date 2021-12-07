using System;

// Token: 0x02000871 RID: 2161
public interface IRollbackStateOwner
{
	// Token: 0x060035E6 RID: 13798
	bool ExportState(ref RollbackStateContainer container);

	// Token: 0x060035E7 RID: 13799
	bool LoadState(RollbackStateContainer container);
}
