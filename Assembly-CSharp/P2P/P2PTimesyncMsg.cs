using System;
using BattleServer;
using network;

namespace P2P
{
	// Token: 0x0200082B RID: 2091
	public class P2PTimesyncMsg : NetMsgBase, IP2PMessage, IMessageToSpecificUser
	{
		// Token: 0x060033C8 RID: 13256 RVA: 0x000F58B0 File Offset: 0x000F3CB0
		public P2PTimesyncMsg()
		{
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000F58B8 File Offset: 0x000F3CB8
		public P2PTimesyncMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000F58E3 File Offset: 0x000F3CE3
		public override uint MsgID()
		{
			return 9U;
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000F58E7 File Offset: 0x000F3CE7
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060033CC RID: 13260 RVA: 0x000F58EA File Offset: 0x000F3CEA
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000F58F2 File Offset: 0x000F3CF2
		public override void SerializeMsg()
		{
			base.Pack(this.timeOffset);
			base.Pack(this.sendTimeLocalMs);
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000F590C File Offset: 0x000F3D0C
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.timeOffset);
			base.Unpack(ref this.sendTimeLocalMs);
		}

		// Token: 0x04002410 RID: 9232
		public ulong _targetUserID;

		// Token: 0x04002411 RID: 9233
		public long timeOffset;

		// Token: 0x04002412 RID: 9234
		public long sendTimeLocalMs;
	}
}
