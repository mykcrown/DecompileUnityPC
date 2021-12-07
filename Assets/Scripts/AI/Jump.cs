// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class Jump : Leaf
	{
		[Serializable]
		public class Data
		{
			public Fixed rngWeight_Up = 10;

			public Fixed rngWeight_Forward = 10;

			public Fixed rngWeight_Back = 10;

			public bool shortHop;
		}

		private enum Option
		{
			Undecided,
			Forward,
			Back,
			Up
		}

		private Jump.Option decidedOption;

		public Jump.Data data = new Jump.Data();

		public Jump() : base(0)
		{
		}

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

		private bool canJump()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

		protected override void beginRun()
		{
			this.decidedOption = Jump.Option.Undecided;
		}

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
	}
}
