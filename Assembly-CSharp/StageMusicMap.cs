using System;
using System.Collections.Generic;

// Token: 0x02000643 RID: 1603
public class StageMusicMap
{
	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x06002746 RID: 10054 RVA: 0x000BF9D3 File Offset: 0x000BDDD3
	// (set) Token: 0x06002747 RID: 10055 RVA: 0x000BF9DB File Offset: 0x000BDDDB
	[Inject]
	public UserAudioSettings userAudioSettings { private get; set; }

	// Token: 0x06002748 RID: 10056 RVA: 0x000BF9E4 File Offset: 0x000BDDE4
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

	// Token: 0x04001CCE RID: 7374
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

	// Token: 0x04001CCF RID: 7375
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
}
