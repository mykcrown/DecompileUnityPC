using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008E RID: 142
	public class HoriFightingStickEX2MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x0001B638 File Offset: 0x00019A38
		public HoriFightingStickEX2MacProfile()
		{
			base.Name = "Hori Fighting Stick EX2";
			base.Meta = "Hori Fighting Stick EX2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(10)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(62725)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
