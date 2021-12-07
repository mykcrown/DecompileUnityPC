// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class Bone
{
	public Transform transform;

	public BodyPart bodyPart;

	public bool hasOffset;

	public Vector3 offset;

	public Vector3 GetBonePosition()
	{
		if (!this.hasOffset)
		{
			if (this.transform == null)
			{
				UnityEngine.Debug.LogError("Bone '" + this.bodyPart + "' has no mapped transform; assign the bone data in the character editor.");
			}
			return this.transform.position;
		}
		return this.transform.position + this.transform.rotation * this.offset;
	}

	public Quaternion GetBoneRotation()
	{
		return this.transform.rotation;
	}

	public void Load(Bone other)
	{
		this.transform = other.transform;
		this.bodyPart = other.bodyPart;
		this.hasOffset = other.hasOffset;
		this.offset = other.offset;
	}
}
