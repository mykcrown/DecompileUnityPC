using System;

namespace InControl.NativeProfile
{
	// Token: 0x020000E7 RID: 231
	public class TSZPelicanControllerMacProfile : Xbox360DriverMacProfile
	{
		// Token: 0x060005C1 RID: 1473 RVA: 0x0001DF0C File Offset: 0x0001C30C
		public TSZPelicanControllerMacProfile()
		{
			base.Name = "TSZ Pelican Controller";
			base.Meta = "TSZ Pelican Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(513)
				}
			};
		}
	}
}
