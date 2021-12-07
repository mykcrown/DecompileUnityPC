// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IPhysicsStateOwner
{
	MoveData CurrentMove
	{
		get;
	}

	PhysicsOverride PhysicsOverride
	{
		get;
		set;
	}

	Vector3F Velocity
	{
		get;
	}
}
