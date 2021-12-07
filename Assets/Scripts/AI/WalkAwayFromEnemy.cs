// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class WalkAwayFromEnemy : Leaf
	{
		[Serializable]
		public class Data
		{
			public bool run;
		}

		private int direction;

		private PlayerReference enemy;

		public WalkAwayFromEnemy.Data data = new WalkAwayFromEnemy.Data();

		public WalkAwayFromEnemy(int frameDuration) : base(frameDuration)
		{
		}

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

		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		protected override void reset()
		{
			this.enemy = null;
		}

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
	}
}
