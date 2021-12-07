// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StageTriggerController : IRollbackStateOwner, IDestroyable
{
	private List<StageTrigger> triggers;

	private List<StageBehaviourGroup> behaviourGroups;

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	public void Init(IStageTriggerDependency triggerDependency, List<StageTrigger> triggers, List<StageTrigger> visualTriggers, List<StageBehaviourGroup> behaviourGroups, List<StageBehaviourGroup> visualBehaviourGroups)
	{
		this.triggers = new List<StageTrigger>();
		this.behaviourGroups = new List<StageBehaviourGroup>();
		for (int i = 0; i < triggers.Count; i++)
		{
			this.injector.Inject(triggers[i]);
			triggers[i].Init(triggerDependency, true);
			this.triggers.Add(triggers[i]);
		}
		for (int j = 0; j < visualTriggers.Count; j++)
		{
			this.injector.Inject(visualTriggers[j]);
			visualTriggers[j].Init(triggerDependency, false);
			this.triggers.Add(visualTriggers[j]);
		}
		for (int k = 0; k < behaviourGroups.Count; k++)
		{
			this.injector.Inject(behaviourGroups[k]);
			behaviourGroups[k].Init(triggerDependency, true);
			this.behaviourGroups.Add(behaviourGroups[k]);
		}
		for (int l = 0; l < visualBehaviourGroups.Count; l++)
		{
			this.injector.Inject(visualBehaviourGroups[l]);
			visualBehaviourGroups[l].Init(triggerDependency, false);
			this.behaviourGroups.Add(visualBehaviourGroups[l]);
		}
	}

	public void Destroy()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			this.triggers[i].Destroy();
		}
		for (int j = 0; j < this.behaviourGroups.Count; j++)
		{
			this.behaviourGroups[j].Destroy();
		}
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			this.triggers[i].TickFrame();
		}
		for (int j = 0; j < this.behaviourGroups.Count; j++)
		{
			this.behaviourGroups[j].TickFrame();
		}
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			this.triggers[i].ExportState(ref container);
		}
		for (int j = 0; j < this.behaviourGroups.Count; j++)
		{
			this.behaviourGroups[j].ExportState(ref container);
		}
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		for (int i = 0; i < this.triggers.Count; i++)
		{
			this.triggers[i].LoadState(container);
		}
		for (int j = 0; j < this.behaviourGroups.Count; j++)
		{
			this.behaviourGroups[j].LoadState(container);
		}
		return true;
	}
}
