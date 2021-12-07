using System;
using System.Collections.Generic;

// Token: 0x02000653 RID: 1619
public class StageTriggerController : IRollbackStateOwner, IDestroyable
{
	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x060027AD RID: 10157 RVA: 0x000C166C File Offset: 0x000BFA6C
	// (set) Token: 0x060027AE RID: 10158 RVA: 0x000C1674 File Offset: 0x000BFA74
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x060027AF RID: 10159 RVA: 0x000C1680 File Offset: 0x000BFA80
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

	// Token: 0x060027B0 RID: 10160 RVA: 0x000C17D0 File Offset: 0x000BFBD0
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

	// Token: 0x060027B1 RID: 10161 RVA: 0x000C1838 File Offset: 0x000BFC38
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

	// Token: 0x060027B2 RID: 10162 RVA: 0x000C18A0 File Offset: 0x000BFCA0
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

	// Token: 0x060027B3 RID: 10163 RVA: 0x000C190C File Offset: 0x000BFD0C
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

	// Token: 0x04001D02 RID: 7426
	private List<StageTrigger> triggers;

	// Token: 0x04001D03 RID: 7427
	private List<StageBehaviourGroup> behaviourGroups;
}
