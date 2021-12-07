using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AB RID: 171
	public class MadCatzFightPadMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x0001C220 File Offset: 0x0001A620
		public MadCatzFightPadMacProfile()
		{
			base.Name = "Mad Catz FightPad";
			base.Meta = "Mad Catz FightPad on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61486)
				}
			};
		}
	}
}
