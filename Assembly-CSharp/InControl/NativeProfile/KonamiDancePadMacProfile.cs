using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009E RID: 158
	public class KonamiDancePadMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0001BCC4 File Offset: 0x0001A0C4
		public KonamiDancePadMacProfile()
		{
			base.Name = "Konami Dance Pad";
			base.Meta = "Konami Dance Pad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(4)
				}
			};
		}
	}
}
