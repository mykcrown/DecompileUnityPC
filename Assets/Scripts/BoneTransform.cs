// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public struct BoneTransform : IBoneTransform, IBoneFloatTransform
{
	private IBodyOwner data;

	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return (Vector3)this.data.GetBonePosition(this.Bone, false);
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
			return this.data.GetBonePosition(this.Bone, false);
		}
	}

	public QuaternionF Rotation
	{
		get
		{
			return this.data.GetRotation(this.Bone, false);
		}
	}

	public BoneTransform(BodyPart bone, IBodyOwner data)
	{
		this.Bone = bone;
		this.data = data;
	}
}
