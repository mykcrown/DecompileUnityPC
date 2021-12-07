// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace FixedPoint
{
	public struct FPoint
	{
		public Fixed X;

		public Fixed Y;

		public static FPoint Create(Fixed X, Fixed Y)
		{
			FPoint result;
			result.X = X;
			result.Y = Y;
			return result;
		}

		public static FPoint VectorAdd(FPoint F1, FPoint F2)
		{
			FPoint result;
			result.X = F1.X + F2.X;
			result.Y = F1.Y + F2.Y;
			return result;
		}

		public static FPoint VectorSubtract(FPoint F1, FPoint F2)
		{
			FPoint result;
			result.X = F1.X - F2.X;
			result.Y = F1.Y - F2.Y;
			return result;
		}

		public static FPoint VectorDivide(FPoint F1, int Divisor)
		{
			FPoint result;
			result.X = F1.X / Divisor;
			result.Y = F1.Y / Divisor;
			return result;
		}
	}
}
