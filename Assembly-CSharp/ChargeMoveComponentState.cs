using System;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
[Serializable]
public class ChargeMoveComponentState : RollbackStateTyped<ChargeMoveComponentState>
{
	// Token: 0x060020D9 RID: 8409 RVA: 0x000A4A85 File Offset: 0x000A2E85
	public override void CopyTo(ChargeMoveComponentState target)
	{
		target.storedChargeFrames = this.storedChargeFrames;
		target.chargeParticle = this.chargeParticle;
	}

	// Token: 0x04001A04 RID: 6660
	public int storedChargeFrames;

	// Token: 0x04001A05 RID: 6661
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public GameObject chargeParticle;
}
