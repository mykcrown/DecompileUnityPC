using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020001ED RID: 493
[Serializable]
public class AnimationData : RollbackStateTyped<AnimationData>
{
	// Token: 0x060008DB RID: 2267 RVA: 0x0004DFDC File Offset: 0x0004C3DC
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

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060008DC RID: 2268 RVA: 0x0004E079 File Offset: 0x0004C479
	public int totalFrames
	{
		get
		{
			return (int)((Fixed)0.001 + Constants.FPS * this.length);
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x0004E0A3 File Offset: 0x0004C4A3
	public Fixed normalizedSpeed
	{
		get
		{
			return this.speed / this.originalSpeed;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x0004E0B8 File Offset: 0x0004C4B8
	public int speedAdjustedFrames
	{
		get
		{
			return (!(this.speed == 0)) ? ((int)((Fixed)0.001 + this.totalFrames / this.speed)) : 0;
		}
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x0004E108 File Offset: 0x0004C508
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

	// Token: 0x0400064C RID: 1612
	public string clipName;

	// Token: 0x0400064D RID: 1613
	public Fixed speed = 1;

	// Token: 0x0400064E RID: 1614
	public Fixed transitionDuration = -1;

	// Token: 0x0400064F RID: 1615
	public WrapMode wrapMode;

	// Token: 0x04000650 RID: 1616
	public bool applyRootMotion;

	// Token: 0x04000651 RID: 1617
	[HideInInspector]
	public int timesPlayed;

	// Token: 0x04000652 RID: 1618
	public Fixed length = 0;

	// Token: 0x04000653 RID: 1619
	[HideInInspector]
	public int stateHash;

	// Token: 0x04000654 RID: 1620
	[HideInInspector]
	public string stateName;

	// Token: 0x04000655 RID: 1621
	[HideInInspector]
	public Fixed originalSpeed = 1;

	// Token: 0x04000656 RID: 1622
	public Fixed timeElapsed = 0;

	// Token: 0x04000657 RID: 1623
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public AnimationClip clip;
}
