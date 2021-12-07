// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class WalkRandom : Leaf
	{
		private int direction;

		public WalkRandom(int frameDuration) : base(frameDuration)
		{
		}

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

		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		protected override void beginRun()
		{
			this.direction = 0;
		}

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
					this.direction = ((!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? (-1) : 1);
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
	}
}
