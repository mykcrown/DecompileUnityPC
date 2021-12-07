using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000526 RID: 1318
public struct LandCancelOptions
{
	// Token: 0x0400175E RID: 5982
	public MoveData landMove;

	// Token: 0x0400175F RID: 5983
	public bool startLandAtCurrentFrame;

	// Token: 0x04001760 RID: 5984
	public bool transferHitDisabledTargets;

	// Token: 0x04001761 RID: 5985
	public bool transferChargeData;

	// Token: 0x04001762 RID: 5986
	public bool haltMovementOnLand;

	// Token: 0x04001763 RID: 5987
	public Fixed maxHorizontalLandSpeed;

	// Token: 0x04001764 RID: 5988
	public Fixed bounceSpeed;

	// Token: 0x04001765 RID: 5989
	public ParticleData bounceParticle;

	// Token: 0x04001766 RID: 5990
	public MoveData bounceCancelMove;

	// Token: 0x04001767 RID: 5991
	public int noAutoCancelFramesBegin;

	// Token: 0x04001768 RID: 5992
	public int noAutoCancelFramesEnd;

	// Token: 0x04001769 RID: 5993
	public AnimationClip landCancelClip;

	// Token: 0x0400176A RID: 5994
	public AnimationClip leftLandCancelClip;

	// Token: 0x0400176B RID: 5995
	public int landLagFrames;

	// Token: 0x0400176C RID: 5996
	public int landLagVisualFrames;

	// Token: 0x0400176D RID: 5997
	public bool useLandCameraShake;

	// Token: 0x0400176E RID: 5998
	public MoveCameraShakeData landingCameraShake;

	// Token: 0x0400176F RID: 5999
	public AudioData sound;
}
