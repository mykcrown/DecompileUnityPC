// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IRotationController : IRollbackStateOwner
{
	QuaternionF Rotation
	{
		get;
	}

	Vector3F EulerAngles
	{
		get;
	}

	void Rotate(Vector3F rotation);
}
