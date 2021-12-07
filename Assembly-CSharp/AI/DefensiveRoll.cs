using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000304 RID: 772
	public class DefensiveRoll : Leaf
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00062D23 File Offset: 0x00061123
		public DefensiveRoll() : base(30)
		{
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00062D2D File Offset: 0x0006112D
		public override NodeResult TickFrame()
		{
			if (!this.canRoll())
			{
				return base.resultFailure;
			}
			if (this.currentFrame >= this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00062D5F File Offset: 0x0006115F
		private bool canRoll()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00062D80 File Offset: 0x00061180
		protected override void run()
		{
			if (base.isInputBreak)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Shield1);
				float value = (float)((!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? 1 : -1);
				this.context.AddInput(InputType.HorizontalAxis, value);
			}
		}
	}
}
