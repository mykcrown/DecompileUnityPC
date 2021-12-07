﻿using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009D RID: 157
	public class JoytekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0001BC64 File Offset: 0x0001A064
		public JoytekXbox360ControllerMacProfile()
		{
			base.Name = "Joytek Xbox 360 Controller";
			base.Meta = "Joytek Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5678),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
