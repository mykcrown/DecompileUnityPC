using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200008A RID: 138
	public class HoriBlueSoloControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000564 RID: 1380 RVA: 0x0001B440 File Offset: 0x00019840
		public HoriBlueSoloControllerMacProfile()
		{
			base.Name = "Hori Blue Solo Controller ";
			base.Meta = "Hori Blue Solo Controller\ton Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(64001)
				}
			};
		}
	}
}
