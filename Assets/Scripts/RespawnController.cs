// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class RespawnController : IRespawnController, ITickable, IRollbackStateOwner
{
	private RespawnControllerModel model = new RespawnControllerModel();

	private RespawnPlatform thePlatform;

	private Effect theEffect;

	private PlayerReference playerRef;

	Vector3F IRespawnController.Position
	{
		get
		{
			return this.model.position;
		}
	}

	bool IRespawnController.HasArrived
	{
		get
		{
			return !this.model.isDead && this.model.arrived;
		}
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public bool IsRespawning
	{
		get
		{
			return !this.model.isDead;
		}
	}

	public void Init(PlayerReference playerRef, GameObject customRespawnPlatform)
	{
		this.playerRef = playerRef;
		this.model.isDead = true;
		this.theEffect = this.gameController.currentGame.DynamicObjects.InstantiateDynamicObject<Effect>(this.config.respawnConfig.respawnPlatformPrefab, 4, true);
		this.thePlatform = this.theEffect.GetComponent<RespawnPlatform>();
		this.thePlatform.AttachCustom(customRespawnPlatform);
		this.theEffect.gameObject.SetActive(false);
	}

	void IRespawnController.StartRespawn(SpawnPointBase point)
	{
		if (this.IsRespawning)
		{
			return;
		}
		this.model.isDead = false;
		this.model.arrived = false;
		this.model.framesAlive = 0;
		this.model.targetPoint = (Vector3F)point.transform.position;
		this.model.position = new Vector3F(this.model.targetPoint.x, this.model.targetPoint.y + this.config.respawnConfig.platformOffscreenHeight);
		this.model.velocity = (this.model.targetPoint - this.model.position).normalized * this.config.respawnConfig.platformSpeed;
		this.theEffect.transform.position = (Vector3)this.model.position;
		this.thePlatform.gameObject.SetActive(true);
	}

	public void TickFrame()
	{
		if (this.model.isDead || this.playerRef == null || this.playerRef.IsSpectating)
		{
			return;
		}
		this.model.framesAlive++;
		if (this.model.framesAlive > this.config.respawnConfig.platformDurationFrames)
		{
			this.Dismount();
			return;
		}
		this.model.position = this.advancePosition(this.model.position);
		if (this.model.position == this.model.targetPoint)
		{
			this.model.arrived = true;
		}
		this.theEffect.transform.position = (Vector3)this.model.position;
	}

	public void Dismount()
	{
		if (this.model.isDead)
		{
			return;
		}
		this.theEffect.gameObject.SetActive(false);
		this.model.isDead = true;
		this.events.Broadcast(new RespawnPlatformExpireEvent(this.playerRef.PlayerNum));
	}

	private Vector3F advancePosition(Vector3F currentPosition)
	{
		if (this.model.arrived)
		{
			return currentPosition;
		}
		Vector3F result;
		if ((this.model.velocity * WTime.fixedDeltaTime).sqrMagnitude >= (this.model.targetPoint - this.model.position).sqrMagnitude)
		{
			result = this.model.targetPoint;
		}
		else
		{
			result = currentPosition + this.model.velocity * WTime.fixedDeltaTime;
		}
		return result;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<RespawnControllerModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<RespawnControllerModel>(ref this.model);
		if (!this.model.isDead)
		{
			this.theEffect.transform.position = (Vector3)this.model.position;
		}
		return true;
	}
}
