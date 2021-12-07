using System;
using System.Collections.Generic;
using FixedPoint;
using strange.extensions.signal.impl;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
public class PlayerController : GameBehavior, IHitOwner, ITrailOwner, IPlayerDelegate, ITickable, IRollbackStateOwner, IBoundsOwner, IPhysicsDelegate, IFacing, ICameraInfluencer, IPlayerInputActor, IPlayerDataOwner, IPositionOwner, IMoveOwner, PlayerStateActor.IPlayerActorDelegate, IItemHolder
{
	// Token: 0x06002236 RID: 8758 RVA: 0x000A9E20 File Offset: 0x000A8220
	public PlayerController()
	{
		this.tickMoveComponent = ((IMoveTickListener listener) => this.tickMoveComponentFn(listener));
		this.onParticleCreated = delegate(ParticleData data, GameObject obj)
		{
			this.onParticleCreatedFn(data, obj);
		};
		this.onDeath = delegate(IDeathListener listener)
		{
			listener.OnDeath();
			return false;
		};
	}

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06002237 RID: 8759 RVA: 0x000A9EFA File Offset: 0x000A82FA
	// (set) Token: 0x06002238 RID: 8760 RVA: 0x000A9F02 File Offset: 0x000A8302
	[Inject]
	public ISpawnController spawnController { get; set; }

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06002239 RID: 8761 RVA: 0x000A9F0B File Offset: 0x000A830B
	// (set) Token: 0x0600223A RID: 8762 RVA: 0x000A9F13 File Offset: 0x000A8313
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x0600223B RID: 8763 RVA: 0x000A9F1C File Offset: 0x000A831C
	// (set) Token: 0x0600223C RID: 8764 RVA: 0x000A9F24 File Offset: 0x000A8324
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x0600223D RID: 8765 RVA: 0x000A9F2D File Offset: 0x000A832D
	// (set) Token: 0x0600223E RID: 8766 RVA: 0x000A9F35 File Offset: 0x000A8335
	[Inject]
	public IInputSettingsScreenAPI api { get; set; }

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x0600223F RID: 8767 RVA: 0x000A9F3E File Offset: 0x000A833E
	// (set) Token: 0x06002240 RID: 8768 RVA: 0x000A9F46 File Offset: 0x000A8346
	[Inject]
	public IPlayerTauntsFinder playerTauntsFinder { get; set; }

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06002241 RID: 8769 RVA: 0x000A9F4F File Offset: 0x000A834F
	// (set) Token: 0x06002242 RID: 8770 RVA: 0x000A9F57 File Offset: 0x000A8357
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06002243 RID: 8771 RVA: 0x000A9F60 File Offset: 0x000A8360
	// (set) Token: 0x06002244 RID: 8772 RVA: 0x000A9F68 File Offset: 0x000A8368
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06002245 RID: 8773 RVA: 0x000A9F71 File Offset: 0x000A8371
	// (set) Token: 0x06002246 RID: 8774 RVA: 0x000A9F79 File Offset: 0x000A8379
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { get; set; }

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06002247 RID: 8775 RVA: 0x000A9F82 File Offset: 0x000A8382
	// (set) Token: 0x06002248 RID: 8776 RVA: 0x000A9F8A File Offset: 0x000A838A
	[Inject]
	public ISkinDataManager skinDataManager { get; set; }

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06002249 RID: 8777 RVA: 0x000A9F93 File Offset: 0x000A8393
	// (set) Token: 0x0600224A RID: 8778 RVA: 0x000A9F9B File Offset: 0x000A839B
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x0600224B RID: 8779 RVA: 0x000A9FA4 File Offset: 0x000A83A4
	// (set) Token: 0x0600224C RID: 8780 RVA: 0x000A9FAC File Offset: 0x000A83AC
	[Inject]
	public IPlayerTauntsFinder tauntFinder { get; set; }

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x0600224D RID: 8781 RVA: 0x000A9FB5 File Offset: 0x000A83B5
	// (set) Token: 0x0600224E RID: 8782 RVA: 0x000A9FBD File Offset: 0x000A83BD
	[Inject]
	public IRespawnPlatformLocator respawnPlatformLocator { get; set; }

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x0600224F RID: 8783 RVA: 0x000A9FC6 File Offset: 0x000A83C6
	// (set) Token: 0x06002250 RID: 8784 RVA: 0x000A9FCE File Offset: 0x000A83CE
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06002251 RID: 8785 RVA: 0x000A9FD7 File Offset: 0x000A83D7
	// (set) Token: 0x06002252 RID: 8786 RVA: 0x000A9FDF File Offset: 0x000A83DF
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06002253 RID: 8787 RVA: 0x000A9FE8 File Offset: 0x000A83E8
	// (set) Token: 0x06002254 RID: 8788 RVA: 0x000A9FF0 File Offset: 0x000A83F0
	[Inject]
	public IHitContextPool hitContextPool { get; set; }

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x06002255 RID: 8789 RVA: 0x000A9FF9 File Offset: 0x000A83F9
	// (set) Token: 0x06002256 RID: 8790 RVA: 0x000AA001 File Offset: 0x000A8401
	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager { get; set; }

	// Token: 0x170007D4 RID: 2004
	// (get) Token: 0x06002257 RID: 8791 RVA: 0x000AA00A File Offset: 0x000A840A
	// (set) Token: 0x06002258 RID: 8792 RVA: 0x000AA012 File Offset: 0x000A8412
	[Inject]
	public IUserGameplaySettingsModel userGameplaySettingsModel { get; set; }

	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06002259 RID: 8793 RVA: 0x000AA01B File Offset: 0x000A841B
	// (set) Token: 0x0600225A RID: 8794 RVA: 0x000AA023 File Offset: 0x000A8423
	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator { get; set; }

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x0600225B RID: 8795 RVA: 0x000AA02C File Offset: 0x000A842C
	public bool IsCurrentFrame
	{
		get
		{
			return base.gameController.currentGame == null || base.gameController.currentGame.FrameController.IsCurrentFrame;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x0600225C RID: 8796 RVA: 0x000AA05F File Offset: 0x000A845F
	public bool CanUsePowerMove
	{
		get
		{
			return this.Reference.IsBenched && this.model.teamPowerMoveCooldownFrames == 0;
		}
	}

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000AA082 File Offset: 0x000A8482
	// (set) Token: 0x0600225E RID: 8798 RVA: 0x000AA08A File Offset: 0x000A848A
	public PlayerModel Model
	{
		get
		{
			return this.model;
		}
		set
		{
			this.model = value;
		}
	}

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x0600225F RID: 8799 RVA: 0x000AA093 File Offset: 0x000A8493
	public CharacterData CharacterData
	{
		get
		{
			return this.characterData;
		}
	}

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x06002260 RID: 8800 RVA: 0x000AA09B File Offset: 0x000A849B
	public CharacterMenusData CharacterMenusData
	{
		get
		{
			return this.characterMenusData;
		}
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06002261 RID: 8801 RVA: 0x000AA0A3 File Offset: 0x000A84A3
	public SkinData SkinData
	{
		get
		{
			return this.skinData;
		}
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06002262 RID: 8802 RVA: 0x000AA0AB File Offset: 0x000A84AB
	// (set) Token: 0x06002263 RID: 8803 RVA: 0x000AA0B8 File Offset: 0x000A84B8
	public Fixed Damage
	{
		get
		{
			return this.model.damage;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentException("Damage values cannot be less than 0.");
			}
			if (value > 999)
			{
				throw new ArgumentException("Damage values cannot be greater than 999.");
			}
			this.model.damage = value;
		}
	}

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x06002264 RID: 8804 RVA: 0x000AA0F8 File Offset: 0x000A84F8
	// (set) Token: 0x06002265 RID: 8805 RVA: 0x000AA105 File Offset: 0x000A8505
	public int StunFrames
	{
		get
		{
			return this.model.stunFrames;
		}
		set
		{
			this.model.stunFrames = value;
		}
	}

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06002266 RID: 8806 RVA: 0x000AA113 File Offset: 0x000A8513
	// (set) Token: 0x06002267 RID: 8807 RVA: 0x000AA120 File Offset: 0x000A8520
	public int Lives
	{
		get
		{
			return this.Reference.Lives;
		}
		set
		{
			this.Reference.Lives = value;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x06002268 RID: 8808 RVA: 0x000AA12E File Offset: 0x000A852E
	public PlayerNum PlayerNum
	{
		get
		{
			return this.Reference.PlayerNum;
		}
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x06002269 RID: 8809 RVA: 0x000AA13B File Offset: 0x000A853B
	public bool IsInvincible
	{
		get
		{
			return this.Invincibility.IsInvincible;
		}
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x0600226A RID: 8810 RVA: 0x000AA148 File Offset: 0x000A8548
	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x0600226B RID: 8811 RVA: 0x000AA150 File Offset: 0x000A8550
	public Vector3F Position
	{
		get
		{
			return this.physics.State.position;
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x0600226C RID: 8812 RVA: 0x000AA162 File Offset: 0x000A8562
	public Vector3F MovementVelocity
	{
		get
		{
			return this.physics.State.movementVelocity;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x0600226D RID: 8813 RVA: 0x000AA174 File Offset: 0x000A8574
	public Vector3F Center
	{
		get
		{
			return this.physics.Center;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x0600226E RID: 8814 RVA: 0x000AA181 File Offset: 0x000A8581
	public Vector3F EmitPosition
	{
		get
		{
			return this.physics.Center;
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x0600226F RID: 8815 RVA: 0x000AA18E File Offset: 0x000A858E
	public StaleMoveQueue StaleMoves
	{
		get
		{
			return this.staleMoveQueue;
		}
	}

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06002270 RID: 8816 RVA: 0x000AA196 File Offset: 0x000A8596
	public IMoveUseTracker MoveUseTracker
	{
		get
		{
			return this.moveUseTracker;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06002271 RID: 8817 RVA: 0x000AA19E File Offset: 0x000A859E
	public MoveController ActiveMove
	{
		get
		{
			return this.activeMove;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06002272 RID: 8818 RVA: 0x000AA1A6 File Offset: 0x000A85A6
	public float CurrentDamage
	{
		get
		{
			return (float)this.model.damage;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06002273 RID: 8819 RVA: 0x000AA1B8 File Offset: 0x000A85B8
	// (set) Token: 0x06002274 RID: 8820 RVA: 0x000AA1C0 File Offset: 0x000A85C0
	public IMoveSet MoveSet { get; private set; }

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06002275 RID: 8821 RVA: 0x000AA1C9 File Offset: 0x000A85C9
	public PlayerPhysicsController Physics
	{
		get
		{
			return this.physics;
		}
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000AA1D1 File Offset: 0x000A85D1
	public IGameInput GetGameInput()
	{
		return this.nonVoidController;
	}

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06002277 RID: 8823 RVA: 0x000AA1D9 File Offset: 0x000A85D9
	public InputController InputController
	{
		get
		{
			return this.inputController;
		}
	}

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06002278 RID: 8824 RVA: 0x000AA1E1 File Offset: 0x000A85E1
	public IGameInput GameInput
	{
		get
		{
			return this.nonVoidController;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06002279 RID: 8825 RVA: 0x000AA1E9 File Offset: 0x000A85E9
	public IFrameOwner FrameOwner
	{
		get
		{
			return base.gameManager;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x0600227A RID: 8826 RVA: 0x000AA1F1 File Offset: 0x000A85F1
	public ConfigData Config
	{
		get
		{
			return base.config;
		}
	}

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x0600227B RID: 8827 RVA: 0x000AA1F9 File Offset: 0x000A85F9
	// (set) Token: 0x0600227C RID: 8828 RVA: 0x000AA206 File Offset: 0x000A8606
	public HorizontalDirection Facing
	{
		get
		{
			return this.model.facing;
		}
		set
		{
			this.model.facing = value;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x0600227D RID: 8829 RVA: 0x000AA214 File Offset: 0x000A8614
	// (set) Token: 0x0600227E RID: 8830 RVA: 0x000AA21C File Offset: 0x000A861C
	public Fixed FacingInterpolation { get; set; }

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x0600227F RID: 8831 RVA: 0x000AA225 File Offset: 0x000A8625
	// (set) Token: 0x06002280 RID: 8832 RVA: 0x000AA22D File Offset: 0x000A862D
	public int FacingTurnaroundWait { get; set; }

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06002281 RID: 8833 RVA: 0x000AA236 File Offset: 0x000A8636
	// (set) Token: 0x06002282 RID: 8834 RVA: 0x000AA23E File Offset: 0x000A863E
	public HorizontalDirection WaitingForFacingTurnaround { get; set; }

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06002283 RID: 8835 RVA: 0x000AA247 File Offset: 0x000A8647
	public HorizontalDirection OppositeFacing
	{
		get
		{
			return (this.Facing != HorizontalDirection.Right) ? HorizontalDirection.Right : HorizontalDirection.Left;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06002284 RID: 8836 RVA: 0x000AA25C File Offset: 0x000A865C
	public bool IsOffstage
	{
		get
		{
			return this.isOffstage;
		}
	}

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06002285 RID: 8837 RVA: 0x000AA264 File Offset: 0x000A8664
	public bool InfluencesCamera
	{
		get
		{
			if (!base.gameManager)
			{
				return false;
			}
			if (!this.model.isDead)
			{
				return !this.IsAllyAssist && this.IsInBattle && (!this.IsEliminated || this.IsTemporary);
			}
			if (base.gameManager.EndedGame)
			{
				return this.model.isDeadForFrames < base.gameManager.Camera.cameraOptions.gameEndCameraDelayFrames;
			}
			if (base.gameManager.Camera.cameraOptions.deadPlayerFocusRespawn)
			{
				return !this.IsAllyAssist && this.IsInBattle && (!this.IsEliminated || this.IsTemporary);
			}
			return this.model.isDeadForFrames < base.gameManager.Camera.cameraOptions.deadPlayerCameraDelayFrames;
		}
	}

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06002286 RID: 8838 RVA: 0x000AA35E File Offset: 0x000A875E
	public int IsDeadForFrames
	{
		get
		{
			if (this.model.isDead)
			{
				return this.model.isDeadForFrames;
			}
			return -1;
		}
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06002287 RID: 8839 RVA: 0x000AA37D File Offset: 0x000A877D
	public bool IsFlourishMode
	{
		get
		{
			return this.State.IsCameraFlourishMode;
		}
	}

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06002288 RID: 8840 RVA: 0x000AA38A File Offset: 0x000A878A
	public bool IsZoomMode
	{
		get
		{
			return this.State.IsCameraZoomMode;
		}
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06002289 RID: 8841 RVA: 0x000AA397 File Offset: 0x000A8797
	HorizontalDirection ICameraInfluencer.Facing
	{
		get
		{
			if (this.putCameraAtRespawnPoint())
			{
				return HorizontalDirection.Right;
			}
			return this.model.facing;
		}
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x000AA3B4 File Offset: 0x000A87B4
	private bool putCameraAtRespawnPoint()
	{
		if (base.gameManager.Camera.cameraOptions.deadPlayerFocusRespawn)
		{
			if (this.State.IsRespawning)
			{
				return true;
			}
			if (this.model.isDead && this.model.isDeadForFrames >= base.gameManager.Camera.cameraOptions.deadPlayerCameraDelayFrames)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x000AA428 File Offset: 0x000A8828
	private Vector2 getCameraPosition()
	{
		if (this.putCameraAtRespawnPoint())
		{
			if (this.State.IsRespawning)
			{
				RespawnPoint respawnPointForPlayer = base.gameManager.Stage.GetRespawnPointForPlayer(this.PlayerNum);
				return respawnPointForPlayer.transform.position;
			}
			return base.gameManager.Camera.cameraOptions.approximateRespawnPoint;
		}
		else
		{
			if (!base.gameManager.Camera.cameraOptions.increaseCameraBoxAccuracy)
			{
				return (Vector2)this.Position;
			}
			if (!this.State.IsHitLagPaused)
			{
				if ((this.State.IsGrounded && !this.State.IsThrown) || this.State.IsTumbling)
				{
					this.cameraAerialMode = false;
				}
				else
				{
					this.cameraAerialMode = true;
				}
			}
			if (this.cameraAerialMode)
			{
				return this.cameraPosition;
			}
			return base.transform.position;
		}
	}

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x0600228C RID: 8844 RVA: 0x000AA52C File Offset: 0x000A892C
	public Rect CameraInfluenceBox
	{
		get
		{
			Rect result = (Rect)this.CameraBoxController.GetCameraBox(this.Facing);
			result.position += this.getCameraPosition();
			if (!base.gameManager.Camera.cameraOptions.increaseCameraBoxAccuracy && this.IsLedgeGrabbing)
			{
				result.y -= PlayerCameraBoxController.LEDGE_GRAB_BUFFER;
			}
			result.y += base.gameManager.Camera.cameraOptions.characterBoxYOffset;
			return result;
		}
	}

	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x0600228D RID: 8845 RVA: 0x000AA5C4 File Offset: 0x000A89C4
	Vector2 ICameraInfluencer.Position
	{
		get
		{
			return this.getCameraPosition();
		}
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000AA5CC File Offset: 0x000A89CC
	public Rect GetScreenspaceClearRect()
	{
		Rect rect = (Rect)this.GetBoundsForScreenSpaceClear();
		Vector3 vector = base.gameManager.Camera.current.WorldToScreenPoint(new Vector2(rect.xMin, rect.yMin - rect.height));
		Vector3 a = base.gameManager.Camera.current.WorldToScreenPoint(new Vector2(rect.xMax, rect.yMin));
		Rect result = new Rect(vector, a - vector);
		return result;
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000AA664 File Offset: 0x000A8A64
	public FixedRect GetBoundsForScreenSpaceClear()
	{
		FixedRect cameraBox = this.CameraBoxController.GetCameraBox(this.Facing);
		Fixed @fixed = 1;
		Fixed fixed2 = 1;
		cameraBox.dimensions.x = cameraBox.dimensions.x - @fixed;
		cameraBox.dimensions.y = cameraBox.dimensions.y - fixed2;
		cameraBox.position.x = cameraBox.position.x + @fixed / 2;
		cameraBox.position.y = cameraBox.position.y - fixed2 / 2;
		cameraBox.position += this.Position;
		return cameraBox;
	}

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06002290 RID: 8848 RVA: 0x000AA71B File Offset: 0x000A8B1B
	public Vector3 Velocity
	{
		get
		{
			return (Vector3)this.Physics.Velocity;
		}
	}

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06002291 RID: 8849 RVA: 0x000AA72D File Offset: 0x000A8B2D
	public EnvironmentBounds Bounds
	{
		get
		{
			return this.physics.Bounds;
		}
	}

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06002292 RID: 8850 RVA: 0x000AA73A File Offset: 0x000A8B3A
	public GrabData GrabData
	{
		get
		{
			return this.model.grabData;
		}
	}

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06002293 RID: 8851 RVA: 0x000AA747 File Offset: 0x000A8B47
	public TeamNum Team
	{
		get
		{
			return this.Reference.Team;
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06002294 RID: 8852 RVA: 0x000AA754 File Offset: 0x000A8B54
	public IBoneController Bones
	{
		get
		{
			return this.hitBoxController;
		}
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06002295 RID: 8853 RVA: 0x000AA75C File Offset: 0x000A8B5C
	// (set) Token: 0x06002296 RID: 8854 RVA: 0x000AA764 File Offset: 0x000A8B64
	public MaterialAnimationsController MaterialAnimationsController { get; private set; }

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x06002297 RID: 8855 RVA: 0x000AA76D File Offset: 0x000A8B6D
	public bool IsLedgeGrabbing
	{
		get
		{
			return this.LedgeGrabController.IsLedgeGrabbing;
		}
	}

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06002298 RID: 8856 RVA: 0x000AA77A File Offset: 0x000A8B7A
	// (set) Token: 0x06002299 RID: 8857 RVA: 0x000AA782 File Offset: 0x000A8B82
	public IPlayerStateActor StateActor { get; set; }

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x0600229A RID: 8858 RVA: 0x000AA78B File Offset: 0x000A8B8B
	AudioManager IPlayerDataOwner.Audio
	{
		get
		{
			return base.audioManager;
		}
	}

	// Token: 0x170007A4 RID: 1956
	// (get) Token: 0x0600229B RID: 8859 RVA: 0x000AA793 File Offset: 0x000A8B93
	// (set) Token: 0x0600229C RID: 8860 RVA: 0x000AA7A0 File Offset: 0x000A8BA0
	int IPlayerInputActor.ButtonsPressedThisFrame
	{
		get
		{
			return this.model.buttonsPressedThisFrame;
		}
		set
		{
			this.model.buttonsPressedThisFrame = value;
		}
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x0600229D RID: 8861 RVA: 0x000AA7AE File Offset: 0x000A8BAE
	// (set) Token: 0x0600229E RID: 8862 RVA: 0x000AA7BB File Offset: 0x000A8BBB
	int IPlayerInputActor.LastBackTapFrame
	{
		get
		{
			return this.model.lastBackTapFrame;
		}
		set
		{
			this.model.lastBackTapFrame = value;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x0600229F RID: 8863 RVA: 0x000AA7C9 File Offset: 0x000A8BC9
	// (set) Token: 0x060022A0 RID: 8864 RVA: 0x000AA7D6 File Offset: 0x000A8BD6
	int IPlayerInputActor.LastTechFrame
	{
		get
		{
			return this.model.lastTechFrame;
		}
		set
		{
			this.model.lastTechFrame = value;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x060022A1 RID: 8865 RVA: 0x000AA7E4 File Offset: 0x000A8BE4
	// (set) Token: 0x060022A2 RID: 8866 RVA: 0x000AA7F1 File Offset: 0x000A8BF1
	int IPlayerInputActor.FallThroughPlatformHeldFrames
	{
		get
		{
			return this.model.fallThroughPlatformHeldFrames;
		}
		set
		{
			this.model.fallThroughPlatformHeldFrames = value;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x060022A3 RID: 8867 RVA: 0x000AA7FF File Offset: 0x000A8BFF
	bool IPlayerDataOwner.AreInputsLocked
	{
		get
		{
			return !base.gameManager.StartedGame && !base.config.uiuxSettings.emotiveStartup;
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000AA827 File Offset: 0x000A8C27
	public bool ReadAnyBufferedInput
	{
		get
		{
			return this.ActionStateReadsAnyBufferedInput || this.MoveReadsAnyBufferedInput;
		}
	}

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x060022A5 RID: 8869 RVA: 0x000AA840 File Offset: 0x000A8C40
	public bool ActionStateReadsAnyBufferedInput
	{
		get
		{
			CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			return action != null && action.readAnyBufferedInput;
		}
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000AA879 File Offset: 0x000A8C79
	public bool MoveReadsAnyBufferedInput
	{
		get
		{
			return this.ActiveMove.Data != null && this.activeMove.Data.readAnyBufferedInput;
		}
	}

	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000AA8A4 File Offset: 0x000A8CA4
	public bool MoveDoesntReadBufferedMovement
	{
		get
		{
			return this.ActiveMove.Data != null && this.activeMove.Data.dontReadMovementInputs;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000AA8D0 File Offset: 0x000A8CD0
	bool IPlayerInputActor.TriggerHeldInputAsTaps
	{
		get
		{
			CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			return action != null && action.triggerHeldInputAsTap;
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000AA908 File Offset: 0x000A8D08
	CharacterActionData IPlayerDataOwner.ActionData
	{
		get
		{
			if (this.State.ActionState == ActionState.UsingMove)
			{
				return null;
			}
			if (this.cachedActionData == null || this.cachedActionData.characterActionState != this.State.ActionState)
			{
				this.cachedActionData = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			}
			return this.cachedActionData;
		}
	}

	// Token: 0x17000807 RID: 2055
	// (get) Token: 0x060022AA RID: 8874 RVA: 0x000AA977 File Offset: 0x000A8D77
	// (set) Token: 0x060022AB RID: 8875 RVA: 0x000AA984 File Offset: 0x000A8D84
	public bool IsActive
	{
		get
		{
			return this.model.isActive;
		}
		set
		{
			this.model.isActive = value;
			if (this.Renderer != null)
			{
				this.Renderer.SetColorModeFlag(ColorMode.Inactive, !this.model.isActive);
			}
		}
	}

	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x060022AC RID: 8876 RVA: 0x000AA9B8 File Offset: 0x000A8DB8
	public bool IsEliminated
	{
		get
		{
			return this.Lives == 0 && (base.gameManager == null || base.gameManager.ModeData.settings.usesLives);
		}
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x060022AD RID: 8877 RVA: 0x000AA9F1 File Offset: 0x000A8DF1
	public bool IsInBattle
	{
		get
		{
			return this.Reference.IsInBattle;
		}
	}

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x060022AE RID: 8878 RVA: 0x000AA9FE File Offset: 0x000A8DFE
	public bool IsAllyAssist
	{
		get
		{
			return this.Reference.IsAllyAssistMove;
		}
	}

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x060022AF RID: 8879 RVA: 0x000AAA0B File Offset: 0x000A8E0B
	public bool AssistAbsorbsHits
	{
		get
		{
			return this.model.assistAbsorbsHits;
		}
	}

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x060022B0 RID: 8880 RVA: 0x000AAA18 File Offset: 0x000A8E18
	public int TemporaryAssistImmuneFrames
	{
		get
		{
			return this.model.temporaryAssistImmunityFrames;
		}
	}

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000AAA25 File Offset: 0x000A8E25
	public bool IsTemporary
	{
		get
		{
			return this.Reference.IsTemporary;
		}
	}

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x060022B2 RID: 8882 RVA: 0x000AAA32 File Offset: 0x000A8E32
	public int TemporaryDurationFrames
	{
		get
		{
			return this.model.temporaryDurationFrames;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x060022B3 RID: 8883 RVA: 0x000AAA3F File Offset: 0x000A8E3F
	public Fixed TemporaryDurationPercent
	{
		get
		{
			return this.model.temporaryDurationFrames / this.model.temporaryDurationTotalFrames;
		}
	}

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000AAA66 File Offset: 0x000A8E66
	// (set) Token: 0x060022B5 RID: 8885 RVA: 0x000AAA6E File Offset: 0x000A8E6E
	public PlayerReference Reference { get; private set; }

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x060022B6 RID: 8886 RVA: 0x000AAA77 File Offset: 0x000A8E77
	CharacterData IPlayerDataOwner.CharacterData
	{
		get
		{
			return this.characterData;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x060022B7 RID: 8887 RVA: 0x000AAA7F File Offset: 0x000A8E7F
	CharacterMenusData IPlayerDataOwner.CharacterMenusData
	{
		get
		{
			return this.characterMenusData;
		}
	}

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x060022B8 RID: 8888 RVA: 0x000AAA88 File Offset: 0x000A8E88
	private InputController allyController
	{
		get
		{
			if (this.cachedAllyController == null && base.gameManager != null)
			{
				this.cachedAllyController = base.gameManager.getAllyReferenceWithValidController(this.Reference).InputController;
			}
			return this.cachedAllyController;
		}
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x060022B9 RID: 8889 RVA: 0x000AAAD9 File Offset: 0x000A8ED9
	private InputController inputController
	{
		get
		{
			return this.Reference.InputController;
		}
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x060022BA RID: 8890 RVA: 0x000AAAE6 File Offset: 0x000A8EE6
	private InputController nonVoidController
	{
		get
		{
			return (!(this.Reference.InputController != null)) ? this.allyController : this.Reference.InputController;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x060022BB RID: 8891 RVA: 0x000AAB14 File Offset: 0x000A8F14
	public IRespawnController RespawnController
	{
		get
		{
			return this.Reference.respawnController;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x060022BC RID: 8892 RVA: 0x000AAB21 File Offset: 0x000A8F21
	// (set) Token: 0x060022BD RID: 8893 RVA: 0x000AAB29 File Offset: 0x000A8F29
	public IAnimationPlayer AnimationPlayer { get; private set; }

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x060022BE RID: 8894 RVA: 0x000AAB32 File Offset: 0x000A8F32
	// (set) Token: 0x060022BF RID: 8895 RVA: 0x000AAB3A File Offset: 0x000A8F3A
	public IAudioOwner AudioOwner { get; private set; }

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000AAB43 File Offset: 0x000A8F43
	// (set) Token: 0x060022C1 RID: 8897 RVA: 0x000AAB4B File Offset: 0x000A8F4B
	public ICombatController Combat { get; private set; }

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000AAB54 File Offset: 0x000A8F54
	// (set) Token: 0x060022C3 RID: 8899 RVA: 0x000AAB5C File Offset: 0x000A8F5C
	public IShield Shield { get; private set; }

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x060022C4 RID: 8900 RVA: 0x000AAB65 File Offset: 0x000A8F65
	// (set) Token: 0x060022C5 RID: 8901 RVA: 0x000AAB6D File Offset: 0x000A8F6D
	public IGameVFX GameVFX { get; private set; }

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000AAB76 File Offset: 0x000A8F76
	// (set) Token: 0x060022C7 RID: 8903 RVA: 0x000AAB7E File Offset: 0x000A8F7E
	public ICharacterRenderer Renderer { get; private set; }

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x060022C8 RID: 8904 RVA: 0x000AAB87 File Offset: 0x000A8F87
	// (set) Token: 0x060022C9 RID: 8905 RVA: 0x000AAB8F File Offset: 0x000A8F8F
	public IInvincibilityController Invincibility { get; private set; }

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x060022CA RID: 8906 RVA: 0x000AAB98 File Offset: 0x000A8F98
	// (set) Token: 0x060022CB RID: 8907 RVA: 0x000AABA0 File Offset: 0x000A8FA0
	public IPlayerOrientation Orientation { get; private set; }

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x060022CC RID: 8908 RVA: 0x000AABA9 File Offset: 0x000A8FA9
	// (set) Token: 0x060022CD RID: 8909 RVA: 0x000AABB1 File Offset: 0x000A8FB1
	public TrailEmitter TrailEmitter { get; private set; }

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x060022CE RID: 8910 RVA: 0x000AABBA File Offset: 0x000A8FBA
	// (set) Token: 0x060022CF RID: 8911 RVA: 0x000AABC2 File Offset: 0x000A8FC2
	public TrailEmitter KnockbackEmitter { get; private set; }

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000AABCB File Offset: 0x000A8FCB
	// (set) Token: 0x060022D1 RID: 8913 RVA: 0x000AABD3 File Offset: 0x000A8FD3
	private ICharacterInputProcessor inputProcessor { get; set; }

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000AABDC File Offset: 0x000A8FDC
	// (set) Token: 0x060022D3 RID: 8915 RVA: 0x000AABE4 File Offset: 0x000A8FE4
	public IPlayerState State { get; private set; }

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x060022D4 RID: 8916 RVA: 0x000AABED File Offset: 0x000A8FED
	// (set) Token: 0x060022D5 RID: 8917 RVA: 0x000AABF5 File Offset: 0x000A8FF5
	public IGrabController GrabController { get; private set; }

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000AABFE File Offset: 0x000A8FFE
	// (set) Token: 0x060022D7 RID: 8919 RVA: 0x000AAC06 File Offset: 0x000A9006
	public ILedgeGrabController LedgeGrabController { get; private set; }

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x060022D8 RID: 8920 RVA: 0x000AAC0F File Offset: 0x000A900F
	// (set) Token: 0x060022D9 RID: 8921 RVA: 0x000AAC17 File Offset: 0x000A9017
	public IPlayerCameraBoxController CameraBoxController { get; private set; }

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x060022DA RID: 8922 RVA: 0x000AAC20 File Offset: 0x000A9020
	private GrabData grabData
	{
		get
		{
			return this.model.grabData;
		}
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000AAC2D File Offset: 0x000A902D
	void IItemHolder.TakeItem(IItem pItem)
	{
		if (this.heldItems != null && pItem != null)
		{
			this.heldItems.Remove(pItem);
		}
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000AAC4D File Offset: 0x000A904D
	void IItemHolder.GiveItem(IItem pItem)
	{
		if (this.heldItems != null && pItem != null)
		{
			this.heldItems.Add(pItem);
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x060022DD RID: 8925 RVA: 0x000AAC6C File Offset: 0x000A906C
	List<IItem> IItemHolder.HeldItems
	{
		get
		{
			return this.heldItems;
		}
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000AAC74 File Offset: 0x000A9074
	public void Init(PlayerSelectionInfo playerInfo, PlayerReference reference, GameModeData modeData, SpawnPointBase spawnPoint)
	{
		this.Reference = reference;
		this.staleMoveQueue = new StaleMoveQueue();
		base.injector.Inject(this.staleMoveQueue);
		this.staleMoveQueue.Init(base.config.staleMoves);
		CharacterDefinition characterDefinition = this.characterDataHelper.GetCharacterDefinition(playerInfo);
		this.characterData = this.characterDataLoader.GetData(characterDefinition);
		this.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		this.LoadComponents(this.characterData.components);
		this.modeData = modeData;
		this.customRespawnPlatform = null;
		GameObject prefab = this.respawnPlatformLocator.GetPrefab(playerInfo);
		if (prefab != null)
		{
			this.customRespawnPlatform = UnityEngine.Object.Instantiate<GameObject>(prefab);
			this.customRespawnPlatform.SetActive(false);
			this.customRespawnPlatform.transform.SetParent(null, false);
		}
		reference.respawnController.Init(reference, this.customRespawnPlatform);
		PlayerController.ComponentExecution<ICharacterInitListener> execute = delegate(ICharacterInitListener listener)
		{
			listener.OnCharacterInit(playerInfo, modeData);
			return false;
		};
		this.ExecuteCharacterComponents<ICharacterInitListener>(execute);
		this.debugText = base.gameManager.UI.GetDebugText(this.PlayerNum);
		this.skinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(this.characterData.characterID, playerInfo.skinKey));
		GameObject skinPrefab = this.characterDataHelper.GetSkinPrefab(characterDefinition, this.skinData);
		if (skinPrefab == null)
		{
			Debug.LogError("Character prefab for " + base.gameObject.name + " not found. Make sure you have selected a prefab character in the Character Editor");
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(skinPrefab);
		gameObject.transform.Translate(-(Vector3)this.characterMenusData.bounds.rotationCenterOffset);
		GameObject gameObject2 = new GameObject("Centered");
		gameObject2.transform.Translate((Vector3)this.characterMenusData.bounds.rotationCenterOffset);
		GameObject gameObject3 = new GameObject("Display");
		RotationController rotationController = gameObject2.AddComponent<RotationController>();
		GameObject gameObject4 = new GameObject("Character");
		GameObject gameObject5 = new GameObject("Audio");
		this.Orientation = new PlayerOrientation();
		base.injector.Inject(this.Orientation);
		this.Orientation.Init(rotationController, this);
		gameObject.transform.SetParent(gameObject3.transform, false);
		gameObject3.transform.SetParent(gameObject2.transform, false);
		gameObject2.transform.SetParent(gameObject4.transform, false);
		gameObject5.transform.SetParent(gameObject4.transform, false);
		this.AudioOwner = new AudioOwner();
		base.injector.Inject(this.AudioOwner);
		(this.AudioOwner as AudioOwner).Init(gameObject5, false);
		base.audioManager.Register(this.AudioOwner);
		gameObject4.transform.SetParent(base.transform);
		this.character = gameObject4;
		this.iconColor = PlayerUtil.GetColor(playerInfo, modeData.settings.usesTeams);
		this.MoveSet = new MoveSet();
		base.injector.Inject(this.MoveSet);
		(this.MoveSet as MoveSet).Init(this.characterData.moveSets[0], this.getTauntOverrides());
		this.AnimationPlayer = new AnimationController();
		base.injector.Inject(this.AnimationPlayer);
		this.AnimationPlayer.Init(this.characterMenusData.avatarData, this.character, base.config, false, false, true);
		this.AnimationPlayer.LoadMoveSet(this.characterData.moveSets[0], false);
		this.AnimationPlayer.LoadCharacterComponentAnimations(this.characterData.moveSets[0], this.characterComponents);
		this.boneData = gameObject.GetComponent<BoneData>();
		this.materialTargetsData = gameObject.GetOrAddComponent<MaterialTargetsData>();
		IAnimationDataSource animationDataSource;
		if (!Debug.isDebugBuild || base.gameManager.IsNetworkGame || this.devConfig.useLocalBakedAnimations)
		{
			animationDataSource = this.bakedAnimationDataManager.Get(this.characterData.characterName);
		}
		else
		{
			Animator componentInChildren = base.GetComponentInChildren<Animator>(true);
			animationDataSource = new RawAnimationDataSource(this.boneData, componentInChildren);
		}
		this.hitBoxController = new BoneController();
		base.injector.Inject(this.hitBoxController);
		this.hitBoxController.Init(this.characterData, this.boneData, animationDataSource, this.Orientation, this.AnimationPlayer, this, this.characterMenusData.bounds.rotationCenterOffset, gameObject4.transform, this);
		this.physics = base.gameObject.AddComponent<PlayerPhysicsController>();
		this.physics.Initialize(this);
		this.Renderer = new CharacterRenderer();
		base.injector.Inject(this.Renderer);
		(this.Renderer as CharacterRenderer).Init(this, new List<Renderer>(this.character.GetComponentsInChildren<Renderer>()), modeData.settings.usesTeams);
		this.Renderer.SetColorModeFlag(ColorMode.Inactive, !this.IsActive);
		foreach (MeshRenderer meshRenderer in this.character.GetComponentsInChildren<MeshRenderer>())
		{
			if (meshRenderer.materials.Length == 1 && meshRenderer.materials[0].name == "Character_Shadow (Instance)")
			{
				meshRenderer.gameObject.SetActive(false);
			}
		}
		this.Invincibility = new InvincibilityController();
		base.injector.Inject(this.Invincibility);
		(this.Invincibility as InvincibilityController).Init(this.Renderer, base.gameManager, this.hitBoxController);
		this.GameVFX = base.injector.GetInstance<IGameVFX>();
		(this.GameVFX as GameVFX).Initialize(base.gameManager.DynamicObjects, this.hitBoxController, this, this.physics, base.config, base.gameManager.Audio, true, this.onParticleCreated);
		this.model.rotation = default(Vector3F);
		this.model.damage = 0;
		this.thisProfile = playerInfo.curProfile;
		this.StateActor = base.injector.GetInstance<PlayerStateActor>().Setup(this, this.inputController, this.MoveSet, this, base.gameManager, this.AudioOwner, base.config);
		this.inputProcessor = base.injector.GetInstance<CharacterInputProcessor>().Setup(this, this.StateActor, this.AudioOwner, base.config, base.gameManager);
		this.State = new PlayerState(this.StateActor, this, base.config, base.battleServerAPI, base.gameManager.FrameController);
		this.Combat = base.injector.GetInstance<PlayerCombatController>();
		this.Combat.Setup(this, base.config, base.events, base.gameManager, base.gameManager.Physics, gameObject3);
		this.debugTextController = new PlayerDebugText(this, this.physics, base.config);
		this.GrabController = new GrabController(this, base.config, this.MoveSet, base.gameManager, base.gameManager);
		this.LedgeGrabController = new PlayerLedgeGrabController(this, base.gameManager, base.gameManager.Stage, base.config.ledgeConfig);
		this.CameraBoxController = new PlayerCameraBoxController(this);
		this.moveUseTracker = new MoveUseTracker(this);
		this.Shield = base.gameObject.AddComponent<ShieldController>();
		base.injector.Inject(this.Shield);
		MoveData[] moves = this.MoveSet.GetMoves(MoveLabel.ShieldGust);
		this.Shield.Initialize(this, base.config.shieldConfig, moves, base.gameManager);
		MoveContext moveContext = new MoveContext();
		moveContext.playerDelegate = this;
		moveContext.hitOwner = this;
		moveContext.gameManager = base.gameManager;
		this.activeMove = base.injector.GetInstance<MoveController>();
		this.activeMove.Init(moveContext);
		this.TrailEmitter = new GameObject("TrailEmitter").AddComponent<TrailEmitter>();
		base.injector.Inject(this.TrailEmitter);
		base.gameManager.DynamicObjects.AddDynamicObject(this.TrailEmitter.gameObject);
		this.TrailEmitter.Init(this, base.config.defaultCharacterEffects.trailData);
		this.TrailEmitter.transform.SetParent(base.transform, false);
		this.KnockbackEmitter = new GameObject("KnockbackEmitter").AddComponent<TrailEmitter>();
		base.injector.Inject(this.KnockbackEmitter);
		base.gameManager.DynamicObjects.AddDynamicObject(this.KnockbackEmitter.gameObject);
		this.KnockbackEmitter.Init(this, base.config.defaultCharacterEffects.trailData);
		this.KnockbackEmitter.transform.SetParent(base.transform, false);
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			this.toggleHitBoxCapsules(true);
		}
		Vector3F initialPosition = Vector3F.up * 5;
		HorizontalDirection horizontalDirection = HorizontalDirection.Right;
		if (spawnPoint != null)
		{
			initialPosition = (Vector3F)spawnPoint.transform.position;
			horizontalDirection = spawnPoint.FacingDirection;
		}
		if (this.physics.SetInitialPosition(initialPosition))
		{
			this.StateActor.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
		}
		else
		{
			this.StateActor.StartCharacterAction(ActionState.FallStraight, null, null, true, 0, false);
		}
		this.physics.OnCollisionBoundsChanged(true);
		this.SetFacingAndRotation(horizontalDirection);
		this.FacingInterpolation = base.gameManager.Camera.GetFacingInterpolation(horizontalDirection);
		this.model.bufferedInput.inputButtonsData.facing = horizontalDirection;
		base.gameManager.events.Broadcast(new CharacterInitEvent(this));
		this.subscribeListeners();
		this.ExecuteCharacterComponents<ICharacterStartListener>(delegate(ICharacterStartListener listener)
		{
			listener.OnCharacterStart();
			return false;
		});
		this.inputProcessor.Cache();
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000AB6BB File Offset: 0x000A9ABB
	public void InitMaterials()
	{
		this.MaterialAnimationsController = base.injector.GetInstance<MaterialAnimationsController>();
		this.MaterialAnimationsController.Init(this, this.materialTargetsData);
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000AB6E0 File Offset: 0x000A9AE0
	private void subscribeListeners()
	{
		base.events.Subscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Subscribe(typeof(UpdateDamageCommand), new Events.EventHandler(this.onUpdateDamage));
		base.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		base.events.Subscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onSpawnCommand));
		base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Subscribe(typeof(AttemptTeamDynamicMoveCommand), new Events.EventHandler(this.onAttemptTeamDynamicMove));
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000AB7B4 File Offset: 0x000A9BB4
	private void unsubscribeListeners()
	{
		base.events.Unsubscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Unsubscribe(typeof(UpdateDamageCommand), new Events.EventHandler(this.onUpdateDamage));
		base.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		base.events.Unsubscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onSpawnCommand));
		base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Unsubscribe(typeof(AttemptTeamDynamicMoveCommand), new Events.EventHandler(this.onAttemptTeamDynamicMove));
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000AB888 File Offset: 0x000A9C88
	private Dictionary<ButtonPress, MoveData> getTauntOverrides()
	{
		Dictionary<ButtonPress, MoveData> dictionary = new Dictionary<ButtonPress, MoveData>();
		UserTaunts forPlayer = this.playerTauntsFinder.GetForPlayer(this.PlayerNum);
		Dictionary<TauntSlot, EquipmentID> slotsForCharacter = forPlayer.GetSlotsForCharacter(this.characterData.characterID);
		foreach (TauntSlot key in slotsForCharacter.Keys)
		{
			EquipmentID id = slotsForCharacter[key];
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item != null)
			{
				ButtonPress key2 = ButtonPress.None;
				switch (key)
				{
				case TauntSlot.UP:
					key2 = ButtonPress.TauntUp;
					break;
				case TauntSlot.DOWN:
					key2 = ButtonPress.TauntDown;
					break;
				case TauntSlot.LEFT:
					key2 = ButtonPress.TauntLeft;
					break;
				case TauntSlot.RIGHT:
					key2 = ButtonPress.TauntRight;
					break;
				}
				MoveData[] moves = this.characterData.moveSets[0].moves;
				MoveData value = null;
				EquipmentTypes type = item.type;
				if (type != EquipmentTypes.EMOTE)
				{
					if (type != EquipmentTypes.HOLOGRAM)
					{
						if (type == EquipmentTypes.VOICE_TAUNT)
						{
							for (int i = 0; i < moves.Length; i++)
							{
								if (moves[i].label == MoveLabel.VoiceLineTaunt)
								{
									value = moves[i];
									break;
								}
							}
						}
					}
					else
					{
						for (int j = 0; j < moves.Length; j++)
						{
							if (moves[j].label == MoveLabel.Hologram)
							{
								value = moves[j];
								break;
							}
						}
					}
				}
				else
				{
					EmoteData emoteData = this.itemLoader.LoadAsset<EmoteData>(item);
					if (emoteData != null)
					{
						value = ((!this.characterData.isPartner) ? emoteData.primaryData : emoteData.partnerData);
					}
				}
				dictionary[key2] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000ABA80 File Offset: 0x000A9E80
	public void LoadSharedAnimations(List<PlayerReference> allPlayers)
	{
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < allPlayers.Count; i++)
		{
			foreach (PlayerController playerController in allPlayers[i].AllControllers)
			{
				if (!hashSet.Contains(playerController.CharacterData.characterName))
				{
					hashSet.Add(playerController.CharacterData.characterName);
					this.AnimationPlayer.LoadSharedAnimations(playerController.MoveSet.MoveSetData, playerController.CharacterData);
				}
				foreach (CharacterDefinition characterDefinition in this.characterDataHelper.GetLinkedCharacters(playerController.CharacterData.characterDefinition))
				{
					if (!hashSet.Contains(playerController.CharacterData.characterName))
					{
						hashSet.Add(playerController.CharacterData.characterName);
						this.AnimationPlayer.LoadSharedAnimations(playerController.MoveSet.MoveSetData, playerController.CharacterData);
					}
				}
			}
		}
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000ABBB8 File Offset: 0x000A9FB8
	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000ABBE4 File Offset: 0x000A9FE4
	private void mirrorPlayer(HorizontalDirection facing)
	{
		bool flag = facing == HorizontalDirection.Left;
		this.hitBoxController.InvertHurtBoxes(flag);
		this.AnimationPlayer.SetMecanimMirror(flag);
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000ABC18 File Offset: 0x000AA018
	public void SetFacingAndRotation(HorizontalDirection direction)
	{
		this.SetFacing(direction);
		this.SetRotation(direction, true);
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000ABC2C File Offset: 0x000AA02C
	public void SetRotation(HorizontalDirection direction, bool allowMirror = true)
	{
		if (this.characterData.reversesStance && allowMirror)
		{
			this.mirrorPlayer(this.Facing);
		}
		this.Orientation.RotateY((direction != HorizontalDirection.Right) ? -90 : 90);
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000ABC7B File Offset: 0x000AA07B
	public void SetFacing(HorizontalDirection direction)
	{
		this.Facing = direction;
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000ABC84 File Offset: 0x000AA084
	public void TickFrame()
	{
		if (this.model.isDead)
		{
			this.model.isDeadForFrames++;
		}
		if (this.model.teamPowerMoveCooldownFrames > 0)
		{
			this.model.teamPowerMoveCooldownFrames--;
		}
		if (this.IsInBattle)
		{
			this.inBattleTickFrame();
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000ABCE9 File Offset: 0x000AA0E9
	public void PlayDelayedParticles()
	{
		this.GameVFX.PlayDelayedParticles();
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000ABCF8 File Offset: 0x000AA0F8
	private void inBattleTickFrame()
	{
		ManualProfileUtil.StartTracking();
		if (this.model.blastZoneImmunityFrames > 0)
		{
			this.model.blastZoneImmunityFrames--;
		}
		if (this.model.ledgeGrabCooldownFrames > 0)
		{
			this.model.ledgeGrabCooldownFrames--;
		}
		if (this.model.emoteCooldownFrames > 0)
		{
			this.model.emoteCooldownFrames--;
			if (this.model.emoteCooldownFrames == 0)
			{
				base.signalBus.GetSignal<HideEmoteCooldownSignal>().Dispatch(this.PlayerNum);
			}
		}
		if (base.config.tauntSettings.useEmotesPerTime && base.battleServerAPI.IsSinglePlayerNetworkGame && this.model.emoteFrameLimitStart + base.config.tauntSettings.emotesPerTimeFrames == base.gameManager.Frame)
		{
			base.signalBus.GetSignal<HideEmoteCooldownSignal>().Dispatch(this.PlayerNum);
		}
		this.isOffstage = PlayerUtil.IsOffstage(this, base.gameManager, (Fixed)0.8999999761581421);
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.AttackAssist) && this.InputController.AttackAssistThisFrame)
		{
			this.physics.PlayerState.fallSpeedMultiplier = ((!this.isOffstage || !this.State.IsInControl || this.State.IsHelpless) ? 1 : base.config.attackAssistOffstageFallSpeedMultiplier);
		}
		this.Combat.TickFrame();
		this.StateActor.TickActionState();
		this.checkFastFallBuffer();
		if (!this.IsAllyAssist)
		{
			this.MaterialAnimationsController.TickFrame();
		}
		bool flag = false;
		InputButtonsData inputButtonsData = InputButtonsData.EmptyInput;
		if (this.IsActive && (base.gameManager.StartedGame || base.config.uiuxSettings.emotiveStartup))
		{
			inputButtonsData = this.ProcessInput(false);
			if (this.isStateForUserIdle() && !inputButtonsData.IsAnyInput())
			{
				flag = true;
				this.model.userIdleFrames++;
			}
			this.StateActor.CheckShielding();
		}
		if (!flag)
		{
			this.model.userIdleFrames = 0;
		}
		this.checkQueuedWavedash();
		int num = base.gameManager.Hits.CheckBodyOverlap(this);
		if (num != 0)
		{
			Vector3F delta = new Vector3F(base.config.knockbackConfig.shoveSpeed * num, 0);
			this.physics.ForceTranslate(delta, false, true);
		}
		int iterations = this.calculatePhysicsIterations();
		this.tickStun(iterations);
		this.tickDownedState();
		this.Shield.TickFrame(inputButtonsData);
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			if (this.characterComponents[i] is ITickCharacterComponent)
			{
				(this.characterComponents[i] as ITickCharacterComponent).TickFrame(inputButtonsData);
			}
		}
		this.physics.State.characterPhysicsOverride = this.getCharacterPhysicsOverrideFromComponents();
		if (!this.State.IsHitLagPaused && this.activeMove.IsActive)
		{
			this.activeMove.TickFrame(inputButtonsData);
			if (this.activeMove.IsActive)
			{
				this.ExecuteCharacterComponents<IMoveTickListener>(this.tickMoveComponent);
			}
		}
		if (this.AnimationPlayer != null)
		{
			int gameFrame = (!this.State.IsMoveActive) ? this.model.actionStateFrame : (this.ActiveMove.Model.gameFrame - 1);
			this.AnimationPlayer.Update(gameFrame);
			CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			if (action != null && this.Bones.HasRootMotion && !this.isSuppressRootMotionOnGrabRelease(action))
			{
				Vector3F vector3F = (!this.State.IsGrounded) ? Vector3F.up : this.Physics.GroundedNormal;
				Vector3F perpendicularVector = MathUtil.GetPerpendicularVector(vector3F);
				Vector3F deltaPosition = this.Bones.DeltaPosition;
				if (deltaPosition != Vector3F.zero && (FixedMath.Abs(deltaPosition.x) > BoneController.MIN_ROOT_MOTION || FixedMath.Abs(deltaPosition.y) > BoneController.MIN_ROOT_MOTION))
				{
					Vector3F delta2 = new Vector3F(Vector3F.Dot(deltaPosition, perpendicularVector), Vector3F.Dot(deltaPosition, vector3F), 0);
					this.physics.ForceTranslate(delta2, false, true);
				}
			}
		}
		this.Renderer.TickFrame();
		this.LedgeGrabController.TickFrame();
		this.Invincibility.TickFrame();
		this.character.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.physics.TickFrame(iterations);
		this.Bones.TickFrame();
		if (base.gameManager.StartedGame)
		{
			MoveLabel fromMove = MoveLabel.None;
			if (this.activeMove.IsActive)
			{
				fromMove = this.activeMove.Data.label;
			}
			this.inputProcessor.ChangeStateIfNecessary(this.nonVoidController, fromMove);
			this.physics.ProcessBoundsIfDirty();
		}
		this.Orientation.TickFrame();
		if ((this.activeMove.MightCollide || this.Shield.IsGusting) && !this.State.IsHitLagPaused)
		{
			this.activeMove.UpdateHitboxPositions();
			base.gameManager.Hits.QueueCollisionCheck(this);
		}
		for (int j = this.model.hostedHits.Count - 1; j >= 0; j--)
		{
			HostedHit hostedHit = this.model.hostedHits[j];
			hostedHit.TickFrame();
			if (hostedHit.IsDead)
			{
				this.model.hostedHits.RemoveAt(j);
				if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
				{
					for (int k = 0; k < hostedHit.hitBoxes.Count; k++)
					{
						HitBoxState key = hostedHit.hitBoxes[j];
						if (this.model.hostedHitCapsules.ContainsKey(key))
						{
							this.model.hostedHitCapsules[key].Clear();
							this.model.hostedHitCapsules.Remove(key);
						}
					}
				}
			}
		}
		this.calculateCharacterVisualBounds();
		if (this.State.IsRespawning)
		{
			Vector3F position = this.RespawnController.Position;
			position.z = this.physics.State.position.z;
			this.Physics.SetPosition(position);
		}
		if (this.State.IsGrounded)
		{
			this.moveUseTracker.Grounded();
		}
		if (this.model.temporaryAssistImmunityFrames > 0)
		{
			this.model.temporaryAssistImmunityFrames--;
		}
		if (this.shouldTickDownAssistDuration())
		{
			this.model.temporaryDurationFrames--;
			if (this.model.temporaryDurationFrames <= 0)
			{
				base.events.Broadcast(new DespawnCharacterCommand(this.PlayerNum, this.Team));
			}
		}
		if (this.IsActive && this.IsAllyAssist && !this.activeMove.IsActive && !this.State.IsGrabbedState)
		{
			base.events.Broadcast(new DespawnCharacterCommand(this.PlayerNum, this.Team));
		}
		if (base.gameManager.Stage != null && !FixedMath.rectContainsPoint((FixedRect)base.gameManager.Stage.SimulationData.BlastZoneBounds, this.Physics.GetPosition(), true) && !this.State.IsDead && !this.State.IsImmuneToBlastZone && !this.State.IsRespawning && (this.Physics.GetPosition().y < (Fixed)((double)base.gameManager.Stage.SimulationData.BlastZoneBounds.y) || this.State.CanDieOffTop))
		{
			this.CharacterDeath(true);
		}
		if (this.State.ActionState != this.AnimationPlayer.CurrentActionState && this.State.ActionState != ActionState.UsingMove && !this.PreventActionStateAnimations)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Action State mismatch: ",
				this.State.ActionState,
				" ",
				this.AnimationPlayer.CurrentActionState
			}));
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000AC614 File Offset: 0x000AAA14
	private int calculatePhysicsIterations()
	{
		int result = 1;
		if (base.config.knockbackConfig.useBalloonKnockback && this.State.IsStunned && !this.State.IsHitLagPaused && this.model.knockbackIteration > 0 && this.Physics.KnockbackVelocity.sqrMagnitude >= base.config.knockbackConfig.balloonKnockbackRequiredMomentum * base.config.knockbackConfig.balloonKnockbackRequiredMomentum)
		{
			result = this.model.knockbackIteration;
		}
		return result;
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000AC6B8 File Offset: 0x000AAAB8
	private bool isSuppressRootMotionOnGrabRelease(CharacterActionData characterAction)
	{
		if (characterAction.characterActionState == ActionState.Thrown && this.GrabController.GrabbingOpponent > this.PlayerNum)
		{
			PlayerController playerController = base.gameController.currentGame.GetPlayerController(this.GrabController.GrabbingOpponent);
			if (playerController.ActiveMove.WillReleaseGrabNextTick(playerController.ActiveMove.Model))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000AC722 File Offset: 0x000AAB22
	public Vector3 GetCharacterTextureCameraPosition()
	{
		return this.characterTextureOffset;
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000AC72C File Offset: 0x000AAB2C
	private void calculateCharacterVisualBounds()
	{
		Vector3F bonePosition = this.Body.GetBonePosition(this.visualBoundsBodyParts[0], false);
		Fixed @fixed = bonePosition.y;
		Fixed fixed2 = bonePosition.y;
		Fixed fixed3 = bonePosition.x;
		Fixed fixed4 = bonePosition.x;
		for (int i = 1; i < this.visualBoundsBodyParts.Count; i++)
		{
			bonePosition = this.Body.GetBonePosition(this.visualBoundsBodyParts[i], false);
			fixed2 = FixedMath.Max(bonePosition.y, fixed2);
			@fixed = FixedMath.Min(bonePosition.y, @fixed);
			fixed3 = FixedMath.Min(bonePosition.x, fixed3);
			fixed4 = FixedMath.Max(bonePosition.x, fixed4);
		}
		float x = base.transform.position.x;
		float num = (float)@fixed;
		float num2 = (float)fixed2;
		float num3 = (num2 + num) / 2f;
		float num4 = (float)this.Physics.GetPosition().y - num3;
		this.characterTextureOffset = new Vector3((this.Facing != HorizontalDirection.Right) ? (-this.characterData.textureCameraOffset.x) : this.characterData.textureCameraOffset.x, -num4 + this.characterData.textureCameraOffset.y, this.characterData.textureCameraOffset.z);
		this.cameraPosition = new Vector2(x, num);
		this.visualBounds = new FixedRect(fixed3, fixed2, fixed4 - fixed3, fixed2 - @fixed);
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000AC8C4 File Offset: 0x000AACC4
	private bool tickMoveComponentFn(IMoveTickListener listener)
	{
		listener.OnTickMove(this.activeMove);
		return false;
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000AC8D3 File Offset: 0x000AACD3
	private bool onLandComponentFn(ILandListener listener)
	{
		listener.OnLand();
		return false;
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000AC8DC File Offset: 0x000AACDC
	private bool onDeathComponentFn(IDeathListener listener)
	{
		listener.OnDeath();
		return false;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000AC8E5 File Offset: 0x000AACE5
	private bool onGrabComponentFn(IGrabListener listener)
	{
		listener.OnGrabbed();
		return false;
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000AC8EE File Offset: 0x000AACEE
	private bool onDropComponentFn(IDropListener listener)
	{
		listener.OnDrop();
		return false;
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000AC8F7 File Offset: 0x000AACF7
	private bool onJumpComponentFn(IJumpListener listener)
	{
		listener.OnJump();
		return false;
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000AC900 File Offset: 0x000AAD00
	private bool onFlinchedComponentFn(IFlinchListener listener)
	{
		listener.OnFlinch();
		return false;
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000AC909 File Offset: 0x000AAD09
	private bool onMoveUsedComponentFn(IMoveUsedListener listener, MoveData move)
	{
		listener.OnMoveUsed(move);
		return false;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000AC914 File Offset: 0x000AAD14
	public void UpdateDebugText()
	{
		if (this.debugText != null && base.gameManager.UI.DebugTextEnabled && this.IsActive)
		{
			this.debugText.text = this.debugTextController.DebugString;
		}
		else if (this.debugText != null)
		{
			this.debugText.text = string.Empty;
		}
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000AC98E File Offset: 0x000AAD8E
	private bool isStateForUserIdle()
	{
		return this.Model.state == MetaState.LedgeHang || this.Model.state == MetaState.Stand;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000AC9B4 File Offset: 0x000AADB4
	private bool shouldTickDownAssistDuration()
	{
		CrewBattleCustomData crewBattleCustomData = this.modeData.customData as CrewBattleCustomData;
		bool flag = false;
		if (crewBattleCustomData != null)
		{
			flag = crewBattleCustomData.endAssistOnKill;
		}
		return this.Reference.IsTemporary && !this.State.IsRespawning && this.model.temporaryDurationFrames > 0 && (this.isOpponentActive() || flag);
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000ACA34 File Offset: 0x000AAE34
	private bool isOpponentActive()
	{
		for (int i = 0; i < base.gameManager.CharacterControllers.Count; i++)
		{
			PlayerController playerController = base.gameManager.CharacterControllers[i];
			if (playerController.IsInBattle)
			{
				if (playerController.Team != this.Team)
				{
					if (!playerController.Reference.IsTemporary && !playerController.Reference.IsAllyAssistMove)
					{
						if (!playerController.Model.isRespawning)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000ACAD8 File Offset: 0x000AAED8
	private void toggleHitBoxCapsules(bool enabled)
	{
		if (enabled)
		{
			foreach (Hit hit in this.Model.hostedHits)
			{
				foreach (HitBoxState hitBoxState in hit.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.Transform);
					capsule.Load((Vector3)hitBoxState.position, (Vector3)hitBoxState.lastPosition, hitBoxState.data.radius, hit.data.DebugDrawColor, hitBoxState.IsCircle);
					this.model.hostedHitCapsules[hitBoxState] = capsule;
				}
			}
		}
		else
		{
			foreach (CapsuleShape capsuleShape in this.Model.hostedHitCapsules.Values)
			{
				capsuleShape.Clear();
			}
			this.Model.hostedHitCapsules.Clear();
		}
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000ACC48 File Offset: 0x000AB048
	private void checkFastFallBuffer()
	{
		bool flag = this.physics.Velocity.y < 0;
		if (!this.model.wasFastFallVelocity && flag)
		{
			this.ProcessBufferedInput();
		}
		this.model.wasFastFallVelocity = flag;
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x000ACC98 File Offset: 0x000AB098
	public InputButtonsData ProcessInput(bool retainBuffer)
	{
		this.inputProcessor.ProcessInput(base.gameManager.Frame, this.nonVoidController, this.Reference, retainBuffer);
		return this.inputProcessor.CurrentInputData;
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000ACCC8 File Offset: 0x000AB0C8
	public bool ProcessBufferedInput()
	{
		if (this.model.processingBufferedInput)
		{
			return false;
		}
		if (!this.IsActive)
		{
			return false;
		}
		bool result = false;
		if (this.model.bufferedInput.inputButtonsData.currentFrame > base.gameManager.Frame)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"ERROR: Found buffered input from frame ",
				this.model.bufferedInput.inputButtonsData.currentFrame,
				" when the current frame is ",
				base.gameManager.Frame
			}));
		}
		this.model.processingBufferedInput = true;
		bool flag = base.gameManager.Frame - this.model.bufferedInput.inputButtonsData.currentFrame <= base.config.inputConfig.inputBufferFrames;
		bool flag2 = this.model.bufferedInput.inputButtonsData.currentFrame != 0 && this.ReadAnyBufferedInput;
		if (flag || flag2)
		{
			if (!flag && this.MoveReadsAnyBufferedInput && !this.ActionStateReadsAnyBufferedInput && this.MoveDoesntReadBufferedMovement)
			{
				this.model.bufferedInput.inputButtonsData.movementButtonsPressed.Clear();
			}
			if (this.model.bufferedInput.inputButtonsData.facing != this.Facing)
			{
				this.model.bufferedInput.inputButtonsData.InvertFacing();
			}
			ProcessInputStateResult processInputStateResult = this.inputProcessor.ProcessInputState(this.model.bufferedInput.inputButtonsData, base.gameManager.Frame, this.nonVoidController.AllowTapJumpThisFrame, this.nonVoidController.AllowRecoveryJumpingThisFrame, this.nonVoidController.RequireDoubleTapToRunThisFrame);
			result = (processInputStateResult.triggeredJump != ButtonPress.None || processInputStateResult.triggeredNonJump);
			if ((processInputStateResult.consumeAll || processInputStateResult.triggeredJump != ButtonPress.None || processInputStateResult.triggeredNonJump) && !this.physics.IsGrounded && this.model.bufferedInput.inputButtonsData.moveButtonsPressed.Count > 0)
			{
				this.model.bufferActivatedFrame = base.gameManager.Frame;
				this.model.bufferActivatedButton = this.model.bufferedInput.inputButtonsData.moveButtonsPressed[0];
			}
			if (processInputStateResult.triggeredNonJump || processInputStateResult.consumeAll)
			{
				this.model.bufferedInput.Clear();
			}
			else if (processInputStateResult.triggeredJump != ButtonPress.None)
			{
				ButtonPressUtil.ListRemove(this.model.bufferedInput.inputButtonsData.movementButtonsPressed, processInputStateResult.triggeredJump);
				ButtonPressUtil.ListRemove(this.model.bufferedInput.inputButtonsData.moveButtonsPressed, processInputStateResult.triggeredJump);
				ButtonPressUtil.ListRemove(this.model.bufferedInput.inputButtonsData.buttonsHeld, processInputStateResult.triggeredJump);
			}
		}
		else
		{
			this.model.bufferedInput.Clear();
		}
		this.model.processingBufferedInput = false;
		return result;
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000AD009 File Offset: 0x000AB409
	void PlayerStateActor.IPlayerActorDelegate.ClearBufferedInput()
	{
		this.model.bufferedInput.Clear();
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000AD01C File Offset: 0x000AB41C
	private void ForceGetUp()
	{
		this.SetMove(this.MoveSet.GetMove(MoveLabel.GetUp), InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000AD05A File Offset: 0x000AB45A
	private void tickStun(int iterations)
	{
		this.ReduceStunFrames(iterations);
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000AD064 File Offset: 0x000AB464
	public void ReduceStunFrames(int frames)
	{
		if (this.model.stunFrames <= 0 || this.State.IsHitLagPaused || this.State.IsDead || frames <= 0)
		{
			return;
		}
		this.model.stunFrames -= frames;
		this.model.smokeTrailFrames -= frames;
		if (this.model.jumpStunFrames > 0)
		{
			this.model.jumpStunFrames -= frames;
			if (this.model.jumpStunFrames <= 0)
			{
				this.ProcessBufferedInput();
			}
		}
		if (this.model.stunFrames <= 0)
		{
			StunType stunType = this.model.stunType;
			if (stunType != StunType.ForceGetUpHitStun)
			{
				if (stunType != StunType.ShieldBreakStun)
				{
					this.StateActor.ReleaseStun();
				}
				else
				{
					this.StateActor.ReleaseShieldBreak();
				}
			}
			else
			{
				this.ForceGetUp();
			}
		}
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x000AD168 File Offset: 0x000AB568
	private void tickDownedState()
	{
		if (!this.State.IsDownState || this.State.IsHitLagPaused)
		{
			return;
		}
		if (!this.State.IsGrounded)
		{
			Debug.LogWarning("Downed while not grounded! This should not happen");
			this.State.MetaState = MetaState.Jump;
			return;
		}
		this.model.downedFrames++;
		if (this.State.IsShieldBroken)
		{
			this.StateActor.BeginDaze();
		}
		else if (this.model.downedFrames >= base.config.knockbackConfig.maxDownedFrames)
		{
			this.SetMove(this.MoveSet.GetMove(MoveLabel.GetUp), InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
		}
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06002305 RID: 8965 RVA: 0x000AD244 File Offset: 0x000AB644
	bool IBoundsOwner.AllowPushing
	{
		get
		{
			return this.IsActive && this.IsInBattle && this.State.IsGrounded && !this.isShovingForbidden() && this.physics.Velocity.magnitude < base.config.knockbackConfig.ignoreShoveThreshold;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06002306 RID: 8966 RVA: 0x000AD2AD File Offset: 0x000AB6AD
	bool IBoundsOwner.AllowTotalShove
	{
		get
		{
			return this.ActiveMove.IsActive && this.ActiveMove.Model.data.shovesEnemies;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06002307 RID: 8967 RVA: 0x000AD2D8 File Offset: 0x000AB6D8
	private bool allowBeingPushed
	{
		get
		{
			return !this.isShovingForbidden() && this.physics.Velocity.magnitude < base.config.knockbackConfig.ignoreShoveThreshold;
		}
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x000AD31C File Offset: 0x000AB71C
	private bool isShovingForbidden()
	{
		if (!this.IsActive || !this.IsInBattle || this.IsAllyAssist || this.State.IsDownState || this.State.IsDownedLooping || this.State.IsGrabbedState)
		{
			return true;
		}
		if (this.ActiveMove.IsActive)
		{
			MoveLabel label = this.ActiveMove.Data.label;
			if (label == MoveLabel.Sidestep || label == MoveLabel.ShieldBackwardRoll || label == MoveLabel.ShieldForwardRoll)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06002309 RID: 8969 RVA: 0x000AD3C0 File Offset: 0x000AB7C0
	bool ITrailOwner.EmitTrail
	{
		get
		{
			return this.IsActive && !this.State.IsDead && ((!this.State.IsGrounded && this.State.IsTumbling && this.State.IsStunned && !this.State.IsHitLagPaused && this.Model.smokeTrailFrames >= 0) || (this.activeMove != null && this.activeMove.IsActive && this.activeMove.EmitTrail));
		}
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000AD465 File Offset: 0x000AB865
	public void ClearDamage()
	{
		this.Model.damage = 0;
		this.staleMoveQueue.Clear();
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000AD484 File Offset: 0x000AB884
	public void CharacterDeath(bool isPrimaryDeath)
	{
		Vector3F velocity = this.physics.Velocity;
		this.model.isDead = true;
		this.model.isDeadForFrames = 0;
		this.physics.ResetStateOnDeath();
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.Died);
		this.ClearDamage();
		this.ExecuteCharacterComponents<IDeathListener>(this.onDeath);
		this.StateActor.StartCharacterAction(ActionState.Death, null, null, true, 0, false);
		bool wasEliminated = false;
		if (isPrimaryDeath)
		{
			if (!this.IsTemporary)
			{
				base.gameManager.events.Broadcast(new DeductPlayerLifeCommand(this));
				wasEliminated = this.IsEliminated;
			}
			base.events.Broadcast(new LogStatEvent(StatType.Death, 1, PointsValueType.Addition, this.PlayerNum, this.Team));
			base.gameManager.events.Broadcast(new CharacterDeathEvent(this, wasEliminated, velocity));
			base.gameManager.Camera.OnCharacterDeath(this, velocity);
		}
		this.Renderer.ToggleVisibility(false);
		this.onRemovedFromGame();
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000AD578 File Offset: 0x000AB978
	public void Despawn()
	{
		if (this.State.IsRespawning)
		{
			this.DismountRespawnPlatform();
		}
		if (!this.State.IsDead)
		{
			this.GameVFX.PlayParticle(base.config.defaultCharacterEffects.despawn, false, TeamNum.None);
		}
		this.ClearState(0);
		this.onRemovedFromGame();
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000AD5D8 File Offset: 0x000AB9D8
	private void onRemovedFromGame()
	{
		this.ExecuteCharacterComponents<IRemovedfromGameListener>(delegate(IRemovedfromGameListener listener)
		{
			listener.OnRemovedFromGame();
			return false;
		});
		this.StateActor.ReleaseShield(false, false);
		this.TrailEmitter.ResetData();
		this.KnockbackEmitter.ResetData();
		this.moveUseTracker.OnRemovedFromGame();
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000AD638 File Offset: 0x000ABA38
	public void ClearState(int startingDamage = 0)
	{
		if (this.State.IsShieldBroken)
		{
			this.StateActor.ReleaseShieldBreak();
		}
		this.LedgeGrabController.ReleaseGrabbedLedge(true, false);
		this.model.stunFrames = 0;
		this.model.knockbackIteration = 0;
		this.model.clearInputBufferOnStunEnd = false;
		this.model.smokeTrailFrames = 0;
		this.model.jumpStunFrames = 0;
		this.model.untechableBounceUsed = false;
		this.model.dashDanceFrames = 0;
		this.model.ledgeGrabCooldownFrames = 0;
		this.model.damage = startingDamage;
		this.model.landingOverrideData = null;
		this.model.helplessStateData = null;
		this.model.ClearLastHitData();
		this.EndActiveMove(MoveEndType.Cancelled, true, false);
		this.GrabController.ReleaseGrabbedOpponent(true);
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000AD718 File Offset: 0x000ABB18
	private void respawn(SpawnPointBase spawnPoint, int startingDamagePercent = 0)
	{
		this.spawnController.AddPlayerToScene(this, startingDamagePercent);
		this.model.isRespawning = true;
		base.audioManager.PlayGameSound(new AudioRequest(base.config.respawnConfig.respawnSound, this.AudioOwner, null));
		this.Invincibility.BeginPlatformIntangibility();
		if (this.MaterialAnimationsController.Overridden)
		{
			this.MaterialAnimationsController.RemoveOverride();
		}
		this.RespawnController.StartRespawn(spawnPoint);
		this.physics.SetPosition(this.RespawnController.Position);
		this.SetFacingAndRotation(spawnPoint.FacingDirection);
		this.StateActor.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
		this.State.MetaState = MetaState.Stand;
		this.ExecuteCharacterComponents<IRespawnListener>(delegate(IRespawnListener listener)
		{
			listener.OnRespawn();
			return false;
		});
		this.OnSpawned();
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000AD801 File Offset: 0x000ABC01
	public void OnSpawned()
	{
		this.Renderer.ToggleVisibility(true);
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000AD810 File Offset: 0x000ABC10
	private void onRespawnPlatformExpire(GameEvent message)
	{
		RespawnPlatformExpireEvent respawnPlatformExpireEvent = message as RespawnPlatformExpireEvent;
		if (respawnPlatformExpireEvent.playerNum == this.PlayerNum)
		{
			this.onDismountRespawnPlatform();
		}
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000AD83C File Offset: 0x000ABC3C
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent changed = message as PlayerEngagementStateChangedEvent;
		if (changed.playerNum == this.PlayerNum)
		{
			switch (changed.engagement)
			{
			case PlayerEngagementState.Primary:
			case PlayerEngagementState.Temporary:
			case PlayerEngagementState.AssistMove:
				if (!this.model.isDead)
				{
					this.Renderer.ToggleVisibility(true);
				}
				break;
			case PlayerEngagementState.Benched:
				this.model.isDead = true;
				this.model.isDeadForFrames = 0;
				this.Renderer.ToggleVisibility(false);
				break;
			case PlayerEngagementState.Disconnected:
				this.Renderer.ToggleVisibility(false);
				break;
			default:
				Debug.LogWarning(string.Concat(new object[]
				{
					"Player ",
					this.PlayerNum,
					" engagement changed to unsupported type  ",
					changed.engagement
				}));
				break;
			}
			this.ExecuteCharacterComponents<IEngagementStateListener>(delegate(IEngagementStateListener listener)
			{
				listener.OnEngagementStateChanged(changed.engagement);
				return false;
			});
		}
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000AD952 File Offset: 0x000ABD52
	public void DismountRespawnPlatform()
	{
		this.RespawnController.Dismount();
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000AD960 File Offset: 0x000ABD60
	private void onDismountRespawnPlatform()
	{
		this.Invincibility.EndPlatformIntangibility();
		this.Invincibility.BeginInvincibility(base.config.respawnConfig.dismountInvincibilityFrames);
		this.model.isRespawning = false;
		this.State.MetaState = MetaState.Jump;
		this.physics.SyncGroundState();
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000AD9B8 File Offset: 0x000ABDB8
	private CharacterPhysicsOverride getCharacterPhysicsOverrideFromComponents()
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is ICharacterPhysicsOverrideComponent)
			{
				ICharacterPhysicsOverrideComponent characterPhysicsOverrideComponent = characterComponent as ICharacterPhysicsOverrideComponent;
				if (characterPhysicsOverrideComponent.Override != null)
				{
					return characterPhysicsOverrideComponent.Override;
				}
			}
		}
		return CharacterPhysicsOverride.NoOverride;
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000ADA18 File Offset: 0x000ABE18
	private void checkQueuedWavedash()
	{
		if (this.model.queuedWavedashDodge)
		{
			if (this.State.IsJumpingState)
			{
				this.model.queuedWavedashDodge = false;
				List<ButtonPress> moveButtonsPressed = new List<ButtonPress>
				{
					ButtonPress.Shield1
				};
				InputButtonsData inputButtonsData = new InputButtonsData();
				inputButtonsData.moveButtonsPressed = moveButtonsPressed;
				if (base.gameManager.Frame - this.model.bufferedInput.inputButtonsData.currentFrame <= base.config.inputConfig.inputBufferFrames && (this.model.bufferedInput.inputButtonsData.horizontalAxisValue != 0 || this.model.bufferedInput.inputButtonsData.verticalAxisValue != 0))
				{
					inputButtonsData.horizontalAxisValue = this.model.bufferedInput.inputButtonsData.horizontalAxisValue;
					inputButtonsData.verticalAxisValue = this.model.bufferedInput.inputButtonsData.verticalAxisValue;
				}
				else
				{
					inputButtonsData.horizontalAxisValue = this.inputProcessor.CurrentInputData.horizontalAxisValue;
					inputButtonsData.verticalAxisValue = this.inputProcessor.CurrentInputData.verticalAxisValue;
				}
				inputButtonsData.facing = this.Facing;
				ButtonPress buttonPress = ButtonPress.None;
				InterruptData interruptData;
				this.SetMove(this.MoveSet.GetMove(inputButtonsData, this, out interruptData, ref buttonPress), inputButtonsData, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
			}
			else if (!this.State.IsTakingOff)
			{
				this.model.queuedWavedashDodge = false;
			}
		}
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000ADBB0 File Offset: 0x000ABFB0
	public MoveData GetAttackAssistMoveReplacement(MoveData move, InputButtonsData inputButtonsData, out Vector2F directionOut)
	{
		bool flag = inputButtonsData.buttonsHeld.Contains(ButtonPress.Strike);
		if (this.State.IsRunning && flag)
		{
			directionOut = default(Vector2F);
			return this.MoveSet.GetMoves(MoveLabel.DashAttack)[0];
		}
		if (inputButtonsData.buttonsHeld.Contains(ButtonPress.UpStrike) || inputButtonsData.buttonsHeld.Contains(ButtonPress.DownStrike) || inputButtonsData.buttonsHeld.Contains(ButtonPress.BackwardStrike) || inputButtonsData.buttonsHeld.Contains(ButtonPress.ForwardStrike))
		{
			directionOut = default(Vector2F);
			return move;
		}
		PlayerReference playerReference = PlayerUtil.FindClosestEnemy(this, base.gameManager.PlayerReferences);
		directionOut = default(Vector2F);
		if (playerReference != null)
		{
			Fixed other = (!(move.moveOverrideFarDistance > 0)) ? this.characterData.attackAssistJabRange : move.moveOverrideFarDistance;
			Vector2F vector2F = default(Vector2F);
			float multi = 0.3f;
			vector2F.x = playerReference.Controller.Center.x - this.Center.x;
			vector2F.y = playerReference.Controller.Center.y - this.Center.y;
			if (this.Physics.State.characterPhysicsOverride == CharacterPhysicsOverride.NoOverride)
			{
				directionOut = vector2F;
			}
			bool flag2 = (vector2F.x < 0 && this.Facing == HorizontalDirection.Left) || (vector2F.x > 0 && this.Facing == HorizontalDirection.Right);
			bool flag3 = (vector2F.x < 0 - this.characterData.attackAssistBairHorizontalOffset && this.Facing == HorizontalDirection.Left) || vector2F.y < this.characterData.attackAssistBairMinimumHeight || (vector2F.x > 0 + this.characterData.attackAssistBairHorizontalOffset && this.Facing == HorizontalDirection.Right);
			bool flag4 = false;
			bool flag5 = false;
			Vector2F vector2F2 = new Vector2F(this.Physics.Velocity.x * this.characterData.attackAssistVelocityOverlapMultiplier, this.Physics.Velocity.y * this.characterData.attackAssistVelocityOverlapMultiplier);
			Vector2F vector2F3 = new Vector2F(playerReference.Controller.Physics.Velocity.x * playerReference.Controller.characterData.attackAssistVelocityOverlapMultiplier, playerReference.Controller.Physics.Velocity.y * playerReference.Controller.characterData.attackAssistVelocityOverlapMultiplier);
			if (!this.State.IsGrounded)
			{
				if (this.Physics.Velocity.x < 0 && vector2F.x < 0)
				{
					directionOut.x = 0;
				}
				else if (this.Physics.Velocity.x > 0 && vector2F.x > 0)
				{
					directionOut.x = 0;
				}
				if (this.Physics.Velocity.y < 0 && vector2F.y < 0)
				{
					directionOut.y = 0;
				}
				else if (this.Physics.Velocity.y > 0 && vector2F.y > 0)
				{
					directionOut.y = 0;
				}
			}
			if (this.State.IsGrounded && inputButtonsData.buttonsHeld.Contains(ButtonPress.Strike) && vector2F.y + vector2F3.y * multi >= this.characterData.attackAssistUpStrikeMinimumRange && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) <= this.characterData.attackAssistUpStrikeHorizontalMaximum && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) >= this.characterData.attackAssistUpStrikeHorizontalMinimum)
			{
				flag4 = true;
			}
			if (this.State.IsGrounded && inputButtonsData.buttonsHeld.Contains(ButtonPress.Strike) && vector2F.y + vector2F3.y * multi <= this.characterData.attackAssistDStrikeMinimumRange && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) <= this.characterData.attackAssistDStrikeHorizontalMaximum && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) >= this.characterData.attackAssistDStrikeHorizontalMinimum)
			{
				flag5 = true;
			}
			if (this.State.IsGrounded && !inputButtonsData.buttonsHeld.Contains(ButtonPress.Strike) && vector2F.y + vector2F3.y * multi >= this.characterData.attackAssistUpTiltMinimumRange && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) <= this.characterData.attackAssistUpTiltHorizontalMaximum && FixedMath.Abs(vector2F.x - vector2F2.x * multi + vector2F3.x * multi) >= this.characterData.attackAssistUpTiltHorizontalMinimum)
			{
				flag4 = true;
			}
			bool flag6 = move.label == MoveLabel.BackwardAir || move.label == MoveLabel.ForwardAir || move.label == MoveLabel.UpAir || move.label == MoveLabel.DownAir || move.label == MoveLabel.NeutralAir;
			if (!this.State.IsGrounded && vector2F.y - vector2F2.y * multi + vector2F3.y * multi >= this.characterData.attackAssistUpAirMinimumRange)
			{
				Fixed one = FixedMath.Abs(vector2F.x - vector2F2.x + vector2F3.x);
				if (one > this.characterData.attackAssistUpAirHorizontalMinimum && one < this.characterData.attackAssistUpAirHorizontalMaximum)
				{
					flag4 = true;
				}
			}
			if (!this.State.IsGrounded && vector2F.y - vector2F2.y * multi + vector2F3.y * multi <= this.characterData.attackAssistDairMinimumRange)
			{
				Fixed other2 = this.characterData.attackAssistDairHorizontalOffset * ((this.Facing != HorizontalDirection.Left) ? -1 : 1);
				Fixed one2 = FixedMath.Abs(vector2F.x + other2 - vector2F2.x + vector2F3.x);
				if (one2 > this.characterData.attackAssistDairHorizontalMinimum && one2 < this.characterData.attackAssistDairHorizontalMaximum)
				{
					flag5 = true;
				}
			}
			if (flag6)
			{
				bool flag7 = (vector2F.x < 0 && this.Facing == HorizontalDirection.Left) || (vector2F.x > 0 && this.Facing == HorizontalDirection.Right);
				if (flag4)
				{
					return this.MoveSet.GetMoves(MoveLabel.UpAir)[0];
				}
				if (flag5)
				{
					return this.MoveSet.GetMoves(MoveLabel.DownAir)[0];
				}
				if (flag3)
				{
					return (!flag || !flag7) ? this.MoveSet.GetMoves(MoveLabel.NeutralAir)[0] : this.MoveSet.GetMoves(MoveLabel.ForwardAir)[0];
				}
				if (!flag3)
				{
					return this.MoveSet.GetMoves(MoveLabel.BackwardAir)[0];
				}
			}
			if (flag4 && move.moveOverrideAbove != null)
			{
				return move.moveOverrideAbove;
			}
			if (flag5 && move.moveOverrideBelow != null)
			{
				return move.moveOverrideBelow;
			}
			if ((move.moveOverrideFarAhead != null || move.moveOverrideFarBehind != null) && FixedMath.Abs(vector2F.x) > other)
			{
				if (move.moveOverrideFarAhead != null && move.moveOverrideFarBehind != null)
				{
					return (!flag2) ? move.moveOverrideFarBehind : move.moveOverrideFarAhead;
				}
				if (move.moveOverrideFarAhead)
				{
					return move.moveOverrideFarAhead;
				}
			}
			else
			{
				if (flag2 && move.moveOverrideAhead != null)
				{
					return move.moveOverrideAhead;
				}
				if (!flag2 && move.moveOverrideBehind != null)
				{
					return move.moveOverrideBehind;
				}
			}
		}
		directionOut = default(Vector2F);
		return move;
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000AE5E4 File Offset: 0x000AC9E4
	public void SetMove(MoveData move, InputButtonsData inputButtonsData, HorizontalDirection sideDirection = HorizontalDirection.None, int uid = 0, int startGameFrame = 0, Vector3F assistTarget = default(Vector3F), MoveTransferSettings transferSettings = null, List<MoveLinkComponentData> linkComponentData = null, MoveSeedData seedData = default(MoveSeedData), ButtonPress buttonUsed = ButtonPress.None)
	{
		if (base.gameDataManager.IsFeatureEnabled(FeatureID.AttackAssist) && base.gameManager != null && !this.IsAllyAssist && inputButtonsData != null && this.InputController != null && this.InputController.AttackAssistThisFrame)
		{
			Vector2F lhs;
			move = this.GetAttackAssistMoveReplacement(move, inputButtonsData, out lhs);
			Fixed @fixed = FixedMath.Min(lhs.magnitude, (!move.hasAttackAssistNudgeOverride) ? this.characterData.attackAssistNudgeMaxDistance : move.attackAssistNudgeMaxDistance);
			Fixed other = (!move.hasAttackAssistNudgeOverride) ? this.characterData.attackAssistNudgeMinDistance : move.attackAssistNudgeMinDistance;
			if (@fixed > other)
			{
				Fixed d = (!move.hasAttackAssistNudgeOverride) ? ((!this.State.IsGrounded) ? this.characterData.attackAssistAirNudgeMultiplier : this.characterData.attackAssistGroundNudgeMultiplier) : move.attackAssistNudgeMultiplier;
				Fixed d2 = (!move.hasAttackAssistNudgeOverride) ? ((!this.State.IsGrounded) ? this.characterData.attackAssistAirNudgeBase : this.characterData.attackAssistGroundNudgeBase) : move.attackAssistNudgeBase;
				Vector2F normalized = lhs.normalized;
				if (!move.ignoreAssistVelocity && lhs != Vector2F.zero)
				{
					this.ActiveMove.Model.addImpulse = normalized * @fixed * d + normalized * d2;
					this.ActiveMove.Model.addImpulseCountdown = 2;
					if (this.State.IsGrounded)
					{
						this.ActiveMove.Model.addImpulse.y = 0;
					}
				}
			}
		}
		if (this.Model.lastMoveName != move.moveName)
		{
			this.Model.repeatTrackMoveCount = 0;
		}
		this.Model.lastMoveName = move.moveName;
		bool isActive = this.ActiveMove.IsActive;
		bool flag = uid != 0;
		InterruptData[] array = null;
		int previousMoveFrame = 0;
		if (isActive)
		{
			array = this.ActiveMove.Data.interrupts;
			previousMoveFrame = this.ActiveMove.Model.internalFrame;
		}
		MoveModel.MoveVisualData moveVisualData = new MoveModel.MoveVisualData();
		int chargeFrames = 0;
		HorizontalDirection deferredFacing = HorizontalDirection.None;
		ChargeConfig chargeConfig = null;
		if (transferSettings == null)
		{
			transferSettings = MoveTransferSettings.Default;
		}
		if (transferSettings.transitioningToContinuingMove)
		{
			this.ActiveMove.Model.visualData.CopyTo(moveVisualData);
			deferredFacing = this.activeMove.Model.deferredFacing;
			chargeFrames = this.ActiveMove.Model.chargeFrames;
		}
		if (transferSettings.transferChargeData)
		{
			chargeFrames = this.ActiveMove.Model.chargeFrames;
			if (this.ActiveMove.Data.chargeOptions.hasOverrideChargeConfig)
			{
				chargeConfig = this.ActiveMove.Data.chargeOptions.overrideChargeConfig;
			}
		}
		if (transferSettings.transferHitDisableTargets)
		{
			this.activeMove.Model.CopyDisabledHits(this.hitDisableDataBuffer);
		}
		if (this.activeMove.IsActive)
		{
			this.EndActiveMove(MoveEndType.Continued, false, transferSettings.transitioningToContinuingMove);
		}
		if (!flag)
		{
			this.model.ignoreNextHelplessness = false;
		}
		if (this.physics.KnockbackVelocity.sqrMagnitude > 0)
		{
			this.physics.AddVelocity(this.physics.KnockbackVelocity, 1, VelocityType.Movement);
			this.physics.StopMovement(true, true, VelocityType.Knockback);
		}
		MoveModel moveModel = this.activeMove.Model;
		moveModel.Clear();
		moveModel.data = move;
		moveModel.inputDirection = sideDirection;
		moveModel.initialFacing = this.Facing;
		moveModel.gameFrame = startGameFrame;
		moveModel.assistTarget = assistTarget;
		moveModel.seedData = seedData;
		moveModel.chargeFrames = chargeFrames;
		moveModel.chargeButton = buttonUsed;
		moveModel.deferredFacing = deferredFacing;
		moveModel.chargeFractionOverride = transferSettings.chargeFractionOverride;
		moveModel.hits.Clear();
		for (int i = 0; i < moveModel.data.hitData.Length; i++)
		{
			moveModel.hits.Add(new Hit(moveModel.data.hitData[i]));
		}
		if (transferSettings.transitioningToContinuingMove)
		{
			this.ActiveMove.Model.visualData.Load(moveVisualData);
		}
		if (uid != 0)
		{
			moveModel.uid = uid;
		}
		if (move.IsTauntMove())
		{
			base.events.Broadcast(new LogStatEvent(StatType.Taunt, 1, PointsValueType.Addition, this.PlayerNum, this.Team));
		}
		if (transferSettings.transferStale)
		{
			moveModel.staleDamageMultiplier = transferSettings.transferStaleMulti;
		}
		else
		{
			moveModel.staleDamageMultiplier = this.staleMoveQueue.GetDamageMultiplier(move);
		}
		if (move.chargeOptions.hasOverrideChargeConfig)
		{
			moveModel.ChargeData = move.chargeOptions.overrideChargeConfig;
		}
		else if (chargeConfig != null)
		{
			moveModel.ChargeData = chargeConfig;
		}
		else
		{
			moveModel.ChargeData = base.config.chargeConfig;
		}
		bool flag2 = false;
		for (int j = 0; j < this.characterComponents.Count; j++)
		{
			ICharacterComponent characterComponent = this.characterComponents[j];
			if (characterComponent is IMoveControllerInitializer)
			{
				bool flag3 = (characterComponent as IMoveControllerInitializer).InitializeMoveController(this.activeMove, moveModel);
				if (flag3)
				{
					flag2 = true;
					break;
				}
			}
		}
		bool flag4 = flag2;
		if (!flag2)
		{
			bool flag5 = false;
			if (sideDirection != HorizontalDirection.None)
			{
				foreach (InputProfileEntry inputProfileEntry in move.activeInputProfile.entries)
				{
					foreach (ButtonPress press in inputProfileEntry.buttonsHeld)
					{
						if (InputUtils.GetUntapped(press) == ButtonPress.Side)
						{
							flag5 = true;
							break;
						}
					}
					if (flag5)
					{
						break;
					}
				}
			}
			if (flag5 && isActive && array != null)
			{
				foreach (InterruptData interruptData in array)
				{
					if (interruptData.interruptType == InterruptType.Move)
					{
						for (int n = 0; n < interruptData.linkableMoves.Length; n++)
						{
							MoveData moveData = interruptData.linkableMoves[n];
							if (moveData == null)
							{
								Debug.LogError("Null link " + move.moveName);
							}
							if (moveData.Equals(move) && !interruptData.allowFacingDirectionChange)
							{
								flag5 = false;
								break;
							}
						}
					}
				}
			}
			bool processMoveFrame = !transferSettings.transitioningToContinuingMove;
			flag4 = this.activeMove.Initialize(moveModel, inputButtonsData, isActive, flag5, linkComponentData, processMoveFrame);
			if (flag4 && transferSettings.transferHitDisableTargets)
			{
				this.hitDisableDataBuffer.CopyTo(this.activeMove.Model.disabledHits);
				this.activeMove.Model.disabledHits.Init(this.activeMove.Data.totalInternalFrames, this.activeMove.Model.hits, false);
				this.activeMove.Model.disabledHits.Renew(previousMoveFrame, this.activeMove.Model.internalFrame);
			}
		}
		if (flag4)
		{
			this.State.SubState = SubStates.Resting;
			if (this.State.IsDownState)
			{
				this.State.MetaState = MetaState.Stand;
			}
			this.StateActor.StartCharacterAction(ActionState.UsingMove, null, null, true, 0, false);
			this.moveUseTracker.OnMoveStart(move);
			this.ExecuteCharacterComponents<IMoveUsedListener, MoveData>(new Func<IMoveUsedListener, MoveData, bool>(this.onMoveUsedComponentFn), move);
		}
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000AEDE0 File Offset: 0x000AD1E0
	List<ButtonPress> PlayerStateActor.IPlayerActorDelegate.GetBufferableInput(InputButtonsData inputButtonsData)
	{
		CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
		if (action != null)
		{
			MoveLabel[] validBufferedMoveLabels = action.validBufferedMoveLabels;
			if (validBufferedMoveLabels != null && validBufferedMoveLabels.Length > 0)
			{
				bool flag = true;
				List<ButtonPress> list = inputButtonsData.moveButtonsPressed;
				foreach (ButtonPress item in action.maskedBufferButtons)
				{
					while (list.Contains(item))
					{
						if (flag)
						{
							flag = false;
							list = new List<ButtonPress>(inputButtonsData.moveButtonsPressed);
						}
						list.Remove(item);
					}
				}
				if (list.Count > 0)
				{
					foreach (MoveLabel label in validBufferedMoveLabels)
					{
						MoveData move = this.MoveSet.GetMove(label);
						if (move != null && this.MoveSet.CanInputBufferMove(move, list))
						{
							return list;
						}
					}
				}
				return null;
			}
		}
		return inputButtonsData.moveButtonsPressed;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000AEEF0 File Offset: 0x000AD2F0
	void PlayerStateActor.IPlayerActorDelegate.Cache(InputButtonsData inputButtonsData)
	{
		this.warmButtonDetection(inputButtonsData, ButtonPress.Forward);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Backward);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Attack);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Special);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Shield1);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Shield2);
		this.warmButtonDetection(inputButtonsData, ButtonPress.SoloAssist);
		this.warmButtonDetection(inputButtonsData, ButtonPress.SoloGust);
		this.warmButtonDetection(inputButtonsData, ButtonPress.Run);
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000AEF4C File Offset: 0x000AD34C
	private void warmButtonDetection(InputButtonsData inputButtonsData, ButtonPress button)
	{
		InterruptData interruptData = null;
		ButtonPress buttonPress = ButtonPress.None;
		inputButtonsData.moveButtonsPressed.Add(button);
		this.MoveSet.GetMove(inputButtonsData, this, out interruptData, ref buttonPress);
		inputButtonsData.moveButtonsPressed.RemoveAt(inputButtonsData.moveButtonsPressed.Count - 1);
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000AEF94 File Offset: 0x000AD394
	bool PlayerStateActor.IPlayerActorDelegate.TryBeginBufferedInterrupt(InputButtonsData inputButtonsData, bool isHighPriority)
	{
		if (!this.Model.isInterruptMoveBuffered || this.Model.bufferInterruptData == null)
		{
			return false;
		}
		if (this.IsAllyAssist || !this.State.CanUseMoves)
		{
			return false;
		}
		if (isHighPriority && !this.Model.bufferInterruptData.interruptHighPriority)
		{
			return false;
		}
		int interruptMinFrame = this.Model.bufferInterruptData.interruptMinFrame;
		if (this.ActiveMove.Model.gameFrame >= interruptMinFrame)
		{
			if (inputButtonsData.facing == HorizontalDirection.None)
			{
				inputButtonsData.facing = this.Facing;
			}
			this.beginMove(this.Model.bufferMoveData, this.Model.bufferInterruptData, this.Model.bufferButtonUsed, inputButtonsData);
			return true;
		}
		return false;
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000AF068 File Offset: 0x000AD468
	bool PlayerStateActor.IPlayerActorDelegate.TryBeginMove(InputButtonsData inputButtonsData)
	{
		if (this.IsAllyAssist || !this.State.CanUseMoves)
		{
			return false;
		}
		if (inputButtonsData.facing == HorizontalDirection.None)
		{
			inputButtonsData.facing = this.Facing;
		}
		InterruptData interrupt = null;
		ButtonPress buttonUsed = ButtonPress.None;
		MoveData move = this.MoveSet.GetMove(inputButtonsData, this, out interrupt, ref buttonUsed);
		return this.TryBeginMoveHelper(move, interrupt, buttonUsed, inputButtonsData);
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000AF0CC File Offset: 0x000AD4CC
	bool PlayerStateActor.IPlayerActorDelegate.TryBeginMove(MoveData moveData, InterruptData interruptData, ButtonPress buttonUsed, InputButtonsData inputButtonsData)
	{
		if (this.IsAllyAssist || !this.State.CanUseMoves)
		{
			return false;
		}
		if (inputButtonsData.facing == HorizontalDirection.None)
		{
			inputButtonsData.facing = this.Facing;
		}
		return this.TryBeginMoveHelper(moveData, interruptData, buttonUsed, inputButtonsData);
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000AF11C File Offset: 0x000AD51C
	public bool IsRateLimited()
	{
		if (!this.State.CanUseEmotes)
		{
			base.signalBus.GetSignal<ShowEmoteCooldownSignal>().Dispatch(this.PlayerNum);
			if (base.battleServerAPI.IsLocalPlayer(this.PlayerNum))
			{
				base.audioManager.PlayGameSound(new AudioRequest(base.config.tauntSettings.emoteCooldownSound, this.AudioOwner, null));
			}
			return true;
		}
		this.model.emoteCooldownFrames = base.config.tauntSettings.emoteCooldownFrames;
		if (base.config.tauntSettings.useEmotesPerTime && base.battleServerAPI.IsSinglePlayerNetworkGame)
		{
			bool flag = false;
			if (this.model.emoteFrameLimitStart != 0 && base.gameManager.Frame - this.model.emoteFrameLimitStart <= base.config.tauntSettings.emotesPerTimeFrames)
			{
				flag = true;
			}
			if (flag)
			{
				this.model.emoteFrameLimitCounter++;
			}
			else
			{
				this.model.emoteFrameLimitStart = base.gameManager.Frame;
				this.model.emoteFrameLimitCounter = 1;
			}
		}
		return false;
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000AF250 File Offset: 0x000AD650
	private bool TryBeginMoveHelper(MoveData moveData, InterruptData interrupt, ButtonPress buttonUsed, InputButtonsData inputButtonsData)
	{
		if (!(moveData != null))
		{
			return false;
		}
		if (ButtonPressUtil.isTauntButton(buttonUsed) && !this.isEmote(buttonUsed))
		{
			return false;
		}
		if (this.checkComponentBlocksMove(moveData))
		{
			return false;
		}
		int num = 0;
		if (interrupt != null)
		{
			num = interrupt.interruptMinFrame;
		}
		if (this.ActiveMove.Model.gameFrame >= num)
		{
			this.beginMove(moveData, interrupt, buttonUsed, inputButtonsData);
		}
		else
		{
			this.Model.bufferMoveData = moveData;
			this.Model.bufferInterruptData = interrupt;
			this.Model.bufferButtonUsed = buttonUsed;
			this.Model.isInterruptMoveBuffered = true;
		}
		return true;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000AF2F8 File Offset: 0x000AD6F8
	private bool isEmote(ButtonPress buttonUsed)
	{
		TauntSlot key;
		if (buttonUsed == ButtonPress.TauntLeft)
		{
			key = TauntSlot.LEFT;
		}
		else if (buttonUsed == ButtonPress.TauntRight)
		{
			key = TauntSlot.RIGHT;
		}
		else if (buttonUsed == ButtonPress.TauntUp)
		{
			key = TauntSlot.UP;
		}
		else
		{
			if (buttonUsed != ButtonPress.TauntDown)
			{
				return false;
			}
			key = TauntSlot.DOWN;
		}
		UserTaunts forPlayer = this.tauntFinder.GetForPlayer(this.PlayerNum);
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = forPlayer.GetSlotsForCharacter(this.CharacterData.characterID);
		EquipmentID id;
		if (slotsForCharacter.TryGetValue(key, out id) && !id.IsNull())
		{
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item != null)
			{
				return item.type == EquipmentTypes.EMOTE;
			}
		}
		return false;
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000AF3A4 File Offset: 0x000AD7A4
	private void beginMove(MoveData moveData, InterruptData interrupt, ButtonPress buttonUsed, InputButtonsData inputButtonsData)
	{
		int num = (interrupt == null) ? 0 : interrupt.nextMoveStartupFrame;
		this.SetFacingAndRotation(this.Facing);
		HorizontalDirection horizontalDirection = HorizontalDirection.None;
		ButtonPress side;
		if (InputUtils.ContainsHorizontal(inputButtonsData.buttonsHeld, out side))
		{
			horizontalDirection = InputUtils.GetDirectionFromButton(inputButtonsData.facing, side);
		}
		MoveTransferSettings moveTransferSettings = new MoveTransferSettings();
		int num2 = 0;
		if (this.activeMove.IsActive && interrupt != null)
		{
			for (int i = 0; i < moveData.requiredMoves.Length; i++)
			{
				MoveData moveData2 = moveData.requiredMoves[i];
				if (moveData2.moveName == this.activeMove.Data.moveName)
				{
					num2 = this.activeMove.Model.uid;
					moveTransferSettings.transferChargeData = interrupt.transferChargeData;
					moveTransferSettings.transferHitDisableTargets = interrupt.transferHitDisabledTargets;
				}
			}
		}
		HorizontalDirection sideDirection = horizontalDirection;
		int uid = num2;
		int startGameFrame = num;
		this.SetMove(moveData, inputButtonsData, sideDirection, uid, startGameFrame, default(Vector3F), moveTransferSettings, null, default(MoveSeedData), buttonUsed);
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000AF4C4 File Offset: 0x000AD8C4
	private bool checkComponentBlocksMove(MoveData moveData)
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			IMoveBlockerComponent moveBlockerComponent = this.characterComponents[i] as IMoveBlockerComponent;
			if (moveBlockerComponent != null && moveBlockerComponent.IsMoveBlocked(moveData))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000AF514 File Offset: 0x000AD914
	public void EndActiveMove(MoveEndType endType, bool processBufferedInput = true, bool transitioningToContinuingMove = false)
	{
		if (!this.activeMove.IsActive)
		{
			return;
		}
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.MoveEnd);
		MoveData data = this.activeMove.Data;
		MoveLabel label = this.activeMove.Data.label;
		this.Model.isInterruptMoveBuffered = false;
		this.activeMove.Reset(endType, transitioningToContinuingMove);
		if (!transitioningToContinuingMove)
		{
			this.TrailEmitter.ResetData();
			this.KnockbackEmitter.ResetData();
		}
		if (endType == MoveEndType.Cancelled)
		{
			this.GrabController.ReleaseGrabbedOpponent(true);
		}
		this.Orientation.SyncToFacing();
		if (endType != MoveEndType.Cancelled && data.makeHelpless && !this.model.ignoreNextHelplessness && !this.State.IsLanding && !this.State.IsGrounded)
		{
			this.makeHelpless(data);
		}
		this.LedgeGrabController.ReleaseGrabbedLedge(true, false);
		if (this.State.IsShieldingState)
		{
			this.StateActor.TryResumeShield();
		}
		this.Invincibility.EndAllMoveIntangibility();
		this.physics.SetOverride(null);
		this.model.fallThroughPlatformHeldFrames = 0;
		foreach (ColorMode colorMode in this.allColorModes)
		{
			if (!PlayerController.colorModeFlagsPersisted.Contains(colorMode) && CharacterRenderer.ResetColorModeOnMoveEnd(colorMode))
			{
				this.Renderer.SetColorModeFlag(colorMode, false);
			}
		}
		for (int j = 0; j < this.characterComponents.Count; j++)
		{
			ICharacterComponent characterComponent = this.characterComponents[j];
			if (characterComponent is IMoveKillListener)
			{
				(characterComponent as IMoveKillListener).OnMoveKilled(data);
			}
		}
		if (processBufferedInput)
		{
			this.inputProcessor.ChangeStateIfNecessary(this.nonVoidController, label);
			this.ProcessInput(true);
		}
		if (endType == MoveEndType.Finished && processBufferedInput && data.canBufferInput)
		{
			this.ProcessBufferedInput();
		}
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000AF710 File Offset: 0x000ADB10
	private void makeHelpless(MoveData move)
	{
		string overrideAnimation = null;
		string overrideLeftAnimation = null;
		if (move.helplessStateData != null && move.helplessStateData.animationClip != null)
		{
			this.model.helplessStateData = move.helplessStateData;
			overrideAnimation = move.helplessStateData.GetClipName(move, false);
			if (move.helplessStateData.leftAnimationClip != null)
			{
				overrideLeftAnimation = move.helplessStateData.GetClipName(move, true);
			}
		}
		this.StateActor.StartCharacterAction(ActionState.FallHelpless, overrideAnimation, overrideLeftAnimation, false, 0, false);
		this.State.SubState = SubStates.Helpless;
		if (move.helplessLandingData != null)
		{
			this.model.landingOverrideData = move.helplessLandingData;
		}
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000AF7BF File Offset: 0x000ADBBF
	public void AddCharacterComponent(ICharacterComponent pComponent)
	{
		if (pComponent != null)
		{
			this.characterComponents.Add(pComponent);
		}
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000AF7D4 File Offset: 0x000ADBD4
	public T GetCharacterComponent<T>() where T : class
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is T)
			{
				return characterComponent as T;
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000AF828 File Offset: 0x000ADC28
	public bool ExecuteCharacterComponents<T>(PlayerController.ComponentExecution<T> execute)
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is T)
			{
				bool flag = execute((T)((object)characterComponent));
				if (flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000AF880 File Offset: 0x000ADC80
	public bool ExecuteCharacterComponents<TComponent, TParam1>(Func<TComponent, TParam1, bool> execute, TParam1 param1)
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is TComponent)
			{
				bool flag = execute((TComponent)((object)characterComponent), param1);
				if (flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x0600232A RID: 9002 RVA: 0x000AF8D8 File Offset: 0x000ADCD8
	List<Hit> IHitOwner.Hits
	{
		get
		{
			this.activeHitsBuffer.Clear();
			if (this.activeMove.IsActive)
			{
				this.activeHitsBuffer.AddRange(this.activeMove.Model.hits);
			}
			if (this.Shield.IsGusting)
			{
				this.activeHitsBuffer.AddRange(this.Shield.ShieldHits);
			}
			for (int i = 0; i < this.model.hostedHits.Count; i++)
			{
				this.activeHitsBuffer.Add(this.model.hostedHits[i]);
			}
			if (this.activeHitsBuffer.Count == 0)
			{
				return null;
			}
			return this.activeHitsBuffer;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x0600232B RID: 9003 RVA: 0x000AF996 File Offset: 0x000ADD96
	List<HurtBoxState> IHitOwner.HurtBoxes
	{
		get
		{
			if (this.IsActive && this.IsInBattle && !this.model.isDead)
			{
				return this.Bones.HurtBoxes;
			}
			return null;
		}
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000AF9CC File Offset: 0x000ADDCC
	bool IHitOwner.AllowClanking(HitData hitData, IHitOwner other)
	{
		if (this.activeMove.IsActive && this.activeMove.Data.IsGrab)
		{
			return false;
		}
		if (!this.Physics.IsGrounded && !other.IsProjectile)
		{
			return false;
		}
		HitType hitType = hitData.hitType;
		return hitType != HitType.Hit || !this.activeMove.IsActive || !this.activeMove.Data.ignoreHitBoxCollision;
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000AFA57 File Offset: 0x000ADE57
	bool IHitOwner.ForceCollisionChecks(CollisionCheckType type, HitData hitData)
	{
		return type == CollisionCheckType.HitBox && (this.activeMove.IsActive && hitData.hitType == HitType.Counter);
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000AFA86 File Offset: 0x000ADE86
	bool IHitOwner.HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext)
	{
		return false;
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x0600232F RID: 9007 RVA: 0x000AFA89 File Offset: 0x000ADE89
	// (set) Token: 0x06002330 RID: 9008 RVA: 0x000AFA96 File Offset: 0x000ADE96
	int IHitOwner.HitOwnerID
	{
		get
		{
			return this.model.hitOwnerID;
		}
		set
		{
			this.model.hitOwnerID = value;
		}
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x06002331 RID: 9009 RVA: 0x000AFAA4 File Offset: 0x000ADEA4
	List<HitBoxState> IHitOwner.ShieldBoxes
	{
		get
		{
			if (!this.State.IsShieldingState)
			{
				return null;
			}
			this.shieldBoxBuffer.Clear();
			for (int i = 0; i < this.Shield.ShieldHits.Count; i++)
			{
				this.shieldBoxBuffer.AddRange(this.Shield.ShieldHits[i].hitBoxes);
			}
			return this.shieldBoxBuffer;
		}
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000AFB18 File Offset: 0x000ADF18
	bool IHitOwner.IsImmune(HitData hitData, IHitOwner enemy)
	{
		bool flag = false;
		if (this.IsAllyAssist && !this.AssistAbsorbsHits && this.model.temporaryAssistImmunityFrames > 0 && !enemy.IsAllyAssist && hitData.hitType != HitType.Gust)
		{
			return true;
		}
		HitType hitType = hitData.hitType;
		if (hitType != HitType.Counter)
		{
			if (hitType == HitType.Grab || hitType == HitType.BlockableGrab)
			{
				flag |= (this.State.IsDownState || this.State.IsLedgeHangingState || this.State.IsGrabbedState || this.GrabController.GrabbedOpponent != PlayerNum.None);
			}
		}
		else
		{
			flag = true;
		}
		HitConditionType conditionType = hitData.conditionType;
		if (conditionType != HitConditionType.AirOnly)
		{
			if (conditionType == HitConditionType.GroundOnly)
			{
				flag |= !this.State.IsGrounded;
			}
		}
		else
		{
			flag |= this.State.IsGrounded;
		}
		return flag;
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06002333 RID: 9011 RVA: 0x000AFC20 File Offset: 0x000AE020
	HitOwnerType IHitOwner.Type
	{
		get
		{
			return HitOwnerType.Player;
		}
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000AFC23 File Offset: 0x000AE023
	bool IHitOwner.CanReflect(HitData hitData)
	{
		return hitData.reflectsProjectiles;
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000AFC2C File Offset: 0x000AE02C
	public bool ShouldReflect(IHitOwner other, ref Vector3F collisionPoint, CollisionCheckType type, Hit myHit = null)
	{
		if (myHit == null)
		{
			return false;
		}
		if (myHit.data.hitType == HitType.Gust)
		{
			return this.Shield.TryToGustObject(other);
		}
		return other.IsProjectile && myHit.data.reflectsProjectiles;
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06002336 RID: 9014 RVA: 0x000AFC7B File Offset: 0x000AE07B
	bool IHitOwner.IsProjectile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000AFC80 File Offset: 0x000AE080
	public bool IsHitActive(Hit hit, IHitOwner other, bool excludePhantomHitbox)
	{
		if (other != null)
		{
			if (hit.data.hitType != HitType.SelfHit && other.PlayerNum == this.PlayerNum)
			{
				return false;
			}
			if (hit.data.hitType == HitType.SelfHit && other.PlayerNum != this.PlayerNum)
			{
				return false;
			}
			if ((!base.gameManager.BattleSettings.isTeamAttack || (this.activeMove.IsActive && this.activeMove.Data.neverTeamAttack)) && other.PlayerNum != this.PlayerNum && other.Team == this.Team)
			{
				return false;
			}
		}
		if (hit == null)
		{
			return false;
		}
		if (hit.data.dataType == HitDataType.Hosted)
		{
			HostedHit hostedHit = hit as HostedHit;
			return hostedHit.IsActiveFor(other, base.gameManager.Frame);
		}
		int num = hit.data.startFrame;
		if (hit.data.phantomInterpolation && excludePhantomHitbox)
		{
			num++;
		}
		return hit.data.skipMoveValidation || (this.activeMove.Model.IsActiveFor(other, this.activeMove.Model.internalFrame) && this.activeMove.IsActive && this.activeMove.Model.internalFrame >= num && this.activeMove.Model.internalFrame <= hit.data.endFrame);
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000AFE14 File Offset: 0x000AE214
	bool IHitOwner.ShouldCancelClankedMove(Hit myHit, Hit otherHit, IHitOwner other)
	{
		if (otherHit.data.hitType == HitType.Counter)
		{
			return true;
		}
		if (myHit.data.hitType == HitType.Counter)
		{
			return false;
		}
		if (myHit.data.hitType == HitType.Hit)
		{
			Fixed one = this.combatCalculator.CalculateModifiedDamage(myHit.data, this);
			Fixed other2 = this.combatCalculator.CalculateModifiedDamage(otherHit.data, other);
			return one - other2 < base.config.priorityConfig.priorityThreshold;
		}
		return false;
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000AFE9B File Offset: 0x000AE29B
	public void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData)
	{
		this.Combat.BeginHitLag(hitLagFrames, owner, hitData, false);
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000AFEAC File Offset: 0x000AE2AC
	void IHitOwner.OnHitBoxCollision(Hit myHit, IHitOwner other, Hit otherHit, ref Vector3F hitPosition, bool cancelMine, bool makeClankEffects)
	{
		HitData data = myHit.data;
		HitData data2 = otherHit.data;
		if (data2.hitType == HitType.Counter)
		{
			if (myHit.data.dataType == HitDataType.Move)
			{
				this.activeMove.Model.disabledHits.DisableForAll();
				this.onHitDoLinks();
			}
		}
		else if (data.hitType == HitType.Counter)
		{
			if (this.activeMove.IsActive)
			{
				int counterHitLagFrames = data.counterHitLagFrames;
				if (counterHitLagFrames > 0)
				{
					other.BeginHitLag(counterHitLagFrames, other, data2);
				}
				for (int i = 0; i < this.activeMove.Data.interrupts.Length; i++)
				{
					InterruptData interruptData = this.activeMove.Data.interrupts[i];
					if (interruptData.ShouldUseLink(LinkCheckType.Counter, this, this.activeMove.Model, this.inputProcessor.CurrentInputData))
					{
						if (!interruptData.ignoreHit)
						{
							HitContext next = this.hitContextPool.GetNext();
							next.collisionPosition = hitPosition;
							this.ReceiveHit(data2, this, ImpactType.DamageOnly, next);
						}
						if (interruptData.faceHit)
						{
							Fixed horizontalValue = other.Position.x - this.Position.x;
							HorizontalDirection direction = InputUtils.GetDirection(horizontalValue);
							this.SetFacingAndRotation(direction);
						}
						int uid = this.activeMove.Model.uid;
						MoveSeedData moveSeedData = default(MoveSeedData);
						if (interruptData.seedCounteredDamage)
						{
							moveSeedData.isActive = true;
							moveSeedData.damage = this.combatCalculator.CalculateModifiedDamage(data2, other);
						}
						MoveData move = interruptData.linkableMoves[0];
						InputButtonsData currentInputData = this.inputProcessor.CurrentInputData;
						HorizontalDirection sideDirection = HorizontalDirection.None;
						int uid2 = uid;
						List<MoveLinkComponentData> allLinkComponentData = this.activeMove.GetAllLinkComponentData();
						MoveSeedData seedData = moveSeedData;
						this.SetMove(move, currentInputData, sideDirection, uid2, 0, default(Vector3F), new MoveTransferSettings
						{
							transferHitDisableTargets = interruptData.transferHitDisabledTargets,
							transferChargeData = interruptData.transferChargeData
						}, allLinkComponentData, seedData, ButtonPress.None);
					}
				}
			}
		}
		else if (data.hitType == HitType.Hit)
		{
			Fixed one = this.combatCalculator.CalculateModifiedDamage(data, this);
			Fixed other2 = this.combatCalculator.CalculateModifiedDamage(data2, other);
			Fixed @fixed = (one + other2) / 2;
			if (this.activeMove.IsActive)
			{
				this.activeMove.Model.disabledHits.Disable(data, other, this.activeMove.Model.internalFrame);
			}
			if (!this.Physics.IsGrounded)
			{
				cancelMine = false;
			}
			int frames = this.clankLagAndAnimation(data, @fixed, cancelMine);
			float amplitude = this.combatCalculator.CalculateHitVibration(data, @fixed, true);
			this.Combat.BeginHitVibration(frames, amplitude, 1f, 0f);
			if (this.Physics.IsGrounded)
			{
				this.clankKnockback(other, @fixed);
			}
			if (cancelMine)
			{
				this.EndActiveMove(MoveEndType.Cancelled, true, false);
			}
			if (makeClankEffects)
			{
				if (base.config.priorityConfig.clankParticle != null)
				{
					this.GameVFX.PlayParticle(base.config.priorityConfig.clankParticle, 30, (Vector3)hitPosition, false);
				}
				base.gameManager.Audio.PlayGameSound(new AudioRequest(base.config.priorityConfig.clankSound, this.AudioOwner, null));
			}
		}
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000B021F File Offset: 0x000AE61F
	bool IHitOwner.ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return (!hitData.knockbackCausesFlinching && !this.State.IsAffectedByUnflinchingKnockback) || this.ArmorResistsHit(hitData, hitInstigator, hitPosition);
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000B0248 File Offset: 0x000AE648
	public bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		Vector2F zero = Vector2F.zero;
		Fixed damage = this.combatCalculator.CalculateModifiedDamageUnstaled(hitData, hitInstigator);
		Fixed knockbackForce = this.combatCalculator.CalculateKnockback(hitData, hitInstigator, this, this.Model.damage, damage, hitPosition, out zero);
		return this.ActiveMove.DoesArmorResistHit(knockbackForce);
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000B0294 File Offset: 0x000AE694
	private int clankLagAndAnimation(HitData myHitData, Fixed clankDamage, bool isCanceled)
	{
		int num = this.combatCalculator.CalculateHitLagForClank(myHitData, clankDamage);
		if (!isCanceled)
		{
			this.Combat.BeginHitLag(num, null, myHitData, false);
		}
		else if (base.config.knockbackConfig.clankLagOnAttackAnimations)
		{
			this.Combat.BeginClankLag(num, myHitData);
		}
		else
		{
			this.Combat.BeginHitLag(num, null, myHitData, false);
			if (base.config.knockbackConfig.clankUseRecoilAnimation)
			{
				this.StateActor.StartCharacterAction(ActionState.Recoil, null, null, true, 0, false);
			}
			else
			{
				this.StateActor.StartCharacterAction(ActionState.Idle, null, null, true, 0, false);
			}
		}
		return num;
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000B0340 File Offset: 0x000AE740
	private void clankKnockback(IHitOwner other, Fixed clankDamage)
	{
		Vector3F normalized = (other.Position - this.Position).normalized;
		normalized.x = -normalized.x;
		normalized.y = base.config.knockbackConfig.clankKnockbackYForce;
		Fixed @fixed = FixedMath.Sqrt(clankDamage) * base.config.knockbackConfig.clankKnockback;
		if (@fixed < base.config.knockbackConfig.clankKnockbackMin)
		{
			@fixed = base.config.knockbackConfig.clankKnockbackMin;
		}
		Vector3F v = normalized * @fixed;
		this.Physics.AddVelocity(v, 1, VelocityType.Movement);
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000B03F4 File Offset: 0x000AE7F4
	bool IHitOwner.OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext)
	{
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.DealDamage);
		HitData data = hit.data;
		foreach (MaterialAnimationTrigger materialAnimationTrigger in data.materialAnimationTriggers)
		{
			if (other.Type == HitOwnerType.Player && materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Target))
			{
				(other as IPlayerDelegate).AddHostedMaterialAnimation(materialAnimationTrigger);
			}
			if (materialAnimationTrigger.MatchesTarget(MaterialAnimationTrigger.TargetType.Attacker))
			{
				((IPlayerDelegate)this).AddHostedMaterialAnimation(materialAnimationTrigger);
			}
		}
		if (impactType != ImpactType.Grab)
		{
			if (impactType == ImpactType.Hit || impactType == ImpactType.Shield)
			{
				if (this.activeMove.IsActive)
				{
					bool flag = impactType == ImpactType.Shield;
					Fixed @fixed = this.combatCalculator.CalculateModifiedDamage(data, this);
					Fixed unstaledDamage = this.combatCalculator.CalculateModifiedDamageUnstaled(data, this);
					hitContext.useKillFlourish = false;
					if (!flag && hitContext.totalHitSuccess == 1)
					{
						PlayerController playerController = other as PlayerController;
						if (playerController != null)
						{
							Vector2F vector2F;
							Fixed knockbackForce = this.combatCalculator.CalculateKnockback(data, this, playerController, playerController.Damage, @fixed, hitPosition, out vector2F);
							hitContext.useKillFlourish = this.shouldStartFlourish(playerController, data, @fixed, unstaledDamage, knockbackForce, hitPosition);
						}
					}
					int num;
					if (hitContext.useKillFlourish)
					{
						num = base.config.flourishConfig.hitLagFrames;
					}
					else
					{
						num = this.combatCalculator.CalculateHitLagFrames(data, this, @fixed, flag);
					}
					this.Combat.BeginHitLag(num, this, data, hitContext.useKillFlourish);
					this.Combat.BeginHitVibration(num, this.combatCalculator.CalculateHitVibration(data, @fixed, true), 1f, 0f);
					if (hitContext.useKillFlourish && base.config.flourishConfig.advanceFrames > 0)
					{
						this.model.ignoreHitLagFrames = base.config.flourishConfig.advanceFrames + 1;
					}
					other.Physics.PlayerState.hitOverrideGravityFrames = 0;
					if (data.useOverrideGravity)
					{
						other.Physics.PlayerState.hitOverrideGravityFrames = data.overrideGravityFrames;
						other.Physics.PlayerState.hitOverrideGravity = data.overrideGravity;
					}
					if (hitContext.useKillFlourish)
					{
						this.model.isFlourishOwner = true;
						if (base.config.flourishConfig.highlightAttacker)
						{
							base.gameManager.Camera.SetHighlightPosition((Vector2)this.Center, base.config.flourishConfig.cameraBoxWidth, base.config.flourishConfig.cameraBoxHeight);
						}
						else if (base.config.flourishConfig.highlightReceiver)
						{
							base.gameManager.Camera.SetHighlightPosition((Vector2)other.Center, base.config.flourishConfig.cameraBoxWidth, base.config.flourishConfig.cameraBoxHeight);
						}
						else
						{
							base.gameManager.Camera.SetHighlightPosition((Vector2)hitPosition, base.config.flourishConfig.cameraBoxWidth, base.config.flourishConfig.cameraBoxHeight);
						}
					}
					ParticleData lethalHit = base.config.defaultCharacterEffects.lethalHit;
					if ((base.config.defaultCharacterEffects.lethalHitSound.sound != null || (lethalHit != null && lethalHit.prefab != null)) && this.combatCalculator.IsLethalHit(data, this, other, hitPosition, 1))
					{
						if (base.config.defaultCharacterEffects.lethalHitSound.sound != null)
						{
							base.gameManager.Audio.PlayGameSound(new AudioRequest(base.config.defaultCharacterEffects.lethalHitSound, this.AudioOwner, null));
						}
						if (lethalHit != null && lethalHit.prefab != null)
						{
							this.GameVFX.PlayParticle(lethalHit, false, TeamNum.None);
						}
					}
					this.GameVFX.CreateHitEffect(this, data, hitPosition, num, impactType, other);
					if (hit is HostedHit)
					{
						(hit as HostedHit).DisableFor(other, base.gameManager.Frame);
					}
					else
					{
						this.activeMove.Model.disabledHits.Disable(data, other, this.activeMove.Model.internalFrame);
					}
					if (!other.IsInvincible && impactType == ImpactType.Hit)
					{
						this.StaleMove(this.activeMove.Data.label, this.activeMove.Data.moveName, this.activeMove.Model.uid);
						this.OnDamageDealt(@fixed, impactType, this.activeMove.Data.chargesMeter);
						other.OnDamageTaken(@fixed, impactType);
					}
				}
			}
		}
		else if (other.Type == HitOwnerType.Player)
		{
			if (!other.IsInvincible)
			{
				if (this.activeMove.IsActive)
				{
					this.activeMove.Model.disabledHits.DisableForAll();
					this.GrabController.OnGrabOpponent(other as PlayerController, this.activeMove.Data, data);
					this.GameVFX.CreateHitEffect(this, data, hitPosition, -1, impactType, other);
				}
			}
		}
		else
		{
			Debug.LogError("Tried to grab invalid type " + other.Type);
		}
		this.onHitDoLinks();
		return false;
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000B0974 File Offset: 0x000AED74
	private bool shouldStartFlourish(PlayerController defender, HitData hitData, Fixed damageDealt, Fixed unstaledDamage, Fixed knockbackForce, Vector3F hitPosition)
	{
		return base.config.flourishConfig.useKillFlourish && this.isFlourishPlayerCount() && this.isFlourishStockCount(defender) && this.isMinKnockback(knockbackForce) && this.isMinDamage(unstaledDamage) && this.isFlourishDistance(defender) && this.combatCalculator.IsLethalHit(hitData, this, defender, hitPosition, 1 / base.config.flourishConfig.requiredOverkill);
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000B0A04 File Offset: 0x000AEE04
	private bool isMinKnockback(Fixed knockbackForce)
	{
		if (base.config.flourishConfig.printDebug)
		{
			Debug.Log("KNOCKBACK FORCE " + knockbackForce);
		}
		return knockbackForce >= base.config.flourishConfig.minKnockback;
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000B0A51 File Offset: 0x000AEE51
	private bool isMinDamage(Fixed damage)
	{
		return damage >= base.config.flourishConfig.minDamage;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000B0A6C File Offset: 0x000AEE6C
	private bool isFlourishDistance(PlayerController defender)
	{
		FixedRect fixedRect = this.visualBounds;
		FixedRect fixedRect2 = defender.visualBounds;
		Fixed @fixed;
		if (fixedRect2.Left > fixedRect.Right)
		{
			@fixed = fixedRect2.Left - fixedRect.Right;
		}
		else if (fixedRect.Left > fixedRect2.Right)
		{
			@fixed = fixedRect.Left - fixedRect2.Right;
		}
		else if (fixedRect.Center.x < fixedRect2.Center.x)
		{
			@fixed = -FixedMath.Abs(fixedRect.Right - fixedRect2.Left);
		}
		else
		{
			@fixed = -FixedMath.Abs(fixedRect2.Right - fixedRect.Left);
		}
		Fixed fixed2;
		if (fixedRect2.Bottom > fixedRect.Top)
		{
			fixed2 = fixedRect2.Bottom - fixedRect.Top;
		}
		else if (fixedRect.Bottom > fixedRect2.Top)
		{
			fixed2 = fixedRect.Bottom - fixedRect2.Top;
		}
		else if (fixedRect.Center.y < fixedRect2.Center.y)
		{
			fixed2 = -FixedMath.Abs(fixedRect.Top - fixedRect2.Bottom);
		}
		else
		{
			fixed2 = -FixedMath.Abs(fixedRect2.Top - fixedRect.Bottom);
		}
		if (base.config.flourishConfig.printDebug)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Bounds rect ",
				fixedRect.Left,
				":",
				fixedRect.Right,
				" ",
				fixedRect.Top,
				":",
				fixedRect.Bottom
			}));
			Debug.Log(string.Concat(new object[]
			{
				"Defender rect ",
				fixedRect2.Left,
				":",
				fixedRect2.Right,
				" ",
				fixedRect2.Top,
				":",
				fixedRect2.Bottom
			}));
			Debug.Log(string.Concat(new object[]
			{
				"DISTANCE CHECKS x:",
				@fixed,
				" y:",
				fixed2
			}));
		}
		return (@fixed >= base.config.flourishConfig.minDistanceX && @fixed <= base.config.flourishConfig.maxDistanceX) || (fixed2 >= base.config.flourishConfig.minDistanceY && fixed2 <= base.config.flourishConfig.maxDistanceY);
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000B0DB7 File Offset: 0x000AF1B7
	private bool isFlourishStockCount(PlayerController defender)
	{
		return !base.config.flourishConfig.lastStockOnly || defender.Lives <= 1;
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000B0DE0 File Offset: 0x000AF1E0
	private bool isFlourishPlayerCount()
	{
		int num = 0;
		foreach (PlayerReference playerReference in base.gameController.currentGame.PlayerReferences)
		{
			if (!playerReference.IsEliminated && playerReference.IsInBattle)
			{
				num++;
			}
		}
		return num <= 2;
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000B0E64 File Offset: 0x000AF264
	private void onHitDoLinks()
	{
		if (this.activeMove.IsActive)
		{
			for (int i = 0; i < this.activeMove.Data.interrupts.Length; i++)
			{
				InterruptData interruptData = this.activeMove.Data.interrupts[i];
				if (interruptData.ShouldUseLink(LinkCheckType.OnHit, this, this.activeMove.Model, this.inputProcessor.CurrentInputData))
				{
					MoveData move = interruptData.linkableMoves[0];
					InputButtonsData currentInputData = this.inputProcessor.CurrentInputData;
					HorizontalDirection sideDirection = HorizontalDirection.None;
					int uid = this.activeMove.Model.uid;
					List<MoveLinkComponentData> allLinkComponentData = this.activeMove.GetAllLinkComponentData();
					MoveTransferSettings transferSettings = new MoveTransferSettings
					{
						transferHitDisableTargets = interruptData.transferHitDisabledTargets,
						transferChargeData = interruptData.transferChargeData,
						transferStale = true,
						transferStaleMulti = this.activeMove.Model.staleDamageMultiplier
					};
					this.SetMove(move, currentInputData, sideDirection, uid, 0, default(Vector3F), transferSettings, allLinkComponentData, default(MoveSeedData), ButtonPress.None);
					break;
				}
			}
		}
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000B0F84 File Offset: 0x000AF384
	public void ReceiveHit(HitData hitData, IHitOwner other, ImpactType impactType, HitContext hitContext)
	{
		this.model.lastHitByPlayerNum = other.PlayerNum;
		this.model.lastHitByTeamNum = other.Team;
		this.model.lastHitFrame = base.gameManager.Frame;
		switch (impactType)
		{
		case ImpactType.Hit:
		case ImpactType.DamageOnly:
			if (!this.IsAllyAssist || other.IsAllyAssist || this.model.temporaryAssistImmunityFrames <= 0)
			{
				this.Combat.OnHitImpact(hitData, other, impactType, ref hitContext);
				this.moveUseTracker.OnReceiveHit();
			}
			else
			{
				this.Combat.OnShieldHitImpact(hitData, other);
			}
			break;
		case ImpactType.Grab:
			if (!this.IsAllyAssist || other.IsAllyAssist || this.model.temporaryAssistImmunityFrames <= 0)
			{
				this.GrabController.OnGrabbedBy(other as PlayerController, hitData.grabType);
				this.moveUseTracker.OnGrabbed();
			}
			break;
		case ImpactType.Shield:
			this.Combat.OnShieldHitImpact(hitData, other);
			break;
		case ImpactType.Reflect:
			if (hitContext.reflectorHitData != null && hitContext.reflectorHitData.reflectSound.sound != null)
			{
				base.audioManager.PlayGameSound(new AudioRequest(hitContext.reflectorHitData.reflectSound, (Vector3)hitContext.collisionPosition, null));
			}
			break;
		}
		for (int i = 0; i < this.model.hostedHits.Count; i++)
		{
			this.model.hostedHits[i].OnHostHit();
		}
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000B1130 File Offset: 0x000AF530
	public void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter)
	{
		this.moveUseTracker.OnGiveHit();
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is IDamageDealtListener)
			{
				(characterComponent as IDamageDealtListener).OnDamageDealt(damage, impactType, chargesMeter);
			}
		}
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000B118C File Offset: 0x000AF58C
	public void OnDamageTaken(Fixed damage, ImpactType impactType)
	{
		for (int i = 0; i < this.characterComponents.Count; i++)
		{
			ICharacterComponent characterComponent = this.characterComponents[i];
			if (characterComponent is IDamageTakenListener)
			{
				(characterComponent as IDamageTakenListener).OnDamageTaken(damage, impactType);
			}
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x0600234A RID: 9034 RVA: 0x000B11DC File Offset: 0x000AF5DC
	public Fixed DamageMultiplier
	{
		get
		{
			Fixed @fixed = 1;
			if (this.activeMove.IsActive)
			{
				@fixed *= this.activeMove.Model.DamageMultiplier;
			}
			return @fixed;
		}
	}

	// Token: 0x17000827 RID: 2087
	// (get) Token: 0x0600234B RID: 9035 RVA: 0x000B1218 File Offset: 0x000AF618
	public Fixed StaleDamageMultiplier
	{
		get
		{
			return (!this.activeMove.IsActive) ? ((Fixed)1.0) : this.activeMove.Model.StaleDamageMultiplier;
		}
	}

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x0600234C RID: 9036 RVA: 0x000B124D File Offset: 0x000AF64D
	public Fixed HitLagMultiplier
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x0600234D RID: 9037 RVA: 0x000B1255 File Offset: 0x000AF655
	ICharacterPhysicsData IPhysicsDelegate.Data
	{
		get
		{
			return this.Physics;
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x0600234E RID: 9038 RVA: 0x000B125D File Offset: 0x000AF65D
	CharacterPhysicsData IPhysicsDelegate.DefaultData
	{
		get
		{
			return ((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData;
		}
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000B126A File Offset: 0x000AF66A
	bool IPhysicsDelegate.IsDirectionHeld(HorizontalDirection direction)
	{
		return this.nonVoidController.IsHorizontalDirectionHeld(direction);
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06002350 RID: 9040 RVA: 0x000B1278 File Offset: 0x000AF678
	Fixed IPhysicsDelegate.GetDirectionHeldAmount
	{
		get
		{
			return this.nonVoidController.GetAxis(this.nonVoidController.horizontalAxis);
		}
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000B1290 File Offset: 0x000AF690
	bool IPhysicsDelegate.ShouldFallThroughPlatforms()
	{
		Fixed axis = this.nonVoidController.GetAxis(this.nonVoidController.verticalAxis);
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is IFallThroughPlatformBlocker && !(characterComponent as IFallThroughPlatformBlocker).CanFallThroughPlatform)
			{
				return false;
			}
		}
		if (this.State.CanFallThroughPlatforms && axis < -FixedMath.Abs(base.config.inputConfig.fallThroughPlatformsThreshold))
		{
			Fixed axis2 = this.nonVoidController.GetAxis(this.nonVoidController.horizontalAxis);
			Fixed @fixed = FixedMath.Atan2(axis, axis2) * FixedMath.Rad2Deg;
			@fixed = FixedMath.DeltaAngle(0, @fixed);
			int num = -90;
			if (@fixed > num - base.config.inputConfig.fallThroughPlatformsMaxAngle && @fixed < num + base.config.inputConfig.fallThroughPlatformsMaxAngle)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000B13D8 File Offset: 0x000AF7D8
	bool IPhysicsDelegate.IsPlatformLastDropped(IPhysicsCollider platformCollider)
	{
		return this.Physics.PlayerState.lastPlatformDroppedThrough != null && platformCollider == this.Physics.PlayerState.lastPlatformDroppedThrough;
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000B1408 File Offset: 0x000AF808
	bool IPhysicsDelegate.ShouldBounce()
	{
		return this.State.IsStunned && this.State.IsTumbling && this.model.stunType == StunType.HitStun && !this.State.IsHitLagPaused && !this.State.IsShieldBroken;
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000B1466 File Offset: 0x000AF866
	bool IPhysicsDelegate.ShouldMaintainVelocityOnCollision()
	{
		return this.State.IsHitLagPaused;
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000B1474 File Offset: 0x000AF874
	bool IPhysicsDelegate.IgnorePhysicsCollisions()
	{
		return this.State.IsLedgeHangingState || this.State.IsLedgeGrabbing || (this.activeMove.IsActive && this.activeMove.Data.ignorePhysicsCollisions) || this.State.IsGrabbedState || this.State.IsRespawning;
	}

	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x06002356 RID: 9046 RVA: 0x000B14E4 File Offset: 0x000AF8E4
	bool IPhysicsDelegate.IsUnderContinuousForce
	{
		get
		{
			return this.ActiveMove.IsActive && this.ActiveMove.Model.applyForceContinuouslyEndFrame != -1;
		}
	}

	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x06002357 RID: 9047 RVA: 0x000B150F File Offset: 0x000AF90F
	MoveData IPhysicsDelegate.CurrentMove
	{
		get
		{
			return this.activeMove.IsActive ? this.activeMove.Data : null;
		}
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000B1534 File Offset: 0x000AF934
	TechType IPhysicsDelegate.AvailableTech(CollisionData collision)
	{
		if (!this.State.CanTech(collision.CollisionSurfaceType))
		{
			return TechType.None;
		}
		switch (collision.CollisionSurfaceType)
		{
		case SurfaceType.Floor:
			return TechType.Ground;
		case SurfaceType.Wall:
			return TechType.Wall;
		case SurfaceType.Ceiling:
			return TechType.Ceiling;
		}
		return TechType.None;
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000B1584 File Offset: 0x000AF984
	void IPhysicsDelegate.PerformTech(TechType techType, CollisionData collision)
	{
		if (this.IsAllyAssist)
		{
			return;
		}
		MoveData moveData = null;
		if (this.State.IsTumbling && this.State.IsTechOffCooldown)
		{
			HorizontalDirection direction = HorizontalDirection.None;
			if (this.inputController.IsHorizontalDirectionHeld(HorizontalDirection.Left))
			{
				direction = HorizontalDirection.Left;
			}
			else if (this.inputController.IsHorizontalDirectionHeld(HorizontalDirection.Right))
			{
				direction = HorizontalDirection.Right;
			}
			moveData = this.getTechMove(techType, direction);
		}
		if (moveData != null)
		{
			if (techType == TechType.Ground)
			{
				this.State.MetaState = MetaState.Stand;
				this.model.ledgeGrabsSinceLanding = 0;
				this.physics.SyncGroundState();
			}
			this.model.stunFrames = 0;
			this.model.knockbackIteration = 0;
			this.model.clearInputBufferOnStunEnd = false;
			this.model.smokeTrailFrames = 0;
			this.model.jumpStunFrames = 0;
			this.SetMove(moveData, this.inputProcessor.CurrentInputData, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
			this.GameVFX.PlayParticle(base.config.defaultCharacterEffects.tech, false, TeamNum.None);
		}
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000B16B4 File Offset: 0x000AFAB4
	private MoveData getTechMove(TechType techType, HorizontalDirection direction)
	{
		MoveData moveData = null;
		if (techType == TechType.Ground)
		{
			if (direction == HorizontalDirection.None)
			{
				moveData = this.MoveSet.GetMove(MoveLabel.Tech);
			}
			else
			{
				ButtonPress buttonPress;
				if (direction == HorizontalDirection.Right)
				{
					buttonPress = ((this.Facing != HorizontalDirection.Right) ? ButtonPress.Backward : ButtonPress.Forward);
				}
				else
				{
					buttonPress = ((this.Facing != HorizontalDirection.Left) ? ButtonPress.Backward : ButtonPress.Forward);
				}
				if (buttonPress == ButtonPress.Forward)
				{
					moveData = this.MoveSet.GetMove(MoveLabel.TechForwardRoll);
					if (moveData == null)
					{
						moveData = this.MoveSet.GetMove(MoveLabel.ShieldForwardRoll);
					}
				}
				else
				{
					moveData = this.MoveSet.GetMove(MoveLabel.TechBackwardRoll);
					if (moveData == null)
					{
						moveData = this.MoveSet.GetMove(MoveLabel.ShieldBackwardRoll);
					}
				}
			}
		}
		else if (techType == TechType.Wall)
		{
			moveData = this.MoveSet.GetMove(MoveLabel.TechWall);
			if (moveData == null)
			{
				moveData = this.MoveSet.GetMove(MoveLabel.Tech);
			}
		}
		else if (techType == TechType.Ceiling)
		{
			moveData = this.MoveSet.GetMove(MoveLabel.TechCeiling);
			if (moveData == null)
			{
				moveData = this.MoveSet.GetMove(MoveLabel.Tech);
			}
		}
		return moveData;
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000B17DC File Offset: 0x000AFBDC
	public void ForceImpact(Fixed damage, Fixed knockbackAngle, Fixed baseKnockback, Fixed knockbackScaling)
	{
		if (this.Facing == HorizontalDirection.Left)
		{
			knockbackAngle = 180 - knockbackAngle;
		}
		HitData hitData = new HitData();
		hitData.damage = (float)damage;
		hitData.knockbackAngle = (float)knockbackAngle;
		hitData.baseKnockback = (float)baseKnockback;
		hitData.knockbackScaling = (float)knockbackScaling;
		HitContext hitContext = new HitContext();
		hitContext.collisionPosition = this.Physics.Center;
		this.Combat.OnHitImpact(hitData, this, ImpactType.Hit, ref hitContext);
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000B1860 File Offset: 0x000AFC60
	public void ForceWindHit(Fixed angle, Fixed force, bool resetXVelocity, bool resetYVelocity)
	{
		this.Physics.StopMovement(resetXVelocity, resetYVelocity, VelocityType.Total);
		this.Physics.ClearFastFall();
		Vector2F a = MathUtil.AngleToVector(angle);
		this.Physics.AddVelocity(a * force, 1, VelocityType.Movement);
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000B18A8 File Offset: 0x000AFCA8
	Fixed IPhysicsDelegate.GetHorizontalAcceleration(bool grounded)
	{
		if (!grounded)
		{
			Fixed one = this.Physics.AirAcceleration * ((!this.State.IsHelpless) ? 1 : this.Physics.HelplessAirAccelerationMultiplier);
			return one * this.getAccelerationMulti();
		}
		if (this.State.IsWalking)
		{
			return this.Physics.WalkAcceleration;
		}
		if (this.State.IsRunPivoting)
		{
			return this.Physics.RunPivotAcceleration;
		}
		if (this.State.IsDashPivoting)
		{
			return 0;
		}
		return this.Physics.DashAcceleration;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000B195C File Offset: 0x000AFD5C
	private Fixed getAccelerationMulti()
	{
		Fixed @fixed = 1;
		if (this.ActiveMove != null && this.ActiveMove.IsActive)
		{
			foreach (BlockMovementData blockMovementData2 in this.ActiveMove.Data.blockMovementData)
			{
				if (this.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame)
				{
					@fixed *= blockMovementData2.airMobilityMulti;
				}
			}
		}
		if (this.model.helplessStateData != null)
		{
			@fixed *= this.model.helplessStateData.airMobilityMulti;
		}
		return @fixed;
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000B1A20 File Offset: 0x000AFE20
	private Fixed getMoveBlockMaxAirSpeedMulti()
	{
		Fixed @fixed = 1;
		if (this.ActiveMove != null && this.ActiveMove.IsActive)
		{
			foreach (BlockMovementData blockMovementData2 in this.ActiveMove.Data.blockMovementData)
			{
				if (this.ActiveMove.Model.internalFrame >= blockMovementData2.startFrame && this.ActiveMove.Model.internalFrame <= blockMovementData2.endFrame)
				{
					@fixed *= blockMovementData2.maxHAirVelocityMulti;
				}
			}
		}
		if (this.model.helplessStateData != null)
		{
			@fixed *= this.model.helplessStateData.maxHAirVelocityMulti;
		}
		return @fixed;
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000B1AE4 File Offset: 0x000AFEE4
	Fixed IPhysicsDelegate.CalculateMaxHorizontalSpeed()
	{
		Fixed @fixed = 0;
		if (this.State.IsGrounded)
		{
			if (this.State.IsCrouching)
			{
				@fixed = 0;
			}
			else if (this.State.IsDashing)
			{
				if (this.nonVoidController.IsHorizontalDirectionHeld(HorizontalDirection.Any))
				{
					@fixed = this.Physics.DashMaxSpeed;
				}
				else
				{
					@fixed = 0;
				}
			}
			else if (this.State.IsRunPivoting)
			{
				bool flag = InputUtils.GetDirectionMultiplier(this.Facing) * this.Physics.Velocity.x > 0;
				if (flag && this.nonVoidController.IsHorizontalDirectionHeld(HorizontalDirection.Any))
				{
					@fixed = this.Physics.DashMaxSpeed;
				}
				else
				{
					@fixed = 0;
				}
			}
			else if (this.State.IsRunning)
			{
				@fixed = this.Physics.RunMaxSpeed;
			}
			else if (this.State.IsTakingOff)
			{
				@fixed = 0;
			}
			else if (this.State.IsWalking)
			{
				if (this.State.ActionState == ActionState.WalkSlow)
				{
					@fixed = this.Physics.SlowWalkMaxSpeed;
				}
				else if (this.State.ActionState == ActionState.WalkMedium)
				{
					@fixed = this.Physics.MediumWalkMaxSpeed;
				}
				else
				{
					@fixed = this.Physics.FastWalkMaxSpeed;
				}
			}
		}
		else if (!this.State.IsGrounded && this.nonVoidController.IsHorizontalDirectionHeld(HorizontalDirection.Any))
		{
			@fixed = this.Physics.AirMaxSpeed;
			if (this.State.IsHelpless)
			{
				@fixed *= this.Physics.HelplessAirSpeedMultiplier;
			}
			@fixed *= this.getMoveBlockMaxAirSpeedMulti();
			Fixed horizontalAxis = this.nonVoidController.GetHorizontalAxis();
			@fixed *= FixedMath.Abs(horizontalAxis);
		}
		return @fixed;
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000B1CEC File Offset: 0x000B00EC
	private void playLandingAnimation(string overrideAnimationName, string overrideLeftAnimationName, int landInterruptFrames, int landVisualFrames, bool isHeavyLand)
	{
		CharacterActionData action = this.MoveSet.Actions.GetAction(ActionState.Landing, false);
		if (action != null && !this.AnimationPlayer.IsAnimationPlaying(action.name))
		{
			if (overrideAnimationName != null || overrideLeftAnimationName != null)
			{
				this.StateActor.StartCharacterAction(ActionState.Landing, overrideAnimationName, overrideLeftAnimationName, true, 0, false);
				int currentAnimationGameFramelength = this.AnimationPlayer.CurrentAnimationGameFramelength;
				this.model.overrideActionStateInterruptibilityFrames = Mathf.Max(0, landVisualFrames - landInterruptFrames);
			}
			else if (Vector3F.Dot(this.physics.GroundedNormal, this.physics.Velocity) < (Fixed)0.005)
			{
				Fixed overrideSpeed = 1;
				if (landVisualFrames > action.frameDuration || !isHeavyLand)
				{
					float num = (float)landVisualFrames / (float)base.config.fps;
					overrideSpeed = (Fixed)((double)this.AnimationPlayer.GetAnimationLength(action.name)) / FixedMath.Max((Fixed)0.01, (Fixed)((double)num));
				}
				this.StateActor.StartCharacterAction(ActionState.Landing, overrideSpeed, this.MoveSet.Actions.landing.name, null, true, 0, false);
				int currentAnimationGameFramelength2 = this.AnimationPlayer.CurrentAnimationGameFramelength;
				this.model.overrideActionStateInterruptibilityFrames = Mathf.Max(0, currentAnimationGameFramelength2 - landInterruptFrames);
			}
		}
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000B1E4C File Offset: 0x000B024C
	public void OnLand(ref Vector3F previousVelocity)
	{
		if (this.State.IsDownState)
		{
			return;
		}
		if (this.State.IsGrabbedState)
		{
			return;
		}
		if (!this.IsInBattle)
		{
			return;
		}
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.Land);
		this.moveUseTracker.Grounded();
		bool flag = false;
		this.Invincibility.EndGrabInvincibility();
		this.model.ledgeGrabCooldownFrames = 0;
		if (this.State.IsTumbling || this.State.IsShieldBroken)
		{
			this.StateActor.BeginDowned(ref previousVelocity);
			if (this.State.IsTechableMode)
			{
				this.GameVFX.PlayDelayedParticle(base.config.defaultCharacterEffects.knockDown, false);
			}
			else
			{
				this.GameVFX.PlayDelayedParticle(base.config.defaultCharacterEffects.untechableKnockdown, false);
			}
		}
		else
		{
			this.model.landedWithAirDodge = false;
			if (this.State.IsBusyWithMove && this.ActiveMove.Data.label == MoveLabel.AirDodge)
			{
				this.model.landedWithAirDodge = true;
			}
			this.State.MetaState = MetaState.Stand;
			this.model.fallThroughPlatformHeldFrames = 0;
			bool flag2 = previousVelocity.y < -base.config.lagConfig.heavyLandSpeedThreshold;
			int landInterruptFrames = (!flag2) ? base.config.lagConfig.lightLandLagFrames : base.config.lagConfig.heavyLandLagFrames;
			int landVisualFrames = (!flag2) ? base.config.lagConfig.lightLandLagFrames : base.config.lagConfig.heavyLandLagFrames;
			ParticleData particleData = (!this.State.ShouldPlayFallOrLandAction || !flag2) ? null : base.config.defaultCharacterEffects.heavyLand;
			if (this.Combat.IsAirHitStunned)
			{
				ActionState groundStunAction = this.Combat.GetGroundStunAction();
				this.StateActor.StartCharacterAction(groundStunAction, null, null, true, this.State.ActionStateFrame, false);
			}
			else
			{
				this.model.stunFrames = 0;
				this.model.knockbackIteration = 0;
				this.model.clearInputBufferOnStunEnd = false;
				this.model.smokeTrailFrames = 0;
				if (this.State.ShouldPlayFallOrLandAction)
				{
					string overrideAnimationName = null;
					string overrideLeftAnimationName = null;
					if (this.activeMove.IsActive)
					{
						MoveData moveData = null;
						if (this.activeMove.OnLand(ref particleData, ref flag, ref landInterruptFrames, ref landVisualFrames, ref overrideAnimationName, ref overrideLeftAnimationName, ref moveData, this.inputProcessor))
						{
							this.playLandingAnimation(overrideAnimationName, overrideLeftAnimationName, landInterruptFrames, landVisualFrames, flag2);
							MoveEndType endType = MoveEndType.Cancelled;
							if (moveData != null)
							{
								endType = MoveEndType.Continued;
							}
							this.EndActiveMove(endType, true, false);
							if (moveData != null)
							{
								this.SetMove(moveData, InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
							}
						}
					}
					else if (this.MoveSet.Actions.landing.animation != null)
					{
						if (this.model.landingOverrideData != null)
						{
							if (this.model.landingOverrideData.landClip != null)
							{
								overrideAnimationName = this.model.landingOverrideData.landClipName;
							}
							if (this.model.landingOverrideData.leftLandClip != null)
							{
								overrideLeftAnimationName = this.model.landingOverrideData.leftLandClipName;
							}
						}
						this.playLandingAnimation(overrideAnimationName, overrideLeftAnimationName, landInterruptFrames, landVisualFrames, flag2);
					}
					for (int i = 0; i < this.model.hostedHits.Count; i++)
					{
						this.model.hostedHits[i].OnHostLand();
					}
					this.model.landingOverrideData = null;
					this.model.helplessStateData = null;
				}
				if (!flag)
				{
					this.model.jumpStunFrames = 0;
					if (this.State.IsHelpless)
					{
						this.State.SubState = SubStates.Resting;
					}
				}
			}
			if (particleData != null)
			{
				this.GameVFX.PlayDelayedParticle(particleData, false);
			}
			if (this.characterData.useLandingCameraShake)
			{
				base.gameManager.Camera.ShakeCamera(new CameraShakeRequest(this.characterData.landingCameraShake.shake));
			}
			if (!flag)
			{
				this.model.ledgeGrabsSinceLanding = 0;
			}
		}
		this.model.untechableBounceUsed = false;
		this.ExecuteCharacterComponents<ILandListener>(new PlayerController.ComponentExecution<ILandListener>(this.onLandComponentFn));
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000B22E1 File Offset: 0x000B06E1
	public void OnJump()
	{
		this.ExecuteCharacterComponents<IJumpListener>(new PlayerController.ComponentExecution<IJumpListener>(this.onJumpComponentFn));
	}

	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06002364 RID: 9060 RVA: 0x000B22F8 File Offset: 0x000B06F8
	public bool AllowFastFall
	{
		get
		{
			foreach (ICharacterComponent characterComponent in this.characterComponents)
			{
				if (characterComponent is IDropListener && !(characterComponent as IDropListener).AllowFastFall)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000B2374 File Offset: 0x000B0774
	public void OnGroundBounce()
	{
		this.moveUseTracker.Grounded();
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000B2384 File Offset: 0x000B0784
	public void OnFall()
	{
		if (!this.activeMove.IsActive || !this.activeMove.OnFall(this.inputProcessor))
		{
			if (this.Combat.IsGroundHitStunned)
			{
				ActionState airStunAction = this.Combat.GetAirStunAction();
				this.StateActor.StartCharacterAction(airStunAction, null, null, true, this.State.ActionStateFrame, false);
			}
		}
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000B23F3 File Offset: 0x000B07F3
	public void StaleMove(MoveLabel label, string name, int uid)
	{
		this.staleMoveQueue.OnMoveHit(label, name, uid);
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000B2404 File Offset: 0x000B0804
	void PlayerStateActor.IPlayerActorDelegate.BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F knockbackVelocity)
	{
		this.Combat.BeginStun(frames, stunType, cannotTech, isSpike, knockbackForce, knockbackVelocity, HitContext.Null, null);
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000B242C File Offset: 0x000B082C
	private void onParticleCreatedFn(ParticleData particle, GameObject gameObject)
	{
		if (particle.tag != ParticleTag.None)
		{
			for (int i = 0; i < this.characterComponents.Count; i++)
			{
				ICharacterComponent characterComponent = this.characterComponents[i];
				if (characterComponent is ITaggedParticleListener)
				{
					(characterComponent as ITaggedParticleListener).OnCreateTaggedParticle(particle.tag, gameObject);
				}
			}
			if (this.activeMove != null)
			{
				this.activeMove.onParticleCreated(particle, gameObject);
			}
		}
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000B24A3 File Offset: 0x000B08A3
	public void LoadPhysicsData(CharacterPhysicsData physicsData)
	{
		((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData = physicsData;
		this.physics.LoadPhysicsData(((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData);
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000B24C7 File Offset: 0x000B08C7
	public void LoadMoveInfo(MoveData move)
	{
		this.MoveSet.LoadMoveInfo(move);
		this.AnimationPlayer.LoadMove(move, true);
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000B24E3 File Offset: 0x000B08E3
	public void LoadCharacterShieldData(CharacterShieldData shieldData)
	{
		this.characterData.shield = shieldData;
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000B24F1 File Offset: 0x000B08F1
	public void LoadCharacterParticleData(CharacterParticleData particleData)
	{
		this.characterData.particles = particleData;
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000B2500 File Offset: 0x000B0900
	public void LoadComponents(CharacterComponent[] components)
	{
		Dictionary<Type, IComponentState> dictionary = new Dictionary<Type, IComponentState>();
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			CharacterComponent characterComponent2 = (CharacterComponent)characterComponent;
			dictionary[characterComponent2.GetType()] = characterComponent2.State;
			characterComponent2.Destroy();
		}
		this.characterComponents.Clear();
		foreach (CharacterComponent characterComponent3 in components)
		{
			CharacterComponent characterComponent4 = UnityEngine.Object.Instantiate<CharacterComponent>(characterComponent3);
			base.injector.Inject(characterComponent4);
			characterComponent4.Init(this);
			if (dictionary.ContainsKey(characterComponent3.GetType()))
			{
				characterComponent4.LoadState(dictionary[characterComponent3.GetType()]);
			}
			this.characterComponents.Add(characterComponent4);
		}
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000B25F4 File Offset: 0x000B09F4
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.AudioOwner != null)
		{
			base.audioManager.Unregister(this.AudioOwner);
		}
		if (this.Renderer != null)
		{
			this.Renderer.Destroy();
		}
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			CharacterComponent characterComponent2 = (CharacterComponent)characterComponent;
			characterComponent2.Destroy();
		}
		this.MaterialAnimationsController.OnDestroy();
		this.characterComponents.Clear();
		this.characterComponents = null;
		this.Bones.Destroy();
		this.hitBoxController = null;
		this.TrailEmitter.Kill();
		this.KnockbackEmitter.Kill();
		if (this.customRespawnPlatform != null)
		{
			UnityEngine.Object.Destroy(this.customRespawnPlatform);
			this.customRespawnPlatform = null;
		}
		if (this.activeMove != null)
		{
			this.activeMove.Destroy();
			this.activeMove = null;
		}
		if (this.Shield != null)
		{
			this.Shield.Destroy();
			this.Shield = null;
		}
		if (base.gameManager != null)
		{
			base.gameManager.DestroyCharacter(this);
		}
		this.unsubscribeListeners();
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000B2754 File Offset: 0x000B0B54
	public void OnDrawGizmos()
	{
		if (this.State == null)
		{
			return;
		}
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Bounds))
		{
			Vector3F offset = this.Physics.GetPosition() + this.Physics.Bounds.centerOffset;
			if (this.State.CanGrabLedge)
			{
				FixedRect ledgeGrabBox = this.LedgeGrabController.GetLedgeGrabBox(this.Facing, this.physics.Bounds);
				GizmoUtil.GizmosDrawRectangle(ledgeGrabBox, offset, Color.red, false);
			}
			if (base.config.flourishConfig.printDebug)
			{
				GizmoUtil.GizmosDrawRectangle(this.visualBounds, Color.yellow, true);
			}
			FixedRect shoveBounds = this.CharacterData.shoveBounds;
			if (this.Facing == HorizontalDirection.Left)
			{
				shoveBounds.position.x = -shoveBounds.Left - shoveBounds.Width;
			}
			GizmoUtil.GizmosDrawRectangle(shoveBounds, offset, Color.green, false);
		}
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Camera))
		{
			Vector3F offset2 = this.Physics.GetPosition() + this.Physics.Bounds.centerOffset;
			FixedRect cameraBox = this.CameraBoxController.GetCameraBox(this.Facing);
			GizmoUtil.GizmosDrawRectangle(cameraBox, offset2, Color.white, false);
		}
		this.Body.DrawGizmos();
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Input))
		{
			IntegerAxis integerAxis = this.nonVoidController.GetIntegerAxis(this.nonVoidController.horizontalAxis);
			IntegerAxis integerAxis2 = this.nonVoidController.GetIntegerAxis(this.nonVoidController.verticalAxis);
			float num = 4f;
			float num2 = num / 2f;
			Vector2 vector = Vector2.up * 4f;
			float num3 = 1f / (float)(IntegerAxis.MAX_VALUE - IntegerAxis.MIN_VALUE + 1) * num;
			for (int i = 0; i <= IntegerAxis.TOTAL_ZONES; i++)
			{
				Vector2 b = Vector2.up * num3 * (float)(i + IntegerAxis.MIN_VALUE);
				Vector2 v = vector - Vector2.right * num2 + b + Vector2.down * num3 / 2f;
				Vector2 v2 = vector + Vector2.right * num2 + b + Vector2.down * num3 / 2f;
				GizmoUtil.GizmosDrawLine(v, v2, Color.green);
			}
			for (int j = 0; j <= IntegerAxis.TOTAL_ZONES; j++)
			{
				Vector2 b2 = Vector2.right * num3 * (float)(j + IntegerAxis.MIN_VALUE);
				Vector2 v3 = vector + Vector2.up * num2 + b2 - Vector2.right * num3 / 2f;
				Vector2 v4 = vector - Vector2.up * num2 + b2 - Vector2.right * num3 / 2f;
				GizmoUtil.GizmosDrawLine(v3, v4, Color.green);
			}
			float d = (float)integerAxis.RawIntegerValue * num3;
			float d2 = (float)integerAxis2.RawIntegerValue * num3;
			GizmoUtil.GizmoFillRectangle(vector + Vector2.right * d + Vector2.up * d2, Vector2.one * num3, Color.red);
			GizmoUtil.GizmosDrawCircle(vector, num2, Color.yellow, 40);
			Vector2 a = (Vector2)this.nonVoidController.GetAxisValue();
			a.Normalize();
			GizmoUtil.GizmosDrawLine(vector, vector + a * num2, Color.cyan);
		}
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000B2B3C File Offset: 0x000B0F3C
	public void onUpdateDamage(GameEvent message)
	{
		UpdateDamageCommand updateDamageCommand = message as UpdateDamageCommand;
		if (updateDamageCommand.PlayerNum == this.PlayerNum)
		{
			this.Damage = updateDamageCommand.Damage;
		}
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000B2B70 File Offset: 0x000B0F70
	private void onAttemptTeamDynamicMove(GameEvent message)
	{
		AttemptTeamDynamicMoveCommand attemptTeamDynamicMoveCommand = message as AttemptTeamDynamicMoveCommand;
		if (attemptTeamDynamicMoveCommand.playerNum == this.PlayerNum)
		{
			bool flag = false;
			if (this.CanUsePowerMove)
			{
				foreach (PlayerReference playerReference in base.gameController.currentGame.PlayerReferences)
				{
					if (playerReference.PlayerNum != this.PlayerNum && playerReference.Controller.Team == attemptTeamDynamicMoveCommand.team && playerReference.CanHostTeamMove)
					{
						playerReference.Controller.TeamDynamicMove(attemptTeamDynamicMoveCommand.spawnParticle);
						this.model.teamPowerMoveCooldownFrames = attemptTeamDynamicMoveCommand.cooldownFrames;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				base.audioManager.PlayMenuSound(SoundKey.generic_dynamicMoveDenied, 0f);
			}
		}
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000B2C68 File Offset: 0x000B1068
	public void TeamDynamicMove(ParticleData spawnParticle)
	{
		if (spawnParticle != null)
		{
			this.GameVFX.PlayParticle(spawnParticle, BodyPart.shield, this.Team);
		}
		base.audioManager.PlayMenuSound(SoundKey.generic_dynamicMoveActivate, 0f);
		this.Shield.BeginGusting();
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000B2CA4 File Offset: 0x000B10A4
	public void TeamPowerMove(ParticleData spawnParticle, CreateArticleAction[] assistArticles)
	{
		if (spawnParticle != null)
		{
			this.GameVFX.PlayParticle(spawnParticle, BodyPart.root, this.Team);
		}
		base.audioManager.PlayMenuSound(SoundKey.generic_powerMoveActivate, 0f);
		foreach (CreateArticleAction createArticleAction in assistArticles)
		{
			ArticleSpawnParameters articleSpawnParameters = this.articleSpawnCalculator.Calculate(createArticleAction, InputButtonsData.EmptyInput, this, null);
			GameObject prefab = createArticleAction.data.prefab;
			if (createArticleAction.data.teamParticle)
			{
				UIColor uicolorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
				if (uicolorFromTeam == UIColor.Blue)
				{
					prefab = createArticleAction.data.bluePrefab;
				}
				else if (uicolorFromTeam == UIColor.Red)
				{
					prefab = createArticleAction.data.redPrefab;
				}
			}
			ArticleController articleController = ArticleData.CreateArticleController(base.gameManager.DynamicObjects, createArticleAction.data.type, prefab, 4);
			articleController.model.physicsModel.Reset();
			articleController.model.physicsModel.position = articleSpawnParameters.sourcePosition;
			articleController.model.rotationAngle = articleSpawnParameters.rotation;
			articleController.model.physicsModel.AddVelocity(ref articleSpawnParameters.velocity, VelocityType.Movement);
			articleController.model.currentFacing = articleSpawnParameters.facing;
			articleController.model.playerOwner = this.PlayerNum;
			articleController.model.team = this.Team;
			articleController.model.movementType = createArticleAction.movementType;
			articleController.model.moveLabel = MoveLabel.AllyAssist;
			articleController.model.moveName = "AllyAssist";
			articleController.model.moveUID = -1;
			articleController.model.staleDamageMultiplier = Fixed.Create(1.0);
			articleController.model.chargeData = null;
			articleController.model.chargeFraction = Fixed.Create(1.0);
			articleController.Init(createArticleAction.data);
		}
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000B2E9C File Offset: 0x000B129C
	private void onSpawnCommand(GameEvent message)
	{
		CharacterSpawnCommand characterSpawnCommand = message as CharacterSpawnCommand;
		if (characterSpawnCommand.player == this.PlayerNum)
		{
			if (!this.State.IsDead && this.IsInBattle)
			{
				Debug.LogWarning("Player " + this.PlayerNum + " received respawn command but was not dead/benched");
			}
			this.Reference.EngagementState = characterSpawnCommand.spawnType;
			if (characterSpawnCommand.spawnType == PlayerEngagementState.Temporary)
			{
				this.model.temporaryDurationFrames = characterSpawnCommand.temporarySpawnDurationFrames;
				this.model.temporaryDurationTotalFrames = this.model.temporaryDurationFrames;
			}
			this.respawn(characterSpawnCommand.spawnPoint, characterSpawnCommand.startingDamagePercent);
		}
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000B2F51 File Offset: 0x000B1351
	void IPlayerDelegate.AddHostedMaterialAnimation(MaterialAnimationTrigger materialAnimation)
	{
		this.MaterialAnimationsController.AddAnimation(materialAnimation);
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000B2F60 File Offset: 0x000B1360
	void IPlayerDelegate.AddHostedHit(HostedHit hit)
	{
		this.model.hostedHits.Add(hit);
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			for (int i = 0; i < hit.hitBoxes.Count; i++)
			{
				if (hit == null)
				{
					Debug.LogWarning("Null hit in hosted hit");
				}
				else
				{
					HitBoxState hitBoxState = hit.hitBoxes[i];
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.Transform);
					capsule.Load(hitBoxState.position, hitBoxState.lastPosition, (Fixed)((double)hitBoxState.data.radius), WColor.DebugHitboxRed, hitBoxState.IsCircle);
					this.model.hostedHitCapsules[hitBoxState] = capsule;
				}
			}
		}
	}

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x06002378 RID: 9080 RVA: 0x000B301D File Offset: 0x000B141D
	Dictionary<HitBoxState, CapsuleShape> IPlayerDelegate.HitCapsules
	{
		get
		{
			return this.model.hostedHitCapsules;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06002379 RID: 9081 RVA: 0x000B302A File Offset: 0x000B142A
	IAnimationPlayer IPlayerDataOwner.AnimationPlayer
	{
		get
		{
			return this.AnimationPlayer;
		}
	}

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x000B3032 File Offset: 0x000B1432
	public IBodyOwner Body
	{
		get
		{
			return this.hitBoxController;
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x0600237B RID: 9083 RVA: 0x000B303A File Offset: 0x000B143A
	IMoveInput IPlayerDelegate.PlayerInput
	{
		get
		{
			return this.nonVoidController;
		}
	}

	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x0600237C RID: 9084 RVA: 0x000B3042 File Offset: 0x000B1442
	List<ICharacterComponent> IPlayerDelegate.Components
	{
		get
		{
			return this.characterComponents;
		}
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x0600237D RID: 9085 RVA: 0x000B304C File Offset: 0x000B144C
	public bool IsComponentRollingPlayer
	{
		get
		{
			for (int i = 0; i < this.characterComponents.Count; i++)
			{
				if (this.characterComponents[i] is IRotationCharacterComponent && (this.characterComponents[i] as IRotationCharacterComponent).IsRotationRolled)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x0600237E RID: 9086 RVA: 0x000B30AC File Offset: 0x000B14AC
	public Fixed ComponentPlayerRoll
	{
		get
		{
			for (int i = 0; i < this.characterComponents.Count; i++)
			{
				if (this.characterComponents[i] is IRotationCharacterComponent)
				{
					IRotationCharacterComponent rotationCharacterComponent = this.characterComponents[i] as IRotationCharacterComponent;
					if (rotationCharacterComponent.IsRotationRolled)
					{
						return rotationCharacterComponent.Roll;
					}
				}
			}
			return 0;
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x0600237F RID: 9087 RVA: 0x000B3118 File Offset: 0x000B1518
	public bool IsRotationRolled
	{
		get
		{
			return this.State.IsGrabbedState || (this.State.ActionState == ActionState.HitTumbleSpin && !this.State.IsHitLagPaused) || (this.activeMove.IsActive && this.activeMove.Data.rotateInMovementDirection) || this.IsComponentRollingPlayer;
		}
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000B3188 File Offset: 0x000B1588
	public bool HasAnimationOverride(ActionState actionState, HorizontalDirection facing, ref string animName)
	{
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is ICharacterAnimationComponent)
			{
				ICharacterAnimationComponent characterAnimationComponent = characterComponent as ICharacterAnimationComponent;
				if (characterAnimationComponent.IsOverridingActionStateAnimation(actionState, facing, ref animName))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06002381 RID: 9089 RVA: 0x000B3208 File Offset: 0x000B1608
	public bool PreventActionStateAnimations
	{
		get
		{
			foreach (ICharacterComponent characterComponent in this.characterComponents)
			{
				if (characterComponent is ICharacterAnimationComponent)
				{
					ICharacterAnimationComponent characterAnimationComponent = characterComponent as ICharacterAnimationComponent;
					return characterAnimationComponent.PreventActionStateAnimations;
				}
			}
			return false;
		}
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000B3280 File Offset: 0x000B1680
	public HorizontalDirection CalculateVictimFacing(bool hitWasReversed)
	{
		if (this.ActiveMove != null && this.ActiveMove.Model.deferredFacing != HorizontalDirection.None)
		{
			return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.ActiveMove.Model.deferredFacing) : this.ActiveMove.Model.deferredFacing;
		}
		return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.Facing) : this.Facing;
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000B32FC File Offset: 0x000B16FC
	void IPlayerDelegate.LoadInstanceData(IPlayerDelegate other)
	{
		if (other is PlayerController)
		{
			PlayerController playerController = (PlayerController)other;
			this.model.damage = playerController.Model.damage;
			this.model.lastHitByPlayerNum = playerController.Model.lastHitByPlayerNum;
			this.model.lastHitByTeamNum = playerController.Model.lastHitByTeamNum;
			this.model.lastHitFrame = playerController.Model.lastHitFrame;
			if (this.Shield != null && playerController.Shield != null)
			{
				this.Shield.ShieldHealth = playerController.Shield.ShieldHealth;
			}
			this.model.moveUses.Clear();
			foreach (KeyValuePair<MoveLabel, int> keyValuePair in playerController.Model.moveUses)
			{
				this.model.moveUses[keyValuePair.Key] = playerController.Model.moveUses[keyValuePair.Key];
			}
			this.staleMoveQueue = playerController.staleMoveQueue;
		}
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000B3438 File Offset: 0x000B1838
	void IPlayerDelegate.ForceGetUp()
	{
		this.ForceGetUp();
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000B3440 File Offset: 0x000B1840
	public bool IsStandingOnStageSurface(out RaycastHitData surfaceHit)
	{
		return this.Physics.IsStandingOnStageSurface(out surfaceHit);
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000B3450 File Offset: 0x000B1850
	bool PlayerStateActor.IPlayerActorDelegate.CanJump()
	{
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is IJumpBlocker && !(characterComponent as IJumpBlocker).CanJump)
			{
				return false;
			}
		}
		return this.State.CanJump;
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000B34D4 File Offset: 0x000B18D4
	bool IPlayerDelegate.CanWallJump(HorizontalDirection wallJumpDirection)
	{
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is IWallJumpBlocker && !(characterComponent as IWallJumpBlocker).CanWallJump)
			{
				return false;
			}
		}
		return this.State.CanWallJump(wallJumpDirection);
	}

	// Token: 0x170007C0 RID: 1984
	// (get) Token: 0x06002388 RID: 9096 RVA: 0x000B355C File Offset: 0x000B195C
	bool IPlayerDelegate.IsLedgeGrabbingBlocked
	{
		get
		{
			foreach (ICharacterComponent characterComponent in this.characterComponents)
			{
				if (characterComponent is ILedgeGrabBlocker && (characterComponent as ILedgeGrabBlocker).IsLedgeGrabbingBlocked)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000B35D8 File Offset: 0x000B19D8
	void IPlayerDelegate.RestartCurrentActionState(bool startAnimationAtCurrentFrame)
	{
		this.StateActor.RestartCurrentActionState(startAnimationAtCurrentFrame);
	}

	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x0600238A RID: 9098 RVA: 0x000B35E6 File Offset: 0x000B19E6
	MoveData IMoveOwner.MoveData
	{
		get
		{
			if (this.ActiveMove != null && this.ActiveMove.Model != null)
			{
				return this.ActiveMove.Model.data;
			}
			return null;
		}
	}

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x0600238B RID: 9099 RVA: 0x000B3615 File Offset: 0x000B1A15
	bool IMoveOwner.MoveIsValid
	{
		get
		{
			return this.ActiveMove != null && this.ActiveMove.Model != null;
		}
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x0600238C RID: 9100 RVA: 0x000B3636 File Offset: 0x000B1A36
	int IMoveOwner.InternalFrame
	{
		get
		{
			if (this.ActiveMove != null && this.ActiveMove.Model != null)
			{
				return this.ActiveMove.Model.internalFrame;
			}
			return 0;
		}
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000B3668 File Offset: 0x000B1A68
	void IPlayerDelegate.PlayHologram(HologramData hologramData)
	{
		ParticleData hologramParticle = base.config.defaultCharacterEffects.hologram;
		if (hologramData.hasOverrideVFX && hologramData.overrideVFX != null)
		{
			hologramParticle = hologramData.overrideVFX;
		}
		base.events.Broadcast(new HologramDisplayCommand(this, hologramParticle, base.config.defaultCharacterEffects.hologramBeam, hologramData.texture));
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000B36CB File Offset: 0x000B1ACB
	private bool isMuteHologram()
	{
		return this.userGameplaySettingsModel.MuteEnemyHolos && base.battleServerAPI.IsSinglePlayerNetworkGame && base.battleServerAPI.GetPrimaryLocalPlayer != this.PlayerNum;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000B3706 File Offset: 0x000B1B06
	private bool isMuteVoiceline()
	{
		return this.userGameplaySettingsModel.MuteEnemyHolos && base.battleServerAPI.IsSinglePlayerNetworkGame && base.battleServerAPI.GetPrimaryLocalPlayer != this.PlayerNum;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000B3744 File Offset: 0x000B1B44
	public void GauntletProceed()
	{
		bool flag = false;
		foreach (PlayerReference playerReference in base.gameManager.PlayerReferences)
		{
			if (playerReference.Team != this.gauntletConditions.CurrentTeam)
			{
				playerReference.Controller.GauntletDemoRetry();
				if (!flag)
				{
					flag = true;
					base.gameManager.PlayerSpawner.GauntletRespawn(playerReference.PlayerNum, PlayerEngagementState.Primary);
				}
			}
		}
		base.signalBus.GetSignal<RoundCountSignal>().Dispatch(this.gauntletConditions.RoundCount);
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000B37FC File Offset: 0x000B1BFC
	public void GauntletDemoRetry()
	{
		this.Lives = base.gameManager.BattleSettings.lives;
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06002392 RID: 9106 RVA: 0x000B3814 File Offset: 0x000B1C14
	private GauntletEndGameCondition gauntletConditions
	{
		get
		{
			foreach (EndGameCondition endGameCondition in base.gameManager.CurrentGameMode.EndGameConditions)
			{
				if (endGameCondition is GauntletEndGameCondition)
				{
					return endGameCondition as GauntletEndGameCondition;
				}
			}
			return null;
		}
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000B3890 File Offset: 0x000B1C90
	public void FreeHologram(HologramData hologramData)
	{
		if (hologramData == null)
		{
			return;
		}
		ParticleData particle = base.config.defaultCharacterEffects.hologram;
		if (hologramData.hasOverrideVFX && hologramData.overrideVFX != null)
		{
			particle = hologramData.overrideVFX;
		}
		Vector3F position = this.Position;
		if (this.Facing == HorizontalDirection.Right)
		{
			position.x += base.config.tauntSettings.holoOffsetX;
		}
		else
		{
			position.x -= base.config.tauntSettings.holoOffsetX;
		}
		position.y += base.config.tauntSettings.holoOffsetY;
		Effect effectController = this.GameVFX.PlayParticle(particle, position, true).EffectController;
		VFXHologramController component = effectController.GetComponent<VFXHologramController>();
		if (component != null)
		{
			component.SetHologramData(hologramData.texture);
		}
		if (this.isMuteHologram())
		{
			effectController.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000B39A0 File Offset: 0x000B1DA0
	public void FreeVoiceTaunt(VoiceTauntData voiceTauntData)
	{
		if (this.isMuteVoiceline())
		{
			return;
		}
		AudioRequest request;
		if (!this.characterData.isPartner)
		{
			request = new AudioRequest(voiceTauntData.primaryAudioData, this.AudioOwner, null);
		}
		else
		{
			request = new AudioRequest(voiceTauntData.partnerAudioData, this.AudioOwner, null);
		}
		base.audioManager.PlayGameSound(request);
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000B3A04 File Offset: 0x000B1E04
	public void PlayVoiceTaunt(VoiceTauntData voiceTauntData)
	{
		AudioRequest request;
		if (!this.characterData.isPartner)
		{
			request = new AudioRequest(voiceTauntData.primaryAudioData, this.AudioOwner, null);
		}
		else
		{
			request = new AudioRequest(voiceTauntData.partnerAudioData, this.AudioOwner, null);
		}
		base.audioManager.PlayGameSound(request);
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000B3A5C File Offset: 0x000B1E5C
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PlayerModel>(ref this.model);
		this.Orientation.LoadState(container);
		this.AnimationPlayer.LoadState(container);
		this.Physics.LoadState(container);
		if (this.inputController != null)
		{
			this.inputController.LoadState(container);
		}
		this.activeMove.LoadState(container);
		this.staleMoveQueue.LoadState(container);
		this.Combat.LoadState(container);
		this.Shield.LoadState(container);
		this.Renderer.LoadState(container);
		this.RespawnController.LoadState(container);
		this.Bones.LoadState(container);
		this.Invincibility.LoadState(container);
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is IRollbackStateOwner)
			{
				((IRollbackStateOwner)characterComponent).LoadState(container);
			}
		}
		return true;
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000B3B84 File Offset: 0x000B1F84
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<PlayerModel>(this.model));
		this.Orientation.ExportState(ref container);
		this.AnimationPlayer.ExportState(ref container);
		this.Physics.ExportState(ref container);
		if (this.inputController != null)
		{
			this.inputController.ExportState(ref container);
		}
		this.activeMove.ExportState(ref container);
		this.staleMoveQueue.ExportState(ref container);
		this.Combat.ExportState(ref container);
		this.Shield.ExportState(ref container);
		this.Renderer.ExportState(ref container);
		this.RespawnController.ExportState(ref container);
		this.Bones.ExportState(ref container);
		this.Invincibility.ExportState(ref container);
		foreach (ICharacterComponent characterComponent in this.characterComponents)
		{
			if (characterComponent is IRollbackStateOwner)
			{
				((IRollbackStateOwner)characterComponent).ExportState(ref container);
			}
		}
		return true;
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000B3CB8 File Offset: 0x000B20B8
	public void OnFlinched()
	{
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.Flinched);
		this.ExecuteCharacterComponents<IFlinchListener>(new PlayerController.ComponentExecution<IFlinchListener>(this.onFlinchedComponentFn));
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000B3CD4 File Offset: 0x000B20D4
	public void OnGrabbed()
	{
		this.ExecuteCharacterComponents<IGrabListener>(new PlayerController.ComponentExecution<IGrabListener>(this.onGrabComponentFn));
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000B3CE9 File Offset: 0x000B20E9
	public void OnDropInput()
	{
		this.ExecuteCharacterComponents<IDropListener>(new PlayerController.ComponentExecution<IDropListener>(this.onDropComponentFn));
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000B3CFE File Offset: 0x000B20FE
	public void DispatchInteraction(PlayerController.InteractionSignalData.Type type)
	{
		base.signalBus.GetSignal<PlayerController.InteractionSignal>().Dispatch(new PlayerController.InteractionSignalData(this, type));
	}

	// Token: 0x04001A9B RID: 6811
	private List<BodyPart> visualBoundsBodyParts = new List<BodyPart>
	{
		BodyPart.head,
		BodyPart.leftFoot,
		BodyPart.rightFoot,
		BodyPart.leftHand,
		BodyPart.rightHand
	};

	// Token: 0x04001A9C RID: 6812
	public FixedRect visualBounds;

	// Token: 0x04001A9D RID: 6813
	private bool cameraAerialMode;

	// Token: 0x04001A9E RID: 6814
	private Vector2 cameraPosition;

	// Token: 0x04001A9F RID: 6815
	private Vector3 characterTextureOffset;

	// Token: 0x04001AA0 RID: 6816
	private bool isOffstage;

	// Token: 0x04001AA3 RID: 6819
	public Color iconColor;

	// Token: 0x04001AA4 RID: 6820
	public PlayerProfile thisProfile;

	// Token: 0x04001AA5 RID: 6821
	private PlayerModel model = new PlayerModel();

	// Token: 0x04001AA6 RID: 6822
	private CharacterData characterData;

	// Token: 0x04001AA7 RID: 6823
	private CharacterMenusData characterMenusData;

	// Token: 0x04001AA8 RID: 6824
	private SkinData skinData;

	// Token: 0x04001AAA RID: 6826
	private GUIText debugText;

	// Token: 0x04001AAB RID: 6827
	private InputController cachedAllyController;

	// Token: 0x04001AAC RID: 6828
	private GameModeData modeData;

	// Token: 0x04001AAE RID: 6830
	private PlayerPhysicsController physics;

	// Token: 0x04001AAF RID: 6831
	private BoneController hitBoxController;

	// Token: 0x04001AB0 RID: 6832
	private MaterialTargetsData materialTargetsData;

	// Token: 0x04001AB1 RID: 6833
	private BoneData boneData;

	// Token: 0x04001AB2 RID: 6834
	private StaleMoveQueue staleMoveQueue;

	// Token: 0x04001ABE RID: 6846
	private IDebugStringComponent debugTextController;

	// Token: 0x04001AC2 RID: 6850
	private IMoveUseTracker moveUseTracker;

	// Token: 0x04001AC3 RID: 6851
	private MoveController activeMove;

	// Token: 0x04001AC4 RID: 6852
	private GameObject character;

	// Token: 0x04001AC5 RID: 6853
	private List<ICharacterComponent> characterComponents = new List<ICharacterComponent>();

	// Token: 0x04001AC6 RID: 6854
	private List<IItem> heldItems = new List<IItem>();

	// Token: 0x04001AC7 RID: 6855
	private GameObject customRespawnPlatform;

	// Token: 0x04001AC8 RID: 6856
	private List<Hit> activeHitsBuffer = new List<Hit>();

	// Token: 0x04001AC9 RID: 6857
	private HitDisableDataMap hitDisableDataBuffer = new HitDisableDataMap();

	// Token: 0x04001ACA RID: 6858
	private CharacterActionData cachedActionData;

	// Token: 0x04001ACB RID: 6859
	private ColorMode[] allColorModes = EnumUtil.GetValues<ColorMode>();

	// Token: 0x04001ACC RID: 6860
	private static HashSet<ColorMode> colorModeFlagsPersisted = new HashSet<ColorMode>(default(ColorModeComparer))
	{
		ColorMode.Helpless,
		ColorMode.Invincible
	};

	// Token: 0x04001ACD RID: 6861
	private PlayerController.ComponentExecution<IMoveTickListener> tickMoveComponent;

	// Token: 0x04001ACE RID: 6862
	private Action<ParticleData, GameObject> onParticleCreated;

	// Token: 0x04001ACF RID: 6863
	private PlayerController.ComponentExecution<IDeathListener> onDeath;

	// Token: 0x04001AD0 RID: 6864
	private List<HitBoxState> shieldBoxBuffer = new List<HitBoxState>();

	// Token: 0x020005E1 RID: 1505
	public class InteractionSignalData
	{
		// Token: 0x060023A3 RID: 9123 RVA: 0x000B3D86 File Offset: 0x000B2186
		public InteractionSignalData(IMoveOwner moveOwner, PlayerController.InteractionSignalData.Type type)
		{
			this.moveOwner = moveOwner;
			this.trigger = type;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000B3D9C File Offset: 0x000B219C
		public InteractionSignalData()
		{
		}

		// Token: 0x04001AD5 RID: 6869
		public IMoveOwner moveOwner;

		// Token: 0x04001AD6 RID: 6870
		public PlayerController.InteractionSignalData.Type trigger;

		// Token: 0x020005E2 RID: 1506
		public enum Type
		{
			// Token: 0x04001AD8 RID: 6872
			None,
			// Token: 0x04001AD9 RID: 6873
			MoveEnd,
			// Token: 0x04001ADA RID: 6874
			Land,
			// Token: 0x04001ADB RID: 6875
			TakeDamage,
			// Token: 0x04001ADC RID: 6876
			DealDamage,
			// Token: 0x04001ADD RID: 6877
			Grabbed,
			// Token: 0x04001ADE RID: 6878
			Flinched,
			// Token: 0x04001ADF RID: 6879
			Died
		}
	}

	// Token: 0x020005E3 RID: 1507
	public class InteractionSignal : Signal<PlayerController.InteractionSignalData>
	{
	}

	// Token: 0x020005E4 RID: 1508
	// (Invoke) Token: 0x060023A7 RID: 9127
	public delegate bool ComponentExecution<T>(T component);
}
