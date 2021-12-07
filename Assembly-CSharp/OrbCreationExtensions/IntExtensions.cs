using System;

namespace OrbCreationExtensions
{
	// Token: 0x02000015 RID: 21
	public static class IntExtensions
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00008068 File Offset: 0x00006468
		public static string MakeString(this int[] anArray)
		{
			string text = string.Empty;
			if (anArray != null)
			{
				if (anArray.Length > 0)
				{
					text += anArray[0];
				}
				for (int i = 1; i < anArray.Length; i++)
				{
					if (anArray.Length <= 0)
					{
						return text;
					}
					text = text + "," + anArray[i];
				}
			}
			return text;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000080D0 File Offset: 0x000064D0
		public static bool ContentsEqualTo(this int[] anArray, int[] array2)
		{
			for (int i = 0; i < anArray.Length; i++)
			{
				if (array2.Length <= i || anArray[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00008108 File Offset: 0x00006508
		public static string MakeString(this int anInt)
		{
			return string.Empty + anInt;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000811A File Offset: 0x0000651A
		public static int MakeInt(this int anInt)
		{
			return anInt;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000811D File Offset: 0x0000651D
		public static bool MakeBool(this int anInt)
		{
			return (float)anInt > 0f;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00008128 File Offset: 0x00006528
		public static float MakeFloat(this int anInt)
		{
			return (float)anInt;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000812C File Offset: 0x0000652C
		public static double MakeDouble(this int anInt)
		{
			return (double)anInt;
		}
	}
}
