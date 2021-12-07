// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncementHelper : ITickable, IRollbackStateOwner
{
	private class AnnouncementHelperState
	{
		public double lastAnnouncementTimeMs;

		public List<AnnouncementData> announcementsTriggeredThisFrame = new List<AnnouncementData>();
	}

	private AnnouncementConfigData data;

	private List<PlayerReference> allPlayers;

	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;

	private Dictionary<string, List<AnnouncementData>> announcementDataMap;

	private Dictionary<StatType, List<AnnouncementStatTracker>> statTrackerMap;

	private GameModeData modeData;

	private AnnouncementHelper.AnnouncementHelperState state;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

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

	private void onGameEnd(GameEvent message)
	{
		this.allPlayers = null;
		this.teamPlayers.Clear();
	}

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

	private void onPlayAnnouncementCommand(GameEvent message)
	{
		PlayAnnouncementCommand playAnnouncementCommand = message as PlayAnnouncementCommand;
		this.receiveAnnouncement(playAnnouncementCommand.AnnouncementType);
	}

	private void receiveAnnouncement(string announcementType)
	{
		AnnouncementData announcementData = this.getAnnouncementData(announcementType);
		if (announcementData != null && !announcementData.isEmpty)
		{
			this.state.announcementsTriggeredThisFrame.Add(announcementData);
		}
	}

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

	public bool LoadState(RollbackStateContainer container)
	{
		foreach (KeyValuePair<StatType, List<AnnouncementStatTracker>> current in this.statTrackerMap)
		{
			List<AnnouncementStatTracker> value = current.Value;
			foreach (AnnouncementStatTracker current2 in value)
			{
				current2.LoadState(container);
			}
		}
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		foreach (KeyValuePair<StatType, List<AnnouncementStatTracker>> current in this.statTrackerMap)
		{
			List<AnnouncementStatTracker> value = current.Value;
			foreach (AnnouncementStatTracker current2 in value)
			{
				current2.ExportState(ref container);
			}
		}
		return true;
	}
}
