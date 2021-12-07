using System;

namespace InControl
{
	// Token: 0x020001C9 RID: 457
	[AutoDiscover]
	public class XTR_G2_WindowsUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007C5 RID: 1989 RVA: 0x000496D0 File Offset: 0x00047AD0
		public XTR_G2_WindowsUnityProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on Windows";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"Windows"
			};
			this.JoystickNames = new string[]
			{
				"KMODEL Simulator - XTR+G2+FMS Controller"
			};
		}
	}
}
