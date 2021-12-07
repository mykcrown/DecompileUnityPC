using System;
using System.Collections;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class AnimationController : IAnimationPlayer, IRollbackStateOwner, IAnimatorDataOwner
{
	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06001F28 RID: 7976 RVA: 0x0009F7CE File Offset: 0x0009DBCE
	// (set) Token: 0x06001F29 RID: 7977 RVA: 0x0009F7D6 File Offset: 0x0009DBD6
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x0009F7DF File Offset: 0x0009DBDF
	// (set) Token: 0x06001F2B RID: 7979 RVA: 0x0009F7E7 File Offset: 0x0009DBE7
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06001F2C RID: 7980 RVA: 0x0009F7F0 File Offset: 0x0009DBF0
	// (set) Token: 0x06001F2D RID: 7981 RVA: 0x0009F7F8 File Offset: 0x0009DBF8
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06001F2E RID: 7982 RVA: 0x0009F801 File Offset: 0x0009DC01
	Dictionary<string, AnimationTimeline> IAnimationPlayer.AnimationTimelineMap
	{
		get
		{
			return this.animationTimelineMap;
		}
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06001F2F RID: 7983 RVA: 0x0009F809 File Offset: 0x0009DC09
	private AnimationConfig data
	{
		get
		{
			return this.configData.animationConfig;
		}
	}

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06001F30 RID: 7984 RVA: 0x0009F816 File Offset: 0x0009DC16
	public Dictionary<string, bool> AnimationReversesFacingMap
	{
		get
		{
			return this.animationReversesFacingMap;
		}
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x0009F820 File Offset: 0x0009DC20
	public void Init(AvatarData avatarData, GameObject gameObject, ConfigData config, bool mirror, bool overrideBlendingEnabled = false, bool replaceAnimator = true)
	{
		this.configData = config;
		this.overrideEnableBlending = overrideBlendingEnabled;
		this.state = new AnimationControllerState();
		GameObject gameObject2 = gameObject;
		Animator animator = gameObject.GetComponentInChildren<Animator>();
		if (animator != null)
		{
			gameObject2 = animator.gameObject;
		}
		if (replaceAnimator)
		{
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponentInChildren<Animator>());
			Animator animator2 = gameObject2.AddComponent<Animator>();
			animator2.avatar = avatarData.humanoidAvatar;
			animator = animator2;
		}
		else
		{
			animator.avatar = avatarData.humanoidAvatar;
		}
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponentInChildren<MecanimControl>());
		this.mecanimControl = gameObject2.AddComponent<MecanimControl>();
		if (this.injector != null)
		{
			this.injector.Inject(this.mecanimControl);
		}
		this.mecanimControl.avatarController = new AvatarController(animator, avatarData);
		this.mecanimControl.Initialize();
		this.mecanimControl.defaultTransitionDuration = 0;
		this.mecanimControl.defaultWrapMode = WrapMode.Once;
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x0009F908 File Offset: 0x0009DD08
	void IAnimationPlayer.ChangedAnimationDuringRollback()
	{
		this.changedAnimationDuringRollback = true;
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x0009F914 File Offset: 0x0009DD14
	void IAnimationPlayer.LoadSharedAnimations(CharacterMoveSetData moveSet, CharacterData owner)
	{
		for (int i = 0; i < moveSet.moves.Length; i++)
		{
			MoveData moveData = moveSet.moves[i];
			if (moveData != null && moveData.opponentAnimationClip != null)
			{
				AnimationTimeline timeline = new AnimationTimeline(1, (int)(moveData.opponentAnimationClip.length * WTime.fps), null, 0);
				this.initAnimation(moveData.opponentAnimationClip, moveData.GetOpponentAnimationClipName(owner), timeline, WrapMode.Once, 0f, ActionState.None, false, false);
			}
		}
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x0009F9A0 File Offset: 0x0009DDA0
	public bool LoadMove(MoveData move, bool replace = false)
	{
		if (move == null)
		{
			Debug.LogWarning("Null move found");
			return false;
		}
		AnimationTimeline timeline = AnimationUtils.CreateMoveTimeline(move, this.configData);
		bool reverseFacing = move.reverseFacing && !move.deferFacingChanges;
		if (move.animationClip != null)
		{
			this.initAnimation(move.animationClip, move.name, timeline, move.wrapMode, 0f, ActionState.None, replace, reverseFacing);
		}
		if (move.animationClipLeft != null)
		{
			this.initAnimation(move.animationClipLeft, move.leftClipName, timeline, move.wrapMode, 0f, ActionState.None, replace, reverseFacing);
		}
		if (move.CancelOnLand)
		{
			float b = (float)move.cancelOptions.landLagVisualFrames / (float)Constants.FPS;
			if (move.cancelOptions.cancelOnLand)
			{
				if (move.cancelOptions.landCancelClip != null)
				{
					float num = move.cancelOptions.landCancelClip.length / Mathf.Max(0.01f, b);
					AnimationTimeline timeline2 = new AnimationTimeline((Fixed)((double)num), move.cancelOptions.landLagVisualFrames, null, 0);
					this.initAnimation(move.cancelOptions.landCancelClip, MoveCancelData.GetClipName(move, false), timeline2, WrapMode.Once, move.cancelOptions.landCancelClip.length, ActionState.Landing, false, false);
				}
				if (move.cancelOptions.leftLandCancelClip != null)
				{
					float num2 = move.cancelOptions.leftLandCancelClip.length / Mathf.Max(0.01f, b);
					AnimationTimeline timeline3 = new AnimationTimeline((Fixed)((double)num2), move.cancelOptions.landLagVisualFrames, null, 0);
					this.initAnimation(move.cancelOptions.leftLandCancelClip, MoveCancelData.GetClipName(move, true), timeline3, WrapMode.Once, move.cancelOptions.leftLandCancelClip.length, ActionState.Landing, false, false);
				}
			}
			for (int i = 0; i < move.interrupts.Length; i++)
			{
				InterruptData interruptData = move.interrupts[i];
				if (interruptData.triggerType == InterruptTriggerType.OnLand)
				{
					if (interruptData.landCancelClip != null)
					{
						float num3 = interruptData.landCancelClip.length / Mathf.Max(0.01f, b);
						AnimationTimeline timeline4 = new AnimationTimeline((Fixed)((double)num3), interruptData.landLagFrames, null, 0);
						this.initAnimation(interruptData.landCancelClip, MoveCancelData.GetClipName(move, false), timeline4, WrapMode.Once, interruptData.landCancelClip.length, ActionState.Landing, false, false);
					}
					if (interruptData.leftLandCancelClip != null)
					{
						float num4 = interruptData.landCancelClip.length / Mathf.Max(0.01f, b);
						AnimationTimeline timeline5 = new AnimationTimeline((Fixed)((double)num4), interruptData.landLagFrames, null, 0);
						this.initAnimation(interruptData.leftLandCancelClip, MoveCancelData.GetClipName(move, true), timeline5, WrapMode.Once, interruptData.leftLandCancelClip.length, ActionState.Landing, false, false);
					}
				}
			}
		}
		if (move.makeHelpless && move.helplessLandingData != null)
		{
			float b2 = (float)move.helplessLandingData.heavyLandFrames / (float)Constants.FPS;
			if (move.helplessLandingData.landClip != null)
			{
				if (move.helplessLandingData.landClipName == string.Empty || move.helplessLandingData.landClipName == null)
				{
					Debug.LogWarning("No land clip name for move " + move.moveName);
				}
				float num5 = move.helplessLandingData.landClip.length / Mathf.Max(0.01f, b2);
				AnimationTimeline timeline6 = new AnimationTimeline((Fixed)((double)num5), move.helplessLandingData.heavyLandFrames, null, 0);
				this.initAnimation(move.helplessLandingData.landClip, move.helplessLandingData.landClipName, timeline6, WrapMode.Once, move.helplessLandingData.landClip.length, ActionState.Landing, replace, false);
			}
			if (move.helplessLandingData.leftLandClip != null)
			{
				if (move.helplessLandingData.leftLandClipName == string.Empty || move.helplessLandingData.leftLandClipName == null)
				{
					Debug.LogWarning("No left land clip name for move " + move.moveName);
				}
				float num6 = move.helplessLandingData.leftLandClip.length / Mathf.Max(0.01f, b2);
				AnimationTimeline timeline7 = new AnimationTimeline((Fixed)((double)num6), move.helplessLandingData.heavyLandFrames, null, 0);
				this.initAnimation(move.helplessLandingData.leftLandClip, move.helplessLandingData.leftLandClipName, timeline7, WrapMode.Once, move.helplessLandingData.leftLandClip.length, ActionState.Landing, replace, false);
			}
		}
		if (move.makeHelpless && move.helplessStateData != null)
		{
			if (move.helplessStateData.animationClip != null)
			{
				int animFrames = (int)((Fixed)((double)move.helplessStateData.animationClip.length) * WTime.fps);
				AnimationTimeline timeline8 = new AnimationTimeline(1, animFrames, null, 0);
				this.initAnimation(move.helplessStateData.animationClip, move.helplessStateData.GetClipName(move, false), timeline8, WrapMode.Once, move.helplessStateData.animationClip.length, ActionState.FallHelpless, replace, false);
			}
			if (move.helplessStateData.leftAnimationClip != null)
			{
				int animFrames2 = (int)((Fixed)((double)move.helplessStateData.leftAnimationClip.length) * WTime.fps);
				AnimationTimeline timeline9 = new AnimationTimeline(1, animFrames2, null, 0);
				this.initAnimation(move.helplessStateData.leftAnimationClip, move.helplessStateData.GetClipName(move, true), timeline9, WrapMode.Once, move.helplessStateData.leftAnimationClip.length, ActionState.FallHelpless, replace, false);
			}
		}
		foreach (MoveComponent moveComponent in move.components)
		{
			if (moveComponent is IMoveOverrideAnimComponent)
			{
				IMoveOverrideAnimComponent moveOverrideAnimComponent = moveComponent as IMoveOverrideAnimComponent;
				AnimationClip[] animationClips = moveOverrideAnimComponent.GetAnimationClips();
				for (int k = 0; k < animationClips.Length; k++)
				{
					this.initAnimation(animationClips[k], move.name + moveOverrideAnimComponent.GetAnimationSuffix(k), timeline, move.wrapMode, 0f, ActionState.None, replace, false);
				}
			}
		}
		return true;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x0009FFE4 File Offset: 0x0009E3E4
	void IAnimationPlayer.LoadAnimation(WavedashAnimationData animationData)
	{
		AnimationTimeline timeline = AnimationUtils.CreateBasicAnimationTimeline(animationData);
		this.initAnimation(animationData.animationClip, animationData.animationId, timeline, WrapMode.Once, animationData.animationClip.length, ActionState.None, false, false);
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x000A001B File Offset: 0x0009E41B
	bool IAnimationPlayer.HasAnimation(string animName)
	{
		return this.animationActions.ContainsKey(animName);
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x000A002C File Offset: 0x0009E42C
	bool IAnimationPlayer.LoadMoveSet(CharacterMoveSetData moveSet, bool returnOnNullMove)
	{
		CharacterActionData action = moveSet.characterActions.GetAction(ActionState.Idle, false);
		this.mecanimControl.SetDefaultClip(action.animation, "default", (Fixed)((double)action.animationSpeed), WrapMode.Loop, false);
		int num = 0;
		foreach (MoveData moveData in moveSet.moves)
		{
			if (moveData == null)
			{
				Debug.LogWarning("Null move found: " + num);
				if (returnOnNullMove)
				{
					return false;
				}
			}
			if (!this.LoadMove(moveData, false) && returnOnNullMove)
			{
				return false;
			}
			num++;
		}
		IEnumerator enumerator = ((IEnumerable)moveSet.characterActions).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				CharacterActionData characterActionData = (CharacterActionData)obj;
				if (!(characterActionData.animation == null))
				{
					AnimationTimeline animationTimeline = AnimationUtils.CreateCharacterActionTimeline(characterActionData, this.configData);
					bool flag = characterActionData.characterActionState == ActionState.Pivot || characterActionData.characterActionState == ActionState.DashPivot || characterActionData.characterActionState == ActionState.RunPivot;
					AnimationClip clip = characterActionData.animation;
					string animName = characterActionData.name;
					AnimationTimeline timeline = animationTimeline;
					WrapMode wrapMode = characterActionData.wrapMode;
					float length = characterActionData.animation.length;
					ActionState characterActionState = characterActionData.characterActionState;
					bool reverseFacing = flag;
					this.initAnimation(clip, animName, timeline, wrapMode, length, characterActionState, false, reverseFacing);
					if (characterActionData.leftAnimation != null)
					{
						clip = characterActionData.leftAnimation;
						animName = characterActionData.LeftAnimationName;
						timeline = animationTimeline;
						wrapMode = characterActionData.wrapMode;
						length = characterActionData.leftAnimation.length;
						characterActionState = characterActionData.characterActionState;
						reverseFacing = flag;
						this.initAnimation(clip, animName, timeline, wrapMode, length, characterActionState, false, reverseFacing);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return true;
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x000A0240 File Offset: 0x0009E640
	bool IAnimationPlayer.LoadCharacterComponentAnimations(CharacterMoveSetData moveSet, IEnumerable<ICharacterComponent> components)
	{
		foreach (ICharacterComponent characterComponent in components)
		{
			CharacterComponent characterComponent2 = (CharacterComponent)characterComponent;
			if (characterComponent2 is ICharacterAnimationComponent)
			{
				ICharacterAnimationComponent characterAnimationComponent = characterComponent2 as ICharacterAnimationComponent;
				CharacterAnimation[] characterAnimations = characterAnimationComponent.GetCharacterAnimations();
				foreach (CharacterAnimation characterAnimation in characterAnimations)
				{
					if (!(characterAnimation.animation == null))
					{
						ActionState actionState = ActionState.None;
						WrapMode wrapMode;
						AnimationTimeline timeline;
						if (characterAnimation is ActionStateAnimationOverride)
						{
							actionState = (characterAnimation as ActionStateAnimationOverride).actionState;
							CharacterActionData action = moveSet.characterActions.GetAction(actionState, false);
							wrapMode = action.wrapMode;
							timeline = AnimationUtils.CreateCharacterActionTimeline(action, this.configData);
							characterAnimation.SetAnimationNames(action.name, action.LeftAnimationName, characterAnimationComponent.AnimationSuffix);
						}
						else
						{
							timeline = AnimationUtils.CreateCharacterAnimationTimeline(characterAnimation);
							wrapMode = characterAnimation.wrapMode;
						}
						this.initAnimation(characterAnimation.animation, characterAnimation.AnimationName, timeline, wrapMode, characterAnimation.animation.length, actionState, false, false);
						if (characterAnimation.leftAnimation != null)
						{
							this.initAnimation(characterAnimation.leftAnimation, characterAnimation.LeftAnimationName, timeline, wrapMode, characterAnimation.leftAnimation.length, actionState, false, false);
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x000A03CC File Offset: 0x0009E7CC
	private void initAnimation(AnimationClip clip, string animName, AnimationTimeline timeline, WrapMode wrapMode, float length = 0f, ActionState actionState = ActionState.None, bool replace = false, bool reverseFacing = false)
	{
		if (length == 0f)
		{
			length = clip.length;
		}
		if (replace)
		{
			this.animationActions.Remove(animName);
			this.animationTimelineMap.Remove(animName);
		}
		if (this.animationActions.ContainsKey(animName))
		{
			return;
		}
		this.animationActions.Add(animName, actionState);
		this.mecanimControl.AddClip(clip, animName, timeline.baseSpeed, wrapMode, (Fixed)((double)length), replace);
		this.animationTimelineMap.Add(animName, timeline);
		this.animationReversesFacingMap.Add(animName, reverseFacing);
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x000A0468 File Offset: 0x0009E868
	public void ForceSyncAnimation()
	{
		this.forceSyncAnimation = true;
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x000A0474 File Offset: 0x0009E874
	public void Update(int gameFrame)
	{
		if (this.CurrentAnimationData == null)
		{
			return;
		}
		if ((this.changedAnimationDuringRollback && !GameClient.IsRollingBack && GameClient.IsCurrentFrame) || this.forceSyncAnimation)
		{
			this.changedAnimationDuringRollback = false;
			this.forceSyncAnimation = false;
			this.mecanimControl.SyncAnimation();
		}
		if (this.state.paused)
		{
			return;
		}
		int trueFramelength = this.animationTimelineMap[this.CurrentAnimationData.clipName].trueFramelength;
		int num = FixedMath.Floor(gameFrame / trueFramelength);
		if (this.CurrentAnimationData.wrapMode == WrapMode.ClampForever && num > 0)
		{
			this.state.currentGameFrame = trueFramelength - 1;
		}
		else
		{
			this.state.currentGameFrame = gameFrame - num * trueFramelength;
		}
		Fixed animationTimeFromGameFrame = this.getAnimationTimeFromGameFrame(this.CurrentAnimationData.clipName, this.state.currentGameFrame);
		this.mecanimControl.UpdateAnimation(animationTimeFromGameFrame);
		this.updateHiddenProps();
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x000A0575 File Offset: 0x0009E975
	void IAnimationPlayer.Test(string current, string previous)
	{
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x000A0577 File Offset: 0x0009E977
	public bool IsAnimationPlaying(string animName)
	{
		return this.mecanimControl.IsPlaying(animName, 0f);
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x000A058A File Offset: 0x0009E98A
	bool IAnimationPlayer.PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, bool disableBlending)
	{
		return this.PlayCharacterAction(characterAction, animationName, mirror, 1, disableBlending);
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x000A05A0 File Offset: 0x0009E9A0
	public bool PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, Fixed overrideSpeed, bool disableBlending)
	{
		if (characterAction == null)
		{
			return false;
		}
		if (this.IsAnimationPlaying(animationName))
		{
			return true;
		}
		if (this.animationActions.ContainsKey(animationName))
		{
			this.state.actionState = this.animationActions[animationName];
		}
		else
		{
			Debug.LogWarning("Failed to find mapped action for " + animationName);
		}
		float blendDuration = -1f;
		float blendOutDuration = -1f;
		if (disableBlending)
		{
			blendOutDuration = (blendDuration = 0f);
		}
		else
		{
			if (characterAction.overrideBlendIn)
			{
				blendDuration = characterAction.blendInDuration;
			}
			if (characterAction.overrideBlendOut)
			{
				blendOutDuration = characterAction.blendOutDuration;
			}
		}
		return this.PlayAnimation(animationName, mirror, 0, overrideSpeed, blendDuration, blendOutDuration);
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x000A0651 File Offset: 0x0009EA51
	public bool PlayThrow(string animName, bool mirror)
	{
		this.state.actionState = ActionState.Thrown;
		return this.PlayAnimation(animName, mirror, 0, 0, 0f, -1f);
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x000A0679 File Offset: 0x0009EA79
	public bool PlayAnimation(string animName, bool mirror, int gameFrame, float blendDuration = -1f, float blendOutDuration = -1f)
	{
		return this.PlayAnimation(animName, mirror, gameFrame, 1, blendDuration, blendOutDuration);
	}

	// Token: 0x06001F42 RID: 8002 RVA: 0x000A0690 File Offset: 0x0009EA90
	public bool PlayAnimation(string animName, bool mirror, int gameFrame, Fixed overrideSpeed, float blendDuration = -1f, float blendOutDuration = -1f)
	{
		if (this.data.blendingEnabled || this.overrideEnableBlending)
		{
			if (this.state.blendOutDuration > -1f && blendDuration == -1f)
			{
				blendDuration = this.state.blendOutDuration;
			}
			this.state.blendOutDuration = blendOutDuration;
			if (blendDuration == -1f)
			{
				blendDuration = (float)this.data.defaultBlendDuration;
			}
		}
		else
		{
			blendDuration = 0f;
		}
		AnimationData animationData = this.mecanimControl.GetAnimationData(animName);
		if (animationData == null)
		{
			Debug.LogWarning("Failed to find animation data for animation " + animName);
			return false;
		}
		Fixed animationTimeFromGameFrame = this.getAnimationTimeFromGameFrame(animName, gameFrame);
		this.state.currentGameFrame = gameFrame;
		if (GameClient.IsRollingBack)
		{
			this.changedAnimationDuringRollback = true;
		}
		this.mecanimControl.Play(animationData, (Fixed)((double)blendDuration), animationTimeFromGameFrame * overrideSpeed, mirror);
		this.state.overrideSpeed = overrideSpeed;
		this.updateHiddenProps();
		return true;
	}

	// Token: 0x06001F43 RID: 8003 RVA: 0x000A079C File Offset: 0x0009EB9C
	private void updateHiddenProps()
	{
		if (this.CurrentAnimationData.clip.isHumanMotion && this.hiddenProps != null)
		{
			foreach (Transform transform in this.hiddenProps)
			{
				transform.localScale = Vector3.one * 1E-12f;
			}
		}
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x000A0828 File Offset: 0x0009EC28
	private Fixed getAnimationTimeFromFrame(string animationName, Fixed gameFrame)
	{
		int num = FixedMath.Floor(gameFrame);
		int num2 = FixedMath.Ceil(gameFrame);
		if (num == num2)
		{
			return this.getAnimationTimeFromGameFrame(animationName, num);
		}
		if (!this.animationTimelineMap.ContainsKey(animationName))
		{
			Debug.LogError("No timeline data found for " + animationName);
			return -1;
		}
		Fixed @fixed = this.animationTimelineMap[animationName].gameFrameToAnimFrameMap[num];
		Fixed one = this.animationTimelineMap[animationName].gameFrameToAnimFrameMap[num2];
		Fixed other = gameFrame - num;
		Fixed one2 = @fixed + (one - @fixed) * other;
		return one2 * this.animationTimelineMap[animationName].baseSpeed;
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x000A08E4 File Offset: 0x0009ECE4
	private Fixed getAnimationTimeFromGameFrame(string animationName, int gameFrame)
	{
		if (!this.animationTimelineMap.ContainsKey(animationName))
		{
			Debug.LogError("No timeline data found for " + animationName);
			return -1;
		}
		if (!this.animationTimelineMap[animationName].gameFrameToAnimFrameMap.ContainsKey(gameFrame))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Frame ",
				gameFrame,
				" not found for animation ",
				animationName
			}));
			return -1;
		}
		return this.animationTimelineMap[animationName].gameFrameToAnimFrameMap[gameFrame] * WTime.fixedDeltaTime * this.animationTimelineMap[animationName].baseSpeed;
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x000A09A0 File Offset: 0x0009EDA0
	void IAnimationPlayer.SetPause(bool isPaused)
	{
		this.state.paused = isPaused;
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x000A09AE File Offset: 0x0009EDAE
	void IAnimationPlayer.SetMecanimMirror(bool mirror)
	{
		this.mecanimControl.SetMirror(mirror, 0, true);
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x000A09C3 File Offset: 0x0009EDC3
	float IAnimationPlayer.GetAnimationLength(string animationName)
	{
		return this.mecanimControl.GetAnimationData(animationName).clip.length;
	}

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06001F49 RID: 8009 RVA: 0x000A09DB File Offset: 0x0009EDDB
	public AnimationData CurrentAnimationData
	{
		get
		{
			return this.mecanimControl.GetCurrentAnimationData();
		}
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x000A09E8 File Offset: 0x0009EDE8
	public int GetNextGameFrameFromAnimationFrame(string animName, Fixed internalFrame)
	{
		for (int i = 0; i < this.animationTimelineMap[animName].trueFramelength; i++)
		{
			if (this.animationTimelineMap[animName].gameFrameToAnimFrameMap[i] > internalFrame)
			{
				return i;
			}
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Failed to find gameFrame for anim ",
			animName,
			" internalFrame ",
			internalFrame
		}));
		return -1;
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x000A0A6C File Offset: 0x0009EE6C
	public Fixed GetAnimationFrameFromGameFrame(string animName, int gameFrame)
	{
		if (!this.animationTimelineMap.ContainsKey(animName))
		{
			Debug.LogError("Missing timeline data for " + animName);
			return 0;
		}
		if (!this.animationTimelineMap[animName].gameFrameToAnimFrameMap.ContainsKey(gameFrame))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Missing timeline frame data for ",
				animName,
				" frame ",
				gameFrame
			}));
			return 0;
		}
		return this.animationTimelineMap[animName].gameFrameToAnimFrameMap[gameFrame];
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000A0B08 File Offset: 0x0009EF08
	public int CurrentAnimationGameFramelength
	{
		get
		{
			return this.animationTimelineMap[this.CurrentAnimationData.clipName].trueFramelength;
		}
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x000A0B28 File Offset: 0x0009EF28
	int IAnimationPlayer.GetActionGameFramelength(CharacterActionData action)
	{
		if (!this.animationTimelineMap.ContainsKey(action.name))
		{
			Debug.LogError("Couldn't find timeline for action " + action.characterActionState);
		}
		return this.animationTimelineMap[action.name].trueFramelength;
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000A0B7B File Offset: 0x0009EF7B
	public Fixed CurrentSpeed
	{
		get
		{
			return this.state.overrideSpeed;
		}
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x000A0B88 File Offset: 0x0009EF88
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<AnimationControllerState>(ref this.state);
		string currentClipName = this.mecanimControl.GetCurrentClipName();
		bool result = this.mecanimControl.LoadState(container);
		if (this.mecanimControl.GetCurrentClipName() != currentClipName)
		{
			this.changedAnimationDuringRollback = true;
		}
		return result;
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000A0BD9 File Offset: 0x0009EFD9
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<AnimationControllerState>(this.state));
		return this.mecanimControl.ExportState(ref container);
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06001F51 RID: 8017 RVA: 0x000A0C00 File Offset: 0x0009F000
	public ActionState CurrentActionState
	{
		get
		{
			return this.state.actionState;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000A0C0D File Offset: 0x0009F00D
	public int CurrentFrame
	{
		get
		{
			return this.state.currentGameFrame;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000A0C1A File Offset: 0x0009F01A
	public string CurrentAnimationName
	{
		get
		{
			return (this.CurrentAnimationData != null) ? this.CurrentAnimationData.clipName : null;
		}
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x000A0C38 File Offset: 0x0009F038
	public void SetHiddenProps(List<Transform> hiddenProps)
	{
		this.hiddenProps = hiddenProps;
	}

	// Token: 0x040018DC RID: 6364
	private MecanimControl mecanimControl;

	// Token: 0x040018DD RID: 6365
	private AnimationControllerState state;

	// Token: 0x040018DE RID: 6366
	private ConfigData configData;

	// Token: 0x040018DF RID: 6367
	private bool changedAnimationDuringRollback;

	// Token: 0x040018E0 RID: 6368
	private bool forceSyncAnimation;

	// Token: 0x040018E1 RID: 6369
	public bool overrideEnableBlending;

	// Token: 0x040018E2 RID: 6370
	private Dictionary<string, AnimationTimeline> animationTimelineMap = new Dictionary<string, AnimationTimeline>();

	// Token: 0x040018E3 RID: 6371
	private Dictionary<string, ActionState> animationActions = new Dictionary<string, ActionState>();

	// Token: 0x040018E4 RID: 6372
	private Dictionary<string, bool> animationReversesFacingMap = new Dictionary<string, bool>();

	// Token: 0x040018E5 RID: 6373
	public List<Transform> hiddenProps;
}
