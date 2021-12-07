using System;
using UnityEngine;

// Token: 0x02000968 RID: 2408
public class AudioTabAPI : IAudioTabAPI
{
	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x0600404A RID: 16458 RVA: 0x00121E6F File Offset: 0x0012026F
	// (set) Token: 0x0600404B RID: 16459 RVA: 0x00121E77 File Offset: 0x00120277
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x0600404C RID: 16460 RVA: 0x00121E80 File Offset: 0x00120280
	// (set) Token: 0x0600404D RID: 16461 RVA: 0x00121E88 File Offset: 0x00120288
	[Inject]
	public UserAudioSettings userAudioSettings { get; set; }

	// Token: 0x0600404E RID: 16462 RVA: 0x00121E91 File Offset: 0x00120291
	public void Reset()
	{
		this.userAudioSettings.Reset();
		this.signalBus.Dispatch(AudioTabAPI.UPDATED);
	}

	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x0600404F RID: 16463 RVA: 0x00121EAE File Offset: 0x001202AE
	// (set) Token: 0x06004050 RID: 16464 RVA: 0x00121EB5 File Offset: 0x001202B5
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

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x06004051 RID: 16465 RVA: 0x00121EC8 File Offset: 0x001202C8
	// (set) Token: 0x06004052 RID: 16466 RVA: 0x00121EE8 File Offset: 0x001202E8
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

	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x06004053 RID: 16467 RVA: 0x00121F20 File Offset: 0x00120320
	// (set) Token: 0x06004054 RID: 16468 RVA: 0x00121F40 File Offset: 0x00120340
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

	// Token: 0x17000F35 RID: 3893
	// (get) Token: 0x06004055 RID: 16469 RVA: 0x00121F78 File Offset: 0x00120378
	// (set) Token: 0x06004056 RID: 16470 RVA: 0x00121F85 File Offset: 0x00120385
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

	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x06004057 RID: 16471 RVA: 0x00121FA3 File Offset: 0x001203A3
	// (set) Token: 0x06004058 RID: 16472 RVA: 0x00121FB0 File Offset: 0x001203B0
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

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x06004059 RID: 16473 RVA: 0x00121FCE File Offset: 0x001203CE
	// (set) Token: 0x0600405A RID: 16474 RVA: 0x00121FDB File Offset: 0x001203DB
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

	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x0600405B RID: 16475 RVA: 0x00121FF9 File Offset: 0x001203F9
	// (set) Token: 0x0600405C RID: 16476 RVA: 0x00122000 File Offset: 0x00120400
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

	// Token: 0x0600405D RID: 16477 RVA: 0x00122012 File Offset: 0x00120412
	private float ConvertAmplitudeToLoudness(float amplitude)
	{
		return Mathf.Log(amplitude * 1.7182817f + 1f, 2.7182817f);
	}

	// Token: 0x0600405E RID: 16478 RVA: 0x0012202B File Offset: 0x0012042B
	private float ConvertLoundnessToAmplitude(float loudness)
	{
		return (Mathf.Pow(2.7182817f, loudness) - 1f) / 1.7182817f;
	}

	// Token: 0x04002B62 RID: 11106
	public static string UPDATED = "AudioTabAPI.UPDATED";

	// Token: 0x04002B65 RID: 11109
	private const float LOGRITHMIC_BASE = 2.7182817f;
}
