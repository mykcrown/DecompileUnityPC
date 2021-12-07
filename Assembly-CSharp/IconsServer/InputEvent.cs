using System;
using BattleServer;
using network;

namespace IconsServer
{
	// Token: 0x020007EF RID: 2031
	public class InputEvent : BatchEvent
	{
		// Token: 0x06003213 RID: 12819 RVA: 0x000F2834 File Offset: 0x000F0C34
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

		// Token: 0x06003214 RID: 12820 RVA: 0x000F28B8 File Offset: 0x000F0CB8
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

		// Token: 0x06003215 RID: 12821 RVA: 0x000F2930 File Offset: 0x000F0D30
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

		// Token: 0x04002348 RID: 9032
		public ulong _targetUserID;

		// Token: 0x04002349 RID: 9033
		public int startFrame;

		// Token: 0x0400234A RID: 9034
		public byte numInputs;

		// Token: 0x0400234B RID: 9035
		public byte playerID;

		// Token: 0x0400234C RID: 9036
		public InputMsg.InputArrayData[] inputArray = new InputMsg.InputArrayData[InputMsg.MaxInputArraySize];
	}
}
