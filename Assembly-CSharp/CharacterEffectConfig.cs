using System;
using UnityEngine;

// Token: 0x020003E5 RID: 997
[Serializable]
public class CharacterEffectConfig : IPreloadedGameAsset
{
	// Token: 0x0600157F RID: 5503 RVA: 0x00076760 File Offset: 0x00074B60
	public ParticleData[] ParticleDataArray()
	{
		return new ParticleData[]
		{
			this.dash,
			this.dashPivot,
			this.brake,
			this.airJump,
			this.heavyLand,
			this.knockDown,
			this.untechableKnockdown,
			this.despawn,
			this.totemDespawn,
			this.tech,
			this.wavedashMax,
			this.wavedashMedium,
			this.wavedashShort,
			this.wavedashInPlace,
			this.ledgeGrab,
			this.fastfall,
			this.lethalHit,
			this.hologram,
			this.hologramBeam
		};
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x0007682C File Offset: 0x00074C2C
	public void RegisterPreload(PreloadContext context)
	{
		foreach (ParticleData particleData in this.ParticleDataArray())
		{
			particleData.RegisterPreload(context);
		}
		this.trailData.RegisterPreload(context);
	}

	// Token: 0x04000F35 RID: 3893
	public ParticleData dash = new ParticleData();

	// Token: 0x04000F36 RID: 3894
	public ParticleData dashPivot = new ParticleData();

	// Token: 0x04000F37 RID: 3895
	public ParticleData brake = new ParticleData();

	// Token: 0x04000F38 RID: 3896
	public ParticleData airJump = new ParticleData();

	// Token: 0x04000F39 RID: 3897
	public ParticleData heavyLand = new ParticleData();

	// Token: 0x04000F3A RID: 3898
	public ParticleData knockDown = new ParticleData();

	// Token: 0x04000F3B RID: 3899
	public ParticleData untechableKnockdown = new ParticleData();

	// Token: 0x04000F3C RID: 3900
	public ParticleData despawn = new ParticleData();

	// Token: 0x04000F3D RID: 3901
	public ParticleData totemDespawn = new ParticleData();

	// Token: 0x04000F3E RID: 3902
	public ParticleData tech = new ParticleData();

	// Token: 0x04000F3F RID: 3903
	public ParticleData wavedashMax = new ParticleData();

	// Token: 0x04000F40 RID: 3904
	public ParticleData wavedashMedium = new ParticleData();

	// Token: 0x04000F41 RID: 3905
	public ParticleData wavedashShort = new ParticleData();

	// Token: 0x04000F42 RID: 3906
	public ParticleData wavedashInPlace = new ParticleData();

	// Token: 0x04000F43 RID: 3907
	public ParticleData ledgeGrab = new ParticleData();

	// Token: 0x04000F44 RID: 3908
	public ParticleData fastfall = new ParticleData();

	// Token: 0x04000F45 RID: 3909
	public ParticleData lethalHit = new ParticleData();

	// Token: 0x04000F46 RID: 3910
	public ParticleData hologram = new ParticleData();

	// Token: 0x04000F47 RID: 3911
	public ParticleData hologramBeam = new ParticleData();

	// Token: 0x04000F48 RID: 3912
	public TrailEmitterData trailData;

	// Token: 0x04000F49 RID: 3913
	public bool enableWeaponTrails = true;

	// Token: 0x04000F4A RID: 3914
	public Material weaponTrailMaterial;

	// Token: 0x04000F4B RID: 3915
	public AudioData fastFallSound;

	// Token: 0x04000F4C RID: 3916
	public AudioData shieldBreakSound;

	// Token: 0x04000F4D RID: 3917
	public AudioData lethalHitSound;

	// Token: 0x04000F4E RID: 3918
	public float teamOutlineWidth = 0.1f;

	// Token: 0x04000F4F RID: 3919
	public bool enableTeamOutlines = true;

	// Token: 0x04000F50 RID: 3920
	public Color[] teamOutlineColors = new Color[]
	{
		WColor.UIBlue,
		WColor.UIRed,
		WColor.UIYellow,
		WColor.UIGreen,
		WColor.UIBlue,
		WColor.UIRed,
		WColor.UIYellow,
		WColor.UIGreen
	};
}
