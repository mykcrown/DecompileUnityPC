// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class DummyRotationController : IRotationController, IRollbackStateOwner
{
	public QuaternionF Rotation
	{
		get;
		private set;
	}

	public Vector3F EulerAngles
	{
		get;
		private set;
	}

	public DummyRotationController(Vector3F eulerAngles)
	{
		this.Rotation = QuaternionF.Euler(eulerAngles);
		this.EulerAngles = eulerAngles;
	}

	void IRotationController.Rotate(Vector3F rotation)
	{
		throw new NotImplementedException();
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return false;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		return false;
	}
}
