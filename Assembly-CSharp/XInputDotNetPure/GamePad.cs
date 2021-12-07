using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x020001EB RID: 491
	public class GamePad
	{
		// Token: 0x060008CA RID: 2250 RVA: 0x0004CF2C File Offset: 0x0004B32C
		public static GamePadState GetState(PlayerIndex playerIndex)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GamePadState.RawState)));
			uint num = Imports.XInputGamePadGetState((uint)playerIndex, intPtr);
			GamePadState.RawState rawState = (GamePadState.RawState)Marshal.PtrToStructure(intPtr, typeof(GamePadState.RawState));
			return new GamePadState(num == 0U, rawState);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0004CF76 File Offset: 0x0004B376
		public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
		{
			Imports.XInputGamePadSetState((uint)playerIndex, leftMotor, rightMotor);
		}
	}
}
