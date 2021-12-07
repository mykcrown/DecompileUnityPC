using System;

namespace InControl.NativeProfile
{
	// Token: 0x02000082 RID: 130
	public class AfterglowPrismaticXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		// Token: 0x0600055C RID: 1372 RVA: 0x0001AD94 File Offset: 0x00019194
		public AfterglowPrismaticXboxOneControllerMacProfile()
		{
			base.Name = "Afterglow Prismatic Xbox One Controller";
			base.Meta = "Afterglow Prismatic Xbox One Controller on Mac";
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(3695),
					ProductID = new ushort?(313)
				}
			};
		}
	}
}
