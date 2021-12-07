// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IPlayerOrientation : IRollbackStateOwner, ITickable
{
	QuaternionF Rotation
	{
		get;
	}

	void Init(IRotationController rotationController, PlayerController playerController);

	void RotateY(Fixed value);

	void RotateX(Fixed value);

	void Rotate(Vector3F rotation);

	void OrientToTumbleSpin(Vector3F velocity);

	void SyncToFacing();
}
