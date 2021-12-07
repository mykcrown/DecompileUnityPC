// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	[AutoDiscover]
	public class XTR_G2_MacNativeProfile : NativeInputDeviceProfile
	{
		public XTR_G2_MacNativeProfile()
		{
			base.Name = "KMODEL Simulator XTR G2 FMS Controller";
			base.Meta = "KMODEL Simulator XTR G2 FMS Controller on OS X";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"OS X"
			};
			this.Matchers = new NativeInputDeviceMatcher[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = new ushort?(2971),
					ProductID = new ushort?(16402),
					NameLiterals = new string[]
					{
						"KMODEL Simulator - XTR+G2+FMS Controller"
					}
				}
			};
		}
	}
}
