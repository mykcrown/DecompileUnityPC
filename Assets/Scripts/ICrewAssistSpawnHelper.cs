// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public interface ICrewAssistSpawnHelper
{
	Vector3F SpawnPoint
	{
		get;
	}

	Vector3F TargetPoint
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	AssistAttackComponent MoveComponent
	{
		get;
	}

	MoveData Move
	{
		get;
	}

	PlayerController Controller
	{
		get;
	}

	PlayerReference Teammate
	{
		get;
	}

	PlayerReference Opponent
	{
		get;
	}

	void BeginContext(PlayerController controller, Dictionary<TeamNum, List<PlayerReference>> teamPlayers);

	bool CannotProceed();

	void PrepareForAssist();

	bool PrepareForPowerMove();

	PlayerReference GetTeammate();

	PlayerReference GetOpponent();
}
