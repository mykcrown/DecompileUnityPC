// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class StageAnimation : StageProp
{
	private const string ANIMATION_KEY = "STAGE_ANIMATION";

	private Animation anim;

	private StageAnimationModel model;

	private bool couldChangeGameStage;

	public override bool IsSimulation
	{
		get
		{
			return base.GetComponentsInChildren<StageSurface>().Length > 0;
		}
	}

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

	public void PlayClip(AnimationClip clip)
	{
		this.AddClipToAnimationComponent(clip);
		this.model.Clip = clip;
		this.model.StartFrame = base.gameManager.Frame;
	}

	private void AddClipToAnimationComponent(AnimationClip clip)
	{
		this.anim.AddClip(clip, "STAGE_ANIMATION");
		this.anim["STAGE_ANIMATION"].speed = 0f;
		this.model.AnimFrameLength = (int)(clip.length * WTime.fps);
		this.anim.Play("STAGE_ANIMATION");
	}

	public override void TickFrame()
	{
		if (!base.gameManager.IsRollingBack && !base.enabled)
		{
			base.enabled = true;
		}
		this.UpdateAnimationTime();
	}

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

	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<StageAnimationModel>(this.model));
	}

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
}
