using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003D8 RID: 984
[Serializable]
public class ShieldConfig
{
	// Token: 0x06001552 RID: 5458 RVA: 0x00075B41 File Offset: 0x00073F41
	public void Rescale(Fixed rescale)
	{
		this.maxShieldRadius *= rescale;
	}

	// Token: 0x04000EA1 RID: 3745
	public Fixed maxShieldRadius = (Fixed)0.824999988079071;

	// Token: 0x04000EA2 RID: 3746
	public Fixed minShieldPercent = (Fixed)0.30000001192092896;

	// Token: 0x04000EA3 RID: 3747
	public Fixed maxShieldHealth = (Fixed)60.0;

	// Token: 0x04000EA4 RID: 3748
	public int shieldExpandFrames = 5;

	// Token: 0x04000EA5 RID: 3749
	public Fixed shieldRestoreHealth = (Fixed)20.0;

	// Token: 0x04000EA6 RID: 3750
	public Fixed holdDepletionPerSecond = (Fixed)10.0;

	// Token: 0x04000EA7 RID: 3751
	public Fixed regenerationPerSecond = (Fixed)8.0;

	// Token: 0x04000EA8 RID: 3752
	public int maxShieldBreakFrames = 300;

	// Token: 0x04000EA9 RID: 3753
	public Fixed shieldBreakFrameDamageMultiplier = -(Fixed)0.5;

	// Token: 0x04000EAA RID: 3754
	public Fixed shieldDamageMultiplier = (Fixed)1.0;

	// Token: 0x04000EAB RID: 3755
	public Fixed shieldTiltMaxDistanceX = (Fixed)0.3499999940395355;

	// Token: 0x04000EAC RID: 3756
	public Fixed shieldTiltMaxDistanceY = (Fixed)0.550000011920929;

	// Token: 0x04000EAD RID: 3757
	public Fixed shieldTiltAmountPerFrame = (Fixed)0.05000000074505806;

	// Token: 0x04000EAE RID: 3758
	public bool allowTiltDuringGust;

	// Token: 0x04000EAF RID: 3759
	public Fixed shieldRotationSpeed = (Fixed)0.0;

	// Token: 0x04000EB0 RID: 3760
	public Color normalShieldColor;

	// Token: 0x04000EB1 RID: 3761
	public Color lowShieldColor;

	// Token: 0x04000EB2 RID: 3762
	public AnimationCurve shieldColorCurve;

	// Token: 0x04000EB3 RID: 3763
	public GameObject shieldPrefab;

	// Token: 0x04000EB4 RID: 3764
	public string shieldMaterialColorName = "_Color";
}
