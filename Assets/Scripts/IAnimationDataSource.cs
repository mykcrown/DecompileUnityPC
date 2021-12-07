// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IAnimationDataSource
{
	bool IsBoneDataAbsolute
	{
		get;
	}

	BoneFrameData GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing);

	bool HasRootDeltaData(string animationName);

	Vector3F GetRootDelta(string animationName, int gameFrame);

	FixedRect GetMaxBounds(string animationName);
}
