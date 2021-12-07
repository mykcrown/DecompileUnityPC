using System;
using FixedPoint;

namespace AI
{
	// Token: 0x0200032D RID: 813
	public class DistanceComposite : Composite
	{
		// Token: 0x06001177 RID: 4471 RVA: 0x00065018 File Offset: 0x00063418
		public override NodeResult TickFrame()
		{
			if (!this.canAct())
			{
				return NodeResult.Failure;
			}
			NodeResult nodeResult = base.TickFrame();
			if (nodeResult != NodeResult.Running)
			{
				this.isActing = false;
			}
			return nodeResult;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00065048 File Offset: 0x00063448
		private bool canAct()
		{
			bool flag = this.isActing;
			bool flag2 = this.isInRange();
			if (!flag && flag2)
			{
				this.isActing = true;
				this.currentIndex = -1;
			}
			return this.isActing;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00065084 File Offset: 0x00063484
		private bool isInRange()
		{
			PlayerReference playerReference = this.context.calculator.FindClosestEnemy(base.player);
			if (playerReference != null)
			{
				if (this.data.yAxisOnly)
				{
					Fixed one = FixedMath.Abs(base.player.Position.y - playerReference.Controller.Position.y);
					if (one >= this.data.minDistance && one <= this.data.maxDistance)
					{
						return true;
					}
				}
				else
				{
					Fixed magnitude = (base.player.Position - playerReference.Controller.Position).magnitude;
					if (magnitude >= this.data.minDistance && magnitude <= this.data.maxDistance)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000B13 RID: 2835
		private bool isActing;

		// Token: 0x04000B14 RID: 2836
		public DistanceComposite.Data data = new DistanceComposite.Data();

		// Token: 0x0200032E RID: 814
		[Serializable]
		public class Data
		{
			// Token: 0x04000B15 RID: 2837
			public Fixed minDistance;

			// Token: 0x04000B16 RID: 2838
			public Fixed maxDistance;

			// Token: 0x04000B17 RID: 2839
			public bool yAxisOnly;
		}
	}
}
