using System;

namespace network
{
	// Token: 0x020007C2 RID: 1986
	public interface INetMsg
	{
		// Token: 0x06003136 RID: 12598
		uint MsgID();

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06003137 RID: 12599
		uint MsgSize { get; }

		// Token: 0x06003138 RID: 12600
		bool Serialize();

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06003139 RID: 12601
		byte[] MsgBuffer { get; }
	}
}
