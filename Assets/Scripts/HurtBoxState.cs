// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class HurtBoxState : ISegmentCollider
{
	[IgnoreRollback(IgnoreRollbackType.Static)]
	public HurtBox hurtBox;

	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private IBodyOwner body;

	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private IHurtBoxOwner hurtBoxOwner;

	public SegmentColliderType Type
	{
		get
		{
			return SegmentColliderType.HurtBox;
		}
	}

	public Fixed Radius
	{
		get
		{
			return (Fixed)((double)this.hurtBox.radius);
		}
	}

	public Vector3F Point1
	{
		get
		{
			return this.hurtBox.GetPoint1(this.body);
		}
	}

	public Vector3F Point2
	{
		get
		{
			return this.hurtBox.GetPoint2(this.body);
		}
	}

	public bool IsCircle
	{
		get
		{
			return this.hurtBox.endBone == BodyPart.none;
		}
	}

	public HurtBoxState(HurtBox hurtBox, IBodyOwner body, IHurtBoxOwner hurtBoxOwner)
	{
		this.hurtBox = hurtBox;
		this.hurtBoxOwner = hurtBoxOwner;
		this.body = body;
	}

	public bool MatchesVisiblityState(HurtBoxVisibilityState visState)
	{
		return this.hurtBoxOwner.MatchesVisibilityState(this.hurtBox.startBone, visState);
	}

	public bool InteractsWithType(HitType hitType)
	{
		return (hitType != HitType.Grab && hitType != HitType.BlockableGrab) || this.hurtBox.isGrabbable;
	}

	public int Priority()
	{
		return (int)this.hurtBox.priority;
	}
}
