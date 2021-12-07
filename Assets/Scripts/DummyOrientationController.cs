// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class DummyOrientationController : IPlayerOrientation, IRollbackStateOwner, ITickable
{
	private IRotationController rotationController;

	public QuaternionF Rotation
	{
		get
		{
			return this.rotationController.Rotation;
		}
	}

	public DummyOrientationController(Vector3F eulerAngles)
	{
		this.rotationController = new DummyRotationController(eulerAngles);
	}

	public void Init(IRotationController rotationController, PlayerController playerController)
	{
		throw new NotImplementedException();
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return this.rotationController.ExportState(ref container);
	}

	public bool LoadState(RollbackStateContainer container)
	{
		return this.rotationController.LoadState(container);
	}

	public void RotateX(Fixed value)
	{
		throw new NotImplementedException();
	}

	public void RotateY(Fixed value)
	{
		throw new NotImplementedException();
	}

	public void SyncToFacing()
	{
		throw new NotImplementedException();
	}

	public void TickFrame()
	{
		throw new NotImplementedException();
	}

	public void Rotate(Vector3F rotation)
	{
		throw new NotImplementedException();
	}

	public void OrientToTumbleSpin(Vector3F velocity)
	{
	}
}
