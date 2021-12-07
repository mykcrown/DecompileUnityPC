using System;
using FixedPoint;

// Token: 0x02000522 RID: 1314
public class MoveTransferSettings
{
	// Token: 0x04001754 RID: 5972
	public bool transitioningToContinuingMove;

	// Token: 0x04001755 RID: 5973
	public bool transferHitDisableTargets;

	// Token: 0x04001756 RID: 5974
	public bool transferChargeData;

	// Token: 0x04001757 RID: 5975
	public bool transferStale;

	// Token: 0x04001758 RID: 5976
	public Fixed transferStaleMulti = 1;

	// Token: 0x04001759 RID: 5977
	public Fixed chargeFractionOverride = -1;

	// Token: 0x0400175A RID: 5978
	public static readonly MoveTransferSettings Default = new MoveTransferSettings();
}
