using System;

namespace InControl
{
	// Token: 0x02000119 RID: 281
	public enum NativeDeviceDriverType : ushort
	{
		// Token: 0x04000467 RID: 1127
		Unknown,
		// Token: 0x04000468 RID: 1128
		HID,
		// Token: 0x04000469 RID: 1129
		USB,
		// Token: 0x0400046A RID: 1130
		Bluetooth,
		// Token: 0x0400046B RID: 1131
		XInput,
		// Token: 0x0400046C RID: 1132
		DirectInput,
		// Token: 0x0400046D RID: 1133
		RawInput
	}
}
