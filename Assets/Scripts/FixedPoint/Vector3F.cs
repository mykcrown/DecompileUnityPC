// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FixedPoint
{
	[Serializable]
	public struct Vector3F
	{
		public Fixed x;

		public Fixed y;

		public Fixed z;

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

		public Vector3F normalized
		{
			get
			{
				return Vector3F.Normalize(this);
			}
		}

		public Fixed magnitude
		{
			get
			{
				return FixedMath.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
			}
		}

		public Fixed sqrMagnitude
		{
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}
		}

		public static Vector3F zero
		{
			get
			{
				return new Vector3F(0, 0, 0);
			}
		}

		public static Vector3F one
		{
			get
			{
				return new Vector3F(1, 1, 1);
			}
		}

		public static Vector3F forward
		{
			get
			{
				return new Vector3F(0, 0, 1);
			}
		}

		public static Vector3F back
		{
			get
			{
				return new Vector3F(0, 0, -1);
			}
		}

		public static Vector3F up
		{
			get
			{
				return new Vector3F(0, 1, 0);
			}
		}

		public static Vector3F down
		{
			get
			{
				return new Vector3F(0, -1, 0);
			}
		}

		public static Vector3F left
		{
			get
			{
				return new Vector3F(-1, 0, 0);
			}
		}

		public static Vector3F right
		{
			get
			{
				return new Vector3F(1, 0, 0);
			}
		}

		[Obsolete("Use Vector3F.forward instead.")]
		public static Vector3F fwd
		{
			get
			{
				return new Vector3F(0, 0, 1);
			}
		}

		public Vector3F(Fixed x, Fixed y, Fixed z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3F(Fixed x, Fixed y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}

		public static Vector3F Lerp(Vector3F a, Vector3F b, Fixed t)
		{
			t = FixedMath.Clamp01(t);
			return new Vector3F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		public static Vector3F LerpUnclamped(Vector3F a, Vector3F b, Fixed t)
		{
			return new Vector3F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
		}

		public static Vector3F Slerp(Vector3F a, Vector3F b, Fixed t)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_Slerp(ref a, ref b, t, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Slerp(ref Vector3F a, ref Vector3F b, Fixed t, out Vector3F value);

		public static Vector3F SlerpUnclamped(Vector3F a, Vector3F b, Fixed t)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_SlerpUnclamped(ref a, ref b, t, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SlerpUnclamped(ref Vector3F a, ref Vector3F b, Fixed t, out Vector3F value);

		private static void Internal_OrthoNormalize2(ref Vector3F a, ref Vector3F b)
		{
			Vector3F.INTERNAL_CALL_Internal_OrthoNormalize2(ref a, ref b);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_OrthoNormalize2(ref Vector3F a, ref Vector3F b);

		private static void Internal_OrthoNormalize3(ref Vector3F a, ref Vector3F b, ref Vector3F c)
		{
			Vector3F.INTERNAL_CALL_Internal_OrthoNormalize3(ref a, ref b, ref c);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_OrthoNormalize3(ref Vector3F a, ref Vector3F b, ref Vector3F c);

		public static void OrthoNormalize(ref Vector3F normal, ref Vector3F tangent)
		{
			Vector3F.Internal_OrthoNormalize2(ref normal, ref tangent);
		}

		public static void OrthoNormalize(ref Vector3F normal, ref Vector3F tangent, ref Vector3F binormal)
		{
			Vector3F.Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
		}

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

		public static Vector3F RotateTowards(Vector3F current, Vector3F target, Fixed maxRadiansDelta, Fixed maxMagnitudeDelta)
		{
			Vector3F result;
			Vector3F.INTERNAL_CALL_RotateTowards(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RotateTowards(ref Vector3F current, ref Vector3F target, Fixed maxRadiansDelta, Fixed maxMagnitudeDelta, out Vector3F value);

		public void Set(Fixed new_x, Fixed new_y, Fixed new_z)
		{
			this.x = new_x;
			this.y = new_y;
			this.z = new_z;
		}

		public static Vector3F Scale(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public void Scale(Vector3F scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
			this.z *= scale.z;
		}

		public static Vector3F Cross(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector3F))
			{
				return false;
			}
			Vector3F vector3F = (Vector3F)other;
			return this.x.Equals(vector3F.x) && this.y.Equals(vector3F.y) && this.z.Equals(vector3F.z);
		}

		public static Vector3F Reflect(Vector3F inDirection, Vector3F inNormal)
		{
			return -2f * Vector3F.Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		public static Vector3F Normalize(Vector3F value)
		{
			Fixed @fixed = Vector3F.Magnitude(value);
			if (@fixed > 0)
			{
				return value / @fixed;
			}
			return Vector3F.zero;
		}

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

		public static Fixed Dot(Vector3F lhs, Vector3F rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		public static Vector3F Project(Vector3F vector, Vector3F onNormal)
		{
			Fixed @fixed = Vector3F.Dot(onNormal, onNormal);
			if (@fixed == 0)
			{
				return Vector3F.zero;
			}
			return onNormal * Vector3F.Dot(vector, onNormal) / @fixed;
		}

		public static Vector3F ProjectOnPlane(Vector3F vector, Vector3F planeNormal)
		{
			return vector - Vector3F.Project(vector, planeNormal);
		}

		[Obsolete("Use Vector3F.ProjectOnPlane instead.")]
		public static Vector3F Exclude(Vector3F excludeThis, Vector3F fromThat)
		{
			return fromThat - Vector3F.Project(fromThat, excludeThis);
		}

		public static Fixed Angle(Vector3F from, Vector3F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector3F.Dot(from.normalized, to.normalized), -1, 1)) * (Fixed)57.29578;
		}

		public static Fixed Distance(Vector3F a, Vector3F b)
		{
			Vector3F vector3F = new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
			return FixedMath.Sqrt(vector3F.x * vector3F.x + vector3F.y * vector3F.y + vector3F.z * vector3F.z);
		}

		public static Vector3F ClampMagnitude(Vector3F vector, Fixed maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		public static Fixed Magnitude(Vector3F a)
		{
			return FixedMath.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
		}

		public static Fixed SqrMagnitude(Vector3F a)
		{
			return a.x * a.x + a.y * a.y + a.z * a.z;
		}

		public static Vector3F Min(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(FixedMath.Min(lhs.x, rhs.x), FixedMath.Min(lhs.y, rhs.y), FixedMath.Min(lhs.z, rhs.z));
		}

		public static Vector3F Max(Vector3F lhs, Vector3F rhs)
		{
			return new Vector3F(FixedMath.Max(lhs.x, rhs.x), FixedMath.Max(lhs.y, rhs.y), FixedMath.Max(lhs.z, rhs.z));
		}

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

		[Obsolete("Use Vector3F.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
		public static Fixed AngleBetween(Vector3F from, Vector3F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector3F.Dot(from.normalized, to.normalized), -1, 1));
		}

		public static Vector3F operator +(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector3F operator -(Vector3F a, Vector3F b)
		{
			return new Vector3F(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector3F operator -(Vector3F a)
		{
			return new Vector3F(-a.x, -a.y, -a.z);
		}

		public static Vector3F operator *(Vector3F a, Fixed d)
		{
			return new Vector3F(a.x * d, a.y * d, a.z * d);
		}

		public static Vector3F operator *(Fixed d, Vector3F a)
		{
			return new Vector3F(a.x * d, a.y * d, a.z * d);
		}

		public static Vector3F operator /(Vector3F a, Fixed d)
		{
			return new Vector3F(a.x / d, a.y / d, a.z / d);
		}

		public static bool operator ==(Vector3F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		public static bool operator !=(Vector3F lhs, Vector3F rhs)
		{
			return !(lhs.x == rhs.x) || !(lhs.y == rhs.y) || !(lhs.z == rhs.z);
		}

		public static explicit operator Vector2(Vector3F v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		public static explicit operator Vector3(Vector3F v)
		{
			return new Vector3((float)v.x, (float)v.y, (float)v.z);
		}

		public static explicit operator Vector3F(Vector2 v)
		{
			return new Vector3F((Fixed)((double)v.x), (Fixed)((double)v.y), 0);
		}

		public static explicit operator Vector3F(Vector3 v)
		{
			return new Vector3F((Fixed)((double)v.x), (Fixed)((double)v.y), (Fixed)((double)v.z));
		}
	}
}
