using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E2 RID: 226
	public class RockCandyXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005BC RID: 1468 RVA: 0x0001DC84 File Offset: 0x0001C084
		public RockCandyXboxOneControllerMacProfile()
		{
			base.Name = "Rock Candy Xbox One Controller";
			base.Meta = "Rock Candy Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(326)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(582)
				},
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(838)
				}
			};
		}
	}
}
