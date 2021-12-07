// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class AimReticleComponentModel : RollbackStateTyped<AimReticleComponentModel>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public GeneratedEffect vfx;

	public override void CopyTo(AimReticleComponentModel target)
	{
		target.vfx = this.vfx;
	}

	public override void Clear()
	{
		base.Clear();
		this.vfx = null;
	}
}
