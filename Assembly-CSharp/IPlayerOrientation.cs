using System;
using FixedPoint;

// Token: 0x020005EE RID: 1518
public interface IPlayerOrientation : IRollbackStateOwner, ITickable
{
	// Token: 0x060023D9 RID: 9177
	void Init(IRotationController rotationController, PlayerController playerController);

	// Token: 0x1700083E RID: 2110
	// (get) Token: 0x060023DA RID: 9178
	QuaternionF Rotation { get; }

	// Token: 0x060023DB RID: 9179
	void RotateY(Fixed value);

	// Token: 0x060023DC RID: 9180
	void RotateX(Fixed value);

	// Token: 0x060023DD RID: 9181
	void Rotate(Vector3F rotation);

	// Token: 0x060023DE RID: 9182
	void OrientToTumbleSpin(Vector3F velocity);

	// Token: 0x060023DF RID: 9183
	void SyncToFacing();
}
