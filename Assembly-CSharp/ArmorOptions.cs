using System;
using FixedPoint;
using UnityEngine.Serialization;

// Token: 0x02000504 RID: 1284
[Serializable]
public class ArmorOptions : IPreloadedGameAsset
{
	// Token: 0x06001BE1 RID: 7137 RVA: 0x0008D1F2 File Offset: 0x0008B5F2
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

	// Token: 0x04001641 RID: 5697
	public bool hasArmor;

	// Token: 0x04001642 RID: 5698
	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	// Token: 0x04001643 RID: 5699
	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	// Token: 0x04001644 RID: 5700
	public Fixed knockbackThreshold = 0;

	// Token: 0x04001645 RID: 5701
	public HitEffectsData LightHitEffects = new HitEffectsData();

	// Token: 0x04001646 RID: 5702
	public HitEffectsData MediumHitEffects = new HitEffectsData();

	// Token: 0x04001647 RID: 5703
	public HitEffectsData MediumHeavyHitEffects = new HitEffectsData();

	// Token: 0x04001648 RID: 5704
	public HitEffectsData HeavyHitEffects = new HitEffectsData();
}
