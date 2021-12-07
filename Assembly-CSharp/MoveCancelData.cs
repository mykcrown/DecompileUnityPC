using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000506 RID: 1286
[Serializable]
public class MoveCancelData
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x0008D29B File Offset: 0x0008B69B
	public AnimationClip landCancelClip
	{
		get
		{
			return this.landCancelClipFile.obj;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x0008D2A8 File Offset: 0x0008B6A8
	public AnimationClip leftLandCancelClip
	{
		get
		{
			return this.leftLandCancelClipFile.obj;
		}
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x0008D2B5 File Offset: 0x0008B6B5
	public static string GetClipName(MoveData move, bool isLeft)
	{
		return string.Format("{0}_land{1}", move.name, (!isLeft) ? string.Empty : "_left");
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x0008D2DC File Offset: 0x0008B6DC
	public LandCancelOptions GetLandCancelOptions()
	{
		return new LandCancelOptions
		{
			landMove = this.landMove,
			startLandAtCurrentFrame = this.startLandMoveAtCurrentFrame,
			transferHitDisabledTargets = this.landTransferHitDisabledTargets,
			transferChargeData = this.landTransferChargeData,
			haltMovementOnLand = this.haltMovementOnLand,
			maxHorizontalLandSpeed = this.maxHorizontalLandSpeed,
			bounceSpeed = (Fixed)((double)this.bounceSpeed),
			bounceParticle = this.bounceParticle,
			bounceCancelMove = this.bounceCancelMove,
			noAutoCancelFramesBegin = this.noAutoCancelFramesBegin,
			noAutoCancelFramesEnd = this.noAutoCancelFramesEnd,
			landCancelClip = this.landCancelClipFile.obj,
			leftLandCancelClip = this.leftLandCancelClipFile.obj,
			landLagFrames = this.landLagFrames,
			landLagVisualFrames = this.landLagVisualFrames,
			useLandCameraShake = this.useLandCameraShake,
			landingCameraShake = this.landingCameraShake,
			sound = this.sound
		};
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x0008D3EC File Offset: 0x0008B7EC
	public FallCancelOptions GetFallCancelOptions()
	{
		return new FallCancelOptions
		{
			fallMove = this.fallMove,
			startFallAtCurrentFrame = this.startFallMoveAtCurrentFrame,
			transferHitDisableTargets = this.fallTransferHitDisabledTargets
		};
	}

	// Token: 0x0400164C RID: 5708
	public bool haltMovementOnLand;

	// Token: 0x0400164D RID: 5709
	public Fixed maxHorizontalLandSpeed = 0;

	// Token: 0x0400164E RID: 5710
	public float bounceSpeed;

	// Token: 0x0400164F RID: 5711
	public MoveData bounceCancelMove;

	// Token: 0x04001650 RID: 5712
	public ParticleData bounceParticle;

	// Token: 0x04001651 RID: 5713
	public bool cancelOnLand;

	// Token: 0x04001652 RID: 5714
	public int noAutoCancelFramesBegin;

	// Token: 0x04001653 RID: 5715
	public int noAutoCancelFramesEnd;

	// Token: 0x04001654 RID: 5716
	public AnimationClipFile landCancelClipFile = new AnimationClipFile();

	// Token: 0x04001655 RID: 5717
	public AnimationClipFile leftLandCancelClipFile = new AnimationClipFile();

	// Token: 0x04001656 RID: 5718
	public int landLagFrames = 10;

	// Token: 0x04001657 RID: 5719
	public int landLagVisualFrames = 20;

	// Token: 0x04001658 RID: 5720
	public bool useLandCameraShake;

	// Token: 0x04001659 RID: 5721
	public MoveCameraShakeData landingCameraShake;

	// Token: 0x0400165A RID: 5722
	public AudioData sound;

	// Token: 0x0400165B RID: 5723
	public MoveData landMove;

	// Token: 0x0400165C RID: 5724
	public bool startLandMoveAtCurrentFrame;

	// Token: 0x0400165D RID: 5725
	public bool landTransferHitDisabledTargets = true;

	// Token: 0x0400165E RID: 5726
	public bool landTransferChargeData;

	// Token: 0x0400165F RID: 5727
	public bool cancelOnFall;

	// Token: 0x04001660 RID: 5728
	public MoveData fallMove;

	// Token: 0x04001661 RID: 5729
	public bool startFallMoveAtCurrentFrame;

	// Token: 0x04001662 RID: 5730
	public bool fallTransferHitDisabledTargets = true;
}
