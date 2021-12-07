// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class HitBoxState : CloneableObject, ISegmentCollider, ICopyable<HitBoxState>, ICopyable
{
	public Vector3F lastPosition;

	public Vector3F position;

	public Fixed overrideRadius = 0;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	public HitBox data;

	public SegmentColliderType Type
	{
		get
		{
			return SegmentColliderType.HitBox;
		}
	}

	public Fixed Radius
	{
		get
		{
			return (!(this.overrideRadius == 0)) ? this.overrideRadius : ((Fixed)((double)this.data.radius));
		}
	}

	public Vector3F Point1
	{
		get
		{
			return this.position;
		}
	}

	public Vector3F Point2
	{
		get
		{
			return this.lastPosition;
		}
	}

	public bool IsCircle
	{
		get
		{
			return (this.lastPosition.x == 0 && this.lastPosition.y == 0) || (this.lastPosition.x == this.position.x && this.lastPosition.y == this.position.y);
		}
	}

	public void Load(HitBox data)
	{
		this.data = data;
	}

	public void CopyTo(HitBoxState target)
	{
		target.lastPosition = this.lastPosition;
		target.position = this.position;
		target.overrideRadius = this.overrideRadius;
		target.data = this.data;
	}

	public override object Clone()
	{
		HitBoxState hitBoxState = new HitBoxState();
		this.CopyTo(hitBoxState);
		return hitBoxState;
	}

	public Vector3F CalculatePosition(Vector3F position, Fixed angle, HorizontalDirection facing, Vector3F scale)
	{
		Vector3F b = (Vector3F)this.data.offset;
		b.x *= scale.x;
		b.y *= scale.y;
		b.z *= scale.z;
		if (this.data.isRelativeOffset)
		{
			b = MathUtil.RotateVector((Vector3F)this.data.offset, angle);
		}
		b.x *= InputUtils.GetDirectionMultiplier(facing);
		return position + b;
	}

	bool ISegmentCollider.MatchesVisiblityState(HurtBoxVisibilityState visState)
	{
		return false;
	}

	bool ISegmentCollider.InteractsWithType(HitType hitType)
	{
		return true;
	}
}
