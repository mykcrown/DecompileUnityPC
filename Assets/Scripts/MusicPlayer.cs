// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

internal class MusicPlayer
{
	private sealed class _beginVolumeTween_c__AnonStorey0
	{
		internal Action callback;

		internal MusicPlayer _this;

		internal void __m__0(float x)
		{
			this._this.volume = x;
		}

		internal void __m__1()
		{
			this._this.killVolumeTween();
			this._this.executeCallback(this.callback);
		}
	}

	private sealed class _FadeOutAndCancel_c__AnonStorey1
	{
		internal Action callback;

		internal MusicPlayer _this;

		internal void __m__0()
		{
			this._this.stopAndCleanup();
			this._this.executeCallback(this.callback);
		}
	}

	private AudioSource musicSource;

	private float individualVolume;

	private float fadeTime = 0.5f;

	private Tweener volumeTween;

	private float currentVolumeTweenTarget = 1f;

	private bool isFadingOut;

	private bool inUse;

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	[Inject]
	public IAudioVolumeConfig audioVolumeConfig
	{
		get;
		set;
	}

	public AudioClip Clip
	{
		get
		{
			return this.musicSource.clip;
		}
	}

	private float targetVolume
	{
		get
		{
			return this.userAudioSettings.GetMusicVolume() * this.audioVolumeConfig.MusicVolume * this.individualVolume;
		}
	}

	private bool needVolumeChange
	{
		get
		{
			return !this.isVolumeAtTarget && !this.isTweeningToTarget;
		}
	}

	private bool isVolumeAtTarget
	{
		get
		{
			return MathUtil.almostEqual(this.targetVolume, this.musicSource.volume, 0.0001f);
		}
	}

	private bool isTweeningToTarget
	{
		get
		{
			return this.volumeTween != null && MathUtil.almostEqual(this.targetVolume, this.currentVolumeTweenTarget, 0.0001f);
		}
	}

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

	public void Init(AudioSource musicSource)
	{
		this.musicSource = musicSource;
		musicSource.loop = true;
		musicSource.playOnAwake = false;
		musicSource.ignoreListenerVolume = true;
		musicSource.priority = 0;
	}

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

	public bool IsAvailable()
	{
		return !this.inUse;
	}

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

	private void beginVolumeTween(float target, Action callback = null)
	{
		MusicPlayer._beginVolumeTween_c__AnonStorey0 _beginVolumeTween_c__AnonStorey = new MusicPlayer._beginVolumeTween_c__AnonStorey0();
		_beginVolumeTween_c__AnonStorey.callback = callback;
		_beginVolumeTween_c__AnonStorey._this = this;
		this.killVolumeTween();
		this.currentVolumeTweenTarget = target;
		this.volumeTween = DOTween.To(new DOGetter<float>(this.get_volume), new DOSetter<float>(_beginVolumeTween_c__AnonStorey.__m__0), target, this.fadeTime).SetEase(Ease.Linear).OnComplete(new TweenCallback(_beginVolumeTween_c__AnonStorey.__m__1));
	}

	public void FadeOutAndCancel(float fadeTime = -1f, Action callback = null)
	{
		MusicPlayer._FadeOutAndCancel_c__AnonStorey1 _FadeOutAndCancel_c__AnonStorey = new MusicPlayer._FadeOutAndCancel_c__AnonStorey1();
		_FadeOutAndCancel_c__AnonStorey.callback = callback;
		_FadeOutAndCancel_c__AnonStorey._this = this;
		this.isFadingOut = true;
		this.fadeTime = ((fadeTime != -1f) ? fadeTime : this.audioVolumeConfig.MusicFadeTime);
		if (this.volume == 0f)
		{
			this.executeCallback(_FadeOutAndCancel_c__AnonStorey.callback);
		}
		else
		{
			this.beginVolumeTween(0f, new Action(_FadeOutAndCancel_c__AnonStorey.__m__0));
		}
	}

	private void executeCallback(Action callback)
	{
		if (callback != null)
		{
			callback();
		}
	}

	private void killVolumeTween()
	{
		TweenUtil.Destroy(ref this.volumeTween);
	}

	private void stopAndCleanup()
	{
		this.isFadingOut = false;
		this.inUse = false;
		this.musicSource.Stop();
		this.musicSource.clip = null;
	}
}
