using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000EA RID: 234
	public class XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005C4 RID: 1476 RVA: 0x0001E02C File Offset: 0x0001C42C
		public XboxOneControllerMacProfile()
		{
			base.Name = "Xbox One Controller";
			base.Meta = "Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(22042)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21786)
				}
			};
		}
	}
}
