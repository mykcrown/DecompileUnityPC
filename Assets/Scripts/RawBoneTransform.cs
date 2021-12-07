// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public struct RawBoneTransform : IBoneFloatTransform
{
	private Transform transform;

	Vector3 IBoneFloatTransform.position
	{
		get
		{
			return this.transform.position;
		}
	}

	Quaternion IBoneFloatTransform.rotation
	{
		get
		{
			return this.transform.rotation;
		}
	}

	public BodyPart Bone
	{
		get;
		private set;
	}

	public RawBoneTransform(Transform transform, BodyPart bone = BodyPart.none)
	{
		this.transform = transform;
		this.Bone = bone;
	}
}
