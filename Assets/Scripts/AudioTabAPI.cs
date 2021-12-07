// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class AudioTabAPI : IAudioTabAPI
{
	public static string UPDATED = "AudioTabAPI.UPDATED";

	private const float LOGRITHMIC_BASE = 2.71828175f;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		get;
		set;
	}

	public float MasterVolume
	{
		get
		{
			return 1f;
		}
		set
		{
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public float SoundsEffectsVolume
	{
		get
		{
			float soundEffectsVolume = this.userAudioSettings.GetSoundEffectsVolume();
			return this.ConvertAmplitudeToLoudness(soundEffectsVolume);
		}
		set
		{
			float loudness = Mathf.Clamp01(value);
			float soundEffects = this.ConvertLoundnessToAmplitude(loudness);
			this.userAudioSettings.SetSoundEffects(soundEffects);
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public float MusicVolume
	{
		get
		{
			float musicVolume = this.userAudioSettings.GetMusicVolume();
			return this.ConvertAmplitudeToLoudness(musicVolume);
		}
		set
		{
			float loudness = Mathf.Clamp01(value);
			float music = this.ConvertLoundnessToAmplitude(loudness);
			this.userAudioSettings.SetMusic(music);
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public bool UseAltSounds
	{
		get
		{
			return this.userAudioSettings.UseAltSounds();
		}
		set
		{
			this.userAudioSettings.SetUseAltSounds(value);
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public bool UseAltMenuMusic
	{
		get
		{
			return this.userAudioSettings.UseAltMenuMusic();
		}
		set
		{
			this.userAudioSettings.SetUseAltMenuMusic(value);
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public bool UseAltBattleMusic
	{
		get
		{
			return this.userAudioSettings.UseAltBattleMusic();
		}
		set
		{
			this.userAudioSettings.SetUseAltBattleMusic(value);
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public float CharacterAnnouncerVolume
	{
		get
		{
			return 1f;
		}
		set
		{
			this.signalBus.Dispatch(AudioTabAPI.UPDATED);
		}
	}

	public void Reset()
	{
		this.userAudioSettings.Reset();
		this.signalBus.Dispatch(AudioTabAPI.UPDATED);
	}

	private float ConvertAmplitudeToLoudness(float amplitude)
	{
		return Mathf.Log(amplitude * 1.71828175f + 1f, 2.71828175f);
	}

	private float ConvertLoundnessToAmplitude(float loudness)
	{
		return (Mathf.Pow(2.71828175f, loudness) - 1f) / 1.71828175f;
	}
}
