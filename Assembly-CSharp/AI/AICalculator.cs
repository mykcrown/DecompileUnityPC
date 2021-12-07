using System;
using FixedPoint;
using UnityEngine;

namespace AI
{
	// Token: 0x020002FB RID: 763
	public class AICalculator : IAICalculator
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x000621C7 File Offset: 0x000605C7
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x000621CF File Offset: 0x000605CF
		[Inject]
		public GameController gameController { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000621D8 File Offset: 0x000605D8
		private GameManager gameManager
		{
			get
			{
				return this.gameController.currentGame;
			}
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x000621E8 File Offset: 0x000605E8
		public bool TestOffstage(PlayerController player)
		{
			int num = this.gameManager.PhysicsWorld.RaycastTerrain(player.Physics.Center, Vector3F.down, this.OFFSTAGE_DEPTH_TEST, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
			return this.gameManager.Physics != null && num == 0;
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x00062250 File Offset: 0x00060650
		public bool TestReachedEdgeOfStage(PlayerController player, int direction)
		{
			Vector2F origin = player.Physics.Center;
			origin.x += (Fixed)1.2999999523162842 * direction;
			int num = this.gameManager.PhysicsWorld.RaycastTerrain(origin, Vector3F.down, this.OFFSTAGE_DEPTH_TEST, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
			return this.gameManager.Physics != null && num == 0;
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000622DD File Offset: 0x000606DD
		public int reactionFrames
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000622E1 File Offset: 0x000606E1
		public int buttonSpamFrames
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000622E4 File Offset: 0x000606E4
		public int RandomizeReactionSpeed(int baseReactionSpeed)
		{
			return baseReactionSpeed + (int)((this.GenerateRandomNumber() - (Fixed)0.5) * this.reactionSpeedVariance);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00062311 File Offset: 0x00060711
		public int RandomizeButtonSpam(int baseButtonSpamFrames)
		{
			return baseButtonSpamFrames + (int)((this.GenerateRandomNumber() - (Fixed)0.5) * this.buttonSpamVariance);
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0006233E File Offset: 0x0006073E
		public Fixed GenerateRandomNumber()
		{
			return (Fixed)((double)UnityEngine.Random.value);
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0006234B File Offset: 0x0006074B
		public PlayerReference FindClosestEnemy(PlayerController player)
		{
			if (this.gameManager != null)
			{
				return PlayerUtil.FindClosestEnemy(player, this.gameManager.PlayerReferences);
			}
			return null;
		}

		// Token: 0x04000A8C RID: 2700
		private Fixed reactionSpeedVariance = 10;

		// Token: 0x04000A8D RID: 2701
		private Fixed buttonSpamVariance = 2;

		// Token: 0x04000A8F RID: 2703
		private Fixed OFFSTAGE_DEPTH_TEST = 100;
	}
}
