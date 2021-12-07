using System;

namespace InControl
{
	// Token: 0x02000138 RID: 312
	public class UnityAnalogSource : InputControlSource
	{
		// Token: 0x06000714 RID: 1812 RVA: 0x0002EF1C File Offset: 0x0002D31C
		public UnityAnalogSource(int analogIndex)
		{
			this.AnalogIndex = analogIndex;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002EF2C File Offset: 0x0002D32C
		public float GetValue(InputDevice inputDevice)
		{
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			return unityInputDevice.ReadRawAnalogValue(this.AnalogIndex);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002EF4C File Offset: 0x0002D34C
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000579 RID: 1401
		public int AnalogIndex;
	}
}
