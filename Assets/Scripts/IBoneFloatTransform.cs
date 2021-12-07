// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IBoneFloatTransform
{
	Vector3 position
	{
		get;
	}

	Quaternion rotation
	{
		get;
	}

	BodyPart Bone
	{
		get;
	}
}
