using System;
using FixedPoint;

// Token: 0x020003BB RID: 955
public class HitContext
{
	// Token: 0x0600149E RID: 5278 RVA: 0x0007325C File Offset: 0x0007165C
	public void Clear()
	{
		this.collisionPosition = default(Vector3F);
		this.collisionVelocity = default(Vector3F);
		this.hurtBoxState = null;
		this.useKillFlourish = false;
		this.totalHitSuccess = 0;
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x0600149F RID: 5279 RVA: 0x0007329C File Offset: 0x0007169C
	public static HitContext Null
	{
		get
		{
			return HitContext.emptyObj;
		}
	}

	// Token: 0x04000DBB RID: 3515
	public Vector3F collisionPosition;

	// Token: 0x04000DBC RID: 3516
	public Vector3F collisionVelocity;

	// Token: 0x04000DBD RID: 3517
	public ISegmentCollider hurtBoxState;

	// Token: 0x04000DBE RID: 3518
	public int totalHitSuccess;

	// Token: 0x04000DBF RID: 3519
	public bool useKillFlourish;

	// Token: 0x04000DC0 RID: 3520
	public HitData reflectorHitData;

	// Token: 0x04000DC1 RID: 3521
	private static HitContext emptyObj = new HitContext();
}
