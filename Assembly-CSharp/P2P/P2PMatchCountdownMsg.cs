using System;
using network;

namespace P2P
{
	// Token: 0x0200081F RID: 2079
	public class P2PMatchCountdownMsg : NetMsgBase, IP2PMessage
	{
		// Token: 0x0600337C RID: 13180 RVA: 0x000F5376 File Offset: 0x000F3776
		public P2PMatchCountdownMsg()
		{
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x000F537E File Offset: 0x000F377E
		public P2PMatchCountdownMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000F53A9 File Offset: 0x000F37A9
		public override uint MsgID()
		{
			return 22U;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000F53AD File Offset: 0x000F37AD
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.ToClient;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x000F53B0 File Offset: 0x000F37B0
		public override void SerializeMsg()
		{
			base.Pack(this.countDownSeconds);
			base.Pack(this.serverStartTimeMs);
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000F53CA File Offset: 0x000F37CA
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.countDownSeconds);
			base.Unpack(ref this.serverStartTimeMs);
		}

		// Token: 0x040023FD RID: 9213
		public uint countDownSeconds;

		// Token: 0x040023FE RID: 9214
		public ulong serverStartTimeMs;
	}
}
