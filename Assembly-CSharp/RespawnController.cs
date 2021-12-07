using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class RespawnController : IRespawnController, ITickable, IRollbackStateOwner
{
	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06002699 RID: 9881 RVA: 0x000BD420 File Offset: 0x000BB820
	// (set) Token: 0x0600269A RID: 9882 RVA: 0x000BD428 File Offset: 0x000BB828
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x0600269B RID: 9883 RVA: 0x000BD431 File Offset: 0x000BB831
	// (set) Token: 0x0600269C RID: 9884 RVA: 0x000BD439 File Offset: 0x000BB839
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x0600269D RID: 9885 RVA: 0x000BD442 File Offset: 0x000BB842
	// (set) Token: 0x0600269E RID: 9886 RVA: 0x000BD44A File Offset: 0x000BB84A
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x0600269F RID: 9887 RVA: 0x000BD453 File Offset: 0x000BB853
	// (set) Token: 0x060026A0 RID: 9888 RVA: 0x000BD45B File Offset: 0x000BB85B
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x060026A1 RID: 9889 RVA: 0x000BD464 File Offset: 0x000BB864
	public void Init(PlayerReference playerRef, GameObject customRespawnPlatform)
	{
		this.playerRef = playerRef;
		this.model.isDead = true;
		this.theEffect = this.gameController.currentGame.DynamicObjects.InstantiateDynamicObject<Effect>(this.config.respawnConfig.respawnPlatformPrefab, 4, true);
		this.thePlatform = this.theEffect.GetComponent<RespawnPlatform>();
		this.thePlatform.AttachCustom(customRespawnPlatform);
		this.theEffect.gameObject.SetActive(false);
	}

	// Token: 0x060026A2 RID: 9890 RVA: 0x000BD4E0 File Offset: 0x000BB8E0
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

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x060026A3 RID: 9891 RVA: 0x000BD5F1 File Offset: 0x000BB9F1
	Vector3F IRespawnController.Position
	{
		get
		{
			return this.model.position;
		}
	}

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x060026A4 RID: 9892 RVA: 0x000BD5FE File Offset: 0x000BB9FE
	public bool IsRespawning
	{
		get
		{
			return !this.model.isDead;
		}
	}

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x060026A5 RID: 9893 RVA: 0x000BD60E File Offset: 0x000BBA0E
	bool IRespawnController.HasArrived
	{
		get
		{
			return !this.model.isDead && this.model.arrived;
		}
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x000BD630 File Offset: 0x000BBA30
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

	// Token: 0x060026A7 RID: 9895 RVA: 0x000BD70C File Offset: 0x000BBB0C
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

	// Token: 0x060026A8 RID: 9896 RVA: 0x000BD764 File Offset: 0x000BBB64
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

	// Token: 0x060026A9 RID: 9897 RVA: 0x000BD7FD File Offset: 0x000BBBFD
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<RespawnControllerModel>(this.model));
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x000BD817 File Offset: 0x000BBC17
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<RespawnControllerModel>(ref this.model);
		if (!this.model.isDead)
		{
			this.theEffect.transform.position = (Vector3)this.model.position;
		}
		return true;
	}

	// Token: 0x04001C3A RID: 7226
	private RespawnControllerModel model = new RespawnControllerModel();

	// Token: 0x04001C3B RID: 7227
	private RespawnPlatform thePlatform;

	// Token: 0x04001C3C RID: 7228
	private Effect theEffect;

	// Token: 0x04001C3D RID: 7229
	private PlayerReference playerRef;
}
