using System;
using FixedPoint;

// Token: 0x02000395 RID: 917
[Serializable]
public class HurtBoxState : ISegmentCollider
{
	// Token: 0x060013A7 RID: 5031 RVA: 0x00070222 File Offset: 0x0006E622
	public HurtBoxState(HurtBox hurtBox, IBodyOwner body, IHurtBoxOwner hurtBoxOwner)
	{
		this.hurtBox = hurtBox;
		this.hurtBoxOwner = hurtBoxOwner;
		this.body = body;
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060013A8 RID: 5032 RVA: 0x0007023F File Offset: 0x0006E63F
	public SegmentColliderType Type
	{
		get
		{
			return SegmentColliderType.HurtBox;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00070242 File Offset: 0x0006E642
	public Fixed Radius
	{
		get
		{
			return (Fixed)((double)this.hurtBox.radius);
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x00070255 File Offset: 0x0006E655
	public Vector3F Point1
	{
		get
		{
			return this.hurtBox.GetPoint1(this.body);
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x00070268 File Offset: 0x0006E668
	public Vector3F Point2
	{
		get
		{
			return this.hurtBox.GetPoint2(this.body);
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x0007027B File Offset: 0x0006E67B
	public bool IsCircle
	{
		get
		{
			return this.hurtBox.endBone == BodyPart.none;
		}
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x0007028B File Offset: 0x0006E68B
	public bool MatchesVisiblityState(HurtBoxVisibilityState visState)
	{
		return this.hurtBoxOwner.MatchesVisibilityState(this.hurtBox.startBone, visState);
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000702A4 File Offset: 0x0006E6A4
	public bool InteractsWithType(HitType hitType)
	{
		return (hitType != HitType.Grab && hitType != HitType.BlockableGrab) || this.hurtBox.isGrabbable;
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x000702C6 File Offset: 0x0006E6C6
	public int Priority()
	{
		return (int)this.hurtBox.priority;
	}

	// Token: 0x04000D24 RID: 3364
	[IgnoreRollback(IgnoreRollbackType.Static)]
	public HurtBox hurtBox;

	// Token: 0x04000D25 RID: 3365
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private IBodyOwner body;

	// Token: 0x04000D26 RID: 3366
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private IHurtBoxOwner hurtBoxOwner;
}
