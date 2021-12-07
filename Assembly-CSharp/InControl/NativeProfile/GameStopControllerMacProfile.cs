using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000088 RID: 136
	public class GameStopControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001B300 File Offset: 0x00019700
		public GameStopControllerMacProfile()
		{
			base.Name = "GameStop Controller";
			base.Meta = "GameStop Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1025)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(769)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(770)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63745)
				}
			};
		}
	}
}
