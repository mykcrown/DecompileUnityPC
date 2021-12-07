// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface ISegmentCollider
{
	SegmentColliderType Type
	{
		get;
	}

	Fixed Radius
	{
		get;
	}

	Vector3F Point1
	{
		get;
	}

	Vector3F Point2
	{
		get;
	}

	bool IsCircle
	{
		get;
	}

	bool MatchesVisiblityState(HurtBoxVisibilityState visState);

	bool InteractsWithType(HitType hitType);
}
