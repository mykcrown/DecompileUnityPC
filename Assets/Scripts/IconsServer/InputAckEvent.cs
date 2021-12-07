// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class InputAckEvent : BatchEvent
	{
		public int latestAckedFrame;

		public byte senderPlayerID;

		public ulong _targetUserID;

		public override void UpdateNetMessage(ref INetMsg message)
		{
			InputAckMsg inputAckMsg = message as InputAckMsg;
			inputAckMsg.latestAckedFrame = (ushort)this.latestAckedFrame;
			inputAckMsg.playerID = this.senderPlayerID;
			inputAckMsg._targetUserID = this._targetUserID;
		}

		public void LoadFrom(InputAckMsg ackMsg)
		{
			this.latestAckedFrame = (int)ackMsg.latestAckedFrame;
			this.senderPlayerID = ackMsg.playerID;
		}
	}
}
