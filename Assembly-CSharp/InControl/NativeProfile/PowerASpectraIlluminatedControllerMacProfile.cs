using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D3 RID: 211
	public class PowerASpectraIlluminatedControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0001D5E8 File Offset: 0x0001B9E8
		public PowerASpectraIlluminatedControllerMacProfile()
		{
			base.Name = "PowerA Spectra Illuminated Controller";
			base.Meta = "PowerA Spectra Illuminated Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21546)
				}
			};
		}
	}
}
