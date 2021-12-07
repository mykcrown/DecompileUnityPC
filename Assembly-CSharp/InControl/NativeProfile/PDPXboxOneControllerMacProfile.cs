using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CD RID: 205
	public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x0001D2AC File Offset: 0x0001B6AC
		public PDPXboxOneControllerMacProfile()
		{
			base.Name = "PDP Xbox One Controller";
			base.Meta = "PDP Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(314)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(354)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(353)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(355)
				}
			};
		}
	}
}
