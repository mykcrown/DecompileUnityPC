using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000140 RID: 320
	public class UnityMouseButtonSource : InputControlSource
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x0002F227 File Offset: 0x0002D627
		public UnityMouseButtonSource()
		{
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002F22F File Offset: 0x0002D62F
		public UnityMouseButtonSource(int buttonId)
		{
			this.ButtonId = buttonId;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002F23E File Offset: 0x0002D63E
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002F25B File Offset: 0x0002D65B
		public bool GetState(InputDevice inputDevice)
		{
			return Input.GetMouseButton(this.ButtonId);
		}

		// Token: 0x04000585 RID: 1413
		public int ButtonId;
	}
}
