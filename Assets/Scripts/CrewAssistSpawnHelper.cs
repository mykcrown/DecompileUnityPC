// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class CrewAssistSpawnHelper : ICrewAssistSpawnHelper
{
	private Dictionary<TeamNum, List<PlayerReference>> teamPlayers;

	private Dictionary<AssistSpawnFlipOffsetOption, FlipOffsetOptionHandler> flipOptionMap = new Dictionary<AssistSpawnFlipOffsetOption, FlipOffsetOptionHandler>();

	public PlayerController Controller
	{
		get;
		private set;
	}

	public MoveData Move
	{
		get;
		private set;
	}

	public AssistAttackComponent MoveComponent
	{
		get;
		private set;
	}

	public PlayerReference Teammate
	{
		get;
		private set;
	}

	public PlayerReference Opponent
	{
		get;
		private set;
	}

	public HorizontalDirection Facing
	{
		get;
		private set;
	}

	public Vector3F SpawnPoint
	{
		get;
		private set;
	}

	public Vector3F TargetPoint
	{
		get;
		private set;
	}

	public CrewAssistSpawnHelper()
	{
		this.flipOptionMap[AssistSpawnFlipOffsetOption.CenterStage] = new FlipOffsetOptionHandler(this.SpawnFlip_CenterStage);
		this.flipOptionMap[AssistSpawnFlipOffsetOption.Facing] = new FlipOffsetOptionHandler(this.SpawnFlip_Facing);
		this.flipOptionMap[AssistSpawnFlipOffsetOption.Opponent] = new FlipOffsetOptionHandler(this.SpawnFlip_Opponent);
	}

	public void BeginContext(PlayerController controller, Dictionary<TeamNum, List<PlayerReference>> teamPlayers)
	{
		this.teamPlayers = teamPlayers;
		this.Controller = controller;
		this.Teammate = this.GetTeammate();
		this.Opponent = this.GetOpponent();
	}

	public void PrepareForAssist()
	{
		this.Move = this.getMove(MoveLabel.AssistAttack);
		this.MoveComponent = this.getComponent();
		this.SpawnPoint = this.getAssistSpawnPoint();
		this.TargetPoint = this.getTargetSpawnPoint();
		this.Facing = this.getSpawnFacing();
	}

	public bool CannotProceed()
	{
		return this.Teammate == null || this.Opponent == null || this.Teammate.Controller == null || this.Opponent.Controller == null || !this.Teammate.CanHostTeamMove || !this.Opponent.CanHostTeamMove;
	}

	public bool PrepareForPowerMove()
	{
		MoveData[] moves = this.Controller.MoveSet.GetMoves(MoveLabel.AllyAssist);
		MoveData[] array = moves;
		for (int i = 0; i < array.Length; i++)
		{
			MoveData moveData = array[i];
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
		MoveData[] array = moves;
		for (int i = 0; i < array.Length; i++)
		{
			MoveData moveData = array[i];
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

	private AssistAttackComponent getComponent()
	{
		AssistAttackComponent result = null;
		if (this.Move != null)
		{
			result = this.Move.GetComponent<AssistAttackComponent>();
		}
		return result;
	}

	public PlayerReference GetOpponent()
	{
		foreach (TeamNum current in this.teamPlayers.Keys)
		{
			if (current != this.Controller.Team)
			{
				for (int i = this.teamPlayers[current].Count - 1; i >= 0; i--)
				{
					PlayerReference playerReference = this.teamPlayers[current][i];
					if (this.isActivePrimaryCombatant(playerReference))
					{
						return playerReference;
					}
				}
			}
		}
		return null;
	}

	public PlayerReference GetTeammate()
	{
		foreach (TeamNum current in this.teamPlayers.Keys)
		{
			if (current == this.Controller.Team)
			{
				for (int i = this.teamPlayers[current].Count - 1; i >= 0; i--)
				{
					PlayerReference playerReference = this.teamPlayers[current][i];
					if (this.isActivePrimaryCombatant(playerReference))
					{
						return playerReference;
					}
				}
			}
		}
		return null;
	}

	private bool isActivePrimaryCombatant(PlayerReference playerRef)
	{
		return playerRef.IsInBattle && !playerRef.IsTemporary && !playerRef.IsAllyAssistMove && !playerRef.Controller.Model.isRespawning && !playerRef.Controller.Model.isDead;
	}

	private HorizontalDirection getSpawnFacing()
	{
		if (this.TargetPoint.x < this.SpawnPoint.x)
		{
			return HorizontalDirection.Left;
		}
		return HorizontalDirection.Right;
	}

	public Vector3F getTargetSpawnPoint()
	{
		PlayerReference assistTargetPlayer = this.getAssistTargetPlayer(this.MoveComponent.targetOption);
		return this.getAssistTargetPoint(assistTargetPlayer, this.MoveComponent.targetOption);
	}

	public Vector3F getAssistSpawnPoint()
	{
		return new Vector3F(this.getAssistSpawnPointX(), this.getAssistSpawnPointY(), this.getAssistSpawnPointZ());
	}

	private Fixed getAssistSpawnPointZ()
	{
		AssistAttackSpawnAxisData spawnZData = this.MoveComponent.spawnZData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnZData.spawnMethod);
		Fixed z = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnZData.spawnMethod).z;
		return this.manageAxisResult(z, assistSpawnPlayerRef, spawnZData);
	}

	private Fixed getAssistSpawnPointY()
	{
		AssistAttackSpawnAxisData spawnYData = this.MoveComponent.spawnYData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnYData.spawnMethod);
		Fixed y = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnYData.spawnMethod).y;
		return this.manageAxisResult(y, assistSpawnPlayerRef, spawnYData);
	}

	private Fixed getAssistSpawnPointX()
	{
		AssistAttackSpawnAxisData spawnXData = this.MoveComponent.spawnXData;
		PlayerReference assistSpawnPlayerRef = this.getAssistSpawnPlayerRef(this.Controller, spawnXData.spawnMethod);
		Fixed x = this.getAssistSpawnTarget(this.Controller, assistSpawnPlayerRef, spawnXData.spawnMethod).x;
		return this.manageAxisResult(x, assistSpawnPlayerRef, spawnXData);
	}

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

	private bool shouldFlipSpawnPosition(PlayerReference spawnPlayer, AssistAttackSpawnAxisData axisData)
	{
		return this.flipOptionMap.ContainsKey(axisData.spawnFlipOffsetOption) && this.flipOptionMap[axisData.spawnFlipOffsetOption](spawnPlayer);
	}

	private bool SpawnFlip_Facing(PlayerReference spawnPlayer)
	{
		return spawnPlayer != null && spawnPlayer.Controller.Facing == HorizontalDirection.Left;
	}

	private bool SpawnFlip_Opponent(PlayerReference spawnPlayer)
	{
		return this.Opponent != null && spawnPlayer != null && spawnPlayer.Controller.Physics.Center.x > this.Opponent.Controller.Physics.Center.x;
	}

	private bool SpawnFlip_CenterStage(PlayerReference spawnPlayer)
	{
		return this.Opponent != null && spawnPlayer != null && this.Opponent.Controller.Physics.Center.x > 0;
	}

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

	private Vector3F getAssistTargetPoint(PlayerReference targetPlayer, AssistTargetOption option)
	{
		if (targetPlayer != null)
		{
			return targetPlayer.Controller.Physics.Center;
		}
		return default(Vector3F);
	}

	private PlayerReference getAssistTargetPlayer(AssistTargetOption option)
	{
		if (option == AssistTargetOption.Opponent)
		{
			return this.Opponent;
		}
		return null;
	}
}
