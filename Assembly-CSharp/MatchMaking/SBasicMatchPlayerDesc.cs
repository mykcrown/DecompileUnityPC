using System;
using network;

namespace MatchMaking
{
	// Token: 0x020007BD RID: 1981
	public class SBasicMatchPlayerDesc
	{
		// Token: 0x06003100 RID: 12544 RVA: 0x000F0D4F File Offset: 0x000EF14F
		public virtual void Pack(NetMsgBase msg)
		{
			msg.Pack(this.name);
			msg.Pack(this.userID);
			msg.Pack(this.isSpectator);
			msg.Pack(this.team);
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000F0D81 File Offset: 0x000EF181
		public virtual void Unpack(NetMsgBase msg)
		{
			msg.Unpack(ref this.name);
			msg.Unpack(ref this.userID);
			msg.Unpack(ref this.isSpectator);
			msg.Unpack(ref this.team);
		}

		// Token: 0x04002277 RID: 8823
		public string name;

		// Token: 0x04002278 RID: 8824
		public ulong userID;

		// Token: 0x04002279 RID: 8825
		public bool isSpectator;

		// Token: 0x0400227A RID: 8826
		public byte team;
	}
}
