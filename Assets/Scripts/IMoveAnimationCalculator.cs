// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IMoveAnimationCalculator
{
	MoveAnimationResult GetPlayableData(MoveData data, IPlayerDelegate playerDelegate);

	string GetBaseAnimationClipName(MoveData data, HorizontalDirection facing);

	AnimationClip GetBaseAnimationClip(MoveData data, HorizontalDirection facing);
}
