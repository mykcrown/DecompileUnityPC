// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : IRollbackStateOwner, ITickable
{
	private PlayerSpawnerState state;

	protected IEvents events;

	protected Dictionary<PlayerNum, PlayerReference> references;

	protected List<PlayerReference> referenceList;

	protected RespawnConfig spawnConfig;

	protected GameModeData modeData;

	protected StageSceneData stage;

	public Dictionary<PlayerNum, int> despawnQueuedFrame = new Dictionary<PlayerNum, int>(6, default(PlayerNumComparer));

	protected SpawnPointModeData spawnData;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
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

	public GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

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

	public bool IsRespawning(PlayerNum player)
	{
		return this.state.respawnQueuedFrame.ContainsKey(player);
	}

	public int GetRespawnDelayFrames(PlayerNum player)
	{
		return (!this.IsRespawning(player)) ? 0 : this.state.respawnQueuedFrame[player];
	}

	public PlayerEngagementState GetSpawnType(PlayerNum player)
	{
		return (!this.IsRespawning(player)) ? PlayerEngagementState.None : this.state.queuedSpawnType[player];
	}

	protected void onDeductLife(GameEvent message)
	{
		DeductPlayerLifeCommand deductPlayerLifeCommand = message as DeductPlayerLifeCommand;
		PlayerReference playerReference = this.references[deductPlayerLifeCommand.character.PlayerNum];
		if (this.modeData.settings.usesLives && !playerReference.IsTemporary && !playerReference.IsAllyAssistMove)
		{
			playerReference.Lives--;
		}
	}

	protected void onCharacterDeath(GameEvent message)
	{
		CharacterDeathEvent characterDeathEvent = message as CharacterDeathEvent;
		PlayerReference player = this.references[characterDeathEvent.character.PlayerNum];
		this.handleCharacterDeath(player);
	}

	private void onDisconnectPlayer(GameEvent message)
	{
		DisconnectPlayerCommand disconnectPlayerCommand = message as DisconnectPlayerCommand;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Must despawn ",
			disconnectPlayerCommand.playerNum,
			" ",
			disconnectPlayerCommand.despawnFrame
		}));
		this.despawnQueuedFrame[disconnectPlayerCommand.playerNum] = disconnectPlayerCommand.despawnFrame;
	}

	protected virtual void handleCharacterDeath(PlayerReference player)
	{
		if (!player.Controller.IsEliminated && player.Controller.State.IsDead)
		{
			this.enqueueRespawn(player.PlayerNum, this.spawnConfig.respawnDelayFrames, PlayerEngagementState.Primary);
		}
	}

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

	public void GauntletRespawn(PlayerNum player, PlayerEngagementState spawnType)
	{
		this.enqueueRespawn(player, 0, spawnType);
	}

	protected void enqueueRespawn(PlayerNum player, int delay, PlayerEngagementState spawnType)
	{
		this.state.respawnQueuedFrame[player] = delay;
		this.state.queuedSpawnType[player] = spawnType;
	}

	protected bool swapRespawn(PlayerNum currentPlayer, PlayerNum newPlayer)
	{
		if (!this.state.respawnQueuedFrame.ContainsKey(currentPlayer))
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
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
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Despawn me ",
					playerNum,
					" ",
					this.gameController.currentGame.Frame
				}));
				PlayerReference playerReference = this.references[playerNum];
				playerReference.EngagementState = PlayerEngagementState.Disconnected;
				foreach (PlayerController current in playerReference.AllControllers)
				{
					current.Despawn();
				}
				this.events.Broadcast(new PlayerDisconnectDespawnEvent());
			}
		}
	}

	protected virtual SpawnPointBase getSpawnPointForPlayer(PlayerNum playerNum, TeamNum team, bool isInitialSpawn)
	{
		if (isInitialSpawn)
		{
			return this.spawnData.GetSpawnPoint(this.references[playerNum].spawnReference);
		}
		return this.stage.RecordPlayerRespawn(playerNum);
	}

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

	public virtual void Destroy()
	{
		this.events.Unsubscribe(typeof(CharacterDeathEvent), new Events.EventHandler(this.onCharacterDeath));
		this.events.Unsubscribe(typeof(ShareLifeCommand), new Events.EventHandler(this.onShareLife));
		this.events.Unsubscribe(typeof(DeductPlayerLifeCommand), new Events.EventHandler(this.onDeductLife));
		this.events.Unsubscribe(typeof(DisconnectPlayerCommand), new Events.EventHandler(this.onDisconnectPlayer));
		this.events = null;
	}

	public virtual bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<PlayerSpawnerState>(ref this.state);
		return true;
	}

	public virtual bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<PlayerSpawnerState>(this.state));
	}
}
