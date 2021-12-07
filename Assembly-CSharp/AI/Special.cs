using System;

namespace AI
{
	// Token: 0x0200031B RID: 795
	public class Special : Leaf
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x00063EF8 File Offset: 0x000622F8
		public Special() : base(0)
		{
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00063F0C File Offset: 0x0006230C
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

		// Token: 0x06001117 RID: 4375 RVA: 0x00063F60 File Offset: 0x00062360
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

		// Token: 0x04000AD6 RID: 2774
		public Special.Data data = new Special.Data();

		// Token: 0x0200031C RID: 796
		[Serializable]
		public class Data
		{
			// Token: 0x04000AD7 RID: 2775
			public float verticalInput;

			// Token: 0x04000AD8 RID: 2776
			public float horizontalInput;

			// Token: 0x04000AD9 RID: 2777
			public int moveCount = 1;
		}
	}
}
