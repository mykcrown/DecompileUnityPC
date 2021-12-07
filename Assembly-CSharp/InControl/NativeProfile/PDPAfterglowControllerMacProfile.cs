using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C5 RID: 197
	public class PDPAfterglowControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x0001CE30 File Offset: 0x0001B230
		public PDPAfterglowControllerMacProfile()
		{
			base.Name = "PDP Afterglow Controller";
			base.Meta = "PDP Afterglow Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(1043)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64252)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63751)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(64253)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(742)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63744)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(275)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(63744)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(531)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(4779),
					ProductID = new ushort?(769)
				}
			};
		}
	}
}
