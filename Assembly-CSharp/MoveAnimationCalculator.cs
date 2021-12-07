using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class MoveAnimationCalculator : IMoveAnimationCalculator
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x06001ABC RID: 6844 RVA: 0x00089856 File Offset: 0x00087C56
	// (set) Token: 0x06001ABD RID: 6845 RVA: 0x0008985E File Offset: 0x00087C5E
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x06001ABE RID: 6846 RVA: 0x00089868 File Offset: 0x00087C68
	MoveAnimationResult IMoveAnimationCalculator.GetPlayableData(MoveData data, IPlayerDelegate playerDelegate)
	{
		MoveAnimationResult result = default(MoveAnimationResult);
		result.animationName = this.GetBaseAnimationClipName(data, playerDelegate.Facing);
		result.mirror = (playerDelegate.CharacterData.reversesStance && data.animationClipLeft == null && playerDelegate.Facing == HorizontalDirection.Left);
		AnimationClip animationClip = (playerDelegate.Facing != HorizontalDirection.Left || !(data.animationClipLeft != null)) ? data.animationClip : data.animationClipLeft;
		result.animationSpeed = ((!(animationClip == null)) ? (animationClip.length * WTime.fps / (float)data.totalInternalFrames) : 0f);
		return result;
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x00089926 File Offset: 0x00087D26
	public string GetBaseAnimationClipName(MoveData data, HorizontalDirection facing)
	{
		return (facing != HorizontalDirection.Left) ? data.name : data.leftClipName;
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x00089940 File Offset: 0x00087D40
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
