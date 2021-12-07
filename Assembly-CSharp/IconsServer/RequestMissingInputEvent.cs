using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007EC RID: 2028
	public class RequestMissingInputEvent : BatchEvent
	{
		// Token: 0x0600320A RID: 12810 RVA: 0x000F26D4 File Offset: 0x000F0AD4
		public override void UpdateNetMessage(ref INetMsg message)
		{
			RequestMissingInputMsg requestMissingInputMsg = message as RequestMissingInputMsg;
			requestMissingInputMsg._targetUserID = this._targetUserID;
			requestMissingInputMsg.forPlayer = this.forPlayer;
			requestMissingInputMsg.fromPlayer = this.fromPlayer;
			requestMissingInputMsg.startFrame = this.startFrame;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000F2719 File Offset: 0x000F0B19
		public void LoadFrom(RequestMissingInputMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.forPlayer = msg.forPlayer;
			this.fromPlayer = msg.fromPlayer;
			this.startFrame = msg.startFrame;
		}

		// Token: 0x0400233E RID: 9022
		public ulong _targetUserID;

		// Token: 0x0400233F RID: 9023
		public int fromPlayer;

		// Token: 0x04002340 RID: 9024
		public int forPlayer;

		// Token: 0x04002341 RID: 9025
		public int startFrame;
	}
}
