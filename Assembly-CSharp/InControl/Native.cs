using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x02000118 RID: 280
	internal static class Native
	{
		// Token: 0x060005F2 RID: 1522
		[DllImport("InControlNative", EntryPoint = "InControl_Init")]
		public static extern void Init(NativeInputOptions options);

		// Token: 0x060005F3 RID: 1523
		[DllImport("InControlNative", EntryPoint = "InControl_Stop")]
		public static extern void Stop();

		// Token: 0x060005F4 RID: 1524
		[DllImport("InControlNative", EntryPoint = "InControl_GetVersionInfo")]
		public static extern void GetVersionInfo(out NativeVersionInfo versionInfo);

		// Token: 0x060005F5 RID: 1525
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceInfo")]
		public static extern bool GetDeviceInfo(uint handle, out NativeDeviceInfo deviceInfo);

		// Token: 0x060005F6 RID: 1526
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceState")]
		public static extern bool GetDeviceState(uint handle, out IntPtr deviceState);

		// Token: 0x060005F7 RID: 1527
		[DllImport("InControlNative", EntryPoint = "InControl_GetDeviceEvents")]
		public static extern int GetDeviceEvents(out IntPtr deviceEvents);

		// Token: 0x060005F8 RID: 1528
		[DllImport("InControlNative", EntryPoint = "InControl_SetHapticState")]
		public static extern void SetHapticState(uint handle, byte motor0, byte motor1);

		// Token: 0x060005F9 RID: 1529
		[DllImport("InControlNative", EntryPoint = "InControl_SetLightColor")]
		public static extern void SetLightColor(uint handle, byte red, byte green, byte blue);

		// Token: 0x060005FA RID: 1530
		[DllImport("InControlNative", EntryPoint = "InControl_SetLightFlash")]
		public static extern void SetLightFlash(uint handle, byte flashOnDuration, byte flashOffDuration);

		// Token: 0x04000465 RID: 1125
		private const string LibraryName = "InControlNative";
	}
}
