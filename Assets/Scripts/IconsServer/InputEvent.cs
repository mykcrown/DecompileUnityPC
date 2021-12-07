// Decompile from assembly: Assembly-CSharp.dll

using BattleServer;
using network;
using System;

namespace IconsServer
{
	public class InputEvent : BatchEvent
	{
		public ulong _targetUserID;

		public int startFrame;

		public byte numInputs;

		public byte playerID;

		public InputMsg.InputArrayData[] inputArray = new InputMsg.InputArrayData[InputMsg.MaxInputArraySize];

		public override void UpdateNetMessage(ref INetMsg message)
		{
			InputMsg inputMsg = message as InputMsg;
			inputMsg._targetUserID = this._targetUserID;
			inputMsg.startFrame = (ushort)this.startFrame;
			inputMsg.numInputs = this.numInputs;
			inputMsg.playerID = this.playerID;
			for (int i = 0; i < this.inputArray.Length; i++)
			{
				inputMsg.inputArray[i].CopyFrom(this.inputArray[i]);
			}
		}

		public void LoadFrom(InputMsg inEvent)
		{
			this._targetUserID = inEvent._targetUserID;
			this.startFrame = (int)inEvent.startFrame;
			this.numInputs = inEvent.numInputs;
			this.playerID = inEvent.playerID;
			for (int i = 0; i < this.inputArray.Length; i++)
			{
				this.inputArray[i].CopyFrom(inEvent.inputArray[i]);
			}
		}

		public void LoadFrom(InputEvent inEvent)
		{
			this._targetUserID = inEvent._targetUserID;
			this.startFrame = inEvent.startFrame;
			this.numInputs = inEvent.numInputs;
			this.playerID = inEvent.playerID;
			for (int i = 0; i < this.inputArray.Length; i++)
			{
				this.inputArray[i].CopyFrom(inEvent.inputArray[i]);
			}
		}
	}
}
