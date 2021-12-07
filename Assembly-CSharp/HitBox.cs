using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020003AC RID: 940
[Serializable]
public class HitBox : ICloneable
{
	// Token: 0x06001437 RID: 5175 RVA: 0x00071F6D File Offset: 0x0007036D
	public void Rescale(Fixed rescale)
	{
		this.offset *= (float)rescale;
		this.radius *= (float)rescale;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x00071F99 File Offset: 0x00070399
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04000D7F RID: 3455
	public BodyPart bodyPart;

	// Token: 0x04000D80 RID: 3456
	public Vector3 offset;

	// Token: 0x04000D81 RID: 3457
	public bool isRelativeOffset;

	// Token: 0x04000D82 RID: 3458
	public float radius = 0.7f;
}
