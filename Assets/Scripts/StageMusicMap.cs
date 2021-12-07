// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class StageMusicMap
{
	private Dictionary<StageID, SoundKey> map = new Dictionary<StageID, SoundKey>
	{
		{
			StageID.CombatLab,
			SoundKey.combatLab_music
		},
		{
			StageID.ForbiddenShrine,
			SoundKey.forbiddenShrine_music
		},
		{
			StageID.MaluMalu,
			SoundKey.maluMalu_music
		},
		{
			StageID.WavedashArena,
			SoundKey.wavedashArena_music
		},
		{
			StageID.Zenith,
			SoundKey.cryoStation_music
		},
		{
			StageID.Shadowbriar,
			SoundKey.shadowbriar_music
		},
		{
			StageID.Cargo,
			SoundKey.cargo_music
		}
	};

	private Dictionary<StageID, SoundKey> altMap = new Dictionary<StageID, SoundKey>
	{
		{
			StageID.CombatLab,
			SoundKey.combatLabAlt_music
		},
		{
			StageID.ForbiddenShrine,
			SoundKey.forbiddenShrineAlt_music
		},
		{
			StageID.MaluMalu,
			SoundKey.maluMalu_music
		},
		{
			StageID.WavedashArena,
			SoundKey.wavedashArena_music
		},
		{
			StageID.Zenith,
			SoundKey.cryoStation_music
		},
		{
			StageID.Shadowbriar,
			SoundKey.shadowbriarAlt_music
		},
		{
			StageID.Cargo,
			SoundKey.cargo_music
		}
	};

	[Inject]
	public UserAudioSettings userAudioSettings
	{
		private get;
		set;
	}

	public SoundKey GetSoundKey(StageID stage)
	{
		SoundKey result = SoundKey.empty;
		Dictionary<StageID, SoundKey> dictionary;
		if (this.userAudioSettings.UseAltBattleMusic())
		{
			dictionary = this.altMap;
		}
		else
		{
			dictionary = this.map;
		}
		if (!dictionary.TryGetValue(stage, out result))
		{
			return SoundKey.wavedashArena_music;
		}
		return result;
	}
}
