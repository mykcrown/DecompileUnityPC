// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class MecanimControlState : RollbackStateTyped<MecanimControlState>
{
	[IgnoreCopyValidation, IsClonedManually]
	public AnimationData currentAnimation;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	public AnimationData currentAnimationPoolingRef = new AnimationData();

	[IgnoreCopyValidation, IsClonedManually]
	public AnimationData previousAnimation;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	public AnimationData previousAnimationPoolingRef = new AnimationData();

	public bool currentMirror;

	public Fixed currentBlendTime;

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

	public override object Clone()
	{
		MecanimControlState mecanimControlState = new MecanimControlState();
		this.CopyTo(mecanimControlState);
		return mecanimControlState;
	}
}
