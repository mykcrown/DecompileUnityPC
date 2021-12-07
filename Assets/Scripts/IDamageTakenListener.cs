// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IDamageTakenListener
{
	void OnDamageTaken(Fixed damage, ImpactType impactType);
}
