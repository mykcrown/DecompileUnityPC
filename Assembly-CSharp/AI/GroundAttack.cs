using System;

namespace AI
{
	// Token: 0x0200030D RID: 781
	public class GroundAttack : Leaf
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x0006332C File Offset: 0x0006172C
		public GroundAttack() : base(0)
		{
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00063340 File Offset: 0x00061740
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
				if (!base.isAbleToAct || !base.player.State.IsGrounded)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000633A0 File Offset: 0x000617A0
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

		// Token: 0x04000AAF RID: 2735
		public GroundAttack.Data data = new GroundAttack.Data();

		// Token: 0x0200030E RID: 782
		[Serializable]
		public class Data
		{
			// Token: 0x04000AB0 RID: 2736
			public float verticalInput;

			// Token: 0x04000AB1 RID: 2737
			public float horizontalInput;
		}
	}
}
