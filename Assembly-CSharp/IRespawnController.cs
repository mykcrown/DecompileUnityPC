using System;
using FixedPoint;

// Token: 0x0200061E RID: 1566
public interface IRespawnController : IRollbackStateOwner, ITickable
{
	// Token: 0x060026AB RID: 9899
	void StartRespawn(SpawnPointBase point);

	// Token: 0x060026AC RID: 9900
	void Dismount();

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x060026AD RID: 9901
	Vector3F Position { get; }

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x060026AE RID: 9902
	bool IsRespawning { get; }

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x060026AF RID: 9903
	bool HasArrived { get; }
}
