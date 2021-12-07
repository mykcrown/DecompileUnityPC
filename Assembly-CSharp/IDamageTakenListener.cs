using System;
using FixedPoint;

// Token: 0x020005BA RID: 1466
public interface IDamageTakenListener
{
	// Token: 0x060020B2 RID: 8370
	void OnDamageTaken(Fixed damage, ImpactType impactType);
}
