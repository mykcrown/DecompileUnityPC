using System;
using FixedPoint;

// Token: 0x020005B9 RID: 1465
public interface IDamageDealtListener
{
	// Token: 0x060020B1 RID: 8369
	void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter);
}
