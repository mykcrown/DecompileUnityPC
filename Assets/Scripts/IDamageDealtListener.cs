// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IDamageDealtListener
{
	void OnDamageDealt(Fixed damage, ImpactType impactType, bool chargesMeter);
}
