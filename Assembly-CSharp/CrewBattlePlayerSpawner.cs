using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class CrewBattlePlayerSpawner : PlayerSpawner, IBenchedPlayerSpawner
{
	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000878D2 File Offset: 0x00085CD2
	// (set) Token: 0x06001A15 RID: 6677 RVA: 0x000878DA File Offset: 0x00085CDA
	[Inject]
	public ISpawnController spawnController { get; set; }

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x000878E3 File Offset: 0x00085CE3
	// (set) Token: 0x06001A17 RID: 6679 RVA: 0x000878EB File Offset: 0x00085CEB
	[Inject]
	public ICrewAssistSpawnHelper assistSpawnHelper { get; set; }

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06001A18 RID: 6680 RVA: 0x000878F4 File Offset: 0x00085CF4
	// (set) Token: 0x06001A19 RID: 6681 RVA: 0x000878FC File Offset: 0x00085CFC
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06001A1A RID: 6682 RVA: 0x00087905 File Offset: 0x00085D05
	// (set) Token: 0x06001A1B RID: 6683 RVA: 0x0008790D File Offset: 0x00085D0D
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06001A1C RID: 6684 RVA: 0x00087916 File Offset: 0x00085D16
	// (set) Token: 0x06001A1D RID: 6685 RVA: 0x0008791E File Offset: 0x00085D1E
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06001A1E RID: 6686 RVA: 0x00087927 File Offset: 0x00085D27
	// (set) Token: 0x06001A1F RID: 6687 RVA: 0x0008792F File Offset: 0x00085D2F
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x06001A20 RID: 6688 RVA: 0x00087938 File Offset: 0x00085D38
	public void Init(IEvents events, GameManager gameManager, Dictionary<PlayerNum, PlayerReference> references, List<PlayerReference> referenceList)
	{
		base.Init(events, gameManager.Stage, gameManager.ModeData, this.config.respawnConfig, references, referenceList);
		this.battleSettings = gameManager.BattleSettings;
		this.crewBattleData = (gameManager.ModeData.customData as CrewBattleCustomData);
		this.state = new CrewBattlePlayerSpawnerState();
		events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x000879B0 File Offset: 0x00085DB0
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

	// Token: 0x06001A22 RID: 6690 RVA: 0x00087A3C File Offset: 0x00085E3C
	private void onGameStart(GameEvent message)
	{
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Subscribe(typeof(RequestTagInCommand), new Events.EventHandler(this.onRequestTagInEvent));
		this.events.Subscribe(typeof(AttemptFriendlyAssistCommand), new Events.EventHandler(this.onAttemptFriendlyAssist));
		this.events.Subscribe(typeof(DespawnCharacterCommand), new Events.EventHandler(this.onDespawnCommand));
		this.events.Subscribe(typeof(AttemptTeamPowerMoveCommand), new Events.EventHandler(this.onAttemptTeamPowerMove));
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00087AEE File Offset: 0x00085EEE
	private int friendlyAssistCount
	{
		get
		{
			return this.battleSettings.settings[BattleSettingType.Assists];
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06001A24 RID: 6692 RVA: 0x00087B01 File Offset: 0x00085F01
	private bool assistStealingEnabled
	{
		get
		{
			return this.battleSettings.settings.ContainsKey(BattleSettingType.AssistStealing) && this.battleSettings.settings[BattleSettingType.AssistStealing] != 0;
		}
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x00087B33 File Offset: 0x00085F33
	public int GetAssistsRemaining(PlayerNum player)
	{
		return this.friendlyAssistCount - this.GetAssistsUsed(player);
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x00087B43 File Offset: 0x00085F43
	public int GetAssistsUsed(PlayerNum player)
	{
		if (!this.state.assistsUsed.ContainsKey(player))
		{
			return 0;
		}
		return this.state.assistsUsed[player];
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00087B70 File Offset: 0x00085F70
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

	// Token: 0x06001A28 RID: 6696 RVA: 0x00087BD0 File Offset: 0x00085FD0
	public PlayerReference GetPlayerRef(PlayerNum playerNum)
	{
		foreach (TeamNum key in this.teamPlayers.Keys)
		{
			for (int i = 0; i < this.teamPlayers[key].Count; i++)
			{
				PlayerReference playerReference = this.teamPlayers[key][i];
				if (playerReference.PlayerNum == playerNum)
				{
					return playerReference;
				}
			}
		}
		return null;
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x00087C78 File Offset: 0x00086078
	protected PlayerReference getNextPlayer(TeamNum team, PlayerReference currentPlayer)
	{
		List<PlayerReference> list = this.teamPlayers[team];
		int num = list.IndexOf(currentPlayer);
		if (num < 0)
		{
			Debug.LogError(string.Concat(new object[]
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

	// Token: 0x06001A2A RID: 6698 RVA: 0x00087CEC File Offset: 0x000860EC
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

	// Token: 0x06001A2B RID: 6699 RVA: 0x00087D5C File Offset: 0x0008615C
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

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00087DC8 File Offset: 0x000861C8
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

	// Token: 0x06001A2D RID: 6701 RVA: 0x00087E28 File Offset: 0x00086228
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

	// Token: 0x06001A2E RID: 6702 RVA: 0x00087E8C File Offset: 0x0008628C
	private int getFramesSinceLastAssist(PlayerNum playerNum)
	{
		if (this.state.lastAssistEndTime.ContainsKey(playerNum))
		{
			return this.state.frameCount - this.state.lastAssistEndTime[playerNum];
		}
		return -1;
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x00087EC3 File Offset: 0x000862C3
	private int getFramesSinceLastAssist(TeamNum team)
	{
		if (this.state.lastAssistTeamEndTime.ContainsKey(team))
		{
			return this.state.frameCount - this.state.lastAssistTeamEndTime[team];
		}
		return -1;
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x00087EFA File Offset: 0x000862FA
	private int getFramesSinceAssistStart(TeamNum team)
	{
		if (this.state.lastAssistTeamStartTime.ContainsKey(team))
		{
			return this.state.frameCount - this.state.lastAssistTeamStartTime[team];
		}
		return -1;
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x00087F34 File Offset: 0x00086334
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
								foreach (PlayerController playerController in playerReference3.AllControllers)
								{
									playerController.Model.temporaryDurationFrames = this.crewBattleData.endAssistDelayFrames;
								}
							}
							else if (this.crewBattleData.grantFramesOnKill > 0)
							{
								foreach (PlayerController playerController2 in playerReference3.AllControllers)
								{
									playerController2.Model.temporaryDurationFrames += this.crewBattleData.grantFramesOnKill;
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

	// Token: 0x06001A32 RID: 6706 RVA: 0x000882F0 File Offset: 0x000866F0
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

	// Token: 0x06001A33 RID: 6707 RVA: 0x00088374 File Offset: 0x00086774
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

	// Token: 0x06001A34 RID: 6708 RVA: 0x0008841C File Offset: 0x0008681C
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
			Debug.LogError("Player died on invalid team " + player.Team);
		}
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000884C8 File Offset: 0x000868C8
	private void onAttemptFriendlyAssist(GameEvent message)
	{
		AttemptFriendlyAssistCommand attemptFriendlyAssistCommand = message as AttemptFriendlyAssistCommand;
		PlayerReference playerReference = this.references[attemptFriendlyAssistCommand.playerNum];
		bool flag = false;
		foreach (PlayerController controller in playerReference.AllControllers)
		{
			this.assistSpawnHelper.BeginContext(controller, this.teamPlayers);
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

	// Token: 0x06001A36 RID: 6710 RVA: 0x00088588 File Offset: 0x00086988
	public bool CanAssist(PlayerReference playerRef)
	{
		this.assistSpawnHelper.BeginContext(playerRef.Controller, this.teamPlayers);
		return this.isAssistPossible(playerRef);
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000885A8 File Offset: 0x000869A8
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

	// Token: 0x06001A38 RID: 6712 RVA: 0x00088728 File Offset: 0x00086B28
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

	// Token: 0x06001A39 RID: 6713 RVA: 0x00088783 File Offset: 0x00086B83
	private int getAssistsUsed(PlayerNum playerNum)
	{
		if (!this.state.assistsUsed.ContainsKey(playerNum))
		{
			return 0;
		}
		return this.state.assistsUsed[playerNum];
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06001A3A RID: 6714 RVA: 0x000887AE File Offset: 0x00086BAE
	private int assisterStartingDamage
	{
		get
		{
			return this.crewBattleData.assistPlayerBaseSpawnDamage;
		}
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000887BC File Offset: 0x00086BBC
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
			foreach (CharacterComponent characterComponent in controller.CharacterData.components)
			{
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

	// Token: 0x06001A3C RID: 6716 RVA: 0x00088928 File Offset: 0x00086D28
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

	// Token: 0x06001A3D RID: 6717 RVA: 0x00088A64 File Offset: 0x00086E64
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

	// Token: 0x06001A3E RID: 6718 RVA: 0x00088B10 File Offset: 0x00086F10
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

	// Token: 0x06001A3F RID: 6719 RVA: 0x00088B84 File Offset: 0x00086F84
	private PlayerNum findRespawnerForTeam(TeamNum team)
	{
		foreach (PlayerReference playerReference in this.teamPlayers[team])
		{
			if (base.IsRespawning(playerReference.PlayerNum))
			{
				return playerReference.PlayerNum;
			}
		}
		return PlayerNum.None;
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x00088C00 File Offset: 0x00087000
	public bool CanTagIn(PlayerNum player)
	{
		PlayerReference playerRef = this.GetPlayerRef(player);
		return this.GetPrimaryPlayer(playerRef.Team) == PlayerNum.None && !playerRef.IsEliminated && this.state.tagInFrames[playerRef.Team] >= this.crewBattleData.respawnBaseWait && (base.IsRespawning(player) || this.isAllowedTagIn(playerRef) || this.state.tagInFrames[playerRef.Team] >= this.crewBattleData.benchedPlayerTagInDelayFrames + this.crewBattleData.respawnBaseWait);
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x00088CAC File Offset: 0x000870AC
	public bool DisplayTagInOptionInHUD()
	{
		return this.crewBattleData.respawnBaseWait > 0;
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x00088CBC File Offset: 0x000870BC
	private bool isAllowedTagIn(PlayerReference playerRef)
	{
		return this.crewBattleData.allPlayersRotate && (!this.state.previousPrimaryPlayer.ContainsKey(playerRef.Team) || this.state.previousPrimaryPlayer[playerRef.Team] != playerRef.PlayerNum);
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x00088D1A File Offset: 0x0008711A
	private void tagIn(PlayerNum currentPlayer, TeamNum team, PlayerNum taggedPlayer)
	{
		this.state.didTagIn[taggedPlayer] = true;
		base.swapRespawn(currentPlayer, taggedPlayer);
		this.events.Broadcast(new TagInPlayerEvent(team, taggedPlayer));
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x00088D4C File Offset: 0x0008714C
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
		foreach (PlayerController playerController in playerReference.AllControllers)
		{
			playerController.Despawn();
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x00088DE0 File Offset: 0x000871E0
	private void onAttemptTeamPowerMove(GameEvent message)
	{
		AttemptTeamPowerMoveCommand attemptTeamPowerMoveCommand = message as AttemptTeamPowerMoveCommand;
		PlayerController playerController = null;
		foreach (PlayerReference playerReference in base.gameController.currentGame.PlayerReferences)
		{
			if (attemptTeamPowerMoveCommand.playerNum == playerReference.PlayerNum)
			{
				playerController = playerReference.Controller;
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
			foreach (PlayerReference playerReference2 in base.gameController.currentGame.PlayerReferences)
			{
				if (playerReference2.Controller != null && playerReference2.PlayerNum != playerController.PlayerNum && playerReference2.Controller.Team == attemptTeamPowerMoveCommand.team && playerReference2.CanHostTeamMove)
				{
					if (this.assistSpawnHelper.PrepareForPowerMove())
					{
						this.trySpawnPowerMove(attemptTeamPowerMoveCommand);
					}
					else
					{
						playerReference2.Controller.TeamPowerMove(attemptTeamPowerMoveCommand.spawnParticle, attemptTeamPowerMoveCommand.assistArticles);
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

	// Token: 0x06001A46 RID: 6726 RVA: 0x00088F9C File Offset: 0x0008739C
	private void onAssistPlayerRemoved(PlayerReference reference)
	{
		this.state.lastAssistEndTime[reference.PlayerNum] = this.state.frameCount;
		this.state.lastAssistTeamEndTime[reference.Team] = this.state.frameCount;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x00088FEB File Offset: 0x000873EB
	public override bool LoadState(RollbackStateContainer container)
	{
		base.LoadState(container);
		container.ReadState<CrewBattlePlayerSpawnerState>(ref this.state);
		return true;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x00089003 File Offset: 0x00087403
	public override bool ExportState(ref RollbackStateContainer container)
	{
		base.ExportState(ref container);
		return container.WriteState(base.rollbackStatePooling.Clone<CrewBattlePlayerSpawnerState>(this.state));
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x00089028 File Offset: 0x00087428
	public override void Destroy()
	{
		this.events.Unsubscribe(typeof(RequestTagInCommand), new Events.EventHandler(this.onRequestTagInEvent));
		this.events.Unsubscribe(typeof(AttemptFriendlyAssistCommand), new Events.EventHandler(this.onAttemptFriendlyAssist));
		this.events.Unsubscribe(typeof(DespawnCharacterCommand), new Events.EventHandler(this.onDespawnCommand));
		this.events.Unsubscribe(typeof(AttemptTeamPowerMoveCommand), new Events.EventHandler(this.onAttemptTeamPowerMove));
		base.Destroy();
	}

	// Token: 0x0400138A RID: 5002
	private CrewBattleCustomData crewBattleData;

	// Token: 0x0400138B RID: 5003
	private BattleSettings battleSettings;

	// Token: 0x0400138C RID: 5004
	private CrewBattlePlayerSpawnerState state;

	// Token: 0x0400138D RID: 5005
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;
}
