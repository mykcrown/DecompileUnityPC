// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IBoundsOwner
{
	EnvironmentBounds Bounds
	{
		get;
	}

	bool AllowPushing
	{
		get;
	}

	bool AllowTotalShove
	{
		get;
	}

	Vector3F Position
	{
		get;
	}

	Vector3F MovementVelocity
	{
		get;
	}

	CharacterData CharacterData
	{
		get;
	}

	HorizontalDirection Facing
	{
		get;
	}

	PlayerPhysicsController Physics
	{
		get;
	}
}
