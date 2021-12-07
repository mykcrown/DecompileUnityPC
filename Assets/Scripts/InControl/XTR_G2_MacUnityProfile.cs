// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	[AutoDiscover]
	public class XTR_G2_MacUnityProfile : UnityInputDeviceProfile
	{
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
