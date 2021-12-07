using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000311 RID: 785
	public class Jump : Leaf
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x000634FF File Offset: 0x000618FF
		public Jump() : base(0)
		{
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00063514 File Offset: 0x00061914
		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.currentFrame >= 30)
				{
					return base.resultFailure;
				}
				if (base.player.State.IsJumpingState)
				{
					return base.resultSuccess;
				}
				return base.resultRunning;
			}
			else
			{
				if (!this.canJump())
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0006357B File Offset: 0x0006197B
		private bool canJump()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0006359B File Offset: 0x0006199B
		protected override void beginRun()
		{
			this.decidedOption = Jump.Option.Undecided;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000635A4 File Offset: 0x000619A4
		protected override void run()
		{
			if (base.isInputBreak)
			{
				this.inProgress = true;
				if (this.decidedOption == Jump.Option.Undecided)
				{
					Fixed one = this.context.GenerateRandomNumber();
					Fixed other = this.data.rngWeight_Up + this.data.rngWeight_Forward + this.data.rngWeight_Back;
					Fixed @fixed = this.data.rngWeight_Up / other;
					Fixed other2 = @fixed + this.data.rngWeight_Forward / other;
					if (one < @fixed)
					{
						this.decidedOption = Jump.Option.Up;
					}
					else if (one < other2)
					{
						this.decidedOption = Jump.Option.Forward;
					}
					else
					{
						this.decidedOption = Jump.Option.Back;
					}
				}
				if (base.player.Facing == HorizontalDirection.Left)
				{
					if (this.decidedOption == Jump.Option.Forward)
					{
						this.context.AddInput(InputType.HorizontalAxis, -1f);
					}
					else if (this.decidedOption == Jump.Option.Back)
					{
						this.context.AddInput(InputType.HorizontalAxis, 1f);
					}
				}
				else if (this.decidedOption == Jump.Option.Forward)
				{
					this.context.AddInput(InputType.HorizontalAxis, 1f);
				}
				else if (this.decidedOption == Jump.Option.Back)
				{
					this.context.AddInput(InputType.HorizontalAxis, -1f);
				}
				if (!this.data.shortHop || this.currentFrame <= 3)
				{
					this.context.AddInput(InputType.Button, ButtonPress.Jump);
				}
			}
		}

		// Token: 0x04000AB4 RID: 2740
		private Jump.Option decidedOption;

		// Token: 0x04000AB5 RID: 2741
		public Jump.Data data = new Jump.Data();

		// Token: 0x02000312 RID: 786
		[Serializable]
		public class Data
		{
			// Token: 0x04000AB6 RID: 2742
			public Fixed rngWeight_Up = 10;

			// Token: 0x04000AB7 RID: 2743
			public Fixed rngWeight_Forward = 10;

			// Token: 0x04000AB8 RID: 2744
			public Fixed rngWeight_Back = 10;

			// Token: 0x04000AB9 RID: 2745
			public bool shortHop;
		}

		// Token: 0x02000313 RID: 787
		private enum Option
		{
			// Token: 0x04000ABB RID: 2747
			Undecided,
			// Token: 0x04000ABC RID: 2748
			Forward,
			// Token: 0x04000ABD RID: 2749
			Back,
			// Token: 0x04000ABE RID: 2750
			Up
		}
	}
}
