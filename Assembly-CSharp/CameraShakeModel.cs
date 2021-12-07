using System;
using FixedPoint;

// Token: 0x02000382 RID: 898
[Serializable]
public class CameraShakeModel : RollbackStateTyped<CameraShakeModel>
{
	// Token: 0x0600131B RID: 4891 RVA: 0x0006F177 File Offset: 0x0006D577
	public override void CopyTo(CameraShakeModel target)
	{
		target.frameCount = this.frameCount;
		target.amplitude = this.amplitude;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x0006F194 File Offset: 0x0006D594
	public override object Clone()
	{
		CameraShakeModel cameraShakeModel = new CameraShakeModel();
		this.CopyTo(cameraShakeModel);
		return cameraShakeModel;
	}

	// Token: 0x04000CB3 RID: 3251
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public int frameCount;

	// Token: 0x04000CB4 RID: 3252
	[IgnoreRollback(IgnoreRollbackType.Visual)]
	public Fixed amplitude;
}
