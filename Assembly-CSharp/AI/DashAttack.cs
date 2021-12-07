using System;

namespace AI
{
	// Token: 0x02000303 RID: 771
	public class DashAttack : Leaf
	{
		// Token: 0x060010D6 RID: 4310 RVA: 0x00062C41 File Offset: 0x00061041
		public DashAttack() : base(0)
		{
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00062C4A File Offset: 0x0006104A
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
				if (!this.canDashAttack())
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00062C89 File Offset: 0x00061089
		private bool canDashAttack()
		{
			return base.isAbleToAct && base.player.State.IsGrounded;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00062CA9 File Offset: 0x000610A9
		protected override void reset()
		{
			this.direction = 0;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00062CB4 File Offset: 0x000610B4
		protected override void run()
		{
			if (this.direction == 0)
			{
				this.direction = ((base.player.Facing != HorizontalDirection.Left) ? 1 : -1);
			}
			this.inProgress = true;
			this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 1f);
			if (this.currentFrame >= 5)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}

		// Token: 0x04000A99 RID: 2713
		private int direction;
	}
}
