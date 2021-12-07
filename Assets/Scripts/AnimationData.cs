// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class AnimationData : RollbackStateTyped<AnimationData>
{
	public string clipName;

	public Fixed speed = 1;

	public Fixed transitionDuration = -1;

	public WrapMode wrapMode;

	public bool applyRootMotion;

	[HideInInspector]
	public int timesPlayed;

	public Fixed length = 0;

	[HideInInspector]
	public int stateHash;

	[HideInInspector]
	public string stateName;

	[HideInInspector]
	public Fixed originalSpeed = 1;

	public Fixed timeElapsed = 0;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public AnimationClip clip;

	public int totalFrames
	{
		get
		{
			return (int)((Fixed)0.001 + Constants.FPS * this.length);
		}
	}

	public Fixed normalizedSpeed
	{
		get
		{
			return this.speed / this.originalSpeed;
		}
	}

	public int speedAdjustedFrames
	{
		get
		{
			return (!(this.speed == 0)) ? ((int)((Fixed)0.001 + this.totalFrames / this.speed)) : 0;
		}
	}

	public override void CopyTo(AnimationData targetIn)
	{
		targetIn.clipName = this.clipName;
		targetIn.speed = this.speed;
		targetIn.transitionDuration = this.transitionDuration;
		targetIn.wrapMode = this.wrapMode;
		targetIn.applyRootMotion = this.applyRootMotion;
		targetIn.timesPlayed = this.timesPlayed;
		targetIn.length = this.length;
		targetIn.stateHash = this.stateHash;
		targetIn.stateName = this.stateName;
		targetIn.originalSpeed = this.originalSpeed;
		targetIn.timeElapsed = this.timeElapsed;
		targetIn.clip = this.clip;
	}

	public void ReplaceFromRollback(AnimationData copyFrom)
	{
		this.speed = copyFrom.speed;
		this.transitionDuration = copyFrom.transitionDuration;
		this.wrapMode = copyFrom.wrapMode;
		this.applyRootMotion = copyFrom.applyRootMotion;
		this.timesPlayed = copyFrom.timesPlayed;
		this.length = copyFrom.length;
		this.stateHash = copyFrom.stateHash;
		this.stateName = copyFrom.stateName;
		this.originalSpeed = copyFrom.originalSpeed;
		this.timeElapsed = copyFrom.timeElapsed;
	}
}
