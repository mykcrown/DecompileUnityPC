using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009F RID: 159
	public class LogitechChillStreamControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x0001BD20 File Offset: 0x0001A120
		public LogitechChillStreamControllerMacProfile()
		{
			base.Name = "Logitech Chill Stream Controller";
			base.Meta = "Logitech Chill Stream Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1133),
					ProductID = new ushort?(49730)
				}
			};
		}
	}
}
