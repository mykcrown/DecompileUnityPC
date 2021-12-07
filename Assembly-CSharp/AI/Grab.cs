using System;

namespace AI
{
	// Token: 0x0200030C RID: 780
	public class Grab : Leaf
	{
		// Token: 0x060010F0 RID: 4336 RVA: 0x000632AD File Offset: 0x000616AD
		public Grab() : base(0)
		{
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000632B6 File Offset: 0x000616B6
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
				if (!base.isAbleToAct)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000632F5 File Offset: 0x000616F5
		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				this.context.AddInput(InputType.Button, ButtonPress.Grab);
			}
		}
	}
}
