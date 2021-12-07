using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CF RID: 207
	public class POWERAFUS1ONTournamentControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001D414 File Offset: 0x0001B814
		public POWERAFUS1ONTournamentControllerMacProfile()
		{
			base.Name = "POWER A FUS1ON Tournament Controller";
			base.Meta = "POWER A FUS1ON Tournament Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(9414),
					ProductID = new ushort?(21399)
				}
			};
		}
	}
}
