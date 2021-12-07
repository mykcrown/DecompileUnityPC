using System;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class TrailEmitterData : MonoBehaviour, IPreloadedGameAsset
{
	// Token: 0x06001724 RID: 5924 RVA: 0x0007D10A File Offset: 0x0007B50A
	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.particlePrefab, PreloadType.EFFECT), this.preloadCount);
	}

	// Token: 0x040011E7 RID: 4583
	public GameObject particlePrefab;

	// Token: 0x040011E8 RID: 4584
	public bool distanceMode;

	// Token: 0x040011E9 RID: 4585
	public float emitDistance;

	// Token: 0x040011EA RID: 4586
	public int emitFrequencyFrames;

	// Token: 0x040011EB RID: 4587
	public int particleLifespanFrames;

	// Token: 0x040011EC RID: 4588
	public int preloadCount = 64;
}
