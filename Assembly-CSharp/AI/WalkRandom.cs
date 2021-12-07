using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000321 RID: 801
	public class WalkRandom : Leaf
	{
		// Token: 0x06001123 RID: 4387 RVA: 0x000642F7 File Offset: 0x000626F7
		public WalkRandom(int frameDuration) : base(frameDuration)
		{
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00064300 File Offset: 0x00062700
		public override NodeResult TickFrame()
		{
			if (!this.canWalk())
			{
				return base.resultFailure;
			}
			if (this.direction != 0 && base.calculator.TestReachedEdgeOfStage(base.player, this.direction))
			{
				return base.resultFailure;
			}
			if (this.currentFrame == this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0006436B File Offset: 0x0006276B
		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0006438B File Offset: 0x0006278B
		protected override void beginRun()
		{
			this.direction = 0;
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00064394 File Offset: 0x00062794
		protected override void run()
		{
			if (this.direction == 0)
			{
				if (base.calculator.TestReachedEdgeOfStage(base.player, -1))
				{
					this.direction = 1;
				}
				else if (base.calculator.TestReachedEdgeOfStage(base.player, 1))
				{
					this.direction = -1;
				}
				else
				{
					this.direction = ((!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? -1 : 1);
				}
				this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 0.2f);
			}
			else if (this.currentFrame >= 5)
			{
				if (this.direction == 1)
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 0.5f);
				}
				else
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 0.6f);
				}
			}
			else
			{
				this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 0.2f);
			}
		}

		// Token: 0x04000AE1 RID: 2785
		private int direction;
	}
}
