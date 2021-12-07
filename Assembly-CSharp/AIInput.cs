using System;
using System.Collections.Generic;

// Token: 0x020002F8 RID: 760
public class AIInput : InputController
{
	// Token: 0x170002DA RID: 730
	// (get) Token: 0x0600109B RID: 4251 RVA: 0x0006200E File Offset: 0x0006040E
	public override bool allowTapJumping
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x0600109C RID: 4252 RVA: 0x00062011 File Offset: 0x00060411
	public override bool allowTapStrike
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x0600109D RID: 4253 RVA: 0x00062014 File Offset: 0x00060414
	public override bool allowRecoveryJumping
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x0600109E RID: 4254 RVA: 0x00062017 File Offset: 0x00060417
	public override bool requireDoubleTapToRun
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x0600109F RID: 4255 RVA: 0x0006201A File Offset: 0x0006041A
	// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00062027 File Offset: 0x00060427
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

	// Token: 0x060010A1 RID: 4257 RVA: 0x00062038 File Offset: 0x00060438
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

	// Token: 0x060010A2 RID: 4258 RVA: 0x00062084 File Offset: 0x00060484
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

	// Token: 0x060010A3 RID: 4259 RVA: 0x000620E8 File Offset: 0x000604E8
	public override void ReadInputValue(InputData inputReference, InputValue values)
	{
		this.PlayerReference.ActiveBehaviorTree.ReadInput(inputReference, values);
	}

	// Token: 0x04000A89 RID: 2697
	private AIInput.AIInputModel model = new AIInput.AIInputModel();

	// Token: 0x020002F9 RID: 761
	internal class AIInputModel
	{
		// Token: 0x04000A8A RID: 2698
		public PlayerReference player;
	}
}
