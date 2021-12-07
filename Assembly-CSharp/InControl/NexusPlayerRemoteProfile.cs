using System;

namespace InControl
{
	// Token: 0x0200018F RID: 399
	[AutoDiscover]
	public class NexusPlayerRemoteProfile : UnityInputDeviceProfile
	{
		// Token: 0x0600078B RID: 1931 RVA: 0x0003DEA0 File Offset: 0x0003C2A0
		public NexusPlayerRemoteProfile()
		{
			base.Name = "Nexus Player Remote";
			base.Meta = "Nexus Player Remote";
			base.DeviceClass = InputDeviceClass.Remote;
			base.IncludePlatforms = new string[]
			{
				"Android"
			};
			this.JoystickNames = new string[]
			{
				"Google Nexus Remote"
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button0
				},
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Back,
					Source = UnityInputDeviceProfile.EscapeKey
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.DPadLeftMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.DPadRightMapping(UnityInputDeviceProfile.Analog4),
				UnityInputDeviceProfile.DPadUpMapping(UnityInputDeviceProfile.Analog5),
				UnityInputDeviceProfile.DPadDownMapping(UnityInputDeviceProfile.Analog5)
			};
		}
	}
}
