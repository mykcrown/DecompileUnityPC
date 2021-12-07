// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AI
{
	public class AttackTowardEnemy : Leaf
	{
		[Serializable]
		public class Data
		{
			public bool strike;
		}

		private PlayerReference enemy;

		public AttackTowardEnemy.Data data = new AttackTowardEnemy.Data();

		public AttackTowardEnemy() : base(0)
		{
		}

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

		protected override void reset()
		{
			this.enemy = null;
		}

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
				float num2 = Mathf.Cos(num * 0.0174532924f);
				float num3 = Mathf.Sin(num * 0.0174532924f);
				float num4 = (!this.data.strike) ? 0.5f : 1f;
				this.context.AddInput(InputType.HorizontalAxis, num2 * num4);
				this.context.AddInput(InputType.VerticalAxis, num3 * num4);
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}
	}
}
