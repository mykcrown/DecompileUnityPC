using System;

namespace AI
{
	// Token: 0x020002FF RID: 767
	public class AirAttack : Leaf
	{
		// Token: 0x060010CD RID: 4301 RVA: 0x0006297C File Offset: 0x00060D7C
		public AirAttack() : base(0)
		{
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00062990 File Offset: 0x00060D90
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
				if (!base.isAbleToAct || base.player.State.IsGrounded)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000629F0 File Offset: 0x00060DF0
		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				float value = (base.player.Facing != HorizontalDirection.Left) ? this.data.horizontalInput : (-this.data.horizontalInput);
				this.context.AddInput(InputType.HorizontalAxis, value);
				this.context.AddInput(InputType.VerticalAxis, this.data.verticalInput);
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}

		// Token: 0x04000A93 RID: 2707
		public AirAttack.Data data = new AirAttack.Data();

		// Token: 0x02000300 RID: 768
		[Serializable]
		public class Data
		{
			// Token: 0x04000A94 RID: 2708
			public float verticalInput;

			// Token: 0x04000A95 RID: 2709
			public float horizontalInput;
		}
	}
}
