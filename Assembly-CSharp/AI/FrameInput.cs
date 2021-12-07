using System;

namespace AI
{
	// Token: 0x02000307 RID: 775
	public class FrameInput : Leaf
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x00062F49 File Offset: 0x00061349
		public FrameInput() : base(0)
		{
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00062F5D File Offset: 0x0006135D
		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00062F78 File Offset: 0x00061378
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

		// Token: 0x04000A9B RID: 2715
		public FrameInput.Data data = new FrameInput.Data();

		// Token: 0x02000308 RID: 776
		[Serializable]
		public class Data
		{
			// Token: 0x04000A9C RID: 2716
			public float verticalInput;

			// Token: 0x04000A9D RID: 2717
			public float horizontalInput;

			// Token: 0x04000A9E RID: 2718
			public bool attackButton;

			// Token: 0x04000A9F RID: 2719
			public bool specialButton;

			// Token: 0x04000AA0 RID: 2720
			public bool shieldButton;

			// Token: 0x04000AA1 RID: 2721
			public bool grabButton;

			// Token: 0x04000AA2 RID: 2722
			public bool jumpButton;
		}
	}
}
