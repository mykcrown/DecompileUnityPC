using System;

namespace InControl
{
	// Token: 0x02000081 RID: 129
	public class NativeButtonSource : InputControlSource
	{
		// Token: 0x06000559 RID: 1369 RVA: 0x0001A3AA File Offset: 0x000187AA
		public NativeButtonSource(int buttonIndex)
		{
			this.ButtonIndex = buttonIndex;
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A3B9 File Offset: 0x000187B9
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A3D8 File Offset: 0x000187D8
		public bool GetState(InputDevice inputDevice)
		{
			NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
			return nativeInputDevice.ReadRawButtonState(this.ButtonIndex);
		}

		// Token: 0x04000464 RID: 1124
		public int ButtonIndex;
	}
}
