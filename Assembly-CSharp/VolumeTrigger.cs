using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200065F RID: 1631
public class VolumeTrigger : StageTrigger
{
	// Token: 0x060027ED RID: 10221 RVA: 0x000C2508 File Offset: 0x000C0908
	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.model = new VolumeTriggerModel();
		VolumeTrigger.physicsContext.world = base.gameController.currentGame.PhysicsWorld;
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x000C2563 File Offset: 0x000C0963
	private void onGameInit(GameEvent message)
	{
		if (this.model.playersInVolume == null)
		{
			this.model.playersInVolume = new bool[12];
		}
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000C2588 File Offset: 0x000C0988
	public override void TickFrame()
	{
		foreach (PlayerController playerController in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerController.PlayerNum, false);
			bool flag = this.model.playersInVolume[intFromPlayerNum];
			VolumeTrigger.physicsContext.colliderOwner = playerController.Physics;
			Vector2F vector2F = playerController.Physics.State.totalVelocity * WTime.fixedDeltaTime;
			FixedRect bounds = PhysicsUtil.ExtendBoundingBox(VolumeTrigger.physicsContext.collider.BoundingBox, vector2F * 4);
			base.gameController.currentGame.PhysicsWorld.GetRelevantSegments(bounds, VolumeTrigger.segmentBuffer);
			VolumeTrigger.collisions.Clear();
			bool flag2 = PhysicsCollisionCalculator.DetectCollisions(VolumeTrigger.physicsContext, VolumeTrigger.segmentBuffer, vector2F, PhysicsSimulator.HazardsMask, VolumeTrigger.collisions);
			bool flag3 = false;
			CollisionData collision = default(CollisionData);
			if (flag2)
			{
				foreach (CollisionData collisionData in VolumeTrigger.collisions)
				{
					if (this.surface.Colliders.Contains(collisionData.terrainCollider as PhysicsCollider))
					{
						collision = collisionData;
						flag3 = true;
						break;
					}
				}
			}
			bool flag4 = false;
			if ((this.triggerType & VolumeTrigger.VolumeTriggerType.OnEnter) != VolumeTrigger.VolumeTriggerType.Never)
			{
				flag4 |= (!flag && flag3);
			}
			if ((this.triggerType & VolumeTrigger.VolumeTriggerType.OnExit) != VolumeTrigger.VolumeTriggerType.Never)
			{
				flag4 |= (!flag3 && flag);
			}
			if ((this.triggerType & VolumeTrigger.VolumeTriggerType.Continuous) != VolumeTrigger.VolumeTriggerType.Never)
			{
				flag4 = (flag4 || flag3);
			}
			if (flag4 && this.Triggered != null)
			{
				base.CallTriggered(new HazardPlayContext
				{
					collision = collision,
					playerNum = playerController.PlayerNum
				});
			}
			this.model.playersInVolume[intFromPlayerNum] = flag3;
		}
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000C27C8 File Offset: 0x000C0BC8
	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<VolumeTriggerModel>(this.model));
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000C27E2 File Offset: 0x000C0BE2
	public override bool LoadState(RollbackStateContainer container)
	{
		return container.ReadState<VolumeTriggerModel>(ref this.model);
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000C27F0 File Offset: 0x000C0BF0
	public override void Destroy()
	{
		base.Destroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x04001D24 RID: 7460
	public VolumeTrigger.VolumeTriggerType triggerType;

	// Token: 0x04001D25 RID: 7461
	public StageSurface surface;

	// Token: 0x04001D26 RID: 7462
	private VolumeTriggerModel model;

	// Token: 0x04001D27 RID: 7463
	private static List<ColliderSegmentReference> segmentBuffer = new List<ColliderSegmentReference>(64);

	// Token: 0x04001D28 RID: 7464
	private static List<CollisionData> collisions = new List<CollisionData>();

	// Token: 0x04001D29 RID: 7465
	private static PhysicsContext physicsContext = new PhysicsContext();

	// Token: 0x02000660 RID: 1632
	[Flags]
	public enum VolumeTriggerType
	{
		// Token: 0x04001D2B RID: 7467
		Never = 0,
		// Token: 0x04001D2C RID: 7468
		OnEnter = 1,
		// Token: 0x04001D2D RID: 7469
		OnExit = 2,
		// Token: 0x04001D2E RID: 7470
		Continuous = 4
	}
}
