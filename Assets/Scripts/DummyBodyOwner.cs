// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DummyBodyOwner : IBodyOwner
{
	private List<HurtBoxState> hurtBoxes = new List<HurtBoxState>();

	private BoneFrameData boneFrameData;

	public List<HurtBoxState> HurtBoxes
	{
		get
		{
			return this.hurtBoxes;
		}
	}

	public Vector3F DeltaPosition
	{
		get
		{
			return Vector3F.zero;
		}
	}

	public bool HasRootMotion
	{
		get
		{
			return false;
		}
	}

	public FixedRect CurrentMaxBounds
	{
		get
		{
			return default(FixedRect);
		}
	}

	public string AnimationName
	{
		get
		{
			return "Debug";
		}
	}

	public int AnimationFrame
	{
		get
		{
			return 0;
		}
	}

	public DummyBodyOwner(HurtBoxState hurtBox, Vector3F position, QuaternionF rotation = default(QuaternionF))
	{
		this.hurtBoxes = new List<HurtBoxState>
		{
			hurtBox
		};
		this.boneFrameData = new BoneFrameData(position, rotation);
	}

	public HurtBoxState GetHurtBox(BodyPart bodyPart)
	{
		return this.hurtBoxes[0];
	}

	public QuaternionF GetRotation(BodyPart bodyPart, bool forceInvert = false)
	{
		if (forceInvert)
		{
			UnityEngine.Debug.LogError("Dummy doesn't support inverting");
		}
		return this.boneFrameData.rotation;
	}

	public Vector3F GetBonePosition(BodyPart bodyPart, bool forceInvert = false)
	{
		if (forceInvert)
		{
			UnityEngine.Debug.LogError("Dummy doesn't support inverting");
		}
		return this.boneFrameData.position;
	}

	public void AttachVFX(BodyPart bodyPart, Transform vfxTransform, Vector3 vfxOffset, ParticleOffsetSpace particleOffsetSpace, bool invertHorizontal)
	{
		throw new NotImplementedException();
	}

	public void DrawGizmos()
	{
	}

	public IBoneTransform GetBoneTransform(BodyPart bodyPart)
	{
		return new BoneTransform(bodyPart, this);
	}

	public IBoneTransform GetBoneOffsetTransform(BodyPart bodyPart, BodyPart secondaryBodyPart, Fixed offset)
	{
		return new BoneTransform(bodyPart, this);
	}
}
