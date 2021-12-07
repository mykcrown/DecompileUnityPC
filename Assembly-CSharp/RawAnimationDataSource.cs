using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class RawAnimationDataSource : IAnimationDataSource
{
	// Token: 0x060011CA RID: 4554 RVA: 0x00066A08 File Offset: 0x00064E08
	public RawAnimationDataSource(BoneData boneData, Animator animator)
	{
		foreach (Bone bone in boneData.Bones)
		{
			this.boneMap[bone.bodyPart] = bone;
		}
		this.animator = animator;
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060011CB RID: 4555 RVA: 0x00066A98 File Offset: 0x00064E98
	bool IAnimationDataSource.IsBoneDataAbsolute
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x00066A9C File Offset: 0x00064E9C
	BoneFrameData IAnimationDataSource.GetBoneFrameData(string animationName, BodyPart bodyPart, int gameFrame, HorizontalDirection facing)
	{
		if (!this.boneMap.ContainsKey(bodyPart))
		{
			Debug.LogError("Failed to find mapped bone data for " + bodyPart);
		}
		Bone bone = this.boneMap[bodyPart];
		return new BoneFrameData((Vector3F)bone.GetBonePosition(), (QuaternionF)bone.transform.rotation);
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x00066AFC File Offset: 0x00064EFC
	bool IAnimationDataSource.HasRootDeltaData(string animationName)
	{
		return true;
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x00066AFF File Offset: 0x00064EFF
	FixedRect IAnimationDataSource.GetMaxBounds(string animationName)
	{
		return new FixedRect(new Vector2F(-4, -4), new Vector2F(8, 8));
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x00066B2A File Offset: 0x00064F2A
	Vector3F IAnimationDataSource.GetRootDelta(string animationName, int gameFrame)
	{
		return (Vector3F)this.animator.deltaPosition;
	}

	// Token: 0x04000B55 RID: 2901
	private Dictionary<BodyPart, Bone> boneMap = new Dictionary<BodyPart, Bone>(default(BodyPartComparer));

	// Token: 0x04000B56 RID: 2902
	private Animator animator;
}
