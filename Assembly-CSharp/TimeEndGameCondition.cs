using System;
using System.Collections.Generic;

// Token: 0x0200067C RID: 1660
public class TimeEndGameCondition : EndGameCondition
{
	// Token: 0x06002905 RID: 10501 RVA: 0x000C6228 File Offset: 0x000C4628
	public void Init(BattleSettings settings)
	{
		this.settings = settings;
		this.timeEndGameConditionModel = new TimeEndGameConditionModel();
		int totalSeconds = (settings.mode != GameMode.CrewBattle) ? settings.durationSeconds : settings.crewsBattle_durationSeconds;
		this.timeKeeper = new TimeKeeper();
		base.injector.Inject(this.timeKeeper);
		this.timeKeeper.Init(totalSeconds);
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06002906 RID: 10502 RVA: 0x000C62CF File Offset: 0x000C46CF
	protected override IEndGameConditionModel Model
	{
		get
		{
			return this.timeEndGameConditionModel;
		}
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000C62D8 File Offset: 0x000C46D8
	public override void Destroy()
	{
		if (this.timeKeeper != null)
		{
			this.timeKeeper.Destroy();
		}
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x000C6340 File Offset: 0x000C4740
	private void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		this.adjustScore(characterDeathEvent.character.PlayerNum, -1);
		if (characterDeathEvent.character.Model.lastHitByPlayerNum != PlayerNum.None)
		{
			this.adjustScore(characterDeathEvent.character.Model.lastHitByPlayerNum, 1);
		}
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x000C6394 File Offset: 0x000C4794
	private void adjustScore(PlayerNum player, int amount)
	{
		this.timeEndGameConditionModel.scores[(int)player] += amount;
	}

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x0600290A RID: 10506 RVA: 0x000C63B9 File Offset: 0x000C47B9
	public override float CurrentSeconds
	{
		get
		{
			return this.timeKeeper.CurrentSeconds;
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x0600290B RID: 10507 RVA: 0x000C63C6 File Offset: 0x000C47C6
	public bool ShouldDisplayTimer
	{
		get
		{
			return this.timeKeeper.ShouldDisplay;
		}
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000C63D4 File Offset: 0x000C47D4
	private void onGameInit(GameEvent message)
	{
		GameInitEvent gameInitEvent = message as GameInitEvent;
		this.playerReferences = gameInitEvent.gameManager.PlayerReferences;
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			PlayerReference playerReference = this.playerReferences[i];
			if (!playerReference.IsSpectating)
			{
				if (!this.teamPlayers.ContainsKey(playerReference.Team))
				{
					this.teamPlayers.Add(playerReference.Team, new List<PlayerReference>());
				}
				this.teamPlayers[playerReference.Team].Add(playerReference);
			}
		}
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x000C6470 File Offset: 0x000C4870
	public override void TickFrame()
	{
		if (this.timeKeeper != null)
		{
			this.timeKeeper.TickFrame();
			if (this.timeKeeper.TotalSeconds > 0 && this.timeKeeper.SecondsElapsed >= (float)this.timeKeeper.TotalSeconds)
			{
				this.timeEndGameConditionModel.isFinished = true;
				this.determineWinners();
			}
		}
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000C64D2 File Offset: 0x000C48D2
	private void determineWinners()
	{
		if (this.settings.rules == GameRules.Stock)
		{
			this.stockModeWinners();
		}
		else
		{
			this.timeModeWinners();
		}
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x000C64F8 File Offset: 0x000C48F8
	private void stockModeWinners()
	{
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		Dictionary<TeamNum, int> dictionary2 = new Dictionary<TeamNum, int>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> keyValuePair in this.teamPlayers)
		{
			TeamNum key = keyValuePair.Key;
			dictionary[key] = 0;
			dictionary2[key] = 0;
			for (int i = 0; i < this.teamPlayers[key].Count; i++)
			{
				PlayerReference playerReference = this.teamPlayers[key][i];
				Dictionary<TeamNum, int> dictionary3;
				TeamNum key2;
				(dictionary3 = dictionary)[key2 = key] = dictionary3[key2] + playerReference.Lives;
				TeamNum key3;
				(dictionary3 = dictionary2)[key3 = key] = dictionary3[key3] + (int)playerReference.Controller.CurrentDamage;
			}
		}
		int num = -999999;
		int num2 = 9999999;
		TeamNum item = TeamNum.None;
		foreach (TeamNum teamNum in dictionary.Keys)
		{
			if (dictionary[teamNum] >= num)
			{
				if (dictionary[teamNum] > num)
				{
					item = teamNum;
					num = dictionary[teamNum];
					num2 = dictionary2[teamNum];
				}
				else if (dictionary2[teamNum] < num2)
				{
					item = teamNum;
					num = dictionary[teamNum];
					num2 = dictionary2[teamNum];
				}
			}
		}
		this.timeEndGameConditionModel.winningTeams.Add(item);
		foreach (TeamNum teamNum2 in dictionary.Keys)
		{
			if (!this.timeEndGameConditionModel.winningTeams.Contains(teamNum2) && dictionary2[teamNum2] == num2 && dictionary[teamNum2] == num)
			{
				this.timeEndGameConditionModel.winningTeams.Add(teamNum2);
			}
		}
		if (this.timeEndGameConditionModel.winningTeams.Count != this.teamPlayers.Count)
		{
			this.addPlayersFromWinningTeams();
		}
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000C6774 File Offset: 0x000C4B74
	private void timeModeWinners()
	{
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> keyValuePair in this.teamPlayers)
		{
			TeamNum key = keyValuePair.Key;
			dictionary[key] = 0;
			for (int i = 0; i < this.teamPlayers[key].Count; i++)
			{
				PlayerReference playerReference = this.teamPlayers[key][i];
				Dictionary<TeamNum, int> dictionary2;
				TeamNum key2;
				(dictionary2 = dictionary)[key2 = key] = dictionary2[key2] + this.timeEndGameConditionModel.scores[(int)playerReference.PlayerNum];
			}
		}
		int num = -999999;
		TeamNum item = TeamNum.None;
		foreach (TeamNum teamNum in dictionary.Keys)
		{
			if (dictionary[teamNum] >= num)
			{
				item = teamNum;
				num = dictionary[teamNum];
			}
		}
		this.timeEndGameConditionModel.winningTeams.Add(item);
		foreach (TeamNum teamNum2 in dictionary.Keys)
		{
			if (!this.timeEndGameConditionModel.winningTeams.Contains(teamNum2) && dictionary[teamNum2] == num)
			{
				this.timeEndGameConditionModel.winningTeams.Add(teamNum2);
			}
		}
		if (this.timeEndGameConditionModel.winningTeams.Count != this.teamPlayers.Count)
		{
			this.addPlayersFromWinningTeams();
		}
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000C6968 File Offset: 0x000C4D68
	private void addPlayersFromWinningTeams()
	{
		foreach (TeamNum key in this.timeEndGameConditionModel.winningTeams)
		{
			if (this.teamPlayers.ContainsKey(key))
			{
				for (int i = 0; i < this.teamPlayers[key].Count; i++)
				{
					PlayerReference playerReference = this.teamPlayers[key][i];
					this.timeEndGameConditionModel.victors.Add(playerReference.PlayerNum);
				}
			}
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000C6A20 File Offset: 0x000C4E20
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<TimeEndGameConditionModel>(this.timeEndGameConditionModel));
		this.timeKeeper.ExportState(ref container);
		return true;
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000C6A49 File Offset: 0x000C4E49
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TimeEndGameConditionModel>(ref this.timeEndGameConditionModel);
		this.timeKeeper.LoadState(container);
		return true;
	}

	// Token: 0x04001DB9 RID: 7609
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();

	// Token: 0x04001DBA RID: 7610
	private List<PlayerReference> playerReferences;

	// Token: 0x04001DBB RID: 7611
	private TimeKeeper timeKeeper;

	// Token: 0x04001DBC RID: 7612
	private BattleSettings settings;

	// Token: 0x04001DBD RID: 7613
	private TimeEndGameConditionModel timeEndGameConditionModel;
}
