// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace FixedPoint
{
	[Serializable]
	public struct Vector2F
	{
		public Fixed x;

		public Fixed y;

		public Fixed this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.x;
				}
				if (index != 1)
				{
					throw new IndexOutOfRangeException("Invalid Vector2F index!");
				}
				return this.y;
			}
			set
			{
				if (index != 0)
				{
					if (index != 1)
					{
						throw new IndexOutOfRangeException("Invalid Vector2F index!");
					}
					this.y = value;
				}
				else
				{
					this.x = value;
				}
			}
		}

		public Vector2F normalized
		{
			get
			{
				Vector2F result = new Vector2F(this.x, this.y);
				result.Normalize();
				return result;
			}
		}

		public Fixed magnitude
		{
			get
			{
				return FixedMath.Sqrt(this.x * this.x + this.y * this.y);
			}
		}

		public Fixed sqrMagnitude
		{
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		public static Vector2F zero
		{
			get
			{
				return new Vector2F(0, 0);
			}
		}

		public static Vector2F one
		{
			get
			{
				return new Vector2F(1, 1);
			}
		}

		public static Vector2F up
		{
			get
			{
				return new Vector2F(0, 1);
			}
		}

		public static Vector2F down
		{
			get
			{
				return new Vector2F(0, -1);
			}
		}

		public static Vector2F left
		{
			get
			{
				return new Vector2F(-1, 0);
			}
		}

		public static Vector2F right
		{
			get
			{
				return new Vector2F(1, 0);
			}
		}

		public static Vector2F upRight
		{
			get
			{
				return new Vector2F(FixedMath.InverseSqrtTwo, FixedMath.InverseSqrtTwo);
			}
		}

		public static Vector2F upLeft
		{
			get
			{
				return new Vector2F(-FixedMath.InverseSqrtTwo, FixedMath.InverseSqrtTwo);
			}
		}

		public static Vector2F downLeft
		{
			get
			{
				return new Vector2F(-FixedMath.InverseSqrtTwo, -FixedMath.InverseSqrtTwo);
			}
		}

		public static Vector2F downRight
		{
			get
			{
				return new Vector2F(FixedMath.InverseSqrtTwo, -FixedMath.InverseSqrtTwo);
			}
		}

		public static Vector2F nan
		{
			get
			{
				return new Vector2F(Fixed.NaN, Fixed.NaN);
			}
		}

		public Vector2F(Fixed x, Fixed y)
		{
			this.x = x;
			this.y = y;
		}

		public Vector2F(Vector2 vec)
		{
			this.x = (Fixed)((double)vec.x);
			this.y = (Fixed)((double)vec.y);
		}

		public Vector2F(Vector3 vec)
		{
			this.x = (Fixed)((double)vec.x);
			this.y = (Fixed)((double)vec.y);
		}

		public void Set(Fixed new_x, Fixed new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		public static Vector2F Lerp(Vector2F a, Vector2F b, Fixed t)
		{
			t = FixedMath.Clamp01(t);
			return new Vector2F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		public static Vector2F LerpUnclamped(Vector2F a, Vector2F b, Fixed t)
		{
			return new Vector2F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		public static Vector2F MoveTowards(Vector2F current, Vector2F target, Fixed maxDistanceDelta)
		{
			Vector2F a = target - current;
			Fixed magnitude = a.magnitude;
			if (magnitude <= maxDistanceDelta || magnitude == 0)
			{
				return target;
			}
			return current + a / magnitude * maxDistanceDelta;
		}

		public static Vector2F Scale(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x * b.x, a.y * b.y);
		}

		public void Scale(Vector2F scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
		}

		public void Normalize()
		{
			Fixed magnitude = this.magnitude;
			if (magnitude > 0)
			{
				this /= magnitude;
			}
			else
			{
				this = Vector2F.zero;
			}
		}

		public void ClampToUnitCircle()
		{
			if (this.magnitude > 1)
			{
				this.Normalize();
			}
		}

		public void Decompose(ref Vector2F direction, ref Fixed magnitude)
		{
			magnitude = this.magnitude;
			if (magnitude > 0)
			{
				direction = this / magnitude;
			}
			else
			{
				direction = Vector2F.zero;
			}
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", this.x, this.y);
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector2F))
			{
				return false;
			}
			Vector2F vector2F = (Vector2F)other;
			return this.x.Equals(vector2F.x) && this.y.Equals(vector2F.y);
		}

		public static Vector2F Reflect(Vector2F inDirection, Vector2F inNormal)
		{
			return -2f * Vector2F.Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		public static Fixed Dot(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		public static Fixed Determinant(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x * rhs.y - lhs.y * rhs.x;
		}

		public static Fixed Angle(Vector2F from, Vector2F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector2F.Dot(from.normalized, to.normalized), -1, 1)) * 57.29578f;
		}

		public static Fixed Distance(Vector2F a, Vector2F b)
		{
			return (a - b).magnitude;
		}

		public static Vector2F ClampMagnitude(Vector2F vector, Fixed maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		public static Fixed SqrMagnitude(Vector2F a)
		{
			return a.x * a.x + a.y * a.y;
		}

		public static bool Approximately(Vector2F a, Vector2F b)
		{
			return Vector2F.Approximately(a, b, Fixed.Epsilon);
		}

		public static bool Approximately(Vector2F a, Vector2F b, Fixed epsilon)
		{
			return FixedMath.ApproximatelyEqual(a.x, b.x, epsilon) && FixedMath.ApproximatelyEqual(a.y, b.y, epsilon);
		}

		public static Vector2F RoundTo(Vector2F vec, Fixed desired, Fixed tolerance)
		{
			Vector2F result = vec;
			if (FixedMath.Abs(vec.x - desired) < tolerance)
			{
				result.x = desired;
			}
			if (FixedMath.Abs(vec.y - desired) < tolerance)
			{
				result.y = desired;
			}
			return result;
		}

		public Fixed SqrMagnitude()
		{
			return this.x * this.x + this.y * this.y;
		}

		public static Vector2F Min(Vector2F lhs, Vector2F rhs)
		{
			return new Vector2F(FixedMath.Min(lhs.x, rhs.x), FixedMath.Min(lhs.y, rhs.y));
		}

		public static Vector2F Max(Vector2F lhs, Vector2F rhs)
		{
			return new Vector2F(FixedMath.Max(lhs.x, rhs.x), FixedMath.Max(lhs.y, rhs.y));
		}

		public static Vector2F operator +(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x + b.x, a.y + b.y);
		}

		public static Vector2F operator -(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x - b.x, a.y - b.y);
		}

		public static Vector2F operator -(Vector2F a)
		{
			return new Vector2F(-a.x, -a.y);
		}

		public static Vector2F operator *(Vector2F a, Fixed d)
		{
			return new Vector2F(a.x * d, a.y * d);
		}

		public static Vector2F operator *(Fixed d, Vector2F a)
		{
			return new Vector2F(a.x * d, a.y * d);
		}

		public static Vector2F operator *(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x * b.x, a.y * b.y);
		}

		public static Vector2F operator /(Vector2F a, Fixed d)
		{
			return new Vector2F(a.x / d, a.y / d);
		}

		public static bool operator ==(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		public static bool operator !=(Vector2F lhs, Vector2F rhs)
		{
			return !(lhs.x == rhs.x) || !(lhs.y == rhs.y);
		}

		public static bool operator ==(Vector2F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && rhs.z == 0;
		}

		public static bool operator !=(Vector2F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && rhs.z == 0;
		}

		public static bool operator ==(Vector3F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == 0;
		}

		public static bool operator !=(Vector3F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == 0;
		}

		public static implicit operator Vector2F(Vector3F v)
		{
			return new Vector2F(v.x, v.y);
		}

		public static implicit operator Vector3F(Vector2F v)
		{
			return new Vector3F(v.x, v.y, 0);
		}

		public static explicit operator Vector3(Vector2F v)
		{
			return new Vector3((float)v.x, (float)v.y, 0f);
		}

		public static explicit operator Vector2(Vector2F v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		public static explicit operator Vector2F(Vector2 v)
		{
			return new Vector2F((Fixed)((double)v.x), (Fixed)((double)v.y));
		}

		public static explicit operator Vector2F(Vector3 v)
		{
			return new Vector2F((Fixed)((double)v.x), (Fixed)((double)v.y));
		}

		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.y);
		}

		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, 0f);
		}

		public static bool TryParse(string str, out Vector2F result)
		{
			string[] array = str.Split(new char[]
			{
				','
			});
			result = Vector2F.zero;
			if (array.Length < 2)
			{
				return false;
			}
			float num = 0f;
			float num2 = 0f;
			if (!float.TryParse(array[0], out num))
			{
				return false;
			}
			if (!float.TryParse(array[1], out num2))
			{
				return false;
			}
			result.x = (Fixed)((double)num);
			result.y = (Fixed)((double)num2);
			return true;
		}
	}
}
