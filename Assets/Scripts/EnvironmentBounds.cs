// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class EnvironmentBounds : CloneableObject, ICopyable<EnvironmentBounds>, ICopyable
{
	public Vector3F centerOffset = new Vector3F(0, 1, 0);

	public Vector2F dimensions = new Vector3F(1, 2, 0);

	public Vector3F up;

	public Vector3F down;

	public Vector3F left;

	public Vector3F right;

	public Vector3F lastUp;

	public Vector3F lastDown;

	public Vector3F lastRight;

	public Vector3F lastLeft;

	public Vector3F lastCenterOffset;

	public bool d_rotated;

	public Vector3F d_leftUpperArm;

	public Vector3F d_rightUpperArm;

	public Vector3F d_leftCalf;

	public Vector3F d_rightCalf;

	public string d_animationName;

	public int d_animationFrame;

	public bool dirty;

	public Fixed HalfWidth
	{
		get
		{
			return this.dimensions.x / 2;
		}
	}

	public Fixed HalfHeight
	{
		get
		{
			return this.dimensions.y / 2;
		}
	}

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

	public override object Clone()
	{
		EnvironmentBounds environmentBounds = new EnvironmentBounds();
		this.CopyTo(environmentBounds);
		return environmentBounds;
	}

	public void Sync()
	{
		this.up = new Vector3F(0, this.dimensions.y / 2, 0);
		this.down = new Vector3F(0, -this.dimensions.y / 2, 0);
		this.left = new Vector3F(-this.dimensions.x / 2, 0, 0);
		this.right = new Vector3F(this.dimensions.x / 2, 0, 0);
	}

	public FixedRect getRect(Vector3F position)
	{
		return new FixedRect(position.x + this.left.x, position.y + this.up.y, this.right.x - this.left.x, this.up.y - this.down.y);
	}

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
}
