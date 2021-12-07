using System;
using System.Collections.Generic;

// Token: 0x020003B9 RID: 953
[Serializable]
public class HitsManagerModel : RollbackStateTyped<HitsManagerModel>
{
	// Token: 0x06001485 RID: 5253 RVA: 0x00072B70 File Offset: 0x00070F70
	public override void CopyTo(HitsManagerModel target)
	{
		target.nextUID = this.nextUID;
		base.copyList<IHitOwner>(this.hitOwners, target.hitOwners);
		base.copyList<IHitOwner>(this.hitOwnersActiveThisFrame, target.hitOwnersActiveThisFrame);
		base.copyList<IBoundsOwner>(this.boundsOwners, target.boundsOwners);
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x00072BC0 File Offset: 0x00070FC0
	public override object Clone()
	{
		HitsManagerModel hitsManagerModel = new HitsManagerModel();
		this.CopyTo(hitsManagerModel);
		return hitsManagerModel;
	}

	// Token: 0x04000DAF RID: 3503
	public int nextUID;

	// Token: 0x04000DB0 RID: 3504
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public List<IHitOwner> hitOwners = new List<IHitOwner>(32);

	// Token: 0x04000DB1 RID: 3505
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public List<IHitOwner> hitOwnersActiveThisFrame = new List<IHitOwner>(32);

	// Token: 0x04000DB2 RID: 3506
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public List<IBoundsOwner> boundsOwners = new List<IBoundsOwner>(32);
}
