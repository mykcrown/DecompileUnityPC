using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007ED RID: 2029
	public class DisconnectEvent : BatchEvent
	{
		// Token: 0x0600320D RID: 12813 RVA: 0x000F2754 File Offset: 0x000F0B54
		public override void UpdateNetMessage(ref INetMsg message)
		{
			DisconnectMsg disconnectMsg = message as DisconnectMsg;
			disconnectMsg._targetUserID = this._targetUserID;
			disconnectMsg.playerID = this.playerID;
			disconnectMsg.frame = this.frame;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000F278D File Offset: 0x000F0B8D
		public void LoadFrom(DisconnectMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.playerID = msg.playerID;
			this.frame = msg.frame;
		}

		// Token: 0x04002342 RID: 9026
		public ulong _targetUserID;

		// Token: 0x04002343 RID: 9027
		public int frame;

		// Token: 0x04002344 RID: 9028
		public int playerID;
	}
}
