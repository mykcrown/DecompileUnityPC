using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public interface IMoveAnimationCalculator
{
	// Token: 0x06001AC1 RID: 6849
	MoveAnimationResult GetPlayableData(MoveData data, IPlayerDelegate playerDelegate);

	// Token: 0x06001AC2 RID: 6850
	string GetBaseAnimationClipName(MoveData data, HorizontalDirection facing);

	// Token: 0x06001AC3 RID: 6851
	AnimationClip GetBaseAnimationClip(MoveData data, HorizontalDirection facing);
}
