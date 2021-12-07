using System;

namespace InControl
{
	// Token: 0x02000116 RID: 278
	[AutoDiscover]
	public class XTR_G2_WindowsNativeProfile : NativeInputDeviceProfile
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x0002A258 File Offset: 0x00028658
		public XTR_G2_WindowsNativeProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"Windows"
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
