using System;

namespace IconsServer
{
	// Token: 0x020007F1 RID: 2033
	public class DesyncEvent : ServerEvent
	{
		// Token: 0x06003219 RID: 12825 RVA: 0x000F2A04 File Offset: 0x000F0E04
		public DesyncEvent(int desyncFrame)
		{
			this.desyncFrame = desyncFrame;
		}

		// Token: 0x04002350 RID: 9040
		public int desyncFrame;
	}
}
