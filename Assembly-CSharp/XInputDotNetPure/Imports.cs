using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x020001DF RID: 479
	internal class Imports
	{
		// Token: 0x060008A2 RID: 2210
		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState32(uint playerIndex, IntPtr state);

		// Token: 0x060008A3 RID: 2211
		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState32(uint playerIndex, float leftMotor, float rightMotor);

		// Token: 0x060008A4 RID: 2212
		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState64(uint playerIndex, IntPtr state);

		// Token: 0x060008A5 RID: 2213
		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState64(uint playerIndex, float leftMotor, float rightMotor);

		// Token: 0x060008A6 RID: 2214 RVA: 0x0004CA98 File Offset: 0x0004AE98
		public static uint XInputGamePadGetState(uint playerIndex, IntPtr state)
		{
			if (IntPtr.Size == 4)
			{
				return Imports.XInputGamePadGetState32(playerIndex, state);
			}
			return Imports.XInputGamePadGetState64(playerIndex, state);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0004CAB4 File Offset: 0x0004AEB4
		public static void XInputGamePadSetState(uint playerIndex, float leftMotor, float rightMotor)
		{
			if (IntPtr.Size == 4)
			{
				Imports.XInputGamePadSetState32(playerIndex, leftMotor, rightMotor);
			}
			else
			{
				Imports.XInputGamePadSetState64(playerIndex, leftMotor, rightMotor);
			}
		}
	}
}
