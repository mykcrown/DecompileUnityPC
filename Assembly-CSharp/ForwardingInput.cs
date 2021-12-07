using System;
using System.Collections.Generic;

// Token: 0x020002FA RID: 762
public class ForwardingInput : InputController
{
	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0006210C File Offset: 0x0006050C
	public override bool allowTapJumping
	{
		get
		{
			return this.target.InputController.allowTapJumping;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060010A7 RID: 4263 RVA: 0x0006211E File Offset: 0x0006051E
	public override bool allowTapStrike
	{
		get
		{
			return this.target.InputController.allowTapStrike;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060010A8 RID: 4264 RVA: 0x00062130 File Offset: 0x00060530
	public override bool allowRecoveryJumping
	{
		get
		{
			return this.target.InputController.allowRecoveryJumping;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x00062142 File Offset: 0x00060542
	public override bool requireDoubleTapToRun
	{
		get
		{
			return this.target.InputController.requireDoubleTapToRun;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x060010AA RID: 4266 RVA: 0x00062154 File Offset: 0x00060554
	public PlayerReference PlayerReference
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0006215C File Offset: 0x0006055C
	public override bool usesBinding(DefaultInputBinding binding, List<InputData> inputList)
	{
		return this.target.InputController.usesBinding(binding, inputList);
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x00062170 File Offset: 0x00060570
	public override void ReadAllInputs(ref InputValuesSnapshot values, InputValue valueBuffer, bool tauntsOnly)
	{
		this.target.InputController.ReadAllInputs(ref values, valueBuffer, tauntsOnly);
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x00062185 File Offset: 0x00060585
	public override void ReadInputValue(InputData inputReference, InputValue values)
	{
		this.target.InputController.ReadInputValue(inputReference, values);
	}

	// Token: 0x04000A8B RID: 2699
	public PlayerReference target;
}
