// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine.Serialization;

[Serializable]
public class ArmorOptions : IPreloadedGameAsset
{
	public bool hasArmor;

	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	public Fixed knockbackThreshold = 0;

	public HitEffectsData LightHitEffects = new HitEffectsData();

	public HitEffectsData MediumHitEffects = new HitEffectsData();

	public HitEffectsData MediumHeavyHitEffects = new HitEffectsData();

	public HitEffectsData HeavyHitEffects = new HitEffectsData();

	public void RegisterPreload(PreloadContext context)
	{
		if (this.hasArmor)
		{
			this.LightHitEffects.RegisterPreload(context);
			this.MediumHitEffects.RegisterPreload(context);
			this.MediumHeavyHitEffects.RegisterPreload(context);
			this.HeavyHitEffects.RegisterPreload(context);
		}
	}
}
