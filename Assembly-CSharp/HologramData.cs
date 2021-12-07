using System;
using UnityEngine;

// Token: 0x02000422 RID: 1058
[Serializable]
public class HologramData : ScriptableObject
{
	// Token: 0x060015F9 RID: 5625 RVA: 0x00077C94 File Offset: 0x00076094
	public void RegisterPreload(PreloadContext context)
	{
		if (this.hasOverrideVFX && this.overrideVFX != null)
		{
			context.Register(new PreloadDef(this.overrideVFX.prefab, PreloadType.EFFECT), 16);
		}
	}

	// Token: 0x040010EF RID: 4335
	public Texture2D texture;

	// Token: 0x040010F0 RID: 4336
	public bool hasOverrideVFX;

	// Token: 0x040010F1 RID: 4337
	public ParticleData overrideVFX;
}
