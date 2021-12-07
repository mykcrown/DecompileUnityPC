using System;
using FixedPoint;
using UnityEngine;

// Token: 0x0200057D RID: 1405
[Serializable]
public class CharacterAnimation
{
	// Token: 0x170006DE RID: 1758
	// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000A0EDC File Offset: 0x0009F2DC
	// (set) Token: 0x06001F8E RID: 8078 RVA: 0x000A0F1B File Offset: 0x0009F31B
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

	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000A0F24 File Offset: 0x0009F324
	// (set) Token: 0x06001F90 RID: 8080 RVA: 0x000A0F79 File Offset: 0x0009F379
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

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000A0F84 File Offset: 0x0009F384
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

	// Token: 0x170006E1 RID: 1761
	// (get) Token: 0x06001F92 RID: 8082 RVA: 0x000A0FD6 File Offset: 0x0009F3D6
	public int frameDuration
	{
		get
		{
			return (int)(this.animationDuration * 60);
		}
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000A0FEA File Offset: 0x0009F3EA
	public void SetAnimationNames(string animName, string leftAnimName, string suffix)
	{
		this.AnimationName = string.Format("{0}_{1}", animName, suffix);
		this.LeftAnimationName = string.Format("{0}_{1}", leftAnimName, suffix);
	}

	// Token: 0x040018F0 RID: 6384
	public AnimationClip animation;

	// Token: 0x040018F1 RID: 6385
	public AnimationClip leftAnimation;

	// Token: 0x040018F2 RID: 6386
	public Fixed playSpeed = 1;

	// Token: 0x040018F3 RID: 6387
	public WrapMode wrapMode = WrapMode.Loop;

	// Token: 0x040018F4 RID: 6388
	private string animName;

	// Token: 0x040018F5 RID: 6389
	private string leftAnimName;
}
