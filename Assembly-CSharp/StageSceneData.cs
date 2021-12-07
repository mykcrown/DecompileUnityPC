using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200064A RID: 1610
public class StageSceneData : GameBehavior, IRollbackStateOwner, ITickable
{
	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x0600276A RID: 10090 RVA: 0x000BFF0B File Offset: 0x000BE30B
	// (set) Token: 0x0600276B RID: 10091 RVA: 0x000BFF13 File Offset: 0x000BE313
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x0600276C RID: 10092 RVA: 0x000BFF1C File Offset: 0x000BE31C
	// (set) Token: 0x0600276D RID: 10093 RVA: 0x000BFF24 File Offset: 0x000BE324
	public StageSimulationData SimulationData { get; private set; }

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x0600276E RID: 10094 RVA: 0x000BFF2D File Offset: 0x000BE32D
	public IEnumerable<StageSurface> Surfaces
	{
		get
		{
			return this.surfaces;
		}
	}

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x0600276F RID: 10095 RVA: 0x000BFF35 File Offset: 0x000BE335
	public IEnumerable<Ledge> Ledges
	{
		get
		{
			return this.ledges;
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06002770 RID: 10096 RVA: 0x000BFF3D File Offset: 0x000BE33D
	public int AnimatorCount
	{
		get
		{
			return (this.animators == null) ? 0 : this.animators.Count;
		}
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x000BFF5C File Offset: 0x000BE35C
	public void GatherData()
	{
		base.gameObject.transform.position = new Vector3(0f, 0f, 0f);
		this.animators = new List<Animator>(UnityEngine.Object.FindObjectsOfType<Animator>());
		this.props = new List<StageProp>(UnityEngine.Object.FindObjectsOfType<StageProp>());
		this.visualTriggers = new List<StageTrigger>(base.gameObject.GetComponents<StageTrigger>());
		this.visualBehaviourGroups = new List<StageBehaviourGroup>(base.gameObject.GetComponents<StageBehaviourGroup>());
		this.SimulationData = UnityEngine.Object.FindObjectOfType<StageSimulationData>();
		if (this.SimulationData != null)
		{
			this.surfaces = new List<StageSurface>(this.SimulationData.GetComponentsInChildren<StageSurface>());
			foreach (StageSurface stageSurface in this.surfaces)
			{
				stageSurface.InitPhysicsColliders();
			}
			this.ledges = new List<Ledge>(this.SimulationData.GetComponentsInChildren<Ledge>());
			foreach (Ledge ledge in this.ledges)
			{
				StageSceneData.SnapLedgeToClosestVertex(ledge, this.Surfaces);
			}
			if (this.SimulationData.overrideVictoryPoseCameraAnimations == null)
			{
				this.SimulationData.overrideVictoryPoseCameraAnimations = new List<AnimationClip>();
			}
			if (this.SimulationData.shouldOverrideVictoryPosePositions == null)
			{
				this.SimulationData.shouldOverrideVictoryPosePositions = new List<bool>();
			}
			if (this.SimulationData.overrideVictoryPosePositions == null)
			{
				this.SimulationData.overrideVictoryPosePositions = new List<Vector3ListWrapper>();
			}
			this.hologramPoints = new List<HologramPoint>(this.SimulationData.GetComponentsInChildren<HologramPoint>());
			BoundsRect[] componentsInChildren = this.SimulationData.GetComponentsInChildren<BoundsRect>();
			foreach (BoundsRect boundsRect in componentsInChildren)
			{
				if (boundsRect.boundsType == BoundsRect.BoundsType.Camera)
				{
					this.SimulationData.cameraBoundsRect = boundsRect;
				}
				else if (boundsRect.boundsType == BoundsRect.BoundsType.BlastZone)
				{
					this.SimulationData.blastZoneBoundsRect = boundsRect;
				}
			}
			this.setupLayers();
			this.SimulationData.triggers = new List<StageTrigger>(this.SimulationData.GetComponents<StageTrigger>());
			this.SimulationData.behaviourGroups = new List<StageBehaviourGroup>(this.SimulationData.GetComponents<StageBehaviourGroup>());
		}
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x000C01DC File Offset: 0x000BE5DC
	private void setupLayers()
	{
		int layer = LayerMask.NameToLayer(Layers.StageData);
		if (this.SimulationData.cameraBoundsRect != null)
		{
			this.SimulationData.cameraBoundsRect.gameObject.layer = layer;
		}
		if (this.SimulationData.blastZoneBoundsRect != null)
		{
			this.SimulationData.blastZoneBoundsRect.gameObject.layer = layer;
		}
		foreach (Ledge ledge in this.ledges)
		{
			ledge.gameObject.layer = layer;
		}
		foreach (StageSurface stageSurface in this.surfaces)
		{
			if (stageSurface.gameObject.layer == LayerMask.NameToLayer(Layers.Default))
			{
				stageSurface.IsPlatform = false;
			}
		}
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x000C0308 File Offset: 0x000BE708
	private static void SnapLedgeToClosestVertex(Ledge ledge, IEnumerable<StageSurface> surfaces)
	{
		Vector2F b = (Vector2F)ledge.transform.position;
		Vector2F v = Vector2F.zero;
		Fixed @fixed = 10000;
		foreach (StageSurface stageSurface in surfaces)
		{
			foreach (PhysicsCollider physicsCollider in stageSurface.Colliders)
			{
				for (int i = 0; i < physicsCollider.Edge.Length; i++)
				{
					Vector2F point = physicsCollider.Edge.GetPoint(i);
					Fixed sqrMagnitude = (point - b).sqrMagnitude;
					if (sqrMagnitude < @fixed)
					{
						v = point;
						@fixed = sqrMagnitude;
					}
				}
			}
		}
		if (@fixed < 10000 && @fixed > 0)
		{
			ledge.transform.position = (Vector3)v;
		}
	}

	// Token: 0x06002774 RID: 10100 RVA: 0x000C0448 File Offset: 0x000BE848
	public void Startup()
	{
		if (base.gameManager == null)
		{
			return;
		}
		this.GatherData();
		this.triggerController = base.injector.GetInstance<StageTriggerController>();
		this.triggerController.Init(base.gameManager, this.SimulationData.triggers, this.visualTriggers, this.SimulationData.behaviourGroups, this.visualBehaviourGroups);
		foreach (StageSurface stageSurface in this.surfaces)
		{
			foreach (PhysicsCollider collider in stageSurface.Colliders)
			{
				base.gameManager.PhysicsWorld.RegisterCollider(collider);
			}
		}
		foreach (StageProp stageProp in this.props)
		{
			stageProp.Init();
		}
		base.events.Subscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onGamePaused));
		base.events.Subscribe(typeof(FrameControlModeChangedEvent), new Events.EventHandler(this.onFrameControlModeChanged));
	}

	// Token: 0x06002775 RID: 10101 RVA: 0x000C0600 File Offset: 0x000BEA00
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.triggerController != null)
		{
			this.triggerController.Destroy();
		}
		base.events.Unsubscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Unsubscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onGamePaused));
		base.events.Unsubscribe(typeof(FrameControlModeChangedEvent), new Events.EventHandler(this.onFrameControlModeChanged));
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06002776 RID: 10102 RVA: 0x000C068C File Offset: 0x000BEA8C
	public RespawnPoint CenterPoint
	{
		get
		{
			return this.SimulationData.spawnData.respawnPoints[0];
		}
	}

	// Token: 0x06002777 RID: 10103 RVA: 0x000C06A0 File Offset: 0x000BEAA0
	public Ledge getLedge(int index)
	{
		if (index < 0 || index >= this.ledges.Count)
		{
			Debug.LogError("Attempted to grab invalid ledge " + index);
		}
		return this.ledges[index];
	}

	// Token: 0x06002778 RID: 10104 RVA: 0x000C06DC File Offset: 0x000BEADC
	public int TestLedgeCollision(FixedRect grabBox, Vector3F characterPosition)
	{
		for (int i = 0; i < this.ledges.Count; i++)
		{
			Ledge ledge = this.ledges[i];
			if (!(characterPosition.y > ledge.Position.y) || characterPosition.x > ledge.Position.x == ledge.facesRight)
			{
				if (!ledge.IsOccupied() && ledge.Position.x >= grabBox.Left + characterPosition.x && ledge.Position.x <= grabBox.Left + characterPosition.x + grabBox.Width && ledge.Position.y >= grabBox.Top + characterPosition.y - grabBox.Height && ledge.Position.y <= grabBox.Top + characterPosition.y)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06002779 RID: 10105 RVA: 0x000C0834 File Offset: 0x000BEC34
	public RespawnPoint RecordPlayerRespawn(PlayerNum player)
	{
		for (int i = 0; i < 8; i++)
		{
			if (this.model.respawnPointsInUse[i] == PlayerNum.None)
			{
				this.model.respawnPointsInUse[i] = player;
				break;
			}
		}
		return this.GetRespawnPointForPlayer(player);
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x000C0884 File Offset: 0x000BEC84
	private void onRespawnPlatformExpire(GameEvent message)
	{
		RespawnPlatformExpireEvent respawnPlatformExpireEvent = message as RespawnPlatformExpireEvent;
		for (int i = 0; i < 8; i++)
		{
			if (this.model.respawnPointsInUse[i] == respawnPlatformExpireEvent.playerNum)
			{
				this.model.respawnPointsInUse[i] = PlayerNum.None;
				break;
			}
		}
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000C08D8 File Offset: 0x000BECD8
	public RespawnPoint GetRespawnPointForPlayer(PlayerNum player)
	{
		for (int i = 0; i < 8; i++)
		{
			if (this.model.respawnPointsInUse[i] == player)
			{
				return this.SimulationData.spawnData.respawnPoints[i];
			}
		}
		return null;
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000C0920 File Offset: 0x000BED20
	public bool LoadState(RollbackStateContainer container)
	{
		for (int i = 0; i < this.props.Count; i++)
		{
			this.props[i].LoadState(container);
		}
		this.triggerController.LoadState(container);
		container.ReadState<StageModel>(ref this.model);
		return true;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000C0978 File Offset: 0x000BED78
	public bool ExportState(ref RollbackStateContainer container)
	{
		for (int i = 0; i < this.props.Count; i++)
		{
			this.props[i].ExportState(ref container);
		}
		this.triggerController.ExportState(ref container);
		return container.WriteState(this.rollbackStatePooling.Clone<StageModel>(this.model));
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000C09DC File Offset: 0x000BEDDC
	public void TickFrame()
	{
		for (int i = 0; i < this.props.Count; i++)
		{
			this.props[i].TickFrame();
		}
		base.gameManager.PhysicsWorld.SortSegments();
		this.triggerController.TickFrame();
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000C0A34 File Offset: 0x000BEE34
	private void onGamePaused(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = message as GamePausedEvent;
		foreach (Animator animator in this.animators)
		{
			if (animator != null)
			{
				animator.enabled = !gamePausedEvent.paused;
			}
		}
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000C0AAC File Offset: 0x000BEEAC
	private void onFrameControlModeChanged(GameEvent message)
	{
		FrameControlModeChangedEvent frameControlModeChangedEvent = message as FrameControlModeChangedEvent;
		bool flag = frameControlModeChangedEvent.mode == FrameControlMode.Manual;
		foreach (Animator animator in this.animators)
		{
			if (animator != null)
			{
				animator.enabled = !flag;
			}
		}
	}

	// Token: 0x04001CDC RID: 7388
	public List<StageProp> props = new List<StageProp>();

	// Token: 0x04001CDD RID: 7389
	public List<StageSurface> surfaces = new List<StageSurface>();

	// Token: 0x04001CDE RID: 7390
	public List<Ledge> ledges = new List<Ledge>();

	// Token: 0x04001CDF RID: 7391
	public List<HologramPoint> hologramPoints = new List<HologramPoint>();

	// Token: 0x04001CE0 RID: 7392
	public List<StageTrigger> visualTriggers = new List<StageTrigger>();

	// Token: 0x04001CE1 RID: 7393
	public List<StageBehaviourGroup> visualBehaviourGroups = new List<StageBehaviourGroup>();

	// Token: 0x04001CE2 RID: 7394
	private List<Animator> animators = new List<Animator>();

	// Token: 0x04001CE3 RID: 7395
	private StageTriggerController triggerController;

	// Token: 0x04001CE4 RID: 7396
	private StageModel model = new StageModel();
}
