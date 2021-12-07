// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class FrameInput : Leaf
	{
		[Serializable]
		public class Data
		{
			public float verticalInput;

			public float horizontalInput;

			public bool attackButton;

			public bool specialButton;

			public bool shieldButton;

			public bool grabButton;

			public bool jumpButton;
		}

		public FrameInput.Data data = new FrameInput.Data();

		public FrameInput() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		protected override void run()
		{
			this.inProgress = true;
			float value = (base.player.Facing != HorizontalDirection.Left) ? this.data.horizontalInput : (-this.data.horizontalInput);
			this.context.AddInput(InputType.HorizontalAxis, value);
			this.context.AddInput(InputType.VerticalAxis, this.data.verticalInput);
			if (this.data.specialButton)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Special);
			}
			if (this.data.attackButton)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
			if (this.data.shieldButton)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Shield1);
			}
			if (this.data.grabButton)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Grab);
			}
			if (this.data.jumpButton)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Jump);
			}
		}
	}
}
