// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MainOptionsCalculator : IMainOptionsCalculator
{
	private OptionDescription modeOpt = new OptionDescription();

	private OptionDescription rulesOpt = new OptionDescription();

	private OptionDescription stockOpt = new OptionDescription();

	private OptionDescription stockTimeOpt = new OptionDescription();

	private OptionDescription crewsStockOpt = new OptionDescription();

	private OptionDescription crewsTimeOpt = new OptionDescription();

	private OptionDescription timeOpt = new OptionDescription();

	private OptionDescription assistOpt = new OptionDescription();

	private OptionDescription assistStealOpt = new OptionDescription();

	private OptionDescription stageSelectOpt = new OptionDescription();

	private OptionDescription teamAttackOpt = new OptionDescription();

	private OptionDescription crewsTeamAttackOpt = new OptionDescription();

	private OptionDescription pauseOpt = new OptionDescription();

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public OptionValueDisplay displayHelper
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.modeOpt.type = BattleSettingType.Mode;
		this.modeOpt.width = 250f;
		foreach (GameModeData current in this.gameDataManager.GameModeData.DataList)
		{
			if (this.isModeEnabled(current))
			{
				this.modeOpt.valueList.Add((int)current.Type);
			}
		}
		this.modeOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.ModeValueDisplay);
		this.rulesOpt.type = BattleSettingType.Rules;
		this.rulesOpt.GenerateSimpleList(0, 1, 1);
		this.rulesOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.RulesValueDisplay);
		this.stockOpt.type = BattleSettingType.Lives;
		this.stockOpt.GenerateSimpleList(1, 99, 1);
		this.crewsStockOpt.type = BattleSettingType.CrewBattle_Lives;
		this.crewsStockOpt.GenerateSimpleList(1, 99, 1);
		this.stockTimeOpt.type = BattleSettingType.Time;
		this.stockTimeOpt.width = 180f;
		this.stockTimeOpt.GenerateSimpleList(0, 5940, 60);
		this.stockTimeOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.TimeValueDisplay);
		this.crewsTimeOpt.type = BattleSettingType.CrewBattle_Time;
		this.crewsTimeOpt.width = 180f;
		this.crewsTimeOpt.GenerateSimpleList(0, 5940, 60);
		this.crewsTimeOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.TimeValueDisplay);
		this.timeOpt.type = BattleSettingType.Time;
		this.timeOpt.width = 180f;
		this.timeOpt.GenerateSimpleList(60, 5940, 60);
		this.timeOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.TimeValueDisplay);
		this.assistOpt.type = BattleSettingType.Assists;
		this.assistOpt.GenerateSimpleList(0, 99, 1);
		this.assistStealOpt.type = BattleSettingType.AssistStealing;
		this.assistStealOpt.GenerateSimpleList(0, 1, 1);
		this.assistStealOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.AssistStealingValueDisplay);
		this.stageSelectOpt.type = BattleSettingType.StageSelect;
		this.stageSelectOpt.GenerateSimpleList(0, 3, 1);
		this.teamAttackOpt.type = BattleSettingType.TeamAttack;
		this.teamAttackOpt.GenerateSimpleList(0, 1, 1);
		this.teamAttackOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.TeamAttackValueDisplay);
		this.crewsTeamAttackOpt.type = BattleSettingType.CrewBattle_TeamAttack;
		this.crewsTeamAttackOpt.GenerateSimpleList(0, 1, 1);
		this.crewsTeamAttackOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.TeamAttackValueDisplay);
		this.pauseOpt.type = BattleSettingType.Pause;
		this.pauseOpt.GenerateSimpleList(0, 1, 1);
		this.pauseOpt.valueDisplayFunction = new Func<int, string>(this.displayHelper.PauseModeValueDisplay);
	}

	private bool isModeEnabled(GameModeData mode)
	{
		return !(mode == null) && mode.enabled && mode.selectableInFreePlay && (!mode.debugOnly || Debug.isDebugBuild) && (mode.demoEnabled || !this.gameDataManager.ConfigData.uiuxSettings.DemoModeEnabled);
	}

	public MainOptionsList GetLeftRightOptions(GameMode mode, GameRules rules)
	{
		MainOptionsList mainOptionsList = new MainOptionsList();
		if (mode != GameMode.Training)
		{
			mainOptionsList.RightSide.Add(this.modeOpt);
			if (mode == GameMode.CrewBattle)
			{
				mainOptionsList.LeftSide.Add(this.crewsStockOpt);
				mainOptionsList.LeftSide.Add(this.crewsTimeOpt);
			}
			else if (rules == GameRules.Stock)
			{
				mainOptionsList.LeftSide.Add(this.stockOpt);
				mainOptionsList.LeftSide.Add(this.stockTimeOpt);
			}
			else
			{
				mainOptionsList.LeftSide.Add(this.timeOpt);
			}
		}
		return mainOptionsList;
	}

	public MoreOptionsList GetAllOptions(GameMode mode, GameRules rules)
	{
		MoreOptionsList moreOptionsList = new MoreOptionsList();
		if (mode == GameMode.FreeForAll || mode == GameMode.Teams || mode == GameMode.Testing)
		{
			moreOptionsList.All.Add(this.modeOpt);
			moreOptionsList.All.Add(this.rulesOpt);
			if (rules == GameRules.Stock)
			{
				moreOptionsList.All.Add(this.stockOpt);
				moreOptionsList.All.Add(this.stockTimeOpt);
			}
			else
			{
				moreOptionsList.All.Add(this.timeOpt);
			}
			if (mode != GameMode.FreeForAll)
			{
				moreOptionsList.All.Add(this.teamAttackOpt);
			}
			moreOptionsList.All.Add(this.pauseOpt);
		}
		else if (mode == GameMode.CrewBattle)
		{
			moreOptionsList.All.Add(this.modeOpt);
			moreOptionsList.All.Add(this.assistOpt);
			moreOptionsList.All.Add(this.assistStealOpt);
			moreOptionsList.All.Add(this.crewsStockOpt);
			moreOptionsList.All.Add(this.crewsTimeOpt);
			moreOptionsList.All.Add(this.crewsTeamAttackOpt);
			moreOptionsList.All.Add(this.pauseOpt);
		}
		return moreOptionsList;
	}
}
