// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RawAnimationDataSource : IAnimationDataSource
{
	private Dictionary<BodyPart, Bone> boneMap = new Dictionary<BodyPart, Bone>(default(BodyPartComparer));

	private Animator animator;

	bool IAnimationDataSource.IsBoneDataAbsolute
	{
		get
		{
			return true;
		}
	}

	public RawAnimationDataSource(BoneData boneData, Animator animator)
	{
		foreach (Bone current in boneData.Bones)
		{
			this.boneMap[current.bodyPart] = current;
		}
		this.animator = animator;
	}

	BoneFrameData IAnimationDataSource.GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing)
	{
		if (!this.boneMap.ContainsKey(bodyPart))
		{
			UnityEngine.Debug.LogError("Failed to find mapped bone data for " + bodyPart);
		}
		Bone bone = this.boneMap[bodyPart];
		return new BoneFrameData((Vector3F)bone.GetBonePosition(), (QuaternionF)bone.transform.rotation);
	}

	bool IAnimationDataSource.HasRootDeltaData(string animationName)
	{
		return true;
	}

	FixedRect IAnimationDataSource.GetMaxBounds(string animationName)
	{
		return new FixedRect(new Vector2F(-4, -4), new Vector2F(8, 8));
	}

	Vector3F IAnimationDataSource.GetRootDelta(string animationName, int gameFrame)
	{
		return (Vector3F)this.animator.deltaPosition;
	}
}
