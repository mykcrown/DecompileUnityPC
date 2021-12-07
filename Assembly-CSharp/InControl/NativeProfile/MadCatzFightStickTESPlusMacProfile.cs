using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AD RID: 173
	public class MadCatzFightStickTESPlusMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x0001C2E0 File Offset: 0x0001A6E0
		public MadCatzFightStickTESPlusMacProfile()
		{
			base.Name = "Mad Catz Fight Stick TES Plus";
			base.Meta = "Mad Catz Fight Stick TES Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61506)
				}
			};
		}
	}
}
