// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StageBehaviourGroup : MonoBehaviour, ITickable, IRollbackStateOwner, IDestroyable, ICloneable
{
	public string Name = string.Empty;

	public List<StageTrigger> Triggers = new List<StageTrigger>();

	private StageBehaviourGroupModel model;

	private IStageTriggerDependency triggerDependency;

	public List<StageBehaviour> Behaviours = new List<StageBehaviour>();

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection dependencyInjection
	{
		get;
		set;
	}

	public void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		this.triggerDependency = triggerDependency;
		this.model = new StageBehaviourGroupModel();
		for (int i = 0; i < this.Behaviours.Count; i++)
		{
			this.Behaviours[i].Init();
			this.dependencyInjection.Inject(this.Behaviours[i]);
		}
		foreach (StageTrigger current in this.Triggers)
		{
			StageTrigger expr_70 = current;
			expr_70.Triggered = (Action<object>)Delegate.Combine(expr_70.Triggered, new Action<object>(this.onTriggered));
		}
		this.model.shouldValidate = isSimulation;
	}

	private void onTriggered(object context)
	{
		this.model.hasBeenTriggered = true;
		this.model.startFrame = this.triggerDependency.Frame;
		this.model.context = context;
	}

	public virtual void Destroy()
	{
		foreach (StageTrigger current in this.Triggers)
		{
			StageTrigger expr_1A = current;
			expr_1A.Triggered = (Action<object>)Delegate.Remove(expr_1A.Triggered, new Action<object>(this.onTriggered));
		}
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<StageBehaviourGroupModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageBehaviourGroupModel>(ref this.model);
		return true;
	}

	public object Clone()
	{
		return null;
	}
}
