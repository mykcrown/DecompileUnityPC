using System;
using System.Collections.Generic;

// Token: 0x0200039A RID: 922
[Serializable]
public class BoneControllerState : RollbackStateTyped<BoneControllerState>
{
	// Token: 0x060013BA RID: 5050 RVA: 0x000704F6 File Offset: 0x0006E8F6
	public override void CopyTo(BoneControllerState targetIn)
	{
		targetIn.invert = this.invert;
		base.copyList<HurtBoxVisibilityState>(this.hurtBoxVisibility, targetIn.hurtBoxVisibility);
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x00070518 File Offset: 0x0006E918
	public override object Clone()
	{
		BoneControllerState boneControllerState = new BoneControllerState();
		this.CopyTo(boneControllerState);
		return boneControllerState;
	}

	// Token: 0x04000D3B RID: 3387
	public bool invert;

	// Token: 0x04000D3C RID: 3388
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<HurtBoxVisibilityState> hurtBoxVisibility = new List<HurtBoxVisibilityState>(32);
}
