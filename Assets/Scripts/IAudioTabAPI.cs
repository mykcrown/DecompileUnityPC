// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IAudioTabAPI
{
	float MasterVolume
	{
		get;
		set;
	}

	float SoundsEffectsVolume
	{
		get;
		set;
	}

	float MusicVolume
	{
		get;
		set;
	}

	bool UseAltSounds
	{
		get;
		set;
	}

	bool UseAltMenuMusic
	{
		get;
		set;
	}

	bool UseAltBattleMusic
	{
		get;
		set;
	}

	float CharacterAnnouncerVolume
	{
		get;
		set;
	}

	void Reset();
}
