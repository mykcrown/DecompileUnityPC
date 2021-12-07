using System;

namespace InControl
{
	// Token: 0x020001CB RID: 459
	[AutoDiscover]
	public class XTR55_G2_WindowsUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007C7 RID: 1991 RVA: 0x00049780 File Offset: 0x00047B80
		public XTR55_G2_WindowsUnityProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"SAILI Simulator --- XTR5.5+G2+FMS Controller"
			};
		}
	}
}
