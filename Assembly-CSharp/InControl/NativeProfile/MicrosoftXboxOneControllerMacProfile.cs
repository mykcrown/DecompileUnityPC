using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000BF RID: 191
	public class MicrosoftXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001CB70 File Offset: 0x0001AF70
		public MicrosoftXboxOneControllerMacProfile()
		{
			base.Name = "Microsoft Xbox One Controller";
			base.Meta = "Microsoft Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(721)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(733)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1118),
					ProductID = new ushort?(746)
				}
			};
		}
	}
}
