using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008C RID: 140
	public class HoriDOA4ArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x0001B52C File Offset: 0x0001992C
		public HoriDOA4ArcadeStickMacProfile()
		{
			base.Name = "Hori DOA4 Arcade Stick";
			base.Meta = "Hori DOA4 Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				}
			};
		}
	}
}
