using System;
using network;

namespace BattleServer
{
	// Token: 0x02000794 RID: 1940
	public class BatchMsg : NetMsgBase, IBufferable
	{
		// Token: 0x06002FDD RID: 12253 RVA: 0x000EEBEB File Offset: 0x000ECFEB
		public BatchMsg()
		{
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x000EEC04 File Offset: 0x000ED004
		public BatchMsg(byte[] msg, uint msgSize)
		{
			this.m_msgSize = msgSize;
			if (msgSize != 0U)
			{
				this.m_msgbuffer = new byte[msgSize];
				Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x000EEC54 File Offset: 0x000ED054
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize * BatchMsg.maxBatchMsgs + 8U];
			this.m_reusable = true;
			int num = 0;
			while ((long)num < (long)((ulong)BatchMsg.maxBatchMsgs))
			{
				this.m_batchedMessages[num] = new BatchMsg.BatchedMsg(bufferSize);
				num++;
			}
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x000EECA3 File Offset: 0x000ED0A3
		public override void ResetBuffer()
		{
			base.ResetBuffer();
			this.batchId = 0;
			this.numBatchedMessages = 0;
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x000EECB9 File Offset: 0x000ED0B9
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			this.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x000EECD6 File Offset: 0x000ED0D6
		public override uint MsgID()
		{
			return 0U;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x000EECDC File Offset: 0x000ED0DC
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

		// Token: 0x06002FE4 RID: 12260 RVA: 0x000EED80 File Offset: 0x000ED180
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
			uint num = 0U;
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
				if (batchedMsg.msgSize != 0U)
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
				num += 1U;
			}
		}

		// Token: 0x04002186 RID: 8582
		public static readonly uint maxBatchMsgs = 30U;

		// Token: 0x04002187 RID: 8583
		public int batchId;

		// Token: 0x04002188 RID: 8584
		public BatchMsg.BatchedMsg[] m_batchedMessages = new BatchMsg.BatchedMsg[BatchMsg.maxBatchMsgs];

		// Token: 0x04002189 RID: 8585
		public int numBatchedMessages;

		// Token: 0x02000795 RID: 1941
		public class BatchedMsg
		{
			// Token: 0x06002FE6 RID: 12262 RVA: 0x000EEEE1 File Offset: 0x000ED2E1
			public BatchedMsg()
			{
			}

			// Token: 0x06002FE7 RID: 12263 RVA: 0x000EEEE9 File Offset: 0x000ED2E9
			public BatchedMsg(uint bufferSize)
			{
				this.msgBuffer = new byte[bufferSize];
			}

			// Token: 0x06002FE8 RID: 12264 RVA: 0x000EEEFE File Offset: 0x000ED2FE
			public BatchedMsg(byte msgId, uint msgSize, byte[] msgBytes)
			{
				this.msgId = msgId;
				this.msgSize = msgSize;
				if (msgSize != 0U)
				{
					this.msgBuffer = new byte[msgSize];
					Buffer.BlockCopy(msgBytes, 0, this.msgBuffer, 0, (int)msgSize);
				}
			}

			// Token: 0x06002FE9 RID: 12265 RVA: 0x000EEF36 File Offset: 0x000ED336
			public void CopyTo(byte msgId, uint msgSize, byte[] msgBytes)
			{
				this.msgId = msgId;
				this.msgSize = msgSize;
				Buffer.BlockCopy(msgBytes, 0, this.msgBuffer, 0, (int)msgSize);
			}

			// Token: 0x0400218A RID: 8586
			public byte msgId;

			// Token: 0x0400218B RID: 8587
			public uint msgSize;

			// Token: 0x0400218C RID: 8588
			public byte[] msgBuffer;
		}
	}
}
