using System;
using UnityEngine;

// Token: 0x020002E5 RID: 741
public class UserAudioSettings
{
	// Token: 0x06000F65 RID: 3941 RVA: 0x0005D580 File Offset: 0x0005B980
	public UserAudioSettings()
	{
		if (PlayerPrefs.HasKey("__soundEffects"))
		{
			this.soundEffectsVolume = PlayerPrefs.GetFloat("__soundEffects");
		}
		if (PlayerPrefs.HasKey("__musicKey"))
		{
			this.musicVolume = PlayerPrefs.GetFloat("__musicKey");
		}
		if (PlayerPrefs.HasKey("__altSoundsKey"))
		{
			this.useAltSounds = PlayerPrefs.GetInt("__altSoundsKey");
		}
		if (PlayerPrefs.HasKey("__altMenuMusicKey"))
		{
			this.useAltMenuMusic = PlayerPrefs.GetInt("__altMenuMusicKey");
		}
		if (PlayerPrefs.HasKey("__altBattleMusicKey"))
		{
			this.useAltBattleMusic = PlayerPrefs.GetInt("__altBattleMusicKey");
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0005D644 File Offset: 0x0005BA44
	public void Reset()
	{
		this.SetSoundEffects(0.375f);
		this.SetMusic(0.375f);
		this.SetUseAltSounds(false);
		this.SetUseAltMenuMusic(false);
		this.SetUseAltBattleMusic(false);
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0005D671 File Offset: 0x0005BA71
	public void SetSoundEffects(float value)
	{
		if (this.soundEffectsVolume == value)
		{
			return;
		}
		this.soundEffectsVolume = value;
		PlayerPrefs.SetFloat("__soundEffects", value);
		PlayerPrefs.Save();
		if (this.OnSoundEffectsVolumeUpdate != null)
		{
			this.OnSoundEffectsVolumeUpdate();
		}
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0005D6AD File Offset: 0x0005BAAD
	public void SetMusic(float value)
	{
		if (this.musicVolume == value)
		{
			return;
		}
		this.musicVolume = value;
		PlayerPrefs.SetFloat("__musicKey", value);
		PlayerPrefs.Save();
		if (this.OnMusicVolumeUpdate != null)
		{
			this.OnMusicVolumeUpdate();
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x0005D6E9 File Offset: 0x0005BAE9
	public void SetUseAltSounds(bool value)
	{
		this.useAltSounds = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altSoundsKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0005D71A File Offset: 0x0005BB1A
	public bool UseAltSounds()
	{
		return this.useAltSounds == 1;
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0005D725 File Offset: 0x0005BB25
	public void SetUseAltMenuMusic(bool value)
	{
		this.useAltMenuMusic = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altMenuMusicKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0005D756 File Offset: 0x0005BB56
	public bool UseAltMenuMusic()
	{
		return this.useAltMenuMusic == 1;
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0005D761 File Offset: 0x0005BB61
	public void SetUseAltBattleMusic(bool value)
	{
		this.useAltBattleMusic = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altBattleMusicKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0005D792 File Offset: 0x0005BB92
	public bool UseAltBattleMusic()
	{
		return this.useAltBattleMusic == 1;
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0005D79D File Offset: 0x0005BB9D
	public float GetSoundEffectsVolume()
	{
		return this.soundEffectsVolume;
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0005D7A5 File Offset: 0x0005BBA5
	public float GetMusicVolume()
	{
		return this.musicVolume;
	}

	// Token: 0x04000A0E RID: 2574
	public Action OnMusicVolumeUpdate;

	// Token: 0x04000A0F RID: 2575
	public Action OnSoundEffectsVolumeUpdate;

	// Token: 0x04000A10 RID: 2576
	private const string SOUND_EFFECTS_KEY = "__soundEffects";

	// Token: 0x04000A11 RID: 2577
	private const string MUSIC_KEY = "__musicKey";

	// Token: 0x04000A12 RID: 2578
	private const string ALT_SOUNDS_KEY = "__altSoundsKey";

	// Token: 0x04000A13 RID: 2579
	private const string ALT_MENU_MUSIC_KEY = "__altMenuMusicKey";

	// Token: 0x04000A14 RID: 2580
	private const string ALT_BATTLE_MUSIC_KEY = "__altBattleMusicKey";

	// Token: 0x04000A15 RID: 2581
	private const float SFX_DEFAULT = 0.375f;

	// Token: 0x04000A16 RID: 2582
	private const float MUSIC_DEFAULT = 0.375f;

	// Token: 0x04000A17 RID: 2583
	private float soundEffectsVolume = 0.375f;

	// Token: 0x04000A18 RID: 2584
	private float musicVolume = 0.375f;

	// Token: 0x04000A19 RID: 2585
	private int useAltSounds;

	// Token: 0x04000A1A RID: 2586
	private int useAltMenuMusic;

	// Token: 0x04000A1B RID: 2587
	private int useAltBattleMusic;
}
