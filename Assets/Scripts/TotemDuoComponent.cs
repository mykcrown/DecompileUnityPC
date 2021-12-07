// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class TotemDuoComponent : CharacterComponent, ICharacterInitListener, IRespawnListener, ICharacterStartListener, IDeathListener
{
	public MoveData waitingForStartMove;

	public MoveData startMove;

	public MoveData crewBattleAssistMove;

	public MoveData swapInMove;

	public MoveData swapInMoveAir;

	public Fixed partnerSpawnOffset;

	public MoveData partnerAssistMove;

	private TotemDuoComponentState state
	{
		get;
		set;
	}

	public override IComponentState State
	{
		get
		{
			return this.state;
		}
	}

	public TotemDuoComponentState MyState
	{
		get
		{
			return this.state;
		}
	}

	public TotemDuoComponent()
	{
		this.state = new TotemDuoComponentState();
	}

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

	public override void Destroy()
	{
		base.events.Unsubscribe(typeof(DespawnTotemPartnerCommand), new Events.EventHandler(this.onDespawnTotemPartnerCommand));
		base.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		base.events.Unsubscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.Destroy();
	}

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

	public override void LoadState(IComponentState state)
	{
		this.state = (state as TotemDuoComponentState);
	}

	public void OnRespawn()
	{
		if (!this.playerDelegate.IsActive)
		{
			this.becomeInactiveWithoutSwapping();
		}
	}

	private void becomeInactiveWithoutSwapping()
	{
		this.playerDelegate.SetMove(this.waitingForStartMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
	}

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

	public void OnDeath()
	{
		if (!this.state.partner.Model.isDead)
		{
			base.events.Broadcast(new DespawnTotemPartnerCommand((PlayerController)this.playerDelegate));
		}
	}

	private void onDespawnTotemPartnerCommand(GameEvent message)
	{
		DespawnTotemPartnerCommand despawnTotemPartnerCommand = message as DespawnTotemPartnerCommand;
		if (this.playerDelegate == despawnTotemPartnerCommand.character)
		{
			this.state.partner.CharacterDeath(false);
			this.state.partner.GameVFX.PlayParticle(this.playerDelegate.Config.defaultCharacterEffects.totemDespawn, false, TeamNum.None);
		}
	}

	private void onGameStart(GameEvent message)
	{
		if (!this.playerDelegate.IsActive)
		{
			this.playerDelegate.SetMove(this.startMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
	}

	private void onRespawnPlatformExpire(GameEvent message)
	{
		RespawnPlatformExpireEvent respawnPlatformExpireEvent = message as RespawnPlatformExpireEvent;
		if (respawnPlatformExpireEvent.playerNum == this.playerDelegate.PlayerNum && !this.playerDelegate.IsActive)
		{
			this.playerDelegate.SetMove(this.startMove, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
	}
}
