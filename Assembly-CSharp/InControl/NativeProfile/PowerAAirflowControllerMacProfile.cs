using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000CE RID: 206
	public class PowerAAirflowControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001D3B4 File Offset: 0x0001B7B4
		public PowerAAirflowControllerMacProfile()
		{
			base.Name = "PowerA Airflow Controller";
			base.Meta = "PowerA Airflow Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(5604),
					ProductID = new ushort?(16138)
				}
			};
		}
	}
}
