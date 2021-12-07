// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class FaceEnemy : Leaf
	{
		private PlayerReference enemy;

		public FaceEnemy() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (!this.canFace())
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
			if (this.currentFrame > 1)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		private bool canFace()
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
				num = -1;
			}
			else
			{
				num = 1;
			}
			this.context.AddInput(InputType.HorizontalAxis, (float)num * 0.6f);
		}
	}
}
