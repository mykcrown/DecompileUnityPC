using System;

namespace FixedPoint
{
	// Token: 0x02000B1D RID: 2845
	public struct FPoint
	{
		// Token: 0x060051C5 RID: 20933 RVA: 0x00152D9C File Offset: 0x0015119C
		public static FPoint Create(Fixed X, Fixed Y)
		{
			FPoint result;
			result.X = X;
			result.Y = Y;
			return result;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00152DBC File Offset: 0x001511BC
		public static FPoint VectorAdd(FPoint F1, FPoint F2)
		{
			FPoint result;
			result.X = F1.X + F2.X;
			result.Y = F1.Y + F2.Y;
			return result;
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00152E00 File Offset: 0x00151200
		public static FPoint VectorSubtract(FPoint F1, FPoint F2)
		{
			FPoint result;
			result.X = F1.X - F2.X;
			result.Y = F1.Y - F2.Y;
			return result;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x00152E44 File Offset: 0x00151244
		public static FPoint VectorDivide(FPoint F1, int Divisor)
		{
			FPoint result;
			result.X = F1.X / Divisor;
			result.Y = F1.Y / Divisor;
			return result;
		}

		// Token: 0x04003488 RID: 13448
		public Fixed X;

		// Token: 0x04003489 RID: 13449
		public Fixed Y;
	}
}
