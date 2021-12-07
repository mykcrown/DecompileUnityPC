using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000AC RID: 172
	public class MadCatzFightStickTE2MacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0001C280 File Offset: 0x0001A680
		public MadCatzFightStickTE2MacProfile()
		{
			base.Name = "Mad Catz Fight Stick TE2";
			base.Meta = "Mad Catz Fight Stick TE2 on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61568)
				}
			};
		}
	}
}
