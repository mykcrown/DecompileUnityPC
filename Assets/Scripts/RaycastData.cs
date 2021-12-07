// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public struct RaycastData
{
	public Vector3F cast;

	public RaycastHitData hit;

	public bool clockwise;

	public int castIndex;

	public RaycastData(Vector3F cast, RaycastHitData hit, bool clockwise, int castIndex)
	{
		this.cast = cast;
		this.hit = hit;
		this.clockwise = clockwise;
		this.castIndex = castIndex;
	}
}
