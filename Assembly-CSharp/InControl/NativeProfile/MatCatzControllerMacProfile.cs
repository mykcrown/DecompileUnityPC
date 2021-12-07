using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BC RID: 188
	public class MatCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0001C880 File Offset: 0x0001AC80
		public MatCatzControllerMacProfile()
		{
			base.Name = "Mat Catz Controller";
			base.Meta = "Mat Catz Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61462)
				}
			};
		}
	}
}
