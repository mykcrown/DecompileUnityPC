using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200013E RID: 318
	public class UnityKeyCodeSource : InputControlSource
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x0002F17B File Offset: 0x0002D57B
		public UnityKeyCodeSource()
		{
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002F183 File Offset: 0x0002D583
		public UnityKeyCodeSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002F192 File Offset: 0x0002D592
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0002F1B0 File Offset: 0x0002D5B0
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (Input.GetKey(this.KeyCodeList[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000583 RID: 1411
		public KeyCode[] KeyCodeList;
	}
}
