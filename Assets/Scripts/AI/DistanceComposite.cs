// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class DistanceComposite : Composite
	{
		[Serializable]
		public class Data
		{
			public Fixed minDistance;

			public Fixed maxDistance;

			public bool yAxisOnly;
		}

		private bool isActing;

		public DistanceComposite.Data data = new DistanceComposite.Data();

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
	}
}
