// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class StaleMoveQueueState : RollbackStateTyped<StaleMoveQueueState>
{
	[IgnoreCopyValidation, IsClonedManually]
	public List<StaleEntry> staleMoves = new List<StaleEntry>(16);

	public override object Clone()
	{
		StaleMoveQueueState staleMoveQueueState = new StaleMoveQueueState();
		this.CopyTo(staleMoveQueueState);
		return staleMoveQueueState;
	}

	public override void CopyTo(StaleMoveQueueState target)
	{
		base.copyList<StaleEntry>(this.staleMoves, target.staleMoves);
	}
}
