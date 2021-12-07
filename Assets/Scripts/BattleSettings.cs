// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class BattleSettings
{
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

	public bool isTrainingMode
	{
		get
		{
			return this.mode == GameMode.Training;
		}
	}

	public bool IsPauseEnabled
	{
		get
		{
			return this.pauseMode == PauseMode.Allowed;
		}
	}

	public BattleSettings Clone()
	{
		BattleSettings battleSettings = new BattleSettings();
		foreach (BattleSettingType current in this.settings.Keys)
		{
			battleSettings.settings[current] = this.settings[current];
		}
		return battleSettings;
	}

	public void Load(Dictionary<BattleSettingType, int> loadFrom)
	{
		foreach (BattleSettingType current in loadFrom.Keys)
		{
			this.settings[current] = loadFrom[current];
		}
	}
}
