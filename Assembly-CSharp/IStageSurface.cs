using System;

// Token: 0x02000650 RID: 1616
public interface IStageSurface : ITickable, IMovingObject
{
	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x0600279D RID: 10141
	bool IsPlatform { get; }

	// Token: 0x0600279E RID: 10142
	void UpdateCollisionData();
}
