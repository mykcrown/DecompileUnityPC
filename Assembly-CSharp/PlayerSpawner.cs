using System;
using System.Collections.Generic;
using FixedPoint;
using IconsServer;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class PlayerSpawner : IRollbackStateOwner, ITickable
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06001A8B RID: 6795 RVA: 0x00086F29 File Offset: 0x00085329
	// (set) Token: 0x06001A8C RID: 6796 RVA: 0x00086F31 File Offset: 0x00085331
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06001A8D RID: 6797 RVA: 0x00086F3A File Offset: 0x0008533A
	// (set) Token: 0x06001A8E RID: 6798 RVA: 0x00086F42 File Offset: 0x00085342
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06001A8F RID: 6799 RVA: 0x00086F4B File Offset: 0x0008534B
	// (set) Token: 0x06001A90 RID: 6800 RVA: 0x00086F53 File Offset: 0x00085353
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06001A91 RID: 6801 RVA: 0x00086F5C File Offset: 0x0008535C
	public GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06001A92 RID: 6802 RVA: 0x00086F69 File Offset: 0x00085369
	// (set) Token: 0x06001A93 RID: 6803 RVA: 0x00086F71 File Offset: 0x00085371
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06001A94 RID: 6804 RVA: 0x00086F7A File Offset: 0x0008537A
	// (set) Token: 0x06001A95 RID: 6805 RVA: 0x00086F82 File Offset: 0x00085382
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x06001A96 RID: 6806 RVA: 0x00086F8C File Offset: 0x0008538C
	public void Init(IEvents events, StageSceneData stage, GameModeData modeData, RespawnConfig spawnConfig, Dictionary<PlayerNum, PlayerReference> references, List<PlayerReference> referenceList)
	{
		this.stage = stage;
		this.spawnConfig = spawnConfig;
		this.events = events;
		this.modeData = modeData;
		this.state = new PlayerSpawnerState();
		this.referenceList = referenceList;
		SpawnModeType spawnModeFromPlayerReferences = SpawnModeUtil.GetSpawnModeFromPlayerReferences(this.referenceList);
		this.spawnData = stage.SimulationData.spawnData.GetSpawnPointModeData(spawnModeFromPlayerReferences);
		SpawnModeUtil.AssignPlayerReferencesSpawnPoints(this.spawnData, this.referenceList);
		this.references = references;
		events.Subscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		events.Subscribe(typeof(ShareLifeCommand), new Events.EventHandler(this.onShareLife));
		events.Subscribe(typeof(DeductPlayerLifeCommand), new Events.EventHandler(this.onDeductLife));
		events.Subscribe(typeof(DisconnectPlayerCommand), new Events.EventHandler(this.onDisconnectPlayer));
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x00087078 File Offset: 0x00085478
	public virtual void PerformInitialSpawn(GameLoadPayload payload, GameManager gameManager, Action<PlayerReference, SpawnPointBase> spawnPlayerReference)
	{
		Dictionary<TeamNum, int> dictionary = new Dictionary<TeamNum, int>();
		for (int i = 0; i < payload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo = payload.players[i];
			if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
			{
				PlayerReference playerReference = this.references[playerSelectionInfo.playerNum];
				spawnPlayerReference(playerReference, this.getSpawnPointForPlayer(playerReference.PlayerNum, playerReference.Team, true));
				bool flag = false;
				if (payload.battleConfig.mode == GameMode.CrewBattle)
				{
					if (!dictionary.ContainsKey(playerReference.Team))
					{
						dictionary[playerReference.Team] = 1;
					}
					else
					{
						Dictionary<TeamNum, int> dictionary2;
						TeamNum team;
						(dictionary2 = dictionary)[team = playerReference.Team] = dictionary2[team] + 1;
						flag = true;
					}
				}
				if (!flag)
				{
					playerReference.EngagementState = PlayerEngagementState.Primary;
				}
				else
				{
					playerReference.EngagementState = PlayerEngagementState.Benched;
				}
			}
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x0008716D File Offset: 0x0008556D
	public bool IsRespawning(PlayerNum player)
	{
		return this.state.respawnQueuedFrame.ContainsKey(player);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x00087180 File Offset: 0x00085580
	public int GetRespawnDelayFrames(PlayerNum player)
	{
		return (!this.IsRespawning(player)) ? 0 : this.state.respawnQueuedFrame[player];
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000871A5 File Offset: 0x000855A5
	public PlayerEngagementState GetSpawnType(PlayerNum player)
	{
		return (!this.IsRespawning(player)) ? PlayerEngagementState.None : this.state.queuedSpawnType[player];
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000871CC File Offset: 0x000855CC
	protected void onDeductLife(GameEvent message)
	{
		DeductPlayerLifeCommand deductPlayerLifeCommand = message as DeductPlayerLifeCommand;
		PlayerReference playerReference = this.references[deductPlayerLifeCommand.character.PlayerNum];
		if (this.modeData.settings.usesLives && !playerReference.IsTemporary && !playerReference.IsAllyAssistMove)
		{
			playerReference.Lives--;
		}
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x00087230 File Offset: 0x00085630
	protected void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		PlayerReference player = this.references[characterDeathEvent.character.PlayerNum];
		this.handleCharacterDeath(player);
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x00087264 File Offset: 0x00085664
	private void onDisconnectPlayer(GameEvent message)
	{
		DisconnectPlayerCommand disconnectPlayerCommand = message as DisconnectPlayerCommand;
		Debug.Log(string.Concat(new object[]
		{
			"Must despawn ",
			disconnectPlayerCommand.playerNum,
			" ",
			disconnectPlayerCommand.despawnFrame
		}));
		this.despawnQueuedFrame[disconnectPlayerCommand.playerNum] = disconnectPlayerCommand.despawnFrame;
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000872CB File Offset: 0x000856CB
	protected virtual void handleCharacterDeath(PlayerReference player)
	{
		if (!player.Controller.IsEliminated && player.Controller.State.IsDead)
		{
			this.enqueueRespawn(player.PlayerNum, this.spawnConfig.respawnDelayFrames, PlayerEngagementState.Primary);
		}
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x0008730C File Offset: 0x0008570C
	protected void onShareLife(GameEvent message)
	{
		ShareLifeCommand shareLifeCommand = message as ShareLifeCommand;
		PlayerReference playerReference = this.references[shareLifeCommand.player];
		int num = -1;
		int num2 = 0;
		Fixed other = 999;
		for (int i = 0; i < this.referenceList.Count; i++)
		{
			PlayerReference playerReference2 = this.referenceList[i];
			if (playerReference2.Team == shareLifeCommand.team && playerReference2.PlayerNum != shareLifeCommand.player && playerReference2.Controller.Lives > 1)
			{
				bool flag = false;
				if (playerReference2.Controller.Lives > num2)
				{
					flag = true;
				}
				else if (playerReference2.Controller.Lives == num2 && playerReference2.Controller.Damage < other)
				{
					flag = true;
				}
				if (flag)
				{
					num = i;
					num2 = playerReference2.Controller.Lives;
					other = playerReference2.Controller.Damage;
				}
			}
		}
		if (num != -1)
		{
			playerReference.Controller.Lives++;
			this.referenceList[num].Controller.Lives--;
			this.enqueueRespawn(playerReference.PlayerNum, this.spawnConfig.respawnDelayFrames, PlayerEngagementState.Primary);
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x00087466 File Offset: 0x00085866
	public void GauntletRespawn(PlayerNum player, PlayerEngagementState spawnType)
	{
		this.enqueueRespawn(player, 0, spawnType);
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00087471 File Offset: 0x00085871
	protected void enqueueRespawn(PlayerNum player, int delay, PlayerEngagementState spawnType)
	{
		this.state.respawnQueuedFrame[player] = delay;
		this.state.queuedSpawnType[player] = spawnType;
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x00087498 File Offset: 0x00085898
	protected bool swapRespawn(PlayerNum currentPlayer, PlayerNum newPlayer)
	{
		if (!this.state.respawnQueuedFrame.ContainsKey(currentPlayer))
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Attempted to tag in player ",
				newPlayer,
				" but ",
				currentPlayer,
				" was not respawning!"
			}));
			return false;
		}
		this.state.respawnQueuedFrame[newPlayer] = this.state.respawnQueuedFrame[currentPlayer];
		this.state.respawnQueuedFrame.Remove(currentPlayer);
		this.state.queuedSpawnType[newPlayer] = this.state.queuedSpawnType[currentPlayer];
		this.state.queuedSpawnType.Remove(currentPlayer);
		return true;
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x00087560 File Offset: 0x00085960
	public virtual void TickFrame()
	{
		for (int i = 0; i < this.referenceList.Count; i++)
		{
			PlayerNum playerNum = this.referenceList[i].PlayerNum;
			if (!this.referenceList[i].PlayerInfo.isSpectator && this.state.respawnQueuedFrame.ContainsKey(playerNum))
			{
				Dictionary<PlayerNum, int> respawnQueuedFrame;
				PlayerNum key;
				int num = (respawnQueuedFrame = this.state.respawnQueuedFrame)[key = playerNum] = respawnQueuedFrame[key] - 1;
				if (num <= 0)
				{
					this.spawnPlayer(playerNum, this.state.queuedSpawnType[playerNum], false, -1, 0);
					this.state.respawnQueuedFrame.Remove(playerNum);
					this.state.queuedSpawnType.Remove(playerNum);
				}
			}
			if (this.despawnQueuedFrame.ContainsKey(playerNum) && this.gameController.currentGame.Frame == this.despawnQueuedFrame[playerNum])
			{
				Debug.Log(string.Concat(new object[]
				{
					"Despawn me ",
					playerNum,
					" ",
					this.gameController.currentGame.Frame
				}));
				PlayerReference playerReference = this.references[playerNum];
				playerReference.EngagementState = PlayerEngagementState.Disconnected;
				foreach (PlayerController playerController in playerReference.AllControllers)
				{
					playerController.Despawn();
				}
				this.events.Broadcast(new PlayerDisconnectDespawnEvent());
			}
		}
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x00087724 File Offset: 0x00085B24
	protected virtual SpawnPointBase getSpawnPointForPlayer(PlayerNum playerNum, TeamNum team, bool isInitialSpawn)
	{
		if (isInitialSpawn)
		{
			return this.spawnData.GetSpawnPoint(this.references[playerNum].spawnReference);
		}
		return this.stage.RecordPlayerRespawn(playerNum);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x00087758 File Offset: 0x00085B58
	protected virtual void spawnPlayer(PlayerNum playerNum, PlayerEngagementState spawnType, bool isInitialSpawn, int temporarySpawnDurationFrames = -1, int startingSpawnDamage = 0)
	{
		PlayerReference playerReference = this.references[playerNum];
		PlayerReference allyReferenceWithValidController = this.gameManager.getAllyReferenceWithValidController(playerReference);
		if (allyReferenceWithValidController != null && allyReferenceWithValidController.InputController as PlayerInputPort != null)
		{
			PlayerInputPort playerInputPort = allyReferenceWithValidController.InputController as PlayerInputPort;
			allyReferenceWithValidController.UnsafeSetInputController(null);
			playerReference.UnsafeSetInputController(playerInputPort);
			this.userInputManager.ResetPlayerMappingForPort(playerInputPort);
			this.userInputManager.AssignPlayerNum(playerInputPort.Id, playerNum);
		}
		this.events.Broadcast(new CharacterSpawnCommand(playerReference.PlayerNum, playerReference.Team, spawnType, this.getSpawnPointForPlayer(playerReference.PlayerNum, playerReference.Team, isInitialSpawn), temporarySpawnDurationFrames, startingSpawnDamage));
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x00087808 File Offset: 0x00085C08
	public virtual void Destroy()
	{
		this.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.events.Unsubscribe(typeof(ShareLifeCommand), new Events.EventHandler(this.onShareLife));
		this.events.Unsubscribe(typeof(DeductPlayerLifeCommand), new Events.EventHandler(this.onDeductLife));
		this.events.Unsubscribe(typeof(DisconnectPlayerCommand), new Events.EventHandler(this.onDisconnectPlayer));
		this.events = null;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000878A0 File Offset: 0x00085CA0
	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PlayerSpawnerState>(ref this.state);
		return true;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000878B0 File Offset: 0x00085CB0
	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<PlayerSpawnerState>(this.state));
	}

	// Token: 0x040013E2 RID: 5090
	private PlayerSpawnerState state;

	// Token: 0x040013E3 RID: 5091
	protected IEvents events;

	// Token: 0x040013E4 RID: 5092
	protected Dictionary<PlayerNum, PlayerReference> references;

	// Token: 0x040013E5 RID: 5093
	protected List<PlayerReference> referenceList;

	// Token: 0x040013E6 RID: 5094
	protected RespawnConfig spawnConfig;

	// Token: 0x040013E7 RID: 5095
	protected GameModeData modeData;

	// Token: 0x040013E8 RID: 5096
	protected StageSceneData stage;

	// Token: 0x040013E9 RID: 5097
	public Dictionary<PlayerNum, int> despawnQueuedFrame = new Dictionary<PlayerNum, int>(6, default(PlayerNumComparer));

	// Token: 0x040013EA RID: 5098
	protected SpawnPointModeData spawnData;
}
