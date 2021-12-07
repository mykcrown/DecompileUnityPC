using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000083 RID: 131
	public class APlayControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600055D RID: 1373 RVA: 0x0001B120 File Offset: 0x00019520
		public APlayControllerMacProfile()
		{
			base.Name = "A Play Controller";
			base.Meta = "A Play Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64251)
				}
			};
		}
	}
}
