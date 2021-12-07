using System;
using UnityEngine;

// Token: 0x0200040E RID: 1038
[Serializable]
public class CharacterColorConfig : ScriptableObject
{
	// Token: 0x04001097 RID: 4247
	public AnimatingColor chargingEmission = new AnimatingColor();

	// Token: 0x04001098 RID: 4248
	public AnimatingColor dazedEmission = new AnimatingColor();

	// Token: 0x04001099 RID: 4249
	public AnimatingColor invincibleEmission = new AnimatingColor();

	// Token: 0x0400109A RID: 4250
	public AnimatingColor invincibleEmissionMed = new AnimatingColor();

	// Token: 0x0400109B RID: 4251
	public AnimatingColor invincibleEmissionSlow = new AnimatingColor();

	// Token: 0x0400109C RID: 4252
	public AnimatingColor helplessColor = new AnimatingColor();

	// Token: 0x0400109D RID: 4253
	public AnimatingColor regrabPreventionColor = new AnimatingColor();

	// Token: 0x0400109E RID: 4254
	public AnimatingColor ledgegrabVulnerable = new AnimatingColor();

	// Token: 0x0400109F RID: 4255
	public bool useImpactEmission;

	// Token: 0x040010A0 RID: 4256
	public int impactEmissionMaxFrames = 10;

	// Token: 0x040010A1 RID: 4257
	public AnimatingColor impactEmission = new AnimatingColor();

	// Token: 0x040010A2 RID: 4258
	public Color tumblingEmission;

	// Token: 0x040010A3 RID: 4259
	public Color emissionColorOffset;

	// Token: 0x040010A4 RID: 4260
	public Color inactiveEmission;

	// Token: 0x040010A5 RID: 4261
	public Color inactiveColorOffset = Color.white;
}
