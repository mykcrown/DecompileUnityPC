using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B3 RID: 179
	public class MadCatzPortableDrumMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0001C520 File Offset: 0x0001A920
		public MadCatzPortableDrumMacProfile()
		{
			base.Name = "Mad Catz Portable Drum";
			base.Meta = "Mad Catz Portable Drum on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(39025)
				}
			};
		}
	}
}
