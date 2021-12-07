using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020002B0 RID: 688
internal class MusicPlayer
{
	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x0005B61B File Offset: 0x00059A1B
	// (set) Token: 0x06000EF5 RID: 3829 RVA: 0x0005B623 File Offset: 0x00059A23
	[Inject]
	public UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x0005B62C File Offset: 0x00059A2C
	// (set) Token: 0x06000EF7 RID: 3831 RVA: 0x0005B634 File Offset: 0x00059A34
	[Inject]
	public IAudioVolumeConfig audioVolumeConfig { get; set; }

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0005B63D File Offset: 0x00059A3D
	public void Init(AudioSource musicSource)
	{
		this.musicSource = musicSource;
		musicSource.loop = true;
		musicSource.playOnAwake = false;
		musicSource.ignoreListenerVolume = true;
		musicSource.priority = 0;
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0005B662 File Offset: 0x00059A62
	public AudioClip Clip
	{
		get
		{
			return this.musicSource.clip;
		}
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0005B670 File Offset: 0x00059A70
	public void Play(AudioClip clip, float volume)
	{
		this.isFadingOut = false;
		this.inUse = true;
		this.individualVolume = volume;
		this.fadeTime = this.audioVolumeConfig.MusicFadeTime;
		this.musicSource.clip = clip;
		this.musicSource.volume = this.targetVolume;
		this.musicSource.Play();
		this.UpdateVolume();
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0005B6D1 File Offset: 0x00059AD1
	private float targetVolume
	{
		get
		{
			return this.userAudioSettings.GetMusicVolume() * this.audioVolumeConfig.MusicVolume * this.individualVolume;
		}
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0005B6F1 File Offset: 0x00059AF1
	public bool IsAvailable()
	{
		return !this.inUse;
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0005B6FC File Offset: 0x00059AFC
	public void UpdateVolume()
	{
		if (this.isFadingOut)
		{
			return;
		}
		if (!this.inUse)
		{
			return;
		}
		float targetVolume = this.targetVolume;
		if (this.needVolumeChange)
		{
			this.beginVolumeTween(targetVolume, null);
		}
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0005B73C File Offset: 0x00059B3C
	private void beginVolumeTween(float target, Action callback = null)
	{
		this.killVolumeTween();
		this.currentVolumeTweenTarget = target;
		this.volumeTween = DOTween.To(new DOGetter<float>(this.get_volume), delegate(float x)
		{
			this.volume = x;
		}, target, this.fadeTime).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.killVolumeTween();
			this.executeCallback(callback);
		});
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0005B7AC File Offset: 0x00059BAC
	public void FadeOutAndCancel(float fadeTime = -1f, Action callback = null)
	{
		this.isFadingOut = true;
		this.fadeTime = ((fadeTime != -1f) ? fadeTime : this.audioVolumeConfig.MusicFadeTime);
		if (this.volume == 0f)
		{
			this.executeCallback(callback);
		}
		else
		{
			this.beginVolumeTween(0f, delegate
			{
				this.stopAndCleanup();
				this.executeCallback(callback);
			});
		}
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x0005B82E File Offset: 0x00059C2E
	private void executeCallback(Action callback)
	{
		if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0005B83C File Offset: 0x00059C3C
	private bool needVolumeChange
	{
		get
		{
			return !this.isVolumeAtTarget && !this.isTweeningToTarget;
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0005B855 File Offset: 0x00059C55
	private bool isVolumeAtTarget
	{
		get
		{
			return MathUtil.almostEqual(this.targetVolume, this.musicSource.volume, 0.0001f);
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0005B872 File Offset: 0x00059C72
	private bool isTweeningToTarget
	{
		get
		{
			return this.volumeTween != null && MathUtil.almostEqual(this.targetVolume, this.currentVolumeTweenTarget, 0.0001f);
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0005B89F File Offset: 0x00059C9F
	// (set) Token: 0x06000F05 RID: 3845 RVA: 0x0005B8AC File Offset: 0x00059CAC
	public float volume
	{
		get
		{
			return this.musicSource.volume;
		}
		set
		{
			this.musicSource.volume = value;
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0005B8BA File Offset: 0x00059CBA
	private void killVolumeTween()
	{
		TweenUtil.Destroy(ref this.volumeTween);
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0005B8C7 File Offset: 0x00059CC7
	private void stopAndCleanup()
	{
		this.isFadingOut = false;
		this.inUse = false;
		this.musicSource.Stop();
		this.musicSource.clip = null;
	}

	// Token: 0x040008A8 RID: 2216
	private AudioSource musicSource;

	// Token: 0x040008A9 RID: 2217
	private float individualVolume;

	// Token: 0x040008AA RID: 2218
	private float fadeTime = 0.5f;

	// Token: 0x040008AB RID: 2219
	private Tweener volumeTween;

	// Token: 0x040008AC RID: 2220
	private float currentVolumeTweenTarget = 1f;

	// Token: 0x040008AD RID: 2221
	private bool isFadingOut;

	// Token: 0x040008AE RID: 2222
	private bool inUse;
}
