using System;
using System.Text.RegularExpressions;

namespace InControl
{
	// Token: 0x0200011E RID: 286
	public class NativeInputDeviceMatcher
	{
		// Token: 0x06000626 RID: 1574 RVA: 0x0002B264 File Offset: 0x00029664
		internal bool Matches(NativeDeviceInfo deviceInfo)
		{
			bool result = false;
			if (this.VendorID != null)
			{
				if (this.VendorID.Value != deviceInfo.vendorID)
				{
					return false;
				}
				result = true;
			}
			if (this.ProductID != null)
			{
				if (this.ProductID.Value != deviceInfo.productID)
				{
					return false;
				}
				result = true;
			}
			if (this.VersionNumber != null)
			{
				if (this.VersionNumber.Value != deviceInfo.versionNumber)
				{
					return false;
				}
				result = true;
			}
			if (this.DriverType != null)
			{
				if (this.DriverType.Value != deviceInfo.driverType)
				{
					return false;
				}
				result = true;
			}
			if (this.TransportType != null)
			{
				if (this.TransportType.Value != deviceInfo.transportType)
				{
					return false;
				}
				result = true;
			}
			if (this.NameLiterals != null && this.NameLiterals.Length > 0)
			{
				int num = this.NameLiterals.Length;
				for (int i = 0; i < num; i++)
				{
					if (string.Equals(deviceInfo.name, this.NameLiterals[i], StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
				return false;
			}
			if (this.NamePatterns != null && this.NamePatterns.Length > 0)
			{
				int num2 = this.NamePatterns.Length;
				for (int j = 0; j < num2; j++)
				{
					if (Regex.IsMatch(deviceInfo.name, this.NamePatterns[j], RegexOptions.IgnoreCase))
					{
						return true;
					}
				}
				return false;
			}
			return result;
		}

		// Token: 0x0400048C RID: 1164
		public ushort? VendorID;

		// Token: 0x0400048D RID: 1165
		public ushort? ProductID;

		// Token: 0x0400048E RID: 1166
		public uint? VersionNumber;

		// Token: 0x0400048F RID: 1167
		public NativeDeviceDriverType? DriverType;

		// Token: 0x04000490 RID: 1168
		public NativeDeviceTransportType? TransportType;

		// Token: 0x04000491 RID: 1169
		public string[] NameLiterals;

		// Token: 0x04000492 RID: 1170
		public string[] NamePatterns;
	}
}
