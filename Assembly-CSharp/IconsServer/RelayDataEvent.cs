using System;

namespace IconsServer
{
	// Token: 0x020007EA RID: 2026
	public class RelayDataEvent : BatchEvent
	{
		// Token: 0x04002339 RID: 9017
		public Guid matchId;

		// Token: 0x0400233A RID: 9018
		public byte[] bytes;
	}
}
