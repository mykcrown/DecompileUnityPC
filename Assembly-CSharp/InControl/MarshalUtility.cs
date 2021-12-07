using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x020001D4 RID: 468
	public static class MarshalUtility
	{
		// Token: 0x0600081F RID: 2079 RVA: 0x0004AB10 File Offset: 0x00048F10
		public static void Copy(IntPtr source, uint[] destination, int length)
		{
			Utility.ArrayExpand<int>(ref MarshalUtility.buffer, length);
			Marshal.Copy(source, MarshalUtility.buffer, 0, length);
			Buffer.BlockCopy(MarshalUtility.buffer, 0, destination, 0, 4 * length);
		}

		// Token: 0x040005DC RID: 1500
		private static int[] buffer = new int[32];
	}
}
