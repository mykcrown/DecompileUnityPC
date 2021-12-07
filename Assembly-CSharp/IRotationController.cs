using System;
using FixedPoint;

// Token: 0x02000600 RID: 1536
public interface IRotationController : IRollbackStateOwner
{
	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06002586 RID: 9606
	QuaternionF Rotation { get; }

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06002587 RID: 9607
	Vector3F EulerAngles { get; }

	// Token: 0x06002588 RID: 9608
	void Rotate(Vector3F rotation);
}
