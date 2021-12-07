using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BE RID: 190
	public class MicrosoftXboxControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x0001CA14 File Offset: 0x0001AE14
		public MicrosoftXboxControllerMacProfile()
		{
			base.Name = "Microsoft Xbox Controller";
			base.Meta = "Microsoft Xbox Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(ushort.MaxValue),
					ProductID = new ushort?(ushort.MaxValue)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(649)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(645)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(514)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(647)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(648)
				}
			};
		}
	}
}
