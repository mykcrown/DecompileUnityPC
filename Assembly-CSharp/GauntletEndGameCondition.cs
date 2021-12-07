using System;
using System.Collections.Generic;

// Token: 0x02000674 RID: 1652
public class GauntletEndGameCondition : EndGameCondition
{
	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000C56D3 File Offset: 0x000C3AD3
	// (set) Token: 0x060028D8 RID: 10456 RVA: 0x000C56DB File Offset: 0x000C3ADB
	public bool WaitingForSpawn { get; set; }

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x060028D9 RID: 10457 RVA: 0x000C56E4 File Offset: 0x000C3AE4
	public TeamNum CurrentTeam
	{
		get
		{
			return this.initialVictor;
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000C56EC File Offset: 0x000C3AEC
	public void Init(BattleSettings settings)
	{
		this.WaitingForSpawn = false;
		this.RoundCount = 1;
		this.initialVictor = TeamNum.None;
		this.endGameConditionModel = new EndGameConditionModel();
		base.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		base.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Subscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000C5780 File Offset: 0x000C3B80
	public override void Destroy()
	{
		base.events.Unsubscribe(typeof(PlayerDisconnectDespawnEvent), new Events.EventHandler(this.onPlayerDisconnectDespawn));
		base.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		base.events.Unsubscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000C57F0 File Offset: 0x000C3BF0
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

	// Token: 0x060028DD RID: 10461 RVA: 0x000C588C File Offset: 0x000C3C8C
	private void onCharacterDeath(GameEvent message)
	{
		this.checkForWinners();
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000C5894 File Offset: 0x000C3C94
	private void onPlayerDisconnectDespawn(GameEvent message)
	{
		this.checkForWinners();
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x000C589C File Offset: 0x000C3C9C
	private void checkForWinners()
	{
		if (this.endGameConditionModel.IsFinished)
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
			if (!this.WaitingForSpawn)
			{
				if (this.initialVictor == TeamNum.None)
				{
					this.initialVictor = list[0];
					this.newRound();
				}
				else if (list[0] == this.initialVictor)
				{
					this.newRound();
				}
				else
				{
					this.endGameConditionModel.IsFinished = true;
					for (int j = 0; j < this.teamPlayers[this.initialVictor].Count; j++)
					{
						PlayerReference playerReference2 = this.teamPlayers[this.initialVictor][j];
						this.endGameConditionModel.Victors.Add(playerReference2.PlayerNum);
					}
					this.endGameConditionModel.WinningTeams.Add(this.initialVictor);
				}
			}
		}
		else if (list.Count == 0)
		{
			this.endGameConditionModel.IsFinished = true;
		}
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000C5A6C File Offset: 0x000C3E6C
	private void newRound()
	{
		this.RoundCount++;
		this.syncRoundCountStat();
		this.WaitingForSpawn = true;
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000C5A8C File Offset: 0x000C3E8C
	private void syncRoundCountStat()
	{
		for (int i = 0; i < this.teamPlayers[this.initialVictor].Count; i++)
		{
			PlayerReference playerReference = this.teamPlayers[this.initialVictor][i];
			base.events.Broadcast(new LogStatEvent(StatType.MaxRound, this.RoundCount, PointsValueType.Raw, playerReference.PlayerNum, this.initialVictor));
		}
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000C5AFD File Offset: 0x000C3EFD
	public override bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(base.rollbackStatePooling.Clone<EndGameConditionModel>(this.endGameConditionModel));
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000C5B17 File Offset: 0x000C3F17
	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<EndGameConditionModel>(ref this.endGameConditionModel);
		return true;
	}

	// Token: 0x04001D93 RID: 7571
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();

	// Token: 0x04001D94 RID: 7572
	private List<PlayerReference> playerReferences;

	// Token: 0x04001D95 RID: 7573
	private TeamNum initialVictor = TeamNum.None;

	// Token: 0x04001D96 RID: 7574
	public int RoundCount = 1;
}
