// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class LivesEndGameCondition : EndGameCondition
{
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();

	private List<PlayerReference> playerReferences;

	private BattleSettings settings;

	public void Init(BattleSettings settings)
	{
		this.settings = settings;
		this.endGameConditionModel = new EndGameConditionModel();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Subscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
	}

	public override void Destroy()
	{
		base.events.Unsubscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
		base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
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

	private void onCharacterDeath(GameEvent message)
	{
		this.checkForWinners();
	}

	private void onPlayerDisconnectDespawn(GameEvent message)
	{
		this.checkForWinners();
	}

	private void checkForWinners()
	{
		if (this.settings.rules != GameRules.Stock || this.endGameConditionModel.IsFinished)
		{
			return;
		}
		List<TeamNum> list = new List<TeamNum>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> current in this.teamPlayers)
		{
			bool flag = false;
			for (int i = 0; i < current.Value.Count; i++)
			{
				PlayerReference playerReference = current.Value[i];
				if (playerReference.Lives > 0 && !playerReference.IsDisconnected)
				{
					list.Add(current.Key);
					break;
				}
			}
			if (flag)
			{
				list.Add(current.Key);
			}
		}
		if (list.Count == 1)
		{
			this.endGameConditionModel.IsFinished = true;
			for (int j = 0; j < this.teamPlayers[list[0]].Count; j++)
			{
				PlayerReference playerReference2 = this.teamPlayers[list[0]][j];
				this.endGameConditionModel.Victors.Add(playerReference2.PlayerNum);
			}
			this.endGameConditionModel.WinningTeams.Add(list[0]);
		}
		else if (list.Count == 0)
		{
			this.endGameConditionModel.IsFinished = true;
		}
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<EndGameConditionModel>(this.endGameConditionModel));
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<EndGameConditionModel>(ref this.endGameConditionModel);
		return true;
	}
}
