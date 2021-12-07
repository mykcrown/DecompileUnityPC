using System;
using FixedPoint;

// Token: 0x020005EF RID: 1519
public class DummyOrientationController : IPlayerOrientation, IRollbackStateOwner, ITickable
{
	// Token: 0x060023E0 RID: 9184 RVA: 0x000B5661 File Offset: 0x000B3A61
	public DummyOrientationController(Vector3F eulerAngles)
	{
		this.rotationController = new DummyRotationController(eulerAngles);
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000B5675 File Offset: 0x000B3A75
	public void Init(IRotationController rotationController, PlayerController playerController)
	{
		throw new NotImplementedException();
	}

	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000B567C File Offset: 0x000B3A7C
	public QuaternionF Rotation
	{
		get
		{
			return this.rotationController.Rotation;
		}
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000B5689 File Offset: 0x000B3A89
	public bool ExportState(ref RollbackStateContainer container)
	{
		return this.rotationController.ExportState(ref container);
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000B5697 File Offset: 0x000B3A97
	public bool LoadState(RollbackStateContainer container)
	{
		return this.rotationController.LoadState(container);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000B56A5 File Offset: 0x000B3AA5
	public void RotateX(Fixed value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000B56AC File Offset: 0x000B3AAC
	public void RotateY(Fixed value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000B56B3 File Offset: 0x000B3AB3
	public void SyncToFacing()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000B56BA File Offset: 0x000B3ABA
	public void TickFrame()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000B56C1 File Offset: 0x000B3AC1
	public void Rotate(Vector3F rotation)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000B56C8 File Offset: 0x000B3AC8
	public void OrientToTumbleSpin(Vector3F velocity)
	{
	}

	// Token: 0x04001B47 RID: 6983
	private IRotationController rotationController;
}
