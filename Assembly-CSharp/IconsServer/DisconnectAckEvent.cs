using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007EE RID: 2030
	public class DisconnectAckEvent : BatchEvent
	{
		// Token: 0x06003210 RID: 12816 RVA: 0x000F27BC File Offset: 0x000F0BBC
		public override void UpdateNetMessage(ref INetMsg message)
		{
			DisconnectAckMsg disconnectAckMsg = message as DisconnectAckMsg;
			disconnectAckMsg._targetUserID = this._targetUserID;
			disconnectAckMsg.senderPlayerID = this.senderID;
			disconnectAckMsg.quittingPlayerID = this.quittingPlayerID;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x000F27F5 File Offset: 0x000F0BF5
		public void LoadFrom(DisconnectAckMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.senderID = msg.senderPlayerID;
			this.quittingPlayerID = msg.quittingPlayerID;
		}

		// Token: 0x04002345 RID: 9029
		public ulong _targetUserID;

		// Token: 0x04002346 RID: 9030
		public int senderID;

		// Token: 0x04002347 RID: 9031
		public int quittingPlayerID;
	}
}
