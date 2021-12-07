using System;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004F5 RID: 1269
[Serializable]
public class ForceData : ICloneable
{
	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x0008C033 File Offset: 0x0008A433
	// (set) Token: 0x06001BAC RID: 7084 RVA: 0x0008C03B File Offset: 0x0008A43B
	public bool casted { get; set; }

	// Token: 0x06001BAD RID: 7085 RVA: 0x0008C044 File Offset: 0x0008A444
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04001566 RID: 5478
	[FormerlySerializedAs("castingFrame")]
	public int frame;

	// Token: 0x04001567 RID: 5479
	[FormerlySerializedAs("resetPreviousVertical")]
	public bool resetYVelocity;

	// Token: 0x04001568 RID: 5480
	[FormerlySerializedAs("resetPreviousHorizontal")]
	public bool resetXVelocity;

	// Token: 0x04001569 RID: 5481
	public bool repeatCast;

	// Token: 0x0400156A RID: 5482
	public int endFrame;

	// Token: 0x0400156B RID: 5483
	public MoveForceType forceType = MoveForceType.Normal;

	// Token: 0x0400156C RID: 5484
	public ForceData.LegalForceState legalState;

	// Token: 0x0400156D RID: 5485
	public int maxRotationAngleAmount = 20;

	// Token: 0x0400156E RID: 5486
	public int neutralRotationAngle = 90;

	// Token: 0x0400156F RID: 5487
	public int minInput360Angle;

	// Token: 0x04001570 RID: 5488
	public int maxInput360Angle;

	// Token: 0x04001571 RID: 5489
	public bool hasDefaultInputDirection;

	// Token: 0x04001572 RID: 5490
	public int defaultInputDirection;

	// Token: 0x04001573 RID: 5491
	public float inputDirectionForce;

	// Token: 0x04001574 RID: 5492
	public bool setFacingDirection = true;

	// Token: 0x04001575 RID: 5493
	public Vector2F stickInfluence;

	// Token: 0x04001576 RID: 5494
	public Vector2 force;

	// Token: 0x04001577 RID: 5495
	public ForceData.MoveUseStaling[] forceStaling = new ForceData.MoveUseStaling[0];

	// Token: 0x04001578 RID: 5496
	public bool isFrictionForce;

	// Token: 0x04001579 RID: 5497
	public bool isContinuous;

	// Token: 0x0400157A RID: 5498
	public bool chargeScalesForce;

	// Token: 0x0400157B RID: 5499
	public bool applyOnChargeScaledDurationComplete;

	// Token: 0x0400157C RID: 5500
	public bool chargeScalesDuration;

	// Token: 0x0400157D RID: 5501
	public int minChargedDuration;

	// Token: 0x0400157E RID: 5502
	public int maxChargedDuration;

	// Token: 0x0400157F RID: 5503
	public string note = string.Empty;

	// Token: 0x020004F6 RID: 1270
	public enum LegalForceState
	{
		// Token: 0x04001582 RID: 5506
		All,
		// Token: 0x04001583 RID: 5507
		AirOnly,
		// Token: 0x04001584 RID: 5508
		GroundOnly
	}

	// Token: 0x020004F7 RID: 1271
	[Serializable]
	public class MoveUseStaling
	{
		// Token: 0x04001585 RID: 5509
		public int numUses;

		// Token: 0x04001586 RID: 5510
		public Vector2F multiplier;
	}
}
