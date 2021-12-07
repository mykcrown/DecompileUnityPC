using System;
using FixedPoint;

// Token: 0x020003AA RID: 938
[Serializable]
public class EnvironmentBounds : CloneableObject, ICopyable<EnvironmentBounds>, ICopyable
{
	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06001423 RID: 5155 RVA: 0x00071A76 File Offset: 0x0006FE76
	public Fixed HalfWidth
	{
		get
		{
			return this.dimensions.x / 2;
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06001424 RID: 5156 RVA: 0x00071A89 File Offset: 0x0006FE89
	public Fixed HalfHeight
	{
		get
		{
			return this.dimensions.y / 2;
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x00071A9C File Offset: 0x0006FE9C
	public void CopyTo(EnvironmentBounds target)
	{
		target.centerOffset = this.centerOffset;
		target.dimensions = this.dimensions;
		target.up = this.up;
		target.down = this.down;
		target.left = this.left;
		target.right = this.right;
		target.lastUp = this.lastUp;
		target.lastDown = this.lastDown;
		target.lastRight = this.lastRight;
		target.lastLeft = this.lastLeft;
		target.lastCenterOffset = this.lastCenterOffset;
		target.dirty = this.dirty;
		target.d_leftUpperArm = this.d_leftUpperArm;
		target.d_rightUpperArm = this.d_rightUpperArm;
		target.d_leftCalf = this.d_leftCalf;
		target.d_rightCalf = this.d_rightCalf;
		target.d_rotated = this.d_rotated;
		target.d_animationName = this.d_animationName;
		target.d_animationFrame = this.d_animationFrame;
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00071B90 File Offset: 0x0006FF90
	public override object Clone()
	{
		EnvironmentBounds environmentBounds = new EnvironmentBounds();
		this.CopyTo(environmentBounds);
		return environmentBounds;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00071BAC File Offset: 0x0006FFAC
	public void Sync()
	{
		this.up = new Vector3F(0, this.dimensions.y / 2, 0);
		this.down = new Vector3F(0, -this.dimensions.y / 2, 0);
		this.left = new Vector3F(-this.dimensions.x / 2, 0, 0);
		this.right = new Vector3F(this.dimensions.x / 2, 0, 0);
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x00071C64 File Offset: 0x00070064
	public FixedRect getRect(Vector3F position)
	{
		return new FixedRect(position.x + this.left.x, position.y + this.up.y, this.right.x - this.left.x, this.up.y - this.down.y);
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x00071CDC File Offset: 0x000700DC
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"EnvironmentBounds: dimensions: ",
			this.dimensions,
			", centerOffset: ",
			this.centerOffset,
			", up: ",
			this.up,
			", down: ",
			this.down,
			", left: ",
			this.left,
			", right: ",
			this.right
		});
	}

	// Token: 0x04000D68 RID: 3432
	public Vector3F centerOffset = new Vector3F(0, 1, 0);

	// Token: 0x04000D69 RID: 3433
	public Vector2F dimensions = new Vector3F(1, 2, 0);

	// Token: 0x04000D6A RID: 3434
	public Vector3F up;

	// Token: 0x04000D6B RID: 3435
	public Vector3F down;

	// Token: 0x04000D6C RID: 3436
	public Vector3F left;

	// Token: 0x04000D6D RID: 3437
	public Vector3F right;

	// Token: 0x04000D6E RID: 3438
	public Vector3F lastUp;

	// Token: 0x04000D6F RID: 3439
	public Vector3F lastDown;

	// Token: 0x04000D70 RID: 3440
	public Vector3F lastRight;

	// Token: 0x04000D71 RID: 3441
	public Vector3F lastLeft;

	// Token: 0x04000D72 RID: 3442
	public Vector3F lastCenterOffset;

	// Token: 0x04000D73 RID: 3443
	public bool d_rotated;

	// Token: 0x04000D74 RID: 3444
	public Vector3F d_leftUpperArm;

	// Token: 0x04000D75 RID: 3445
	public Vector3F d_rightUpperArm;

	// Token: 0x04000D76 RID: 3446
	public Vector3F d_leftCalf;

	// Token: 0x04000D77 RID: 3447
	public Vector3F d_rightCalf;

	// Token: 0x04000D78 RID: 3448
	public string d_animationName;

	// Token: 0x04000D79 RID: 3449
	public int d_animationFrame;

	// Token: 0x04000D7A RID: 3450
	public bool dirty;
}
