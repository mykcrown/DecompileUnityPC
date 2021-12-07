// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class HologramDisplay : BaseItem3DPreviewDisplay
{
	public enum PlayBehavior
	{
		None,
		Once,
		Loop,
		PauseAfterFadeIn
	}

	private VFXHologramController hologramController;

	private HologramDisplay.PlayBehavior playBehavior;

	private float playTimer;

	private float playBehaviorInterval;

	private void Awake()
	{
		this.hologramController = base.GetComponent<VFXHologramController>();
		base.onClick += new Action(this.replayHologram);
	}

	protected override void Update()
	{
		if (this.playBehavior == HologramDisplay.PlayBehavior.None)
		{
			base.enabled = false;
			return;
		}
		base.Update();
		switch (this.playBehavior)
		{
		case HologramDisplay.PlayBehavior.None:
			IL_3B:
			break;
		case HologramDisplay.PlayBehavior.Once:
			return;
		case HologramDisplay.PlayBehavior.Loop:
			this.playTimer -= Time.deltaTime;
			if (this.playTimer <= 0f)
			{
				this.Replay();
			}
			return;
		case HologramDisplay.PlayBehavior.PauseAfterFadeIn:
			if (this.playTimer > 0f)
			{
				this.playTimer -= Time.deltaTime;
				if (this.playTimer <= 0f)
				{
					this.hologramController.Pause();
				}
			}
			return;
		}
		goto IL_3B;
	}

	public void SetTexture(Texture2D texture)
	{
		this.hologramController.SetHologramData(texture);
	}

	private void replayHologram()
	{
		this.hologramController.Replay();
	}

	public void PlayHologram(HologramDisplay.PlayBehavior behavior, float time = 0f)
	{
		this.playBehavior = behavior;
		this.playBehaviorInterval = time;
		this.playTimer = this.playBehaviorInterval;
		this.replayHologram();
	}

	public void Replay()
	{
		this.playTimer = this.playBehaviorInterval;
		this.replayHologram();
	}
}
