using System;

namespace InControl.NativeProfile
{
	// Token: 0x0200009B RID: 155
	public class HoriXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x0001BBA8 File Offset: 0x00019FA8
		public HoriXboxOneControllerMacProfile()
		{
			base.Name = "Hori Xbox One Controller";
			base.Meta = "Hori Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3853),
					ProductID = new ushort?(103)
				}
			};
		}
	}
}
