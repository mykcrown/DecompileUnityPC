using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public struct BoneOffsetTransform : IBoneTransform, IBoneFloatTransform
{
	// Token: 0x06001412 RID: 5138 RVA: 0x0007165C File Offset: 0x0006FA5C
	public BoneOffsetTransform(BodyPart primaryBone, BodyPart secondaryBone, Fixed offset, IBodyOwner data)
	{
		this.Bone = primaryBone;
		this.secondaryBone = secondaryBone;
		this.data = data;
		this.offset = offset;
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x0007167B File Offset: 0x0006FA7B
	// (set) Token: 0x06001414 RID: 5140 RVA: 0x00071683 File Offset: 0x0006FA83
	public BodyPart Bone { get; private set; }

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06001415 RID: 5141 RVA: 0x0007168C File Offset: 0x0006FA8C
	public Vector3F Position
	{
		get
		{
			if (this.offset == 0 || this.Bone == this.secondaryBone)
			{
				return this.data.GetBonePosition(this.Bone, false);
			}
			Vector3F bonePosition = this.data.GetBonePosition(this.Bone, false);
			Vector3F bonePosition2 = this.data.GetBonePosition(this.secondaryBone, false);
			return bonePosition + (bonePosition - bonePosition2).normalized * this.offset;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06001416 RID: 5142 RVA: 0x00071714 File Offset: 0x0006FB14
	public QuaternionF Rotation
	{
		get
		{
			return this.data.GetRotation(this.Bone, false);
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06001417 RID: 5143 RVA: 0x00071728 File Offset: 0x0006FB28
	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return (Vector3)this.Position;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06001418 RID: 5144 RVA: 0x00071735 File Offset: 0x0006FB35
	Quaternion IBoneFloatTransform.rotation
	{
		get
		{
			return (Quaternion)this.data.GetRotation(this.Bone, false);
		}
	}

	// Token: 0x04000D5A RID: 3418
	private IBodyOwner data;

	// Token: 0x04000D5C RID: 3420
	private BodyPart secondaryBone;

	// Token: 0x04000D5D RID: 3421
	private Fixed offset;
}
