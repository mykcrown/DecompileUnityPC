using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x0200007A RID: 122
	public abstract class InputDeviceManager
	{
		// Token: 0x060004BE RID: 1214
		public abstract void Update(ulong updateTick, float deltaTime);

		// Token: 0x060004BF RID: 1215 RVA: 0x00013952 File Offset: 0x00011D52
		public virtual void Destroy()
		{
		}

		// Token: 0x040003FC RID: 1020
		protected List<InputDevice> devices = new List<InputDevice>();
	}
}
