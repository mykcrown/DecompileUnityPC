using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AA RID: 170
	public class MadCatzFightPadControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0001C194 File Offset: 0x0001A594
		public MadCatzFightPadControllerMacProfile()
		{
			base.Name = "Mad Catz FightPad Controller";
			base.Meta = "Mad Catz FightPad Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61480)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18216)
				}
			};
		}
	}
}
