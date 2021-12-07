using System;
using FixedPoint;

// Token: 0x0200059C RID: 1436
[Serializable]
public class CharacterShieldData
{
	// Token: 0x040019DE RID: 6622
	public Vector3F shieldOffset = new Vector3F(0, (Fixed)2.299999952316284, 0);

	// Token: 0x040019DF RID: 6623
	public bool useDefaultShield = true;

	// Token: 0x040019E0 RID: 6624
	public float maxShieldRadius;
}
