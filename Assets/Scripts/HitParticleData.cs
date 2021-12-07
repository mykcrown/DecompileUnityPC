// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class HitParticleData : MonoBehaviour, IPreloadedGameAsset
{
	public enum DirectionMode
	{
		KNOCKBACK,
		SPLIT_THE_DIFFERENCE
	}

	[FormerlySerializedAs("particle")]
	public GameObject hitParticle;

	public AudioData hitSound;

	public AudioData altSound;

	public bool isSoundOverridable;

	[FormerlySerializedAs("frames")]
	public int particleFrames = 20;

	public bool overrideAttachPoint;

	public BodyPart AttachPoint;

	public bool scaleWithShield;

	public int preloadCount = 6;

	public HitParticleData.DirectionMode directionMode = HitParticleData.DirectionMode.SPLIT_THE_DIFFERENCE;

	public ParticleQualityFilter qualityFilter = ParticleQualityFilter.Default;

	[NonSerialized]
	public bool editorToggle;

	public void RegisterPreload(PreloadContext context)
	{
		context.Register(new PreloadDef(this.hitParticle, PreloadType.EFFECT), this.preloadCount);
	}

	public bool IsAppropriateQualityLevel(ThreeTierQualityLevel qualityLevel)
	{
		int num = (int)this.qualityFilter;
		int num2 = 1 << (int)qualityLevel;
		return (num & num2) != 0;
	}
}
