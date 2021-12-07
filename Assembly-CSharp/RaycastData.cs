using System;
using FixedPoint;

// Token: 0x02000561 RID: 1377
public struct RaycastData
{
	// Token: 0x06001E34 RID: 7732 RVA: 0x000991DF File Offset: 0x000975DF
	public RaycastData(Vector3F cast, RaycastHitData hit, bool clockwise, int castIndex)
	{
		this.cast = cast;
		this.hit = hit;
		this.clockwise = clockwise;
		this.castIndex = castIndex;
	}

	// Token: 0x04001865 RID: 6245
	public Vector3F cast;

	// Token: 0x04001866 RID: 6246
	public RaycastHitData hit;

	// Token: 0x04001867 RID: 6247
	public bool clockwise;

	// Token: 0x04001868 RID: 6248
	public int castIndex;
}
