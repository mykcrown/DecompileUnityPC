// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class TimeEndGameCondition : EndGameCondition
{
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();

	private List<PlayerReference> playerReferences;

	private TimeKeeper timeKeeper;

	private BattleSettings settings;

	private TimeEndGameConditionModel timeEndGameConditionModel;

	protected override IEndGameConditionModel Model
	{
		get
		{
			return this.timeEndGameConditionModel;
		}
	}

	public override float CurrentSeconds
	{
		get
		{
			return this.timeKeeper.CurrentSeconds;
		}
	}

	public bool ShouldDisplayTimer
	{
		get
		{
			return this.timeKeeper.ShouldDisplay;
		}
	}

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

	public override void Destroy()
	{
		if (this.timeKeeper != null)
		{
			this.timeKeeper.Destroy();
		}
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
	}

	private void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		this.adjustScore(characterDeathEvent.character.PlayerNum, -1);
		if (characterDeathEvent.character.Model.lastHitByPlayerNum != PlayerNum.None)
		{
			this.adjustScore(characterDeathEvent.character.Model.lastHitByPlayerNum, 1);
		}
	}

	private void adjustScore(PlayerNum player, int amount)
	{
		this.timeEndGameConditionModel.scores[(int)player] += amount;
	}

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

	private void stockModeWinners()
	{
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		Dictionary<TeamNum, int> dictionary2 = new Dictionary<TeamNum, int>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> current in this.teamPlayers)
		{
			TeamNum key = current.Key;
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
		foreach (TeamNum current2 in dictionary.Keys)
		{
			if (dictionary[current2] >= num)
			{
				if (dictionary[current2] > num)
				{
					item = current2;
					num = dictionary[current2];
					num2 = dictionary2[current2];
				}
				else if (dictionary2[current2] < num2)
				{
					item = current2;
					num = dictionary[current2];
					num2 = dictionary2[current2];
				}
			}
		}
		this.timeEndGameConditionModel.winningTeams.Add(item);
		foreach (TeamNum current3 in dictionary.Keys)
		{
			if (!this.timeEndGameConditionModel.winningTeams.Contains(current3) && dictionary2[current3] == num2 && dictionary[current3] == num)
			{
				this.timeEndGameConditionModel.winningTeams.Add(current3);
			}
		}
		if (this.timeEndGameConditionModel.winningTeams.Count != this.teamPlayers.Count)
		{
			this.addPlayersFromWinningTeams();
		}
	}

	private void timeModeWinners()
	{
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> current in this.teamPlayers)
		{
			TeamNum key = current.Key;
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
		foreach (TeamNum current2 in dictionary.Keys)
		{
			if (dictionary[current2] >= num)
			{
				item = current2;
				num = dictionary[current2];
			}
		}
		this.timeEndGameConditionModel.winningTeams.Add(item);
		foreach (TeamNum current3 in dictionary.Keys)
		{
			if (!this.timeEndGameConditionModel.winningTeams.Contains(current3) && dictionary[current3] == num)
			{
				this.timeEndGameConditionModel.winningTeams.Add(current3);
			}
		}
		if (this.timeEndGameConditionModel.winningTeams.Count != this.teamPlayers.Count)
		{
			this.addPlayersFromWinningTeams();
		}
	}

	private void addPlayersFromWinningTeams()
	{
		foreach (TeamNum current in this.timeEndGameConditionModel.winningTeams)
		{
			if (this.teamPlayers.ContainsKey(current))
			{
				for (int i = 0; i < this.teamPlayers[current].Count; i++)
				{
					PlayerReference playerReference = this.teamPlayers[current][i];
					this.timeEndGameConditionModel.victors.Add(playerReference.PlayerNum);
				}
			}
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<TimeEndGameConditionModel>(this.timeEndGameConditionModel));
		this.timeKeeper.ExportState(ref container);
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<TimeEndGameConditionModel>(ref this.timeEndGameConditionModel);
		this.timeKeeper.LoadState(container);
		return true;
	}
}
