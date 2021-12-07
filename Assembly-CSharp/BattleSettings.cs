using System;
using System.Collections.Generic;

// Token: 0x020003E2 RID: 994
[Serializable]
public class BattleSettings
{
	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06001560 RID: 5472 RVA: 0x00076267 File Offset: 0x00074667
	// (set) Token: 0x06001561 RID: 5473 RVA: 0x00076275 File Offset: 0x00074675
	public GameMode mode
	{
		get
		{
			return (GameMode)this.settings[BattleSettingType.Mode];
		}
		set
		{
			this.settings[BattleSettingType.Mode] = (int)value;
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06001562 RID: 5474 RVA: 0x00076284 File Offset: 0x00074684
	// (set) Token: 0x06001563 RID: 5475 RVA: 0x00076292 File Offset: 0x00074692
	public GameRules rules
	{
		get
		{
			return (GameRules)this.settings[BattleSettingType.Rules];
		}
		set
		{
			this.settings[BattleSettingType.Rules] = (int)value;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001564 RID: 5476 RVA: 0x000762A1 File Offset: 0x000746A1
	// (set) Token: 0x06001565 RID: 5477 RVA: 0x000762B0 File Offset: 0x000746B0
	public PauseMode pauseMode
	{
		get
		{
			return (PauseMode)this.settings[BattleSettingType.Pause];
		}
		set
		{
			this.settings[BattleSettingType.Pause] = (int)value;
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06001566 RID: 5478 RVA: 0x000762C0 File Offset: 0x000746C0
	// (set) Token: 0x06001567 RID: 5479 RVA: 0x000762CE File Offset: 0x000746CE
	public int durationSeconds
	{
		get
		{
			return this.settings[BattleSettingType.Time];
		}
		set
		{
			this.settings[BattleSettingType.Time] = value;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06001568 RID: 5480 RVA: 0x000762DD File Offset: 0x000746DD
	// (set) Token: 0x06001569 RID: 5481 RVA: 0x000762EC File Offset: 0x000746EC
	public int crewsBattle_durationSeconds
	{
		get
		{
			return this.settings[BattleSettingType.CrewBattle_Time];
		}
		set
		{
			this.settings[BattleSettingType.CrewBattle_Time] = value;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x0600156A RID: 5482 RVA: 0x000762FC File Offset: 0x000746FC
	// (set) Token: 0x0600156B RID: 5483 RVA: 0x0007630A File Offset: 0x0007470A
	public int lives
	{
		get
		{
			return this.settings[BattleSettingType.Lives];
		}
		set
		{
			this.settings[BattleSettingType.Lives] = value;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x0600156C RID: 5484 RVA: 0x00076319 File Offset: 0x00074719
	// (set) Token: 0x0600156D RID: 5485 RVA: 0x00076328 File Offset: 0x00074728
	public int crewBattle_lives
	{
		get
		{
			return this.settings[BattleSettingType.CrewBattle_Lives];
		}
		set
		{
			this.settings[BattleSettingType.CrewBattle_Lives] = value;
		}
	}

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x0600156E RID: 5486 RVA: 0x00076338 File Offset: 0x00074738
	// (set) Token: 0x0600156F RID: 5487 RVA: 0x00076352 File Offset: 0x00074752
	public bool teamAttack
	{
		get
		{
			return this.settings[BattleSettingType.TeamAttack] != 0;
		}
		set
		{
			this.settings[BattleSettingType.TeamAttack] = ((!value) ? 0 : 1);
		}
	}

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x06001570 RID: 5488 RVA: 0x0007636D File Offset: 0x0007476D
	// (set) Token: 0x06001571 RID: 5489 RVA: 0x00076388 File Offset: 0x00074788
	public bool crewBattle_teamAttack
	{
		get
		{
			return this.settings[BattleSettingType.CrewBattle_TeamAttack] != 0;
		}
		set
		{
			this.settings[BattleSettingType.CrewBattle_TeamAttack] = ((!value) ? 0 : 1);
		}
	}

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x06001572 RID: 5490 RVA: 0x000763A4 File Offset: 0x000747A4
	// (set) Token: 0x06001573 RID: 5491 RVA: 0x000763BE File Offset: 0x000747BE
	public bool crewBattle_assistSteal
	{
		get
		{
			return this.settings[BattleSettingType.AssistStealing] != 0;
		}
		set
		{
			this.settings[BattleSettingType.AssistStealing] = ((!value) ? 0 : 1);
		}
	}

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06001574 RID: 5492 RVA: 0x000763D9 File Offset: 0x000747D9
	// (set) Token: 0x06001575 RID: 5493 RVA: 0x000763E7 File Offset: 0x000747E7
	public int assists
	{
		get
		{
			return this.settings[BattleSettingType.Assists];
		}
		set
		{
			this.settings[BattleSettingType.Assists] = value;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06001576 RID: 5494 RVA: 0x000763F6 File Offset: 0x000747F6
	public bool isTeamAttack
	{
		get
		{
			if (this.mode == GameMode.CrewBattle)
			{
				return this.crewBattle_teamAttack;
			}
			return this.teamAttack;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06001577 RID: 5495 RVA: 0x00076411 File Offset: 0x00074811
	public bool isTrainingMode
	{
		get
		{
			return this.mode == GameMode.Training;
		}
	}

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06001578 RID: 5496 RVA: 0x0007641C File Offset: 0x0007481C
	public bool IsPauseEnabled
	{
		get
		{
			return this.pauseMode == PauseMode.Allowed;
		}
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x00076428 File Offset: 0x00074828
	public BattleSettings Clone()
	{
		BattleSettings battleSettings = new BattleSettings();
		foreach (BattleSettingType key in this.settings.Keys)
		{
			battleSettings.settings[key] = this.settings[key];
		}
		return battleSettings;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x000764A4 File Offset: 0x000748A4
	public void Load(Dictionary<BattleSettingType, int> loadFrom)
	{
		foreach (BattleSettingType key in loadFrom.Keys)
		{
			this.settings[key] = loadFrom[key];
		}
	}

	// Token: 0x04000F26 RID: 3878
	public Dictionary<BattleSettingType, int> settings = new Dictionary<BattleSettingType, int>(default(BattleSettingTypeComparer))
	{
		{
			BattleSettingType.Mode,
			1
		},
		{
			BattleSettingType.Rules,
			0
		},
		{
			BattleSettingType.Lives,
			4
		},
		{
			BattleSettingType.Points,
			5
		},
		{
			BattleSettingType.TeamAttack,
			1
		},
		{
			BattleSettingType.CrewBattle_TeamAttack,
			0
		},
		{
			BattleSettingType.Teams,
			0
		},
		{
			BattleSettingType.Time,
			480
		},
		{
			BattleSettingType.CrewBattle_Time,
			900
		},
		{
			BattleSettingType.Assists,
			1
		},
		{
			BattleSettingType.AssistStealing,
			1
		},
		{
			BattleSettingType.StageSelect,
			0
		},
		{
			BattleSettingType.Pause,
			0
		},
		{
			BattleSettingType.CrewBattle_Lives,
			2
		}
	};
}
