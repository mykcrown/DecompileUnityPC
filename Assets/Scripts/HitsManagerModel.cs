// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class HitsManagerModel : RollbackStateTyped<HitsManagerModel>
{
	public int nextUID;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<IHitOwner> hitOwners = new List<IHitOwner>(32);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<IHitOwner> hitOwnersActiveThisFrame = new List<IHitOwner>(32);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public List<IBoundsOwner> boundsOwners = new List<IBoundsOwner>(32);

	public override void CopyTo(HitsManagerModel target)
	{
		target.nextUID = this.nextUID;
		base.copyList<IHitOwner>(this.hitOwners, target.hitOwners);
		base.copyList<IHitOwner>(this.hitOwnersActiveThisFrame, target.hitOwnersActiveThisFrame);
		base.copyList<IBoundsOwner>(this.boundsOwners, target.boundsOwners);
	}

	public override object Clone()
	{
		HitsManagerModel hitsManagerModel = new HitsManagerModel();
		this.CopyTo(hitsManagerModel);
		return hitsManagerModel;
	}
}
