using System;

// Token: 0x02000652 RID: 1618
public abstract class StageTrigger : GameBehavior, ITickable, IRollbackStateOwner, IDestroyable, ICloneable
{
	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x060027A3 RID: 10147 RVA: 0x000BE8DB File Offset: 0x000BCCDB
	// (set) Token: 0x060027A4 RID: 10148 RVA: 0x000BE8E3 File Offset: 0x000BCCE3
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x060027A5 RID: 10149 RVA: 0x000BE8EC File Offset: 0x000BCCEC
	public void CallTriggered(object obj)
	{
		if (this.onTriggerAudio != SoundKey.empty)
		{
			base.audioManager.PlayGameSound(this.onTriggerAudio, null);
		}
		this.Triggered(obj);
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x000BE918 File Offset: 0x000BCD18
	public virtual void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		this.triggerDependency = triggerDependency;
	}

	// Token: 0x060027A7 RID: 10151 RVA: 0x000BE921 File Offset: 0x000BCD21
	public virtual void TickFrame()
	{
	}

	// Token: 0x060027A8 RID: 10152 RVA: 0x000BE923 File Offset: 0x000BCD23
	public virtual void Destroy()
	{
	}

	// Token: 0x060027A9 RID: 10153 RVA: 0x000BE925 File Offset: 0x000BCD25
	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		return true;
	}

	// Token: 0x060027AA RID: 10154 RVA: 0x000BE928 File Offset: 0x000BCD28
	public virtual bool LoadState(RollbackStateContainer container)
	{
		return true;
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000BE92B File Offset: 0x000BCD2B
	public object Clone()
	{
		return null;
	}

	// Token: 0x04001CFD RID: 7421
	public string Name = string.Empty;

	// Token: 0x04001CFE RID: 7422
	public Action<object> Triggered;

	// Token: 0x04001CFF RID: 7423
	public SoundKey onTriggerAudio;

	// Token: 0x04001D00 RID: 7424
	protected IStageTriggerDependency triggerDependency;
}
