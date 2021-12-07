using System;
using UnityEngine;

namespace FixedPoint
{
	// Token: 0x02000B22 RID: 2850
	[Serializable]
	public struct Vector2F
	{
		// Token: 0x06005224 RID: 21028 RVA: 0x001A92A0 File Offset: 0x001A76A0
		public Vector2F(Fixed x, Fixed y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x001A92B0 File Offset: 0x001A76B0
		public Vector2F(Vector2 vec)
		{
			this.x = (Fixed)((double)vec.x);
			this.y = (Fixed)((double)vec.y);
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001A92D8 File Offset: 0x001A76D8
		public Vector2F(Vector3 vec)
		{
			this.x = (Fixed)((double)vec.x);
			this.y = (Fixed)((double)vec.y);
		}

		// Token: 0x17001320 RID: 4896
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

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x001A9354 File Offset: 0x001A7754
		public Vector2F normalized
		{
			get
			{
				Vector2F result = new Vector2F(this.x, this.y);
				result.Normalize();
				return result;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x0600522A RID: 21034 RVA: 0x001A937C File Offset: 0x001A777C
		public Fixed magnitude
		{
			get
			{
				return FixedMath.Sqrt(this.x * this.x + this.y * this.y);
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x0600522B RID: 21035 RVA: 0x001A93AA File Offset: 0x001A77AA
		public Fixed sqrMagnitude
		{
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x0600522C RID: 21036 RVA: 0x001A93D3 File Offset: 0x001A77D3
		public static Vector2F zero
		{
			get
			{
				return new Vector2F(0, 0);
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x0600522D RID: 21037 RVA: 0x001A93E6 File Offset: 0x001A77E6
		public static Vector2F one
		{
			get
			{
				return new Vector2F(1, 1);
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x0600522E RID: 21038 RVA: 0x001A93F9 File Offset: 0x001A77F9
		public static Vector2F up
		{
			get
			{
				return new Vector2F(0, 1);
			}
		}

		// Token: 0x17001327 RID: 4903
		// (get) Token: 0x0600522F RID: 21039 RVA: 0x001A940C File Offset: 0x001A780C
		public static Vector2F down
		{
			get
			{
				return new Vector2F(0, -1);
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x06005230 RID: 21040 RVA: 0x001A941F File Offset: 0x001A781F
		public static Vector2F left
		{
			get
			{
				return new Vector2F(-1, 0);
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06005231 RID: 21041 RVA: 0x001A9432 File Offset: 0x001A7832
		public static Vector2F right
		{
			get
			{
				return new Vector2F(1, 0);
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x001A9445 File Offset: 0x001A7845
		public static Vector2F upRight
		{
			get
			{
				return new Vector2F(FixedMath.InverseSqrtTwo, FixedMath.InverseSqrtTwo);
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x001A9456 File Offset: 0x001A7856
		public static Vector2F upLeft
		{
			get
			{
				return new Vector2F(-FixedMath.InverseSqrtTwo, FixedMath.InverseSqrtTwo);
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06005234 RID: 21044 RVA: 0x001A946C File Offset: 0x001A786C
		public static Vector2F downLeft
		{
			get
			{
				return new Vector2F(-FixedMath.InverseSqrtTwo, -FixedMath.InverseSqrtTwo);
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x001A9487 File Offset: 0x001A7887
		public static Vector2F downRight
		{
			get
			{
				return new Vector2F(FixedMath.InverseSqrtTwo, -FixedMath.InverseSqrtTwo);
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06005236 RID: 21046 RVA: 0x001A949D File Offset: 0x001A789D
		public static Vector2F nan
		{
			get
			{
				return new Vector2F(Fixed.NaN, Fixed.NaN);
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x001A94AE File Offset: 0x001A78AE
		public void Set(Fixed new_x, Fixed new_y)
		{
			this.x = new_x;
			this.y = new_y;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x001A94C0 File Offset: 0x001A78C0
		public static Vector2F Lerp(Vector2F a, Vector2F b, Fixed t)
		{
			t = FixedMath.Clamp01(t);
			return new Vector2F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x001A9524 File Offset: 0x001A7924
		public static Vector2F LerpUnclamped(Vector2F a, Vector2F b, Fixed t)
		{
			return new Vector2F(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
		}

		// Token: 0x0600523A RID: 21050 RVA: 0x001A9580 File Offset: 0x001A7980
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

		// Token: 0x0600523B RID: 21051 RVA: 0x001A95CA File Offset: 0x001A79CA
		public static Vector2F Scale(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x * b.x, a.y * b.y);
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x001A95F7 File Offset: 0x001A79F7
		public void Scale(Vector2F scale)
		{
			this.x *= scale.x;
			this.y *= scale.y;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x001A962C File Offset: 0x001A7A2C
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

		// Token: 0x0600523E RID: 21054 RVA: 0x001A966E File Offset: 0x001A7A6E
		public void ClampToUnitCircle()
		{
			if (this.magnitude > 1)
			{
				this.Normalize();
			}
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x001A9688 File Offset: 0x001A7A88
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

		// Token: 0x06005240 RID: 21056 RVA: 0x001A96D9 File Offset: 0x001A7AD9
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.x, this.y);
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x001A96FB File Offset: 0x001A7AFB
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001A9724 File Offset: 0x001A7B24
		public override bool Equals(object other)
		{
			if (!(other is Vector2F))
			{
				return false;
			}
			Vector2F vector2F = (Vector2F)other;
			return this.x.Equals(vector2F.x) && this.y.Equals(vector2F.y);
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x001A9787 File Offset: 0x001A7B87
		public static Vector2F Reflect(Vector2F inDirection, Vector2F inNormal)
		{
			return -2f * Vector2F.Dot(inNormal, inDirection) * inNormal + inDirection;
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x001A97A6 File Offset: 0x001A7BA6
		public static Fixed Dot(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x001A97D3 File Offset: 0x001A7BD3
		public static Fixed Determinant(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x * rhs.y - lhs.y * rhs.x;
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x001A9800 File Offset: 0x001A7C00
		public static Fixed Angle(Vector2F from, Vector2F to)
		{
			return FixedMath.Acos(FixedMath.Clamp(Vector2F.Dot(from.normalized, to.normalized), -1, 1)) * 57.29578f;
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x001A9838 File Offset: 0x001A7C38
		public static Fixed Distance(Vector2F a, Vector2F b)
		{
			return (a - b).magnitude;
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x001A9854 File Offset: 0x001A7C54
		public static Vector2F ClampMagnitude(Vector2F vector, Fixed maxLength)
		{
			if (vector.sqrMagnitude > maxLength * maxLength)
			{
				return vector.normalized * maxLength;
			}
			return vector;
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x001A987D File Offset: 0x001A7C7D
		public static Fixed SqrMagnitude(Vector2F a)
		{
			return a.x * a.x + a.y * a.y;
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001A98AA File Offset: 0x001A7CAA
		public static bool Approximately(Vector2F a, Vector2F b)
		{
			return Vector2F.Approximately(a, b, Fixed.Epsilon);
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x001A98B8 File Offset: 0x001A7CB8
		public static bool Approximately(Vector2F a, Vector2F b, Fixed epsilon)
		{
			return FixedMath.ApproximatelyEqual(a.x, b.x, epsilon) && FixedMath.ApproximatelyEqual(a.y, b.y, epsilon);
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x001A98EC File Offset: 0x001A7CEC
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

		// Token: 0x0600524D RID: 21069 RVA: 0x001A9946 File Offset: 0x001A7D46
		public Fixed SqrMagnitude()
		{
			return this.x * this.x + this.y * this.y;
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001A996F File Offset: 0x001A7D6F
		public static Vector2F Min(Vector2F lhs, Vector2F rhs)
		{
			return new Vector2F(FixedMath.Min(lhs.x, rhs.x), FixedMath.Min(lhs.y, rhs.y));
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x001A999C File Offset: 0x001A7D9C
		public static Vector2F Max(Vector2F lhs, Vector2F rhs)
		{
			return new Vector2F(FixedMath.Max(lhs.x, rhs.x), FixedMath.Max(lhs.y, rhs.y));
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x001A99C9 File Offset: 0x001A7DC9
		public static Vector2F operator +(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x + b.x, a.y + b.y);
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x001A99F6 File Offset: 0x001A7DF6
		public static Vector2F operator -(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x - b.x, a.y - b.y);
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x001A9A23 File Offset: 0x001A7E23
		public static Vector2F operator -(Vector2F a)
		{
			return new Vector2F(-a.x, -a.y);
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x001A9A42 File Offset: 0x001A7E42
		public static Vector2F operator *(Vector2F a, Fixed d)
		{
			return new Vector2F(a.x * d, a.y * d);
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x001A9A63 File Offset: 0x001A7E63
		public static Vector2F operator *(Fixed d, Vector2F a)
		{
			return new Vector2F(a.x * d, a.y * d);
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x001A9A84 File Offset: 0x001A7E84
		public static Vector2F operator *(Vector2F a, Vector2F b)
		{
			return new Vector2F(a.x * b.x, a.y * b.y);
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x001A9AB1 File Offset: 0x001A7EB1
		public static Vector2F operator /(Vector2F a, Fixed d)
		{
			return new Vector2F(a.x / d, a.y / d);
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001A9AD2 File Offset: 0x001A7ED2
		public static bool operator ==(Vector2F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y;
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x001A9B02 File Offset: 0x001A7F02
		public static bool operator !=(Vector2F lhs, Vector2F rhs)
		{
			return !(lhs.x == rhs.x) || !(lhs.y == rhs.y);
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x001A9B38 File Offset: 0x001A7F38
		public static bool operator ==(Vector2F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && rhs.z == 0;
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001A9B88 File Offset: 0x001A7F88
		public static bool operator !=(Vector2F lhs, Vector3F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && rhs.z == 0;
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x001A9BD8 File Offset: 0x001A7FD8
		public static bool operator ==(Vector3F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == 0;
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x001A9C28 File Offset: 0x001A8028
		public static bool operator !=(Vector3F lhs, Vector2F rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == 0;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x001A9C75 File Offset: 0x001A8075
		public static implicit operator Vector2F(Vector3F v)
		{
			return new Vector2F(v.x, v.y);
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001A9C8A File Offset: 0x001A808A
		public static implicit operator Vector3F(Vector2F v)
		{
			return new Vector3F(v.x, v.y, 0);
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x001A9CA5 File Offset: 0x001A80A5
		public static explicit operator Vector3(Vector2F v)
		{
			return new Vector3((float)v.x, (float)v.y, 0f);
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x001A9CC9 File Offset: 0x001A80C9
		public static explicit operator Vector2(Vector2F v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x001A9CE8 File Offset: 0x001A80E8
		public static explicit operator Vector2F(Vector2 v)
		{
			return new Vector2F((Fixed)((double)v.x), (Fixed)((double)v.y));
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x001A9D09 File Offset: 0x001A8109
		public static explicit operator Vector2F(Vector3 v)
		{
			return new Vector2F((Fixed)((double)v.x), (Fixed)((double)v.y));
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x001A9D2A File Offset: 0x001A812A
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.y);
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001A9D47 File Offset: 0x001A8147
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, 0f);
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x001A9D6C File Offset: 0x001A816C
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

		// Token: 0x0400349F RID: 13471
		public Fixed x;

		// Token: 0x040034A0 RID: 13472
		public Fixed y;
	}
}
