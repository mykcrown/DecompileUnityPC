using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E9 RID: 233
	public class Xbox360MortalKombatFightStickMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x0001DFCC File Offset: 0x0001C3CC
		public Xbox360MortalKombatFightStickMacProfile()
		{
			base.Name = "Xbox 360 Mortal Kombat Fight Stick";
			base.Meta = "Xbox 360 Mortal Kombat Fight Stick on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(7085),
					ProductID = new ushort?(63750)
				}
			};
		}
	}
}
