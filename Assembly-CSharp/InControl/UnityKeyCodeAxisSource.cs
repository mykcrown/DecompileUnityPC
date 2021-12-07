using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200013C RID: 316
	public class UnityKeyCodeAxisSource : InputControlSource
	{
		// Token: 0x06000722 RID: 1826 RVA: 0x0002F0A3 File Offset: 0x0002D4A3
		public UnityKeyCodeAxisSource()
		{
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002F0AB File Offset: 0x0002D4AB
		public UnityKeyCodeAxisSource(KeyCode negativeKeyCode, KeyCode positiveKeyCode)
		{
			this.NegativeKeyCode = negativeKeyCode;
			this.PositiveKeyCode = positiveKeyCode;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0002F0C4 File Offset: 0x0002D4C4
		public float GetValue(InputDevice inputDevice)
		{
			int num = 0;
			if (Input.GetKey(this.NegativeKeyCode))
			{
				num--;
			}
			if (Input.GetKey(this.PositiveKeyCode))
			{
				num++;
			}
			return (float)num;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002F0FD File Offset: 0x0002D4FD
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000580 RID: 1408
		public KeyCode NegativeKeyCode;

		// Token: 0x04000581 RID: 1409
		public KeyCode PositiveKeyCode;
	}
}
