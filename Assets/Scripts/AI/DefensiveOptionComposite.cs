// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

namespace AI
{
	public class DefensiveOptionComposite : Composite
	{
		[Serializable]
		public class Data
		{
			public float targetXDistance = 3.5f;

			public float targetYDistance = 5f;

			public Fixed defendChance = (Fixed)0.33000001311302185;
		}

		private bool enemyNearbyState;

		private bool useDefense;

		public DefensiveOptionComposite.Data data = new DefensiveOptionComposite.Data();

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
	}
}
