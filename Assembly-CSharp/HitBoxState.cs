using System;
using FixedPoint;

// Token: 0x020003AB RID: 939
[Serializable]
public class HitBoxState : CloneableObject, ISegmentCollider, ICopyable<HitBoxState>, ICopyable
{
	// Token: 0x0600142B RID: 5163 RVA: 0x00071D90 File Offset: 0x00070190
	public void Load(HitBox data)
	{
		this.data = data;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00071D99 File Offset: 0x00070199
	public void CopyTo(HitBoxState target)
	{
		target.lastPosition = this.lastPosition;
		target.position = this.position;
		target.overrideRadius = this.overrideRadius;
		target.data = this.data;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x00071DCC File Offset: 0x000701CC
	public override object Clone()
	{
		HitBoxState hitBoxState = new HitBoxState();
		this.CopyTo(hitBoxState);
		return hitBoxState;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x00071DE8 File Offset: 0x000701E8
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

	// Token: 0x0600142F RID: 5167 RVA: 0x00071E97 File Offset: 0x00070297
	bool ISegmentCollider.MatchesVisiblityState(HurtBoxVisibilityState visState)
	{
		return false;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x00071E9A File Offset: 0x0007029A
	bool ISegmentCollider.InteractsWithType(HitType hitType)
	{
		return true;
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06001431 RID: 5169 RVA: 0x00071E9D File Offset: 0x0007029D
	public SegmentColliderType Type
	{
		get
		{
			return SegmentColliderType.HitBox;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06001432 RID: 5170 RVA: 0x00071EA0 File Offset: 0x000702A0
	public Fixed Radius
	{
		get
		{
			return (!(this.overrideRadius == 0)) ? this.overrideRadius : ((Fixed)((double)this.data.radius));
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06001433 RID: 5171 RVA: 0x00071ECF File Offset: 0x000702CF
	public Vector3F Point1
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06001434 RID: 5172 RVA: 0x00071ED7 File Offset: 0x000702D7
	public Vector3F Point2
	{
		get
		{
			return this.lastPosition;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06001435 RID: 5173 RVA: 0x00071EE0 File Offset: 0x000702E0
	public bool IsCircle
	{
		get
		{
			return (this.lastPosition.x == 0 && this.lastPosition.y == 0) || (this.lastPosition.x == this.position.x && this.lastPosition.y == this.position.y);
		}
	}

	// Token: 0x04000D7B RID: 3451
	public Vector3F lastPosition;

	// Token: 0x04000D7C RID: 3452
	public Vector3F position;

	// Token: 0x04000D7D RID: 3453
	public Fixed overrideRadius = 0;

	// Token: 0x04000D7E RID: 3454
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	public HitBox data;
}
