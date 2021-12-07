using System;

namespace AI
{
	// Token: 0x02000306 RID: 774
	public class FaceEnemy : Leaf
	{
		// Token: 0x060010E1 RID: 4321 RVA: 0x00062E07 File Offset: 0x00061207
		public FaceEnemy() : base(0)
		{
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00062E10 File Offset: 0x00061210
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

		// Token: 0x060010E3 RID: 4323 RVA: 0x00062EBD File Offset: 0x000612BD
		private bool canFace()
		{
			return base.isNeutral && base.player.State.IsGrounded;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00062EDD File Offset: 0x000612DD
		protected override void reset()
		{
			this.enemy = null;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00062EE8 File Offset: 0x000612E8
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

		// Token: 0x04000A9A RID: 2714
		private PlayerReference enemy;
	}
}
