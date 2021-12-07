// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class RequestMissingInputEvent : BatchEvent
	{
		public ulong _targetUserID;

		public int fromPlayer;

		public int forPlayer;

		public int startFrame;

		public override void UpdateNetMessage(ref INetMsg message)
		{
			RequestMissingInputMsg requestMissingInputMsg = message as RequestMissingInputMsg;
			requestMissingInputMsg._targetUserID = this._targetUserID;
			requestMissingInputMsg.forPlayer = this.forPlayer;
			requestMissingInputMsg.fromPlayer = this.fromPlayer;
			requestMissingInputMsg.startFrame = this.startFrame;
		}

		public void LoadFrom(RequestMissingInputMsg msg)
		{
			this._targetUserID = msg._targetUserID;
			this.forPlayer = msg.forPlayer;
			this.fromPlayer = msg.fromPlayer;
			this.startFrame = msg.startFrame;
		}
	}
}
