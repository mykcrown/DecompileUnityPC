// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class AvatarController
{
	public Animator animator;

	public AvatarData avatarData;

	public AvatarController(Animator animator, AvatarData avatarData)
	{
		this.animator = animator;
		this.avatarData = avatarData;
	}

	public bool SetAppropriateAvatar(AnimationClip clip)
	{
		if (this.animator == null)
		{
			UnityEngine.Debug.LogError("Animator was not set.");
			return false;
		}
		if (clip == null)
		{
			UnityEngine.Debug.LogError("No clip was set.");
			return false;
		}
		if (this.animator.isHuman == clip.isHumanMotion)
		{
			return false;
		}
		if (this.avatarData == null)
		{
			UnityEngine.Debug.LogError("Avatar data was not set.");
			return false;
		}
		if (clip.isHumanMotion)
		{
			if (this.avatarData.humanoidAvatar == null)
			{
				UnityEngine.Debug.LogError("Humanoid Avatar not assigned.");
				return false;
			}
			this.animator.avatar = this.avatarData.humanoidAvatar;
		}
		else
		{
			if (this.avatarData.genericAvatar == null)
			{
				UnityEngine.Debug.LogError("Generic Avatar not assigned.");
				return false;
			}
			this.animator.avatar = this.avatarData.genericAvatar;
		}
		return true;
	}
}
