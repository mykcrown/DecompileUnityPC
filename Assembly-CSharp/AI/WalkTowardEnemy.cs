using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000322 RID: 802
	public class WalkTowardEnemy : Leaf
	{
		// Token: 0x06001128 RID: 4392 RVA: 0x000644AD File Offset: 0x000628AD
		public WalkTowardEnemy(int frameDuration) : base(frameDuration)
		{
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000644C4 File Offset: 0x000628C4
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
			if (base.calculator.TestReachedEdgeOfStage(base.player, this.getDirection()))
			{
				return base.resultFailure;
			}
			Fixed magnitude = (base.player.Position - this.enemy.Controller.Position).magnitude;
			if (magnitude <= this.data.stopDistance)
			{
				return base.resultSuccess;
			}
			if (this.currentFrame == this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x000645DF File Offset: 0x000629DF
		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000645FF File Offset: 0x000629FF
		protected override void reset()
		{
			this.enemy = null;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00064608 File Offset: 0x00062A08
		private int getDirection()
		{
			if (this.enemy.Controller.Position.x < base.player.Position.x)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00064650 File Offset: 0x00062A50
		protected override void run()
		{
			int direction = this.getDirection();
			if (this.data.run)
			{
				this.context.AddInput(InputType.HorizontalAxis, (float)direction * 1f);
			}
			else if (this.currentFrame >= 5)
			{
				if (direction == 1)
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)direction * 0.5f);
				}
				else
				{
					this.context.AddInput(InputType.HorizontalAxis, (float)direction * 0.6f);
				}
			}
			else
			{
				this.context.AddInput(InputType.HorizontalAxis, (float)direction * 0.2f);
			}
		}

		// Token: 0x04000AE2 RID: 2786
		private PlayerReference enemy;

		// Token: 0x04000AE3 RID: 2787
		public WalkTowardEnemy.Data data = new WalkTowardEnemy.Data();

		// Token: 0x02000323 RID: 803
		[Serializable]
		public class Data
		{
			// Token: 0x04000AE4 RID: 2788
			public Fixed stopDistance = 2;

			// Token: 0x04000AE5 RID: 2789
			public bool run;
		}
	}
}
