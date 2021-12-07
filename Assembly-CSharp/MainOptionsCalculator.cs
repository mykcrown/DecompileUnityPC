using System;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
public class MainOptionsCalculator : IMainOptionsCalculator
{
	// Token: 0x17000E08 RID: 3592
	// (get) Token: 0x06003A54 RID: 14932 RVA: 0x0011121E File Offset: 0x0010F61E
	// (set) Token: 0x06003A55 RID: 14933 RVA: 0x00111226 File Offset: 0x0010F626
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x06003A56 RID: 14934 RVA: 0x0011122F File Offset: 0x0010F62F
	// (set) Token: 0x06003A57 RID: 14935 RVA: 0x00111237 File Offset: 0x0010F637
	[Inject]
	public OptionValueDisplay displayHelper { get; set; }

	// Token: 0x06003A58 RID: 14936 RVA: 0x00111240 File Offset: 0x0010F640
	[PostConstruct]
	public void Init()
	{
		this.modeOpt.type = BattleSettingType.Mode;
		this.modeOpt.width = 250f;
		foreach (GameModeData gameModeData in this.gameDataManager.GameModeData.DataList)
		{
			if (this.isModeEnabled(gameModeData))
			{
				this.modeOpt.valueList.Add((int)gameModeData.Type);
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

	// Token: 0x06003A59 RID: 14937 RVA: 0x0011155C File Offset: 0x0010F95C
	private bool isModeEnabled(GameModeData mode)
	{
		return !(mode == null) && mode.enabled && mode.selectableInFreePlay && (!mode.debugOnly || Debug.isDebugBuild) && (mode.demoEnabled || !this.gameDataManager.ConfigData.uiuxSettings.DemoModeEnabled);
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x001115D0 File Offset: 0x0010F9D0
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

	// Token: 0x06003A5B RID: 14939 RVA: 0x00111670 File Offset: 0x0010FA70
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

	// Token: 0x04002814 RID: 10260
	private OptionDescription modeOpt = new OptionDescription();

	// Token: 0x04002815 RID: 10261
	private OptionDescription rulesOpt = new OptionDescription();

	// Token: 0x04002816 RID: 10262
	private OptionDescription stockOpt = new OptionDescription();

	// Token: 0x04002817 RID: 10263
	private OptionDescription stockTimeOpt = new OptionDescription();

	// Token: 0x04002818 RID: 10264
	private OptionDescription crewsStockOpt = new OptionDescription();

	// Token: 0x04002819 RID: 10265
	private OptionDescription crewsTimeOpt = new OptionDescription();

	// Token: 0x0400281A RID: 10266
	private OptionDescription timeOpt = new OptionDescription();

	// Token: 0x0400281B RID: 10267
	private OptionDescription assistOpt = new OptionDescription();

	// Token: 0x0400281C RID: 10268
	private OptionDescription assistStealOpt = new OptionDescription();

	// Token: 0x0400281D RID: 10269
	private OptionDescription stageSelectOpt = new OptionDescription();

	// Token: 0x0400281E RID: 10270
	private OptionDescription teamAttackOpt = new OptionDescription();

	// Token: 0x0400281F RID: 10271
	private OptionDescription crewsTeamAttackOpt = new OptionDescription();

	// Token: 0x04002820 RID: 10272
	private OptionDescription pauseOpt = new OptionDescription();
}
