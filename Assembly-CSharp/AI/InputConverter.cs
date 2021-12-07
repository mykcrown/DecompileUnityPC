using System;
using System.Collections.Generic;

namespace AI
{
	// Token: 0x02000337 RID: 823
	public class InputConverter : IInputConverter
	{
		// Token: 0x0600118E RID: 4494 RVA: 0x00065434 File Offset: 0x00063834
		public InputConverter()
		{
			this.tiltUp.inputType = InputType.VerticalAxis;
			this.tiltUp.value = 0.5f;
			this.tiltDown.inputType = InputType.VerticalAxis;
			this.tiltDown.value = -0.5f;
			this.pressUp.inputType = InputType.VerticalAxis;
			this.pressUp.value = 1f;
			this.pressDown.inputType = InputType.VerticalAxis;
			this.pressDown.value = -1f;
			this.pressJump.inputType = InputType.Button;
			this.pressJump.button = ButtonPress.Jump;
			this.pressB.inputType = InputType.Button;
			this.pressB.button = ButtonPress.Special;
			this.pressAttack.inputType = InputType.Button;
			this.pressAttack.button = ButtonPress.Attack;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0006556C File Offset: 0x0006396C
		public void AddMacro(ref List<Input> list, Macro macro)
		{
			switch (macro)
			{
			case Macro.UpSpecial:
				list.Add(this.pressUp);
				list.Add(this.pressB);
				break;
			case Macro.DownSpecial:
				list.Add(this.pressDown);
				list.Add(this.pressB);
				break;
			case Macro.NeutralSpecial:
				list.Add(this.pressB);
				break;
			case Macro.Jump:
				list.Add(this.pressJump);
				break;
			case Macro.DownTilt:
				list.Add(this.tiltDown);
				list.Add(this.pressAttack);
				break;
			case Macro.UpTilt:
				list.Add(this.tiltUp);
				list.Add(this.pressAttack);
				break;
			case Macro.Jab:
				list.Add(this.pressAttack);
				break;
			case Macro.DownStrike:
				list.Add(this.pressDown);
				list.Add(this.pressAttack);
				break;
			case Macro.UpStrike:
				list.Add(this.pressUp);
				list.Add(this.pressAttack);
				break;
			}
		}

		// Token: 0x04000B28 RID: 2856
		private Input pressUp = default(Input);

		// Token: 0x04000B29 RID: 2857
		private Input pressDown = default(Input);

		// Token: 0x04000B2A RID: 2858
		private Input pressJump = default(Input);

		// Token: 0x04000B2B RID: 2859
		private Input pressB = default(Input);

		// Token: 0x04000B2C RID: 2860
		private Input pressAttack = default(Input);

		// Token: 0x04000B2D RID: 2861
		private Input tiltDown = default(Input);

		// Token: 0x04000B2E RID: 2862
		private Input tiltUp = default(Input);
	}
}
