using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x02000498 RID: 1176
public class CrewAssistSpawnHelper : ICrewAssistSpawnHelper
{
	// Token: 0x060019B8 RID: 6584 RVA: 0x000853C8 File Offset: 0x000837C8
	public CrewAssistSpawnHelper()
	{
		this.flipOptionMap[AssistSpawnFlipOffsetOption.CenterStage] = new FlipOffsetOptionHandler(this.SpawnFlip_CenterStage);
		this.flipOptionMap[AssistSpawnFlipOffsetOption.Facing] = new FlipOffsetOptionHandler(this.SpawnFlip_Facing);
		this.flipOptionMap[AssistSpawnFlipOffsetOption.Opponent] = new FlipOffsetOptionHandler(this.SpawnFlip_Opponent);
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x060019B9 RID: 6585 RVA: 0x0008542E File Offset: 0x0008382E
	// (set) Token: 0x060019BA RID: 6586 RVA: 0x00085436 File Offset: 0x00083836
	public PlayerController Controller { get; private set; }

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x060019BB RID: 6587 RVA: 0x0008543F File Offset: 0x0008383F
	// (set) Token: 0x060019BC RID: 6588 RVA: 0x00085447 File Offset: 0x00083847
	public MoveData Move { get; private set; }

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x060019BD RID: 6589 RVA: 0x00085450 File Offset: 0x00083850
	// (set) Token: 0x060019BE RID: 6590 RVA: 0x00085458 File Offset: 0x00083858
	public AssistAttackComponent MoveComponent { get; private set; }

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x060019BF RID: 6591 RVA: 0x00085461 File Offset: 0x00083861
	// (set) Token: 0x060019C0 RID: 6592 RVA: 0x00085469 File Offset: 0x00083869
	public PlayerReference Teammate { get; private set; }

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x060019C1 RID: 6593 RVA: 0x00085472 File Offset: 0x00083872
	// (set) Token: 0x060019C2 RID: 6594 RVA: 0x0008547A File Offset: 0x0008387A
	public PlayerReference Opponent { get; private set; }

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x060019C3 RID: 6595 RVA: 0x00085483 File Offset: 0x00083883
	// (set) Token: 0x060019C4 RID: 6596 RVA: 0x0008548B File Offset: 0x0008388B
	public HorizontalDirection Facing { get; private set; }

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x060019C5 RID: 6597 RVA: 0x00085494 File Offset: 0x00083894
	// (set) Token: 0x060019C6 RID: 6598 RVA: 0x0008549C File Offset: 0x0008389C
	public Vector3F SpawnPoint { get; private set; }

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x060019C7 RID: 6599 RVA: 0x000854A5 File Offset: 0x000838A5
	// (set) Token: 0x060019C8 RID: 6600 RVA: 0x000854AD File Offset: 0x000838AD
	public Vector3F TargetPoint { get; private set; }

	// Token: 0x060019C9 RID: 6601 RVA: 0x000854B6 File Offset: 0x000838B6
	public void BeginContext(PlayerController controller, Dictionary<TeamNum, List<PlayerReference>> teamPlayers)
	{
		this.teamPlayers = teamPlayers;
		this.Controller = controller;
		this.Teammate = this.GetTeammate();
		this.Opponent = this.GetOpponent();
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000854DE File Offset: 0x000838DE
	public void PrepareForAssist()
	{
		this.Move = this.getMove(MoveLabel.AssistAttack);
		this.MoveComponent = this.getComponent();
		this.SpawnPoint = this.getAssistSpawnPoint();
		this.TargetPoint = this.getTargetSpawnPoint();
		this.Facing = this.getSpawnFacing();
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00085520 File Offset: 0x00083920
	public bool CannotProceed()
	{
		return this.Teammate == null || this.Opponent == null || this.Teammate.Controller == null || this.Opponent.Controller == null || !this.Teammate.CanHostTeamMove || !this.Opponent.CanHostTeamMove;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00085590 File Offset: 0x00083990
	public bool PrepareForPowerMove()
	{
		MoveData[] moves = this.Controller.MoveSet.GetMoves(MoveLabel.AllyAssist);
		foreach (MoveData moveData in moves)
		{
			if (moveData.requiredMoves == null || moveData.requiredMoves.Length == 0)
			{
				this.Move = moveData;
				break;
			}
		}
		this.MoveComponent = this.getComponent();
		this.SpawnPoint = this.Teammate.Controller.Position;
		this.TargetPoint = this.SpawnPoint;
		this.Facing = this.Teammate.Controller.Facing;
		if (this.MoveComponent)
		{
			PlayerController playerController = this.Teammate.Controller;
			if (this.MoveComponent.targetOption == AssistTargetOption.Opponent)
			{
				playerController = ((this.Opponent == null || !(this.Opponent.Controller != null)) ? this.Teammate.Controller : this.Opponent.Controller);
				this.SpawnPoint = playerController.Position;
			}
			this.SpawnPoint += new Vector3F((playerController.Facing != HorizontalDirection.Left) ? this.MoveComponent.spawnXData.spawnOffset : (-this.MoveComponent.spawnXData.spawnOffset), 0, 0);
			if (this.MoveComponent.spawnXData.spawnFlipOffsetOption == AssistSpawnFlipOffsetOption.Opponent)
			{
				playerController = ((this.Opponent == null || !(this.Opponent.Controller != null)) ? this.Teammate.Controller : this.Opponent.Controller);
				this.Facing = ((!(this.SpawnPoint.x < playerController.Position.x)) ? HorizontalDirection.Left : HorizontalDirection.Right);
			}
		}
		return this.Move != null;
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x0008579C File Offset: 0x00083B9C
	private MoveData getMove(MoveLabel moveType = MoveLabel.AssistAttack)
	{
		MoveData result = null;
		MoveData y = null;
		TotemDuoComponent characterComponent = this.Controller.GetCharacterComponent<TotemDuoComponent>();
		if (characterComponent != null)
		{
			y = characterComponent.partnerAssistMove;
		}
		MoveData[] moves = this.Controller.MoveSet.GetMoves(moveType);
		foreach (MoveData moveData in moves)
		{
			if (moveData == y && !this.Controller.IsActive)
			{
				result = moveData;
				break;
			}
			if (moveData != y && this.Controller.IsActive)
			{
				result = moveData;
				break;
			}
		}
		return result;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x0008584C File Offset: 0x00083C4C
	private AssistAttackComponent getComponent()
	{
		AssistAttackComponent result = null;
		if (this.Move != null)
		{
			result = this.Move.GetComponent<AssistAttackComponent>();
		}
		return result;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x0008587C File Offset: 0x00083C7C
	public PlayerReference GetOpponent()
	{
		foreach (TeamNum teamNum in this.teamPlayers.Keys)
		{
			if (teamNum != this.Controller.Team)
			{
				for (int i = this.teamPlayers[teamNum].Count - 1; i >= 0; i--)
				{
					PlayerReference playerReference = this.teamPlayers[teamNum][i];
					if (this.isActivePrimaryCombatant(playerReference))
					{
						return playerReference;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x00085938 File Offset: 0x00083D38
	public PlayerReference GetTeammate()
	{
		foreach (TeamNum teamNum in this.teamPlayers.Keys)
		{
			if (teamNum == this.Controller.Team)
			{
				for (int i = this.teamPlayers[teamNum].Count - 1; i >= 0; i--)
				{
					PlayerReference playerReference = this.teamPlayers[teamNum][i];
					if (this.isActivePrimaryCombatant(playerReference))
					{
						return playerReference;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000859F4 File Offset: 0x00083DF4
	private bool isActivePrimaryCombatant(PlayerReference playerRef)
	{
		return playerRef.IsInBattle && !playerRef.IsTemporary && !playerRef.IsAllyAssistMove && !playerRef.Controller.Model.isRespawning && !playerRef.Controller.Model.isDead;
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x00085A58 File Offset: 0x00083E58
	private HorizontalDirection getSpawnFacing()
	{
		if (this.TargetPoint.x < this.SpawnPoint.x)
		{
			return HorizontalDirection.Left;
		}
		return HorizontalDirection.Right;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x00085A90 File Offset: 0x00083E90
	public Vector3F getTargetSpawnPoint()
	{
		PlayerReference assistTargetPlayer = this.getAssistTargetPlayer(this.MoveComponent.targetOption);
		return this.getAssistTargetPoint(assistTargetPlayer, this.MoveComponent.targetOption);
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x00085AC3 File Offset: 0x00083EC3
	public Vector3F getAssistSpawnPoint()
	{
		return new Vector3F(this.getAssistSpawnPointX(), this.getAssistSpawnPointY(), this.getAssistSpawnPointZ());
	}

	// Token: 0x060019D5 RID: 6613 RVA: 0x00085ADC File Offset: 0x00083EDC
	private Fixed getAssistSpawnPointZ()
	{
		AssistAttackSpawnAxisData spawnZData = this.MoveComponent.spawnZData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnZData.spawnMethod);
		Fixed z = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnZData.spawnMethod).z;
		return this.manageAxisResult(z, assistSpawnPlayerRef, spawnZData);
	}

	// Token: 0x060019D6 RID: 6614 RVA: 0x00085B30 File Offset: 0x00083F30
	private Fixed getAssistSpawnPointY()
	{
		AssistAttackSpawnAxisData spawnYData = this.MoveComponent.spawnYData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnYData.spawnMethod);
		Fixed y = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnYData.spawnMethod).y;
		return this.manageAxisResult(y, assistSpawnPlayerRef, spawnYData);
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x00085B84 File Offset: 0x00083F84
	private Fixed getAssistSpawnPointX()
	{
		AssistAttackSpawnAxisData spawnXData = this.MoveComponent.spawnXData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnXData.spawnMethod);
		Fixed x = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnXData.spawnMethod).x;
		return this.manageAxisResult(x, assistSpawnPlayerRef, spawnXData);
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x00085BD8 File Offset: 0x00083FD8
	private Fixed manageAxisResult(Fixed result, PlayerReference targetPlayer, AssistAttackSpawnAxisData axisData)
	{
		if (this.shouldFlipSpawnPosition(targetPlayer, axisData))
		{
			result -= axisData.spawnOffset;
		}
		else
		{
			result += axisData.spawnOffset;
		}
		if (axisData.enforceMaxValue)
		{
			result = FixedMath.Max(axisData.minValue, result);
		}
		if (axisData.enforceMaxValue)
		{
			result = FixedMath.Min(axisData.maxValue, result);
		}
		return result;
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00085C46 File Offset: 0x00084046
	private bool shouldFlipSpawnPosition(PlayerReference spawnPlayer, AssistAttackSpawnAxisData axisData)
	{
		return this.flipOptionMap.ContainsKey(axisData.spawnFlipOffsetOption) && this.flipOptionMap[axisData.spawnFlipOffsetOption](spawnPlayer);
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x00085C77 File Offset: 0x00084077
	private bool SpawnFlip_Facing(PlayerReference spawnPlayer)
	{
		return spawnPlayer != null && spawnPlayer.Controller.Facing == HorizontalDirection.Left;
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x00085C94 File Offset: 0x00084094
	private bool SpawnFlip_Opponent(PlayerReference spawnPlayer)
	{
		return this.Opponent != null && spawnPlayer != null && spawnPlayer.Controller.Physics.Center.x > this.Opponent.Controller.Physics.Center.x;
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x00085CF4 File Offset: 0x000840F4
	private bool SpawnFlip_CenterStage(PlayerReference spawnPlayer)
	{
		return this.Opponent != null && spawnPlayer != null && this.Opponent.Controller.Physics.Center.x > 0;
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x00085D40 File Offset: 0x00084140
	private Vector3F getAssistSpawnTarget(PlayerController controller, PlayerReference targetPlayer, AssistSpawnMethod method)
	{
		if (method == AssistSpawnMethod.Midpoint)
		{
			if (this.Opponent != null && this.Teammate != null)
			{
				return Vector3F.Lerp(this.Teammate.Controller.Physics.Center, this.Opponent.Controller.Physics.Center, (Fixed)0.5);
			}
			if (this.Opponent != null)
			{
				return this.Opponent.Controller.Physics.Center;
			}
			if (this.Teammate != null)
			{
				return this.Teammate.Controller.Physics.Center;
			}
		}
		else if (targetPlayer != null)
		{
			return targetPlayer.Controller.Physics.Center;
		}
		return default(Vector3F);
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x00085E0F File Offset: 0x0008420F
	private PlayerReference getAssistSpawnPlayerRef(PlayerController controller, AssistSpawnMethod method)
	{
		if (method == AssistSpawnMethod.Opponent)
		{
			return this.Opponent;
		}
		if (method == AssistSpawnMethod.Midpoint)
		{
			return this.Teammate;
		}
		if (method == AssistSpawnMethod.Teammate)
		{
			return this.Teammate;
		}
		return null;
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x00085E3C File Offset: 0x0008423C
	private Vector3F getAssistTargetPoint(PlayerReference targetPlayer, AssistTargetOption option)
	{
		if (targetPlayer != null)
		{
			return targetPlayer.Controller.Physics.Center;
		}
		return default(Vector3F);
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x00085E69 File Offset: 0x00084269
	private PlayerReference getAssistTargetPlayer(AssistTargetOption option)
	{
		if (option == AssistTargetOption.Opponent)
		{
			return this.Opponent;
		}
		return null;
	}

	// Token: 0x04001348 RID: 4936
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;

	// Token: 0x04001351 RID: 4945
	private Dictionary<AssistSpawnFlipOffsetOption, FlipOffsetOptionHandler> flipOptionMap = new Dictionary<AssistSpawnFlipOffsetOption, FlipOffsetOptionHandler>();
}
