using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BA RID: 186
	public class MadCatzSSF4ChunLiFightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000594 RID: 1428 RVA: 0x0001C7C0 File Offset: 0x0001ABC0
		public MadCatzSSF4ChunLiFightStickTEMacProfile()
		{
			base.Name = "Mad Catz SSF4 Chun-Li Fight Stick TE";
			base.Meta = "Mad Catz SSF4 Chun-Li Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61501)
				}
			};
		}
	}
}
