// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class DisconnectAckEvent : BatchEvent
	{
		public ulong _targetUserID;

		public int senderID;

		public int quittingPlayerID;

		public override void UpdateNetMessage(ref INetMsg message)
		{
			DisconnectAckMsg disconnectAckMsg = message as DisconnectAckMsg;
			disconnectAckMsg._targetUserID = this._targetUserID;
			disconnectAckMsg.senderPlayerID = this.senderID;
			disconnectAckMsg.quittingPlayerID = this.quittingPlayerID;
		}

		public void LoadFrom(DisconnectAckMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.senderID = msg.senderPlayerID;
			this.quittingPlayerID = msg.quittingPlayerID;
		}
	}
}
