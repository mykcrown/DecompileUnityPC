using System;
using FixedPoint;

// Token: 0x02000353 RID: 851
[RollbackStatePoolMultiplier(2)]
[Serializable]
public class AnnouncementStatModel : RollbackStateTyped<AnnouncementStatModel>
{
	// Token: 0x060011F3 RID: 4595 RVA: 0x00067757 File Offset: 0x00065B57
	public override void CopyTo(AnnouncementStatModel target)
	{
		target.currentQuantity = this.currentQuantity;
		target.lastRecordedSeconds = this.lastRecordedSeconds;
	}

	// Token: 0x04000B83 RID: 2947
	public int currentQuantity;

	// Token: 0x04000B84 RID: 2948
	public Fixed lastRecordedSeconds;
}
