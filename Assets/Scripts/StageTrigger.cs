// Decompile from assembly: Assembly-CSharp.dll

using System;

public abstract class StageTrigger : GameBehavior, ITickable, IRollbackStateOwner, IDestroyable, ICloneable
{
	public string Name = string.Empty;

	public Action<object> Triggered;

	public SoundKey onTriggerAudio;

	protected IStageTriggerDependency triggerDependency;

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public void CallTriggered(object obj)
	{
		if (this.onTriggerAudio != SoundKey.empty)
		{
			base.audioManager.PlayGameSound(this.onTriggerAudio, null);
		}
		this.Triggered(obj);
	}

	public virtual void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		this.triggerDependency = triggerDependency;
	}

	public virtual void TickFrame()
	{
	}

	public virtual void Destroy()
	{
	}

	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		return true;
	}

	public virtual bool LoadState(RollbackStateContainer container)
	{
		return true;
	}

	public object Clone()
	{
		return null;
	}
}
