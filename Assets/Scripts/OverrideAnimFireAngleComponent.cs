// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class OverrideAnimFireAngleComponent : MoveComponent, IMoveOverrideAnimComponent, IMoveStartComponent, ITaggedParticleListener
{
	public FireAngleAnimationClipData[] animationClips = new FireAngleAnimationClipData[0];

	public ParticleTag MuzzleParticleTag;

	public AnimationClip[] GetAnimationClips()
	{
		AnimationClip[] array = new AnimationClip[this.animationClips.Length];
		for (int i = 0; i < this.animationClips.Length; i++)
		{
			array[i] = this.animationClips[i].animation;
		}
		return array;
	}

	public string GetAnimationSuffix(int i)
	{
		return string.Concat(new object[]
		{
			"_",
			base.GetType().ToString(),
			"_",
			i
		});
	}

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		MoveModel model = player.ActiveMove.Model;
		for (int i = 0; i < this.animationClips.Length; i++)
		{
			FireAngleAnimationClipData fireAngleAnimationClipData = this.animationClips[i];
			if (model.articleFireAngle >= fireAngleAnimationClipData.MinAngle && model.articleFireAngle <= fireAngleAnimationClipData.MaxAngle)
			{
				string animName = model.data.name + this.GetAnimationSuffix(i);
				this.playerDelegate.AnimationPlayer.PlayAnimation(animName, false, model.gameFrame, (!model.data.overrideBlendingIn) ? -1f : model.data.blendingIn, -1f);
				break;
			}
		}
	}

	public void OnCreateTaggedParticle(ParticleTag tag, GameObject particle)
	{
		if (tag == this.MuzzleParticleTag)
		{
			Vector3 vector;
			if (this.playerDelegate.Facing == HorizontalDirection.Left)
			{
				vector = (Vector3)MathUtil.AngleToVector(180 - this.moveDelegate.Model.articleFireAngle);
				vector *= -1f;
			}
			else
			{
				vector = (Vector3)MathUtil.AngleToVector(this.moveDelegate.Model.articleFireAngle);
			}
			vector.Normalize();
			particle.transform.right = vector;
		}
	}
}
