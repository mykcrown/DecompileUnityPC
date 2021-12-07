// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class UserAudioSettings
{
	public Action OnMusicVolumeUpdate;

	public Action OnSoundEffectsVolumeUpdate;

	private const string SOUND_EFFECTS_KEY = "__soundEffects";

	private const string MUSIC_KEY = "__musicKey";

	private const string ALT_SOUNDS_KEY = "__altSoundsKey";

	private const string ALT_MENU_MUSIC_KEY = "__altMenuMusicKey";

	private const string ALT_BATTLE_MUSIC_KEY = "__altBattleMusicKey";

	private const float SFX_DEFAULT = 0.375f;

	private const float MUSIC_DEFAULT = 0.375f;

	private float soundEffectsVolume = 0.375f;

	private float musicVolume = 0.375f;

	private int useAltSounds;

	private int useAltMenuMusic;

	private int useAltBattleMusic;

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

	public void Reset()
	{
		this.SetSoundEffects(0.375f);
		this.SetMusic(0.375f);
		this.SetUseAltSounds(false);
		this.SetUseAltMenuMusic(false);
		this.SetUseAltBattleMusic(false);
	}

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

	public void SetUseAltSounds(bool value)
	{
		this.useAltSounds = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altSoundsKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public bool UseAltSounds()
	{
		return this.useAltSounds == 1;
	}

	public void SetUseAltMenuMusic(bool value)
	{
		this.useAltMenuMusic = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altMenuMusicKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public bool UseAltMenuMusic()
	{
		return this.useAltMenuMusic == 1;
	}

	public void SetUseAltBattleMusic(bool value)
	{
		this.useAltBattleMusic = ((!value) ? 0 : 1);
		PlayerPrefs.SetInt("__altBattleMusicKey", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public bool UseAltBattleMusic()
	{
		return this.useAltBattleMusic == 1;
	}

	public float GetSoundEffectsVolume()
	{
		return this.soundEffectsVolume;
	}

	public float GetMusicVolume()
	{
		return this.musicVolume;
	}
}
