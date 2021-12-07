using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000B24 RID: 2852
public class MathUtil
{
	// Token: 0x060052A9 RID: 21161 RVA: 0x001AAAFD File Offset: 0x001A8EFD
	public static float Mix(float start, float end, float percent, float strength = 1f)
	{
		return MathUtil.MixIn(start, end, percent, strength);
	}

	// Token: 0x060052AA RID: 21162 RVA: 0x001AAB08 File Offset: 0x001A8F08
	public static float MixIn(float start, float end, float percent, float strength = 1f)
	{
		return Mathf.Pow(percent, strength) * (end - start) + start;
	}

	// Token: 0x060052AB RID: 21163 RVA: 0x001AAB17 File Offset: 0x001A8F17
	public static float MixOut(float start, float end, float percent, float strength = 1f)
	{
		return (1f - Mathf.Pow(1f - percent, strength)) * (end - start) + start;
	}

	// Token: 0x060052AC RID: 21164 RVA: 0x001AAB34 File Offset: 0x001A8F34
	public static float MixInOut(float start, float end, float percent, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (percent < 0.5f)
		{
			return MathUtil.MixIn(start, num, percent * 2f, strength);
		}
		return MathUtil.MixOut(num, end, (percent - 0.5f) * 2f, strength);
	}

	// Token: 0x060052AD RID: 21165 RVA: 0x001AAB80 File Offset: 0x001A8F80
	public static float MixOutIn(float start, float end, float percent, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (percent < 0.5f)
		{
			return MathUtil.MixOut(start, num, percent * 0.5f, strength);
		}
		return MathUtil.MixIn(num, end, (percent - 0.5f) * 2f, strength);
	}

	// Token: 0x060052AE RID: 21166 RVA: 0x001AABC9 File Offset: 0x001A8FC9
	public static float UnMix(float start, float end, float value, float strength = 1f)
	{
		return MathUtil.UnMixIn(start, end, value, strength);
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x001AABD4 File Offset: 0x001A8FD4
	public static float UnMixIn(float start, float end, float value, float strength = 1f)
	{
		return Mathf.Pow((value - start) / (end - start), 1f / strength);
	}

	// Token: 0x060052B0 RID: 21168 RVA: 0x001AABE9 File Offset: 0x001A8FE9
	public static float UnMixOut(float start, float end, float value, float strength = 1f)
	{
		return (Mathf.Pow(-1f * ((value - start) / (end - start) - 1f), 1f / strength) - 1f) * -1f;
	}

	// Token: 0x060052B1 RID: 21169 RVA: 0x001AAC18 File Offset: 0x001A9018
	public static float UnMixInOut(float start, float end, float value, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (value < num)
		{
			return MathUtil.UnMixIn(start, num, value, strength) * 0.5f;
		}
		return MathUtil.UnMixOut(num, end, value, strength) * 0.5f + 0.5f;
	}

	// Token: 0x060052B2 RID: 21170 RVA: 0x001AAC60 File Offset: 0x001A9060
	public static float UnMixOutIn(float start, float end, float value, float strength = 1f)
	{
		float num = (end - start) * 0.5f + start;
		if (value < num)
		{
			return MathUtil.UnMixOut(start, num, value, strength) * 0.5f;
		}
		return MathUtil.UnMixIn(num, end, value, strength) * 0.5f + 0.5f;
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001AACA8 File Offset: 0x001A90A8
	public static bool rectContainsPoint(Rect rect, Vector2 point, bool centeredAtTopLeft = true)
	{
		return !((point.x < rect.x || point.x > rect.x + rect.width || ((!centeredAtTopLeft) ? (point.y < rect.y) : (point.y > rect.y))) ? true : ((!centeredAtTopLeft) ? (point.y > rect.y + rect.height) : (point.y < rect.y - rect.height)));
	}

	// Token: 0x060052B4 RID: 21172 RVA: 0x001AAD54 File Offset: 0x001A9154
	public static Fixed toRadians(Fixed degrees)
	{
		return degrees * FixedMath.PI / 180;
	}

	// Token: 0x060052B5 RID: 21173 RVA: 0x001AAD70 File Offset: 0x001A9170
	public static float toRadians(float degrees)
	{
		return degrees * 3.1415927f / 180f;
	}

	// Token: 0x060052B6 RID: 21174 RVA: 0x001AAD7F File Offset: 0x001A917F
	public static float toDegrees(float radians)
	{
		return radians * 180f / 3.1415927f;
	}

	// Token: 0x060052B7 RID: 21175 RVA: 0x001AAD8E File Offset: 0x001A918E
	public static Fixed toDegrees(Fixed radians)
	{
		return radians * 180 / FixedMath.PI;
	}

	// Token: 0x060052B8 RID: 21176 RVA: 0x001AADA8 File Offset: 0x001A91A8
	public static bool almostEqual(Vector3 val1, Vector3 val2, float epsilon = 0.01f)
	{
		return MathUtil.almostEqual(val1.x, val2.x, epsilon) && MathUtil.almostEqual(val1.y, val2.y, epsilon) && MathUtil.almostEqual(val1.z, val2.z, epsilon);
	}

	// Token: 0x060052B9 RID: 21177 RVA: 0x001AAE07 File Offset: 0x001A9207
	public static bool almostEqual(float val1, float val2, float epsilon = 0.01f)
	{
		return Mathf.Abs(val1 - val2) < epsilon;
	}

	// Token: 0x060052BA RID: 21178 RVA: 0x001AAE14 File Offset: 0x001A9214
	public static bool almostGreater(float val1, float val2, float epsilon = 0.01f)
	{
		return val2 - val1 < epsilon;
	}

	// Token: 0x060052BB RID: 21179 RVA: 0x001AAE1C File Offset: 0x001A921C
	public static bool almostLess(float val1, float val2, float epsilon = 0.01f)
	{
		return val1 - val2 < epsilon;
	}

	// Token: 0x060052BC RID: 21180 RVA: 0x001AAE24 File Offset: 0x001A9224
	public static int GetSign(float val, float epsilon = 0.01f)
	{
		if (MathUtil.almostEqual(val, 0f, epsilon))
		{
			return 0;
		}
		return (val <= 0f) ? -1 : 1;
	}

	// Token: 0x060052BD RID: 21181 RVA: 0x001AAE4B File Offset: 0x001A924B
	public static int GetSign(Fixed val)
	{
		if (val == 0)
		{
			return 0;
		}
		return (!(val > 0)) ? -1 : 1;
	}

	// Token: 0x060052BE RID: 21182 RVA: 0x001AAE6E File Offset: 0x001A926E
	public static bool SignsMatch(float val1, float val2, float epsilon = 0.01f)
	{
		return MathUtil.GetSign(val1, epsilon) == MathUtil.GetSign(val2, epsilon);
	}

	// Token: 0x060052BF RID: 21183 RVA: 0x001AAE80 File Offset: 0x001A9280
	public static bool SignsMatch(Fixed val1, Fixed val2)
	{
		return MathUtil.GetSign(val1) == MathUtil.GetSign(val2);
	}

	// Token: 0x060052C0 RID: 21184 RVA: 0x001AAE90 File Offset: 0x001A9290
	public static int OppositeSign(float val, float epsilon = 0.01f)
	{
		if (MathUtil.almostEqual(val, 0f, epsilon))
		{
			return 0;
		}
		return (val <= 0f) ? 1 : -1;
	}

	// Token: 0x060052C1 RID: 21185 RVA: 0x001AAEB8 File Offset: 0x001A92B8
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

	// Token: 0x060052C2 RID: 21186 RVA: 0x001AAFD8 File Offset: 0x001A93D8
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

	// Token: 0x060052C3 RID: 21187 RVA: 0x001AB16C File Offset: 0x001A956C
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

	// Token: 0x060052C4 RID: 21188 RVA: 0x001AB28C File Offset: 0x001A968C
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

	// Token: 0x060052C5 RID: 21189 RVA: 0x001AB468 File Offset: 0x001A9868
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

	// Token: 0x060052C6 RID: 21190 RVA: 0x001AB6D4 File Offset: 0x001A9AD4
	public static bool IsLinePointBetweenSegmentBounds(Vector3F point, Vector3F start, Vector3F end)
	{
		bool flag = FixedMath.Abs(start.x - end.x) < (Fixed)0.0001;
		bool flag2 = FixedMath.Abs(start.y - end.y) < (Fixed)0.0001;
		bool flag3 = (flag && !flag2) || point.x == start.x || point.x == end.x || point.x - start.x > 0 != point.x - end.x > 0;
		bool flag4 = (!flag && flag2) || point.y == start.y || point.y == end.y || point.y - start.y > 0 != point.y - end.y > 0;
		return flag3 && flag4;
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x001AB834 File Offset: 0x001A9C34
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

	// Token: 0x060052C8 RID: 21192 RVA: 0x001ABA10 File Offset: 0x001A9E10
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
		float num = float.MaxValue;
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

	// Token: 0x060052C9 RID: 21193 RVA: 0x001ABAA8 File Offset: 0x001A9EA8
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

	// Token: 0x060052CA RID: 21194 RVA: 0x001ABB54 File Offset: 0x001A9F54
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
		float num = float.MaxValue;
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

	// Token: 0x060052CB RID: 21195 RVA: 0x001ABBEC File Offset: 0x001A9FEC
	public static bool AreLinesParallel(Vector2F ps1, Vector2F pe1, Vector2F ps2, Vector2F pe2)
	{
		Fixed one = pe1.y - ps1.y;
		Fixed other = ps1.x - pe1.x;
		Fixed one2 = pe2.y - ps2.y;
		Fixed other2 = ps2.x - pe2.x;
		Fixed one3 = one * other2 - one2 * other;
		return one3 == 0;
	}

	// Token: 0x060052CC RID: 21196 RVA: 0x001ABC68 File Offset: 0x001AA068
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

	// Token: 0x060052CD RID: 21197 RVA: 0x001ABD74 File Offset: 0x001AA174
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

	// Token: 0x060052CE RID: 21198 RVA: 0x001ABE2C File Offset: 0x001AA22C
	public static Vector3F RotateVector(Vector3F input, Fixed rotateZDegrees)
	{
		Fixed one = FixedMath.Cos(MathUtil.toRadians(rotateZDegrees));
		Fixed one2 = FixedMath.Sin(MathUtil.toRadians(rotateZDegrees));
		return new Vector3F(one * input.x - one2 * input.y, one2 * input.x + one * input.y, input.z);
	}

	// Token: 0x060052CF RID: 21199 RVA: 0x001ABE9C File Offset: 0x001AA29C
	public static Vector3 RotateVector(Vector3 input, float rotateZDegrees)
	{
		float num = Mathf.Cos(MathUtil.toRadians(rotateZDegrees));
		float num2 = Mathf.Sin(MathUtil.toRadians(rotateZDegrees));
		return new Vector3(num * input.x - num2 * input.y, num2 * input.x + num * input.y, input.z);
	}

	// Token: 0x060052D0 RID: 21200 RVA: 0x001ABEF3 File Offset: 0x001AA2F3
	public static Vector3F AngleToVector(int degrees)
	{
		return MathUtil.AngleToVector(degrees);
	}

	// Token: 0x060052D1 RID: 21201 RVA: 0x001ABF00 File Offset: 0x001AA300
	public static Vector3 AngleToVector(float degrees)
	{
		Vector3 vector = new Vector3(Mathf.Cos(MathUtil.toRadians(degrees)), Mathf.Sin(MathUtil.toRadians(degrees)));
		return vector.normalized;
	}

	// Token: 0x060052D2 RID: 21202 RVA: 0x001ABF34 File Offset: 0x001AA334
	public static Vector3F AngleToVector(Fixed degrees)
	{
		Vector3F vector3F = new Vector3F(FixedMath.Cos(MathUtil.toRadians(degrees)), FixedMath.Sin(MathUtil.toRadians(degrees)));
		return vector3F.normalized;
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001ABF65 File Offset: 0x001AA365
	public static Fixed VectorToAngle(ref Vector3F vector)
	{
		return MathUtil.toDegrees(FixedMath.Atan2(vector.y, vector.x));
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001ABF7D File Offset: 0x001AA37D
	public static Fixed VectorToAngle(ref Vector2F vector)
	{
		return MathUtil.toDegrees(FixedMath.Atan2(vector.y, vector.x));
	}

	// Token: 0x060052D5 RID: 21205 RVA: 0x001ABF95 File Offset: 0x001AA395
	public static float VectorToAngle(float x, float y)
	{
		return MathUtil.toDegrees(Mathf.Atan2(x, y));
	}

	// Token: 0x060052D6 RID: 21206 RVA: 0x001ABFA3 File Offset: 0x001AA3A3
	public static float VectorToAngle(ref Vector2 vector)
	{
		return MathUtil.toDegrees(Mathf.Atan2(vector.y, vector.x));
	}

	// Token: 0x060052D7 RID: 21207 RVA: 0x001ABFBB File Offset: 0x001AA3BB
	public static float VectorToAngle(ref Vector3 vector)
	{
		return MathUtil.toDegrees(Mathf.Atan2(vector.y, vector.x));
	}

	// Token: 0x060052D8 RID: 21208 RVA: 0x001ABFD4 File Offset: 0x001AA3D4
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

	// Token: 0x060052D9 RID: 21209 RVA: 0x001AC01A File Offset: 0x001AA41A
	public static Vector3 GetPerpendicularVector(Vector3 input)
	{
		return new Vector3(input.y, -input.x, input.z);
	}

	// Token: 0x060052DA RID: 21210 RVA: 0x001AC037 File Offset: 0x001AA437
	public static Vector3F GetPerpendicularVector(Vector3F input)
	{
		return new Vector3F(input.y, -input.x, input.z);
	}

	// Token: 0x060052DB RID: 21211 RVA: 0x001AC058 File Offset: 0x001AA458
	public static Vector2 GetPerpendicularVector(Vector2 input)
	{
		return new Vector3(input.y, -input.x);
	}

	// Token: 0x060052DC RID: 21212 RVA: 0x001AC073 File Offset: 0x001AA473
	public static Vector2F GetPerpendicularVector(Vector2F input)
	{
		return new Vector2F(input.y, -input.x);
	}

	// Token: 0x060052DD RID: 21213 RVA: 0x001AC090 File Offset: 0x001AA490
	public static int Modulo(int a, int b)
	{
		int num = a % b;
		return (num >= 0) ? num : (num + b);
	}

	// Token: 0x060052DE RID: 21214 RVA: 0x001AC0B4 File Offset: 0x001AA4B4
	public static int Wrap(int lowerBound, int upperBound, int val)
	{
		int b = upperBound - lowerBound;
		return MathUtil.Modulo(val, b) + lowerBound;
	}

	// Token: 0x060052DF RID: 21215 RVA: 0x001AC0D0 File Offset: 0x001AA4D0
	public static int IntLog2(int value)
	{
		uint num = (uint)value >> 16;
		if (num > 0U)
		{
			uint num2 = num >> 8;
			return (num2 <= 0U) ? (16 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num)]) : (24 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num2)]);
		}
		uint num3 = (uint)value >> 8;
		return (num3 <= 0U) ? ((int)MathUtil.LOG_TABLE_256[value]) : (8 + (int)MathUtil.LOG_TABLE_256[(int)((UIntPtr)num3)]);
	}

	// Token: 0x040034A4 RID: 13476
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
}
