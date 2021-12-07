using System;

namespace InControl
{
	// Token: 0x020000F8 RID: 248
	[AutoDiscover]
	public class XTR_G2_MacNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x00020B4C File Offset: 0x0001EF4C
		public XTR_G2_MacNativeProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on OS X";
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
						"KMODEL Simulator - XTR+G2+FMS Controller"
					}
				}
			};
		}
	}
}
