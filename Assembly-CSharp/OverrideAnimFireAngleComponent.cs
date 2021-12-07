using System;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class OverrideAnimFireAngleComponent : MoveComponent, IMoveOverrideAnimComponent, IMoveStartComponent, ITaggedParticleListener
{
	// Token: 0x06001B68 RID: 7016 RVA: 0x0008B078 File Offset: 0x00089478
	public AnimationClip[] GetAnimationClips()
	{
		AnimationClip[] array = new AnimationClip[this.animationClips.Length];
		for (int i = 0; i < this.animationClips.Length; i++)
		{
			array[i] = this.animationClips[i].animation;
		}
		return array;
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x0008B0BD File Offset: 0x000894BD
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

	// Token: 0x06001B6A RID: 7018 RVA: 0x0008B0F4 File Offset: 0x000894F4
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

	// Token: 0x06001B6B RID: 7019 RVA: 0x0008B1BC File Offset: 0x000895BC
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

	// Token: 0x040014B6 RID: 5302
	public FireAngleAnimationClipData[] animationClips = new FireAngleAnimationClipData[0];

	// Token: 0x040014B7 RID: 5303
	public ParticleTag MuzzleParticleTag;
}
