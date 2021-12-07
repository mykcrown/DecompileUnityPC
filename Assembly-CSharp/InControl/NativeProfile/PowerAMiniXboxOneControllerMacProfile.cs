using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D2 RID: 210
	public class PowerAMiniXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005AC RID: 1452 RVA: 0x0001D588 File Offset: 0x0001B988
		public PowerAMiniXboxOneControllerMacProfile()
		{
			base.Name = "Power A Mini Xbox One Controller";
			base.Meta = "Power A Mini Xbox One Controller on Mac";
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
