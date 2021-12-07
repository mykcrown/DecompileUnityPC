using System;
using network;

namespace IconsServer
{
	// Token: 0x020007B1 RID: 1969
	public class SItemIdAndVersion
	{
		// Token: 0x060030FB RID: 12539 RVA: 0x000F0CCE File Offset: 0x000EF0CE
		public void Pack(NetMsgBase msg)
		{
			msg.Pack((uint)this.itemId);
			msg.Pack((ushort)this.version);
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000F0CEC File Offset: 0x000EF0EC
		public void Unpack(NetMsgBase msg)
		{
			uint num = 0U;
			msg.Unpack(ref num);
			this.itemId = (ulong)num;
			ushort num2 = 0;
			msg.Unpack(ref num2);
			this.version = (ulong)num2;
		}

		// Token: 0x04002239 RID: 8761
		public ulong itemId = ulong.MaxValue;

		// Token: 0x0400223A RID: 8762
		public ulong version;
	}
}
