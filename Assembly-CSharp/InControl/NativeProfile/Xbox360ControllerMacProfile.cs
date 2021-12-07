using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E8 RID: 232
	public class Xbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x0001DF6C File Offset: 0x0001C36C
		public Xbox360ControllerMacProfile()
		{
			base.Name = "Xbox 360 Controller";
			base.Meta = "Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(62721)
				}
			};
		}
	}
}
