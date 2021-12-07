using System;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x02000789 RID: 1929
	public class InputMsg : NetMsgFast, IQueueDuplicateHandler, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		// Token: 0x06002F8E RID: 12174 RVA: 0x000EE3DB File Offset: 0x000EC7DB
		public InputMsg()
		{
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000EE3F3 File Offset: 0x000EC7F3
		public InputMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8U;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000EE430 File Offset: 0x000EC830
		public void CopyFrom(InputMsg copyMessage)
		{
			this._targetUserID = copyMessage._targetUserID;
			this.startFrame = copyMessage.startFrame;
			this.numInputs = copyMessage.numInputs;
			this.playerID = copyMessage.playerID;
			for (int i = 0; i < (int)this.numInputs; i++)
			{
				this.inputArray[i].CopyFrom(copyMessage.inputArray[i]);
			}
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000EE4A6 File Offset: 0x000EC8A6
		public override uint MsgID()
		{
			return 1U;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000EE4A9 File Offset: 0x000EC8A9
		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002F93 RID: 12179 RVA: 0x000EE4AC File Offset: 0x000EC8AC
		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000EE4B4 File Offset: 0x000EC8B4
		public override void SerializeMsg()
		{
			base.Pack(this.playerID, 4U);
			base.Pack(this.startFrame, 16U);
			base.Pack(this.numInputs, 7U);
			for (int i = 0; i < (int)this.numInputs; i++)
			{
				base.Pack(this.inputArray[i].usePreviousFrame);
				if (!this.inputArray[i].usePreviousFrame)
				{
					base.Pack(this.inputArray[i].buttons, 51U);
					base.Pack(this.inputArray[i].flags, 5U);
					base.Pack(this.inputArray[i].axes, 16U);
				}
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x000EE578 File Offset: 0x000EC978
		public override void DeserializeMsg()
		{
			base.Unpack(ref this.playerID, 4U);
			base.Unpack(ref this.startFrame, 16U);
			base.Unpack(ref this.numInputs, 7U);
			for (int i = 0; i < (int)this.numInputs; i++)
			{
				base.Unpack(ref this.inputArray[i].usePreviousFrame);
				if (!this.inputArray[i].usePreviousFrame)
				{
					base.Unpack(ref this.inputArray[i].buttons, 51U);
					base.Unpack(ref this.inputArray[i].flags, 5U);
					base.Unpack(ref this.inputArray[i].axes, 16U);
				}
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x000EE63B File Offset: 0x000ECA3B
		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x000EE64A File Offset: 0x000ECA4A
		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000EE668 File Offset: 0x000ECA68
		public bool HandledAsDuplicate(INetMsg messageInQueue)
		{
			InputMsg inputMsg = messageInQueue as InputMsg;
			if (this.playerID == inputMsg.playerID && (this.numInputs > inputMsg.numInputs || this.startFrame > inputMsg.startFrame))
			{
				inputMsg.CopyFrom(this);
				return true;
			}
			return false;
		}

		// Token: 0x04002160 RID: 8544
		public ulong _targetUserID;

		// Token: 0x04002161 RID: 8545
		public ushort startFrame;

		// Token: 0x04002162 RID: 8546
		public byte numInputs;

		// Token: 0x04002163 RID: 8547
		public byte playerID;

		// Token: 0x04002164 RID: 8548
		public static readonly int MaxInputArraySize = 120;

		// Token: 0x04002165 RID: 8549
		public InputMsg.InputArrayData[] inputArray = new InputMsg.InputArrayData[InputMsg.MaxInputArraySize];

		// Token: 0x0200078A RID: 1930
		public struct InputArrayData
		{
			// Token: 0x06002F9A RID: 12186 RVA: 0x000EE6C2 File Offset: 0x000ECAC2
			public void UsePreviousFrame()
			{
				this.usePreviousFrame = true;
			}

			// Token: 0x06002F9B RID: 12187 RVA: 0x000EE6CB File Offset: 0x000ECACB
			public void Set(ulong buttons, uint flags, ushort axes)
			{
				this.usePreviousFrame = false;
				this.buttons = buttons;
				this.flags = flags;
				this.axes = axes;
			}

			// Token: 0x06002F9C RID: 12188 RVA: 0x000EE6E9 File Offset: 0x000ECAE9
			public void CopyFrom(InputMsg.InputArrayData target)
			{
				this.usePreviousFrame = target.usePreviousFrame;
				this.buttons = target.buttons;
				this.flags = target.flags;
				this.axes = target.axes;
			}

			// Token: 0x04002166 RID: 8550
			public bool usePreviousFrame;

			// Token: 0x04002167 RID: 8551
			public ulong buttons;

			// Token: 0x04002168 RID: 8552
			public uint flags;

			// Token: 0x04002169 RID: 8553
			public ushort axes;
		}
	}
}
