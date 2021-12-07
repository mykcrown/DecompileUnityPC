// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	[AutoDiscover]
	public class XTR55_G2_MacUnityProfile : UnityInputDeviceProfile
	{
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
