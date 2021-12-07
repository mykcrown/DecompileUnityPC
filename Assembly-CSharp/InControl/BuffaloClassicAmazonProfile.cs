using System;

namespace InControl
{
	// Token: 0x02000150 RID: 336
	[AutoDiscover]
	public class BuffaloClassicAmazonProfile : UnityInputDeviceProfile
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x00032290 File Offset: 0x00030690
		public BuffaloClassicAmazonProfile()
		{
			base.Name = "Buffalo Class Gamepad";
			base.Meta = "Buffalo Class Gamepad on Amazon Fire TV";
			base.DeviceClass = InputDeviceClass.Controller;
			base.IncludePlatforms = new string[]
			{
				"Amazon AFT"
			};
			this.JoystickNames = new string[]
			{
				"USB,2-axis 8-button gamepad  "
			};
			base.ButtonMappings = new InputControlMapping[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action2,
					Source = UnityInputDeviceProfile.Button15
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action1,
					Source = UnityInputDeviceProfile.Button16
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action4,
					Source = UnityInputDeviceProfile.Button17
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = UnityInputDeviceProfile.Button18
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = UnityInputDeviceProfile.Button19
				}
			};
			base.AnalogMappings = new InputControlMapping[]
			{
				UnityInputDeviceProfile.DPadLeftMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.DPadRightMapping(UnityInputDeviceProfile.Analog0),
				UnityInputDeviceProfile.DPadUpMapping(UnityInputDeviceProfile.Analog1),
				UnityInputDeviceProfile.DPadDownMapping(UnityInputDeviceProfile.Analog1)
			};
		}
	}
}
