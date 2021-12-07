using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A9 RID: 169
	public class MadCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001C0E0 File Offset: 0x0001A4E0
		public MadCatzControllerMacProfile()
		{
			base.Name = "Mad Catz Controller";
			base.Meta = "Mad Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18198)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63746)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61642)
				}
			};
		}
	}
}
