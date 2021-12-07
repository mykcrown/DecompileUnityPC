using System;
using UnityEngine;

namespace FixedPoint
{
	// Token: 0x02000B20 RID: 2848
	[Serializable]
	public struct QuaternionF
	{
		// Token: 0x06005208 RID: 21000 RVA: 0x00153C6E File Offset: 0x0015206E
		public QuaternionF(Fixed x, Fixed y, Fixed z, Fixed w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x00153C8D File Offset: 0x0015208D
		// (set) Token: 0x0600520A RID: 21002 RVA: 0x00153CB2 File Offset: 0x001520B2
		public Vector3F eulerAngles
		{
			get
			{
				return QuaternionF.Internal_MakePositive(QuaternionF.Internal_ToEulerRad(this) * (Fixed)57.295780181884766);
			}
			set
			{
				this = QuaternionF.Internal_FromEulerRad(value * (Fixed)0.0174532924);
			}
		}

		// Token: 0x1700131E RID: 4894
		public Fixed this[int index]
		{
			get
			{
				Fixed result;
				switch (index)
				{
				case 0:
					result = this.x;
					break;
				case 1:
					result = this.y;
					break;
				case 2:
					result = this.z;
					break;
				case 3:
					result = this.w;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
				return result;
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
				case 3:
					this.w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Quaternion index!");
				}
			}
		}

		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x0600520D RID: 21005 RVA: 0x00153D9B File Offset: 0x0015219B
		public static QuaternionF identity
		{
			get
			{
				return new QuaternionF(0, 0, 0, 1);
			}
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x00153DBC File Offset: 0x001521BC
		public static QuaternionF Inverse(QuaternionF quaternion)
		{
			QuaternionF result;
			QuaternionF.Inverse(quaternion, out result);
			return result;
		}

		// Token: 0x0600520F RID: 21007 RVA: 0x00153DD4 File Offset: 0x001521D4
		public static void Inverse(QuaternionF quaternion, out QuaternionF result)
		{
			result.x = -quaternion.x;
			result.y = -quaternion.y;
			result.z = -quaternion.z;
			result.w = quaternion.w;
		}

		// Token: 0x06005210 RID: 21008 RVA: 0x00153E24 File Offset: 0x00152224
		public static QuaternionF Euler(Fixed x, Fixed y, Fixed z)
		{
			return QuaternionF.Internal_FromEulerRad(new Vector3F(x, y, z) * (Fixed)0.0174532924);
		}

		// Token: 0x06005211 RID: 21009 RVA: 0x00153E46 File Offset: 0x00152246
		public static QuaternionF Euler(Vector3F euler)
		{
			return QuaternionF.Internal_FromEulerRad(euler * (Fixed)0.0174532924);
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x00153E64 File Offset: 0x00152264
		private static Vector3F Internal_ToEulerRad(QuaternionF rotation)
		{
			Fixed other = rotation.w * rotation.w;
			Fixed one = rotation.x * rotation.x;
			Fixed other2 = rotation.y * rotation.y;
			Fixed other3 = rotation.z * rotation.z;
			Fixed one2 = one + other2 + other3 + other;
			Fixed one3 = rotation.x * rotation.w - rotation.y * rotation.z;
			Vector3F result;
			if (one3 > 0.4995f * one2)
			{
				result.y = 2f * FixedMath.Atan2(rotation.y, rotation.x);
				result.x = FixedMath.PI / 2;
				result.z = 0;
				return result;
			}
			if (one3 < -0.4995f * one2)
			{
				result.y = -2f * FixedMath.Atan2(rotation.y, rotation.x);
				result.x = -FixedMath.PI / 2;
				result.z = 0;
				return result;
			}
			QuaternionF quaternionF = new QuaternionF(rotation.w, rotation.z, rotation.x, rotation.y);
			result.y = FixedMath.Atan2(2 * quaternionF.x * quaternionF.w + 2 * quaternionF.y * quaternionF.z, 1 - 2 * (quaternionF.z * quaternionF.z + quaternionF.w * quaternionF.w));
			result.x = FixedMath.Asin(2 * (quaternionF.x * quaternionF.z - quaternionF.w * quaternionF.y));
			result.z = FixedMath.Atan2(2 * quaternionF.x * quaternionF.y + 2 * quaternionF.z * quaternionF.w, 1 - 2 * (quaternionF.y * quaternionF.y + quaternionF.z * quaternionF.z));
			return result;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x0015411C File Offset: 0x0015251C
		private static QuaternionF Internal_FromEulerRad(Vector3F euler)
		{
			Fixed other = (Fixed)0.5;
			Fixed one = FixedMath.Cos(euler.x * other);
			Fixed one2 = FixedMath.Sin(euler.x * other);
			Fixed other2 = FixedMath.Cos(euler.y * other);
			Fixed other3 = FixedMath.Sin(euler.y * other);
			Fixed other4 = FixedMath.Cos(euler.z * other);
			Fixed other5 = FixedMath.Sin(euler.z * other);
			QuaternionF result;
			result.w = one * other2 * other4 + one2 * other3 * other5;
			result.x = one2 * other2 * other4 + one * other3 * other5;
			result.y = one * other3 * other4 - one2 * other2 * other5;
			result.z = one * other2 * other5 - one2 * other3 * other4;
			return result;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x00154257 File Offset: 0x00152657
		public void Set(Fixed new_x, Fixed new_y, Fixed new_z, Fixed new_w)
		{
			this.x = new_x;
			this.y = new_y;
			this.z = new_z;
			this.w = new_w;
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x00154278 File Offset: 0x00152678
		public static QuaternionF operator *(QuaternionF lhs, QuaternionF rhs)
		{
			return new QuaternionF(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001543F8 File Offset: 0x001527F8
		public static Vector3F operator *(QuaternionF rotation, Vector3F point)
		{
			Fixed other = rotation.x * 2;
			Fixed other2 = rotation.y * 2;
			Fixed other3 = rotation.z * 2;
			Fixed one = rotation.x * other;
			Fixed @fixed = rotation.y * other2;
			Fixed other4 = rotation.z * other3;
			Fixed one2 = rotation.x * other2;
			Fixed one3 = rotation.x * other3;
			Fixed one4 = rotation.y * other3;
			Fixed other5 = rotation.w * other;
			Fixed other6 = rotation.w * other2;
			Fixed other7 = rotation.w * other3;
			Vector3F result;
			result.x = (1 - (@fixed + other4)) * point.x + (one2 - other7) * point.y + (one3 + other6) * point.z;
			result.y = (one2 + other7) * point.x + (1 - (one + other4)) * point.y + (one4 - other5) * point.z;
			result.z = (one3 - other6) * point.x + (one4 + other5) * point.y + (1 - (one + @fixed)) * point.z;
			return result;
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x001545B8 File Offset: 0x001529B8
		public static bool operator ==(QuaternionF lhs, QuaternionF rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x00154624 File Offset: 0x00152A24
		public static bool operator !=(QuaternionF lhs, QuaternionF rhs)
		{
			return !(lhs.x == rhs.x) || !(lhs.y == rhs.y) || !(lhs.z == rhs.z) || !(lhs.w == rhs.w);
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x00154694 File Offset: 0x00152A94
		public static Fixed Dot(QuaternionF a, QuaternionF b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001546FC File Offset: 0x00152AFC
		public static Fixed Angle(QuaternionF a, QuaternionF b)
		{
			Fixed f = QuaternionF.Dot(a, b);
			return FixedMath.Acos(FixedMath.Min(FixedMath.Abs(f), 1)) * 2 * (Fixed)57.29578;
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x00154740 File Offset: 0x00152B40
		private static Vector3F Internal_MakePositive(Vector3F euler)
		{
			Fixed @fixed = -(Fixed)0.005729578;
			Fixed other = 360 + @fixed;
			if (euler.x < @fixed)
			{
				euler.x += 360;
			}
			else if (euler.x > other)
			{
				euler.x -= 360;
			}
			if (euler.y < @fixed)
			{
				euler.y += 360;
			}
			else if (euler.y > other)
			{
				euler.y -= 360;
			}
			if (euler.z < @fixed)
			{
				euler.z += 360;
			}
			else if (euler.z > other)
			{
				euler.z -= 360;
			}
			return euler;
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x00154874 File Offset: 0x00152C74
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2 ^ this.w.GetHashCode() >> 1;
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001548D0 File Offset: 0x00152CD0
		public override bool Equals(object other)
		{
			bool result;
			if (!(other is QuaternionF))
			{
				result = false;
			}
			else
			{
				QuaternionF quaternionF = (QuaternionF)other;
				result = (this.x.Equals(quaternionF.x) && this.y.Equals(quaternionF.y) && this.z.Equals(quaternionF.z) && this.w.Equals(quaternionF.w));
			}
			return result;
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x00154980 File Offset: 0x00152D80
		public override string ToString()
		{
			return string.Format("({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.w
			});
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x001549D8 File Offset: 0x00152DD8
		public string ToString(string format)
		{
			return string.Format("({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString(format),
				this.y.ToString(format),
				this.z.ToString(format),
				this.w.ToString(format)
			});
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x00154A31 File Offset: 0x00152E31
		public static explicit operator QuaternionF(Quaternion v)
		{
			return new QuaternionF((Fixed)((double)v.x), (Fixed)((double)v.y), (Fixed)((double)v.z), (Fixed)((double)v.w));
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x00154A6C File Offset: 0x00152E6C
		public static explicit operator Quaternion(QuaternionF v)
		{
			return new Quaternion((float)v.x, (float)v.y, (float)v.z, (float)v.w);
		}

		// Token: 0x04003499 RID: 13465
		public Fixed x;

		// Token: 0x0400349A RID: 13466
		public Fixed y;

		// Token: 0x0400349B RID: 13467
		public Fixed z;

		// Token: 0x0400349C RID: 13468
		public Fixed w;
	}
}
