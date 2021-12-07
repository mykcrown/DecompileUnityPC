// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class GetupOption : Leaf
	{
		[Serializable]
		public class Data
		{
			public Fixed rngWeight_Attack = 9;

			public Fixed rngWeight_Left = 6;

			public Fixed rngWeight_Right = 6;

			public Fixed rngWeight_Stand = 13;
		}

		private enum Option
		{
			Undecided,
			Stand,
			Attack,
			Left,
			Right
		}

		private GetupOption.Option decidedOption;

		public GetupOption.Data data = new GetupOption.Data();

		public GetupOption() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (!this.shouldGetup())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		private bool shouldGetup()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsDead && base.player.State.IsDownedLooping;
		}

		protected override void beginRun()
		{
			this.decidedOption = GetupOption.Option.Undecided;
		}

		protected override void run()
		{
			if (base.reactionSpeedCheck)
			{
				if (this.decidedOption == GetupOption.Option.Undecided)
				{
					Fixed one = this.context.GenerateRandomNumber();
					Fixed other = this.data.rngWeight_Attack + this.data.rngWeight_Left + this.data.rngWeight_Right + this.data.rngWeight_Stand;
					Fixed @fixed = this.data.rngWeight_Attack / other;
					Fixed fixed2 = @fixed + this.data.rngWeight_Left / other;
					Fixed other2 = fixed2 + this.data.rngWeight_Right / other;
					if (one < @fixed)
					{
						this.decidedOption = GetupOption.Option.Attack;
					}
					else if (one < fixed2)
					{
						this.decidedOption = GetupOption.Option.Left;
					}
					else if (one < other2)
					{
						this.decidedOption = GetupOption.Option.Right;
					}
					else
					{
						this.decidedOption = GetupOption.Option.Stand;
					}
				}
				if (base.isInputBreak)
				{
					switch (this.decidedOption)
					{
					case GetupOption.Option.Stand:
						this.context.AddInput(InputType.VerticalAxis, 1f);
						break;
					case GetupOption.Option.Attack:
						this.context.AddInput(InputType.Button, ButtonPress.Attack);
						break;
					case GetupOption.Option.Left:
						this.context.AddInput(InputType.HorizontalAxis, -1f);
						break;
					case GetupOption.Option.Right:
						this.context.AddInput(InputType.HorizontalAxis, 1f);
						break;
					}
				}
			}
		}
	}
}
