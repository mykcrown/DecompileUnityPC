// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

public static class AnimationUtils
{
	public static AnimationTimeline CreateBasicAnimationTimeline(WavedashAnimationData animationData)
	{
		return AnimationUtils.createBasicAnimationTimeline(animationData);
	}

	private static AnimationTimeline createBasicAnimationTimeline(WavedashAnimationData animationData)
	{
		Fixed animationSpeed = animationData.animationSpeed;
		int animFrames = (int)((Fixed)((double)animationData.animationClip.length) / animationSpeed * 60);
		return new AnimationTimeline(animationSpeed, animFrames, null, 0);
	}

	public static AnimationTimeline CreateCharacterActionTimeline(CharacterActionData actionData, ConfigData configData)
	{
		ActionState characterActionState = actionData.characterActionState;
		AnimationTimeline result;
		if (characterActionState != ActionState.DashPivot)
		{
			result = new AnimationTimeline((Fixed)((double)actionData.animationSpeed), actionData.frameDuration, null, actionData.skipFrames);
		}
		else
		{
			int dashPivotFrames = configData.lagConfig.dashPivotFrames;
			Fixed baseSpeed = (Fixed)((double)actionData.animation.length) / (dashPivotFrames * WTime.fixedDeltaTime);
			result = new AnimationTimeline(baseSpeed, dashPivotFrames, null, 0);
		}
		return result;
	}

	public static AnimationTimeline CreateCharacterAnimationTimeline(CharacterAnimation animation)
	{
		return new AnimationTimeline(animation.playSpeed, animation.frameDuration, null, 0);
	}

	public static AnimationTimeline CreateMoveTimeline(MoveData move, ConfigData configData)
	{
		List<AnimationSpeedMultiplier> list = new List<AnimationSpeedMultiplier>();
		if (move.animationSpeedMultipliers != null)
		{
			list.AddRange(move.animationSpeedMultipliers);
		}
		AnimationTimeline animationTimeline = new AnimationTimeline((Fixed)((double)move.baseAnimationSpeed), move.totalInternalFrames, list, 0);
		if (move.chargeOptions.canCharge)
		{
			ChargeConfig chargeConfig = configData.chargeConfig;
			if (move.chargeOptions.hasOverrideChargeConfig)
			{
				chargeConfig = move.chargeOptions.overrideChargeConfig;
			}
			Fixed other = chargeConfig.maxChargeFrames / (Fixed)((double)WTime.fps);
			int src = animationTimeline.CalculateGameFrameCount(move.chargeOptions.chargeBeginFrame, move.chargeOptions.chargeReleaseFrame);
			Fixed one = src / other;
			Fixed one2 = one / (Fixed)((double)WTime.fps);
			if (one2 == 0)
			{
				throw new Exception("Charge settings of " + move.name + " exceed bounds of animation.");
			}
			animationTimeline.AddSpeedMultiplier(new AnimationSpeedMultiplier(move.chargeOptions.chargeBeginFrame, move.chargeOptions.chargeReleaseFrame, one2.ToFloat()));
		}
		return animationTimeline;
	}
}
