using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C6 RID: 198
	public class PDPBattlefieldXBoxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x0001D00C File Offset: 0x0001B40C
		public PDPBattlefieldXBoxOneControllerMacProfile()
		{
			base.Name = "PDP Battlefield XBox One Controller";
			base.Meta = "PDP Battlefield XBox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(356)
				}
			};
		}
	}
}
