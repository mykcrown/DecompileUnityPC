using System;
using FixedPoint;

// Token: 0x0200064D RID: 1613
[RollbackStatePoolMultiplier(5)]
[Serializable]
public class StageSurfaceModel : StageObjectModel<StageSurfaceModel>
{
	// Token: 0x06002787 RID: 10119 RVA: 0x000C0CC9 File Offset: 0x000BF0C9
	public override void CopyTo(StageSurfaceModel target)
	{
		base.CopyTo(target);
		target.collidersEnabled = this.collidersEnabled;
		target.lastPosition = this.lastPosition;
		target.position = this.position;
	}

	// Token: 0x04001CF3 RID: 7411
	public bool collidersEnabled = true;

	// Token: 0x04001CF4 RID: 7412
	public Vector3F lastPosition;

	// Token: 0x04001CF5 RID: 7413
	public Vector3F position;
}
