using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000096 RID: 150
	public class HoriRealArcadeProHayabusaMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x0001B9A8 File Offset: 0x00019DA8
		public HoriRealArcadeProHayabusaMacProfile()
		{
			base.Name = "Hori Real Arcade Pro Hayabusa";
			base.Meta = "Hori Real Arcade Pro Hayabusa on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(99)
				}
			};
		}
	}
}
