using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C1 RID: 193
	public class MKKlassikFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x0001CC84 File Offset: 0x0001B084
		public MKKlassikFightStickMacProfile()
		{
			base.Name = "MK Klassik Fight Stick";
			base.Meta = "MK Klassik Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(771)
				}
			};
		}
	}
}
