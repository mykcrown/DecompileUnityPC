// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ForwardingInput : InputController
{
	public PlayerReference target;

	public override bool allowTapJumping
	{
		get
		{
			return this.target.InputController.allowTapJumping;
		}
	}

	public override bool allowTapStrike
	{
		get
		{
			return this.target.InputController.allowTapStrike;
		}
	}

	public override bool allowRecoveryJumping
	{
		get
		{
			return this.target.InputController.allowRecoveryJumping;
		}
	}

	public override bool requireDoubleTapToRun
	{
		get
		{
			return this.target.InputController.requireDoubleTapToRun;
		}
	}

	public PlayerReference PlayerReference
	{
		get
		{
			return this.target;
		}
	}

	public override bool usesBinding(DefaultInputBinding binding, List<InputData> inputList)
	{
		return this.target.InputController.usesBinding(binding, inputList);
	}

	public override void ReadAllInputs(ref InputValuesSnapshot values, InputValue valueBuffer, bool tauntsOnly)
	{
		this.target.InputController.ReadAllInputs(ref values, valueBuffer, tauntsOnly);
	}

	public override void ReadInputValue(InputData inputReference, InputValue values)
	{
		this.target.InputController.ReadInputValue(inputReference, values);
	}
}
