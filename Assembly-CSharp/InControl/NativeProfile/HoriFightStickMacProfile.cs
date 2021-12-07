using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000090 RID: 144
	public class HoriFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x0001B774 File Offset: 0x00019B74
		public HoriFightStickMacProfile()
		{
			base.Name = "Hori Fight Stick";
			base.Meta = "Hori Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(13)
				}
			};
		}
	}
}
