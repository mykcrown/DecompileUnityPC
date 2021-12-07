using System;
using FixedPoint;

// Token: 0x020003C6 RID: 966
[RollbackStatePoolMultiplier(2)]
[Serializable]
public class ComboStateModel : RollbackStateTyped<ComboStateModel>
{
	// Token: 0x06001520 RID: 5408 RVA: 0x00074FBC File Offset: 0x000733BC
	public override void CopyTo(ComboStateModel target)
	{
		target.Damage = this.Damage;
		target.Count = this.Count;
		target.IsRecovered = this.IsRecovered;
		target.FramesRecovered = this.FramesRecovered;
		target.IsActive = this.IsActive;
		target.WindowFrames = this.WindowFrames;
	}

	// Token: 0x04000DE2 RID: 3554
	public Fixed Damage;

	// Token: 0x04000DE3 RID: 3555
	public int Count;

	// Token: 0x04000DE4 RID: 3556
	public bool IsRecovered;

	// Token: 0x04000DE5 RID: 3557
	public int FramesRecovered;

	// Token: 0x04000DE6 RID: 3558
	public bool IsActive;

	// Token: 0x04000DE7 RID: 3559
	public int WindowFrames;
}
