// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class BoneControllerState : RollbackStateTyped<BoneControllerState>
{
	public bool invert;

	[IgnoreCopyValidation, IsClonedManually]
	public List<HurtBoxVisibilityState> hurtBoxVisibility = new List<HurtBoxVisibilityState>(32);

	public override void CopyTo(BoneControllerState targetIn)
	{
		targetIn.invert = this.invert;
		base.copyList<HurtBoxVisibilityState>(this.hurtBoxVisibility, targetIn.hurtBoxVisibility);
	}

	public override object Clone()
	{
		BoneControllerState boneControllerState = new BoneControllerState();
		this.CopyTo(boneControllerState);
		return boneControllerState;
	}
}
