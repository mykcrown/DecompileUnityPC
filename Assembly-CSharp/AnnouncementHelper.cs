using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200034F RID: 847
public class AnnouncementHelper : ITickable, IRollbackStateOwner
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x060011D6 RID: 4566 RVA: 0x00066C2A File Offset: 0x0006502A
	// (set) Token: 0x060011D7 RID: 4567 RVA: 0x00066C32 File Offset: 0x00065032
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060011D8 RID: 4568 RVA: 0x00066C3B File Offset: 0x0006503B
	// (set) Token: 0x060011D9 RID: 4569 RVA: 0x00066C43 File Offset: 0x00065043
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060011DA RID: 4570 RVA: 0x00066C4C File Offset: 0x0006504C
	// (set) Token: 0x060011DB RID: 4571 RVA: 0x00066C54 File Offset: 0x00065054
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060011DC RID: 4572 RVA: 0x00066C5D File Offset: 0x0006505D
	// (set) Token: 0x060011DD RID: 4573 RVA: 0x00066C65 File Offset: 0x00065065
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060011DE RID: 4574 RVA: 0x00066C6E File Offset: 0x0006506E
	// (set) Token: 0x060011DF RID: 4575 RVA: 0x00066C76 File Offset: 0x00065076
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x060011E0 RID: 4576 RVA: 0x00066C80 File Offset: 0x00065080
	public void Init()
	{
		this.data = this.config.announcements;
		this.state = new AnnouncementHelper.AnnouncementHelperState();
		if (!this.data.AnnouncementsEnabled)
		{
			return;
		}
		this.statTrackerMap = new Dictionary<StatType, List<AnnouncementStatTracker>>(default(StatTypeComparer));
		this.announcementDataMap = new Dictionary<string, List<AnnouncementData>>();
		this.teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>(default(TeamNumComparer));
		this.LoadData(this.data);
		this.events.Subscribe(typeof(GameInitEvent), new Events.EventHandler(this.onGameInit));
		this.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		this.events.Subscribe(typeof(LogStatEvent), new Events.EventHandler(this.onStatLogged));
		this.events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.events.Subscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onCharacterSpawn));
		this.events.Subscribe(typeof(PlayAnnouncementCommand), new Events.EventHandler(this.onPlayAnnouncementCommand));
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x00066DCC File Offset: 0x000651CC
	public void Destroy()
	{
		if (this.statTrackerMap != null)
		{
			this.statTrackerMap.Clear();
		}
		if (this.announcementDataMap != null)
		{
			this.announcementDataMap.Clear();
		}
		this.events.Unsubscribe(typeof(LogStatEvent), new Events.EventHandler(this.onStatLogged));
		this.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.events.Unsubscribe(typeof(CharacterSpawnCommand), new Events.EventHandler(this.onCharacterSpawn));
		this.events.Unsubscribe(typeof(PlayAnnouncementCommand), new Events.EventHandler(this.onPlayAnnouncementCommand));
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x00066E8C File Offset: 0x0006528C
	private void onGameInit(GameEvent message)
	{
		this.allPlayers = this.gameController.currentGame.PlayerReferences;
		this.modeData = this.gameController.currentGame.ModeData;
		this.teamPlayers.Clear();
		for (int i = 0; i < this.allPlayers.Count; i++)
		{
			PlayerReference playerReference = this.allPlayers[i];
			if (!this.teamPlayers.ContainsKey(playerReference.Team))
			{
				this.teamPlayers[playerReference.Team] = new List<PlayerReference>();
			}
			this.teamPlayers[playerReference.Team].Add(playerReference);
		}
		this.LoadData(this.data);
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00066F48 File Offset: 0x00065348
	private void onGameEnd(GameEvent message)
	{
		this.allPlayers = null;
		this.teamPlayers.Clear();
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x00066F5C File Offset: 0x0006535C
	public void LoadData(AnnouncementConfigData announcements)
	{
		this.announcementDataMap.Clear();
		for (int i = 0; i < announcements.AnnouncementDataBank.Count; i++)
		{
			string first = announcements.AnnouncementDataBank[i].First;
			List<AnnouncementData> second = announcements.AnnouncementDataBank[i].Second;
			this.announcementDataMap.Add(first, second);
		}
		this.statTrackerMap.Clear();
		if (this.allPlayers != null && this.allPlayers.Count > 0)
		{
			for (int j = 0; j < announcements.AnnouncementStats.Count; j++)
			{
				AnnouncementStatData announcementStatData = announcements.AnnouncementStats[j];
				if (!this.statTrackerMap.ContainsKey(announcementStatData.stat))
				{
					this.statTrackerMap.Add(announcementStatData.stat, new List<AnnouncementStatTracker>());
				}
				if (announcementStatData.perPlayer)
				{
					for (int k = 0; k < this.allPlayers.Count; k++)
					{
						PlayerNum playerNum = this.allPlayers[k].PlayerNum;
						AnnouncementStatTracker announcementStatTracker = new AnnouncementStatTracker();
						this.injector.Inject(announcementStatTracker);
						announcementStatTracker.Init(announcementStatData, new Action<string>(this.receiveAnnouncement), playerNum);
						this.statTrackerMap[announcementStatData.stat].Add(announcementStatTracker);
					}
				}
				else
				{
					AnnouncementStatTracker announcementStatTracker2 = new AnnouncementStatTracker();
					this.injector.Inject(announcementStatTracker2);
					announcementStatTracker2.Init(announcementStatData, new Action<string>(this.receiveAnnouncement), PlayerNum.All);
					this.statTrackerMap[announcementStatData.stat].Add(announcementStatTracker2);
				}
			}
		}
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x0006710C File Offset: 0x0006550C
	public void TickFrame()
	{
		if (!this.data.AnnouncementsEnabled)
		{
			return;
		}
		AnnouncementData announcementData = null;
		Priority priority = Priority.None;
		for (int i = 0; i < this.state.announcementsTriggeredThisFrame.Count; i++)
		{
			AnnouncementData announcementData2 = this.state.announcementsTriggeredThisFrame[i];
			if (announcementData2 != null)
			{
				int priority2 = (int)announcementData2.priority;
				if (priority2 > (int)priority)
				{
					priority = announcementData2.priority;
					announcementData = announcementData2;
				}
			}
		}
		if (announcementData != null)
		{
			this.playAnnouncement(announcementData);
		}
		this.state.announcementsTriggeredThisFrame.Clear();
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x000671A0 File Offset: 0x000655A0
	private void onStatLogged(GameEvent message)
	{
		LogStatEvent logStatEvent = message as LogStatEvent;
		if (this.statTrackerMap.ContainsKey(logStatEvent.stat))
		{
			List<AnnouncementStatTracker> list = this.statTrackerMap[logStatEvent.stat];
			bool canAnnounce = true;
			for (int i = 0; i < list.Count; i++)
			{
				canAnnounce = !list[i].RecordStat(logStatEvent.value, logStatEvent.player, canAnnounce, this.gameController.currentGame.Frame);
			}
		}
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x00067224 File Offset: 0x00065624
	private void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		if (characterDeathEvent.wasEliminated && this.gameController.currentGame != null && !this.gameController.currentGame.IsNetworkGame)
		{
			string text = AnnouncementType.None;
			switch (characterDeathEvent.character.PlayerNum)
			{
			case PlayerNum.Player1:
				text = AnnouncementType.Battle_Eliminated_Player1;
				break;
			case PlayerNum.Player2:
				text = AnnouncementType.Battle_Eliminated_Player2;
				break;
			case PlayerNum.Player3:
				text = AnnouncementType.Battle_Eliminated_Player3;
				break;
			case PlayerNum.Player4:
				text = AnnouncementType.Battle_Eliminated_Player4;
				break;
			case PlayerNum.Player5:
				text = AnnouncementType.Battle_Eliminated_Player5;
				break;
			case PlayerNum.Player6:
				text = AnnouncementType.Battle_Eliminated_Player6;
				break;
			}
			if (text != AnnouncementType.None)
			{
				this.receiveAnnouncement(text);
			}
		}
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x000672FC File Offset: 0x000656FC
	private void onCharacterSpawn(GameEvent message)
	{
		CharacterSpawnCommand characterSpawnCommand = message as CharacterSpawnCommand;
		if (characterSpawnCommand.spawnType == PlayerEngagementState.Temporary && this.modeData.Type == GameMode.CrewBattle)
		{
			this.receiveAnnouncement(AnnouncementType.Battle_Team_AssistActive);
		}
		else if (characterSpawnCommand.spawnType == PlayerEngagementState.Primary && this.modeData.settings.usesTeams && this.modeData.settings.announceTeamFinalStock && this.countTeamLivesRemaining(characterSpawnCommand.team) == 0)
		{
			string text = AnnouncementType.None;
			switch (characterSpawnCommand.team)
			{
			case TeamNum.Team1:
				text = AnnouncementType.Battle_Team_FinalStock_Team1;
				break;
			case TeamNum.Team2:
				text = AnnouncementType.Battle_Team_FinalStock_Team2;
				break;
			case TeamNum.Team3:
				text = AnnouncementType.Battle_Team_FinalStock_Team3;
				break;
			case TeamNum.Team4:
				text = AnnouncementType.Battle_Team_FinalStock_Team4;
				break;
			}
			if (text != AnnouncementType.None)
			{
				this.receiveAnnouncement(text);
			}
		}
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x000673F4 File Offset: 0x000657F4
	private int countTeamLivesRemaining(TeamNum team)
	{
		if (!this.teamPlayers.ContainsKey(team))
		{
			return 0;
		}
		int num = 0;
		List<PlayerReference> list = this.teamPlayers[team];
		for (int i = 0; i < list.Count; i++)
		{
			PlayerReference playerReference = list[i];
			if (playerReference.Team == team)
			{
				num += playerReference.Lives;
			}
		}
		return num;
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00067458 File Offset: 0x00065858
	private void onPlayAnnouncementCommand(GameEvent message)
	{
		PlayAnnouncementCommand playAnnouncementCommand = message as PlayAnnouncementCommand;
		this.receiveAnnouncement(playAnnouncementCommand.AnnouncementType);
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x00067478 File Offset: 0x00065878
	private void receiveAnnouncement(string announcementType)
	{
		AnnouncementData announcementData = this.getAnnouncementData(announcementType);
		if (announcementData != null && !announcementData.isEmpty)
		{
			this.state.announcementsTriggeredThisFrame.Add(announcementData);
		}
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x000674B0 File Offset: 0x000658B0
	private void playAnnouncement(AnnouncementData announcementData)
	{
		long currentTimeMs = WTime.currentTimeMs;
		if ((double)currentTimeMs < this.state.lastAnnouncementTimeMs + (double)this.data.MinMsBetweenAnnouncements)
		{
			return;
		}
		if (announcementData != null)
		{
			if (announcementData.sound.sound != null)
			{
				AudioData sound = announcementData.sound.MultiplyVolume(this.data.AnnouncementsVolume);
				this.audioManager.PlaySFX(sound);
			}
			this.state.lastAnnouncementTimeMs = (double)currentTimeMs;
		}
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x00067530 File Offset: 0x00065930
	private AnnouncementData getAnnouncementData(string type)
	{
		if (!this.announcementDataMap.ContainsKey(type))
		{
			return null;
		}
		List<AnnouncementData> list = this.announcementDataMap[type];
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num += list[i].weight;
		}
		float num2 = (float)UnityEngine.Random.Range(0, num);
		num = 0;
		int j = 0;
		while (j < list.Count)
		{
			num += list[j].weight;
			if (num2 < (float)num)
			{
				if (list[j].isEmpty)
				{
					return null;
				}
				return list[j];
			}
			else
			{
				j++;
			}
		}
		return null;
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x000675E4 File Offset: 0x000659E4
	public bool LoadState(RollbackStateContainer container)
	{
		foreach (KeyValuePair<StatType, List<AnnouncementStatTracker>> keyValuePair in this.statTrackerMap)
		{
			List<AnnouncementStatTracker> value = keyValuePair.Value;
			foreach (AnnouncementStatTracker announcementStatTracker in value)
			{
				announcementStatTracker.LoadState(container);
			}
		}
		return true;
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x0006768C File Offset: 0x00065A8C
	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (KeyValuePair<StatType, List<AnnouncementStatTracker>> keyValuePair in this.statTrackerMap)
		{
			List<AnnouncementStatTracker> value = keyValuePair.Value;
			foreach (AnnouncementStatTracker announcementStatTracker in value)
			{
				announcementStatTracker.ExportState(ref container);
			}
		}
		return true;
	}

	// Token: 0x04000B70 RID: 2928
	private AnnouncementConfigData data;

	// Token: 0x04000B71 RID: 2929
	private List<PlayerReference> allPlayers;

	// Token: 0x04000B72 RID: 2930
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;

	// Token: 0x04000B73 RID: 2931
	private Dictionary<string, List<AnnouncementData>> announcementDataMap;

	// Token: 0x04000B74 RID: 2932
	private Dictionary<StatType, List<AnnouncementStatTracker>> statTrackerMap;

	// Token: 0x04000B75 RID: 2933
	private GameModeData modeData;

	// Token: 0x04000B76 RID: 2934
	private AnnouncementHelper.AnnouncementHelperState state;

	// Token: 0x02000350 RID: 848
	private class AnnouncementHelperState
	{
		// Token: 0x04000B77 RID: 2935
		public double lastAnnouncementTimeMs;

		// Token: 0x04000B78 RID: 2936
		public List<AnnouncementData> announcementsTriggeredThisFrame = new List<AnnouncementData>();
	}
}
