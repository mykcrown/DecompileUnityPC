// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MathUtil
{
	private static readonly sbyte[] LOG_TABLE_256 = new sbyte[]
	{
		-1,
		0,
		1,
		1,
		2,
		2,
		2,
		2,
		3,
		3,
		3,
		3,
		3,
		3,
		3,
		3,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		4,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		5,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		6,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7,
		7
	};

	public static float Mix(float start, float end, float percent, float strength = 1f)
	{
		return MathUtil.MixIn(start, end, percent, strength);
	}

	public static float MixIn(float start, float end, float percent, float strength = 1f)
	{
		return Mathf.Pow(percent, strength) * (end - start) + start;
	}

	public static float MixOut(float start, float end, float percent, float strength = 1f)
	{
		return (1f - Mathf.Pow(1f - percent, strength)) * (end - start) + start;
	}

	public static float MixInOut(float start, float end, float percent, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (percent < 0.5f)
		{
			return MathUtil.MixIn(start, num, percent * 2f, strength);
		}
		return MathUtil.MixOut(num, end, (percent - 0.5f) * 2f, strength);
	}

	public static float MixOutIn(float start, float end, float percent, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (percent < 0.5f)
		{
			return MathUtil.MixOut(start, num, percent * 0.5f, strength);
		}
		return MathUtil.MixIn(num, end, (percent - 0.5f) * 2f, strength);
	}

	public static float UnMix(float start, float end, float value, float strength = 1f)
	{
		return MathUtil.UnMixIn(start, end, value, strength);
	}

	public static float UnMixIn(float start, float end, float value, float strength = 1f)
	{
		return Mathf.Pow((value - start) / (end - start), 1f / strength);
	}

	public static float UnMixOut(float start, float end, float value, float strength = 1f)
	{
		return (Mathf.Pow(-1f * ((value - start) / (end - start) - 1f), 1f / strength) - 1f) * -1f;
	}

	public static float UnMixInOut(float start, float end, float value, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (value < num)
		{
			return MathUtil.UnMixIn(start, num, value, strength) * 0.5f;
		}
		return MathUtil.UnMixOut(num, end, value, strength) * 0.5f + 0.5f;
	}

	public static float UnMixOutIn(float start, float end, float value, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (value < num)
		{
			return MathUtil.UnMixOut(start, num, value, strength) * 0.5f;
		}
		return MathUtil.UnMixIn(num, end, value, strength) * 0.5f + 0.5f;
	}

	public static bool rectContainsPoint(Rect rect, Vector2 point, bool centeredAtTopLeft = true)
	{
		return !((point.x < rect.x || point.x > rect.x + rect.width || ((!centeredAtTopLeft) ? (point.y < rect.y) : (point.y > rect.y))) ? true : ((!centeredAtTopLeft) ? (point.y > rect.y + rect.height) : (point.y < rect.y - rect.height)));
	}

	public static Fixed toRadians(Fixed degrees)
	{
		return degrees * FixedMath.PI / 180;
	}

	public static float toRadians(float degrees)
	{
		return degrees * 3.14159274f / 180f;
	}

	public static float toDegrees(float radians)
	{
		return radians * 180f / 3.14159274f;
	}

	public static Fixed toDegrees(Fixed radians)
	{
		return radians * 180 / FixedMath.PI;
	}

	public static bool almostEqual(Vector3 val1, Vector3 val2, float epsilon = 0.01f)
	{
		return MathUtil.almostEqual(val1.x, val2.x, epsilon) && MathUtil.almostEqual(val1.y, val2.y, epsilon) && MathUtil.almostEqual(val1.z, val2.z, epsilon);
	}

	public static bool almostEqual(float val1, float val2, float epsilon = 0.01f)
	{
		return Mathf.Abs(val1 - val2) < epsilon;
	}

	public static bool almostGreater(float val1, float val2, float epsilon = 0.01f)
	{
		return val2 - val1 < epsilon;
	}

	public static bool almostLess(float val1, float val2, float epsilon = 0.01f)
	{
		return val1 - val2 < epsilon;
	}

	public static int GetSign(float val, float epsilon = 0.01f)
	{
		if (MathUtil.almostEqual(val, 0f, epsilon))
		{
			return 0;
		}
		return (val <= 0f) ? (-1) : 1;
	}

	public static int GetSign(Fixed val)
	{
		if (val == 0)
		{
			return 0;
		}
		return (!(val > 0)) ? (-1) : 1;
	}

	public static bool SignsMatch(float val1, float val2, float epsilon = 0.01f)
	{
		return MathUtil.GetSign(val1, epsilon) == MathUtil.GetSign(val2, epsilon);
	}

	public static bool SignsMatch(Fixed val1, Fixed val2)
	{
		return MathUtil.GetSign(val1) == MathUtil.GetSign(val2);
	}

	public static int OppositeSign(float val, float epsilon = 0.01f)
	{
		if (MathUtil.almostEqual(val, 0f, epsilon))
		{
			return 0;
		}
		return (val <= 0f) ? 1 : (-1);
	}

	public static float SqDistBetweenPointAndLineSegment(Vector2 a, Vector2 b, Vector2 p)
	{
		float num = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
		if (num == 0f)
		{
			return (p - a).sqrMagnitude;
		}
		float num2 = ((p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y)) / num;
		num2 = Mathf.Max(0f, Mathf.Min(1f, num2));
		float num3 = a.x + num2 * (b.x - a.x);
		float num4 = a.y + num2 * (b.y - a.y);
		return (p.x - num3) * (p.x - num3) + (p.y - num4) * (p.y - num4);
	}

	public static Fixed SqDistBetweenPointAndLineSegment(Vector2F a, Vector2F b, Vector2F p)
	{
		Fixed @fixed = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
		if (@fixed == 0)
		{
			return (p - a).sqrMagnitude;
		}
		Fixed fixed2 = ((p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y)) / @fixed;
		fixed2 = FixedMath.Max(0, FixedMath.Min(1, fixed2));
		Fixed other = a.x + fixed2 * (b.x - a.x);
		Fixed other2 = a.y + fixed2 * (b.y - a.y);
		return (p.x - other) * (p.x - other) + (p.y - other2) * (p.y - other2);
	}

	public static float SqDistBetweenPointAndLineSegment(Vector3 a, Vector3 b, Vector3 p)
	{
		float num = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
		if (num == 0f)
		{
			return (p - a).sqrMagnitude;
		}
		float num2 = ((p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y)) / num;
		num2 = Mathf.Max(0f, Mathf.Min(1f, num2));
		float num3 = a.x + num2 * (b.x - a.x);
		float num4 = a.y + num2 * (b.y - a.y);
		return (p.x - num3) * (p.x - num3) + (p.y - num4) * (p.y - num4);
	}

	public static void FindLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out bool lines_intersect, out bool segments_intersect, out Vector2 intersection, out Vector2 close_p1, out Vector2 close_p2)
	{
		float num = p2.x - p1.x;
		float num2 = p2.y - p1.y;
		float num3 = p4.x - p3.x;
		float num4 = p4.y - p3.y;
		float num5 = num2 * num3 - num * num4;
		float num6 = ((p1.x - p3.x) * num4 + (p3.y - p1.y) * num3) / num5;
		if (float.IsInfinity(num6))
		{
			lines_intersect = false;
			segments_intersect = false;
			intersection = new Vector2(float.NaN, float.NaN);
			close_p1 = new Vector2(float.NaN, float.NaN);
			close_p2 = new Vector2(float.NaN, float.NaN);
			return;
		}
		lines_intersect = true;
		float num7 = ((p3.x - p1.x) * num2 + (p1.y - p3.y) * num) / -num5;
		intersection = new Vector2(p1.x + num * num6, p1.y + num2 * num6);
		segments_intersect = (num6 >= 0f && num6 <= 1f && num7 >= 0f && num7 <= 1f);
		if (num6 < 0f)
		{
			num6 = 0f;
		}
		else if (num6 > 1f)
		{
			num6 = 1f;
		}
		if (num7 < 0f)
		{
			num7 = 0f;
		}
		else if (num7 > 1f)
		{
			num7 = 1f;
		}
		close_p1 = new Vector2(p1.x + num * num6, p1.y + num2 * num6);
		close_p2 = new Vector2(p3.x + num3 * num7, p3.y + num4 * num7);
	}

	public static void FindLineIntersection(Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4, out bool lines_intersect, out bool segments_intersect, out Vector2F intersection, out Vector2F close_p1, out Vector2F close_p2)
	{
		Fixed @fixed = p2.x - p1.x;
		Fixed fixed2 = p2.y - p1.y;
		Fixed fixed3 = p4.x - p3.x;
		Fixed fixed4 = p4.y - p3.y;
		Fixed fixed5 = fixed2 * fixed3 - @fixed * fixed4;
		if (fixed5 == 0)
		{
			lines_intersect = false;
			segments_intersect = false;
			intersection = new Vector2F(0, 0);
			close_p1 = new Vector2F(0, 0);
			close_p2 = new Vector2F(0, 0);
			return;
		}
		Fixed fixed6 = ((p1.x - p3.x) * fixed4 + (p3.y - p1.y) * fixed3) / fixed5;
		lines_intersect = true;
		Fixed fixed7 = ((p3.x - p1.x) * fixed2 + (p1.y - p3.y) * @fixed) / -fixed5;
		intersection = new Vector2F(p1.x + @fixed * fixed6, p1.y + fixed2 * fixed6);
		segments_intersect = (fixed6 >= 0 && fixed6 <= 1 && fixed7 >= 0 && fixed7 <= 1);
		if (fixed6 < 0)
		{
			fixed6 = 0;
		}
		else if (fixed6 > 1)
		{
			fixed6 = 1;
		}
		if (fixed7 < 0)
		{
			fixed7 = 0;
		}
		else if (fixed7 > 1)
		{
			fixed7 = 1;
		}
		close_p1 = new Vector2F(p1.x + @fixed * fixed6, p1.y + fixed2 * fixed6);
		close_p2 = new Vector2F(p3.x + fixed3 * fixed7, p3.y + fixed4 * fixed7);
	}

	public static bool IsLinePointBetweenSegmentBounds(Vector3F point, Vector3F start, Vector3F end)
	{
		bool flag = FixedMath.Abs(start.x - end.x) < (Fixed)0.0001;
		bool flag2 = FixedMath.Abs(start.y - end.y) < (Fixed)0.0001;
		bool flag3 = (flag && !flag2) || point.x == start.x || point.x == end.x || point.x - start.x > 0 != point.x - end.x > 0;
		bool flag4 = (!flag && flag2) || point.y == start.y || point.y == end.y || point.y - start.y > 0 != point.y - end.y > 0;
		return flag3 && flag4;
	}

	public static void FindLineIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out bool lines_intersect, out bool segments_intersect, out Vector3 intersection, out Vector3 close_p1, out Vector3 close_p2)
	{
		float num = p2.x - p1.x;
		float num2 = p2.y - p1.y;
		float num3 = p4.x - p3.x;
		float num4 = p4.y - p3.y;
		float num5 = num2 * num3 - num * num4;
		float num6 = ((p1.x - p3.x) * num4 + (p3.y - p1.y) * num3) / num5;
		if (float.IsInfinity(num6))
		{
			lines_intersect = false;
			segments_intersect = false;
			intersection = new Vector3(float.NaN, float.NaN);
			close_p1 = new Vector3(float.NaN, float.NaN);
			close_p2 = new Vector3(float.NaN, float.NaN);
			return;
		}
		lines_intersect = true;
		float num7 = ((p3.x - p1.x) * num2 + (p1.y - p3.y) * num) / -num5;
		intersection = new Vector3(p1.x + num * num6, p1.y + num2 * num6);
		segments_intersect = (num6 >= 0f && num6 <= 1f && num7 >= 0f && num7 <= 1f);
		if (num6 < 0f)
		{
			num6 = 0f;
		}
		else if (num6 > 1f)
		{
			num6 = 1f;
		}
		if (num7 < 0f)
		{
			num7 = 0f;
		}
		else if (num7 > 1f)
		{
			num7 = 1f;
		}
		close_p1 = new Vector3(p1.x + num * num6, p1.y + num2 * num6);
		close_p2 = new Vector3(p3.x + num3 * num7, p3.y + num4 * num7);
	}

	public static float FindSqDistanceBetweenSegments(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		bool flag;
		bool flag2;
		Vector2 vector;
		Vector2 vector2;
		Vector2 vector3;
		MathUtil.FindLineIntersection(p1, p2, p3, p4, out flag, out flag2, out vector, out vector2, out vector3);
		if (flag2)
		{
			vector2 = vector;
			vector3 = vector;
			return 0f;
		}
		float num = 3.40282347E+38f;
		float num2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p1);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p2);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p3);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p4);
		if (num2 < num)
		{
			num = num2;
		}
		return num;
	}

	public static Fixed FindSqDistanceBetweenSegments(Vector2F p1, Vector2F p2, Vector2F p3, Vector2F p4)
	{
		bool flag;
		bool flag2;
		Vector2F vector2F;
		Vector2F vector2F2;
		Vector2F vector2F3;
		MathUtil.FindLineIntersection(p1, p2, p3, p4, out flag, out flag2, out vector2F, out vector2F2, out vector2F3);
		if (flag2)
		{
			vector2F2 = vector2F;
			vector2F3 = vector2F;
			return 0;
		}
		Fixed @fixed = Fixed.MaxValue;
		Fixed fixed2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p1);
		if (fixed2 < @fixed)
		{
			@fixed = fixed2;
		}
		fixed2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p2);
		if (fixed2 < @fixed)
		{
			@fixed = fixed2;
		}
		fixed2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p3);
		if (fixed2 < @fixed)
		{
			@fixed = fixed2;
		}
		fixed2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p4);
		if (fixed2 < @fixed)
		{
			@fixed = fixed2;
		}
		return @fixed;
	}

	public static float FindSqDistanceBetweenSegments(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	{
		bool flag;
		bool flag2;
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		MathUtil.FindLineIntersection(p1, p2, p3, p4, out flag, out flag2, out vector, out vector2, out vector3);
		if (flag2)
		{
			vector2 = vector;
			vector3 = vector;
			return 0f;
		}
		float num = 3.40282347E+38f;
		float num2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p1);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p3, p4, p2);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p3);
		if (num2 < num)
		{
			num = num2;
		}
		num2 = MathUtil.SqDistBetweenPointAndLineSegment(p1, p2, p4);
		if (num2 < num)
		{
			num = num2;
		}
		return num;
	}

	public static bool AreLinesParallel(Vector2F ps1, Vector2F pe1, Vector2F ps2, Vector2F pe2)
	{
		Fixed one = pe1.y - ps1.y;
		Fixed other = ps1.x - pe1.x;
		Fixed one2 = pe2.y - ps2.y;
		Fixed other2 = ps2.x - pe2.x;
		Fixed one3 = one * other2 - one2 * other;
		return one3 == 0;
	}

	public static Vector2F LineIntersectionPoint(Vector2F ps1, Vector2F pe1, Vector2F ps2, Vector2F pe2)
	{
		Fixed one = pe1.y - ps1.y;
		Fixed @fixed = ps1.x - pe1.x;
		Fixed other = one * ps1.x + @fixed * ps1.y;
		Fixed one2 = pe2.y - ps2.y;
		Fixed fixed2 = ps2.x - pe2.x;
		Fixed other2 = one2 * ps2.x + fixed2 * ps2.y;
		Fixed fixed3 = one * fixed2 - one2 * @fixed;
		if (fixed3 == 0)
		{
			throw new Exception("Lines are parallel");
		}
		return new Vector2F((fixed2 * other - @fixed * other2) / fixed3, (one * other2 - one2 * other) / fixed3);
	}

	public static Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
	{
		float num = pe1.y - ps1.y;
		float num2 = ps1.x - pe1.x;
		float num3 = num * ps1.x + num2 * ps1.y;
		float num4 = pe2.y - ps2.y;
		float num5 = ps2.x - pe2.x;
		float num6 = num4 * ps2.x + num5 * ps2.y;
		float num7 = num * num5 - num4 * num2;
		if (num7 == 0f)
		{
			throw new Exception("Lines are parallel");
		}
		return new Vector2((num5 * num3 - num2 * num6) / num7, (num * num6 - num4 * num3) / num7);
	}

	public static Vector3F RotateVector(Vector3F input, Fixed rotateZDegrees)
	{
		Fixed one = FixedMath.Cos(MathUtil.toRadians(rotateZDegrees));
		Fixed one2 = FixedMath.Sin(MathUtil.toRadians(rotateZDegrees));
		return new Vector3F(one * input.x - one2 * input.y, one2 * input.x + one * input.y, input.z);
	}

	public static Vector3 RotateVector(Vector3 input, float rotateZDegrees)
	{
		float num = Mathf.Cos(MathUtil.toRadians(rotateZDegrees));
		float num2 = Mathf.Sin(MathUtil.toRadians(rotateZDegrees));
		return new Vector3(num * input.x - num2 * input.y, num2 * input.x + num * input.y, input.z);
	}

	public static Vector3F AngleToVector(int degrees)
	{
		return MathUtil.AngleToVector(degrees);
	}

	public static Vector3 AngleToVector(float degrees)
	{
		Vector3 vector = new Vector3(Mathf.Cos(MathUtil.toRadians(degrees)), Mathf.Sin(MathUtil.toRadians(degrees)));
		return vector.normalized;
	}

	public static Vector3F AngleToVector(Fixed degrees)
	{
		Vector3F vector3F = new Vector3F(FixedMath.Cos(MathUtil.toRadians(degrees)), FixedMath.Sin(MathUtil.toRadians(degrees)));
		return vector3F.normalized;
	}

	public static Fixed VectorToAngle(ref Vector3F vector)
	{
		return MathUtil.toDegrees(FixedMath.Atan2(vector.y, vector.x));
	}

	public static Fixed VectorToAngle(ref Vector2F vector)
	{
		return MathUtil.toDegrees(FixedMath.Atan2(vector.y, vector.x));
	}

	public static float VectorToAngle(float x, float y)
	{
		return MathUtil.toDegrees(Mathf.Atan2(x, y));
	}

	public static float VectorToAngle(ref Vector2 vector)
	{
		return MathUtil.toDegrees(Mathf.Atan2(vector.y, vector.x));
	}

	public static float VectorToAngle(ref Vector3 vector)
	{
		return MathUtil.toDegrees(Mathf.Atan2(vector.y, vector.x));
	}

	public static int RoundAngleWithGranularity(int angle, int granularity)
	{
		if (angle < 360)
		{
			angle += 360;
		}
		if (granularity == 360)
		{
			return angle;
		}
		int num = angle % granularity;
		int num2 = angle / granularity;
		if (num > granularity / 2)
		{
			num2++;
		}
		return granularity * num2;
	}

	public static Vector3 GetPerpendicularVector(Vector3 input)
	{
		return new Vector3(input.y, -input.x, input.z);
	}

	public static Vector3F GetPerpendicularVector(Vector3F input)
	{
		return new Vector3F(input.y, -input.x, input.z);
	}

	public static Vector2 GetPerpendicularVector(Vector2 input)
	{
		return new Vector3(input.y, -input.x);
	}

	public static Vector2F GetPerpendicularVector(Vector2F input)
	{
		return new Vector2F(input.y, -input.x);
	}

	public static int Modulo(int a, int b)
	{
		int num = a % b;
		return (num >= 0) ? num : (num + b);
	}

	public static int Wrap(int lowerBound, int upperBound, int val)
	{
		int b = upperBound - lowerBound;
		return MathUtil.Modulo(val, b) + lowerBound;
	}

	public static int IntLog2(int value)
	{
		uint num = (uint)value >> 16;
		if (num > 0u)
		{
			uint num2 = num >> 8;
			return (num2 <= 0u) ? (16 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num)]) : (24 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num2)]);
		}
		uint num3 = (uint)value >> 8;
		return (num3 <= 0u) ? ((int)MathUtil.LOG_TABLE_256[value]) : (8 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num3)]);
	}
}
