using System;

namespace InControl
{
	// Token: 0x020001CA RID: 458
	[AutoDiscover]
	public class XTR55_G2_MacUnityProfile : UnityInputDeviceProfile
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x00049728 File Offset: 0x00047B28
		public XTR55_G2_MacUnityProfile()
		{
			base.Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			base.Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.JoystickNames = new string[]
			{
				"              SAILI Simulator --- XTR5.5+G2+FMS Controller"
			};
		}
	}
}
