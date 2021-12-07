using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B1 RID: 177
	public class MadCatzMLGFightStickTEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x0001C460 File Offset: 0x0001A860
		public MadCatzMLGFightStickTEMacProfile()
		{
			base.Name = "Mad Catz MLG Fight Stick TE";
			base.Meta = "Mad Catz MLG Fight Stick TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61502)
				}
			};
		}
	}
}
