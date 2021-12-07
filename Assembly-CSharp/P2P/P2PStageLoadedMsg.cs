using System;
using network;

namespace P2P
{
	// Token: 0x0200081B RID: 2075
	public class P2PStageLoadedMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x06003366 RID: 13158 RVA: 0x000F4F88 File Offset: 0x000F3388
		public P2PStageLoadedMsg()
		{
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x000F4F90 File Offset: 0x000F3390
		public P2PStageLoadedMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x000F4FBB File Offset: 0x000F33BB
		public override uint MsgID()
		{
			return 14U;
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x000F4FBF File Offset: 0x000F33BF
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToHost;
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x000F4FC2 File Offset: 0x000F33C2
		public override void SerializeMsg()
		{
			base.Pack(this.steamID);
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x000F4FD0 File Offset: 0x000F33D0
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.steamID);
		}

		// Token: 0x040023F0 RID: 9200
		public ulong steamID;
	}
}
