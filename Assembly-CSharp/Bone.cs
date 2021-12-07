using System;
using UnityEngine;

// Token: 0x02000399 RID: 921
[Serializable]
public class Bone
{
	// Token: 0x060013B6 RID: 5046 RVA: 0x00070428 File Offset: 0x0006E828
	public Vector3 GetBonePosition()
	{
		if (!this.hasOffset)
		{
			if (this.transform == null)
			{
				Debug.LogError("Bone '" + this.bodyPart + "' has no mapped transform; assign the bone data in the character editor.");
			}
			return this.transform.position;
		}
		return this.transform.position + this.transform.rotation * this.offset;
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x000704A2 File Offset: 0x0006E8A2
	public Quaternion GetBoneRotation()
	{
		return this.transform.rotation;
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000704AF File Offset: 0x0006E8AF
	public void Load(Bone other)
	{
		this.transform = other.transform;
		this.bodyPart = other.bodyPart;
		this.hasOffset = other.hasOffset;
		this.offset = other.offset;
	}

	// Token: 0x04000D37 RID: 3383
	public Transform transform;

	// Token: 0x04000D38 RID: 3384
	public BodyPart bodyPart;

	// Token: 0x04000D39 RID: 3385
	public bool hasOffset;

	// Token: 0x04000D3A RID: 3386
	public Vector3 offset;
}
