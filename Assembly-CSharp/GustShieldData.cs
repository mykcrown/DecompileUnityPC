using System;
using FixedPoint;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020004D2 RID: 1234
[Serializable]
public class GustShieldData
{
	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x06001B3C RID: 6972 RVA: 0x0008AF53 File Offset: 0x00089353
	public int TotalActiveFrames
	{
		get
		{
			return this.expandFrames + this.holdFrames + this.shrinkFrames;
		}
	}

	// Token: 0x04001483 RID: 5251
	public bool enableGustShield = true;

	// Token: 0x04001484 RID: 5252
	public bool useFixedGustShieldSize;

	// Token: 0x04001485 RID: 5253
	public Fixed fixedGustShieldRadius = (Fixed)1.2999999523162842;

	// Token: 0x04001486 RID: 5254
	public Fixed maxSizeMultiplier = (Fixed)1.2999999523162842;

	// Token: 0x04001487 RID: 5255
	public bool gustSizeDependentOnHealth;

	// Token: 0x04001488 RID: 5256
	public bool gustPlayerTriggersSuccess = true;

	// Token: 0x04001489 RID: 5257
	public int expandFrames = 30;

	// Token: 0x0400148A RID: 5258
	public int holdFrames = 5;

	// Token: 0x0400148B RID: 5259
	public int shrinkFrames = 5;

	// Token: 0x0400148C RID: 5260
	public int reflectFrameStart;

	// Token: 0x0400148D RID: 5261
	public int reflectFrameEnd;

	// Token: 0x0400148E RID: 5262
	public int knockbackFrameStart;

	// Token: 0x0400148F RID: 5263
	public int knockbackFrameEnd;

	// Token: 0x04001490 RID: 5264
	public GustShieldCostType costType = GustShieldCostType.Mixed;

	// Token: 0x04001491 RID: 5265
	public bool subtractHealthOnUse;

	// Token: 0x04001492 RID: 5266
	public Fixed gustUsableThresholdPercent = (Fixed)0.4000000059604645;

	// Token: 0x04001493 RID: 5267
	public Fixed gustUsableThresholdValue = (Fixed)0.0;

	// Token: 0x04001494 RID: 5268
	public Fixed shieldHealthCostOfMaxPercent = (Fixed)0.0;

	// Token: 0x04001495 RID: 5269
	public Fixed shieldHealthCostOfCurrentPercent = (Fixed)0.5;

	// Token: 0x04001496 RID: 5270
	public Fixed shieldHealthCostFlatValue = (Fixed)0.0;

	// Token: 0x04001497 RID: 5271
	public Fixed shieldHealthAfterUseFlatValue = (Fixed)0.0;

	// Token: 0x04001498 RID: 5272
	public Fixed shieldHealthAfterUsePercentOfMax = (Fixed)0.15000000596046448;

	// Token: 0x04001499 RID: 5273
	public bool resetsXVelocity_ground = true;

	// Token: 0x0400149A RID: 5274
	public bool resetsYVelocity_ground = true;

	// Token: 0x0400149B RID: 5275
	public bool resetsXVelocity_air = true;

	// Token: 0x0400149C RID: 5276
	public bool resetsYVelocity_air = true;

	// Token: 0x0400149D RID: 5277
	public bool causesFlinching_air;

	// Token: 0x0400149E RID: 5278
	public bool causesFlinching_ground;

	// Token: 0x0400149F RID: 5279
	public bool allowGustShieldBreak = true;

	// Token: 0x040014A0 RID: 5280
	public ParticleData gustParticle = new ParticleData();

	// Token: 0x040014A1 RID: 5281
	public Color gustColor;

	// Token: 0x040014A2 RID: 5282
	public AnimationCurve gustColorCurve = new AnimationCurve();

	// Token: 0x040014A3 RID: 5283
	public Fixed rotationSpeed = (Fixed)360.0;

	// Token: 0x040014A4 RID: 5284
	public Fixed gustKnockback_ground = (Fixed)80.0;

	// Token: 0x040014A5 RID: 5285
	[FormerlySerializedAs("gustKnockback")]
	public Fixed gustKnockback_air = (Fixed)80.0;

	// Token: 0x040014A6 RID: 5286
	public int gustKnockbackAngleClamp_ground = 90;

	// Token: 0x040014A7 RID: 5287
	[FormerlySerializedAs("gustKnockbackAngleClamp")]
	public int gustKnockbackAngleClamp_air = 90;

	// Token: 0x040014A8 RID: 5288
	public bool gustShieldIgnoresWeight;

	// Token: 0x040014A9 RID: 5289
	public bool gustShieldIgnoresDamage;
}
