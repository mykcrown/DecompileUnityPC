using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class StageAnimation : StageProp
{
	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x0600270C RID: 9996 RVA: 0x000BF0AA File Offset: 0x000BD4AA
	public override bool IsSimulation
	{
		get
		{
			return base.GetComponentsInChildren<StageSurface>().Length > 0;
		}
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x000BF0B8 File Offset: 0x000BD4B8
	public override void Awake()
	{
		base.Awake();
		this.model = new StageAnimationModel();
		this.anim = base.GetComponent<Animation>();
		if (this.anim == null)
		{
			this.anim = base.gameObject.AddComponent<Animation>();
		}
		this.couldChangeGameStage = this.IsSimulation;
		this.model.shouldValidate = this.couldChangeGameStage;
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000BF121 File Offset: 0x000BD521
	public void PlayClip(AnimationClip clip)
	{
		this.AddClipToAnimationComponent(clip);
		this.model.Clip = clip;
		this.model.StartFrame = base.gameManager.Frame;
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000BF14C File Offset: 0x000BD54C
	private void AddClipToAnimationComponent(AnimationClip clip)
	{
		this.anim.AddClip(clip, "STAGE_ANIMATION");
		this.anim["STAGE_ANIMATION"].speed = 0f;
		this.model.AnimFrameLength = (int)(clip.length * WTime.fps);
		this.anim.Play("STAGE_ANIMATION");
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x000BF1AD File Offset: 0x000BD5AD
	public override void TickFrame()
	{
		if (!base.gameManager.IsRollingBack && !base.enabled)
		{
			base.enabled = true;
		}
		this.UpdateAnimationTime();
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x000BF1D8 File Offset: 0x000BD5D8
	private void UpdateAnimationTime()
	{
		int num = base.gameManager.Frame - this.model.StartFrame;
		if (num >= 0 && this.model.Clip != null)
		{
			if (base.enabled)
			{
				this.anim["STAGE_ANIMATION"].time = (float)num / WTime.fps;
			}
			if (num > this.model.AnimFrameLength)
			{
				if (base.enabled)
				{
					this.anim.RemoveClip("STAGE_ANIMATION");
				}
				this.model.Clip = null;
				this.model.AnimFrameLength = 0;
			}
			else if (this.couldChangeGameStage)
			{
				this.anim.Sample();
			}
		}
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000BF2A2 File Offset: 0x000BD6A2
	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<StageAnimationModel>(this.model));
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000BF2BC File Offset: 0x000BD6BC
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageAnimationModel>(ref this.model);
		if (this.model.Clip != null && this.couldChangeGameStage)
		{
			this.AddClipToAnimationComponent(this.model.Clip);
			this.UpdateAnimationTime();
		}
		if (!this.couldChangeGameStage)
		{
			base.enabled = false;
		}
		return true;
	}

	// Token: 0x04001CA2 RID: 7330
	private const string ANIMATION_KEY = "STAGE_ANIMATION";

	// Token: 0x04001CA3 RID: 7331
	private Animation anim;

	// Token: 0x04001CA4 RID: 7332
	private StageAnimationModel model;

	// Token: 0x04001CA5 RID: 7333
	private bool couldChangeGameStage;
}
