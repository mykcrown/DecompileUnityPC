using System;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
[Serializable]
public class InputStateSnapshot : RollbackStateTyped<InputStateSnapshot>
{
	// Token: 0x06002A12 RID: 10770 RVA: 0x000DE398 File Offset: 0x000DC798
	public InputStateSnapshot()
	{
		this.init(2, InputStateSnapshot.INPUT_COUNT);
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000DE3AC File Offset: 0x000DC7AC
	public InputStateSnapshot(int bufferSize, int inputCount)
	{
		if (inputCount != InputStateSnapshot.INPUT_COUNT)
		{
			throw new Exception(string.Concat(new object[]
			{
				"Created Input state with incorrect length ",
				inputCount,
				", should be ",
				InputStateSnapshot.INPUT_COUNT
			}));
		}
		this.init(bufferSize, inputCount);
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000DE40C File Offset: 0x000DC80C
	public override void CopyTo(InputStateSnapshot target)
	{
		for (int i = 0; i < this.inputBuffer.Length; i++)
		{
			for (int j = 0; j < this.inputBuffer[i].Length; j++)
			{
				if (target.inputBuffer[i][j] != null)
				{
					target.inputBuffer[i][j].Load(this.inputBuffer[i][j]);
				}
			}
		}
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000DE478 File Offset: 0x000DC878
	public override object Clone()
	{
		InputStateSnapshot inputStateSnapshot = new InputStateSnapshot(this.inputBuffer.Length, this.inputBuffer[0].Length);
		this.CopyTo(inputStateSnapshot);
		return inputStateSnapshot;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000DE4A8 File Offset: 0x000DC8A8
	private void init(int bufferSize, int inputCount)
	{
		this.inputBuffer = new InputState[bufferSize][];
		for (int i = 0; i < bufferSize; i++)
		{
			this.inputBuffer[i] = new InputState[inputCount];
			for (int j = 0; j < inputCount; j++)
			{
				this.inputBuffer[i][j] = new InputState();
			}
		}
	}

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x06002A17 RID: 10775 RVA: 0x000DE502 File Offset: 0x000DC902
	public InputState[] PreviousInputs
	{
		get
		{
			return this.GetInputs(1);
		}
	}

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x06002A18 RID: 10776 RVA: 0x000DE50B File Offset: 0x000DC90B
	public InputState[] Inputs
	{
		get
		{
			return this.GetInputs(0);
		}
	}

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06002A19 RID: 10777 RVA: 0x000DE514 File Offset: 0x000DC914
	public int BufferSize
	{
		get
		{
			return this.inputBuffer.Length;
		}
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x000DE51E File Offset: 0x000DC91E
	public InputState[] GetInputs(int framesPrevious)
	{
		if (this.inputBuffer.Length <= framesPrevious)
		{
			Debug.LogError("No inputs for <" + framesPrevious + "> frames ago. Could need to make the buffer larger.");
			return null;
		}
		return this.inputBuffer[framesPrevious];
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x000DE554 File Offset: 0x000DC954
	public void RecordInput(InputValue value, int index)
	{
		for (int i = this.inputBuffer.Length - 1; i >= 1; i--)
		{
			InputState[] array = this.inputBuffer[i];
			InputState[] array2 = this.inputBuffer[i - 1];
			if (array[index] == null)
			{
				array[index] = new InputState();
			}
			array[index].Load(array2[index]);
		}
		this.Inputs[index].LoadValue(value);
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x000DE5BC File Offset: 0x000DC9BC
	public override void Clear()
	{
		base.Clear();
		for (int i = 0; i < this.inputBuffer.Length; i++)
		{
			for (int j = 0; j < this.inputBuffer[i].Length; j++)
			{
				this.inputBuffer[i][j].Clear();
			}
		}
	}

	// Token: 0x04001E36 RID: 7734
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private InputState[][] inputBuffer;

	// Token: 0x04001E37 RID: 7735
	public static int INPUT_COUNT = 42;
}
