// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class RelayMsg : NetMsgBase, IBufferable, IP2PMessage, IP2PAdjustBroadcastMode
	{
		public byte[] bytes;

		public uint byteSize;

		private BroadcastType broadcastMode;

		public RelayMsg()
		{
		}

		public RelayMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		public override uint MsgID()
		{
			return 0u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return this.broadcastMode;
		}

		public void SetBroadcastMode(BroadcastType broadcastMode)
		{
			this.broadcastMode = broadcastMode;
		}

		public override void SerializeMsg()
		{
			base.PackByteArray(this.bytes);
			base.Pack((int)this.broadcastMode);
		}

		public override void DeserializeMsg()
		{
			base.UnpackByteArray(ref this.bytes);
			uint num = 3u;
			base.Unpack(ref num);
			this.broadcastMode = (BroadcastType)num;
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
			this.bytes = new byte[bufferSize];
			this.m_reusable = true;
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}
	}
}
