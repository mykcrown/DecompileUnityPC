using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200041E RID: 1054
[Serializable]
public class HitParticleData : MonoBehaviour, IPreloadedGameAsset
{
	// Token: 0x060015F4 RID: 5620 RVA: 0x00077BF3 File Offset: 0x00075FF3
	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.hitParticle, PreloadType.EFFECT), this.preloadCount);
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x00077C10 File Offset: 0x00076010
	public bool IsAppropriateQualityLevel(ThreeTierQualityLevel qualityLevel)
	{
		int num = (int)this.qualityFilter;
		int num2 = 1 << (int)qualityLevel;
		return (num & num2) != 0;
	}

	// Token: 0x040010CE RID: 4302
	[FormerlySerializedAs("particle")]
	public GameObject hitParticle;

	// Token: 0x040010CF RID: 4303
	public AudioData hitSound;

	// Token: 0x040010D0 RID: 4304
	public AudioData altSound;

	// Token: 0x040010D1 RID: 4305
	public bool isSoundOverridable;

	// Token: 0x040010D2 RID: 4306
	[FormerlySerializedAs("frames")]
	public int particleFrames = 20;

	// Token: 0x040010D3 RID: 4307
	public bool overrideAttachPoint;

	// Token: 0x040010D4 RID: 4308
	public BodyPart AttachPoint;

	// Token: 0x040010D5 RID: 4309
	public bool scaleWithShield;

	// Token: 0x040010D6 RID: 4310
	public int preloadCount = 6;

	// Token: 0x040010D7 RID: 4311
	public HitParticleData.DirectionMode directionMode = HitParticleData.DirectionMode.SPLIT_THE_DIFFERENCE;

	// Token: 0x040010D8 RID: 4312
	public ParticleQualityFilter qualityFilter = ParticleQualityFilter.Default;

	// Token: 0x040010D9 RID: 4313
	[NonSerialized]
	public bool editorToggle;

	// Token: 0x0200041F RID: 1055
	public enum DirectionMode
	{
		// Token: 0x040010DB RID: 4315
		KNOCKBACK,
		// Token: 0x040010DC RID: 4316
		SPLIT_THE_DIFFERENCE
	}
}
