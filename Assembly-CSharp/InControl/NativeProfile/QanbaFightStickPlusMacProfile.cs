using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000D6 RID: 214
	public class QanbaFightStickPlusMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x0001D708 File Offset: 0x0001BB08
		public QanbaFightStickPlusMacProfile()
		{
			base.Name = "Qanba Fight Stick Plus";
			base.Meta = "Qanba Fight Stick Plus on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(1848),
					ProductID = new ushort?(48879)
				}
			};
		}
	}
}
