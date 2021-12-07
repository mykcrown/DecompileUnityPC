using System;
using FixedPoint;

// Token: 0x0200045D RID: 1117
public interface ITrailOwner
{
	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06001711 RID: 5905
	bool EmitTrail { get; }

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06001712 RID: 5906
	Vector3F EmitPosition { get; }
}
