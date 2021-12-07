using System;
using System.Runtime.InteropServices;

namespace InControl
{
	// Token: 0x0200011A RID: 282
	public struct NativeDeviceInfo
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x0002A376 File Offset: 0x00028776
		public bool HasSameVendorID(NativeDeviceInfo deviceInfo)
		{
			return this.vendorID == deviceInfo.vendorID;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002A387 File Offset: 0x00028787
		public bool HasSameProductID(NativeDeviceInfo deviceInfo)
		{
			return this.productID == deviceInfo.productID;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002A398 File Offset: 0x00028798
		public bool HasSameVersionNumber(NativeDeviceInfo deviceInfo)
		{
			return this.versionNumber == deviceInfo.versionNumber;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0002A3A9 File Offset: 0x000287A9
		public bool HasSameLocation(NativeDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.location) && this.location == deviceInfo.location;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0002A3CF File Offset: 0x000287CF
		public bool HasSameSerialNumber(NativeDeviceInfo deviceInfo)
		{
			return !string.IsNullOrEmpty(this.serialNumber) && this.serialNumber == deviceInfo.serialNumber;
		}

		// Token: 0x0400046E RID: 1134
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string name;

		// Token: 0x0400046F RID: 1135
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string location;

		// Token: 0x04000470 RID: 1136
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string serialNumber;

		// Token: 0x04000471 RID: 1137
		public ushort vendorID;

		// Token: 0x04000472 RID: 1138
		public ushort productID;

		// Token: 0x04000473 RID: 1139
		public uint versionNumber;

		// Token: 0x04000474 RID: 1140
		public NativeDeviceDriverType driverType;

		// Token: 0x04000475 RID: 1141
		public NativeDeviceTransportType transportType;

		// Token: 0x04000476 RID: 1142
		public uint numButtons;

		// Token: 0x04000477 RID: 1143
		public uint numAnalogs;
	}
}
