using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009C RID: 156
	public class IonDrumRockerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x0001BC04 File Offset: 0x0001A004
		public IonDrumRockerMacProfile()
		{
			base.Name = "Ion Drum Rocker";
			base.Meta = "Ion Drum Rocker on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(304)
				}
			};
		}
	}
}
