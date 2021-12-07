using System;
using UnityEngine;

// Token: 0x020004E1 RID: 1249
public interface IMoveOverrideAnimComponent
{
	// Token: 0x06001B5C RID: 7004
	string GetAnimationSuffix(int i);

	// Token: 0x06001B5D RID: 7005
	AnimationClip[] GetAnimationClips();
}
