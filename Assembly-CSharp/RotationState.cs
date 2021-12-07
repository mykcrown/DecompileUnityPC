using System;
using FixedPoint;

// Token: 0x020005FE RID: 1534
[Serializable]
public class RotationState : RollbackStateTyped<RotationState>
{
	// Token: 0x0600257C RID: 9596 RVA: 0x000B9B90 File Offset: 0x000B7F90
	public override void CopyTo(RotationState target)
	{
		target.eulerAngles = this.eulerAngles;
		target.quaternion = this.quaternion;
	}

	// Token: 0x04001BA8 RID: 7080
	public Vector3F eulerAngles;

	// Token: 0x04001BA9 RID: 7081
	public QuaternionF quaternion;
}
