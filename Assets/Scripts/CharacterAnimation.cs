// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class CharacterAnimation
{
	public AnimationClip animation;

	public AnimationClip leftAnimation;

	public Fixed playSpeed = 1;

	public WrapMode wrapMode = WrapMode.Loop;

	private string animName;

	private string leftAnimName;

	public string AnimationName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.animName))
			{
				return this.animName;
			}
			return (!(this.animation != null)) ? string.Empty : this.animation.name;
		}
		set
		{
			this.animName = value;
		}
	}

	public string LeftAnimationName
	{
		get
		{
			if (!string.IsNullOrEmpty(this.leftAnimName))
			{
				return this.leftAnimName;
			}
			return (!(this.leftAnimation != null)) ? string.Format("{0}_left", this.AnimationName) : this.leftAnimation.name;
		}
		set
		{
			this.leftAnimName = value;
		}
	}

	public Fixed animationDuration
	{
		get
		{
			if (this.playSpeed == 0 || this.animation == null)
			{
				return 0;
			}
			return (Fixed)((double)this.animation.length) / this.playSpeed;
		}
	}

	public int frameDuration
	{
		get
		{
			return (int)(this.animationDuration * 60);
		}
	}

	public void SetAnimationNames(string animName, string leftAnimName, string suffix)
	{
		this.AnimationName = string.Format("{0}_{1}", animName, suffix);
		this.LeftAnimationName = string.Format("{0}_{1}", leftAnimName, suffix);
	}
}
