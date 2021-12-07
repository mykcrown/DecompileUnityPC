// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class CharacterEffectConfig : IPreloadedGameAsset
{
	public ParticleData dash = new ParticleData();

	public ParticleData dashPivot = new ParticleData();

	public ParticleData brake = new ParticleData();

	public ParticleData airJump = new ParticleData();

	public ParticleData heavyLand = new ParticleData();

	public ParticleData knockDown = new ParticleData();

	public ParticleData untechableKnockdown = new ParticleData();

	public ParticleData despawn = new ParticleData();

	public ParticleData totemDespawn = new ParticleData();

	public ParticleData tech = new ParticleData();

	public ParticleData wavedashMax = new ParticleData();

	public ParticleData wavedashMedium = new ParticleData();

	public ParticleData wavedashShort = new ParticleData();

	public ParticleData wavedashInPlace = new ParticleData();

	public ParticleData ledgeGrab = new ParticleData();

	public ParticleData fastfall = new ParticleData();

	public ParticleData lethalHit = new ParticleData();

	public ParticleData hologram = new ParticleData();

	public ParticleData hologramBeam = new ParticleData();

	public TrailEmitterData trailData;

	public bool enableWeaponTrails = true;

	public Material weaponTrailMaterial;

	public AudioData fastFallSound;

	public AudioData shieldBreakSound;

	public AudioData lethalHitSound;

	public float teamOutlineWidth = 0.1f;

	public bool enableTeamOutlines = true;

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

	public void RegisterPreload(PreloadContext context)
	{
		ParticleData[] array = this.ParticleDataArray();
		for (int i = 0; i < array.Length; i++)
		{
			ParticleData particleData = array[i];
			particleData.RegisterPreload(context);
		}
		this.trailData.RegisterPreload(context);
	}
}
