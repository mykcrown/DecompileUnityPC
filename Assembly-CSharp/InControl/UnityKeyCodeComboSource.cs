using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200013D RID: 317
	public class UnityKeyCodeComboSource : InputControlSource
	{
		// Token: 0x06000726 RID: 1830 RVA: 0x0002F10B File Offset: 0x0002D50B
		public UnityKeyCodeComboSource()
		{
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0002F113 File Offset: 0x0002D513
		public UnityKeyCodeComboSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002F122 File Offset: 0x0002D522
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002F140 File Offset: 0x0002D540
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (!Input.GetKey(this.KeyCodeList[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000582 RID: 1410
		public KeyCode[] KeyCodeList;
	}
}
