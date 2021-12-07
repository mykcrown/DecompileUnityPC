// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ParticleData : ICloneable, IPreloadedGameAsset
{
	public GameObject prefab;

	public GameObject redPrefab;

	public GameObject bluePrefab;

	public bool switchPrefabForSkin;

	public bool teamParticles;

	public ParticlePrefabSkinDictionary prefabsForSkins = new ParticlePrefabSkinDictionary();

	public int frames = 20;

	public int preloadCount = 8;

	[FormerlySerializedAs("stick")]
	public bool attach;

	public bool prewarm;

	public int prewarmFrames;

	public int softKillFrameDuration = Effect.DefaultSoftKillDuration;

	public Vector3 offSet;

	public ParticleOffsetSpace offSetSpace;

	public BodyPart bodyPart = BodyPart.root;

	public ParticleFacing particleFacing = ParticleFacing.MatchPlayerFacing;

	public ParticleTag tag;

	public bool billboard;

	public Vector3 resizeScale = Vector3.one;

	public float lifetimeScale = 1f;

	public ParticleQualityFilter qualityFilter = ParticleQualityFilter.Default;

	public bool editorToggle
	{
		get;
		set;
	}

	public bool HasPrefab()
	{
		return this.prefab != null || this.switchPrefabForSkin || (this.redPrefab != null && this.bluePrefab != null);
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

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
				foreach (KeyValuePair<string, GameObject> current in this.prefabsForSkins)
				{
					context.Register(new PreloadDef(current.Value, PreloadType.EFFECT), this.preloadCount);
				}
			}
		}
	}

	public bool IsAppropriateQualityLevel(ThreeTierQualityLevel qualityLevel)
	{
		int num = (int)this.qualityFilter;
		int num2 = 1 << (int)qualityLevel;
		return (num & num2) != 0;
	}
}
