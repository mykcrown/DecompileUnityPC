// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	internal class Imports
	{
		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState32(uint playerIndex, IntPtr state);

		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState32(uint playerIndex, float leftMotor, float rightMotor);

		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState64(uint playerIndex, IntPtr state);

		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState64(uint playerIndex, float leftMotor, float rightMotor);

		public static uint XInputGamePadGetState(uint playerIndex, IntPtr state)
		{
			if (IntPtr.Size == 4)
			{
				return Imports.XInputGamePadGetState32(playerIndex, state);
			}
			return Imports.XInputGamePadGetState64(playerIndex, state);
		}

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
