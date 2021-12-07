// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel : RollbackStateTyped<PlayerModel>
{
	[IgnoreCopyValidation, IsClonedManually]
	public List<HostedHit> hostedHits = new List<HostedHit>(32);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static), IsClonedManually]
	private List<TimedHostedHit> timedHostedHitsPool = new List<TimedHostedHit>();

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hostedHitCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public Vector3F displayOffset;

	[IgnoreCopyValidation, IsClonedManually]
	public SerializableDictionary<MoveLabel, int> moveUses = new SerializableDictionary<MoveLabel, int>(16, default(MoveLabelComparer));

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public LandingOverrideData landingOverrideData;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public HelplessStateData helplessStateData;

	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public MoveData bufferMoveData;

	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public InterruptData bufferInterruptData;

	[IgnoreCopyValidation, IsClonedManually]
	public GrabData grabData = new GrabData();

	[IgnoreCopyValidation, IsClonedManually]
	public BufferedPlayerInput bufferedInput = new BufferedPlayerInput();

	public Fixed damage;

	public int stunFrames;

	public int bufferActivatedFrame;

	public ButtonPress bufferActivatedButton;

	public int knockbackIteration;

	public bool clearInputBufferOnStunEnd;

	public int smokeTrailFrames;

	public StunType stunType;

	public StunTechMode stunTechMode;

	public bool previousFrameBufferTap;

	public bool stunIsSpike;

	public bool untechableBounceUsed;

	public bool floatyNameStun;

	public int jumpStunFrames;

	public int hitLagFrames;

	public int ignoreHitLagFrames;

	public int chainGrabPreventionFrames;

	public int tumbleWiggleBreakInputCount;

	public ButtonPress tumbleWiggleLastButton = ButtonPress.None;

	public bool isClankLag;

	public bool isKillCamHitlag;

	public bool isCameraZoomHitLag;

	public bool isFlourishOwner;

	public HorizontalDirection facing;

	public MetaState state;

	public SubStates subState;

	public int lastTechFrame;

	public bool isActive = true;

	public ActionState actionState = ActionState.None;

	public int actionStateFrame;

	public int jumpBeginFrame;

	public int overrideActionStateInterruptibilityFrames;

	public ButtonPress jumpButtonSource;

	public int teamPowerMoveCooldownFrames;

	public bool assistAbsorbsHits;

	public int temporaryDurationTotalFrames = -1;

	public int temporaryDurationFrames = -1;

	public int temporaryAssistImmunityFrames = -1;

	public int blastZoneImmunityFrames;

	public int userIdleFrames;

	public string lastMoveName;

	public int repeatTrackMoveCount;

	public bool isDead;

	public int isDeadForFrames;

	public bool isRespawning;

	public Vector3F rotation;

	public int fallThroughPlatformHeldFrames;

	public bool landedWithAirDodge;

	public HorizontalDirection lastTapDirection;

	public int lastConsumedTapInput;

	public int lastBackTapFrame;

	public int buttonsPressedThisFrame;

	public int lastLeftInputFrame;

	public int lastRightInputFrame;

	public bool isInterruptMoveBuffered;

	public ButtonPress bufferButtonUsed;

	public int grabbedLedgeIndex = -1;

	public int ledgeReleaseFrame;

	public int ledgeGrabbedFrame;

	public int ledgeLagFrames;

	public int ledgeGrabsSinceLanding;

	public int ledgeGrabCooldownFrames;

	public int downedFrames;

	public int dashDanceFrames;

	public bool processingBufferedInput;

	public int hitOwnerID;

	public bool ignoreNextHelplessness;

	public bool queuedWavedashDodge;

	public PlayerNum lastHitByPlayerNum;

	public TeamNum lastHitByTeamNum;

	public int lastHitFrame;

	public int emoteCooldownFrames;

	public int emoteFrameLimitStart;

	public int emoteFrameLimitCounter;

	public bool wasFastFallVelocity;

	public PlayerModel()
	{
		for (int i = 0; i < this.hostedHits.Capacity; i++)
		{
			this.timedHostedHitsPool.Add(new TimedHostedHit());
		}
	}

	public override void CopyTo(PlayerModel targetIn)
	{
		targetIn.damage = this.damage;
		targetIn.stunFrames = this.stunFrames;
		targetIn.bufferActivatedFrame = this.bufferActivatedFrame;
		targetIn.bufferActivatedButton = this.bufferActivatedButton;
		targetIn.knockbackIteration = this.knockbackIteration;
		targetIn.clearInputBufferOnStunEnd = this.clearInputBufferOnStunEnd;
		targetIn.smokeTrailFrames = this.smokeTrailFrames;
		targetIn.stunType = this.stunType;
		targetIn.stunTechMode = this.stunTechMode;
		targetIn.stunIsSpike = this.stunIsSpike;
		targetIn.previousFrameBufferTap = this.previousFrameBufferTap;
		targetIn.untechableBounceUsed = this.untechableBounceUsed;
		targetIn.floatyNameStun = this.floatyNameStun;
		targetIn.jumpStunFrames = this.jumpStunFrames;
		targetIn.hitLagFrames = this.hitLagFrames;
		targetIn.teamPowerMoveCooldownFrames = this.teamPowerMoveCooldownFrames;
		targetIn.ignoreHitLagFrames = this.ignoreHitLagFrames;
		targetIn.tumbleWiggleBreakInputCount = this.tumbleWiggleBreakInputCount;
		targetIn.tumbleWiggleLastButton = this.tumbleWiggleLastButton;
		targetIn.isClankLag = this.isClankLag;
		targetIn.isKillCamHitlag = this.isKillCamHitlag;
		targetIn.isCameraZoomHitLag = this.isCameraZoomHitLag;
		targetIn.isFlourishOwner = this.isFlourishOwner;
		targetIn.facing = this.facing;
		targetIn.state = this.state;
		targetIn.subState = this.subState;
		targetIn.lastTechFrame = this.lastTechFrame;
		targetIn.isActive = this.isActive;
		targetIn.actionState = this.actionState;
		targetIn.actionStateFrame = this.actionStateFrame;
		targetIn.jumpBeginFrame = this.jumpBeginFrame;
		targetIn.overrideActionStateInterruptibilityFrames = this.overrideActionStateInterruptibilityFrames;
		targetIn.jumpButtonSource = this.jumpButtonSource;
		targetIn.temporaryDurationTotalFrames = this.temporaryDurationTotalFrames;
		targetIn.temporaryDurationFrames = this.temporaryDurationFrames;
		targetIn.temporaryAssistImmunityFrames = this.temporaryAssistImmunityFrames;
		targetIn.assistAbsorbsHits = this.assistAbsorbsHits;
		targetIn.blastZoneImmunityFrames = this.blastZoneImmunityFrames;
		targetIn.userIdleFrames = this.userIdleFrames;
		targetIn.lastMoveName = this.lastMoveName;
		targetIn.repeatTrackMoveCount = this.repeatTrackMoveCount;
		targetIn.isDead = this.isDead;
		targetIn.isDeadForFrames = this.isDeadForFrames;
		targetIn.isRespawning = this.isRespawning;
		targetIn.rotation = this.rotation;
		targetIn.fallThroughPlatformHeldFrames = this.fallThroughPlatformHeldFrames;
		targetIn.lastTapDirection = this.lastTapDirection;
		targetIn.lastConsumedTapInput = this.lastConsumedTapInput;
		targetIn.lastBackTapFrame = this.lastBackTapFrame;
		targetIn.buttonsPressedThisFrame = this.buttonsPressedThisFrame;
		targetIn.lastLeftInputFrame = this.lastLeftInputFrame;
		targetIn.lastRightInputFrame = this.lastRightInputFrame;
		targetIn.isInterruptMoveBuffered = this.isInterruptMoveBuffered;
		targetIn.bufferButtonUsed = this.bufferButtonUsed;
		targetIn.grabbedLedgeIndex = this.grabbedLedgeIndex;
		targetIn.ledgeReleaseFrame = this.ledgeReleaseFrame;
		targetIn.ledgeGrabbedFrame = this.ledgeGrabbedFrame;
		targetIn.ledgeLagFrames = this.ledgeLagFrames;
		targetIn.ledgeGrabCooldownFrames = this.ledgeGrabCooldownFrames;
		targetIn.ledgeGrabsSinceLanding = this.ledgeGrabsSinceLanding;
		targetIn.downedFrames = this.downedFrames;
		targetIn.dashDanceFrames = this.dashDanceFrames;
		targetIn.processingBufferedInput = this.processingBufferedInput;
		targetIn.hitOwnerID = this.hitOwnerID;
		targetIn.ignoreNextHelplessness = this.ignoreNextHelplessness;
		targetIn.queuedWavedashDodge = this.queuedWavedashDodge;
		targetIn.lastHitByPlayerNum = this.lastHitByPlayerNum;
		targetIn.lastHitByTeamNum = this.lastHitByTeamNum;
		targetIn.lastHitFrame = this.lastHitFrame;
		targetIn.chainGrabPreventionFrames = this.chainGrabPreventionFrames;
		targetIn.emoteCooldownFrames = this.emoteCooldownFrames;
		targetIn.emoteFrameLimitStart = this.emoteFrameLimitStart;
		targetIn.emoteFrameLimitCounter = this.emoteFrameLimitCounter;
		targetIn.wasFastFallVelocity = this.wasFastFallVelocity;
		targetIn.landedWithAirDodge = this.landedWithAirDodge;
		targetIn.displayOffset = this.displayOffset;
		targetIn.landingOverrideData = this.landingOverrideData;
		targetIn.helplessStateData = this.helplessStateData;
		this.grabData.CopyTo(targetIn.grabData);
		this.bufferedInput.CopyTo(targetIn.bufferedInput);
		targetIn.bufferMoveData = this.bufferMoveData;
		targetIn.bufferInterruptData = this.bufferInterruptData;
		base.copyDictionary<MoveLabel, int>(this.moveUses, targetIn.moveUses);
		base.copyDictionary<HitBoxState, CapsuleShape>(this.hostedHitCapsules, targetIn.hostedHitCapsules);
		targetIn.hostedHits.Clear();
		for (int i = 0; i < this.hostedHits.Count; i++)
		{
			if (this.hostedHits[i] is TimedHostedHit)
			{
				targetIn.hostedHits.Add(targetIn.timedHostedHitsPool[i]);
				(this.hostedHits[i] as TimedHostedHit).CopyTo(targetIn.hostedHits[i] as TimedHostedHit);
			}
		}
	}

	public override object Clone()
	{
		PlayerModel playerModel = new PlayerModel();
		this.CopyTo(playerModel);
		return playerModel;
	}

	public void ClearLastHitData()
	{
		this.lastHitByPlayerNum = PlayerNum.None;
		this.lastHitByTeamNum = TeamNum.None;
		this.lastHitFrame = 0;
	}

	public void ClearLastTumbleData()
	{
		this.tumbleWiggleLastButton = ButtonPress.None;
		this.tumbleWiggleBreakInputCount = 0;
	}
}
