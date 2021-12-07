// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class AirAttack : Leaf
	{
		[Serializable]
		public class Data
		{
			public float verticalInput;

			public float horizontalInput;
		}

		public AirAttack.Data data = new AirAttack.Data();

		public AirAttack() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.moveCompleteCount >= 1)
				{
					return base.resultSuccess;
				}
				return base.resultRunning;
			}
			else
			{
				if (!base.isAbleToAct || base.player.State.IsGrounded)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				float value = (base.player.Facing != HorizontalDirection.Left) ? this.data.horizontalInput : (-this.data.horizontalInput);
				this.context.AddInput(InputType.HorizontalAxis, value);
				this.context.AddInput(InputType.VerticalAxis, this.data.verticalInput);
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}
	}
}
