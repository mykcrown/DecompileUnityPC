using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000C0 RID: 192
	public class MicrosoftXboxOneEliteControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0001CC24 File Offset: 0x0001B024
		public MicrosoftXboxOneEliteControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Elite Controller";
			base.Meta = "Microsoft Xbox One Elite Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(739)
				}
			};
		}
	}
}
