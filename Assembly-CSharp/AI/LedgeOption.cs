using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000314 RID: 788
	public class LedgeOption : Leaf
	{
		// Token: 0x06001101 RID: 4353 RVA: 0x00063754 File Offset: 0x00061B54
		public LedgeOption() : base(0)
		{
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00063768 File Offset: 0x00061B68
		public override NodeResult TickFrame()
		{
			if (!this.shouldLedgeOption())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00063782 File Offset: 0x00061B82
		private bool shouldLedgeOption()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsDead && base.player.State.IsLedgeHanging;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000637C1 File Offset: 0x00061BC1
		protected override void beginRun()
		{
			this.decidedOption = LedgeOption.Option.Undecided;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x000637CC File Offset: 0x00061BCC
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
						this.context.AddInput(InputType.HorizontalAxis, (float)((!(base.player.Physics.Center.x < 0)) ? -1 : 1));
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
						this.context.AddInput(InputType.HorizontalAxis, (float)((!(base.player.Physics.Center.x < 0)) ? 1 : -1));
						break;
					}
				}
			}
		}

		// Token: 0x04000ABF RID: 2751
		private LedgeOption.Option decidedOption;

		// Token: 0x04000AC0 RID: 2752
		public LedgeOption.Data data = new LedgeOption.Data();

		// Token: 0x02000315 RID: 789
		[Serializable]
		public class Data
		{
			// Token: 0x04000AC1 RID: 2753
			public Fixed rngWeight_Attack = 10;

			// Token: 0x04000AC2 RID: 2754
			public Fixed rngWeight_Stand = 10;

			// Token: 0x04000AC3 RID: 2755
			public Fixed rngWeight_Jump = 10;

			// Token: 0x04000AC4 RID: 2756
			public Fixed rngWeight_Roll = 10;

			// Token: 0x04000AC5 RID: 2757
			public Fixed rngWeight_Drop = 10;
		}

		// Token: 0x02000316 RID: 790
		private enum Option
		{
			// Token: 0x04000AC7 RID: 2759
			Undecided,
			// Token: 0x04000AC8 RID: 2760
			Stand,
			// Token: 0x04000AC9 RID: 2761
			Attack,
			// Token: 0x04000ACA RID: 2762
			Jump,
			// Token: 0x04000ACB RID: 2763
			Roll,
			// Token: 0x04000ACC RID: 2764
			Drop
		}
	}
}
