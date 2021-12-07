using System;

namespace IconsServer
{
	// Token: 0x020007FC RID: 2044
	public class SNetwork : NetEvent
	{
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000F319C File Offset: 0x000F159C
		public override EEvents Type
		{
			get
			{
				return EEvents.Network;
			}
		}

		// Token: 0x04002379 RID: 9081
		public SNetwork.ENetEvent networkEvent;

		// Token: 0x0400237A RID: 9082
		public bool success;

		// Token: 0x0400237B RID: 9083
		public string msg;

		// Token: 0x020007FD RID: 2045
		public enum ENetEvent
		{
			// Token: 0x0400237D RID: 9085
			Invalid,
			// Token: 0x0400237E RID: 9086
			Connect,
			// Token: 0x0400237F RID: 9087
			Disconnect,
			// Token: 0x04002380 RID: 9088
			Error
		}
	}
}
