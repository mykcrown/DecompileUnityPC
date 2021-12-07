// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class WalkTowardEnemy : Leaf
	{
		[Serializable]
		public class Data
		{
			public Fixed stopDistance = 2;

			public bool run;
		}

		private PlayerReference enemy;

		public WalkTowardEnemy.Data data = new WalkTowardEnemy.Data();

		public WalkTowardEnemy(int frameDuration) : base(frameDuration)
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

		private bool canWalk()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		protected override void reset()
		{
			this.enemy = null;
		}

		private int getDirection()
		{
			if (this.enemy.Controller.Position.x < base.player.Position.x)
			{
				return -1;
			}
			return 1;
		}

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
	}
}
