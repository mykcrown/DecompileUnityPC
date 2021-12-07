using System;

// Token: 0x020004C6 RID: 1222
[Serializable]
public class AimReticleComponentModel : RollbackStateTyped<AimReticleComponentModel>
{
	// Token: 0x06001B06 RID: 6918 RVA: 0x0008A059 File Offset: 0x00088459
	public override void CopyTo(AimReticleComponentModel target)
	{
		target.vfx = this.vfx;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x0008A067 File Offset: 0x00088467
	public override void Clear()
	{
		base.Clear();
		this.vfx = null;
	}

	// Token: 0x04001453 RID: 5203
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public GeneratedEffect vfx;
}
