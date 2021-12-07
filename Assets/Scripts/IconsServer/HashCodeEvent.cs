// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class HashCodeEvent : BatchEvent
	{
		public int frame;

		public short hashCode;

		public byte senderId;

		public override void UpdateNetMessage(ref INetMsg message)
		{
			HashCodeMsg hashCodeMsg = message as HashCodeMsg;
			hashCodeMsg.frame = (ushort)this.frame;
			hashCodeMsg.hashCode = this.hashCode;
			hashCodeMsg.senderId = this.senderId;
		}

		public void LoadFrom(HashCodeMsg hashCodeMsg)
		{
			this.frame = (int)hashCodeMsg.frame;
			this.hashCode = hashCodeMsg.hashCode;
			this.senderId = hashCodeMsg.senderId;
		}
	}
}
