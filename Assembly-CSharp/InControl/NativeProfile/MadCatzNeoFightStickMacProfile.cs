using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B2 RID: 178
	public class MadCatzNeoFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600058C RID: 1420 RVA: 0x0001C4C0 File Offset: 0x0001A8C0
		public MadCatzNeoFightStickMacProfile()
		{
			base.Name = "Mad Catz Neo Fight Stick";
			base.Meta = "Mad Catz Neo Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61498)
				}
			};
		}
	}
}
