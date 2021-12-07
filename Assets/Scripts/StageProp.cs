// Decompile from assembly: Assembly-CSharp.dll

using System;

public abstract class StageProp : GameBehavior, ITickable, IRollbackStateOwner
{
	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public abstract bool IsSimulation
	{
		get;
	}

	public virtual void Init()
	{
	}

	public abstract void TickFrame();

	public abstract bool ExportState(ref RollbackStateContainer container);

	public abstract bool LoadState(RollbackStateContainer container);
}
