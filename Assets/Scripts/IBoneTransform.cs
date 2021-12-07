// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IBoneTransform
{
	Vector3F Position
	{
		get;
	}

	QuaternionF Rotation
	{
		get;
	}

	BodyPart Bone
	{
		get;
	}
}
