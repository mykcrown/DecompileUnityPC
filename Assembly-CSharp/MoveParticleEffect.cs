using System;
using UnityEngine.Serialization;

// Token: 0x02000501 RID: 1281
[Serializable]
public class MoveParticleEffect : ICloneable, IPreloadedGameAsset
{
	// Token: 0x06001BDA RID: 7130 RVA: 0x0008D15E File Offset: 0x0008B55E
	public object Clone()
	{
		return CloneUtil.SlowDeepClone<MoveParticleEffect>(this);
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x0008D166 File Offset: 0x0008B566
	public void RegisterPreload(PreloadContext context)
	{
		this.particleEffect.RegisterPreload(context);
	}

	// Token: 0x04001638 RID: 5688
	[FormerlySerializedAs("castingFrame")]
	public int frame;

	// Token: 0x04001639 RID: 5689
	public MoveEffectCancelCondition cancelCondition;

	// Token: 0x0400163A RID: 5690
	public ParticleData particleEffect;
}
