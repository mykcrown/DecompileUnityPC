using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class HologramDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x06002E81 RID: 11905 RVA: 0x000EB340 File Offset: 0x000E9740
	private void Awake()
	{
		this.hologramController = base.GetComponent<VFXHologramController>();
		base.onClick += this.replayHologram;
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x000EB360 File Offset: 0x000E9760
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
		case HologramDisplay.PlayBehavior.Loop:
			this.playTimer -= Time.deltaTime;
			if (this.playTimer <= 0f)
			{
				this.Replay();
			}
			break;
		case HologramDisplay.PlayBehavior.PauseAfterFadeIn:
			if (this.playTimer > 0f)
			{
				this.playTimer -= Time.deltaTime;
				if (this.playTimer <= 0f)
				{
					this.hologramController.Pause();
				}
			}
			break;
		}
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x000EB421 File Offset: 0x000E9821
	public void SetTexture(Texture2D texture)
	{
		this.hologramController.SetHologramData(texture);
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x000EB42F File Offset: 0x000E982F
	private void replayHologram()
	{
		this.hologramController.Replay();
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x000EB43C File Offset: 0x000E983C
	public void PlayHologram(HologramDisplay.PlayBehavior behavior, float time = 0f)
	{
		this.playBehavior = behavior;
		this.playBehaviorInterval = time;
		this.playTimer = this.playBehaviorInterval;
		this.replayHologram();
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x000EB45E File Offset: 0x000E985E
	public void Replay()
	{
		this.playTimer = this.playBehaviorInterval;
		this.replayHologram();
	}

	// Token: 0x040020A8 RID: 8360
	private VFXHologramController hologramController;

	// Token: 0x040020A9 RID: 8361
	private HologramDisplay.PlayBehavior playBehavior;

	// Token: 0x040020AA RID: 8362
	private float playTimer;

	// Token: 0x040020AB RID: 8363
	private float playBehaviorInterval;

	// Token: 0x02000756 RID: 1878
	public enum PlayBehavior
	{
		// Token: 0x040020AD RID: 8365
		None,
		// Token: 0x040020AE RID: 8366
		Once,
		// Token: 0x040020AF RID: 8367
		Loop,
		// Token: 0x040020B0 RID: 8368
		PauseAfterFadeIn
	}
}
