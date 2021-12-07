using System;
using FixedPoint;

// Token: 0x02000572 RID: 1394
public interface IPhysicsStateOwner
{
	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06001F10 RID: 7952
	MoveData CurrentMove { get; }

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06001F11 RID: 7953
	// (set) Token: 0x06001F12 RID: 7954
	PhysicsOverride PhysicsOverride { get; set; }

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06001F13 RID: 7955
	Vector3F Velocity { get; }
}
