using System;
using UnityEngine;

namespace AI
{
	// Token: 0x02000301 RID: 769
	public class AttackTowardEnemy : Leaf
	{
		// Token: 0x060010D1 RID: 4305 RVA: 0x00062A8C File Offset: 0x00060E8C
		public AttackTowardEnemy() : base(0)
		{
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00062AA0 File Offset: 0x00060EA0
		public override NodeResult TickFrame()
		{
			if (!base.isAbleToAct)
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
			if (this.moveCompleteCount >= 1)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00062B4D File Offset: 0x00060F4D
		protected override void reset()
		{
			this.enemy = null;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00062B58 File Offset: 0x00060F58
		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				Vector2 from = (Vector2)(this.enemy.Controller.Position - base.player.Position);
				float num = Vector2.Angle(from, Vector2.right);
				if (from.y < 0f)
				{
					num *= -1f;
				}
				float num2 = Mathf.Cos(num * 0.017453292f);
				float num3 = Mathf.Sin(num * 0.017453292f);
				float num4 = (!this.data.strike) ? 0.5f : 1f;
				this.context.AddInput(InputType.HorizontalAxis, num2 * num4);
				this.context.AddInput(InputType.VerticalAxis, num3 * num4);
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}

		// Token: 0x04000A96 RID: 2710
		private PlayerReference enemy;

		// Token: 0x04000A97 RID: 2711
		public AttackTowardEnemy.Data data = new AttackTowardEnemy.Data();

		// Token: 0x02000302 RID: 770
		[Serializable]
		public class Data
		{
			// Token: 0x04000A98 RID: 2712
			public bool strike;
		}
	}
}
