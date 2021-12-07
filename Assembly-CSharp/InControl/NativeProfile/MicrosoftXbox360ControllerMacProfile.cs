using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BD RID: 189
	public class MicrosoftXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x0001C8E0 File Offset: 0x0001ACE0
		public MicrosoftXbox360ControllerMacProfile()
		{
			base.Name = "Microsoft Xbox 360 Controller";
			base.Meta = "Microsoft Xbox 360 Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(654)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(655)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(307)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(63233)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(672)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(672)
				}
			};
		}
	}
}
