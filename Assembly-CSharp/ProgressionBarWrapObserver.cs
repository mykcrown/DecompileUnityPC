using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200094D RID: 2381
public class ProgressionBarWrapObserver : ClientBehavior
{
	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x06003F41 RID: 16193 RVA: 0x0011FC5B File Offset: 0x0011E05B
	// (set) Token: 0x06003F42 RID: 16194 RVA: 0x0011FC63 File Offset: 0x0011E063
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x06003F43 RID: 16195 RVA: 0x0011FC6C File Offset: 0x0011E06C
	private void OnEnable()
	{
		this.bar.onWrap.AddListener(new UnityAction(this.Show));
		if (this.alsoOnCap)
		{
			this.bar.onCap.AddListener(new UnityAction(this.Show));
		}
		this.bar.onValueUpdate.AddListener(new UnityAction(this.ValueUpdated));
		this.ValueUpdated();
		this.tickSound = this.soundFileManager.GetSoundAsAudioData(SoundKey.playerProgression_experienceTick);
		this.experiencePerTick = base.config.soundSettings.experiencePerTick;
		this.tickPitchVariation = base.config.soundSettings.tickPitchVariation;
	}

	// Token: 0x06003F44 RID: 16196 RVA: 0x0011FD20 File Offset: 0x0011E120
	private void OnDisable()
	{
		this.bar.onWrap.RemoveListener(new UnityAction(this.Show));
		if (this.alsoOnCap)
		{
			this.bar.onCap.RemoveListener(new UnityAction(this.Show));
		}
		this.bar.onValueUpdate.RemoveListener(new UnityAction(this.ValueUpdated));
	}

	// Token: 0x06003F45 RID: 16197 RVA: 0x0011FD8C File Offset: 0x0011E18C
	private void Show()
	{
		this.item.SetActive(true);
	}

	// Token: 0x06003F46 RID: 16198 RVA: 0x0011FD9A File Offset: 0x0011E19A
	private void ValueUpdated()
	{
		if (this.bar.CurrentValue - this.bar.MostRecentStartValue == 0UL)
		{
			this.item.SetActive(false);
		}
		this.updateTickSound();
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x0011FDCC File Offset: 0x0011E1CC
	private void updateTickSound()
	{
		if (this.tickSound.sound == null)
		{
			return;
		}
		if (this.lastTickValue + (float)this.experiencePerTick < this.bar.CurrentValue)
		{
			float time = (this.bar.CurrentValue - this.bar.MostRecentStartValue) / (this.bar.Target - this.bar.MostRecentStartValue);
			float volumeMultiplier = 1f;
			if (this.bar.ThresholdForValue(this.bar.CurrentValue) != this.bar.ThresholdForValue(this.bar.MostRecentStartValue))
			{
				volumeMultiplier = base.config.soundSettings.experienceAfterLevelUpDampen;
			}
			AudioRequest request = new AudioRequest(this.tickSound, null);
			request.pitch = this.tickPitchVariation.Evaluate(time);
			request.volumeMultiplier = volumeMultiplier;
			base.audioManager.PlayMenuSound(request, 0f);
			this.lastTickValue = this.bar.CurrentValue;
		}
	}

	// Token: 0x04002AE3 RID: 10979
	[SerializeField]
	private ProgressionBar bar;

	// Token: 0x04002AE4 RID: 10980
	[SerializeField]
	private GameObject item;

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	private bool alsoOnCap = true;

	// Token: 0x04002AE6 RID: 10982
	private AudioData tickSound;

	// Token: 0x04002AE7 RID: 10983
	private int experiencePerTick;

	// Token: 0x04002AE8 RID: 10984
	private AnimationCurve tickPitchVariation;

	// Token: 0x04002AE9 RID: 10985
	private float lastTickValue;
}
