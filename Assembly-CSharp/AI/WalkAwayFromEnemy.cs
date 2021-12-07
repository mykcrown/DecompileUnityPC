using System;

namespace AI
{
	// Token: 0x0200031F RID: 799
	public class WalkAwayFromEnemy : Leaf
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x00064100 File Offset: 0x00062500
		public WalkAwayFromEnemy(int frameDuration) : base(frameDuration)
		{
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00064114 File Offset: 0x00062514
		public override NodeResult TickFrame()
		{
			if (!this.canWalk())
			{
				return base.resultFailure;
			}
			this.checkForEnd();
			if (this.enemy == null)
			{
				this.enemy = base.calculator.FindClosestEnemy(base.player);
			}
			if (this.enemy == null)
			{
				return base.resultFailure;
			}
			if (this.enemy.Controller.State.IsDead || this.enemy.Controller.State.IsRespawning)
			{
				return base.resultFailure;
			}
			if (this.direction != 0 && base.calculator.TestReachedEdgeOfStage(base.player, this.direction))
			{
				return base.resultSuccess;
			}
			if (this.currentFrame == this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000641F4 File Offset: 0x000625F4
		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00064214 File Offset: 0x00062614
		protected override void reset()
		{
			this.enemy = null;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00064220 File Offset: 0x00062620
		protected override void run()
		{
			int num;
			if (this.enemy.Controller.Position.x < base.player.Position.x)
			{
				num = 1;
			}
			else
			{
				num = -1;
			}
			if (this.data.run)
			{
				this.context.AddInput(InputType.HorizontalAxis, (float)num * 1f);
			}
			else if (this.currentFrame >= 5)
			{
				if (num == 1)
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)num * 0.5f);
				}
				else
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)num * 0.6f);
				}
			}
			else
			{
				this.context.AddInput(InputType.HorizontalAxis, (float)num * 0.2f);
			}
		}

		// Token: 0x04000ADD RID: 2781
		private int direction;

		// Token: 0x04000ADE RID: 2782
		private PlayerReference enemy;

		// Token: 0x04000ADF RID: 2783
		public WalkAwayFromEnemy.Data data = new WalkAwayFromEnemy.Data();

		// Token: 0x02000320 RID: 800
		[Serializable]
		public class Data
		{
			// Token: 0x04000AE0 RID: 2784
			public bool run;
		}
	}
}
