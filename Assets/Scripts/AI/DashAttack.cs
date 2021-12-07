// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class DashAttack : Leaf
	{
		private int direction;

		public DashAttack() : base(0)
		{
		}

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

		private bool canDashAttack()
		{
			return base.isAbleToAct && base.player.State.IsGrounded;
		}

		protected override void reset()
		{
			this.direction = 0;
		}

		protected override void run()
		{
			if (this.direction == 0)
			{
				this.direction = ((base.player.Facing != HorizontalDirection.Left) ? 1 : (-1));
			}
			this.inProgress = true;
			this.context.AddInput(InputType.HorizontalAxis, (float)this.direction * 1f);
			if (this.currentFrame >= 5)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}
	}
}
