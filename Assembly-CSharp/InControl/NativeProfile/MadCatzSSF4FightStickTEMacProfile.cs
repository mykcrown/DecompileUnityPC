using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BB RID: 187
	public class MadCatzSSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x0001C820 File Offset: 0x0001AC20
		public MadCatzSSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(63288)
				}
			};
		}
	}
}
