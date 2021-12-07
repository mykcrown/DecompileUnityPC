using System;

namespace InControl
{
	// Token: 0x020001C8 RID: 456
	[AutoDiscover]
	public class XTR_G2_MacUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x00049678 File Offset: 0x00047A78
		public XTR_G2_MacUnityProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.JoystickNames = new string[]
			{
				"FeiYing Model KMODEL Simulator - XTR+G2+FMS Controller"
			};
		}
	}
}
