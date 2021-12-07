// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace AI
{
	public class InputConverter : IInputConverter
	{
		private Input pressUp = default(Input);

		private Input pressDown = default(Input);

		private Input pressJump = default(Input);

		private Input pressB = default(Input);

		private Input pressAttack = default(Input);

		private Input tiltDown = default(Input);

		private Input tiltUp = default(Input);

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
	}
}
