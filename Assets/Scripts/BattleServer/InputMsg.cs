// Decompile from assembly: Assembly-CSharp.dll

using network;
using P2P;
using System;

namespace BattleServer
{
	public class InputMsg : NetMsgFast, IQueueDuplicateHandler, IBufferable, IP2PMessage, IUDPMessage, IMessageToSpecificUser
	{
		public struct InputArrayData
		{
			public bool usePreviousFrame;

			public ulong buttons;

			public uint flags;

			public ushort axes;

			public void UsePreviousFrame()
			{
				this.usePreviousFrame = true;
			}

			public void Set(ulong buttons, uint flags, ushort axes)
			{
				this.usePreviousFrame = false;
				this.buttons = buttons;
				this.flags = flags;
				this.axes = axes;
			}

			public void CopyFrom(InputMsg.InputArrayData target)
			{
				this.usePreviousFrame = target.usePreviousFrame;
				this.buttons = target.buttons;
				this.flags = target.flags;
				this.axes = target.axes;
			}
		}

		public ulong _targetUserID;

		public ushort startFrame;

		public byte numInputs;

		public byte playerID;

		public static readonly int MaxInputArraySize = 120;

		public InputMsg.InputArrayData[] inputArray = new InputMsg.InputArrayData[InputMsg.MaxInputArraySize];

		public ulong TargetUserID
		{
			get
			{
				return this._targetUserID;
			}
		}

		public InputMsg()
		{
		}

		public InputMsg(byte[] msg, uint msgSize)
		{
			this.m_size = msgSize * 8u;
			this.m_msgbuffer = new byte[msgSize];
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
		}

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

		public override uint MsgID()
		{
			return 1u;
		}

		public BroadcastType GetBroadcastMode()
		{
			return BroadcastType.Relay;
		}

		public override void SerializeMsg()
		{
			base.Pack(this.playerID, 4u);
			base.Pack(this.startFrame, 16u);
			base.Pack(this.numInputs, 7u);
			for (int i = 0; i < (int)this.numInputs; i++)
			{
				base.Pack(this.inputArray[i].usePreviousFrame);
				if (!this.inputArray[i].usePreviousFrame)
				{
					base.Pack(this.inputArray[i].buttons, 51u);
					base.Pack(this.inputArray[i].flags, 5u);
					base.Pack(this.inputArray[i].axes, 16u);
				}
			}
		}

		public override void DeserializeMsg()
		{
			base.Unpack(ref this.playerID, 4u);
			base.Unpack(ref this.startFrame, 16u);
			base.Unpack(ref this.numInputs, 7u);
			for (int i = 0; i < (int)this.numInputs; i++)
			{
				base.Unpack(ref this.inputArray[i].usePreviousFrame);
				if (!this.inputArray[i].usePreviousFrame)
				{
					base.Unpack(ref this.inputArray[i].buttons, 51u);
					base.Unpack(ref this.inputArray[i].flags, 5u);
					base.Unpack(ref this.inputArray[i].axes, 16u);
				}
			}
		}

		public void SetAsBufferable(uint bufferSize)
		{
			this.m_msgbuffer = new byte[bufferSize];
		}

		public void DeserializeToBuffer(byte[] msg, uint msgSize)
		{
			base.ResetBuffer();
			Buffer.BlockCopy(msg, 0, this.m_msgbuffer, 0, (int)msgSize);
			this.DeserializeMsg();
		}

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
	}
}
