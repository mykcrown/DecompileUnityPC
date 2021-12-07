// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

namespace AI
{
	public class AICalculator : IAICalculator
	{
		private Fixed reactionSpeedVariance = 10;

		private Fixed buttonSpamVariance = 2;

		private Fixed OFFSTAGE_DEPTH_TEST = 100;

		[Inject]
		public GameController gameController
		{
			get;
			set;
		}

		private GameManager gameManager
		{
			get
			{
				return this.gameController.currentGame;
			}
		}

		public int reactionFrames
		{
			get
			{
				return 30;
			}
		}

		public int buttonSpamFrames
		{
			get
			{
				return 6;
			}
		}

		public bool TestOffstage(PlayerController player)
		{
			int num = this.gameManager.PhysicsWorld.RaycastTerrain(player.Physics.Center, Vector3F.down, this.OFFSTAGE_DEPTH_TEST, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
			return this.gameManager.Physics != null && num == 0;
		}

		public bool TestReachedEdgeOfStage(PlayerController player, int direction)
		{
			Vector2F origin = player.Physics.Center;
			origin.x += (Fixed)1.2999999523162842 * direction;
			int num = this.gameManager.PhysicsWorld.RaycastTerrain(origin, Vector3F.down, this.OFFSTAGE_DEPTH_TEST, PhysicsSimulator.GroundAndPlatformMask, null, RaycastFlags.Default, default(Fixed));
			return this.gameManager.Physics != null && num == 0;
		}

		public int RandomizeReactionSpeed(int baseReactionSpeed)
		{
			return baseReactionSpeed + (int)((this.GenerateRandomNumber() - (Fixed)0.5) * this.reactionSpeedVariance);
		}

		public int RandomizeButtonSpam(int baseButtonSpamFrames)
		{
			return baseButtonSpamFrames + (int)((this.GenerateRandomNumber() - (Fixed)0.5) * this.buttonSpamVariance);
		}

		public Fixed GenerateRandomNumber()
		{
			return (Fixed)((double)UnityEngine.Random.value);
		}

		public PlayerReference FindClosestEnemy(PlayerController player)
		{
			if (this.gameManager != null)
			{
				return PlayerUtil.FindClosestEnemy(player, this.gameManager.PlayerReferences);
			}
			return null;
		}
	}
}
