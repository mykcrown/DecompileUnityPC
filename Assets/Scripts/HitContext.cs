// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class HitContext
{
	public Vector3F collisionPosition;

	public Vector3F collisionVelocity;

	public ISegmentCollider hurtBoxState;

	public int totalHitSuccess;

	public bool useKillFlourish;

	public HitData reflectorHitData;

	private static HitContext emptyObj = new HitContext();

	public static HitContext Null
	{
		get
		{
			return HitContext.emptyObj;
		}
	}

	public void Clear()
	{
		this.collisionPosition = default(Vector3F);
		this.collisionVelocity = default(Vector3F);
		this.hurtBoxState = null;
		this.useKillFlourish = false;
		this.totalHitSuccess = 0;
	}
}
