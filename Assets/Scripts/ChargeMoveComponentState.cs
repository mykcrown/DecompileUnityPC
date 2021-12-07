// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class ChargeMoveComponentState : RollbackStateTyped<ChargeMoveComponentState>
{
	public int storedChargeFrames;

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[NonSerialized]
	public GameObject chargeParticle;

	public override void CopyTo(ChargeMoveComponentState target)
	{
		target.storedChargeFrames = this.storedChargeFrames;
		target.chargeParticle = this.chargeParticle;
	}
}
