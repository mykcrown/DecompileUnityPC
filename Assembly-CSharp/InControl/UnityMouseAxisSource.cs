using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200013F RID: 319
	public class UnityMouseAxisSource : InputControlSource
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x0002F1EB File Offset: 0x0002D5EB
		public UnityMouseAxisSource()
		{
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0002F1F3 File Offset: 0x0002D5F3
		public UnityMouseAxisSource(string axis)
		{
			this.MouseAxisQuery = "mouse " + axis;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0002F20C File Offset: 0x0002D60C
		public float GetValue(InputDevice inputDevice)
		{
			return Input.GetAxisRaw(this.MouseAxisQuery);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0002F219 File Offset: 0x0002D619
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000584 RID: 1412
		public string MouseAxisQuery;
	}
}
