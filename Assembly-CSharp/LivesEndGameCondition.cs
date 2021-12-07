using System;
using System.Collections.Generic;

// Token: 0x02000675 RID: 1653
public class LivesEndGameCondition : EndGameCondition
{
	// Token: 0x060028E5 RID: 10469 RVA: 0x000C5B3C File Offset: 0x000C3F3C
	public void Init(BattleSettings settings)
	{
		this.settings = settings;
		this.endGameConditionModel = new EndGameConditionModel();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Subscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x000C5BC0 File Offset: 0x000C3FC0
	public override void Destroy()
	{
		base.events.Unsubscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
		base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000C5C30 File Offset: 0x000C4030
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

	// Token: 0x060028E8 RID: 10472 RVA: 0x000C5CCC File Offset: 0x000C40CC
	private void onCharacterDeath(GameEvent message)
	{
		this.checkForWinners();
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000C5CD4 File Offset: 0x000C40D4
	private void onPlayerDisconnectDespawn(GameEvent message)
	{
		this.checkForWinners();
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000C5CDC File Offset: 0x000C40DC
	private void checkForWinners()
	{
		if (this.settings.rules != GameRules.Stock || this.endGameConditionModel.IsFinished)
		{
			return;
		}
		List<TeamNum> list = new List<TeamNum>();
		foreach (KeyValuePair<TeamNum, List<PlayerReference>> keyValuePair in this.teamPlayers)
		{
			bool flag = false;
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				PlayerReference playerReference = keyValuePair.Value[i];
				if (playerReference.Lives > 0 && !playerReference.IsDisconnected)
				{
					list.Add(keyValuePair.Key);
					break;
				}
			}
			if (flag)
			{
				list.Add(keyValuePair.Key);
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

	// Token: 0x060028EB RID: 10475 RVA: 0x000C5E74 File Offset: 0x000C4274
	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<EndGameConditionModel>(this.endGameConditionModel));
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000C5E8E File Offset: 0x000C428E
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<EndGameConditionModel>(ref this.endGameConditionModel);
		return true;
	}

	// Token: 0x04001D98 RID: 7576
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();

	// Token: 0x04001D99 RID: 7577
	private List<PlayerReference> playerReferences;

	// Token: 0x04001D9A RID: 7578
	private BattleSettings settings;
}
