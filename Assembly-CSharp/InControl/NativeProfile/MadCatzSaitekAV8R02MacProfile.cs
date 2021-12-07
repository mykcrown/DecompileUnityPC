using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B5 RID: 181
	public class MadCatzSaitekAV8R02MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0001C5E0 File Offset: 0x0001A9E0
		public MadCatzSaitekAV8R02MacProfile()
		{
			base.Name = "Mad Catz Saitek AV8R02";
			base.Meta = "Mad Catz Saitek AV8R02 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(52009)
				}
			};
		}
	}
}
