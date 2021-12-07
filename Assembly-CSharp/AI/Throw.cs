using System;

namespace AI
{
	// Token: 0x0200031D RID: 797
	public class Throw : Leaf
	{
		// Token: 0x06001119 RID: 4377 RVA: 0x00063FF8 File Offset: 0x000623F8
		public Throw() : base(0)
		{
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0006400C File Offset: 0x0006240C
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
				if (!base.isAbleToAct || !base.player.State.IsStandardGrabbingState)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0006406C File Offset: 0x0006246C
		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				float value = (base.player.Facing != HorizontalDirection.Left) ? this.data.horizontalInput : (this.data.horizontalInput * -1f);
				this.context.AddInput(InputType.VerticalAxis, this.data.verticalInput);
				this.context.AddInput(InputType.HorizontalAxis, value);
			}
		}

		// Token: 0x04000ADA RID: 2778
		public Throw.Data data = new Throw.Data();

		// Token: 0x0200031E RID: 798
		[Serializable]
		public class Data
		{
			// Token: 0x04000ADB RID: 2779
			public float verticalInput;

			// Token: 0x04000ADC RID: 2780
			public float horizontalInput;
		}
	}
}
