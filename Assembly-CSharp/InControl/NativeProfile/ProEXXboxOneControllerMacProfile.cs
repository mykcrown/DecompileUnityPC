using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D5 RID: 213
	public class ProEXXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x0001D6A8 File Offset: 0x0001BAA8
		public ProEXXboxOneControllerMacProfile()
		{
			base.Name = "Pro EX Xbox One Controller";
			base.Meta = "Pro EX Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21562)
				}
			};
		}
	}
}
