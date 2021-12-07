using System;
using System.Collections.Generic;
using FixedPoint;

namespace AI
{
	// Token: 0x0200032B RID: 811
	public class DefensiveOptionComposite : Composite
	{
		// Token: 0x06001172 RID: 4466 RVA: 0x00064DD4 File Offset: 0x000631D4
		public override NodeResult TickFrame()
		{
			if (!this.canDefend())
			{
				return NodeResult.Failure;
			}
			NodeResult nodeResult = base.TickFrame();
			if (nodeResult != NodeResult.Running)
			{
				this.useDefense = false;
			}
			return nodeResult;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00064E04 File Offset: 0x00063204
		private bool canDefend()
		{
			bool flag = this.enemyNearbyState;
			this.enemyNearbyState = this.isEnemyNearby();
			if (!flag && this.enemyNearbyState)
			{
				this.useDefense = (this.context.GenerateRandomNumber() < this.data.defendChance);
				this.currentIndex = -1;
			}
			return this.useDefense;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00064E70 File Offset: 0x00063270
		private bool isEnemyNearby()
		{
			List<PlayerReference> playerReferences = this.context.gameManager.PlayerReferences;
			for (int i = 0; i < playerReferences.Count; i++)
			{
				PlayerReference playerReference = playerReferences[i];
				if (!(playerReference.Controller == null) && playerReference.IsInBattle && playerReference.Controller.IsActive)
				{
					if ((base.player.Team == TeamNum.None || playerReference.Team != base.player.Team) && !playerReference.Controller.State.IsDead && !playerReference.Controller.State.IsRespawning)
					{
						if (FixedMath.Abs(base.player.Position.x - playerReference.Controller.Position.x) < (Fixed)((double)this.data.targetXDistance) && FixedMath.Abs(base.player.Position.y - playerReference.Controller.Position.y) < (Fixed)((double)this.data.targetYDistance))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000B0D RID: 2829
		private bool enemyNearbyState;

		// Token: 0x04000B0E RID: 2830
		private bool useDefense;

		// Token: 0x04000B0F RID: 2831
		public DefensiveOptionComposite.Data data = new DefensiveOptionComposite.Data();

		// Token: 0x0200032C RID: 812
		[Serializable]
		public class Data
		{
			// Token: 0x04000B10 RID: 2832
			public float targetXDistance = 3.5f;

			// Token: 0x04000B11 RID: 2833
			public float targetYDistance = 5f;

			// Token: 0x04000B12 RID: 2834
			public Fixed defendChance = (Fixed)0.33000001311302185;
		}
	}
}
