// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class Special : Leaf
	{
		[Serializable]
		public class Data
		{
			public float verticalInput;

			public float horizontalInput;

			public int moveCount = 1;
		}

		public Special.Data data = new Special.Data();

		public Special() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.moveCompleteCount >= this.data.moveCount)
				{
					return base.resultSuccess;
				}
				return base.resultRunning;
			}
			else
			{
				if (!base.isAbleToAct)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		protected override void run()
		{
			if (base.isInputBreak && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				float value = (base.player.Facing != HorizontalDirection.Left) ? this.data.horizontalInput : (-this.data.horizontalInput);
				this.context.AddInput(InputType.HorizontalAxis, value);
				this.context.AddInput(InputType.VerticalAxis, this.data.verticalInput);
				this.context.AddInput(InputType.Button, ButtonPress.Special);
			}
		}
	}
}
