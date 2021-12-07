// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BoneData : GameBehavior
{
	public List<Bone> Bones = new List<Bone>();

	public List<Transform> HiddenProps = new List<Transform>();

	public bool PreviewInvertRotation
	{
		get;
		set;
	}

	public bool PreviewMirror
	{
		get;
		set;
	}

	public Bone GetBone(BodyPart bodyPart)
	{
		foreach (Bone current in this.Bones)
		{
			if (current.bodyPart == bodyPart)
			{
				return current;
			}
		}
		return null;
	}

	public void AutoConfigure(Animator animator, Transform root, Transform throwBone)
	{
		Dictionary<HumanBodyBones, BodyPart> dictionary = new Dictionary<HumanBodyBones, BodyPart>
		{
			{
				HumanBodyBones.Head,
				BodyPart.head
			},
			{
				HumanBodyBones.Chest,
				BodyPart.upperTorso
			},
			{
				HumanBodyBones.Spine,
				BodyPart.lowerTorso
			},
			{
				HumanBodyBones.LeftUpperArm,
				BodyPart.leftUpperArm
			},
			{
				HumanBodyBones.RightUpperArm,
				BodyPart.rightUpperArm
			},
			{
				HumanBodyBones.LeftLowerArm,
				BodyPart.leftForearm
			},
			{
				HumanBodyBones.RightLowerArm,
				BodyPart.rightForearm
			},
			{
				HumanBodyBones.LeftHand,
				BodyPart.leftHand
			},
			{
				HumanBodyBones.RightHand,
				BodyPart.rightHand
			},
			{
				HumanBodyBones.LeftUpperLeg,
				BodyPart.leftThigh
			},
			{
				HumanBodyBones.RightUpperLeg,
				BodyPart.rightThigh
			},
			{
				HumanBodyBones.LeftLowerLeg,
				BodyPart.leftCalf
			},
			{
				HumanBodyBones.RightLowerLeg,
				BodyPart.rightCalf
			},
			{
				HumanBodyBones.LeftFoot,
				BodyPart.leftFoot
			},
			{
				HumanBodyBones.RightFoot,
				BodyPart.rightFoot
			},
			{
				HumanBodyBones.LeftLittleDistal,
				BodyPart.leftLittleTip
			},
			{
				HumanBodyBones.LeftRingDistal,
				BodyPart.leftRingTip
			},
			{
				HumanBodyBones.LeftMiddleDistal,
				BodyPart.leftMiddleTip
			},
			{
				HumanBodyBones.LeftIndexDistal,
				BodyPart.leftIndexTip
			},
			{
				HumanBodyBones.LeftThumbDistal,
				BodyPart.leftThumbTip
			},
			{
				HumanBodyBones.RightLittleDistal,
				BodyPart.rightLittleTip
			},
			{
				HumanBodyBones.RightRingDistal,
				BodyPart.rightRingTip
			},
			{
				HumanBodyBones.RightMiddleDistal,
				BodyPart.rightMiddleTip
			},
			{
				HumanBodyBones.RightIndexDistal,
				BodyPart.rightIndexTip
			},
			{
				HumanBodyBones.RightThumbDistal,
				BodyPart.rightThumbTip
			},
			{
				HumanBodyBones.LeftToes,
				BodyPart.leftToes
			},
			{
				HumanBodyBones.RightToes,
				BodyPart.rightToes
			}
		};
		List<Bone> list = new List<Bone>();
		foreach (HumanBodyBones current in dictionary.Keys)
		{
			Transform boneTransform = animator.GetBoneTransform(current);
			if (boneTransform != null)
			{
				Bone bone = new Bone();
				BodyPart bodyPart = dictionary[current];
				bone.bodyPart = bodyPart;
				bone.transform = boneTransform;
				list.Add(bone);
			}
		}
		list.Add(new Bone
		{
			bodyPart = BodyPart.root,
			transform = root
		});
		list.Add(new Bone
		{
			bodyPart = BodyPart.throwBone,
			transform = throwBone
		});
		this.Bones = list;
	}
}
