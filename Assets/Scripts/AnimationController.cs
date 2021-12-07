// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : IAnimationPlayer, IRollbackStateOwner, IAnimatorDataOwner
{
	private MecanimControl mecanimControl;

	private AnimationControllerState state;

	private ConfigData configData;

	private bool changedAnimationDuringRollback;

	private bool forceSyncAnimation;

	public bool overrideEnableBlending;

	private Dictionary<string, AnimationTimeline> animationTimelineMap = new Dictionary<string, AnimationTimeline>();

	private Dictionary<string, ActionState> animationActions = new Dictionary<string, ActionState>();

	private Dictionary<string, bool> animationReversesFacingMap = new Dictionary<string, bool>();

	public List<Transform> hiddenProps;

	Dictionary<string, AnimationTimeline> IAnimationPlayer.AnimationTimelineMap
	{
		get
		{
			return this.animationTimelineMap;
		}
	}

	[Inject]
	public IDependencyInjection injector
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
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	private AnimationConfig data
	{
		get
		{
			return this.configData.animationConfig;
		}
	}

	public Dictionary<string, bool> AnimationReversesFacingMap
	{
		get
		{
			return this.animationReversesFacingMap;
		}
	}

	public AnimationData CurrentAnimationData
	{
		get
		{
			return this.mecanimControl.GetCurrentAnimationData();
		}
	}

	public int CurrentAnimationGameFramelength
	{
		get
		{
			return this.animationTimelineMap[this.CurrentAnimationData.clipName].trueFramelength;
		}
	}

	public Fixed CurrentSpeed
	{
		get
		{
			return this.state.overrideSpeed;
		}
	}

	public ActionState CurrentActionState
	{
		get
		{
			return this.state.actionState;
		}
	}

	public int CurrentFrame
	{
		get
		{
			return this.state.currentGameFrame;
		}
	}

	public string CurrentAnimationName
	{
		get
		{
			return (this.CurrentAnimationData != null) ? this.CurrentAnimationData.clipName : null;
		}
	}

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

	void IAnimationPlayer.ChangedAnimationDuringRollback()
	{
		this.changedAnimationDuringRollback = true;
	}

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

	public bool LoadMove(MoveData move, bool replace = false)
	{
		if (move == null)
		{
			UnityEngine.Debug.LogWarning("Null move found");
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
					UnityEngine.Debug.LogWarning("No land clip name for move " + move.moveName);
				}
				float num5 = move.helplessLandingData.landClip.length / Mathf.Max(0.01f, b2);
				AnimationTimeline timeline6 = new AnimationTimeline((Fixed)((double)num5), move.helplessLandingData.heavyLandFrames, null, 0);
				this.initAnimation(move.helplessLandingData.landClip, move.helplessLandingData.landClipName, timeline6, WrapMode.Once, move.helplessLandingData.landClip.length, ActionState.Landing, replace, false);
			}
			if (move.helplessLandingData.leftLandClip != null)
			{
				if (move.helplessLandingData.leftLandClipName == string.Empty || move.helplessLandingData.leftLandClipName == null)
				{
					UnityEngine.Debug.LogWarning("No left land clip name for move " + move.moveName);
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
		MoveComponent[] components = move.components;
		for (int j = 0; j < components.Length; j++)
		{
			MoveComponent moveComponent = components[j];
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

	void IAnimationPlayer.LoadAnimation(WavedashAnimationData animationData)
	{
		AnimationTimeline timeline = AnimationUtils.CreateBasicAnimationTimeline(animationData);
		this.initAnimation(animationData.animationClip, animationData.animationId, timeline, WrapMode.Once, animationData.animationClip.length, ActionState.None, false, false);
	}

	bool IAnimationPlayer.HasAnimation(string animName)
	{
		return this.animationActions.ContainsKey(animName);
	}

	bool IAnimationPlayer.LoadMoveSet(CharacterMoveSetData moveSet, bool returnOnNullMove)
	{
		CharacterActionData action = moveSet.characterActions.GetAction(ActionState.Idle, false);
		this.mecanimControl.SetDefaultClip(action.animation, "default", (Fixed)((double)action.animationSpeed), WrapMode.Loop, false);
		int num = 0;
		MoveData[] moves = moveSet.moves;
		for (int i = 0; i < moves.Length; i++)
		{
			MoveData moveData = moves[i];
			if (moveData == null)
			{
				UnityEngine.Debug.LogWarning("Null move found: " + num);
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
				CharacterActionData characterActionData = (CharacterActionData)enumerator.Current;
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

	bool IAnimationPlayer.LoadCharacterComponentAnimations(CharacterMoveSetData moveSet, IEnumerable<ICharacterComponent> components)
	{
		using (IEnumerator<ICharacterComponent> enumerator = components.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharacterComponent characterComponent = (CharacterComponent)enumerator.Current;
				if (characterComponent is ICharacterAnimationComponent)
				{
					ICharacterAnimationComponent characterAnimationComponent = characterComponent as ICharacterAnimationComponent;
					CharacterAnimation[] characterAnimations = characterAnimationComponent.GetCharacterAnimations();
					CharacterAnimation[] array = characterAnimations;
					for (int i = 0; i < array.Length; i++)
					{
						CharacterAnimation characterAnimation = array[i];
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
		}
		return true;
	}

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

	public void ForceSyncAnimation()
	{
		this.forceSyncAnimation = true;
	}

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

	void IAnimationPlayer.Test(string current, string previous)
	{
	}

	public bool IsAnimationPlaying(string animName)
	{
		return this.mecanimControl.IsPlaying(animName, 0f);
	}

	bool IAnimationPlayer.PlayCharacterAction(CharacterActionData characterAction, string animationName, bool mirror, bool disableBlending)
	{
		return this.PlayCharacterAction(characterAction, animationName, mirror, 1, disableBlending);
	}

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
			UnityEngine.Debug.LogWarning("Failed to find mapped action for " + animationName);
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

	public bool PlayThrow(string animName, bool mirror)
	{
		this.state.actionState = ActionState.Thrown;
		return this.PlayAnimation(animName, mirror, 0, 0, 0f, -1f);
	}

	public bool PlayAnimation(string animName, bool mirror, int gameFrame, float blendDuration = -1f, float blendOutDuration = -1f)
	{
		return this.PlayAnimation(animName, mirror, gameFrame, 1, blendDuration, blendOutDuration);
	}

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
			UnityEngine.Debug.LogWarning("Failed to find animation data for animation " + animName);
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

	private void updateHiddenProps()
	{
		if (this.CurrentAnimationData.clip.isHumanMotion && this.hiddenProps != null)
		{
			foreach (Transform current in this.hiddenProps)
			{
				current.localScale = Vector3.one * 1E-12f;
			}
		}
	}

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
			UnityEngine.Debug.LogError("No timeline data found for " + animationName);
			return -1;
		}
		Fixed @fixed = this.animationTimelineMap[animationName].gameFrameToAnimFrameMap[num];
		Fixed one = this.animationTimelineMap[animationName].gameFrameToAnimFrameMap[num2];
		Fixed other = gameFrame - num;
		Fixed one2 = @fixed + (one - @fixed) * other;
		return one2 * this.animationTimelineMap[animationName].baseSpeed;
	}

	private Fixed getAnimationTimeFromGameFrame(string animationName, int gameFrame)
	{
		if (!this.animationTimelineMap.ContainsKey(animationName))
		{
			UnityEngine.Debug.LogError("No timeline data found for " + animationName);
			return -1;
		}
		if (!this.animationTimelineMap[animationName].gameFrameToAnimFrameMap.ContainsKey(gameFrame))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	void IAnimationPlayer.SetPause(bool isPaused)
	{
		this.state.paused = isPaused;
	}

	void IAnimationPlayer.SetMecanimMirror(bool mirror)
	{
		this.mecanimControl.SetMirror(mirror, 0, true);
	}

	float IAnimationPlayer.GetAnimationLength(string animationName)
	{
		return this.mecanimControl.GetAnimationData(animationName).clip.length;
	}

	public int GetNextGameFrameFromAnimationFrame(string animName, Fixed internalFrame)
	{
		for (int i = 0; i < this.animationTimelineMap[animName].trueFramelength; i++)
		{
			if (this.animationTimelineMap[animName].gameFrameToAnimFrameMap[i] > internalFrame)
			{
				return i;
			}
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Failed to find gameFrame for anim ",
			animName,
			" internalFrame ",
			internalFrame
		}));
		return -1;
	}

	public Fixed GetAnimationFrameFromGameFrame(string animName, int gameFrame)
	{
		if (!this.animationTimelineMap.ContainsKey(animName))
		{
			UnityEngine.Debug.LogError("Missing timeline data for " + animName);
			return 0;
		}
		if (!this.animationTimelineMap[animName].gameFrameToAnimFrameMap.ContainsKey(gameFrame))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	int IAnimationPlayer.GetActionGameFramelength(CharacterActionData action)
	{
		if (!this.animationTimelineMap.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Couldn't find timeline for action " + action.characterActionState);
		}
		return this.animationTimelineMap[action.name].trueFramelength;
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<AnimationControllerState>(this.state));
		return this.mecanimControl.ExportState(ref container);
	}

	public void SetHiddenProps(List<Transform> hiddenProps)
	{
		this.hiddenProps = hiddenProps;
	}
}
