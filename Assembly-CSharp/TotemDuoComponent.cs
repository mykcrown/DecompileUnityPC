using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class TotemDuoComponent : CharacterComponent, ICharacterInitListener, IRespawnListener, ICharacterStartListener, IDeathListener
{
	// Token: 0x0600213E RID: 8510 RVA: 0x000A6114 File Offset: 0x000A4514
	public TotemDuoComponent()
	{
		this.state = new TotemDuoComponentState();
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x0600213F RID: 8511 RVA: 0x000A6127 File Offset: 0x000A4527
	// (set) Token: 0x06002140 RID: 8512 RVA: 0x000A612F File Offset: 0x000A452F
	private TotemDuoComponentState state { get; set; }

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x06002141 RID: 8513 RVA: 0x000A6138 File Offset: 0x000A4538
	public override IComponentState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000A6140 File Offset: 0x000A4540
	public void OnCharacterInit(PlayerSelectionInfo playerInfo, GameModeData modeData)
	{
		this.state.isClone = !this.playerDelegate.IsActive;
		if (!this.state.isClone)
		{
			PlayerController playerController = base.dependencyInjection.CreateComponentWithGameObject<PlayerController>("Player " + this.playerDelegate.PlayerNum + " Partner");
			playerController.transform.SetParent(this.playerDelegate.Transform.parent, true);
			base.gameManager.CharacterControllers.Add(playerController);
			base.gameManager.CameraInfluencers.Add(playerController);
			PlayerReference playerReference = base.gameManager.GetPlayerReference(this.playerDelegate.PlayerNum);
			playerReference.AddController(playerController);
			playerController.IsActive = false;
			PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)playerInfo.Clone();
			playerSelectionInfo.characterIndex = (playerInfo.characterIndex + 1) % 2;
			playerController.Init(playerSelectionInfo, playerReference, modeData, null);
			this.state.partner = playerController;
			this.state.partner.LoadInstanceData(this.playerDelegate);
			this.state.partnerComponent = playerController.GetCharacterComponent<TotemDuoComponent>();
			if (this.state.partnerComponent != null)
			{
				this.state.partnerComponent.state.partner = this.playerDelegate;
				this.state.partnerComponent.state.partnerComponent = this;
			}
		}
		base.events.Subscribe(typeof(DespawnTotemPartnerCommand), new Events.EventHandler(this.onDespawnTotemPartnerCommand));
		base.events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		base.events.Subscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000A6308 File Offset: 0x000A4708
	public override void Destroy()
	{
		base.events.Unsubscribe(typeof(DespawnTotemPartnerCommand), new Events.EventHandler(this.onDespawnTotemPartnerCommand));
		base.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		base.events.Unsubscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.Destroy();
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000A6380 File Offset: 0x000A4780
	public void OnCharacterStart()
	{
		if (this.state.isClone)
		{
			this.becomeInactiveWithoutSwapping();
		}
		else
		{
			this.state.partner.SetFacingAndRotation(this.playerDelegate.Facing);
			Vector3F b = new Vector3F(this.partnerSpawnOffset * InputUtils.GetDirectionMultiplier(this.playerDelegate.Facing), 0, 0);
			this.state.partner.Physics.SetPosition(this.playerDelegate.Position + b);
		}
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000A6417 File Offset: 0x000A4817
	public override void LoadState(IComponentState state)
	{
		this.state = (state as TotemDuoComponentState);
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06002146 RID: 8518 RVA: 0x000A6425 File Offset: 0x000A4825
	public TotemDuoComponentState MyState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x000A642D File Offset: 0x000A482D
	public void OnRespawn()
	{
		if (!this.playerDelegate.IsActive)
		{
			this.becomeInactiveWithoutSwapping();
		}
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000A6448 File Offset: 0x000A4848
	private void becomeInactiveWithoutSwapping()
	{
		this.playerDelegate.SetMove(this.waitingForStartMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x000A6484 File Offset: 0x000A4884
	public void PerformSwap()
	{
		if (!this.state.ignoreNextSwap)
		{
			HorizontalDirection facing = this.state.partner.Facing;
			this.state.partner.LoadInstanceData(this.playerDelegate);
			this.state.partner.SetFacingAndRotation(facing);
			this.state.partner.IsActive = true;
			Vector3 position = this.state.partner.Transform.position;
			position.z = 0f;
			this.state.partner.Transform.position = position;
			if (this.state.partner.Physics.IsGrounded)
			{
				this.state.partner.SetMove(this.state.partnerComponent.swapInMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
			}
			else
			{
				this.state.partner.SetMove(this.state.partnerComponent.swapInMoveAir, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
			}
		}
		this.state.ignoreNextSwap = false;
		this.playerDelegate.IsActive = false;
		Vector3 position2 = this.playerDelegate.Transform.position;
		position2.z = 0f;
		this.playerDelegate.Transform.position = position2;
		base.events.Broadcast(new CharacterSwapEvent((PlayerController)this.playerDelegate));
		base.events.Broadcast(new CharacterSwapEvent((PlayerController)this.state.partner));
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x000A6640 File Offset: 0x000A4A40
	public void OnDeath()
	{
		if (!this.state.partner.Model.isDead)
		{
			base.events.Broadcast(new DespawnTotemPartnerCommand((PlayerController)this.playerDelegate));
		}
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x000A6678 File Offset: 0x000A4A78
	private void onDespawnTotemPartnerCommand(GameEvent message)
	{
		DespawnTotemPartnerCommand despawnTotemPartnerCommand = message as DespawnTotemPartnerCommand;
		if (this.playerDelegate == despawnTotemPartnerCommand.character)
		{
			this.state.partner.CharacterDeath(false);
			this.state.partner.GameVFX.PlayParticle(this.playerDelegate.Config.defaultCharacterEffects.totemDespawn, false, TeamNum.None);
		}
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000A66DC File Offset: 0x000A4ADC
	private void onGameStart(GameEvent message)
	{
		if (!this.playerDelegate.IsActive)
		{
			this.playerDelegate.SetMove(this.startMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000A6728 File Offset: 0x000A4B28
	private void onRespawnPlatformExpire(GameEvent message)
	{
		RespawnPlatformExpireEvent respawnPlatformExpireEvent = message as RespawnPlatformExpireEvent;
		if (respawnPlatformExpireEvent.playerNum == this.playerDelegate.PlayerNum && !this.playerDelegate.IsActive)
		{
			this.playerDelegate.SetMove(this.startMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
	}

	// Token: 0x04001A3D RID: 6717
	public MoveData waitingForStartMove;

	// Token: 0x04001A3E RID: 6718
	public MoveData startMove;

	// Token: 0x04001A3F RID: 6719
	public MoveData crewBattleAssistMove;

	// Token: 0x04001A40 RID: 6720
	public MoveData swapInMove;

	// Token: 0x04001A41 RID: 6721
	public MoveData swapInMoveAir;

	// Token: 0x04001A42 RID: 6722
	public Fixed partnerSpawnOffset;

	// Token: 0x04001A43 RID: 6723
	public MoveData partnerAssistMove;
}
