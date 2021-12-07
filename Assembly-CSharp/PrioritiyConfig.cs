using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003E6 RID: 998
[Serializable]
public class PrioritiyConfig : IPreloadedGameAsset
{
	// Token: 0x06001582 RID: 5506 RVA: 0x00076887 File Offset: 0x00074C87
	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.clankParticle, PreloadType.EFFECT), 4);
	}

	// Token: 0x04000F51 RID: 3921
	public Fixed priorityThreshold = (Fixed)10.0;

	// Token: 0x04000F52 RID: 3922
	public GameObject clankParticle;

	// Token: 0x04000F53 RID: 3923
	public AudioData clankSound;
}
