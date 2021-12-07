// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public abstract class EndGameCondition : ITickable, IRollbackStateOwner
{
	protected EndGameConditionModel endGameConditionModel;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	protected virtual IEndGameConditionModel Model
	{
		get
		{
			return this.endGameConditionModel;
		}
	}

	public bool IsFinished
	{
		get
		{
			return this.Model.IsFinished;
		}
	}

	public List<PlayerNum> Victors
	{
		get
		{
			return this.Model.Victors;
		}
	}

	public List<TeamNum> WinningTeams
	{
		get
		{
			return this.Model.WinningTeams;
		}
	}

	public virtual float CurrentSeconds
	{
		get
		{
			return 0f;
		}
	}

	public virtual void TickFrame()
	{
	}

	public virtual void Destroy()
	{
	}

	public abstract bool ExportState(ref RollbackStateContainer container);

	public abstract bool LoadState(RollbackStateContainer container);
}
