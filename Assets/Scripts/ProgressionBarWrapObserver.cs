// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionBarWrapObserver : ClientBehavior
{
	[SerializeField]
	private ProgressionBar bar;

	[SerializeField]
	private GameObject item;

	[SerializeField]
	private bool alsoOnCap = true;

	private AudioData tickSound;

	private int experiencePerTick;

	private AnimationCurve tickPitchVariation;

	private float lastTickValue;

	[Inject]
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

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

	private void OnDisable()
	{
		this.bar.onWrap.RemoveListener(new UnityAction(this.Show));
		if (this.alsoOnCap)
		{
			this.bar.onCap.RemoveListener(new UnityAction(this.Show));
		}
		this.bar.onValueUpdate.RemoveListener(new UnityAction(this.ValueUpdated));
	}

	private void Show()
	{
		this.item.SetActive(true);
	}

	private void ValueUpdated()
	{
		if (this.bar.CurrentValue - this.bar.MostRecentStartValue == 0uL)
		{
			this.item.SetActive(false);
		}
		this.updateTickSound();
	}

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
}
