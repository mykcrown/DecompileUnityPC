// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MoveAnimationCalculator : IMoveAnimationCalculator
{
	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	MoveAnimationResult IMoveAnimationCalculator.GetPlayableData(MoveData data, IPlayerDelegate playerDelegate)
	{
		MoveAnimationResult result = default(MoveAnimationResult);
		result.animationName = this.GetBaseAnimationClipName(data, playerDelegate.Facing);
		result.mirror = (playerDelegate.CharacterData.reversesStance && data.animationClipLeft == null && playerDelegate.Facing == HorizontalDirection.Left);
		AnimationClip animationClip = (playerDelegate.Facing != HorizontalDirection.Left || !(data.animationClipLeft != null)) ? data.animationClip : data.animationClipLeft;
		result.animationSpeed = ((!(animationClip == null)) ? (animationClip.length * WTime.fps / (float)data.totalInternalFrames) : 0f);
		return result;
	}

	public string GetBaseAnimationClipName(MoveData data, HorizontalDirection facing)
	{
		return (facing != HorizontalDirection.Left) ? data.name : data.leftClipName;
	}

	public AnimationClip GetBaseAnimationClip(MoveData moveData, HorizontalDirection facing)
	{
		AnimationClip result = moveData.animationClip;
		if (facing == HorizontalDirection.Left)
		{
			result = ((!(moveData.animationClipLeft == null)) ? moveData.animationClipLeft : moveData.animationClip);
		}
		return result;
	}
}
