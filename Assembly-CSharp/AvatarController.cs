using System;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class AvatarController
{
	// Token: 0x06001198 RID: 4504 RVA: 0x00065BA0 File Offset: 0x00063FA0
	public AvatarController(Animator animator, AvatarData avatarData)
	{
		this.animator = animator;
		this.avatarData = avatarData;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00065BB8 File Offset: 0x00063FB8
	public bool SetAppropriateAvatar(AnimationClip clip)
	{
		if (this.animator == null)
		{
			Debug.LogError("Animator was not set.");
			return false;
		}
		if (clip == null)
		{
			Debug.LogError("No clip was set.");
			return false;
		}
		if (this.animator.isHuman == clip.isHumanMotion)
		{
			return false;
		}
		if (this.avatarData == null)
		{
			Debug.LogError("Avatar data was not set.");
			return false;
		}
		if (clip.isHumanMotion)
		{
			if (this.avatarData.humanoidAvatar == null)
			{
				Debug.LogError("Humanoid Avatar not assigned.");
				return false;
			}
			this.animator.avatar = this.avatarData.humanoidAvatar;
		}
		else
		{
			if (this.avatarData.genericAvatar == null)
			{
				Debug.LogError("Generic Avatar not assigned.");
				return false;
			}
			this.animator.avatar = this.avatarData.genericAvatar;
		}
		return true;
	}

	// Token: 0x04000B3C RID: 2876
	public Animator animator;

	// Token: 0x04000B3D RID: 2877
	public AvatarData avatarData;
}
