using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x0200062F RID: 1583
[Serializable]
public class LedgeModel : StageObjectModel<LedgeModel>
{
	// Token: 0x060026E6 RID: 9958 RVA: 0x000BE52C File Offset: 0x000BC92C
	public override void CopyTo(LedgeModel target)
	{
		base.CopyTo(target);
		target.playerSlots.Clear();
		foreach (KeyValuePair<int, PlayerNum> keyValuePair in this.playerSlots)
		{
			target.playerSlots[keyValuePair.Key] = keyValuePair.Value;
		}
		target.position = this.position;
	}

	// Token: 0x04001C76 RID: 7286
	[IsClonedManually]
	[IgnoreCopyValidation]
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	public Dictionary<int, PlayerNum> playerSlots = new Dictionary<int, PlayerNum>();

	// Token: 0x04001C77 RID: 7287
	public Vector3F position;
}
