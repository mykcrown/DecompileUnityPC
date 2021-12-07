using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007EB RID: 2027
	public class HashCodeEvent : BatchEvent
	{
		// Token: 0x06003207 RID: 12807 RVA: 0x000F266C File Offset: 0x000F0A6C
		public override void UpdateNetMessage(ref INetMsg message)
		{
			HashCodeMsg hashCodeMsg = message as HashCodeMsg;
			hashCodeMsg.frame = (ushort)this.frame;
			hashCodeMsg.hashCode = this.hashCode;
			hashCodeMsg.senderId = this.senderId;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000F26A6 File Offset: 0x000F0AA6
		public void LoadFrom(HashCodeMsg hashCodeMsg)
		{
			this.frame = (int)hashCodeMsg.frame;
			this.hashCode = hashCodeMsg.hashCode;
			this.senderId = hashCodeMsg.senderId;
		}

		// Token: 0x0400233B RID: 9019
		public int frame;

		// Token: 0x0400233C RID: 9020
		public short hashCode;

		// Token: 0x0400233D RID: 9021
		public byte senderId;
	}
}
