// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CrewBattlePlayerSpawner : PlayerSpawner, IBenchedPlayerSpawner
{
	private CrewBattleCustomData crewBattleData;

	private BattleSettings battleSettings;

	private CrewBattlePlayerSpawnerState state;

	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;

	[Inject]
	public ISpawnController spawnController
	{
		get;
		set;
	}

	[Inject]
	public ICrewAssistSpawnHelper assistSpawnHelper
	{
		get;
		set;
	}

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
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
	public ConfigData config
	{
		get;
		set;
	}

	private int friendlyAssistCount
	{
		get
		{
			return this.battleSettings.settings[BattleSettingType.Assists];
		}
	}

	private bool assistStealingEnabled
	{
		get
		{
			return this.battleSettings.settings.ContainsKey(BattleSettingType.AssistStealing) && this.battleSettings.settings[BattleSettingType.AssistStealing] != 0;
		}
	}

	private int temporaryPlayerCount
	{
		get
		{
			int num = 0;
			PlayerNum[] values = EnumUtil.GetValues<PlayerNum>();
			for (int i = 0; i < values.Length; i++)
			{
				if (this.references.ContainsKey(values[i]))
				{
					PlayerReference playerReference = this.references[values[i]];
					if (playerReference.IsTemporary)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	private int assisterStartingDamage
	{
		get
		{
			return this.crewBattleData.assistPlayerBaseSpawnDamage;
		}
	}

	public void Init(IEvents events, GameManager gameManager, Dictionary<PlayerNum, PlayerReference> references, List<PlayerReference> referenceList)
	{
		base.Init(events, gameManager.Stage, gameManager.ModeData, this.config.respawnConfig, references, referenceList);
		this.battleSettings = gameManager.BattleSettings;
		this.crewBattleData = (gameManager.ModeData.customData as CrewBattleCustomData);
		this.state = new CrewBattlePlayerSpawnerState();
		events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	public override void PerformInitialSpawn(GameLoadPayload payload, GameManager gameManager, Action<PlayerReference, SpawnPointBase> spawnPlayerReference)
	{
		base.PerformInitialSpawn(payload, gameManager, spawnPlayerReference);
		this.teamPlayers = new Dictionary<TeamNum, List<PlayerReference>>();
		List<PlayerReference> playerReferences = gameManager.PlayerReferences;
		for (int i = 0; i < playerReferences.Count; i++)
		{
			PlayerReference playerReference = playerReferences[i];
			if (!this.teamPlayers.ContainsKey(playerReference.Team))
			{
				this.teamPlayers[playerReference.Team] = new List<PlayerReference>();
			}
			this.teamPlayers[playerReference.Team].Add(playerReference);
		}
	}

	private void onGameStart(GameEvent message)
	{
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Subscribe(typeof(RequestTagInCommand), new Events.EventHandler(this.onRequestTagInEvent));
		this.events.Subscribe(typeof(AttemptFriendlyAssistCommand), new Events.EventHandler(this.onAttemptFriendlyAssist));
		this.events.Subscribe(typeof(DespawnCharacterCommand), new Events.EventHandler(this.onDespawnCommand));
		this.events.Subscribe(typeof(AttemptTeamPowerMoveCommand), new Events.EventHandler(this.onAttemptTeamPowerMove));
	}

	public int GetAssistsRemaining(PlayerNum player)
	{
		return this.friendlyAssistCount - this.GetAssistsUsed(player);
	}

	public int GetAssistsUsed(PlayerNum player)
	{
		if (!this.state.assistsUsed.ContainsKey(player))
		{
			return 0;
		}
		return this.state.assistsUsed[player];
	}

	public PlayerNum GetPrimaryPlayer(TeamNum team)
	{
		PlayerNum result = PlayerNum.None;
		for (int i = 0; i < this.teamPlayers[team].Count; i++)
		{
			PlayerReference playerReference = this.teamPlayers[team][i];
			if (playerReference.EngagementState == PlayerEngagementState.Primary)
			{
				result = playerReference.PlayerNum;
				break;
			}
		}
		return result;
	}

	public PlayerReference GetPlayerRef(PlayerNum playerNum)
	{
		foreach (TeamNum current in this.teamPlayers.Keys)
		{
			for (int i = 0; i < this.teamPlayers[current].Count; i++)
			{
				PlayerReference playerReference = this.teamPlayers[current][i];
				if (playerReference.PlayerNum == playerNum)
				{
					return playerReference;
				}
			}
		}
		return null;
	}

	protected PlayerReference getNextPlayer(TeamNum team, PlayerReference currentPlayer)
	{
		List<PlayerReference> list = this.teamPlayers[team];
		int num = list.IndexOf(currentPlayer);
		if (num < 0)
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to find next player on team ",
				team,
				" after player ",
				currentPlayer.PlayerNum
			}));
			return currentPlayer;
		}
		return list[(num + 1) % list.Count];
	}

	protected PlayerReference findNextPlayerInRotation(TeamNum team, PlayerReference currentPlayer)
	{
		List<PlayerReference> list = this.teamPlayers[team];
		int num = list.IndexOf(currentPlayer);
		for (int i = 1; i <= list.Count; i++)
		{
			int num2 = num + i;
			if (num2 > list.Count - 1)
			{
				num2 -= list.Count;
			}
			PlayerReference playerReference = list[num2];
			if (!playerReference.IsEliminated)
			{
				return playerReference;
			}
		}
		return null;
	}

	private int temporaryPlayersOnTeam(TeamNum team)
	{
		int num = 0;
		PlayerNum[] values = EnumUtil.GetValues<PlayerNum>();
		for (int i = 0; i < values.Length; i++)
		{
			if (this.references.ContainsKey(values[i]))
			{
				PlayerReference playerReference = this.references[values[i]];
				if (playerReference.Team == team && playerReference.IsTemporary)
				{
					num++;
				}
			}
		}
		return num;
	}

	protected override void spawnPlayer(PlayerNum playerNum, PlayerEngagementState spawnType, bool isInitialSpawn, int temporarySpawnDurationFrames = -1, int startingSpawnDamage = 0)
	{
		if (spawnType == PlayerEngagementState.Temporary && startingSpawnDamage == 0)
		{
			startingSpawnDamage = this.assisterStartingDamage;
		}
		this.state.didTagIn.Remove(playerNum);
		if (spawnType == PlayerEngagementState.Temporary)
		{
			this.onAssistSpawnedIn(this.GetPlayerRef(playerNum), false);
			temporarySpawnDurationFrames = this.crewBattleData.friendlyAssistDurationFrames;
		}
		base.spawnPlayer(playerNum, spawnType, isInitialSpawn, temporarySpawnDurationFrames, startingSpawnDamage);
	}

	private int getFramesSinceLastAssist(PlayerNum playerNum)
	{
		if (this.state.lastAssistEndTime.ContainsKey(playerNum))
		{
			return this.state.frameCount - this.state.lastAssistEndTime[playerNum];
		}
		return -1;
	}

	private int getFramesSinceLastAssist(TeamNum team)
	{
		if (this.state.lastAssistTeamEndTime.ContainsKey(team))
		{
			return this.state.frameCount - this.state.lastAssistTeamEndTime[team];
		}
		return -1;
	}

	private int getFramesSinceAssistStart(TeamNum team)
	{
		if (this.state.lastAssistTeamStartTime.ContainsKey(team))
		{
			return this.state.frameCount - this.state.lastAssistTeamStartTime[team];
		}
		return -1;
	}

	protected override void handleCharacterDeath(PlayerReference player)
	{
		if (player.Controller.State.IsDead)
		{
			if (player.IsAllyAssistMove)
			{
				return;
			}
			if (player.IsTemporary)
			{
				player.EngagementState = PlayerEngagementState.Benched;
				this.onAssistPlayerRemoved(player);
				if (this.assistStealingEnabled)
				{
					this.handleAssistSteal(player);
				}
			}
			else if (player.IsInBattle)
			{
				this.state.previousPrimaryPlayer[player.Team] = player.PlayerNum;
				PlayerNum playerNum = PlayerNum.None;
				if (this.crewBattleData.rotateTagInPriority)
				{
					PlayerReference playerReference = this.findNextPlayerInRotation(player.Team, player);
					if (playerReference != null)
					{
						playerNum = playerReference.PlayerNum;
						player.EngagementState = PlayerEngagementState.Benched;
					}
				}
				else
				{
					playerNum = player.PlayerNum;
					player.EngagementState = PlayerEngagementState.Benched;
					if (player.Controller.IsEliminated)
					{
						PlayerReference playerReference2 = this.findNextPlayerInRotation(player.Team, player);
						if (playerReference2 != null)
						{
							playerNum = this.getNextPlayer(player.Team, player).PlayerNum;
							player.EngagementState = PlayerEngagementState.Benched;
						}
					}
				}
				this.state.tagInFrames[player.Team] = 0;
				if (playerNum != PlayerNum.None)
				{
					base.enqueueRespawn(playerNum, this.crewBattleData.respawnDelayFrames + this.crewBattleData.respawnBaseWait, PlayerEngagementState.Primary);
				}
				if (this.crewBattleData.assistTeammateDeathRefundFrames > 0)
				{
					int framesSinceAssistStart = this.getFramesSinceAssistStart(player.Team);
					if (framesSinceAssistStart != -1 && framesSinceAssistStart <= this.crewBattleData.assistTeammateDeathRefundFrames)
					{
						this.cancelAndRefundAssist(player.Team);
					}
				}
				if (this.crewBattleData.endAssistOnKill || this.crewBattleData.grantFramesOnKill > 0)
				{
					TeamNum key = (player.Team != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2;
					for (int i = 0; i < this.teamPlayers[key].Count; i++)
					{
						PlayerReference playerReference3 = this.teamPlayers[key][i];
						if (playerReference3.EngagementState == PlayerEngagementState.Temporary)
						{
							if (this.crewBattleData.endAssistOnKill && playerReference3.Controller.Model.temporaryDurationFrames > this.crewBattleData.endAssistDelayFrames)
							{
								foreach (PlayerController current in playerReference3.AllControllers)
								{
									current.Model.temporaryDurationFrames = this.crewBattleData.endAssistDelayFrames;
								}
							}
							else if (this.crewBattleData.grantFramesOnKill > 0)
							{
								foreach (PlayerController current2 in playerReference3.AllControllers)
								{
									current2.Model.temporaryDurationFrames += this.crewBattleData.grantFramesOnKill;
								}
							}
						}
					}
				}
				if (this.crewBattleData.endAssistOnTeammateDeath)
				{
					for (int j = 0; j < this.teamPlayers[player.Team].Count; j++)
					{
						PlayerReference playerReference4 = this.teamPlayers[player.Team][j];
						if (playerReference4.EngagementState == PlayerEngagementState.Temporary && playerReference4.Controller.Model.temporaryDurationFrames > this.crewBattleData.endAssistDelayFrames)
						{
							playerReference4.Controller.Model.temporaryDurationFrames = this.crewBattleData.endAssistDelayFrames;
						}
					}
				}
			}
		}
	}

	private void cancelAndRefundAssist(TeamNum team)
	{
		for (int i = 0; i < this.teamPlayers[team].Count; i++)
		{
			PlayerReference playerReference = this.teamPlayers[team][i];
			if (playerReference.EngagementState == PlayerEngagementState.Temporary)
			{
				playerReference.Controller.Model.temporaryDurationFrames = 1;
				Dictionary<PlayerNum, int> assistsUsed;
				PlayerNum playerNum;
				(assistsUsed = this.state.assistsUsed)[playerNum = playerReference.PlayerNum] = assistsUsed[playerNum] - 1;
			}
		}
	}

	public override void TickFrame()
	{
		base.TickFrame();
		this.state.frameCount++;
		if (this.GetPrimaryPlayer(TeamNum.Team1) == PlayerNum.None)
		{
			Dictionary<TeamNum, int> tagInFrames;
			(tagInFrames = this.state.tagInFrames)[TeamNum.Team1] = tagInFrames[TeamNum.Team1] + 1;
		}
		else
		{
			this.state.tagInFrames[TeamNum.Team1] = 0;
		}
		if (this.GetPrimaryPlayer(TeamNum.Team2) == PlayerNum.None)
		{
			Dictionary<TeamNum, int> tagInFrames;
			(tagInFrames = this.state.tagInFrames)[TeamNum.Team2] = tagInFrames[TeamNum.Team2] + 1;
		}
		else
		{
			this.state.tagInFrames[TeamNum.Team2] = 0;
		}
	}

	private void handleAssistSteal(PlayerReference player)
	{
		if (player.Team == TeamNum.Team1 || player.Team == TeamNum.Team2)
		{
			PlayerNum primaryPlayer = this.GetPrimaryPlayer((player.Team != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2);
			if (primaryPlayer != PlayerNum.None)
			{
				if (!this.state.assistsUsed.ContainsKey(primaryPlayer))
				{
					this.state.assistsUsed[primaryPlayer] = 0;
				}
				Dictionary<PlayerNum, int> assistsUsed;
				PlayerNum key;
				(assistsUsed = this.state.assistsUsed)[key = primaryPlayer] = assistsUsed[key] - 1;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Player died on invalid team " + player.Team);
		}
	}

	private void onAttemptFriendlyAssist(GameEvent message)
	{
		AttemptFriendlyAssistCommand attemptFriendlyAssistCommand = message as AttemptFriendlyAssistCommand;
		PlayerReference playerReference = this.references[attemptFriendlyAssistCommand.playerNum];
		bool flag = false;
		foreach (PlayerController current in playerReference.AllControllers)
		{
			this.assistSpawnHelper.BeginContext(current, this.teamPlayers);
			if (this.isAssistPossible(playerReference))
			{
				this.assistSpawnHelper.PrepareForAssist();
				this.doAssistMove(attemptFriendlyAssistCommand);
				flag = true;
			}
		}
		if (flag)
		{
			this.onAssistSpawnedIn(playerReference, attemptFriendlyAssistCommand.debugMode);
			playerReference.EngagementState = PlayerEngagementState.Temporary;
		}
	}

	public bool CanAssist(PlayerReference playerRef)
	{
		this.assistSpawnHelper.BeginContext(playerRef.Controller, this.teamPlayers);
		return this.isAssistPossible(playerRef);
	}

	private bool isAssistPossible(PlayerReference playerRef)
	{
		if (!playerRef.IsBenched)
		{
			return false;
		}
		if (!base.gameManager.StartedGame)
		{
			return false;
		}
		if (!this.crewBattleData.friendlyAssistEnabled)
		{
			return false;
		}
		if (this.assistSpawnHelper.Teammate == null)
		{
			return false;
		}
		if (this.assistSpawnHelper.Opponent == null)
		{
			return false;
		}
		if (base.IsRespawning(playerRef.PlayerNum))
		{
			return false;
		}
		if (this.temporaryPlayerCount >= this.crewBattleData.maxSimultaneousAssists && !this.allowSimultaneousAssist(playerRef.Team))
		{
			return false;
		}
		if (this.getAssistsUsed(playerRef.PlayerNum) >= this.friendlyAssistCount)
		{
			return false;
		}
		if (this.crewBattleData.individualPlayerAssistCooldown > 0)
		{
			int framesSinceLastAssist = this.getFramesSinceLastAssist(playerRef.PlayerNum);
			if (framesSinceLastAssist != -1 && framesSinceLastAssist < this.crewBattleData.individualPlayerAssistCooldown)
			{
				return false;
			}
		}
		if (this.crewBattleData.teamAssistCooldown > 0)
		{
			int framesSinceLastAssist2 = this.getFramesSinceLastAssist(playerRef.Team);
			if (framesSinceLastAssist2 != -1 && framesSinceLastAssist2 <= this.crewBattleData.teamAssistCooldown)
			{
				return false;
			}
		}
		if (this.crewBattleData.enemyAssistCooldown > 0)
		{
			TeamNum team = (playerRef.Team != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2;
			int framesSinceLastAssist3 = this.getFramesSinceLastAssist(team);
			if (framesSinceLastAssist3 != -1 && framesSinceLastAssist3 <= this.crewBattleData.enemyAssistCooldown)
			{
				return false;
			}
		}
		return playerRef.Controller.CanUsePowerMove;
	}

	private bool allowSimultaneousAssist(TeamNum playerTeam)
	{
		if (this.crewBattleData.simultaneousAssistWindow > 0 && this.temporaryPlayersOnTeam(playerTeam) == 0)
		{
			TeamNum team = (playerTeam != TeamNum.Team1) ? TeamNum.Team1 : TeamNum.Team2;
			int framesSinceAssistStart = this.getFramesSinceAssistStart(team);
			if (framesSinceAssistStart != -1 && framesSinceAssistStart <= this.crewBattleData.simultaneousAssistWindow)
			{
				return true;
			}
		}
		return false;
	}

	private int getAssistsUsed(PlayerNum playerNum)
	{
		if (!this.state.assistsUsed.ContainsKey(playerNum))
		{
			return 0;
		}
		return this.state.assistsUsed[playerNum];
	}

	private void doAssistMove(AttemptFriendlyAssistCommand assist)
	{
		PlayerController controller = this.assistSpawnHelper.Controller;
		AssistAttackComponent moveComponent = this.assistSpawnHelper.MoveComponent;
		Vector3F spawnPoint = this.assistSpawnHelper.SpawnPoint;
		Vector3F targetPoint = this.assistSpawnHelper.TargetPoint;
		HorizontalDirection facing = this.assistSpawnHelper.Facing;
		int num;
		if (assist.debugMode)
		{
			num = 300;
		}
		else
		{
			num = this.crewBattleData.friendlyAssistDurationFrames;
		}
		this.spawnController.AddPlayerToScene(controller, this.assisterStartingDamage);
		controller.Model.temporaryDurationFrames = ((moveComponent.overrideAssistFrames <= 0) ? num : moveComponent.overrideAssistFrames);
		controller.Model.temporaryDurationTotalFrames = controller.Model.temporaryDurationFrames;
		controller.Physics.SetPosition(spawnPoint);
		controller.SetFacingAndRotation(facing);
		controller.SetMove(this.assistSpawnHelper.Move, null, HorizontalDirection.None, 0, 0, targetPoint, null, null, default(MoveSeedData), ButtonPress.None);
		if (!controller.IsActive)
		{
			CharacterComponent[] components = controller.CharacterData.components;
			for (int i = 0; i < components.Length; i++)
			{
				CharacterComponent characterComponent = components[i];
				if (characterComponent is TotemDuoComponent)
				{
					MoveData crewBattleAssistMove = (characterComponent as TotemDuoComponent).crewBattleAssistMove;
					if (crewBattleAssistMove != null)
					{
						controller.SetMove(crewBattleAssistMove, null, HorizontalDirection.None, 0, 0, targetPoint, null, null, default(MoveSeedData), ButtonPress.None);
					}
				}
			}
		}
	}

	private void trySpawnPowerMove(AttemptTeamPowerMoveCommand powerMove)
	{
		PlayerController controller = this.assistSpawnHelper.Controller;
		AssistAttackComponent moveComponent = this.assistSpawnHelper.MoveComponent;
		Vector3F spawnPoint = this.assistSpawnHelper.SpawnPoint;
		Vector3F targetPoint = this.assistSpawnHelper.TargetPoint;
		HorizontalDirection facing = this.assistSpawnHelper.Facing;
		this.spawnController.AddPlayerToScene(controller, this.assisterStartingDamage);
		if (!controller.MaterialAnimationsController.Overridden)
		{
			Material material = new Material(Shader.Find("WaveDash/AssistCharacter"));
			material.SetColor("_OutlineColor", controller.Renderer.GetOutlineColorFromTeam(controller.Team));
			controller.MaterialAnimationsController.OverrideAllMaterials(material);
		}
		controller.Model.assistAbsorbsHits = (moveComponent != null && moveComponent.assistAbsorbsHits);
		controller.Model.teamPowerMoveCooldownFrames = powerMove.cooldownFrames;
		controller.Model.temporaryAssistImmunityFrames = powerMove.immunityFrames;
		controller.Physics.SetPosition(spawnPoint);
		controller.SetFacingAndRotation(this.assistSpawnHelper.Facing);
		controller.Reference.EngagementState = PlayerEngagementState.AssistMove;
		controller.SetMove(this.assistSpawnHelper.Move, null, HorizontalDirection.None, 0, 0, targetPoint, null, null, default(MoveSeedData), ButtonPress.None);
	}

	private void onAssistSpawnedIn(PlayerReference playerRef, bool isDebug)
	{
		if (!isDebug)
		{
			if (!this.state.assistsUsed.ContainsKey(playerRef.PlayerNum))
			{
				this.state.assistsUsed[playerRef.PlayerNum] = 1;
			}
			else
			{
				Dictionary<PlayerNum, int> assistsUsed;
				PlayerNum playerNum;
				(assistsUsed = this.state.assistsUsed)[playerNum = playerRef.PlayerNum] = assistsUsed[playerNum] + 1;
			}
		}
		this.state.lastAssistTeamStartTime[playerRef.Team] = this.state.frameCount;
		playerRef.Controller.Model.blastZoneImmunityFrames = this.crewBattleData.assistBlastZoneImmunityFrames;
	}

	private void onRequestTagInEvent(GameEvent message)
	{
		RequestTagInCommand requestTagInCommand = message as RequestTagInCommand;
		if (this.CanTagIn(requestTagInCommand.playerNum))
		{
			if (!base.IsRespawning(requestTagInCommand.playerNum))
			{
				TeamNum team = this.GetPlayerRef(requestTagInCommand.playerNum).Team;
				PlayerNum currentPlayer = this.findRespawnerForTeam(team);
				this.tagIn(currentPlayer, requestTagInCommand.team, requestTagInCommand.playerNum);
			}
			base.enqueueRespawn(requestTagInCommand.playerNum, 0, PlayerEngagementState.Primary);
		}
	}

	private PlayerNum findRespawnerForTeam(TeamNum team)
	{
		foreach (PlayerReference current in this.teamPlayers[team])
		{
			if (base.IsRespawning(current.PlayerNum))
			{
				return current.PlayerNum;
			}
		}
		return PlayerNum.None;
	}

	public bool CanTagIn(PlayerNum player)
	{
		PlayerReference playerRef = this.GetPlayerRef(player);
		return this.GetPrimaryPlayer(playerRef.Team) == PlayerNum.None && !playerRef.IsEliminated && this.state.tagInFrames[playerRef.Team] >= this.crewBattleData.respawnBaseWait && (base.IsRespawning(player) || this.isAllowedTagIn(playerRef) || this.state.tagInFrames[playerRef.Team] >= this.crewBattleData.benchedPlayerTagInDelayFrames + this.crewBattleData.respawnBaseWait);
	}

	public bool DisplayTagInOptionInHUD()
	{
		return this.crewBattleData.respawnBaseWait > 0;
	}

	private bool isAllowedTagIn(PlayerReference playerRef)
	{
		return this.crewBattleData.allPlayersRotate && (!this.state.previousPrimaryPlayer.ContainsKey(playerRef.Team) || this.state.previousPrimaryPlayer[playerRef.Team] != playerRef.PlayerNum);
	}

	private void tagIn(PlayerNum currentPlayer, TeamNum team, PlayerNum taggedPlayer)
	{
		this.state.didTagIn[taggedPlayer] = true;
		base.swapRespawn(currentPlayer, taggedPlayer);
		this.events.Broadcast(new TagInPlayerEvent(team, taggedPlayer));
	}

	private void onDespawnCommand(GameEvent message)
	{
		DespawnCharacterCommand despawnCharacterCommand = message as DespawnCharacterCommand;
		PlayerReference playerReference = this.references[despawnCharacterCommand.player];
		bool flag = playerReference.EngagementState == PlayerEngagementState.Temporary;
		playerReference.EngagementState = PlayerEngagementState.Benched;
		if (flag)
		{
			this.onAssistPlayerRemoved(playerReference);
		}
		foreach (PlayerController current in playerReference.AllControllers)
		{
			current.Despawn();
		}
	}

	private void onAttemptTeamPowerMove(GameEvent message)
	{
		AttemptTeamPowerMoveCommand attemptTeamPowerMoveCommand = message as AttemptTeamPowerMoveCommand;
		PlayerController playerController = null;
		foreach (PlayerReference current in base.gameController.currentGame.PlayerReferences)
		{
			if (attemptTeamPowerMoveCommand.playerNum == current.PlayerNum)
			{
				playerController = current.Controller;
				this.assistSpawnHelper.BeginContext(playerController, this.teamPlayers);
			}
		}
		if (playerController == null || this.assistSpawnHelper.CannotProceed())
		{
			return;
		}
		bool flag = false;
		if (playerController.CanUsePowerMove)
		{
			foreach (PlayerReference current2 in base.gameController.currentGame.PlayerReferences)
			{
				if (current2.Controller != null && current2.PlayerNum != playerController.PlayerNum && current2.Controller.Team == attemptTeamPowerMoveCommand.team && current2.CanHostTeamMove)
				{
					if (this.assistSpawnHelper.PrepareForPowerMove())
					{
						this.trySpawnPowerMove(attemptTeamPowerMoveCommand);
					}
					else
					{
						current2.Controller.TeamPowerMove(attemptTeamPowerMoveCommand.spawnParticle, attemptTeamPowerMoveCommand.assistArticles);
					}
					playerController.Model.teamPowerMoveCooldownFrames = attemptTeamPowerMoveCommand.cooldownFrames;
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.audioManager.PlayMenuSound(SoundKey.generic_powerMoveDenied, 0f);
		}
	}

	private void onAssistPlayerRemoved(PlayerReference reference)
	{
		this.state.lastAssistEndTime[reference.PlayerNum] = this.state.frameCount;
		this.state.lastAssistTeamEndTime[reference.Team] = this.state.frameCount;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		container.ReadState<CrewBattlePlayerSpawnerState>(ref this.state);
		return true;
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		return container.WriteState(base.rollbackStatePooling.Clone<CrewBattlePlayerSpawnerState>(this.state));
	}

	public override void Destroy()
	{
		this.events.Unsubscribe(typeof(RequestTagInCommand), new Events.EventHandler(this.onRequestTagInEvent));
		this.events.Unsubscribe(typeof(AttemptFriendlyAssistCommand), new Events.EventHandler(this.onAttemptFriendlyAssist));
		this.events.Unsubscribe(typeof(DespawnCharacterCommand), new Events.EventHandler(this.onDespawnCommand));
		this.events.Unsubscribe(typeof(AttemptTeamPowerMoveCommand), new Events.EventHandler(this.onAttemptTeamPowerMove));
		base.Destroy();
	}
}
