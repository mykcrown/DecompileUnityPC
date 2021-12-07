// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class AIInput : InputController
{
	internal class AIInputModel
	{
		public PlayerReference player;
	}

	private AIInput.AIInputModel model = new AIInput.AIInputModel();

	public override bool allowTapJumping
	{
		get
		{
			return false;
		}
	}

	public override bool allowTapStrike
	{
		get
		{
			return true;
		}
	}

	public override bool allowRecoveryJumping
	{
		get
		{
			return false;
		}
	}

	public override bool requireDoubleTapToRun
	{
		get
		{
			return false;
		}
	}

	public PlayerReference PlayerReference
	{
		get
		{
			return this.model.player;
		}
		set
		{
			this.model.player = value;
		}
	}

	public override bool usesBinding(DefaultInputBinding binding, List<InputData> inputList)
	{
		for (int i = 0; i < inputList.Count; i++)
		{
			InputData inputData = inputList[i];
			if (inputData.inputType == InputType.Button && binding.button == inputData.button)
			{
				return false;
			}
		}
		return true;
	}

	public override void ReadAllInputs(ref InputValuesSnapshot values, InputValue valueBuffer, bool tauntsOnly)
	{
		if (!tauntsOnly)
		{
			for (int i = 0; i < base.allInputData.Count; i++)
			{
				InputData inputData = base.allInputData[i];
				if (!InputController.IsMetagameInput(inputData.button, false))
				{
					valueBuffer.Clear();
					this.ReadInputValue(inputData, valueBuffer);
					values.SetValue(inputData, valueBuffer);
				}
			}
		}
	}

	public override void ReadInputValue(InputData inputReference, InputValue values)
	{
		this.PlayerReference.ActiveBehaviorTree.ReadInput(inputReference, values);
	}
}
