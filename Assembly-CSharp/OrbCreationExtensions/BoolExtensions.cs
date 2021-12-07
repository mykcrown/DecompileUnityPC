using System;

namespace OrbCreationExtensions
{
	// Token: 0x02000016 RID: 22
	public static class BoolExtensions
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00008130 File Offset: 0x00006530
		public static string MakeString(this bool aBool)
		{
			if (aBool)
			{
				return "1";
			}
			return "0";
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008143 File Offset: 0x00006543
		public static int MakeInt(this bool aBool)
		{
			if (aBool)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000814E File Offset: 0x0000654E
		public static bool MakeBool(this bool aBool)
		{
			return aBool;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00008151 File Offset: 0x00006551
		public static float MakeFloat(this bool aBool)
		{
			if (aBool)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00008164 File Offset: 0x00006564
		public static double MakeDouble(this bool aBool)
		{
			if (aBool)
			{
				return 1.0;
			}
			return 0.0;
		}
	}
}
