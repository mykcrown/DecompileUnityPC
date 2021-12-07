using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B7 RID: 183
	public class MadCatzSF4FightStickSEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x0001C6A0 File Offset: 0x0001AAA0
		public MadCatzSF4FightStickSEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick SE";
			base.Meta = "Mad Catz SF4 Fight Stick SE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(18200)
				}
			};
		}
	}
}
