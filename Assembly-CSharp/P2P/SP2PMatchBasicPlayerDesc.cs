using System;
using MatchMaking;
using network;

namespace P2P
{
	// Token: 0x02000817 RID: 2071
	public class SP2PMatchBasicPlayerDesc : SBasicMatchPlayerDesc
	{
		// Token: 0x0600335C RID: 13148 RVA: 0x000F4DA4 File Offset: 0x000F31A4
		public override void Pack(NetMsgBase msg)
		{
			base.Pack(msg);
			msg.Pack(this.steamID);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000F4DB9 File Offset: 0x000F31B9
		public override void Unpack(NetMsgBase msg)
		{
			base.Unpack(msg);
			msg.Unpack(ref this.steamID);
		}

		// Token: 0x040023DF RID: 9183
		public ulong steamID;
	}
}
