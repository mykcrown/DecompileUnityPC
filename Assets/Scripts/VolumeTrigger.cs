// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class VolumeTrigger : StageTrigger
{
	[Flags]
	public enum VolumeTriggerType
	{
		Never = 0,
		OnEnter = 1,
		OnExit = 2,
		Continuous = 4
	}

	public VolumeTrigger.VolumeTriggerType triggerType;

	public StageSurface surface;

	private VolumeTriggerModel model;

	private static List<ColliderSegmentReference> segmentBuffer = new List<ColliderSegmentReference>(64);

	private static List<CollisionData> collisions = new List<CollisionData>();

	private static PhysicsContext physicsContext = new PhysicsContext();

	public override void Init(IStageTriggerDependency triggerDependency, bool isSimulation)
	{
		base.Init(triggerDependency, isSimulation);
		this.model = new VolumeTriggerModel();
		VolumeTrigger.physicsContext.world = base.gameController.currentGame.PhysicsWorld;
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	private void onGameInit(GameEvent message)
	{
		if (this.model.playersInVolume == null)
		{
			this.model.playersInVolume = new bool[12];
		}
	}

	public override void TickFrame()
	{
		foreach (PlayerController current in this.triggerDependency.GetPlayers())
		{
			int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(current.PlayerNum, false);
			bool flag = this.model.playersInVolume[intFromPlayerNum];
			VolumeTrigger.physicsContext.colliderOwner = current.Physics;
			Vector2F vector2F = current.Physics.State.totalVelocity * WTime.fixedDeltaTime;
			FixedRect bounds = PhysicsUtil.ExtendBoundingBox(VolumeTrigger.physicsContext.collider.BoundingBox, vector2F * 4);
			base.gameController.currentGame.PhysicsWorld.GetRelevantSegments(bounds, VolumeTrigger.segmentBuffer);
			VolumeTrigger.collisions.Clear();
			bool flag2 = PhysicsCollisionCalculator.DetectCollisions(VolumeTrigger.physicsContext, VolumeTrigger.segmentBuffer, vector2F, PhysicsSimulator.HazardsMask, VolumeTrigger.collisions);
			bool flag3 = false;
			CollisionData collision = default(CollisionData);
			if (flag2)
			{
				foreach (CollisionData current2 in VolumeTrigger.collisions)
				{
					if (this.surface.Colliders.Contains(current2.terrainCollider as PhysicsCollider))
					{
						collision = current2;
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
				flag4 |= flag3;
			}
			if (flag4 && this.Triggered != null)
			{
				base.CallTriggered(new HazardPlayContext
				{
					collision = collision,
					playerNum = current.PlayerNum
				});
			}
			this.model.playersInVolume[intFromPlayerNum] = flag3;
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<VolumeTriggerModel>(this.model));
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		return container.ReadState<VolumeTriggerModel>(ref this.model);
	}

	public override void Destroy()
	{
		base.Destroy();
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}
}
