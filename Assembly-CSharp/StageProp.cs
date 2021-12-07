using System;

// Token: 0x02000647 RID: 1607
public abstract class StageProp : GameBehavior, ITickable, IRollbackStateOwner
{
	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x0600275C RID: 10076 RVA: 0x000BE5C0 File Offset: 0x000BC9C0
	// (set) Token: 0x0600275D RID: 10077 RVA: 0x000BE5C8 File Offset: 0x000BC9C8
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x0600275E RID: 10078
	public abstract bool IsSimulation { get; }

	// Token: 0x0600275F RID: 10079 RVA: 0x000BE5D1 File Offset: 0x000BC9D1
	public virtual void Init()
	{
	}

	// Token: 0x06002760 RID: 10080
	public abstract void TickFrame();

	// Token: 0x06002761 RID: 10081
	public abstract bool ExportState(ref RollbackStateContainer container);

	// Token: 0x06002762 RID: 10082
	public abstract bool LoadState(RollbackStateContainer container);
}
