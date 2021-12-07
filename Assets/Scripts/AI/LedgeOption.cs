// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class LedgeOption : Leaf
	{
		[Serializable]
		public class Data
		{
			public Fixed rngWeight_Attack = 10;

			public Fixed rngWeight_Stand = 10;

			public Fixed rngWeight_Jump = 10;

			public Fixed rngWeight_Roll = 10;

			public Fixed rngWeight_Drop = 10;
		}

		private enum Option
		{
			Undecided,
			Stand,
			Attack,
			Jump,
			Roll,
			Drop
		}

		private LedgeOption.Option decidedOption;

		public LedgeOption.Data data = new LedgeOption.Data();

		public LedgeOption() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (!this.shouldLedgeOption())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		private bool shouldLedgeOption()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsDead && base.player.State.IsLedgeHanging;
		}

		protected override void beginRun()
		{
			this.decidedOption = LedgeOption.Option.Undecided;
		}

		protected override void run()
		{
			if (base.reactionSpeedCheck)
			{
				if (this.decidedOption == LedgeOption.Option.Undecided)
				{
					Fixed one = this.context.GenerateRandomNumber();
					Fixed other = this.data.rngWeight_Attack + this.data.rngWeight_Stand + this.data.rngWeight_Jump + this.data.rngWeight_Roll + this.data.rngWeight_Drop;
					Fixed @fixed = this.data.rngWeight_Attack / other;
					Fixed fixed2 = @fixed + this.data.rngWeight_Stand / other;
					Fixed fixed3 = fixed2 + this.data.rngWeight_Jump / other;
					Fixed other2 = fixed3 + this.data.rngWeight_Roll / other;
					if (one < @fixed)
					{
						this.decidedOption = LedgeOption.Option.Attack;
					}
					else if (one < fixed2)
					{
						this.decidedOption = LedgeOption.Option.Stand;
					}
					else if (one < fixed3)
					{
						this.decidedOption = LedgeOption.Option.Jump;
					}
					else if (one < other2)
					{
						this.decidedOption = LedgeOption.Option.Roll;
					}
					else
					{
						this.decidedOption = LedgeOption.Option.Drop;
					}
				}
				if (base.isInputBreak)
				{
					switch (this.decidedOption)
					{
					case LedgeOption.Option.Stand:
						this.context.AddInput(InputType.HorizontalAxis, (float)((!(base.player.Physics.Center.x < 0)) ? (-1) : 1));
						break;
					case LedgeOption.Option.Attack:
						this.context.AddInput(InputType.Button, ButtonPress.Attack);
						break;
					case LedgeOption.Option.Jump:
						this.context.AddInput(InputType.Button, ButtonPress.Jump);
						break;
					case LedgeOption.Option.Roll:
						this.context.AddInput(InputType.Button, ButtonPress.Shield1);
						break;
					case LedgeOption.Option.Drop:
						this.context.AddInput(InputType.HorizontalAxis, (float)((!(base.player.Physics.Center.x < 0)) ? 1 : (-1)));
						break;
					}
				}
			}
		}
	}
}
