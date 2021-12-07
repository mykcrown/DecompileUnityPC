// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	public class UnityAnalogSource : InputControlSource
	{
		public int AnalogIndex;

		public UnityAnalogSource(int analogIndex)
		{
			this.AnalogIndex = analogIndex;
		}

		public float GetValue(InputDevice inputDevice)
		{
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			return unityInputDevice.ReadRawAnalogValue(this.AnalogIndex);
		}

		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}
	}
}
