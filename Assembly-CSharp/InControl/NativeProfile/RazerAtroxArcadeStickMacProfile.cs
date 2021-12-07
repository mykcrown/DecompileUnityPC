using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D7 RID: 215
	public class RazerAtroxArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B1 RID: 1457 RVA: 0x0001D768 File Offset: 0x0001BB68
		public RazerAtroxArcadeStickMacProfile()
		{
			base.Name = "Razer Atrox Arcade Stick";
			base.Meta = "Razer Atrox Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5426),
					ProductID = new ushort?(2560)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(20480)
				}
			};
		}
	}
}
