using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020001EE RID: 494
[RequireComponent(typeof(Animator))]
public class MecanimControl : MonoBehaviour, IRollbackStateOwner
{
	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060008E1 RID: 2273 RVA: 0x0004E1EC File Offset: 0x0004C5EC
	// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0004E1F4 File Offset: 0x0004C5F4
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x060008E3 RID: 2275 RVA: 0x0004E200 File Offset: 0x0004C600
	public void AddClip(AnimationClip clip, string newName, Fixed speed, WrapMode wrapMode, Fixed length, bool replace = false)
	{
		if (this.GetAnimationData(newName) != null && !replace)
		{
			Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
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
			foreach (AnimationData animationData2 in list)
			{
				if (animationData2.clipName == newName)
				{
					list.Remove(animationData2);
					break;
				}
			}
		}
		list.Add(animationData);
		this.animations = list.ToArray();
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0004E32C File Offset: 0x0004C72C
	// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0004E339 File Offset: 0x0004C739
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

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0004E347 File Offset: 0x0004C747
	// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0004E354 File Offset: 0x0004C754
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

	// Token: 0x060008E8 RID: 2280 RVA: 0x0004E374 File Offset: 0x0004C774
	public void Initialize()
	{
		this.animator = base.gameObject.GetComponent<Animator>();
		this.controller1 = (RuntimeAnimatorController)Resources.Load("controller1");
		this.controller2 = (RuntimeAnimatorController)Resources.Load("controller2");
		this.controller3 = (RuntimeAnimatorController)Resources.Load("controller3");
		this.controller4 = (RuntimeAnimatorController)Resources.Load("controller4");
		foreach (AnimationData animationData in this.animations)
		{
			if (animationData.wrapMode == WrapMode.Default)
			{
				animationData.wrapMode = this.defaultWrapMode;
			}
			animationData.clip.wrapMode = animationData.wrapMode;
		}
		this.state = new MecanimControlState();
		this.animator.enabled = false;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0004E444 File Offset: 0x0004C844
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

	// Token: 0x060008EA RID: 2282 RVA: 0x0004E510 File Offset: 0x0004C910
	private void updateAnimationToTime(AnimationData animation, Fixed targetTime)
	{
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0004E514 File Offset: 0x0004C914
	public void RemoveClip(string name)
	{
		List<AnimationData> list = new List<AnimationData>(this.animations);
		list.Remove(this.GetAnimationData(name));
		this.animations = list.ToArray();
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0004E548 File Offset: 0x0004C948
	public void RemoveClip(AnimationClip clip)
	{
		List<AnimationData> list = new List<AnimationData>(this.animations);
		list.Remove(this.GetAnimationData(clip));
		this.animations = list.ToArray();
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0004E57C File Offset: 0x0004C97C
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

	// Token: 0x060008EE RID: 2286 RVA: 0x0004E604 File Offset: 0x0004CA04
	public void AddClip(AnimationClip clip, string newName)
	{
		this.AddClip(clip, newName, 1, this.defaultWrapMode);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0004E61C File Offset: 0x0004CA1C
	public void AddClip(AnimationClip clip, string newName, Fixed speed, WrapMode wrapMode)
	{
		if (this.GetAnimationData(newName) != null)
		{
			Debug.LogWarning("An animation with the name '" + newName + "' already exists.");
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

	// Token: 0x060008F0 RID: 2288 RVA: 0x0004E6E4 File Offset: 0x0004CAE4
	public AnimationData GetAnimationData(string clipName)
	{
		foreach (AnimationData animationData in this.animations)
		{
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

	// Token: 0x060008F1 RID: 2289 RVA: 0x0004E744 File Offset: 0x0004CB44
	public AnimationData GetAnimationData(AnimationClip clip)
	{
		foreach (AnimationData animationData in this.animations)
		{
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

	// Token: 0x060008F2 RID: 2290 RVA: 0x0004E7A1 File Offset: 0x0004CBA1
	public void CrossFade(string clipName, Fixed blendingTime)
	{
		this.CrossFade(clipName, blendingTime, 0, this.currentMirror);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0004E7B7 File Offset: 0x0004CBB7
	public void CrossFade(string clipName, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), blendingTime, animationTime, mirror);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0004E7CA File Offset: 0x0004CBCA
	public void CrossFade(AnimationData animationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(animationData, blendingTime, animationTime, mirror);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0004E7D7 File Offset: 0x0004CBD7
	public void Play(string clipName, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), blendingTime, animationTime, mirror);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0004E7EA File Offset: 0x0004CBEA
	public void Play(AnimationClip clip, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clip), blendingTime, animationTime, mirror);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0004E7FD File Offset: 0x0004CBFD
	public void Play(string clipName, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clipName), 0, 0, mirror);
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0004E819 File Offset: 0x0004CC19
	public void Play(string clipName)
	{
		this._playAnimation(this.GetAnimationData(clipName), 0, 0, this.currentMirror);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0004E83A File Offset: 0x0004CC3A
	public void Play(AnimationClip clip, bool mirror)
	{
		this._playAnimation(this.GetAnimationData(clip), 0, 0, mirror);
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0004E856 File Offset: 0x0004CC56
	public void Play(AnimationClip clip)
	{
		this._playAnimation(this.GetAnimationData(clip), 0, 0, this.currentMirror);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0004E877 File Offset: 0x0004CC77
	public void Play(AnimationData animationData, bool mirror)
	{
		this._playAnimation(animationData, animationData.transitionDuration, 0, mirror);
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0004E88D File Offset: 0x0004CC8D
	public void Play(AnimationData animationData)
	{
		this._playAnimation(animationData, animationData.transitionDuration, 0, this.currentMirror);
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0004E8A8 File Offset: 0x0004CCA8
	public void Play(AnimationData animationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this._playAnimation(animationData, blendingTime, animationTime, mirror);
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0004E8B5 File Offset: 0x0004CCB5
	public void Play()
	{
		this.animator.speed = Mathf.Abs((float)this.currentAnimationData.speed);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0004E8D7 File Offset: 0x0004CCD7
	private void _playAnimation(AnimationData targetAnimationData, Fixed blendingTime, Fixed animationTime, bool mirror)
	{
		this.setAnimation(targetAnimationData, this.currentAnimationData, blendingTime, animationTime, mirror, true);
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0004E8EC File Offset: 0x0004CCEC
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

	// Token: 0x06000901 RID: 2305 RVA: 0x0004EAC0 File Offset: 0x0004CEC0
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
				Debug.LogWarning("Blending between avatar types is not supported.");
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

	// Token: 0x06000902 RID: 2306 RVA: 0x0004EC45 File Offset: 0x0004D045
	public bool IsPlaying(string clipName)
	{
		return this.IsPlaying(this.GetAnimationData(clipName));
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0004EC54 File Offset: 0x0004D054
	public bool IsPlaying(string clipName, float weight)
	{
		return this.IsPlaying(this.GetAnimationData(clipName), weight);
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0004EC64 File Offset: 0x0004D064
	public bool IsPlaying(AnimationClip clip)
	{
		return this.IsPlaying(this.GetAnimationData(clip));
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0004EC73 File Offset: 0x0004D073
	public bool IsPlaying(AnimationClip clip, float weight)
	{
		return this.IsPlaying(this.GetAnimationData(clip), weight);
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0004EC83 File Offset: 0x0004D083
	public bool IsPlaying(AnimationData animData)
	{
		return this.IsPlaying(animData, 0f);
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0004EC94 File Offset: 0x0004D094
	public bool IsPlaying(AnimationData animData, float weight)
	{
		return animData != null && this.currentAnimationData != null && (this.currentAnimationData != animData || animData.wrapMode != WrapMode.Once || animData.timesPlayed <= 0) && (this.currentAnimationData == animData || this.currentAnimationData.clip == animData.clip);
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0004ED00 File Offset: 0x0004D100
	public string GetCurrentClipName()
	{
		return this.currentAnimationData.clipName;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0004ED0D File Offset: 0x0004D10D
	public AnimationData GetCurrentAnimationData()
	{
		return this.currentAnimationData;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0004ED15 File Offset: 0x0004D115
	public int GetCurrentClipPlayCount()
	{
		return this.currentAnimationData.timesPlayed;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0004ED22 File Offset: 0x0004D122
	public Fixed GetCurrentClipTime()
	{
		return this.currentAnimationData.timeElapsed;
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0004ED2F File Offset: 0x0004D12F
	public Fixed GetCurrentClipLength()
	{
		return this.currentAnimationData.length;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0004ED3C File Offset: 0x0004D13C
	public void Stop()
	{
		this.Play(this.defaultAnimation.clip, this.defaultTransitionDuration, 0, this.currentMirror);
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0004ED61 File Offset: 0x0004D161
	public void SetWrapMode(WrapMode wrapMode)
	{
		this.defaultWrapMode = wrapMode;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0004ED6A File Offset: 0x0004D16A
	public void SetWrapMode(AnimationData animationData, WrapMode wrapMode)
	{
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0004ED80 File Offset: 0x0004D180
	public void SetWrapMode(AnimationClip clip, WrapMode wrapMode)
	{
		AnimationData animationData = this.GetAnimationData(clip);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0004EDA8 File Offset: 0x0004D1A8
	public void SetWrapMode(string clipName, WrapMode wrapMode)
	{
		AnimationData animationData = this.GetAnimationData(clipName);
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0004EDD0 File Offset: 0x0004D1D0
	public float GetSpeed()
	{
		return this.animator.speed;
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0004EDDD File Offset: 0x0004D1DD
	public bool GetMirror()
	{
		return this.currentMirror;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0004EDE5 File Offset: 0x0004D1E5
	public void SetMirror(bool toggle)
	{
		this.SetMirror(toggle, 0, false);
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0004EDF5 File Offset: 0x0004D1F5
	public void SetMirror(bool toggle, Fixed blendingTime)
	{
		this.SetMirror(toggle, blendingTime, false);
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0004EE00 File Offset: 0x0004D200
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

	// Token: 0x06000917 RID: 2327 RVA: 0x0004EE63 File Offset: 0x0004D263
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<MecanimControlState>(this.state));
		return true;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0004EE80 File Offset: 0x0004D280
	private AnimationData ApplyToAnimationData(AnimationData newData)
	{
		AnimationData animationData = this.GetAnimationData(newData.clipName);
		animationData.ReplaceFromRollback(newData);
		return animationData;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0004EEA4 File Offset: 0x0004D2A4
	public void SyncAnimation()
	{
		float num = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * this.currentAnimationData.clip.length;
		AnimatorClipInfo[] currentAnimatorClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);
		if ((this.currentAnimationData != null && (currentAnimatorClipInfo.Length == 0 || this.currentAnimationData.clipName != currentAnimatorClipInfo[0].clip.name || !FixedMath.ApproximatelyEqual((Fixed)((double)num), this.state.currentAnimation.timeElapsed))) || this.currentMirror != this.animatorMirror)
		{
			this.setAnimation(this.state.currentAnimation, this.state.previousAnimation, this.state.currentBlendTime, this.state.currentAnimation.timeElapsed, this.state.currentMirror, false);
		}
	}

	// Token: 0x0600091A RID: 2330 RVA: 0x0004EF94 File Offset: 0x0004D394
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

	// Token: 0x0600091B RID: 2331 RVA: 0x0004F04C File Offset: 0x0004D44C
	public void OnDestroy()
	{
		foreach (AnimationClip obj in this.instantiatedClips)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	// Token: 0x04000659 RID: 1625
	public AnimationData defaultAnimation = new AnimationData();

	// Token: 0x0400065A RID: 1626
	public AnimationData[] animations = new AnimationData[0];

	// Token: 0x0400065B RID: 1627
	public bool debugMode;

	// Token: 0x0400065C RID: 1628
	public bool alwaysPlay;

	// Token: 0x0400065D RID: 1629
	public bool overrideRootMotion;

	// Token: 0x0400065E RID: 1630
	public Fixed defaultTransitionDuration = (Fixed)0.15;

	// Token: 0x0400065F RID: 1631
	public WrapMode defaultWrapMode = WrapMode.Loop;

	// Token: 0x04000660 RID: 1632
	private Animator animator;

	// Token: 0x04000661 RID: 1633
	public AvatarController avatarController;

	// Token: 0x04000662 RID: 1634
	private RuntimeAnimatorController controller1;

	// Token: 0x04000663 RID: 1635
	private RuntimeAnimatorController controller2;

	// Token: 0x04000664 RID: 1636
	private RuntimeAnimatorController controller3;

	// Token: 0x04000665 RID: 1637
	private RuntimeAnimatorController controller4;

	// Token: 0x04000666 RID: 1638
	private MecanimControlState state;

	// Token: 0x04000667 RID: 1639
	private Fixed lastVisibleAnimationTime = 0;

	// Token: 0x04000668 RID: 1640
	private bool animatorMirror;

	// Token: 0x04000669 RID: 1641
	private List<AnimationClip> instantiatedClips = new List<AnimationClip>();

	// Token: 0x020001EF RID: 495
	// (Invoke) Token: 0x0600091D RID: 2333
	public delegate void AnimEvent(AnimationData animationData);
}
