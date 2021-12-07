using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000092 RID: 146
	public class HoriPadUltimateMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x0001B82C File Offset: 0x00019C2C
		public HoriPadUltimateMacProfile()
		{
			base.Name = "HoriPad Ultimate";
			base.Meta = "HoriPad Ultimate on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(144)
				}
			};
		}
	}
}
