// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : GameBehavior, IHitOwner, ITrailOwner, IPlayerDelegate, ITickable, IRollbackStateOwner, IBoundsOwner, IPhysicsDelegate, IFacing, ICameraInfluencer, IPlayerInputActor, IPlayerDataOwner, IPositionOwner, IMoveOwner, PlayerStateActor.IPlayerActorDelegate, IItemHolder
{
	public class InteractionSignalData
	{
		public enum Type
		{
			None,
			MoveEnd,
			Land,
			TakeDamage,
			DealDamage,
			Grabbed,
			Flinched,
			Died
		}

		public IMoveOwner moveOwner;

		public PlayerController.InteractionSignalData.Type trigger;

		public InteractionSignalData(IMoveOwner moveOwner, PlayerController.InteractionSignalData.Type type)
		{
			this.moveOwner = moveOwner;
			this.trigger = type;
		}

		public InteractionSignalData()
		{
		}
	}

	public class InteractionSignal : Signal<PlayerController.InteractionSignalData>
	{
	}

	public delegate bool ComponentExecution<T>(T component);

	private sealed class _Init_c__AnonStorey0
	{
		internal PlayerSelectionInfo playerInfo;

		internal GameModeData modeData;

		internal bool __m__0(ICharacterInitListener listener)
		{
			listener.OnCharacterInit(this.playerInfo, this.modeData);
			return false;
		}
	}

	private sealed class _onEngagementStateChanged_c__AnonStorey1
	{
		internal PlayerEngagementStateChangedEvent changed;

		internal bool __m__0(IEngagementStateListener listener)
		{
			listener.OnEngagementStateChanged(this.changed.engagement);
			return false;
		}
	}

	private List<BodyPart> visualBoundsBodyParts = new List<BodyPart>
	{
		BodyPart.head,
		BodyPart.leftFoot,
		BodyPart.rightFoot,
		BodyPart.leftHand,
		BodyPart.rightHand
	};

	public FixedRect visualBounds;

	private bool cameraAerialMode;

	private Vector2 cameraPosition;

	private Vector3 characterTextureOffset;

	private bool isOffstage;

	public Color iconColor;

	public PlayerProfile thisProfile;

	private PlayerModel model = new PlayerModel();

	private CharacterData characterData;

	private CharacterMenusData characterMenusData;

	private SkinData skinData;

	private GUIText debugText;

	private InputController cachedAllyController;

	private GameModeData modeData;

	private PlayerPhysicsController physics;

	private BoneController hitBoxController;

	private MaterialTargetsData materialTargetsData;

	private BoneData boneData;

	private StaleMoveQueue staleMoveQueue;

	private IDebugStringComponent debugTextController;

	private IMoveUseTracker moveUseTracker;

	private MoveController activeMove;

	private GameObject character;

	private List<ICharacterComponent> characterComponents = new List<ICharacterComponent>();

	private List<IItem> heldItems = new List<IItem>();

	private GameObject customRespawnPlatform;

	private List<Hit> activeHitsBuffer = new List<Hit>();

	private HitDisableDataMap hitDisableDataBuffer = new HitDisableDataMap();

	private CharacterActionData cachedActionData;

	private ColorMode[] allColorModes = EnumUtil.GetValues<ColorMode>();

	private static HashSet<ColorMode> colorModeFlagsPersisted = new HashSet<ColorMode>(default(ColorModeComparer))
	{
		ColorMode.Helpless,
		ColorMode.Invincible
	};

	private PlayerController.ComponentExecution<IMoveTickListener> tickMoveComponent;

	private Action<ParticleData, GameObject> onParticleCreated;

	private PlayerController.ComponentExecution<IDeathListener> onDeath;

	private List<HitBoxState> shieldBoxBuffer = new List<HitBoxState>();

	private static PlayerController.ComponentExecution<IDeathListener> __f__am_cache0;

	private static PlayerController.ComponentExecution<ICharacterStartListener> __f__am_cache1;

	private static PlayerController.ComponentExecution<IRemovedfromGameListener> __f__am_cache2;

	private static PlayerController.ComponentExecution<IRespawnListener> __f__am_cache3;

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

	Vector2 ICameraInfluencer.Position
	{
		get
		{
			return this.getCameraPosition();
		}
	}

	AudioManager IPlayerDataOwner.Audio
	{
		get
		{
			return base.audioManager;
		}
	}

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

	bool IPlayerDataOwner.AreInputsLocked
	{
		get
		{
			return !base.gameManager.StartedGame && !base.config.uiuxSettings.emotiveStartup;
		}
	}

	bool IPlayerInputActor.TriggerHeldInputAsTaps
	{
		get
		{
			CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			return action != null && action.triggerHeldInputAsTap;
		}
	}

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

	CharacterData IPlayerDataOwner.CharacterData
	{
		get
		{
			return this.characterData;
		}
	}

	CharacterMenusData IPlayerDataOwner.CharacterMenusData
	{
		get
		{
			return this.characterMenusData;
		}
	}

	List<IItem> IItemHolder.HeldItems
	{
		get
		{
			return this.heldItems;
		}
	}

	bool IBoundsOwner.AllowPushing
	{
		get
		{
			return this.IsActive && this.IsInBattle && this.State.IsGrounded && !this.isShovingForbidden() && this.physics.Velocity.magnitude < base.config.knockbackConfig.ignoreShoveThreshold;
		}
	}

	bool IBoundsOwner.AllowTotalShove
	{
		get
		{
			return this.ActiveMove.IsActive && this.ActiveMove.Model.data.shovesEnemies;
		}
	}

	bool ITrailOwner.EmitTrail
	{
		get
		{
			return this.IsActive && !this.State.IsDead && ((!this.State.IsGrounded && this.State.IsTumbling && this.State.IsStunned && !this.State.IsHitLagPaused && this.Model.smokeTrailFrames >= 0) || (this.activeMove != null && this.activeMove.IsActive && this.activeMove.EmitTrail));
		}
	}

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

	HitOwnerType IHitOwner.Type
	{
		get
		{
			return HitOwnerType.Player;
		}
	}

	bool IHitOwner.IsProjectile
	{
		get
		{
			return false;
		}
	}

	ICharacterPhysicsData IPhysicsDelegate.Data
	{
		get
		{
			return this.Physics;
		}
	}

	CharacterPhysicsData IPhysicsDelegate.DefaultData
	{
		get
		{
			return ((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData;
		}
	}

	Fixed IPhysicsDelegate.GetDirectionHeldAmount
	{
		get
		{
			return this.nonVoidController.GetAxis(this.nonVoidController.horizontalAxis);
		}
	}

	bool IPhysicsDelegate.IsUnderContinuousForce
	{
		get
		{
			return this.ActiveMove.IsActive && this.ActiveMove.Model.applyForceContinuouslyEndFrame != -1;
		}
	}

	MoveData IPhysicsDelegate.CurrentMove
	{
		get
		{
			return this.activeMove.IsActive ? this.activeMove.Data : null;
		}
	}

	Dictionary<HitBoxState, CapsuleShape> IPlayerDelegate.HitCapsules
	{
		get
		{
			return this.model.hostedHitCapsules;
		}
	}

	IAnimationPlayer IPlayerDataOwner.AnimationPlayer
	{
		get
		{
			return this.AnimationPlayer;
		}
	}

	IMoveInput IPlayerDelegate.PlayerInput
	{
		get
		{
			return this.nonVoidController;
		}
	}

	List<ICharacterComponent> IPlayerDelegate.Components
	{
		get
		{
			return this.characterComponents;
		}
	}

	bool IPlayerDelegate.IsLedgeGrabbingBlocked
	{
		get
		{
			foreach (ICharacterComponent current in this.characterComponents)
			{
				if (current is ILedgeGrabBlocker && (current as ILedgeGrabBlocker).IsLedgeGrabbingBlocked)
				{
					return true;
				}
			}
			return false;
		}
	}

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

	bool IMoveOwner.MoveIsValid
	{
		get
		{
			return this.ActiveMove != null && this.ActiveMove.Model != null;
		}
	}

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

	[Inject]
	public ISpawnController spawnController
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public IInputSettingsScreenAPI api
	{
		get;
		set;
	}

	[Inject]
	public IPlayerTauntsFinder playerTauntsFinder
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IPlayerTauntsFinder tauntFinder
	{
		get;
		set;
	}

	[Inject]
	public IRespawnPlatformLocator respawnPlatformLocator
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

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[Inject]
	public IHitContextPool hitContextPool
	{
		get;
		set;
	}

	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager
	{
		get;
		set;
	}

	[Inject]
	public IUserGameplaySettingsModel userGameplaySettingsModel
	{
		get;
		set;
	}

	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator
	{
		get;
		set;
	}

	public bool IsCurrentFrame
	{
		get
		{
			return base.gameController.currentGame == null || base.gameController.currentGame.FrameController.IsCurrentFrame;
		}
	}

	public bool CanUsePowerMove
	{
		get
		{
			return this.Reference.IsBenched && this.model.teamPowerMoveCooldownFrames == 0;
		}
	}

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

	public CharacterData CharacterData
	{
		get
		{
			return this.characterData;
		}
	}

	public CharacterMenusData CharacterMenusData
	{
		get
		{
			return this.characterMenusData;
		}
	}

	public SkinData SkinData
	{
		get
		{
			return this.skinData;
		}
	}

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

	public PlayerNum PlayerNum
	{
		get
		{
			return this.Reference.PlayerNum;
		}
	}

	public bool IsInvincible
	{
		get
		{
			return this.Invincibility.IsInvincible;
		}
	}

	public Transform Transform
	{
		get
		{
			return base.transform;
		}
	}

	public Vector3F Position
	{
		get
		{
			return this.physics.State.position;
		}
	}

	public Vector3F MovementVelocity
	{
		get
		{
			return this.physics.State.movementVelocity;
		}
	}

	public Vector3F Center
	{
		get
		{
			return this.physics.Center;
		}
	}

	public Vector3F EmitPosition
	{
		get
		{
			return this.physics.Center;
		}
	}

	public StaleMoveQueue StaleMoves
	{
		get
		{
			return this.staleMoveQueue;
		}
	}

	public IMoveUseTracker MoveUseTracker
	{
		get
		{
			return this.moveUseTracker;
		}
	}

	public MoveController ActiveMove
	{
		get
		{
			return this.activeMove;
		}
	}

	public float CurrentDamage
	{
		get
		{
			return (float)this.model.damage;
		}
	}

	public IMoveSet MoveSet
	{
		get;
		private set;
	}

	public PlayerPhysicsController Physics
	{
		get
		{
			return this.physics;
		}
	}

	public InputController InputController
	{
		get
		{
			return this.inputController;
		}
	}

	public IGameInput GameInput
	{
		get
		{
			return this.nonVoidController;
		}
	}

	public IFrameOwner FrameOwner
	{
		get
		{
			return base.gameManager;
		}
	}

	public ConfigData Config
	{
		get
		{
			return base.config;
		}
	}

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

	public Fixed FacingInterpolation
	{
		get;
		set;
	}

	public int FacingTurnaroundWait
	{
		get;
		set;
	}

	public HorizontalDirection WaitingForFacingTurnaround
	{
		get;
		set;
	}

	public HorizontalDirection OppositeFacing
	{
		get
		{
			return (this.Facing != HorizontalDirection.Right) ? HorizontalDirection.Right : HorizontalDirection.Left;
		}
	}

	public bool IsOffstage
	{
		get
		{
			return this.isOffstage;
		}
	}

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

	public bool IsFlourishMode
	{
		get
		{
			return this.State.IsCameraFlourishMode;
		}
	}

	public bool IsZoomMode
	{
		get
		{
			return this.State.IsCameraZoomMode;
		}
	}

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

	public Vector3 Velocity
	{
		get
		{
			return (Vector3)this.Physics.Velocity;
		}
	}

	public EnvironmentBounds Bounds
	{
		get
		{
			return this.physics.Bounds;
		}
	}

	public GrabData GrabData
	{
		get
		{
			return this.model.grabData;
		}
	}

	public TeamNum Team
	{
		get
		{
			return this.Reference.Team;
		}
	}

	public IBoneController Bones
	{
		get
		{
			return this.hitBoxController;
		}
	}

	public MaterialAnimationsController MaterialAnimationsController
	{
		get;
		private set;
	}

	public bool IsLedgeGrabbing
	{
		get
		{
			return this.LedgeGrabController.IsLedgeGrabbing;
		}
	}

	public IPlayerStateActor StateActor
	{
		get;
		set;
	}

	public bool ReadAnyBufferedInput
	{
		get
		{
			return this.ActionStateReadsAnyBufferedInput || this.MoveReadsAnyBufferedInput;
		}
	}

	public bool ActionStateReadsAnyBufferedInput
	{
		get
		{
			CharacterActionData action = this.MoveSet.Actions.GetAction(this.State.ActionState, false);
			return action != null && action.readAnyBufferedInput;
		}
	}

	public bool MoveReadsAnyBufferedInput
	{
		get
		{
			return this.ActiveMove.Data != null && this.activeMove.Data.readAnyBufferedInput;
		}
	}

	public bool MoveDoesntReadBufferedMovement
	{
		get
		{
			return this.ActiveMove.Data != null && this.activeMove.Data.dontReadMovementInputs;
		}
	}

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

	public bool IsEliminated
	{
		get
		{
			return this.Lives == 0 && (base.gameManager == null || base.gameManager.ModeData.settings.usesLives);
		}
	}

	public bool IsInBattle
	{
		get
		{
			return this.Reference.IsInBattle;
		}
	}

	public bool IsAllyAssist
	{
		get
		{
			return this.Reference.IsAllyAssistMove;
		}
	}

	public bool AssistAbsorbsHits
	{
		get
		{
			return this.model.assistAbsorbsHits;
		}
	}

	public int TemporaryAssistImmuneFrames
	{
		get
		{
			return this.model.temporaryAssistImmunityFrames;
		}
	}

	public bool IsTemporary
	{
		get
		{
			return this.Reference.IsTemporary;
		}
	}

	public int TemporaryDurationFrames
	{
		get
		{
			return this.model.temporaryDurationFrames;
		}
	}

	public Fixed TemporaryDurationPercent
	{
		get
		{
			return this.model.temporaryDurationFrames / this.model.temporaryDurationTotalFrames;
		}
	}

	public PlayerReference Reference
	{
		get;
		private set;
	}

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

	private InputController inputController
	{
		get
		{
			return this.Reference.InputController;
		}
	}

	private InputController nonVoidController
	{
		get
		{
			return (!(this.Reference.InputController != null)) ? this.allyController : this.Reference.InputController;
		}
	}

	public IRespawnController RespawnController
	{
		get
		{
			return this.Reference.respawnController;
		}
	}

	public IAnimationPlayer AnimationPlayer
	{
		get;
		private set;
	}

	public IAudioOwner AudioOwner
	{
		get;
		private set;
	}

	public ICombatController Combat
	{
		get;
		private set;
	}

	public IShield Shield
	{
		get;
		private set;
	}

	public IGameVFX GameVFX
	{
		get;
		private set;
	}

	public ICharacterRenderer Renderer
	{
		get;
		private set;
	}

	public IInvincibilityController Invincibility
	{
		get;
		private set;
	}

	public IPlayerOrientation Orientation
	{
		get;
		private set;
	}

	public TrailEmitter TrailEmitter
	{
		get;
		private set;
	}

	public TrailEmitter KnockbackEmitter
	{
		get;
		private set;
	}

	private ICharacterInputProcessor inputProcessor
	{
		get;
		set;
	}

	public IPlayerState State
	{
		get;
		private set;
	}

	public IGrabController GrabController
	{
		get;
		private set;
	}

	public ILedgeGrabController LedgeGrabController
	{
		get;
		private set;
	}

	public IPlayerCameraBoxController CameraBoxController
	{
		get;
		private set;
	}

	private GrabData grabData
	{
		get
		{
			return this.model.grabData;
		}
	}

	private bool allowBeingPushed
	{
		get
		{
			return !this.isShovingForbidden() && this.physics.Velocity.magnitude < base.config.knockbackConfig.ignoreShoveThreshold;
		}
	}

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

	public Fixed StaleDamageMultiplier
	{
		get
		{
			return (!this.activeMove.IsActive) ? ((Fixed)1.0) : this.activeMove.Model.StaleDamageMultiplier;
		}
	}

	public Fixed HitLagMultiplier
	{
		get
		{
			return 1;
		}
	}

	public bool AllowFastFall
	{
		get
		{
			foreach (ICharacterComponent current in this.characterComponents)
			{
				if (current is IDropListener && !(current as IDropListener).AllowFastFall)
				{
					return false;
				}
			}
			return true;
		}
	}

	public IBodyOwner Body
	{
		get
		{
			return this.hitBoxController;
		}
	}

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

	public bool IsRotationRolled
	{
		get
		{
			return this.State.IsGrabbedState || (this.State.ActionState == ActionState.HitTumbleSpin && !this.State.IsHitLagPaused) || (this.activeMove.IsActive && this.activeMove.Data.rotateInMovementDirection) || this.IsComponentRollingPlayer;
		}
	}

	public bool PreventActionStateAnimations
	{
		get
		{
			foreach (ICharacterComponent current in this.characterComponents)
			{
				if (current is ICharacterAnimationComponent)
				{
					ICharacterAnimationComponent characterAnimationComponent = current as ICharacterAnimationComponent;
					return characterAnimationComponent.PreventActionStateAnimations;
				}
			}
			return false;
		}
	}

	private GauntletEndGameCondition gauntletConditions
	{
		get
		{
			foreach (EndGameCondition current in base.gameManager.CurrentGameMode.EndGameConditions)
			{
				if (current is GauntletEndGameCondition)
				{
					return current as GauntletEndGameCondition;
				}
			}
			return null;
		}
	}

	public PlayerController()
	{
		this.tickMoveComponent = new PlayerController.ComponentExecution<IMoveTickListener>(this._PlayerController_m__0);
		this.onParticleCreated = new Action<ParticleData, GameObject>(this._PlayerController_m__1);
		if (PlayerController.__f__am_cache0 == null)
		{
			PlayerController.__f__am_cache0 = new PlayerController.ComponentExecution<IDeathListener>(PlayerController._PlayerController_m__2);
		}
		this.onDeath = PlayerController.__f__am_cache0;
	}

	public IGameInput GetGameInput()
	{
		return this.nonVoidController;
	}

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

	public Rect GetScreenspaceClearRect()
	{
		Rect rect = (Rect)this.GetBoundsForScreenSpaceClear();
		Vector3 vector = base.gameManager.Camera.current.WorldToScreenPoint(new Vector2(rect.xMin, rect.yMin - rect.height));
		Vector3 a = base.gameManager.Camera.current.WorldToScreenPoint(new Vector2(rect.xMax, rect.yMin));
		Rect result = new Rect(vector, a - vector);
		return result;
	}

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

	void IItemHolder.TakeItem(IItem pItem)
	{
		if (this.heldItems != null && pItem != null)
		{
			this.heldItems.Remove(pItem);
		}
	}

	void IItemHolder.GiveItem(IItem pItem)
	{
		if (this.heldItems != null && pItem != null)
		{
			this.heldItems.Add(pItem);
		}
	}

	public void Init(PlayerSelectionInfo playerInfo, PlayerReference reference, GameModeData modeData, SpawnPointBase spawnPoint)
	{
		PlayerController._Init_c__AnonStorey0 _Init_c__AnonStorey = new PlayerController._Init_c__AnonStorey0();
		_Init_c__AnonStorey.playerInfo = playerInfo;
		_Init_c__AnonStorey.modeData = modeData;
		this.Reference = reference;
		this.staleMoveQueue = new StaleMoveQueue();
		base.injector.Inject(this.staleMoveQueue);
		this.staleMoveQueue.Init(base.config.staleMoves);
		CharacterDefinition characterDefinition = this.characterDataHelper.GetCharacterDefinition(_Init_c__AnonStorey.playerInfo);
		this.characterData = this.characterDataLoader.GetData(characterDefinition);
		this.characterMenusData = this.characterMenusDataLoader.GetData(characterDefinition);
		this.LoadComponents(this.characterData.components);
		this.modeData = _Init_c__AnonStorey.modeData;
		this.customRespawnPlatform = null;
		GameObject prefab = this.respawnPlatformLocator.GetPrefab(_Init_c__AnonStorey.playerInfo);
		if (prefab != null)
		{
			this.customRespawnPlatform = UnityEngine.Object.Instantiate<GameObject>(prefab);
			this.customRespawnPlatform.SetActive(false);
			this.customRespawnPlatform.transform.SetParent(null, false);
		}
		reference.respawnController.Init(reference, this.customRespawnPlatform);
		PlayerController.ComponentExecution<ICharacterInitListener> execute = new PlayerController.ComponentExecution<ICharacterInitListener>(_Init_c__AnonStorey.__m__0);
		this.ExecuteCharacterComponents<ICharacterInitListener>(execute);
		this.debugText = base.gameManager.UI.GetDebugText(this.PlayerNum);
		this.skinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(this.characterData.characterID, _Init_c__AnonStorey.playerInfo.skinKey));
		GameObject skinPrefab = this.characterDataHelper.GetSkinPrefab(characterDefinition, this.skinData);
		if (skinPrefab == null)
		{
			UnityEngine.Debug.LogError("Character prefab for " + base.gameObject.name + " not found. Make sure you have selected a prefab character in the Character Editor");
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
		this.iconColor = PlayerUtil.GetColor(_Init_c__AnonStorey.playerInfo, _Init_c__AnonStorey.modeData.settings.usesTeams);
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
		(this.Renderer as CharacterRenderer).Init(this, new List<Renderer>(this.character.GetComponentsInChildren<Renderer>()), _Init_c__AnonStorey.modeData.settings.usesTeams);
		this.Renderer.SetColorModeFlag(ColorMode.Inactive, !this.IsActive);
		MeshRenderer[] componentsInChildren = this.character.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshRenderer meshRenderer = componentsInChildren[i];
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
		this.thisProfile = _Init_c__AnonStorey.playerInfo.curProfile;
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
		if (PlayerController.__f__am_cache1 == null)
		{
			PlayerController.__f__am_cache1 = new PlayerController.ComponentExecution<ICharacterStartListener>(PlayerController._Init_m__3);
		}
		this.ExecuteCharacterComponents<ICharacterStartListener>(PlayerController.__f__am_cache1);
		this.inputProcessor.Cache();
	}

	public void InitMaterials()
	{
		this.MaterialAnimationsController = base.injector.GetInstance<MaterialAnimationsController>();
		this.MaterialAnimationsController.Init(this, this.materialTargetsData);
	}

	private void subscribeListeners()
	{
		base.events.Subscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Subscribe(typeof(UpdateDamageCommand), new Events.EventHandler(this.onUpdateDamage));
		base.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		base.events.Subscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onSpawnCommand));
		base.events.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Subscribe(typeof(AttemptTeamDynamicMoveCommand), new Events.EventHandler(this.onAttemptTeamDynamicMove));
	}

	private void unsubscribeListeners()
	{
		base.events.Unsubscribe(typeof(RespawnPlatformExpireEvent), new Events.EventHandler(this.onRespawnPlatformExpire));
		base.events.Unsubscribe(typeof(UpdateDamageCommand), new Events.EventHandler(this.onUpdateDamage));
		base.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugChannel));
		base.events.Unsubscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onSpawnCommand));
		base.events.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
		base.events.Unsubscribe(typeof(AttemptTeamDynamicMoveCommand), new Events.EventHandler(this.onAttemptTeamDynamicMove));
	}

	private Dictionary<ButtonPress, MoveData> getTauntOverrides()
	{
		Dictionary<ButtonPress, MoveData> dictionary = new Dictionary<ButtonPress, MoveData>();
		UserTaunts forPlayer = this.playerTauntsFinder.GetForPlayer(this.PlayerNum);
		Dictionary<TauntSlot, EquipmentID> slotsForCharacter = forPlayer.GetSlotsForCharacter(this.characterData.characterID);
		foreach (TauntSlot current in slotsForCharacter.Keys)
		{
			EquipmentID id = slotsForCharacter[current];
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item != null)
			{
				ButtonPress key = ButtonPress.None;
				switch (current)
				{
				case TauntSlot.UP:
					key = ButtonPress.TauntUp;
					break;
				case TauntSlot.DOWN:
					key = ButtonPress.TauntDown;
					break;
				case TauntSlot.LEFT:
					key = ButtonPress.TauntLeft;
					break;
				case TauntSlot.RIGHT:
					key = ButtonPress.TauntRight;
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
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	public void LoadSharedAnimations(List<PlayerReference> allPlayers)
	{
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < allPlayers.Count; i++)
		{
			foreach (PlayerController current in allPlayers[i].AllControllers)
			{
				if (!hashSet.Contains(current.CharacterData.characterName))
				{
					hashSet.Add(current.CharacterData.characterName);
					this.AnimationPlayer.LoadSharedAnimations(current.MoveSet.MoveSetData, current.CharacterData);
				}
				CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(current.CharacterData.characterDefinition);
				for (int j = 0; j < linkedCharacters.Length; j++)
				{
					CharacterDefinition characterDefinition = linkedCharacters[j];
					if (!hashSet.Contains(current.CharacterData.characterName))
					{
						hashSet.Add(current.CharacterData.characterName);
						this.AnimationPlayer.LoadSharedAnimations(current.MoveSet.MoveSetData, current.CharacterData);
					}
				}
			}
		}
	}

	private void onToggleDebugChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HitBoxes)
		{
			this.toggleHitBoxCapsules(toggleDebugDrawChannelCommand.enabled);
		}
	}

	private void mirrorPlayer(HorizontalDirection facing)
	{
		bool flag = facing == HorizontalDirection.Left;
		this.hitBoxController.InvertHurtBoxes(flag);
		this.AnimationPlayer.SetMecanimMirror(flag);
	}

	public void SetFacingAndRotation(HorizontalDirection direction)
	{
		this.SetFacing(direction);
		this.SetRotation(direction, true);
	}

	public void SetRotation(HorizontalDirection direction, bool allowMirror = true)
	{
		if (this.characterData.reversesStance && allowMirror)
		{
			this.mirrorPlayer(this.Facing);
		}
		this.Orientation.RotateY((direction != HorizontalDirection.Right) ? (-90) : 90);
	}

	public void SetFacing(HorizontalDirection direction)
	{
		this.Facing = direction;
	}

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

	public void PlayDelayedParticles()
	{
		this.GameVFX.PlayDelayedParticles();
	}

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
		this.isOffstage = PlayerUtil.IsOffstage(this, base.gameManager, (Fixed)0.89999997615814209);
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
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Action State mismatch: ",
				this.State.ActionState,
				" ",
				this.AnimationPlayer.CurrentActionState
			}));
		}
	}

	private int calculatePhysicsIterations()
	{
		int result = 1;
		if (base.config.knockbackConfig.useBalloonKnockback && this.State.IsStunned && !this.State.IsHitLagPaused && this.model.knockbackIteration > 0 && this.Physics.KnockbackVelocity.sqrMagnitude >= base.config.knockbackConfig.balloonKnockbackRequiredMomentum * base.config.knockbackConfig.balloonKnockbackRequiredMomentum)
		{
			result = this.model.knockbackIteration;
		}
		return result;
	}

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

	public Vector3 GetCharacterTextureCameraPosition()
	{
		return this.characterTextureOffset;
	}

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

	private bool tickMoveComponentFn(IMoveTickListener listener)
	{
		listener.OnTickMove(this.activeMove);
		return false;
	}

	private bool onLandComponentFn(ILandListener listener)
	{
		listener.OnLand();
		return false;
	}

	private bool onDeathComponentFn(IDeathListener listener)
	{
		listener.OnDeath();
		return false;
	}

	private bool onGrabComponentFn(IGrabListener listener)
	{
		listener.OnGrabbed();
		return false;
	}

	private bool onDropComponentFn(IDropListener listener)
	{
		listener.OnDrop();
		return false;
	}

	private bool onJumpComponentFn(IJumpListener listener)
	{
		listener.OnJump();
		return false;
	}

	private bool onFlinchedComponentFn(IFlinchListener listener)
	{
		listener.OnFlinch();
		return false;
	}

	private bool onMoveUsedComponentFn(IMoveUsedListener listener, MoveData move)
	{
		listener.OnMoveUsed(move);
		return false;
	}

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

	private bool isStateForUserIdle()
	{
		return this.Model.state == MetaState.LedgeHang || this.Model.state == MetaState.Stand;
	}

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

	private void toggleHitBoxCapsules(bool enabled)
	{
		if (enabled)
		{
			foreach (Hit current in this.Model.hostedHits)
			{
				foreach (HitBoxState current2 in current.hitBoxes)
				{
					CapsuleShape capsule = CapsulePool.Instance.GetCapsule(this.Transform);
					capsule.Load((Vector3)current2.position, (Vector3)current2.lastPosition, current2.data.radius, current.data.DebugDrawColor, current2.IsCircle);
					this.model.hostedHitCapsules[current2] = capsule;
				}
			}
		}
		else
		{
			foreach (CapsuleShape current3 in this.Model.hostedHitCapsules.Values)
			{
				current3.Clear();
			}
			this.Model.hostedHitCapsules.Clear();
		}
	}

	private void checkFastFallBuffer()
	{
		bool flag = this.physics.Velocity.y < 0;
		if (!this.model.wasFastFallVelocity && flag)
		{
			this.ProcessBufferedInput();
		}
		this.model.wasFastFallVelocity = flag;
	}

	public InputButtonsData ProcessInput(bool retainBuffer)
	{
		this.inputProcessor.ProcessInput(base.gameManager.Frame, this.nonVoidController, this.Reference, retainBuffer);
		return this.inputProcessor.CurrentInputData;
	}

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
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	void PlayerStateActor.IPlayerActorDelegate.ClearBufferedInput()
	{
		this.model.bufferedInput.Clear();
	}

	private void ForceGetUp()
	{
		this.SetMove(this.MoveSet.GetMove(MoveLabel.GetUp), InputButtonsData.EmptyInput, HorizontalDirection.None, 0, 0, default(Vector3F), null, null, default(MoveSeedData), ButtonPress.None);
	}

	private void tickStun(int iterations)
	{
		this.ReduceStunFrames(iterations);
	}

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

	private void tickDownedState()
	{
		if (!this.State.IsDownState || this.State.IsHitLagPaused)
		{
			return;
		}
		if (!this.State.IsGrounded)
		{
			UnityEngine.Debug.LogWarning("Downed while not grounded! This should not happen");
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

	public void ClearDamage()
	{
		this.Model.damage = 0;
		this.staleMoveQueue.Clear();
	}

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

	private void onRemovedFromGame()
	{
		if (PlayerController.__f__am_cache2 == null)
		{
			PlayerController.__f__am_cache2 = new PlayerController.ComponentExecution<IRemovedfromGameListener>(PlayerController._onRemovedFromGame_m__4);
		}
		this.ExecuteCharacterComponents<IRemovedfromGameListener>(PlayerController.__f__am_cache2);
		this.StateActor.ReleaseShield(false, false);
		this.TrailEmitter.ResetData();
		this.KnockbackEmitter.ResetData();
		this.moveUseTracker.OnRemovedFromGame();
	}

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
		if (PlayerController.__f__am_cache3 == null)
		{
			PlayerController.__f__am_cache3 = new PlayerController.ComponentExecution<IRespawnListener>(PlayerController._respawn_m__5);
		}
		this.ExecuteCharacterComponents<IRespawnListener>(PlayerController.__f__am_cache3);
		this.OnSpawned();
	}

	public void OnSpawned()
	{
		this.Renderer.ToggleVisibility(true);
	}

	private void onRespawnPlatformExpire(GameEvent message)
	{
		RespawnPlatformExpireEvent respawnPlatformExpireEvent = message as RespawnPlatformExpireEvent;
		if (respawnPlatformExpireEvent.playerNum == this.PlayerNum)
		{
			this.onDismountRespawnPlatform();
		}
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerController._onEngagementStateChanged_c__AnonStorey1 _onEngagementStateChanged_c__AnonStorey = new PlayerController._onEngagementStateChanged_c__AnonStorey1();
		_onEngagementStateChanged_c__AnonStorey.changed = (message as PlayerEngagementStateChangedEvent);
		if (_onEngagementStateChanged_c__AnonStorey.changed.playerNum == this.PlayerNum)
		{
			switch (_onEngagementStateChanged_c__AnonStorey.changed.engagement)
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
				UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"Player ",
					this.PlayerNum,
					" engagement changed to unsupported type  ",
					_onEngagementStateChanged_c__AnonStorey.changed.engagement
				}));
				break;
			}
			this.ExecuteCharacterComponents<IEngagementStateListener>(new PlayerController.ComponentExecution<IEngagementStateListener>(_onEngagementStateChanged_c__AnonStorey.__m__0));
		}
	}

	public void DismountRespawnPlatform()
	{
		this.RespawnController.Dismount();
	}

	private void onDismountRespawnPlatform()
	{
		this.Invincibility.EndPlatformIntangibility();
		this.Invincibility.BeginInvincibility(base.config.respawnConfig.dismountInvincibilityFrames);
		this.model.isRespawning = false;
		this.State.MetaState = MetaState.Jump;
		this.physics.SyncGroundState();
	}

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
				Fixed other2 = this.characterData.attackAssistDairHorizontalOffset * ((this.Facing != HorizontalDirection.Left) ? (-1) : 1);
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
				InputProfileEntry[] entries = move.activeInputProfile.entries;
				for (int k = 0; k < entries.Length; k++)
				{
					InputProfileEntry inputProfileEntry = entries[k];
					ButtonPress[] buttonsHeld = inputProfileEntry.buttonsHeld;
					for (int l = 0; l < buttonsHeld.Length; l++)
					{
						ButtonPress press = buttonsHeld[l];
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
				for (int m = 0; m < array.Length; m++)
				{
					InterruptData interruptData = array[m];
					if (interruptData.interruptType == InterruptType.Move)
					{
						for (int n = 0; n < interruptData.linkableMoves.Length; n++)
						{
							MoveData moveData = interruptData.linkableMoves[n];
							if (moveData == null)
							{
								UnityEngine.Debug.LogError("Null link " + move.moveName);
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
				ButtonPress[] maskedBufferButtons = action.maskedBufferButtons;
				for (int i = 0; i < maskedBufferButtons.Length; i++)
				{
					ButtonPress item = maskedBufferButtons[i];
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
					MoveLabel[] array = validBufferedMoveLabels;
					for (int j = 0; j < array.Length; j++)
					{
						MoveLabel label = array[j];
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

	private void warmButtonDetection(InputButtonsData inputButtonsData, ButtonPress button)
	{
		InterruptData interruptData = null;
		ButtonPress buttonPress = ButtonPress.None;
		inputButtonsData.moveButtonsPressed.Add(button);
		this.MoveSet.GetMove(inputButtonsData, this, out interruptData, ref buttonPress);
		inputButtonsData.moveButtonsPressed.RemoveAt(inputButtonsData.moveButtonsPressed.Count - 1);
	}

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
		ColorMode[] array = this.allColorModes;
		for (int i = 0; i < array.Length; i++)
		{
			ColorMode colorMode = array[i];
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

	public void AddCharacterComponent(ICharacterComponent pComponent)
	{
		if (pComponent != null)
		{
			this.characterComponents.Add(pComponent);
		}
	}

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

	bool IHitOwner.ForceCollisionChecks(CollisionCheckType type, HitData hitData)
	{
		return type == CollisionCheckType.HitBox && (this.activeMove.IsActive && hitData.hitType == HitType.Counter);
	}

	bool IHitOwner.HandleComponentHitInteraction(Hit otherHit, IHitOwner other, CollisionCheckType checkType, HitContext hitContext)
	{
		return false;
	}

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

	bool IHitOwner.CanReflect(HitData hitData)
	{
		return hitData.reflectsProjectiles;
	}

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

	public void BeginHitLag(int hitLagFrames, IHitOwner owner, HitData hitData)
	{
		this.Combat.BeginHitLag(hitLagFrames, owner, hitData, false);
	}

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

	bool IHitOwner.ResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		return (!hitData.knockbackCausesFlinching && !this.State.IsAffectedByUnflinchingKnockback) || this.ArmorResistsHit(hitData, hitInstigator, hitPosition);
	}

	public bool ArmorResistsHit(HitData hitData, IHitOwner hitInstigator, Vector3F hitPosition)
	{
		Vector2F zero = Vector2F.zero;
		Fixed damage = this.combatCalculator.CalculateModifiedDamageUnstaled(hitData, hitInstigator);
		Fixed knockbackForce = this.combatCalculator.CalculateKnockback(hitData, hitInstigator, this, this.Model.damage, damage, hitPosition, out zero);
		return this.ActiveMove.DoesArmorResistHit(knockbackForce);
	}

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

	bool IHitOwner.OnHitSuccess(Hit hit, IHitOwner other, ImpactType impactType, ref Vector3F hitPosition, ref Vector3F hitVelocity, HitContext hitContext)
	{
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.DealDamage);
		HitData data = hit.data;
		MaterialAnimationTrigger[] materialAnimationTriggers = data.materialAnimationTriggers;
		for (int i = 0; i < materialAnimationTriggers.Length; i++)
		{
			MaterialAnimationTrigger materialAnimationTrigger = materialAnimationTriggers[i];
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
			UnityEngine.Debug.LogError("Tried to grab invalid type " + other.Type);
		}
		this.onHitDoLinks();
		return false;
	}

	private bool shouldStartFlourish(PlayerController defender, HitData hitData, Fixed damageDealt, Fixed unstaledDamage, Fixed knockbackForce, Vector3F hitPosition)
	{
		return base.config.flourishConfig.useKillFlourish && this.isFlourishPlayerCount() && this.isFlourishStockCount(defender) && this.isMinKnockback(knockbackForce) && this.isMinDamage(unstaledDamage) && this.isFlourishDistance(defender) && this.combatCalculator.IsLethalHit(hitData, this, defender, hitPosition, 1 / base.config.flourishConfig.requiredOverkill);
	}

	private bool isMinKnockback(Fixed knockbackForce)
	{
		if (base.config.flourishConfig.printDebug)
		{
			UnityEngine.Debug.Log("KNOCKBACK FORCE " + knockbackForce);
		}
		return knockbackForce >= base.config.flourishConfig.minKnockback;
	}

	private bool isMinDamage(Fixed damage)
	{
		return damage >= base.config.flourishConfig.minDamage;
	}

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
			UnityEngine.Debug.Log(string.Concat(new object[]
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
			UnityEngine.Debug.Log(string.Concat(new object[]
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
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"DISTANCE CHECKS x:",
				@fixed,
				" y:",
				fixed2
			}));
		}
		return (@fixed >= base.config.flourishConfig.minDistanceX && @fixed <= base.config.flourishConfig.maxDistanceX) || (fixed2 >= base.config.flourishConfig.minDistanceY && fixed2 <= base.config.flourishConfig.maxDistanceY);
	}

	private bool isFlourishStockCount(PlayerController defender)
	{
		return !base.config.flourishConfig.lastStockOnly || defender.Lives <= 1;
	}

	private bool isFlourishPlayerCount()
	{
		int num = 0;
		foreach (PlayerReference current in base.gameController.currentGame.PlayerReferences)
		{
			if (!current.IsEliminated && current.IsInBattle)
			{
				num++;
			}
		}
		return num <= 2;
	}

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

	bool IPhysicsDelegate.IsDirectionHeld(HorizontalDirection direction)
	{
		return this.nonVoidController.IsHorizontalDirectionHeld(direction);
	}

	bool IPhysicsDelegate.ShouldFallThroughPlatforms()
	{
		Fixed axis = this.nonVoidController.GetAxis(this.nonVoidController.verticalAxis);
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is IFallThroughPlatformBlocker && !(current as IFallThroughPlatformBlocker).CanFallThroughPlatform)
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

	bool IPhysicsDelegate.IsPlatformLastDropped(IPhysicsCollider platformCollider)
	{
		return this.Physics.PlayerState.lastPlatformDroppedThrough != null && platformCollider == this.Physics.PlayerState.lastPlatformDroppedThrough;
	}

	bool IPhysicsDelegate.ShouldBounce()
	{
		return this.State.IsStunned && this.State.IsTumbling && this.model.stunType == StunType.HitStun && !this.State.IsHitLagPaused && !this.State.IsShieldBroken;
	}

	bool IPhysicsDelegate.ShouldMaintainVelocityOnCollision()
	{
		return this.State.IsHitLagPaused;
	}

	bool IPhysicsDelegate.IgnorePhysicsCollisions()
	{
		return this.State.IsLedgeHangingState || this.State.IsLedgeGrabbing || (this.activeMove.IsActive && this.activeMove.Data.ignorePhysicsCollisions) || this.State.IsGrabbedState || this.State.IsRespawning;
	}

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

	public void ForceWindHit(Fixed angle, Fixed force, bool resetXVelocity, bool resetYVelocity)
	{
		this.Physics.StopMovement(resetXVelocity, resetYVelocity, VelocityType.Total);
		this.Physics.ClearFastFall();
		Vector2F a = MathUtil.AngleToVector(angle);
		this.Physics.AddVelocity(a * force, 1, VelocityType.Movement);
	}

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

	private Fixed getAccelerationMulti()
	{
		Fixed @fixed = 1;
		if (this.ActiveMove != null && this.ActiveMove.IsActive)
		{
			BlockMovementData[] blockMovementData = this.ActiveMove.Data.blockMovementData;
			for (int i = 0; i < blockMovementData.Length; i++)
			{
				BlockMovementData blockMovementData2 = blockMovementData[i];
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

	private Fixed getMoveBlockMaxAirSpeedMulti()
	{
		Fixed @fixed = 1;
		if (this.ActiveMove != null && this.ActiveMove.IsActive)
		{
			BlockMovementData[] blockMovementData = this.ActiveMove.Data.blockMovementData;
			for (int i = 0; i < blockMovementData.Length; i++)
			{
				BlockMovementData blockMovementData2 = blockMovementData[i];
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

	public void OnJump()
	{
		this.ExecuteCharacterComponents<IJumpListener>(new PlayerController.ComponentExecution<IJumpListener>(this.onJumpComponentFn));
	}

	public void OnGroundBounce()
	{
		this.moveUseTracker.Grounded();
	}

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

	public void StaleMove(MoveLabel label, string name, int uid)
	{
		this.staleMoveQueue.OnMoveHit(label, name, uid);
	}

	void PlayerStateActor.IPlayerActorDelegate.BeginStun(int frames, StunType stunType, bool cannotTech, bool isSpike, Fixed knockbackForce, Vector2F knockbackVelocity)
	{
		this.Combat.BeginStun(frames, stunType, cannotTech, isSpike, knockbackForce, knockbackVelocity, HitContext.Null, null);
	}

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

	public void LoadPhysicsData(CharacterPhysicsData physicsData)
	{
		((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData = physicsData;
		this.physics.LoadPhysicsData(((IDefaultCharacterPhysicsDataOwner)this.characterData).DefaultPhysicsData);
	}

	public void LoadMoveInfo(MoveData move)
	{
		this.MoveSet.LoadMoveInfo(move);
		this.AnimationPlayer.LoadMove(move, true);
	}

	public void LoadCharacterShieldData(CharacterShieldData shieldData)
	{
		this.characterData.shield = shieldData;
	}

	public void LoadCharacterParticleData(CharacterParticleData particleData)
	{
		this.characterData.particles = particleData;
	}

	public void LoadComponents(CharacterComponent[] components)
	{
		Dictionary<System.Type, IComponentState> dictionary = new Dictionary<System.Type, IComponentState>();
		using (List<ICharacterComponent>.Enumerator enumerator = this.characterComponents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterComponent characterComponent = (CharacterComponent)enumerator.Current;
				dictionary[characterComponent.GetType()] = characterComponent.State;
				characterComponent.Destroy();
			}
		}
		this.characterComponents.Clear();
		for (int i = 0; i < components.Length; i++)
		{
			CharacterComponent characterComponent2 = components[i];
			CharacterComponent characterComponent3 = UnityEngine.Object.Instantiate<CharacterComponent>(characterComponent2);
			base.injector.Inject(characterComponent3);
			characterComponent3.Init(this);
			if (dictionary.ContainsKey(characterComponent2.GetType()))
			{
				characterComponent3.LoadState(dictionary[characterComponent2.GetType()]);
			}
			this.characterComponents.Add(characterComponent3);
		}
	}

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
		using (List<ICharacterComponent>.Enumerator enumerator = this.characterComponents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterComponent characterComponent = (CharacterComponent)enumerator.Current;
				characterComponent.Destroy();
			}
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

	public void onUpdateDamage(GameEvent message)
	{
		UpdateDamageCommand updateDamageCommand = message as UpdateDamageCommand;
		if (updateDamageCommand.PlayerNum == this.PlayerNum)
		{
			this.Damage = updateDamageCommand.Damage;
		}
	}

	private void onAttemptTeamDynamicMove(GameEvent message)
	{
		AttemptTeamDynamicMoveCommand attemptTeamDynamicMoveCommand = message as AttemptTeamDynamicMoveCommand;
		if (attemptTeamDynamicMoveCommand.playerNum == this.PlayerNum)
		{
			bool flag = false;
			if (this.CanUsePowerMove)
			{
				foreach (PlayerReference current in base.gameController.currentGame.PlayerReferences)
				{
					if (current.PlayerNum != this.PlayerNum && current.Controller.Team == attemptTeamDynamicMoveCommand.team && current.CanHostTeamMove)
					{
						current.Controller.TeamDynamicMove(attemptTeamDynamicMoveCommand.spawnParticle);
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

	public void TeamDynamicMove(ParticleData spawnParticle)
	{
		if (spawnParticle != null)
		{
			this.GameVFX.PlayParticle(spawnParticle, BodyPart.shield, this.Team);
		}
		base.audioManager.PlayMenuSound(SoundKey.generic_dynamicMoveActivate, 0f);
		this.Shield.BeginGusting();
	}

	public void TeamPowerMove(ParticleData spawnParticle, CreateArticleAction[] assistArticles)
	{
		if (spawnParticle != null)
		{
			this.GameVFX.PlayParticle(spawnParticle, BodyPart.root, this.Team);
		}
		base.audioManager.PlayMenuSound(SoundKey.generic_powerMoveActivate, 0f);
		for (int i = 0; i < assistArticles.Length; i++)
		{
			CreateArticleAction createArticleAction = assistArticles[i];
			ArticleSpawnParameters articleSpawnParameters = this.articleSpawnCalculator.Calculate(createArticleAction, InputButtonsData.EmptyInput, this, null);
			GameObject prefab = createArticleAction.data.prefab;
			if (createArticleAction.data.teamParticle)
			{
				UIColor uIColorFromTeam = PlayerUtil.GetUIColorFromTeam(this.Team);
				if (uIColorFromTeam == UIColor.Blue)
				{
					prefab = createArticleAction.data.bluePrefab;
				}
				else if (uIColorFromTeam == UIColor.Red)
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

	private void onSpawnCommand(GameEvent message)
	{
		CharacterSpawnCommand characterSpawnCommand = message as CharacterSpawnCommand;
		if (characterSpawnCommand.player == this.PlayerNum)
		{
			if (!this.State.IsDead && this.IsInBattle)
			{
				UnityEngine.Debug.LogWarning("Player " + this.PlayerNum + " received respawn command but was not dead/benched");
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

	void IPlayerDelegate.AddHostedMaterialAnimation(MaterialAnimationTrigger materialAnimation)
	{
		this.MaterialAnimationsController.AddAnimation(materialAnimation);
	}

	void IPlayerDelegate.AddHostedHit(HostedHit hit)
	{
		this.model.hostedHits.Add(hit);
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			for (int i = 0; i < hit.hitBoxes.Count; i++)
			{
				if (hit == null)
				{
					UnityEngine.Debug.LogWarning("Null hit in hosted hit");
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

	public bool HasAnimationOverride(ActionState actionState, HorizontalDirection facing, ref string animName)
	{
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is ICharacterAnimationComponent)
			{
				ICharacterAnimationComponent characterAnimationComponent = current as ICharacterAnimationComponent;
				if (characterAnimationComponent.IsOverridingActionStateAnimation(actionState, facing, ref animName))
				{
					return true;
				}
			}
		}
		return false;
	}

	public HorizontalDirection CalculateVictimFacing(bool hitWasReversed)
	{
		if (this.ActiveMove != null && this.ActiveMove.Model.deferredFacing != HorizontalDirection.None)
		{
			return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.ActiveMove.Model.deferredFacing) : this.ActiveMove.Model.deferredFacing;
		}
		return (!hitWasReversed) ? InputUtils.GetOppositeDirection(this.Facing) : this.Facing;
	}

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
			foreach (KeyValuePair<MoveLabel, int> current in playerController.Model.moveUses)
			{
				this.model.moveUses[current.Key] = playerController.Model.moveUses[current.Key];
			}
			this.staleMoveQueue = playerController.staleMoveQueue;
		}
	}

	void IPlayerDelegate.ForceGetUp()
	{
		this.ForceGetUp();
	}

	public bool IsStandingOnStageSurface(out RaycastHitData surfaceHit)
	{
		return this.Physics.IsStandingOnStageSurface(out surfaceHit);
	}

	bool PlayerStateActor.IPlayerActorDelegate.CanJump()
	{
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is IJumpBlocker && !(current as IJumpBlocker).CanJump)
			{
				return false;
			}
		}
		return this.State.CanJump;
	}

	bool IPlayerDelegate.CanWallJump(HorizontalDirection wallJumpDirection)
	{
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is IWallJumpBlocker && !(current as IWallJumpBlocker).CanWallJump)
			{
				return false;
			}
		}
		return this.State.CanWallJump(wallJumpDirection);
	}

	void IPlayerDelegate.RestartCurrentActionState(bool startAnimationAtCurrentFrame)
	{
		this.StateActor.RestartCurrentActionState(startAnimationAtCurrentFrame);
	}

	void IPlayerDelegate.PlayHologram(HologramData hologramData)
	{
		ParticleData hologramParticle = base.config.defaultCharacterEffects.hologram;
		if (hologramData.hasOverrideVFX && hologramData.overrideVFX != null)
		{
			hologramParticle = hologramData.overrideVFX;
		}
		base.events.Broadcast(new HologramDisplayCommand(this, hologramParticle, base.config.defaultCharacterEffects.hologramBeam, hologramData.texture));
	}

	private bool isMuteHologram()
	{
		return this.userGameplaySettingsModel.MuteEnemyHolos && base.battleServerAPI.IsSinglePlayerNetworkGame && base.battleServerAPI.GetPrimaryLocalPlayer != this.PlayerNum;
	}

	private bool isMuteVoiceline()
	{
		return this.userGameplaySettingsModel.MuteEnemyHolos && base.battleServerAPI.IsSinglePlayerNetworkGame && base.battleServerAPI.GetPrimaryLocalPlayer != this.PlayerNum;
	}

	public void GauntletProceed()
	{
		bool flag = false;
		foreach (PlayerReference current in base.gameManager.PlayerReferences)
		{
			if (current.Team != this.gauntletConditions.CurrentTeam)
			{
				current.Controller.GauntletDemoRetry();
				if (!flag)
				{
					flag = true;
					base.gameManager.PlayerSpawner.GauntletRespawn(current.PlayerNum, PlayerEngagementState.Primary);
				}
			}
		}
		base.signalBus.GetSignal<RoundCountSignal>().Dispatch(this.gauntletConditions.RoundCount);
	}

	public void GauntletDemoRetry()
	{
		this.Lives = base.gameManager.BattleSettings.lives;
	}

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
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).LoadState(container);
			}
		}
		return true;
	}

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
		foreach (ICharacterComponent current in this.characterComponents)
		{
			if (current is IRollbackStateOwner)
			{
				((IRollbackStateOwner)current).ExportState(ref container);
			}
		}
		return true;
	}

	public void OnFlinched()
	{
		this.DispatchInteraction(PlayerController.InteractionSignalData.Type.Flinched);
		this.ExecuteCharacterComponents<IFlinchListener>(new PlayerController.ComponentExecution<IFlinchListener>(this.onFlinchedComponentFn));
	}

	public void OnGrabbed()
	{
		this.ExecuteCharacterComponents<IGrabListener>(new PlayerController.ComponentExecution<IGrabListener>(this.onGrabComponentFn));
	}

	public void OnDropInput()
	{
		this.ExecuteCharacterComponents<IDropListener>(new PlayerController.ComponentExecution<IDropListener>(this.onDropComponentFn));
	}

	public void DispatchInteraction(PlayerController.InteractionSignalData.Type type)
	{
		base.signalBus.GetSignal<PlayerController.InteractionSignal>().Dispatch(new PlayerController.InteractionSignalData(this, type));
	}

	private bool _PlayerController_m__0(IMoveTickListener listener)
	{
		return this.tickMoveComponentFn(listener);
	}

	private void _PlayerController_m__1(ParticleData data, GameObject obj)
	{
		this.onParticleCreatedFn(data, obj);
	}

	private static bool _PlayerController_m__2(IDeathListener listener)
	{
		listener.OnDeath();
		return false;
	}

	private static bool _Init_m__3(ICharacterStartListener listener)
	{
		listener.OnCharacterStart();
		return false;
	}

	private static bool _onRemovedFromGame_m__4(IRemovedfromGameListener listener)
	{
		listener.OnRemovedFromGame();
		return false;
	}

	private static bool _respawn_m__5(IRespawnListener listener)
	{
		listener.OnRespawn();
		return false;
	}
}
