using System;

namespace InControl
{
	// Token: 0x020000F9 RID: 249
	[AutoDiscover]
	public class XTR55_G2_MacNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005D3 RID: 1491 RVA: 0x00020BDC File Offset: 0x0001EFDC
		public XTR55_G2_MacNativeProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2971),
					ProductID = new ushort?(16402),
					NameLiterals = new string[]
					{
						"SAILI Simulator --- XTR5.5+G2+FMS Controller"
					}
				}
			};
		}
	}
}
