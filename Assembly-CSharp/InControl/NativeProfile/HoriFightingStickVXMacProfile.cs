using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008F RID: 143
	public class HoriFightingStickVXMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0001B6E8 File Offset: 0x00019AE8
		public HoriFightingStickVXMacProfile()
		{
			base.Name = "Hori Fighting Stick VX";
			base.Meta = "Hori Fighting Stick VX on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62723)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21762)
				}
			};
		}
	}
}
