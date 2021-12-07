// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.Serialization;

[Serializable]
public class MoveParticleEffect : ICloneable, IPreloadedGameAsset
{
	[FormerlySerializedAs("castingFrame")]
	public int frame;

	public MoveEffectCancelCondition cancelCondition;

	public ParticleData particleEffect;

	public object Clone()
	{
		return CloneUtil.SlowDeepClone<MoveParticleEffect>(this);
	}

	public void RegisterPreload(PreloadContext context)
	{
		this.particleEffect.RegisterPreload(context);
	}
}
