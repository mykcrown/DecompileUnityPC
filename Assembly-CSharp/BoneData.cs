using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class BoneData : GameBehavior
{
	// Token: 0x170003CC RID: 972
	// (get) Token: 0x0600141A RID: 5146 RVA: 0x0007176C File Offset: 0x0006FB6C
	// (set) Token: 0x0600141B RID: 5147 RVA: 0x00071774 File Offset: 0x0006FB74
	public bool PreviewInvertRotation { get; set; }

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x0600141C RID: 5148 RVA: 0x0007177D File Offset: 0x0006FB7D
	// (set) Token: 0x0600141D RID: 5149 RVA: 0x00071785 File Offset: 0x0006FB85
	public bool PreviewMirror { get; set; }

	// Token: 0x0600141E RID: 5150 RVA: 0x00071790 File Offset: 0x0006FB90
	public Bone GetBone(BodyPart bodyPart)
	{
		foreach (Bone bone in this.Bones)
		{
			if (bone.bodyPart == bodyPart)
			{
				return bone;
			}
		}
		return null;
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000717FC File Offset: 0x0006FBFC
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
		foreach (HumanBodyBones humanBodyBones in dictionary.Keys)
		{
			Transform boneTransform = animator.GetBoneTransform(humanBodyBones);
			if (boneTransform != null)
			{
				Bone bone = new Bone();
				BodyPart bodyPart = dictionary[humanBodyBones];
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

	// Token: 0x04000D65 RID: 3429
	public List<Bone> Bones = new List<Bone>();

	// Token: 0x04000D66 RID: 3430
	public List<Transform> HiddenProps = new List<Transform>();
}
