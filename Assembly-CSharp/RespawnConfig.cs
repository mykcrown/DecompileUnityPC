using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003E4 RID: 996
[Serializable]
public class RespawnConfig : IPreloadedGameAsset
{
	// Token: 0x0600157D RID: 5501 RVA: 0x000765A2 File Offset: 0x000749A2
	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.respawnPlatformPrefab, PreloadType.EFFECT), 4);
		context.Register(new PreloadDef(this.deathEffectPrefab, PreloadType.EFFECT), 4);
	}

	// Token: 0x04000F2C RID: 3884
	public Fixed platformOffscreenHeight = (Fixed)8.0;

	// Token: 0x04000F2D RID: 3885
	public int platformDurationFrames = 330;

	// Token: 0x04000F2E RID: 3886
	public int dismountInvincibilityFrames = 100;

	// Token: 0x04000F2F RID: 3887
	public Fixed platformSpeed = (Fixed)8.0;

	// Token: 0x04000F30 RID: 3888
	public GameObject respawnPlatformPrefab;

	// Token: 0x04000F31 RID: 3889
	public GameObject deathEffectPrefab;

	// Token: 0x04000F32 RID: 3890
	public AudioData deathEffectSound;

	// Token: 0x04000F33 RID: 3891
	public AudioData respawnSound;

	// Token: 0x04000F34 RID: 3892
	public int respawnDelayFrames = 30;
}
