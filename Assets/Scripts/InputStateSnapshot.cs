// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class InputStateSnapshot : RollbackStateTyped<InputStateSnapshot>
{
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	private InputState[][] inputBuffer;

	public static int INPUT_COUNT = 42;

	public InputState[] PreviousInputs
	{
		get
		{
			return this.GetInputs(1);
		}
	}

	public InputState[] Inputs
	{
		get
		{
			return this.GetInputs(0);
		}
	}

	public int BufferSize
	{
		get
		{
			return this.inputBuffer.Length;
		}
	}

	public InputStateSnapshot()
	{
		this.init(2, InputStateSnapshot.INPUT_COUNT);
	}

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

	public override object Clone()
	{
		InputStateSnapshot inputStateSnapshot = new InputStateSnapshot(this.inputBuffer.Length, this.inputBuffer[0].Length);
		this.CopyTo(inputStateSnapshot);
		return inputStateSnapshot;
	}

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

	public InputState[] GetInputs(int framesPrevious)
	{
		if (this.inputBuffer.Length <= framesPrevious)
		{
			UnityEngine.Debug.LogError("No inputs for <" + framesPrevious + "> frames ago. Could need to make the buffer larger.");
			return null;
		}
		return this.inputBuffer[framesPrevious];
	}

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
}
