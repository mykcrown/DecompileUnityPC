using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FixedPoint
{
	// Token: 0x02000B23 RID: 2851
	[Serializable]
	public struct Vector3F
	{
		// Token: 0x06005266 RID: 21094 RVA: 0x001A9DEA File Offset: 0x001A81EA
		public Vector3F(Fixed x, Fixed y, Fixed z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x001A9E01 File Offset: 0x001A8201
		public Vector3F(Fixed x, Fixed y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

		// Token: 0x1700132F RID: 4911
		public Fixed this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.x;
				case 1:
					return this.y;
				case 2:
					return this.z;
				default:
					throw new IndexOutOfRangeException("Invalid Vector3F index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.x = value;
					break;
				case 1:
					this.y = value;
					break;
				case 2:
					this.z = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Vector3F index!");
				}
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x001A9EAB File Offset: 0x001A82AB
		public Vector3F normalized
		{
			get
			{
				return Vector3F.Normalize(this);
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x001A9EB8 File Offset: 0x001A82B8
		public Fixed magnitude
		{
			get
			{
				return FixedMath.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x0600526C RID: 21100 RVA: 0x001A9F07 File Offset: 0x001A8307
		public Fixed sqrMagnitude
		{
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x001A9F46 File Offset: 0x001A8346
		public static Vector3F zero
		{
			get
			{
				return new Vector3F(0, 0, 0);
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x0600526E RID: 21102 RVA: 0x001A9F5F File Offset: 0x001A835F
		public static Vector3F one
		{
			get
			{
				return new Vector3F(1, 1, 1);
			}
		}

		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x0600526F RID: 21103 RVA: 0x001A9F78 File Offset: 0x001A8378
		public static Vector3F forward
		{
			get
			{
				return new Vector3F(0, 0, 1);
			}
		}

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06005270 RID: 21104 RVA: 0x001A9F91 File Offset: 0x001A8391
		public static Vector3F back
		{
			get
			{
				return new Vector3F(0, 0, -1);
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06005271 RID: 21105 RVA: 0x001A9FAA File Offset: 0x001A83AA
		public static Vector3F up
		{
			get
			{
				return new Vector3F(0, 1, 0);
			}
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06005272 RID: 21106 RVA: 0x001A9FC3 File Offset: 0x001A83C3
		public static Vector3F down
		{
			get
			{
				return new Vector3F(0, -1, 0);
			}
		}

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06005273 RID: 21107 RVA: 0x001A9FDC File Offset: 0x001A83DC
		public static Vector3F left
		{
			get
			{
				return new Vector3F(-1, 0, 0);
			}
		}

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06005274 RID: 21108 RVA: 0x001A9FF5 File Offset: 0x001A83F5
		public static Vector3F right
		{
			get
			{
				return new Vector3F(1, 0, 0);
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x001AA00E File Offset: 0x001A840E
		[Obsolete("Use Vector3F.forward instead.")]
		public static Vector3F fwd
		{
			get
			{
				return new Vector3F(0, 0, 1);
			}
		}

		// Token: 0x06005276 RID: 21110 RVA: 0x001AA028 File Offset: 0x001A8428
		public static Vector3F Lerp(Vector3F a, Vector3F b, Fixed t)
		{
			t = FixedMath.Clamp01(t);
			return new Vector3F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x001AA0B4 File Offset: 0x001A84B4
		public static Vector3F LerpUnclamped(Vector3F a, Vector3F b, Fixed t)
		{
			return new Vector3F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x001AA138 File Offset: 0x001A8538
		public static Vector3F Slerp(Vector3F a, Vector3F b, Fixed t)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_Slerp(ref a, ref b, t, out result);
			return result;
		}

		// Token: 0x06005279 RID: 21113
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Slerp(ref Vector3F a, ref Vector3F b, Fixed t, out Vector3F value);

		// Token: 0x0600527A RID: 21114 RVA: 0x001AA154 File Offset: 0x001A8554
		public static Vector3F SlerpUnclamped(Vector3F a, Vector3F b, Fixed t)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_SlerpUnclamped(ref a, ref b, t, out result);
			return result;
		}

		// Token: 0x0600527B RID: 21115
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SlerpUnclamped(ref Vector3F a, ref Vector3F b, Fixed t, out Vector3F value);

		// Token: 0x0600527C RID: 21116 RVA: 0x001AA16E File Offset: 0x001A856E
		private static void Internal_OrthoNormalize2(ref Vector3F a, ref Vector3F b)
		{
			Vector3F.INTERNAL_CALL_Internal_OrthoNormalize2(ref a, ref b);
		}

		// Token: 0x0600527D RID: 21117
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_OrthoNormalize2(ref Vector3F a, ref Vector3F b);

		// Token: 0x0600527E RID: 21118 RVA: 0x001AA177 File Offset: 0x001A8577
		private static void Internal_OrthoNormalize3(ref Vector3F a, ref Vector3F b, ref Vector3F c)
		{
			Vector3F.INTERNAL_CALL_Internal_OrthoNormalize3(ref a, ref b, ref c);
		}

		// Token: 0x0600527F RID: 21119
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_OrthoNormalize3(ref Vector3F a, ref Vector3F b, ref Vector3F c);

		// Token: 0x06005280 RID: 21120 RVA: 0x001AA181 File Offset: 0x001A8581
		public static void OrthoNormalize(ref Vector3F normal, ref Vector3F tangent)
		{
			Vector3F.Internal_OrthoNormalize2(ref normal, ref tangent);
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001AA18A File Offset: 0x001A858A
		public static void OrthoNormalize(ref Vector3F normal, ref Vector3F tangent, ref Vector3F binormal)
		{
			Vector3F.Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x001AA194 File Offset: 0x001A8594
		public static Vector3F MoveTowards(Vector3F current, Vector3F target, Fixed maxDistanceDelta)
		{
			Vector3F a = target - current;
			Fixed magnitude = a.magnitude;
			if (magnitude <= maxDistanceDelta || magnitude == 0)
			{
				return target;
			}
			return current + a / magnitude * maxDistanceDelta;
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x001AA1E0 File Offset: 0x001A85E0
		public static Vector3F RotateTowards(Vector3F current, Vector3F target, Fixed maxRadiansDelta, Fixed maxMagnitudeDelta)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_RotateTowards(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta, out result);
			return result;
		}

		// Token: 0x06005284 RID: 21124
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RotateTowards(ref Vector3F current, ref Vector3F target, Fixed maxRadiansDelta, Fixed maxMagnitudeDelta, out Vector3F value);

		// Token: 0x06005285 RID: 21125 RVA: 0x001AA1FB File Offset: 0x001A85FB
		public void Set(Fixed new_x, Fixed new_y, Fixed new_z)
		{
			this.x = new_x;
			this.y = new_y;
			this.z = new_z;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x001AA212 File Offset: 0x001A8612
		public static Vector3F Scale(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001AA254 File Offset: 0x001A8654
		public void Scale(Vector3F scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
			this.z *= scale.z;
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001AA2AC File Offset: 0x001A86AC
		public static Vector3F Cross(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x001AA33F File Offset: 0x001A873F
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x001AA37C File Offset: 0x001A877C
		public override bool Equals(object other)
		{
			if (!(other is Vector3F))
			{
				return false;
			}
			Vector3F vector3F = (Vector3F)other;
			return this.x.Equals(vector3F.x) && this.y.Equals(vector3F.y) && this.z.Equals(vector3F.z);
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001AA401 File Offset: 0x001A8801
		public static Vector3F Reflect(Vector3F inDirection, Vector3F inNormal)
		{
			return -2f * Vector3F.Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x001AA420 File Offset: 0x001A8820
		public static Vector3F Normalize(Vector3F value)
		{
			Fixed @fixed = Vector3F.Magnitude(value);
			if (@fixed > 0)
			{
				return value / @fixed;
			}
			return Vector3F.zero;
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x001AA450 File Offset: 0x001A8850
		public void Normalize()
		{
			Fixed @fixed = Vector3F.Magnitude(this);
			if (@fixed > 0)
			{
				this /= @fixed;
			}
			else
			{
				this = Vector3F.zero;
			}
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x001AA498 File Offset: 0x001A8898
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.x,
				", ",
				this.y,
				", ",
				this.z,
				")"
			});
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x001AA4FC File Offset: 0x001A88FC
		public static Fixed Dot(Vector3F lhs, Vector3F rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x001AA54C File Offset: 0x001A894C
		public static Vector3F Project(Vector3F vector, Vector3F onNormal)
		{
			Fixed @fixed = Vector3F.Dot(onNormal, onNormal);
			if (@fixed == 0)
			{
				return Vector3F.zero;
			}
			return onNormal * Vector3F.Dot(vector, onNormal) / @fixed;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x001AA586 File Offset: 0x001A8986
		public static Vector3F ProjectOnPlane(Vector3F vector, Vector3F planeNormal)
		{
			return vector - Vector3F.Project(vector, planeNormal);
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x001AA595 File Offset: 0x001A8995
		[Obsolete("Use Vector3F.ProjectOnPlane instead.")]
		public static Vector3F Exclude(Vector3F excludeThis, Vector3F fromThat)
		{
			return fromThat - Vector3F.Project(fromThat, excludeThis);
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x001AA5A4 File Offset: 0x001A89A4
		public static Fixed Angle(Vector3F from, Vector3F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector3F.Dot(from.normalized, to.normalized), -1, 1)) * (Fixed)57.29578;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x001AA5E4 File Offset: 0x001A89E4
		public static Fixed Distance(Vector3F a, Vector3F b)
		{
			Vector3F vector3F = new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
			return FixedMath.Sqrt(vector3F.x * vector3F.x + vector3F.y * vector3F.y + vector3F.z * vector3F.z);
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x001AA679 File Offset: 0x001A8A79
		public static Vector3F ClampMagnitude(Vector3F vector, Fixed maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x001AA6A4 File Offset: 0x001A8AA4
		public static Fixed Magnitude(Vector3F a)
		{
			return FixedMath.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001AA6FC File Offset: 0x001A8AFC
		public static Fixed SqrMagnitude(Vector3F a)
		{
			return a.x * a.x + a.y * a.y + a.z * a.z;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x001AA74C File Offset: 0x001A8B4C
		public static Vector3F Min(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(FixedMath.Min(lhs.x, rhs.x), FixedMath.Min(lhs.y, rhs.y), FixedMath.Min(lhs.z, rhs.z));
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x001AA78C File Offset: 0x001A8B8C
		public static Vector3F Max(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(FixedMath.Max(lhs.x, rhs.x), FixedMath.Max(lhs.y, rhs.y), FixedMath.Max(lhs.z, rhs.z));
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x001AA7CC File Offset: 0x001A8BCC
		public static Vector3F RoundTo(Vector3F vec, Fixed desired, Fixed tolerance)
		{
			Vector3F result = vec;
			if (FixedMath.Abs(vec.x - desired) < tolerance)
			{
				result.x = desired;
			}
			if (FixedMath.Abs(vec.y - desired) < tolerance)
			{
				result.y = desired;
			}
			if (FixedMath.Abs(vec.z - desired) < tolerance)
			{
				result.z = desired;
			}
			return result;
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x001AA84B File Offset: 0x001A8C4B
		[Obsolete("Use Vector3F.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
		public static Fixed AngleBetween(Vector3F from, Vector3F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector3F.Dot(from.normalized, to.normalized), -1, 1));
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x001AA876 File Offset: 0x001A8C76
		public static Vector3F operator +(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x001AA8B6 File Offset: 0x001A8CB6
		public static Vector3F operator -(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x001AA8F6 File Offset: 0x001A8CF6
		public static Vector3F operator -(Vector3F a)
		{
			return new Vector3F(-a.x, -a.y, -a.z);
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x001AA921 File Offset: 0x001A8D21
		public static Vector3F operator *(Vector3F a, Fixed d)
		{
			return new Vector3F(a.x * d, a.y * d, a.z * d);
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x001AA94F File Offset: 0x001A8D4F
		public static Vector3F operator *(Fixed d, Vector3F a)
		{
			return new Vector3F(a.x * d, a.y * d, a.z * d);
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x001AA97D File Offset: 0x001A8D7D
		public static Vector3F operator /(Vector3F a, Fixed d)
		{
			return new Vector3F(a.x / d, a.y / d, a.z / d);
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x001AA9AC File Offset: 0x001A8DAC
		public static bool operator ==(Vector3F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x001AAA00 File Offset: 0x001A8E00
		public static bool operator !=(Vector3F lhs, Vector3F rhs)
		{
			return !(lhs.x == rhs.x) || !(lhs.y == rhs.y) || !(lhs.z == rhs.z);
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x001AAA56 File Offset: 0x001A8E56
		public static explicit operator Vector2(Vector3F v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x001AAA75 File Offset: 0x001A8E75
		public static explicit operator Vector3(Vector3F v)
		{
			return new Vector3((float)v.x, (float)v.y, (float)v.z);
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001AAAA0 File Offset: 0x001A8EA0
		public static explicit operator Vector3F(Vector2 v)
		{
			return new Vector3F((Fixed)((double)v.x), (Fixed)((double)v.y), 0);
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x001AAAC7 File Offset: 0x001A8EC7
		public static explicit operator Vector3F(Vector3 v)
		{
			return new Vector3F((Fixed)((double)v.x), (Fixed)((double)v.y), (Fixed)((double)v.z));
		}

		// Token: 0x040034A1 RID: 13473
		public Fixed x;

		// Token: 0x040034A2 RID: 13474
		public Fixed y;

		// Token: 0x040034A3 RID: 13475
		public Fixed z;
	}
}
