// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

namespace BattleServer
{
	public class BatchMsg : NetMsgBase, IBufferable
	{
		public class BatchedMsg
		{
			public byte msgId;

			public uint msgSize;

			public byte[] msgBuffer;

			public BatchedMsg()
			{
			}

			public BatchedMsg(uint bufferSize)
			{
				this.msgBuffer = new byte[bufferSize];
			}

			public BatchedMsg(byte msgId, uint msgSize, byte[] msgBytes)
			{
				this.msgId = msgId;
				this.msgSize = msgSize;
				if (msgSize != 0u)
				{
					this.msgBuffer = new byte[msgSize];
					Buffer.BlockCopy(msgBytes, 0, this.msgBuffer, 0, (int)msgSize);
				}
			}

			public void CopyTo(byte msgId, uint msgSize, byte[] msgBytes)
			{
				this.msgId = msgId;
				this.msgSize = msgSize;
				Buffer.BlockCopy(msgBytes, 0, this.msgBuffer, 0, (int)msgSize);
			}
		}

		public static readonly uint maxBatchMsgs = 30u;

		public int batchId;

		public BatchMsg.BatchedMsg[] m_batchedMessages = new BatchMsg.BatchedMsg[BatchMsg.maxBatchMsgs];

		public int numBatchedMessages;

		public BatchMsg()
		{
		}

		public BatchMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			if (msgSize != 0u)
			{
				this.m_msgbuffer = new byte[msgSize];
				Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			}
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize * BatchMsg.maxBatchMsgs + 8u];
			this.m_reusable = true;
			int num = 0;
			while ((long)num < (long)((ulong)BatchMsg.maxBatchMsgs))
			{
				this.m_batchedMessages[num] = new BatchMsg.BatchedMsg(bufferSize);
				num++;
			}
		}

		public override void ResetBuffer()
		{
			base.ResetBuffer();
			this.batchId = 0;
			this.numBatchedMessages = 0;
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		public override uint MsgID()
		{
			return 0u;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.batchId);
			base.Pack((uint)this.numBatchedMessages);
			for (int i = 0; i < this.numBatchedMessages; i++)
			{
				BatchMsg.BatchedMsg batchedMsg = this.m_batchedMessages[i];
				base.Pack(batchedMsg.msgId);
				base.Pack(batchedMsg.msgSize);
				if (batchedMsg.msgBuffer != null && batchedMsg.msgBuffer.Length != 0)
				{
					if (this.m_reusable)
					{
						base.PackByteArrayBuffered(batchedMsg.msgBuffer, batchedMsg.msgSize);
					}
					else
					{
						base.PackByteArray(batchedMsg.msgBuffer);
					}
				}
			}
		}

		public override void DeserializeMsg()
		{
			try
			{
				base.Unpack(ref this.batchId);
				base.Unpack(ref this.numBatchedMessages);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString() + "0");
			}
			uint num = 0u;
			while ((ulong)num != (ulong)((long)this.numBatchedMessages))
			{
				BatchMsg.BatchedMsg batchedMsg = this.m_batchedMessages[(int)num];
				try
				{
					base.Unpack(ref batchedMsg.msgId);
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.ToString() + "1." + num);
				}
				try
				{
					base.Unpack(ref batchedMsg.msgSize);
				}
				catch (Exception ex3)
				{
					throw new Exception(ex3.ToString() + "2." + batchedMsg.msgId);
				}
				if (batchedMsg.msgSize != 0u)
				{
					try
					{
						if (this.m_reusable)
						{
							base.UnpackByteArrayBuffered(ref batchedMsg.msgBuffer);
						}
						else
						{
							base.UnpackByteArray(ref batchedMsg.msgBuffer);
						}
					}
					catch (Exception ex4)
					{
						throw new Exception(ex4.ToString() + "3." + batchedMsg.msgId);
					}
				}
				num += 1u;
			}
		}
	}
}
