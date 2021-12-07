using System;
using System.Collections.Generic;

// Token: 0x02000661 RID: 1633
[Serializable]
public class StaleMoveQueueState : RollbackStateTyped<StaleMoveQueueState>
{
	// Token: 0x060027F5 RID: 10229 RVA: 0x000C2850 File Offset: 0x000C0C50
	public override object Clone()
	{
		StaleMoveQueueState staleMoveQueueState = new StaleMoveQueueState();
		this.CopyTo(staleMoveQueueState);
		return staleMoveQueueState;
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x000C286B File Offset: 0x000C0C6B
	public override void CopyTo(StaleMoveQueueState target)
	{
		base.copyList<StaleEntry>(this.staleMoves, target.staleMoves);
	}

	// Token: 0x04001D2F RID: 7471
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<StaleEntry> staleMoves = new List<StaleEntry>(16);
}
