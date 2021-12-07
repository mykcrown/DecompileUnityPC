// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public struct BoneFrameData
{
	public Vector3F position
	{
		get;
		private set;
	}

	public QuaternionF rotation
	{
		get;
		private set;
	}

	public BoneFrameData(Vector3F position, QuaternionF rotation)
	{
		this = default(BoneFrameData);
		this.position = position;
		this.rotation = rotation;
	}
}
