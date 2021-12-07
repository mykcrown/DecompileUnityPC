using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D1 RID: 209
	public class PowerAMiniProExControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x0001D4D4 File Offset: 0x0001B8D4
		public PowerAMiniProExControllerMacProfile()
		{
			base.Name = "PowerA Mini Pro Ex Controller";
			base.Meta = "PowerA Mini Pro Ex Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16128)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21274)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21248)
				}
			};
		}
	}
}
