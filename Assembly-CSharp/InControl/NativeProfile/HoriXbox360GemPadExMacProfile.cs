using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009A RID: 154
	public class HoriXbox360GemPadExMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x0001BB48 File Offset: 0x00019F48
		public HoriXbox360GemPadExMacProfile()
		{
			base.Name = "Hori Xbox 360 Gem Pad Ex";
			base.Meta = "Hori Xbox 360 Gem Pad Ex on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21773)
				}
			};
		}
	}
}
