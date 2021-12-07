using System;
using FixedPoint;

// Token: 0x0200045C RID: 1116
[RollbackStatePoolMultiplier(8)]
[Serializable]
public class TrailEmitterModel : RollbackStateTyped<TrailEmitterModel>
{
	// Token: 0x06001710 RID: 5904 RVA: 0x0007CDD6 File Offset: 0x0007B1D6
	public override void CopyTo(TrailEmitterModel target)
	{
		target.defaultData = this.defaultData;
		target.overrideData = this.overrideData;
		target.frameCount = this.frameCount;
		target.isDead = this.isDead;
		target.lastEmitPosition = this.lastEmitPosition;
	}

	// Token: 0x040011DC RID: 4572
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public TrailEmitterData defaultData;

	// Token: 0x040011DD RID: 4573
	[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public TrailEmitterData overrideData;

	// Token: 0x040011DE RID: 4574
	public int frameCount;

	// Token: 0x040011DF RID: 4575
	public Vector2F lastEmitPosition;

	// Token: 0x040011E0 RID: 4576
	public bool isDead;
}
