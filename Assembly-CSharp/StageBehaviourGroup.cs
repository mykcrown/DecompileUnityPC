using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class StageBehaviourGroup : MonoBehaviour, ITickable, IRollbackStateOwner, IDestroyable, ICloneable
{
	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x0600271B RID: 10011 RVA: 0x000BF37F File Offset: 0x000BD77F
	// (set) Token: 0x0600271C RID: 10012 RVA: 0x000BF387 File Offset: 0x000BD787
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000997 RID: 2455
	// (get) Token: 0x0600271D RID: 10013 RVA: 0x000BF390 File Offset: 0x000BD790
	// (set) Token: 0x0600271E RID: 10014 RVA: 0x000BF398 File Offset: 0x000BD798
	[Inject]
	public IDependencyInjection dependencyInjection { get; set; }

	// Token: 0x0600271F RID: 10015 RVA: 0x000BF3A4 File Offset: 0x000BD7A4
	public void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		this.triggerDependency = triggerDependency;
		this.model = new StageBehaviourGroupModel();
		for (int i = 0; i < this.Behaviours.Count; i++)
		{
			this.Behaviours[i].Init();
			this.dependencyInjection.Inject(this.Behaviours[i]);
		}
		foreach (StageTrigger stageTrigger in this.Triggers)
		{
			StageTrigger stageTrigger2 = stageTrigger;
			stageTrigger2.Triggered = (Action<object>)Delegate.Combine(stageTrigger2.Triggered, new Action<object>(this.onTriggered));
		}
		this.model.shouldValidate = isSimulation;
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x000BF480 File Offset: 0x000BD880
	private void onTriggered(object context)
	{
		this.model.hasBeenTriggered = true;
		this.model.startFrame = this.triggerDependency.Frame;
		this.model.context = context;
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x000BF4B0 File Offset: 0x000BD8B0
	public virtual void Destroy()
	{
		foreach (StageTrigger stageTrigger in this.Triggers)
		{
			StageTrigger stageTrigger2 = stageTrigger;
			stageTrigger2.Triggered = (Action<object>)Delegate.Remove(stageTrigger2.Triggered, new Action<object>(this.onTriggered));
		}
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000BF528 File Offset: 0x000BD928
	public void TickFrame()
	{
		for (int i = 0; i < this.Behaviours.Count; i++)
		{
			if (this.model.hasBeenTriggered && this.triggerDependency.Frame - this.model.startFrame == this.Behaviours[i].StartDelay)
			{
				this.Behaviours[i].Play(this.model.context);
			}
		}
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000BF5AA File Offset: 0x000BD9AA
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<StageBehaviourGroupModel>(this.model));
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000BF5C4 File Offset: 0x000BD9C4
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageBehaviourGroupModel>(ref this.model);
		return true;
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x000BF5D4 File Offset: 0x000BD9D4
	public object Clone()
	{
		return null;
	}

	// Token: 0x04001CAC RID: 7340
	public string Name = string.Empty;

	// Token: 0x04001CAD RID: 7341
	public List<StageTrigger> Triggers = new List<StageTrigger>();

	// Token: 0x04001CAE RID: 7342
	private StageBehaviourGroupModel model;

	// Token: 0x04001CAF RID: 7343
	private IStageTriggerDependency triggerDependency;

	// Token: 0x04001CB0 RID: 7344
	public List<StageBehaviour> Behaviours = new List<StageBehaviour>();
}
