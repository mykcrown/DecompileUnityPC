using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x020005EB RID: 1515
[Serializable]
public class PlayerModel : RollbackStateTyped<PlayerModel>
{
	// Token: 0x060023C5 RID: 9157 RVA: 0x000B4DE4 File Offset: 0x000B31E4
	public PlayerModel()
	{
		for (int i = 0; i < this.hostedHits.Capacity; i++)
		{
			this.timedHostedHitsPool.Add(new TimedHostedHit());
		}
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000B4EBC File Offset: 0x000B32BC
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

	// Token: 0x060023C7 RID: 9159 RVA: 0x000B5350 File Offset: 0x000B3750
	public override object Clone()
	{
		PlayerModel playerModel = new PlayerModel();
		this.CopyTo(playerModel);
		return playerModel;
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000B536B File Offset: 0x000B376B
	public void ClearLastHitData()
	{
		this.lastHitByPlayerNum = PlayerNum.None;
		this.lastHitByTeamNum = TeamNum.None;
		this.lastHitFrame = 0;
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000B5384 File Offset: 0x000B3784
	public void ClearLastTumbleData()
	{
		this.tumbleWiggleLastButton = ButtonPress.None;
		this.tumbleWiggleBreakInputCount = 0;
	}

	// Token: 0x04001AEB RID: 6891
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<HostedHit> hostedHits = new List<HostedHit>(32);

	// Token: 0x04001AEC RID: 6892
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.Static)]
	private List<TimedHostedHit> timedHostedHitsPool = new List<TimedHostedHit>();

	// Token: 0x04001AED RID: 6893
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public Dictionary<HitBoxState, CapsuleShape> hostedHitCapsules = new Dictionary<HitBoxState, CapsuleShape>(32, default(HitBoxStateComparer));

	// Token: 0x04001AEE RID: 6894
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public Vector3F displayOffset;

	// Token: 0x04001AEF RID: 6895
	[IsClonedManually]
	[IgnoreCopyValidation]
	public SerializableDictionary<MoveLabel, int> moveUses = new SerializableDictionary<MoveLabel, int>(16, default(MoveLabelComparer));

	// Token: 0x04001AF0 RID: 6896
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public LandingOverrideData landingOverrideData;

	// Token: 0x04001AF1 RID: 6897
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public HelplessStateData helplessStateData;

	// Token: 0x04001AF2 RID: 6898
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public MoveData bufferMoveData;

	// Token: 0x04001AF3 RID: 6899
	[IgnoreRollback(IgnoreRollbackType.Debug)]
	[NonSerialized]
	public InterruptData bufferInterruptData;

	// Token: 0x04001AF4 RID: 6900
	[IsClonedManually]
	[IgnoreCopyValidation]
	public GrabData grabData = new GrabData();

	// Token: 0x04001AF5 RID: 6901
	[IsClonedManually]
	[IgnoreCopyValidation]
	public BufferedPlayerInput bufferedInput = new BufferedPlayerInput();

	// Token: 0x04001AF6 RID: 6902
	public Fixed damage;

	// Token: 0x04001AF7 RID: 6903
	public int stunFrames;

	// Token: 0x04001AF8 RID: 6904
	public int bufferActivatedFrame;

	// Token: 0x04001AF9 RID: 6905
	public ButtonPress bufferActivatedButton;

	// Token: 0x04001AFA RID: 6906
	public int knockbackIteration;

	// Token: 0x04001AFB RID: 6907
	public bool clearInputBufferOnStunEnd;

	// Token: 0x04001AFC RID: 6908
	public int smokeTrailFrames;

	// Token: 0x04001AFD RID: 6909
	public StunType stunType;

	// Token: 0x04001AFE RID: 6910
	public StunTechMode stunTechMode;

	// Token: 0x04001AFF RID: 6911
	public bool previousFrameBufferTap;

	// Token: 0x04001B00 RID: 6912
	public bool stunIsSpike;

	// Token: 0x04001B01 RID: 6913
	public bool untechableBounceUsed;

	// Token: 0x04001B02 RID: 6914
	public bool floatyNameStun;

	// Token: 0x04001B03 RID: 6915
	public int jumpStunFrames;

	// Token: 0x04001B04 RID: 6916
	public int hitLagFrames;

	// Token: 0x04001B05 RID: 6917
	public int ignoreHitLagFrames;

	// Token: 0x04001B06 RID: 6918
	public int chainGrabPreventionFrames;

	// Token: 0x04001B07 RID: 6919
	public int tumbleWiggleBreakInputCount;

	// Token: 0x04001B08 RID: 6920
	public ButtonPress tumbleWiggleLastButton = ButtonPress.None;

	// Token: 0x04001B09 RID: 6921
	public bool isClankLag;

	// Token: 0x04001B0A RID: 6922
	public bool isKillCamHitlag;

	// Token: 0x04001B0B RID: 6923
	public bool isCameraZoomHitLag;

	// Token: 0x04001B0C RID: 6924
	public bool isFlourishOwner;

	// Token: 0x04001B0D RID: 6925
	public HorizontalDirection facing;

	// Token: 0x04001B0E RID: 6926
	public MetaState state;

	// Token: 0x04001B0F RID: 6927
	public SubStates subState;

	// Token: 0x04001B10 RID: 6928
	public int lastTechFrame;

	// Token: 0x04001B11 RID: 6929
	public bool isActive = true;

	// Token: 0x04001B12 RID: 6930
	public ActionState actionState = ActionState.None;

	// Token: 0x04001B13 RID: 6931
	public int actionStateFrame;

	// Token: 0x04001B14 RID: 6932
	public int jumpBeginFrame;

	// Token: 0x04001B15 RID: 6933
	public int overrideActionStateInterruptibilityFrames;

	// Token: 0x04001B16 RID: 6934
	public ButtonPress jumpButtonSource;

	// Token: 0x04001B17 RID: 6935
	public int teamPowerMoveCooldownFrames;

	// Token: 0x04001B18 RID: 6936
	public bool assistAbsorbsHits;

	// Token: 0x04001B19 RID: 6937
	public int temporaryDurationTotalFrames = -1;

	// Token: 0x04001B1A RID: 6938
	public int temporaryDurationFrames = -1;

	// Token: 0x04001B1B RID: 6939
	public int temporaryAssistImmunityFrames = -1;

	// Token: 0x04001B1C RID: 6940
	public int blastZoneImmunityFrames;

	// Token: 0x04001B1D RID: 6941
	public int userIdleFrames;

	// Token: 0x04001B1E RID: 6942
	public string lastMoveName;

	// Token: 0x04001B1F RID: 6943
	public int repeatTrackMoveCount;

	// Token: 0x04001B20 RID: 6944
	public bool isDead;

	// Token: 0x04001B21 RID: 6945
	public int isDeadForFrames;

	// Token: 0x04001B22 RID: 6946
	public bool isRespawning;

	// Token: 0x04001B23 RID: 6947
	public Vector3F rotation;

	// Token: 0x04001B24 RID: 6948
	public int fallThroughPlatformHeldFrames;

	// Token: 0x04001B25 RID: 6949
	public bool landedWithAirDodge;

	// Token: 0x04001B26 RID: 6950
	public HorizontalDirection lastTapDirection;

	// Token: 0x04001B27 RID: 6951
	public int lastConsumedTapInput;

	// Token: 0x04001B28 RID: 6952
	public int lastBackTapFrame;

	// Token: 0x04001B29 RID: 6953
	public int buttonsPressedThisFrame;

	// Token: 0x04001B2A RID: 6954
	public int lastLeftInputFrame;

	// Token: 0x04001B2B RID: 6955
	public int lastRightInputFrame;

	// Token: 0x04001B2C RID: 6956
	public bool isInterruptMoveBuffered;

	// Token: 0x04001B2D RID: 6957
	public ButtonPress bufferButtonUsed;

	// Token: 0x04001B2E RID: 6958
	public int grabbedLedgeIndex = -1;

	// Token: 0x04001B2F RID: 6959
	public int ledgeReleaseFrame;

	// Token: 0x04001B30 RID: 6960
	public int ledgeGrabbedFrame;

	// Token: 0x04001B31 RID: 6961
	public int ledgeLagFrames;

	// Token: 0x04001B32 RID: 6962
	public int ledgeGrabsSinceLanding;

	// Token: 0x04001B33 RID: 6963
	public int ledgeGrabCooldownFrames;

	// Token: 0x04001B34 RID: 6964
	public int downedFrames;

	// Token: 0x04001B35 RID: 6965
	public int dashDanceFrames;

	// Token: 0x04001B36 RID: 6966
	public bool processingBufferedInput;

	// Token: 0x04001B37 RID: 6967
	public int hitOwnerID;

	// Token: 0x04001B38 RID: 6968
	public bool ignoreNextHelplessness;

	// Token: 0x04001B39 RID: 6969
	public bool queuedWavedashDodge;

	// Token: 0x04001B3A RID: 6970
	public PlayerNum lastHitByPlayerNum;

	// Token: 0x04001B3B RID: 6971
	public TeamNum lastHitByTeamNum;

	// Token: 0x04001B3C RID: 6972
	public int lastHitFrame;

	// Token: 0x04001B3D RID: 6973
	public int emoteCooldownFrames;

	// Token: 0x04001B3E RID: 6974
	public int emoteFrameLimitStart;

	// Token: 0x04001B3F RID: 6975
	public int emoteFrameLimitCounter;

	// Token: 0x04001B40 RID: 6976
	public bool wasFastFallVelocity;
}
