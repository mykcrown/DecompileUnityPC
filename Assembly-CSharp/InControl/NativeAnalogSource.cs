using System;

namespace InControl
{
	// Token: 0x02000080 RID: 128
	public class NativeAnalogSource : InputControlSource
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x0001A36D File Offset: 0x0001876D
		public NativeAnalogSource(int analogIndex)
		{
			this.AnalogIndex = analogIndex;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A37C File Offset: 0x0001877C
		public float GetValue(InputDevice inputDevice)
		{
			NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
			return nativeInputDevice.ReadRawAnalogValue(this.AnalogIndex);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A39C File Offset: 0x0001879C
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000463 RID: 1123
		public int AnalogIndex;
	}
}
