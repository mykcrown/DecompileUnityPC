using System;
using FixedPoint;

// Token: 0x020005C0 RID: 1472
[Serializable]
public struct FrameDamage
{
	// Token: 0x060020BE RID: 8382 RVA: 0x000A4918 File Offset: 0x000A2D18
	public FrameDamage(int frame, Fixed damage)
	{
		this.frame = frame;
		this.damage = damage;
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x000A4928 File Offset: 0x000A2D28
	public void Clear()
	{
		this.frame = 0;
		this.damage = 0;
	}

	// Token: 0x040019EE RID: 6638
	public int frame;

	// Token: 0x040019EF RID: 6639
	public Fixed damage;
}
