using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007F0 RID: 2032
	public class InputAckEvent : BatchEvent
	{
		// Token: 0x06003217 RID: 12823 RVA: 0x000F29B0 File Offset: 0x000F0DB0
		public override void UpdateNetMessage(ref INetMsg message)
		{
			InputAckMsg inputAckMsg = message as InputAckMsg;
			inputAckMsg.latestAckedFrame = (ushort)this.latestAckedFrame;
			inputAckMsg.playerID = this.senderPlayerID;
			inputAckMsg._targetUserID = this._targetUserID;
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x000F29EA File Offset: 0x000F0DEA
		public void LoadFrom(InputAckMsg ackMsg)
		{
			this.latestAckedFrame = (int)ackMsg.latestAckedFrame;
			this.senderPlayerID = ackMsg.playerID;
		}

		// Token: 0x0400234D RID: 9037
		public int latestAckedFrame;

		// Token: 0x0400234E RID: 9038
		public byte senderPlayerID;

		// Token: 0x0400234F RID: 9039
		public ulong _targetUserID;
	}
}
