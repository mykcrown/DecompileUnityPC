using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000309 RID: 777
	public class GetupOption : Leaf
	{
		// Token: 0x060010EA RID: 4330 RVA: 0x00063078 File Offset: 0x00061478
		public GetupOption() : base(0)
		{
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0006308C File Offset: 0x0006148C
		public override NodeResult TickFrame()
		{
			if (!this.shouldGetup())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x000630A6 File Offset: 0x000614A6
		private bool shouldGetup()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsDead && base.player.State.IsDownedLooping;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x000630E5 File Offset: 0x000614E5
		protected override void beginRun()
		{
			this.decidedOption = GetupOption.Option.Undecided;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000630F0 File Offset: 0x000614F0
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

		// Token: 0x04000AA3 RID: 2723
		private GetupOption.Option decidedOption;

		// Token: 0x04000AA4 RID: 2724
		public GetupOption.Data data = new GetupOption.Data();

		// Token: 0x0200030A RID: 778
		[Serializable]
		public class Data
		{
			// Token: 0x04000AA5 RID: 2725
			public Fixed rngWeight_Attack = 9;

			// Token: 0x04000AA6 RID: 2726
			public Fixed rngWeight_Left = 6;

			// Token: 0x04000AA7 RID: 2727
			public Fixed rngWeight_Right = 6;

			// Token: 0x04000AA8 RID: 2728
			public Fixed rngWeight_Stand = 13;
		}

		// Token: 0x0200030B RID: 779
		private enum Option
		{
			// Token: 0x04000AAA RID: 2730
			Undecided,
			// Token: 0x04000AAB RID: 2731
			Stand,
			// Token: 0x04000AAC RID: 2732
			Attack,
			// Token: 0x04000AAD RID: 2733
			Left,
			// Token: 0x04000AAE RID: 2734
			Right
		}
	}
}
