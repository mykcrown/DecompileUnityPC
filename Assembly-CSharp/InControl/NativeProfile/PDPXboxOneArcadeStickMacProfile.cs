using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CC RID: 204
	public class PDPXboxOneArcadeStickMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x0001D24C File Offset: 0x0001B64C
		public PDPXboxOneArcadeStickMacProfile()
		{
			base.Name = "PDP Xbox One Arcade Stick";
			base.Meta = "PDP Xbox One Arcade Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(348)
				}
			};
		}
	}
}
