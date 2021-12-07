// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class PhysicsRaycastCalculator
{
	private static Fixed COLLISION_TOLERANCE = (Fixed)0.005;

	private static RaycastHitData[] hitBuffer = new RaycastHitData[1];

	public static bool CalculateRaycast(PhysicsContext context, Vector3F castPoint, Vector3F cast, int castIndex, int castMask, List<RaycastData> hitCasts, bool clockwise)
	{
		bool result = false;
		RaycastHitData hit;
		bool firstRaycastHit = PhysicsRaycastCalculator.GetFirstRaycastHit(context, castPoint, cast.normalized, cast.magnitude, castMask, out hit, default(Fixed));
		if (firstRaycastHit && !FixedMath.ApproximatelyEqual(cast.magnitude, hit.distance, PhysicsRaycastCalculator.COLLISION_TOLERANCE) && !FixedMath.ApproximatelyEqual(hit.distance, 0, PhysicsRaycastCalculator.COLLISION_TOLERANCE))
		{
			result = true;
			hitCasts.Add(new RaycastData
			{
				hit = hit,
				cast = cast,
				clockwise = clockwise,
				castIndex = castIndex
			});
		}
		return result;
	}

	public static bool GetFirstRaycastHit(PhysicsContext context, Vector2F origin, Vector2F normDirection, Fixed distance, int mask, out RaycastHitData hit, Fixed tolerance = default(Fixed))
	{
		int num = context.world.RaycastTerrain(origin, normDirection, distance, mask, PhysicsRaycastCalculator.hitBuffer, RaycastFlags.Default, tolerance);
		if (num > 0)
		{
			hit = PhysicsRaycastCalculator.hitBuffer[0];
			return true;
		}
		hit = default(RaycastHitData);
		return false;
	}
}
