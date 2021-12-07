// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class DisconnectEvent : BatchEvent
	{
		public ulong _targetUserID;

		public int frame;

		public int playerID;

		public override void UpdateNetMessage(ref INetMsg message)
		{
			DisconnectMsg disconnectMsg = message as DisconnectMsg;
			disconnectMsg._targetUserID = this._targetUserID;
			disconnectMsg.playerID = this.playerID;
			disconnectMsg.frame = this.frame;
		}

		public void LoadFrom(DisconnectMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.playerID = msg.playerID;
			this.frame = msg.frame;
		}
	}
}
