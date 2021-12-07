using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B8 RID: 184
	public class MadCatzSF4FightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x0001C700 File Offset: 0x0001AB00
		public MadCatzSF4FightStickTEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick TE";
			base.Meta = "Mad Catz SF4 Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18232)
				}
			};
		}
	}
}
