// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public class AnimationFrameData
{
	public Dictionary<int, Dictionary<BodyPart, BoneFrameData>> boneData;

	public Dictionary<int, Vector3F> rootDeltaData;

	public FixedRect maxBounds;

	public bool reversesFacing;

	public bool HasRootDeltaData
	{
		get
		{
			return this.rootDeltaData != null;
		}
	}

	public Vector3F GetRootDeltaData(int gameFrame)
	{
		if (!this.rootDeltaData.ContainsKey(gameFrame))
		{
			return Vector3F.zero;
		}
		return this.rootDeltaData[gameFrame];
	}
}
