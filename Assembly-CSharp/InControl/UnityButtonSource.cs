using System;

namespace InControl
{
	// Token: 0x02000139 RID: 313
	public class UnityButtonSource : InputControlSource
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x0002EF5A File Offset: 0x0002D35A
		public UnityButtonSource(int buttonIndex)
		{
			this.ButtonIndex = buttonIndex;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002EF69 File Offset: 0x0002D369
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002EF88 File Offset: 0x0002D388
		public bool GetState(InputDevice inputDevice)
		{
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			return unityInputDevice.ReadRawButtonState(this.ButtonIndex);
		}

		// Token: 0x0400057A RID: 1402
		public int ButtonIndex;
	}
}
