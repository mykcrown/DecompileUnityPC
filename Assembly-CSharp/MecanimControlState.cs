using System;
using FixedPoint;

// Token: 0x020001F0 RID: 496
[Serializable]
public class MecanimControlState : RollbackStateTyped<MecanimControlState>
{
	// Token: 0x06000921 RID: 2337 RVA: 0x0004F0C8 File Offset: 0x0004D4C8
	public override void CopyTo(MecanimControlState targetIn)
	{
		targetIn.currentMirror = this.currentMirror;
		targetIn.currentBlendTime = this.currentBlendTime;
		if (this.currentAnimation == null)
		{
			targetIn.currentAnimation = null;
		}
		else
		{
			targetIn.currentAnimation = targetIn.currentAnimationPoolingRef;
			this.currentAnimation.CopyTo(targetIn.currentAnimation);
		}
		if (this.previousAnimation == null)
		{
			targetIn.previousAnimation = null;
		}
		else
		{
			targetIn.previousAnimation = targetIn.previousAnimationPoolingRef;
			this.previousAnimation.CopyTo(targetIn.previousAnimation);
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0004F158 File Offset: 0x0004D558
	public override object Clone()
	{
		MecanimControlState mecanimControlState = new MecanimControlState();
		this.CopyTo(mecanimControlState);
		return mecanimControlState;
	}

	// Token: 0x0400066A RID: 1642
	[IsClonedManually]
	[IgnoreCopyValidation]
	public AnimationData currentAnimation;

	// Token: 0x0400066B RID: 1643
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Static)]
	public AnimationData currentAnimationPoolingRef = new AnimationData();

	// Token: 0x0400066C RID: 1644
	[IsClonedManually]
	[IgnoreCopyValidation]
	public AnimationData previousAnimation;

	// Token: 0x0400066D RID: 1645
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Static)]
	public AnimationData previousAnimationPoolingRef = new AnimationData();

	// Token: 0x0400066E RID: 1646
	public bool currentMirror;

	// Token: 0x0400066F RID: 1647
	public Fixed currentBlendTime;
}
