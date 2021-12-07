using System;

namespace IconsServer
{
	// Token: 0x020007FE RID: 2046
	public class SMsg : NetEvent
	{
		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x000F31B2 File Offset: 0x000F15B2
		public override EEvents Type
		{
			get
			{
				return EEvents.Msg;
			}
		}

		// Token: 0x04002381 RID: 9089
		public byte msgId = byte.MaxValue;

		// Token: 0x04002382 RID: 9090
		public byte[] buffer;

		// Token: 0x04002383 RID: 9091
		public uint bufferSize;
	}
}
