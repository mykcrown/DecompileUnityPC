using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000A6 RID: 166
	public class MadCatzArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x0001BFC0 File Offset: 0x0001A3C0
		public MadCatzArcadeStickMacProfile()
		{
			base.Name = "Mad Catz Arcade Stick";
			base.Meta = "Mad Catz Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18264)
				}
			};
		}
	}
}
