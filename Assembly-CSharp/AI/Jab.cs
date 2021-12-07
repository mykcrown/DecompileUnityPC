using System;

namespace AI
{
	// Token: 0x0200030F RID: 783
	public class Jab : Leaf
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x0006343C File Offset: 0x0006183C
		public Jab() : base(0)
		{
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00063450 File Offset: 0x00061850
		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.moveCompleteCount >= this.data.jabCount)
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

		// Token: 0x060010F9 RID: 4345 RVA: 0x000634B9 File Offset: 0x000618B9
		protected override void run()
		{
			if (base.isInputBreak && this.moveStartedCount < this.data.jabCount)
			{
				this.inProgress = true;
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}

		// Token: 0x04000AB2 RID: 2738
		public Jab.Data data = new Jab.Data();

		// Token: 0x02000310 RID: 784
		[Serializable]
		public class Data
		{
			// Token: 0x04000AB3 RID: 2739
			public int jabCount = 3;
		}
	}
}
