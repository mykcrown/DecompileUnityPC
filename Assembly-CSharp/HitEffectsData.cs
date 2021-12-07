using System;

// Token: 0x02000420 RID: 1056
[Serializable]
public class HitEffectsData : IPreloadedGameAsset
{
	// Token: 0x060015F7 RID: 5623 RVA: 0x00077C48 File Offset: 0x00076048
	public void RegisterPreload(PreloadContext context)
	{
		foreach (HitParticleData hitParticleData in this.hitParticles)
		{
			if (!(hitParticleData == null))
			{
				hitParticleData.RegisterPreload(context);
			}
		}
	}

	// Token: 0x040010DD RID: 4317
	public HitParticleData[] hitParticles = new HitParticleData[0];

	// Token: 0x040010DE RID: 4318
	public AudioData attackSound;

	// Token: 0x040010DF RID: 4319
	public AudioData altAttackSound;

	// Token: 0x040010E0 RID: 4320
	[NonSerialized]
	public bool editorToggle;
}
