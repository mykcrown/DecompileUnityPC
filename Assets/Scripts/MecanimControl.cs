// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MecanimControl : MonoBehaviour, IRollbackStateOwner
{
	public delegate void AnimEvent(AnimationData animationData);

	public AnimationData defaultAnimation = new AnimationData();

	public AnimationData[] animations = new AnimationData[0];

	public bool debugMode;

	public bool alwaysPlay;

	public bool overrideRootMotion;

	public Fixed defaultTransitionDuration = (Fixed)0.15;

	public WrapMode defaultWrapMode = WrapMode.Loop;

	private Animator animator;

	public AvatarController avatarController;

	private RuntimeAnimatorController controller1;

	private RuntimeAnimatorController controller2;

	private RuntimeAnimatorController controller3;

	private RuntimeAnimatorController controller4;

	private MecanimControlState state;

	private Fixed lastVisibleAnimationTime = 0;

	private bool animatorMirror;

	private List<AnimationClip> instantiatedClips = new List<AnimationClip>();

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	private AnimationData currentAnimationData
	{
		get
		{
			return this.state.currentAnimation;
		}
		set
		{
			this.state.currentAnimation = value;
		}
	}

	private bool currentMirror
	{
		get
		{
			return this.state.currentMirror;
		}
		set
		{
			this.state.currentMirror = value;
			if (!GameClient.IsRollingBack)
			{
				this.animatorMirror = value;
			}
		}
	}

	public void AddClip(AnimationClip clip, string newName, Fixed speed, WrapMode wrapMode, Fixed length, bool replace = false)
	{
		if (this.GetAnimationData(newName) != null && !replace)
		{
			UnityEngine.Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
		}
		AnimationData animationData = new AnimationData();
		animationData.clip = UnityEngine.Object.Instantiate<AnimationClip>(clip);
		this.instantiatedClips.Add(animationData.clip);
		if (wrapMode == WrapMode.Default)
		{
			wrapMode = this.defaultWrapMode;
		}
		animationData.clip.wrapMode = wrapMode;
		animationData.clip.name = newName;
		animationData.clipName = newName;
		animationData.speed = speed;
		animationData.originalSpeed = speed;
		animationData.length = length;
		animationData.wrapMode = wrapMode;
		List<AnimationData> list = new List<AnimationData>(this.animations);
		if (replace)
		{
			foreach (AnimationData current in list)
			{
				if (current.clipName == newName)
				{
					list.Remove(current);
					break;
				}
			}
		}
		list.Add(animationData);
		this.animations = list.ToArray();
	}

	public void Initialize()
	{
		this.animator = base.gameObject.GetComponent<Animator>();
		this.controller1 = (RuntimeAnimatorController)Resources.Load("controller1");
		this.controller2 = (RuntimeAnimatorController)Resources.Load("controller2");
		this.controller3 = (RuntimeAnimatorController)Resources.Load("controller3");
		this.controller4 = (RuntimeAnimatorController)Resources.Load("controller4");
		AnimationData[] array = this.animations;
		for (int i = 0; i < array.Length; i++)
		{
			AnimationData animationData = array[i];
			if (animationData.wrapMode == WrapMode.Default)
			{
				animationData.wrapMode = this.defaultWrapMode;
			}
			animationData.clip.wrapMode = animationData.wrapMode;
		}
		this.state = new MecanimControlState();
		this.animator.enabled = false;
	}

	public void UpdateAnimation(Fixed animationTime)
	{
		if (this.currentAnimationData.clip == null)
		{
			return;
		}
		this.currentAnimationData.timeElapsed = animationTime;
		if (GameClient.IsCurrentFrame)
		{
			if (animationTime < this.lastVisibleAnimationTime)
			{
				this.animator.PlayInFixedTime(this.currentAnimationData.stateName, 0, this.currentAnimationData.timeElapsed.ToFloat());
				this.animator.Update(0f);
			}
			else
			{
				float speed = (float)(animationTime - this.lastVisibleAnimationTime);
				this.animator.speed = speed;
				this.animator.Update(1f);
				this.animator.speed = 1f;
			}
			this.lastVisibleAnimationTime = animationTime;
		}
	}

	private void updateAnimationToTime(AnimationData animation, Fixed targetTime)
	{
	}

	public void RemoveClip(string name)
	{
		List<AnimationData> list = new List<AnimationData>(this.animations);
		list.Remove(this.GetAnimationData(name));
		this.animations = list.ToArray();
	}

	public void RemoveClip(AnimationClip clip)
	{
		List<AnimationData> list = new List<AnimationData>(this.animations);
		list.Remove(this.GetAnimationData(clip));
		this.animations = list.ToArray();
	}

	public void SetDefaultClip(AnimationClip clip, string name, Fixed speed, WrapMode wrapMode, bool mirror)
	{
		this.defaultAnimation.clip = UnityEngine.Object.Instantiate<AnimationClip>(clip);
		this.instantiatedClips.Add(this.defaultAnimation.clip);
		this.defaultAnimation.clip.wrapMode = wrapMode;
		this.defaultAnimation.clipName = name;
		this.defaultAnimation.speed = speed;
		this.defaultAnimation.originalSpeed = speed;
		this.defaultAnimation.transitionDuration = -1;
		this.defaultAnimation.wrapMode = wrapMode;
	}

	public void AddClip(AnimationClip clip, string newName)
	{
		this.AddClip(clip, newName, 1, this.defaultWrapMode);
	}

	public void AddClip(AnimationClip clip, string newName, Fixed speed, WrapMode wrapMode)
	{
		if (this.GetAnimationData(newName) != null)
		{
			UnityEngine.Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
		}
		AnimationData animationData = new AnimationData();
		animationData.clip = UnityEngine.Object.Instantiate<AnimationClip>(clip);
		this.instantiatedClips.Add(animationData.clip);
		if (wrapMode == WrapMode.Default)
		{
			wrapMode = this.defaultWrapMode;
		}
		animationData.clip.wrapMode = wrapMode;
		animationData.clip.name = newName;
		animationData.clipName = newName;
		animationData.speed = speed;
		animationData.originalSpeed = speed;
		animationData.length = (Fixed)((double)clip.length);
		animationData.wrapMode = wrapMode;
		this.animations = new List<AnimationData>(this.animations)
		{
			animationData
		}.ToArray();
	}

	public AnimationData GetAnimationData(string clipName)
	{
		AnimationData[] array = this.animations;
		for (int i = 0; i < array.Length; i++)
		{
			AnimationData animationData = array[i];
			if (animationData.clipName == clipName)
			{
				return animationData;
			}
		}
		if (clipName == this.defaultAnimation.clipName)
		{
			return this.defaultAnimation;
		}
		return null;
	}

	public AnimationData GetAnimationData(AnimationClip clip)
	{
		AnimationData[] array = this.animations;
		for (int i = 0; i < array.Length; i++)
		{
			AnimationData animationData = array[i];
			if (animationData.clip == clip)
			{
				return animationData;
			}
		}
		if (clip == this.defaultAnimation.clip)
		{
			return this.defaultAnimation;
		}
		return null;
	}

	public void CrossFade(string clipName, Fixed blendingTime)
	{
		this.CrossFade(clipName, blendingTime, 0, this.currentMirror);
	}

	public void CrossFade(string clipName, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), blendingTime, animationTime, mirror);
	}

	public void CrossFade(AnimationData animationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(animationData, blendingTime, animationTime, mirror);
	}

	public void Play(string clipName, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), blendingTime, animationTime, mirror);
	}

	public void Play(AnimationClip clip, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clip), blendingTime, animationTime, mirror);
	}

	public void Play(string clipName, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), 0, 0, mirror);
	}

	public void Play(string clipName)
	{
		this._playAnimation(this.GetAnimationData(clipName), 0, 0, this.currentMirror);
	}

	public void Play(AnimationClip clip, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clip), 0, 0, mirror);
	}

	public void Play(AnimationClip clip)
	{
		this._playAnimation(this.GetAnimationData(clip), 0, 0, this.currentMirror);
	}

	public void Play(AnimationData animationData, bool mirror)
	{
		this._playAnimation(animationData, animationData.transitionDuration, 0, mirror);
	}

	public void Play(AnimationData animationData)
	{
		this._playAnimation(animationData, animationData.transitionDuration, 0, this.currentMirror);
	}

	public void Play(AnimationData animationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(animationData, blendingTime, animationTime, mirror);
	}

	public void Play()
	{
		this.animator.speed = Mathf.Abs((float)this.currentAnimationData.speed);
	}

	private void _playAnimation(AnimationData targetAnimationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this.setAnimation(targetAnimationData, this.currentAnimationData, blendingTime, animationTime, mirror, true);
	}

	private void setAnimation(AnimationData targetAnimationData, AnimationData previousAnimationData, Fixed blendingTime, Fixed animationTime, bool mirror, bool resetFromBeginning)
	{
		if (targetAnimationData == null || targetAnimationData.clip == null)
		{
			return;
		}
		if (previousAnimationData == targetAnimationData)
		{
			previousAnimationData = this.state.previousAnimation;
		}
		if (resetFromBeginning)
		{
			targetAnimationData.timesPlayed = 0;
		}
		this.currentMirror = mirror;
		this.state.currentBlendTime = blendingTime;
		if (!GameClient.IsRollingBack)
		{
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
			if (mirror)
			{
				if (targetAnimationData.originalSpeed > 0)
				{
					animatorOverrideController.runtimeAnimatorController = this.controller2;
				}
				else
				{
					animatorOverrideController.runtimeAnimatorController = this.controller4;
				}
			}
			else if (targetAnimationData.originalSpeed > 0)
			{
				animatorOverrideController.runtimeAnimatorController = this.controller1;
			}
			else
			{
				animatorOverrideController.runtimeAnimatorController = this.controller3;
			}
			if (previousAnimationData != null)
			{
				animatorOverrideController["State1"] = previousAnimationData.clip;
			}
			animatorOverrideController["State2"] = targetAnimationData.clip;
			this.animator.runtimeAnimatorController = animatorOverrideController;
		}
		if (blendingTime == -1 && previousAnimationData != null)
		{
			blendingTime = previousAnimationData.transitionDuration;
		}
		if (blendingTime == -1)
		{
			blendingTime = this.defaultTransitionDuration;
		}
		this.setAnimationPosition(targetAnimationData, previousAnimationData, animationTime, blendingTime);
		targetAnimationData.timeElapsed = animationTime;
		if (this.overrideRootMotion)
		{
			this.animator.applyRootMotion = targetAnimationData.applyRootMotion;
		}
		this.state.currentAnimation = targetAnimationData;
		this.state.currentAnimation.stateName = "State2";
		this.state.previousAnimation = previousAnimationData;
		if (this.state.previousAnimation != null)
		{
			this.state.previousAnimation.speed = this.state.previousAnimation.originalSpeed;
			this.state.previousAnimation.timesPlayed = 0;
		}
	}

	private void setAnimationPosition(AnimationData targetAnimationData, AnimationData previousAnimationData, Fixed animationTime, Fixed blendingTime)
	{
		targetAnimationData.timeElapsed = 0;
		Fixed @fixed = animationTime;
		Fixed one = targetAnimationData.timesPlayed * targetAnimationData.length + @fixed;
		if (this.avatarController != null)
		{
			bool flag = this.avatarController.SetAppropriateAvatar(targetAnimationData.clip);
			if (flag && blendingTime > 0)
			{
				UnityEngine.Debug.LogWarning("Blending between avatar types is not supported.");
			}
		}
		if (previousAnimationData == null || blendingTime <= 0 || one >= blendingTime)
		{
			if (!GameClient.IsRollingBack)
			{
				this.animator.PlayInFixedTime("State2", 0, @fixed.ToFloat());
				this.animator.Update(0f);
				this.lastVisibleAnimationTime = @fixed;
			}
		}
		else
		{
			previousAnimationData.stateName = "State1";
			if (!GameClient.IsRollingBack)
			{
				this.animator.PlayInFixedTime(previousAnimationData.stateName, 0, previousAnimationData.timeElapsed.ToFloat());
				this.animator.Update(0f);
				this.animator.CrossFadeInFixedTime("State2", (float)(blendingTime * targetAnimationData.speed), 0, 0f);
				this.animator.Update(0f);
				this.animator.speed = animationTime.ToFloat();
				this.animator.Update(1f);
				this.animator.speed = 1f;
				this.lastVisibleAnimationTime = @fixed;
			}
			targetAnimationData.timeElapsed = @fixed;
		}
	}

	public bool IsPlaying(string clipName)
	{
		return this.IsPlaying(this.GetAnimationData(clipName));
	}

	public bool IsPlaying(string clipName, float weight)
	{
		return this.IsPlaying(this.GetAnimationData(clipName), weight);
	}

	public bool IsPlaying(AnimationClip clip)
	{
		return this.IsPlaying(this.GetAnimationData(clip));
	}

	public bool IsPlaying(AnimationClip clip, float weight)
	{
		return this.IsPlaying(this.GetAnimationData(clip), weight);
	}

	public bool IsPlaying(AnimationData animData)
	{
		return this.IsPlaying(animData, 0f);
	}

	public bool IsPlaying(AnimationData animData, float weight)
	{
		return animData != null && this.currentAnimationData != null && (this.currentAnimationData != animData || animData.wrapMode != WrapMode.Once || animData.timesPlayed <= 0) && (this.currentAnimationData == animData || this.currentAnimationData.clip == animData.clip);
	}

	public string GetCurrentClipName()
	{
		return this.currentAnimationData.clipName;
	}

	public AnimationData GetCurrentAnimationData()
	{
		return this.currentAnimationData;
	}

	public int GetCurrentClipPlayCount()
	{
		return this.currentAnimationData.timesPlayed;
	}

	public Fixed GetCurrentClipTime()
	{
		return this.currentAnimationData.timeElapsed;
	}

	public Fixed GetCurrentClipLength()
	{
		return this.currentAnimationData.length;
	}

	public void Stop()
	{
		this.Play(this.defaultAnimation.clip, this.defaultTransitionDuration, 0, this.currentMirror);
	}

	public void SetWrapMode(WrapMode wrapMode)
	{
		this.defaultWrapMode = wrapMode;
	}

	public void SetWrapMode(AnimationData animationData, WrapMode wrapMode)
	{
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(AnimationClip clip, WrapMode wrapMode)
	{
		AnimationData animationData = this.GetAnimationData(clip);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(string clipName, WrapMode wrapMode)
	{
		AnimationData animationData = this.GetAnimationData(clipName);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public float GetSpeed()
	{
		return this.animator.speed;
	}

	public bool GetMirror()
	{
		return this.currentMirror;
	}

	public void SetMirror(bool toggle)
	{
		this.SetMirror(toggle, 0, false);
	}

	public void SetMirror(bool toggle, Fixed blendingTime)
	{
		this.SetMirror(toggle, blendingTime, false);
	}

	public void SetMirror(bool toggle, Fixed blendingTime, bool forceMirror)
	{
		if (this.currentMirror == toggle && !forceMirror)
		{
			return;
		}
		if (blendingTime == 0)
		{
			blendingTime = this.defaultTransitionDuration;
		}
		this._playAnimation(this.currentAnimationData, blendingTime, (this.currentAnimationData != null) ? this.currentAnimationData.timeElapsed : 0, toggle);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<MecanimControlState>(this.state));
		return true;
	}

	private AnimationData ApplyToAnimationData(AnimationData newData)
	{
		AnimationData animationData = this.GetAnimationData(newData.clipName);
		animationData.ReplaceFromRollback(newData);
		return animationData;
	}

	public void SyncAnimation()
	{
		float num = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * this.currentAnimationData.clip.length;
		AnimatorClipInfo[] currentAnimatorClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);
		if ((this.currentAnimationData != null && (currentAnimatorClipInfo.Length == 0 || this.currentAnimationData.clipName != currentAnimatorClipInfo[0].clip.name || !FixedMath.ApproximatelyEqual((Fixed)((double)num), this.state.currentAnimation.timeElapsed))) || this.currentMirror != this.animatorMirror)
		{
			this.setAnimation(this.state.currentAnimation, this.state.previousAnimation, this.state.currentBlendTime, this.state.currentAnimation.timeElapsed, this.state.currentMirror, false);
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<MecanimControlState>(ref this.state);
		if (this.state.currentAnimation != null)
		{
			this.state.currentAnimation = this.ApplyToAnimationData(this.state.currentAnimation);
		}
		if (this.state.previousAnimation != null)
		{
			this.state.previousAnimation = this.ApplyToAnimationData(this.state.previousAnimation);
		}
		this.setAnimation(this.state.currentAnimation, this.state.previousAnimation, this.state.currentBlendTime, this.state.currentAnimation.timeElapsed, this.state.currentMirror, false);
		return true;
	}

	public void OnDestroy()
	{
		foreach (AnimationClip current in this.instantiatedClips)
		{
			UnityEngine.Object.Destroy(current);
		}
	}
}
