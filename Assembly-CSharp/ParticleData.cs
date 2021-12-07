using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000455 RID: 1109
[Serializable]
public class ParticleData : ICloneable, IPreloadedGameAsset
{
	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x060016FB RID: 5883 RVA: 0x0007C604 File Offset: 0x0007AA04
	// (set) Token: 0x060016FC RID: 5884 RVA: 0x0007C60C File Offset: 0x0007AA0C
	public bool editorToggle { get; set; }

	// Token: 0x060016FD RID: 5885 RVA: 0x0007C618 File Offset: 0x0007AA18
	public bool HasPrefab()
	{
		return this.prefab != null || this.switchPrefabForSkin || (this.redPrefab != null && this.bluePrefab != null);
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x0007C664 File Offset: 0x0007AA64
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x0007C66C File Offset: 0x0007AA6C
	public void RegisterPreload(PreloadContext context)
	{
		if (this.IsAppropriateQualityLevel(context.particleQuality))
		{
			context.Register(new PreloadDef(this.prefab, PreloadType.EFFECT), this.preloadCount);
			if (this.redPrefab != null)
			{
				context.Register(new PreloadDef(this.redPrefab, PreloadType.EFFECT), this.preloadCount);
			}
			if (this.bluePrefab != null)
			{
				context.Register(new PreloadDef(this.bluePrefab, PreloadType.EFFECT), this.preloadCount);
			}
			if (this.switchPrefabForSkin)
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair in this.prefabsForSkins)
				{
					context.Register(new PreloadDef(keyValuePair.Value, PreloadType.EFFECT), this.preloadCount);
				}
			}
		}
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x0007C760 File Offset: 0x0007AB60
	public bool IsAppropriateQualityLevel(ThreeTierQualityLevel qualityLevel)
	{
		int num = (int)this.qualityFilter;
		int num2 = 1 << (int)qualityLevel;
		return (num & num2) != 0;
	}

	// Token: 0x040011AF RID: 4527
	public GameObject prefab;

	// Token: 0x040011B0 RID: 4528
	public GameObject redPrefab;

	// Token: 0x040011B1 RID: 4529
	public GameObject bluePrefab;

	// Token: 0x040011B2 RID: 4530
	public bool switchPrefabForSkin;

	// Token: 0x040011B3 RID: 4531
	public bool teamParticles;

	// Token: 0x040011B4 RID: 4532
	public ParticlePrefabSkinDictionary prefabsForSkins = new ParticlePrefabSkinDictionary();

	// Token: 0x040011B5 RID: 4533
	public int frames = 20;

	// Token: 0x040011B6 RID: 4534
	public int preloadCount = 8;

	// Token: 0x040011B7 RID: 4535
	[FormerlySerializedAs("stick")]
	public bool attach;

	// Token: 0x040011B8 RID: 4536
	public bool prewarm;

	// Token: 0x040011B9 RID: 4537
	public int prewarmFrames;

	// Token: 0x040011BA RID: 4538
	public int softKillFrameDuration = Effect.DefaultSoftKillDuration;

	// Token: 0x040011BB RID: 4539
	public Vector3 offSet;

	// Token: 0x040011BC RID: 4540
	public ParticleOffsetSpace offSetSpace;

	// Token: 0x040011BD RID: 4541
	public BodyPart bodyPart = BodyPart.root;

	// Token: 0x040011BE RID: 4542
	public ParticleFacing particleFacing = ParticleFacing.MatchPlayerFacing;

	// Token: 0x040011BF RID: 4543
	public ParticleTag tag;

	// Token: 0x040011C0 RID: 4544
	public bool billboard;

	// Token: 0x040011C1 RID: 4545
	public Vector3 resizeScale = Vector3.one;

	// Token: 0x040011C2 RID: 4546
	public float lifetimeScale = 1f;

	// Token: 0x040011C3 RID: 4547
	public ParticleQualityFilter qualityFilter = ParticleQualityFilter.Default;
}
