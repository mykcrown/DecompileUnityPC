using System;
using System.Collections.Generic;

// Token: 0x0200042E RID: 1070
[Serializable]
public class DynamicObjectContainerState : RollbackStateTyped<DynamicObjectContainerState>
{
	// Token: 0x0600160C RID: 5644 RVA: 0x00077D2F File Offset: 0x0007612F
	public override void CopyTo(DynamicObjectContainerState target)
	{
		base.copyList<ITickable>(this.tickables, target.tickables);
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x00077D44 File Offset: 0x00076144
	public override object Clone()
	{
		DynamicObjectContainerState dynamicObjectContainerState = new DynamicObjectContainerState();
		this.CopyTo(dynamicObjectContainerState);
		return dynamicObjectContainerState;
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x00077D5F File Offset: 0x0007615F
	public override void Clear()
	{
		base.Clear();
		this.tickables.Clear();
	}

	// Token: 0x040010F7 RID: 4343
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public List<ITickable> tickables = new List<ITickable>(512);
}
