using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000B6 RID: 182
	public class MadCatzSF4FightStickRound2TEMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x06000590 RID: 1424 RVA: 0x0001C640 File Offset: 0x0001AA40
		public MadCatzSF4FightStickRound2TEMacProfile()
		{
			base.Name = "Mad Catz SF4 Fight Stick Round 2 TE";
			base.Meta = "Mad Catz SF4 Fight Stick Round 2 TE on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(61496)
				}
			};
		}
	}
}
