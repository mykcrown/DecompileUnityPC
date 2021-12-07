// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IRespawnController : IRollbackStateOwner, ITickable
{
	Vector3F Position
	{
		get;
	}

	bool IsRespawning
	{
		get;
	}

	bool HasArrived
	{
		get;
	}

	void StartRespawn(SpawnPointBase point);

	void Dismount();
}
