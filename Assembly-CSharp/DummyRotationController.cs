using System;
using FixedPoint;

// Token: 0x02000601 RID: 1537
public class DummyRotationController : IRotationController, IRollbackStateOwner
{
	// Token: 0x06002589 RID: 9609 RVA: 0x000B9D46 File Offset: 0x000B8146
	public DummyRotationController(Vector3F eulerAngles)
	{
		this.Rotation = QuaternionF.Euler(eulerAngles);
		this.EulerAngles = eulerAngles;
	}

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x0600258A RID: 9610 RVA: 0x000B9D61 File Offset: 0x000B8161
	// (set) Token: 0x0600258B RID: 9611 RVA: 0x000B9D69 File Offset: 0x000B8169
	public QuaternionF Rotation { get; private set; }

	// Token: 0x1700092E RID: 2350
	// (get) Token: 0x0600258C RID: 9612 RVA: 0x000B9D72 File Offset: 0x000B8172
	// (set) Token: 0x0600258D RID: 9613 RVA: 0x000B9D7A File Offset: 0x000B817A
	public Vector3F EulerAngles { get; private set; }

	// Token: 0x0600258E RID: 9614 RVA: 0x000B9D83 File Offset: 0x000B8183
	void IRotationController.Rotate(Vector3F rotation)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600258F RID: 9615 RVA: 0x000B9D8A File Offset: 0x000B818A
	public bool ExportState(ref RollbackStateContainer container)
	{
		return false;
	}

	// Token: 0x06002590 RID: 9616 RVA: 0x000B9D8D File Offset: 0x000B818D
	public bool LoadState(RollbackStateContainer container)
	{
		return false;
	}
}
