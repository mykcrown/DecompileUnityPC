// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public struct BoneOffsetTransform : IBoneTransform, IBoneFloatTransform
{
	private IBodyOwner data;

	private BodyPart secondaryBone;

	private Fixed offset;

	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return (Vector3)this.Position;
		}
	}

	Quaternion IBoneFloatTransform.rotation
	{
		get
		{
			return (Quaternion)this.data.GetRotation(this.Bone, false);
		}
	}

	public BodyPart Bone
	{
		get;
		private set;
	}

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

	public QuaternionF Rotation
	{
		get
		{
			return this.data.GetRotation(this.Bone, false);
		}
	}

	public BoneOffsetTransform(BodyPart primaryBone, BodyPart secondaryBone, Fixed offset, IBodyOwner data)
	{
		this.Bone = primaryBone;
		this.secondaryBone = secondaryBone;
		this.data = data;
		this.offset = offset;
	}
}
