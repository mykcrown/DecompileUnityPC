// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneData : GameBehavior, IRollbackStateOwner, ITickable
{
	public List<StageProp> props = new List<StageProp>();

	public List<StageSurface> surfaces = new List<StageSurface>();

	public List<Ledge> ledges = new List<Ledge>();

	public List<HologramPoint> hologramPoints = new List<HologramPoint>();

	public List<StageTrigger> visualTriggers = new List<StageTrigger>();

	public List<StageBehaviourGroup> visualBehaviourGroups = new List<StageBehaviourGroup>();

	private List<Animator> animators = new List<Animator>();

	private StageTriggerController triggerController;

	private StageModel model = new StageModel();

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public StageSimulationData SimulationData
	{
		get;
		private set;
	}

	public IEnumerable<StageSurface> Surfaces
	{
		get
		{
			return this.surfaces;
		}
	}

	public IEnumerable<Ledge> Ledges
	{
		get
		{
			return this.ledges;
		}
	}

	public int AnimatorCount
	{
		get
		{
			return (this.animators == null) ? 0 : this.animators.Count;
		}
	}

	public RespawnPoint CenterPoint
	{
		get
		{
			return this.SimulationData.spawnData.respawnPoints[0];
		}
	}

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
			foreach (StageSurface current in this.surfaces)
			{
				current.InitPhysicsColliders();
			}
			this.ledges = new List<Ledge>(this.SimulationData.GetComponentsInChildren<Ledge>());
			foreach (Ledge current2 in this.ledges)
			{
				StageSceneData.SnapLedgeToClosestVertex(current2, this.Surfaces);
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
			BoundsRect[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				BoundsRect boundsRect = array[i];
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
		foreach (Ledge current in this.ledges)
		{
			current.gameObject.layer = layer;
		}
		foreach (StageSurface current2 in this.surfaces)
		{
			if (current2.gameObject.layer == LayerMask.NameToLayer(Layers.Default))
			{
				current2.IsPlatform = false;
			}
		}
	}

	private static void SnapLedgeToClosestVertex(Ledge ledge, IEnumerable<StageSurface> surfaces)
	{
		Vector2F b = (Vector2F)ledge.transform.position;
		Vector2F v = Vector2F.zero;
		Fixed @fixed = 10000;
		foreach (StageSurface current in surfaces)
		{
			foreach (PhysicsCollider current2 in current.Colliders)
			{
				for (int i = 0; i < current2.Edge.Length; i++)
				{
					Vector2F point = current2.Edge.GetPoint(i);
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

	public void Startup()
	{
		if (base.gameManager == null)
		{
			return;
		}
		this.GatherData();
		this.triggerController = base.injector.GetInstance<StageTriggerController>();
		this.triggerController.Init(base.gameManager, this.SimulationData.triggers, this.visualTriggers, this.SimulationData.behaviourGroups, this.visualBehaviourGroups);
		foreach (StageSurface current in this.surfaces)
		{
			foreach (PhysicsCollider current2 in current.Colliders)
			{
				base.gameManager.PhysicsWorld.RegisterCollider(current2);
			}
		}
		foreach (StageProp current3 in this.props)
		{
			current3.Init();
		}
		base.events.Subscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.onGamePaused));
		base.events.Subscribe(typeof(FrameControlModeChangedEvent), new Events.EventHandler(this.onFrameControlModeChanged));
	}

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

	public Ledge getLedge(int index)
	{
		if (index < 0 || index >= this.ledges.Count)
		{
			UnityEngine.Debug.LogError("Attempted to grab invalid ledge " + index);
		}
		return this.ledges[index];
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		for (int i = 0; i < this.props.Count; i++)
		{
			this.props[i].ExportState(ref container);
		}
		this.triggerController.ExportState(ref container);
		return container.WriteState(this.rollbackStatePooling.Clone<StageModel>(this.model));
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.props.Count; i++)
		{
			this.props[i].TickFrame();
		}
		base.gameManager.PhysicsWorld.SortSegments();
		this.triggerController.TickFrame();
	}

	private void onGamePaused(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = message as GamePausedEvent;
		foreach (Animator current in this.animators)
		{
			if (current != null)
			{
				current.enabled = !gamePausedEvent.paused;
			}
		}
	}

	private void onFrameControlModeChanged(GameEvent message)
	{
		FrameControlModeChangedEvent frameControlModeChangedEvent = message as FrameControlModeChangedEvent;
		bool flag = frameControlModeChangedEvent.mode == FrameControlMode.Manual;
		foreach (Animator current in this.animators)
		{
			if (current != null)
			{
				current.enabled = !flag;
			}
		}
	}
}
